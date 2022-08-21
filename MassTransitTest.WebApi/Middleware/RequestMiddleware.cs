using MassTransit;
using MassTransitTest.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MassTransitTest.Middleware
{
    public class RequestMiddleware
    {

        private readonly RequestDelegate next;
        private readonly RabbitMQOptions rabbitMQOptions;

        public RequestMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.rabbitMQOptions = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
        }

        public async Task Invoke(HttpContext context, IRequestClient<MassTransitTestRequest> requestClient)
        {
            await this.next.Invoke(context);

            if (context.Response.StatusCode == 404)
            {
                var path = context.Request.Path;
                if (path.HasValue)
                {
                    var request = context.Request;
                    var split = path.Value.Trim('/').Split("/");
                    var controller = string.Empty;
                    var method = string.Empty;
                    var id = string.Empty;
                    if (split.Length > 1)
                    {
                        controller = split.GetValue(1)?.ToString()?.ToLower();
                        if (split.Length > 2)
                        {
                            method = split.GetValue(2)?.ToString()?.ToLower();
                            if (split.Length > 3)
                            {
                                id = split.GetValue(3)?.ToString()?.ToLower();
                            }
                            var body = await new StreamReader(request.Body).ReadToEndAsync();

                            var queueRequest = new MassTransitTestRequest
                            {
                                RoutingKey = rabbitMQOptions.RoutingKey,
                                ModuleName = controller,
                                OperationName = method,
                                Id = id,
                                RequestBody = body
                            };
                            Console.WriteLine("Message sent from client: " + JsonConvert.SerializeObject(queueRequest));

                            var response = await requestClient.GetResponse<MassTransitTestResponse>(queueRequest);
                            
                            Console.WriteLine("Message received from server: " + JsonConvert.SerializeObject(response.Message));

                            context.Response.StatusCode = response.Message.Code;
                            context.Response.ContentType = "application/json; charset=utf-8";

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(response.Message));

                            //await context.Response.WriteAsJsonAsync(response.Message);
                        }
                    }
                }
            }
        }
    }
}
