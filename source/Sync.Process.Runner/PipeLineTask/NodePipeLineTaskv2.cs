using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace StoneAge.Synchronous.Process.Runner.PipeLineTask
{
    public abstract class NodePipeLineTaskv2 : ProcessPipeLineTask
    {
        private readonly string _arguments;
        private readonly string _applicationPath;

        protected NodePipeLineTaskv2(string applicationPath, object arguments)
        {
            if (string.IsNullOrWhiteSpace(applicationPath)) throw new ArgumentException(nameof(applicationPath));
            _applicationPath = applicationPath;
            _arguments = JsonConvert.SerializeObject(arguments);
        }

        public override ProcessStartInfo CommandToExecute()
        {
            var fileName = "node";
            var arguments = $"\"{_applicationPath}\"";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName = "cmd.exe";
                arguments = $"/C node \"{_applicationPath}\"";
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
}
