using System;
using System.Threading.Tasks;

namespace StoneAge.Synchronous.Process.Runner
{
    public interface IProcess : IDisposable
    {
        void Start();
        void WaitForExit();
        Task<string> ReadStdOutToEndAsync();
        Task<string> ReadStdErrToEndAsync();
        void WriteToStdInput(string input);
    }
}