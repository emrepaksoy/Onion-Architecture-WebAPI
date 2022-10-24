using BooksBazaarAPI.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Persistance
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BooksBazaarAPIDbContext>
    {
        public BooksBazaarAPIDbContext  CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<BooksBazaarAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.GetConnectionString);
            return new(dbContextOptionsBuilder.Options);

        }
    }
}
