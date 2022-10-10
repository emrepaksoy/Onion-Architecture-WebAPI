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

//Cors Politikalarýný ayarlamamýz saðlayan servis. Cors Middleware çaðýrýlmalýdýr.
//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
//    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials())); // her gelen isteðe izin verir. :)


//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("isteklerin nerden geleceði bildirilir.")..AllowAnyHeader().AllowAnyMethod()));


builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
         // yukarýda yapýlan iþlem default olarak gelen filter ý kaldýrýr istenilen þekilde filter oluþturulabilir.

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
    logging.RequestHeaders.Add("sec-ch-ua"); // kullanýcý ya dair bilgileri getirir.
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
             ValidateAudience = true,    //oluþturacaðýmýz token deðerini hangi sitelerin kullanacaðýný belirleyen deðer misal -> www.asd.com
            ValidateIssuer = true,      //oluþturulacak token deðerinin kimin daðýttýðýný ifade eden deðer. misal -> www.myapi.com
            ValidateLifetime = true,    //oluþturulan token deðerinin süresinin kontrol edildiði doðrulama.
            ValidateIssuerSigningKey = true, //Üretilecek token deðerinin uygulamaya ait bir deðer olduðunu ifade eden suciry ker verisinin doðrulanmasýdýr. 

            ValidAudience = builder.Configuration["Token:Audience"],
             ValidIssuer = builder.Configuration["Token:Issuer"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
             LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

             NameClaimType = ClaimTypes.Name // JWT üzerinden Name claimne karþýlýk gelen deðeri USer.Identity.Name propertysinden elde edebilebilir.
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

//yukarýda belirlenen Cors politikasýnýn uygulama middleware olarak iþlemesini saðlar.
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





