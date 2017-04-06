using System.Collections.Generic;

namespace NextBus.Tracing
{
    public static class Trace
    {
        public static List<ITraceListener> Listeners { get; set; } = new List<ITraceListener>(); 

        public static void WriteLine(string line)
        {
            foreach (var traceListener in Listeners)
            {
                traceListener.WriteLine(line);
            }
        }

        public static void Write(string line)
        {
            foreach (var traceListener in Listeners)
            {
                traceListener.Write(line);
            }
        }
    }
}
