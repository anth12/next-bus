
using System.Collections;
using System.Collections.Generic;

namespace NextBus.Tracing
{
    public interface ITraceListener
    {
        void WriteLine(string line);
        void Write(string value);

        IEnumerable<string> GetValues();

        void Clear();
    }
}
