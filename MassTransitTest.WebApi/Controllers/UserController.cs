using MassTransit;
using MassTransitTest.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MassTransitTest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRequestClient<MassTransitTestRequest> requestClient;
        private readonly RabbitMQOptions rabbitMQOptions;

        public UserController(IRequestClient<MassTransitTestRequest> requestClient, IConfiguration configuration)
        {
            this.requestClient = requestClient;
            this.rabbitMQOptions = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
        }

        [HttpPost]
        public async Task<MassTransitTestResponse> Login(LoginRequest request)
        {
            var controller = HttpContext.GetRouteValue("controller")?.ToString()?.ToLower();
            var method = HttpContext.GetRouteValue("action")?.ToString()?.ToLower();

            var queueRequest = new MassTransitTestRequest
            {
                RoutingKey = rabbitMQOptions.RoutingKey,
                ModuleName = controller,
                OperationName = method,
                RequestBody = JsonConvert.SerializeObject(request)
            };

            Console.WriteLine("Message sent from client: " + JsonConvert.SerializeObject(queueRequest));

            var response = await requestClient.GetResponse<MassTransitTestResponse>(queueRequest);

            Console.WriteLine("Message from backend: " + JsonConvert.SerializeObject(response.Message));

            return response.Message;
        }
    }
}
