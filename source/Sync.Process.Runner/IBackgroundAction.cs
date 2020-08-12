using StoneAge.CleanArchitecture.Domain;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace StoneAge.Synchronous.Process.Runner
{
    public interface IBackgroundAction
    {
        void Start(IRespondWithNoResultSuccessOrError<ErrorOutput> presenter);
        void Stop();
    }
}
