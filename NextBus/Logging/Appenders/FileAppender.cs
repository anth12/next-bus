using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextBus.Logging.Appenders
{
    public class FileAppender : ILogAppender
    {
        
        public void Write(LogEntry log)
        {
            throw new NotImplementedException();
        }

        public void ClearAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LogEntry>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
