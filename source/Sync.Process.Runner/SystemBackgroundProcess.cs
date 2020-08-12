using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoneAge.Synchronous.Process.Runner
{
    public class SystemBackgroundProcess : IBackgroundProcess
    {
        private readonly System.Diagnostics.Process _process;

        public SystemBackgroundProcess()
        {
            _process = new System.Diagnostics.Process {EnableRaisingEvents = true};
        }

        public void Dispose()
        {
            _process?.Kill();
            _process?.Dispose();
        }

        public ProcessStartInfo StartInfo
        {
            get => _process.StartInfo;
            set => _process.StartInfo = value;
        }

        public void Start()
        {
            _process.Start();
        }

        public void Stop()
        {
            _process.Kill();
        }

        public Task<string> ReadStdErrToEndAsync()
        {
            return _process.StandardError.ReadToEndAsync();
        }
    }
}