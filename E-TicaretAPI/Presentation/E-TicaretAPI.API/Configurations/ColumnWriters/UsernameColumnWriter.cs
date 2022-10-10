using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace E_TicaretAPI.API.Configurations.ColumnWriters
{
    public class UsernameColumnWriter : ColumnWriterBase
    {
        public UsernameColumnWriter() : base(NpgsqlDbType.Varchar)
        {

        }

        public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
          var (username, value) =   logEvent.Properties.FirstOrDefault(p => p.Key == "user_name");

            return value?.ToString() ?? null;
        }
    }
}
