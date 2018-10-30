using StoneAge.CleanArchitecture.Domain;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace StoneAge.Synchronous.Process.Runner
{
    public interface ISynchronousAction : IAction<string>
    {
        void Execute(string input, IRespondWithSuccessOrError<string, ErrorOutput> presenter);
    }
}
