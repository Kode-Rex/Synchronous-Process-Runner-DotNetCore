using System;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;
using StoneAge.Synchronous.Process.Runner.PipeLineTask;

namespace StoneAge.Synchronous.Process.Runner
{
    public class BackgroundAction : IBackgroundAction
    {
        private readonly IProcessFactory _processFactory;
        private readonly ProcessPipeLineTask _processPipeLineTask;

        private IBackgroundProcess _process;

        public BackgroundAction(ProcessPipeLineTask processPipeLineTask) : this(processPipeLineTask, new ProcessFactory()) { }

        public BackgroundAction(ProcessPipeLineTask processPipeLineTask, IProcessFactory processFactory)
        {
            _processPipeLineTask = processPipeLineTask;
            _processFactory = processFactory;
        }

        public void Start(IRespondWithNoResultSuccessOrError<ErrorOutput> presenter)
        {
            Execute(presenter);
        }

        public void Stop()
        {
            _process?.Stop();
        }

        private void Execute(IRespondWithNoResultSuccessOrError<ErrorOutput> presenter)
        {
            var processStartInfo = _processPipeLineTask.CommandToExecute();
            
            try
            {
                _process = _processFactory.CreateBackgroundProcess(processStartInfo);
                
                _process.Start();

                var errorTask = _process.ReadStdErrToEndAsync();

                if (!errorTask.Wait(500))
                {
                    // was there error data to fetch?
                    presenter.Respond();
                }

                var error = errorTask.Result;
                if (HasError(error))
                {
                    var errorOutput = new ErrorOutput();
                    var trimedArugments = processStartInfo.Arguments.Substring(3);
                    errorOutput.AddError($"Failed to execute {trimedArugments}");
                    errorOutput.AddError(error);
                    presenter.Respond(errorOutput);
                    return;
                }

                presenter.Respond();
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