using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.Application.Exceptions
{
    public class UserCreateFailedExcepiton : Exception
    {
        public UserCreateFailedExcepiton() : base("Kullanıcı oluşturma işlemi başarısız. Beklenmeyen bir hata ile karşılaşıldı !")
        {

        }

        public UserCreateFailedExcepiton(string? message) : base(message)
        {

        }

        public UserCreateFailedExcepiton(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
