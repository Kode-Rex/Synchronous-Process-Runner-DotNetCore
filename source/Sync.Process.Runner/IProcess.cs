using System;
using System.Threading.Tasks;

namespace StoneAge.Synchronous.Process.Runner
{
    public interface IProcess : IDisposable
    {
        bool TimeoutOccured { get; }

        void Start();
        void WaitForExit(int processTimeoutSeconds);
        Task<string> ReadStdOutToEndAsync();
        Task<string> ReadStdErrToEndAsync();
        void WriteToStdInput(string input);
    }
}