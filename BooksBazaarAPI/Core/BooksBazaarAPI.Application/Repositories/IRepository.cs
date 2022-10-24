
using BooksBazaarAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace BooksBazaarAPI.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }

    }
}
