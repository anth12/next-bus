using System.Collections.Generic;

namespace NextBus.Tracing
{
    public class InMemoryTraceListener : ITraceListener
    {
        public List<string> Values { get; set; } = new List<string>();
         
        public void WriteLine(string line)
        {
            Values.Add(line);
        }

        public void Write(string value)
        {
            Values.Add(value);
        }
    }
}
