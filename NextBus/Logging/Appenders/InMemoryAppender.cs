using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextBus.Logging.Appenders
{
    public class InMemoryAppender : ILogAppender
    {
        private List<LogEntry> Items { get; set; } = new List<LogEntry>();
         
        public void Write(LogEntry log)
        {
            Items.Add(log);
        }

        public void ClearAll()
        {
            Items.Clear();
        }

        public async Task<IEnumerable<LogEntry>> ReadAllAsync()
        {
            return await Task.FromResult(Items);
        }
    }
}
