using System;
using System.Collections.Generic;
using NextBus.Logging.Appenders;

namespace NextBus.Logging
{
    /// <summary>
    /// Shared log provider to write info/warning/errors to all listening log appenders
    /// </summary>
    public static class LogHelper
    {
        public static List<ILogAppender> Appenders { get; set; } = new List<ILogAppender>();

        #region Info

        public static void Info(string title)
        {
            Write(new LogEntry
            {
                Type = LogType.Info,
                Title = title
            });
        }

        public static void Info(string title, string message)
        {
            Write(new LogEntry
            {
                Type = LogType.Info,
                Title = title,
                Message = message
            });
        }

        public static void Info<TType>(string title, string message)
        {
            Write(new LogEntry
            {
                Type = LogType.Info,
                Title = title,
                Message = message,
                Source = typeof(TType).FullName
            });
        }


        public static void Info(string title, Type source)
        {
            Write(new LogEntry
            {
                Type = LogType.Info,
                Title = title,
                Source = source.FullName
            });
        }

        public static void Info(string title, string message, Type source)
        {
            Write(new LogEntry
            {
                Type = LogType.Info,
                Title = title,
                Message = message,
                Source = source.FullName
            });
        }

        #endregion

        #region Warn

        public static void Warn(string title)
        {
            Write(new LogEntry
            {
                Type = LogType.Warn,
                Title = title
            });
        }

        public static void Warn(string title, string message)
        {
            Write(new LogEntry
            {
                Type = LogType.Warn,
                Title = title,
                Message = message
            });
        }

        public static void Warn<TType>(string title, string message)
        {
            Write(new LogEntry
            {
                Type = LogType.Warn,
                Title = title,
                Message = message,
                Source = typeof(TType).FullName
            });
        }

        public static void Warn(string title, Type source)
        {
            Write(new LogEntry
            {
                Type = LogType.Warn,
                Title = title,
                Source = source.FullName
            });
        }

        public static void Warn(string title, string message, Type source)
        {
            Write(new LogEntry
            {
                Type = LogType.Warn,
                Title = title,
                Message = message,
                Source = source.FullName
            });
        }

        #endregion

        #region Error

        public static void Error(string title, string message)
        {
            Write(new LogEntry
            {
                Type = LogType.Error,
                Title = title,
                Message = message
            });
        }

        public static void Error<TType>(string title, string message)
        {
            Write(new LogEntry
            {
                Type = LogType.Error,
                Title = title,
                Message = message,
                Source = typeof(TType).FullName
            });
        }

        public static void Error(string title, Exception exception)
        {
            Write(new LogEntry
            {
                Type = LogType.Error,
                Title = title,
                Message = exception.ToString()
            });
        }

        public static void Error<TType>(string title, Exception exception)
        {
            Write(new LogEntry
            {
                Type = LogType.Error,
                Title = title,
                Message = exception.ToString(),
                Source = typeof(TType).FullName
            });
        }
        

        public static void Error<TType>(Exception exception)
        {
            Write(new LogEntry
            {
                Type = LogType.Error,
                Title = exception.Message,
                Message = exception.ToString(),
                Source = typeof(TType).FullName
            });
        }
        
        public static void Error(string title, Exception exception, Type source)
        {
            Write(new LogEntry
            {
                Type = LogType.Error,
                Title = title,
                Message = exception.ToString(),
                Source = source.FullName
            });
        }
        
        #endregion

        /// <summary>
        /// Writes the log entry to all configured appenders
        /// </summary>
        public static void Write(LogEntry log)
        {
            foreach (var logAppender in Appenders)
            {
                logAppender.Write(log);
            }
        }

    }
}
