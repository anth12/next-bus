using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextBus.Logging.Appenders
{
    public class InMemoryAppender : ILogAppender
    {
        private List<LogEntry> Items { get; set; } = new List<LogEntry>();
         
        public Task Write(LogEntry log)
        {
            Items.Add(log);
            return Task.FromResult(true);
        }

        public Task ClearAll()
        {
            Items.Clear();
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<LogEntry>> ReadAllAsync()
        {
            return await Task.FromResult(Items);
        }
    }
}
