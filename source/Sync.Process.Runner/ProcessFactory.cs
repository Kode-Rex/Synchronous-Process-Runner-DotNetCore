using System.Diagnostics;

namespace StoneAge.Synchronous.Process.Runner
{
    public class ProcessFactory : IProcessFactory
    {
        public IProcess CreateProcess(ProcessStartInfo startInfo)
        {
            return new SystemProcess
            {
                StartInfo = startInfo
            };
        }
    }
}