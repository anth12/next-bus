using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextBus.Logging.Appenders
{
    public interface ILogAppender
    {
        void Write(LogEntry log);


        void ClearAll();

        Task<IEnumerable<LogEntry>> ReadAllAsync();
    }
}
