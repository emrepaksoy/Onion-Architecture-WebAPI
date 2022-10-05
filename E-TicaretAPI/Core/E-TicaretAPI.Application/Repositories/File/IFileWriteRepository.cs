using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F = E_TicaretAPI.Domain.Entities;
namespace E_TicaretAPI.Application.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<F::File>
    {

    }
}
