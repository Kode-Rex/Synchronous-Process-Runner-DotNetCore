using System.Diagnostics;

namespace StoneAge.Synchronous.Process.Runner.PipeLineTask
{
    public abstract class ProcessPipeLineTask
    {
        public abstract ProcessStartInfo CommandToExecute();
    }
}