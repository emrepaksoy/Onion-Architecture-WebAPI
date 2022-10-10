using E_TicaretAPI.Domain.Entities;
using E_TicaretAPI.Domain.Entities.Common;
using E_TicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.Persistence.Contexts
{
    public class ETicaretAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ETicaretAPIDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<Domain.Entities.File> Files{ get; set; }
        public DbSet<ProductImageFile> ProductImageFiles{ get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //base class dan gelir Entityler ü<erinden yapılan değişikliklerin ya da yeni eklenen verinin yakalanmasını sağlayan propertydir.Gelen girdiler Entries de yakalanır.

           var datas =  ChangeTracker.Entries<BaseEntity>();
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

    }
}
