using System;

namespace NextBus.Logging
{
    public class LogEntry
    {
        /// <summary>
        /// UTC Time the log entry was raised
        /// </summary>
        public DateTime DateUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// [Nullable] Source of the log entry e.g. the fullname of the call raising the log
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Log title summarising the 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Main body of the log entry. Exceptions will be stored here...
        /// </summary>
        public string Message { get; set; }
        
        public LogType Type { get; set; }
    }

    public enum LogType
    {
        Info,
        Warn,
        Error
    }
}
