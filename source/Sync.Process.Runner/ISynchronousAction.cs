using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace TddBuddy.Synchronous.Process.Runner
{
    public interface ISynchronousAction : IAction<string>
    {
        void Execute(string input, IRespondWithSuccessOrError<string, ErrorOutputMessage> presenter);
    }
}
