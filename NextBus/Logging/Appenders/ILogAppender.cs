using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextBus.Logging.Appenders
{
    public interface ILogAppender
    {
        Task Write(LogEntry log);


        Task ClearAll();

        Task<IEnumerable<LogEntry>> ReadAllAsync();
    }
}
