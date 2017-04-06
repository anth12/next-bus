using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextBus.Logging.Sync
{
    public interface ISyncProvider
    {
        Task<bool> PushAsync(List<LogEntry> logs);
    }
}
