using BooksBazaarAPI.Application.Repositories;
using BooksBazaarAPI.Domain.Entities;
using BooksBazaarAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Persistance.Repositories
{
    public class BookImageFileWriteRepository : WriteRepository<BookImageFile>, IBookImageFileWriteRepository
    {
        public BookImageFileWriteRepository(BooksBazaarAPIDbContext context) : base(context)
        {
        }
    }
}
