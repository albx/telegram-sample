using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json.Serialization;

namespace TelegramSample.WebHook
{
    public class WebHookFunction
    {
        private readonly ILogger _logger;

        public WebHookFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WebHookFunction>();
        }

        [Function("WebHookFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "webhook")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var update = await req.ReadFromJsonAsync<Update>();

            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }

        public record Update
        {
            [JsonPropertyName("update_id")]
            public int UpdateId { get; set; }

            public Message Message { get; set; } = default!;
        }

        public record Message
        {
            [JsonPropertyName("message_id")]
            public int MessageId { get; set; }

            public string Text { get; set; } = string.Empty;
        }
    }
}
