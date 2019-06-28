using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace StoneAge.Synchronous.Process.Runner.PipeLineTask
{
    public abstract class BashPipeLineTask : ProcessPipeLineTask
    {
        private readonly string _arguments;
        private readonly string _application;

        protected BashPipeLineTask(string application, string arguments)
        {
            if (string.IsNullOrWhiteSpace(application)) throw new ArgumentException(nameof(application));
            _application = application;
            _arguments = arguments;
        }

        public override ProcessStartInfo CommandToExecute()
        {
            var fileName = "/bin/bash";
            var arguments = $"{_application} {_arguments}";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new WrongOsException("This task will not run on Windows!");
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8
            };

            return processStartInfo;
        }
    }

    public class WrongOsException : Exception
    {
        public WrongOsException(string message) : base(message)
        {
        }
    }
}
