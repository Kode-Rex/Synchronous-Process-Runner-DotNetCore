using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoneAge.Synchronous.Process.Runner
{
    public class SystemProcess : IProcess
    {
        private readonly System.Diagnostics.Process _process;

        public bool TimeoutOccured { get; private set; }

        public SystemProcess()
        {
            _process = new System.Diagnostics.Process {EnableRaisingEvents = true};
        }

        public void Dispose()
        {
            _process.Dispose();
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

        public void WaitForExit(int timeoutSeconds)
        {
            if (Process_Has_Timeout(timeoutSeconds))
            {
                Wait_For_Process_To_Exit_Within_Timeout_Interval(timeoutSeconds);
                return;
            }

            Wait_For_Process_To_Exit_Indefinitely();
        }

        public Task<string> ReadStdOutToEndAsync()
        {
            return _process.StandardOutput.ReadToEndAsync();
        }

        public Task<string> ReadStdErrToEndAsync()
        {
            return _process.StandardError.ReadToEndAsync();
        }

        public void WriteToStdInput(string input)
        {
            if (input == null)
            {
                return;
            }

            using (var inputStreamWriter = new StreamWriter(_process.StandardInput.BaseStream, new UTF8Encoding(false)))
            {
                inputStreamWriter.Write(input);
            }
        }

        private bool Did_Timeout_Happen(bool exitStatus)
        {
            return !exitStatus;
        }

        private void Wait_For_Process_To_Exit_Indefinitely()
        {
            _process.WaitForExit();
        }

        private void Wait_For_Process_To_Exit_Within_Timeout_Interval(int timeoutSeconds)
        {
            var timeout = timeoutSeconds * 1000;
            _process.WaitForExit(timeout);
            TimeoutOccured = !_process.HasExited;
            if (TimeoutOccured)
            {
                _process.Kill();
            }
        }

        private static bool Process_Has_Timeout(int timeoutSeconds)
        {
            return timeoutSeconds > 0;
        }
    }
}