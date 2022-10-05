using E_TicaretAPI.Application.Repositories;
using E_TicaretAPI.Domain.Entities.Common;
using E_TicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly ETicaretAPIDbContext _context;

        public WriteRepository(ETicaretAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
                
        }
            
        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }
        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            T model = await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            return Remove(model);
        }

        public bool Update(T model)
        {
            //Ef de bir veritabanın elde edilen veriyi güncellemek için ilgili verinin context sınıfından  gelmesi yeterlidir.
            //Güncelleme işlemi yapmak için ilgili veri üzerinde propertylerde değişiklik yapmamız tracking mekanızmasi tarafından bunun update sorgusu olduğu anlaşılır ve savachanges yapıldığında update sorgusu execute edilir.

            //eğer ki ilgili veri context üzerinden gelmiyorsa yani tracking edilmiyorsa ama ellimiz de bir ıd varsa ve tracking edilmese bile bu Id ye karşılık olan veri istenilen değerlerle güncellenmek isteniyorsa ef deki Update() fonksiyonu kullanılabilir.
            EntityEntry entityEntry =  Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        
    }
}
