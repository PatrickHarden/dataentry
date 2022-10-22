using dataentry.Publishing.Models;
using Microsoft.Extensions.Logging;

namespace dataentry.Publishing.Commands
{
    public interface ICommand
    {
        void Execute(PublishState initialState, PublishState desiredState, ILogger log);
    }
}
