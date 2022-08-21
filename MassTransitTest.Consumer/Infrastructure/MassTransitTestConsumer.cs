using MassTransit;
using MassTransitTest.Consumer.Operations.Interfaces;
using MassTransitTest.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace MassTransitTest.Consumer.Infrastructure
{
    public class MassTransitTestConsumer : IConsumer<MassTransitTestRequest>
    {
        private readonly IBusRegistrationContext busRegistrationContext;

        public MassTransitTestConsumer(IBusRegistrationContext busRegistrationContext)
        {
            this.busRegistrationContext = busRegistrationContext;
        }

        public async Task Consume(ConsumeContext<MassTransitTestRequest> context)
        {
            var response = new MassTransitTestResponse();
            Console.WriteLine("Message receive from client: " + JsonConvert.SerializeObject(context.Message));

            try
            {
                var _scope = busRegistrationContext.CreateScope();
                var type = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(IBaseOperations))
                 && mytype.IsInterface
                 && mytype.Name.ToLower().Contains(context.Message.ModuleName)).FirstOrDefault();

                if (type != null)
                {
                    Type resolvedInterface = Type.GetType(type.AssemblyQualifiedName);
                    if (resolvedInterface != null)
                    {
                        var dependencyInjection = _scope.ServiceProvider.GetService(resolvedInterface);
                        if (dependencyInjection != null)
                        {
                            var method = dependencyInjection.GetType().GetMethods().Where(x => x.Name.ToLower().Equals(context.Message.OperationName)).FirstOrDefault();
                            if (method != null)
                            {
                                response = method.Invoke(dependencyInjection, new object[] { context.Message }) as MassTransitTestResponse;
                            }
                            else
                            {
                                response.Code = 404;
                                response.Message = "Method not found";
                            }
                        }
                        else
                        {
                            response.Code = 404;
                            response.Message = "Dependency not found";
                        }
                    }
                    else
                    {
                        response.Code = 404;
                        response.Message = "Operation not found";
                    }
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Module not found";
                }
            }
            catch (Exception exp)
            {
                response.Code = 500;
                response.Message = "Exception: " + exp.Message;
            }

            Console.WriteLine("Message sent from server: " + JsonConvert.SerializeObject(response));

            await context.RespondAsync<MassTransitTestResponse>(response);
        }
    }
}
