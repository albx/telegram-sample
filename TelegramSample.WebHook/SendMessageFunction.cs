using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Telegram.Bot;

namespace TelegramSample.WebHook
{
    public class SendMessageFunction
    {
        private readonly ILogger _logger;

        private readonly string _telegramBotToken;

        public SendMessageFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<SendMessageFunction>();
            _telegramBotToken = configuration["TelegramBotToken"];
        }

        [Function("SendMessageFunction")]
        public async Task<HttpResponseData> SendMessage(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "send")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var model = await req.ReadFromJsonAsync<InputModel>();
            if (model is null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var client = new TelegramBotClient(_telegramBotToken);
            var messageSent = await client.SendTextMessageAsync(
                model.ChatId,
                model.Message);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { messageSent.Date });
            return response;
        }

        public record InputModel(
            [Required] string ChatId,
            [Required] string Message);
    }
}
