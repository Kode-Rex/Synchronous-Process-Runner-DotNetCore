using System.Diagnostics;

namespace StoneAge.Synchronous.Process.Runner
{
    public class ProcessFactory : IProcessFactory
    {
        public IBackgroundProcess CreateBackgroundProcess(ProcessStartInfo startInfo)
        {
            return new SystemBackgroundProcess
            {
                StartInfo = startInfo
            };
        }

        public IProcess CreateProcess(ProcessStartInfo startInfo)
        {
            return new SystemProcess
            {
                StartInfo = startInfo
            };
        }
    }
}