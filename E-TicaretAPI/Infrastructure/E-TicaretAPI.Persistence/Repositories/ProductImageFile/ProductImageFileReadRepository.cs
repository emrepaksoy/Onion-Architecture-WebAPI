
using E_TicaretAPI.Application.Repositories;
using E_TicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E_TicaretAPI.Persistence.Repositories
{
    public class ProductImageFileReadRepository : ReadRepository<E_TicaretAPI.Domain.Entities.ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
