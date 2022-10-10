using E_TicaretAPI.API.Configurations.ColumnWriters;
using E_TicaretAPI.API.Extensions;
using E_TicaretAPI.Application;
using E_TicaretAPI.Application.Validators.Products;
using E_TicaretAPI.Infrastructure;
using E_TicaretAPI.Infrastructure.Filters;
using E_TicaretAPI.Infrastructure.Services.Storage.Azure;
using E_TicaretAPI.Infrastructure.Services.Storage.Local;
using E_TicaretAPI.Persistence;
using E_TicaretAPI.SignalR;
using E_TicaretAPI.SignalR.Hubs;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastuctureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();

//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();

//builder.Services.AddStorage(E_TicaretAPI.Infrastructure.Enums.StorageType.Azure);

//Cors Politikalar�n� ayarlamam�z sa�layan servis. Cors Middleware �a��r�lmal�d�r.
//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
//    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials())); // her gelen iste�e izin verir. :)


//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("isteklerin nerden gelece�i bildirilir.")..AllowAnyHeader().AllowAnyMethod()));


builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
         // yukar�da yap�lan i�lem default olarak gelen filter � kald�r�r istenilen �ekilde filter olu�turulabilir.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    //.WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
        needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text)},
            {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text)},
            {"level", new LevelColumnWriter(true , NpgsqlDbType.Varchar)},
            {"time_stamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp)},
            {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text)},
            {"log_event", new LogEventSerializedColumnWriter(NpgsqlDbType.Json)},
            {"user_name", new UsernameColumnWriter()}
        })
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua"); // kullan�c� ya dair bilgileri getirir.
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
     {
         options.TokenValidationParameters = new()
         {
             ValidateAudience = true,    //olu�turaca��m�z token de�erini hangi sitelerin kullanaca��n� belirleyen de�er misal -> www.asd.com
            ValidateIssuer = true,      //olu�turulacak token de�erinin kimin da��tt���n� ifade eden de�er. misal -> www.myapi.com
            ValidateLifetime = true,    //olu�turulan token de�erinin s�resinin kontrol edildi�i do�rulama.
            ValidateIssuerSigningKey = true, //�retilecek token de�erinin uygulamaya ait bir de�er oldu�unu ifade eden suciry ker verisinin do�rulanmas�d�r. 

            ValidAudience = builder.Configuration["Token:Audience"],
             ValidIssuer = builder.Configuration["Token:Issuer"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
             LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

             NameClaimType = ClaimTypes.Name // JWT �zerinden Name claimne kar��l�k gelen de�eri USer.Identity.Name propertysinden elde edebilebilir.
        };
     });

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();
app.UseSerilogRequestLogging();

app.UseHttpLogging();

//yukar�da belirlenen Cors politikas�n�n uygulama middleware olarak i�lemesini sa�lar.
//app.UseCors();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async(context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.MapControllers();

app.MapHubs();

app.Run();





