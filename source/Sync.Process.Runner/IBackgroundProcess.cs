using System;
using System.Threading.Tasks;

namespace StoneAge.Synchronous.Process.Runner
{
    public interface IBackgroundProcess : IDisposable
    {
        void Start();
        void Stop();
        Task<string> ReadStdErrToEndAsync();
    }
}