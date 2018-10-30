using System.Diagnostics;

namespace StoneAge.Synchronous.Process.Runner
{
    public interface IProcessFactory
    {
        IProcess CreateProcess(ProcessStartInfo startInfo);
    }
}