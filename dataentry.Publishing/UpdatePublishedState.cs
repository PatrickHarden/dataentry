using dataentry.Publishing.Commands;
using dataentry.Publishing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace dataentry.Publishing
{
    public class UpdatePublishedState
    {
        public ICommand _command;

        public UpdatePublishedState(
            ICommand command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
        }

        [FunctionName("Published")]
        public void RunPublishCommand(
            [TimerTrigger("%TimerInterval%", RunOnStartup = true)]TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"Publish function stated executed at: {DateTime.Now}");

            _command.Execute(PublishState.Publishing, PublishState.Published, log);

            log.LogInformation($"Publish function finished executed at: {DateTime.Now}");
        }

        [FunctionName("UnPublished")]
        public void RunUnPublishCommand(
            [TimerTrigger("%TimerInterval%", RunOnStartup = true)]TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"UnPublish function stated executed at: {DateTime.Now}");

            _command.Execute(PublishState.Unpublishing, PublishState.Unpublished, log);

            log.LogInformation($"UnPublish function stated executed at: {DateTime.Now}");
        }
    }
}