
using BooksBazaarAPI.Domain.Entities;
using BooksBazaarAPI.Domain.Entities.Common;
using BooksBazaarAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace BooksBazaarAPI.Persistance.Contexts
{
    public class BooksBazaarAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public BooksBazaarAPIDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments{ get; set; }
        public DbSet<BookImageFile> BookImageFiles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //base class dan gelir Entityler ü<erinden yapılan değişikliklerin ya da yeni eklenen verinin yakalanmasını sağlayan propertydir.Gelen girdiler Entries de yakalanır.

            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow,

                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<BookImageFile>()
        //        .HasKey(x => x.Id);

        //    builder.Entity<Book>()
        //        .HasOne(x => x.BookImageFile)
        //        .WithOne(x => x.Book)
        //        .HasForeignKey<BookImageFile>(x => x.Id);
        //}

    }
}
