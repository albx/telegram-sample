using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Telegram.Bot;

namespace TelegramSample.WebHook
{
    public class SendMessageFromQueueFunction
    {
        private readonly ILogger _logger;

        private readonly string _telegramBotToken;

        public SendMessageFromQueueFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<SendMessageFromQueueFunction>();
            _telegramBotToken = configuration["TelegramBotToken"];
        }

        [Function("SendMessageFromQueueFunction")]
        public async Task Run(
            [QueueTrigger("messages")] MessageToSend messageToSend)
        {
            var client = new TelegramBotClient(_telegramBotToken);
            var messageSent = await client.SendTextMessageAsync(
                messageToSend.ChatId,
                messageToSend.Message);

            _logger.LogInformation("Message sent on {MessageDate}: {MessageText}", messageSent.Date, messageSent.Text);
        }

        public record MessageToSend(string ChatId, string Message);
    }
}
