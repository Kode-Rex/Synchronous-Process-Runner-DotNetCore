using System;
using StoneAge.Synchronous.Process.Runner.PipeLineTask;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace StoneAge.Synchronous.Process.Runner
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
            Execute(null, presenter);
        }

        public void Execute(string input, IRespondWithSuccessOrError<string, ErrorOutputMessage> presenter)
        {
            var processStartInfo = _processPipeLineTask.CommandToExecute();
            
            try
            {
                using (var process = _processFactory.CreateProcess(processStartInfo))
                {
                    process.Start();

                    var readerTask = process.ReadStdOutToEndAsync();
                    var errorTask = process.ReadStdErrToEndAsync();

                process.WriteToStdInput(input);

                    process.WaitForExit();

                    var error = errorTask.Result;
                    if (HasError(error))
                    {
                        var errorOutput = new ErrorOutputMessage();
                        var trimedArugments = processStartInfo.Arguments.Substring(3);
                        errorOutput.AddError($"Failed to execute {trimedArugments}");
                        errorOutput.AddError(error);
                        presenter.Respond(errorOutput);
                        return;
                    }

                    presenter.Respond(readerTask.Result);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to execute [{processStartInfo.FileName}] with [{processStartInfo.Arguments}]",e);
            }
        }

        private static bool HasError(string error)
        {
            return !string.IsNullOrWhiteSpace(error);
        }
    }
}