
namespace NextBus.Tracing
{
    public interface ITraceListener
    {
        void WriteLine(string line);
        void Write(string value);
    }
}
