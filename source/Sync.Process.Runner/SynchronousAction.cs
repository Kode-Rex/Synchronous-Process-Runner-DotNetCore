using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace TddBuddy.Synchronous.Process.Runner
{
    public class SynchronousAction : ISynchronousAction
    {
        private readonly IProcessFactory _processFactory;
        private readonly ProcessPipeLineTask _processPipeLineTask;

        public SynchronousAction(ProcessPipeLineTask processPipeLineTask, IProcessFactory processFactory)
        {
            _processPipeLineTask = processPipeLineTask;
            _processFactory = processFactory;
        }

        public void Execute(IRespondWithSuccessOrError<string, ErrorOutputMessage> presenter)
        {
            var processStartInfo = _processPipeLineTask.CommandToExecute();

            using (var process = _processFactory.CreateProcess(processStartInfo))
            {
                process.Start();
                var readerTask = process.ReadStdOutToEndAsync();
                var errorTask = process.ReadStdErrToEndAsync();

                process.WaitForExit();

                var error = errorTask.Result;
                if (HasError(error))
                {
                    var errorOutput = new ErrorOutputMessage();
                    errorOutput.AddError(error);
                    presenter.Respond(errorOutput);
                    return;
                }

                presenter.Respond(readerTask.Result);
            }
        }

        private static bool HasError(string error)
        {
            return !string.IsNullOrWhiteSpace(error);
        }
    }
}