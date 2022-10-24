using BooksBazaarAPI.Application.Repositories;
using BooksBazaarAPI.Domain.Entities;
using BooksBazaarAPI.Persistance.Contexts;

namespace BooksBazaarAPI.Persistance.Repositories
{
    public class BookWriteRepository : WriteRepository<Book>, IBookWriteRepository
    {
        public BookWriteRepository(BooksBazaarAPIDbContext context) : base(context)
        {
             
        }
    }
}
