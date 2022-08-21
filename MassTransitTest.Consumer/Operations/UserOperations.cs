using MassTransitTest.Consumer.Operations.Interfaces;
using MassTransitTest.Core.Models;
using Newtonsoft.Json;

namespace MassTransitTest.Consumer.Operations
{
    public class UserOperations : IUserOperations
    {

        public MassTransitTestResponse Login(MassTransitTestRequest request)
        {
            var response = new MassTransitTestResponse();
            var loginRequest = JsonConvert.DeserializeObject<LoginRequest>(request.RequestBody);
            if (loginRequest != null)
            {
                if (loginRequest.Email.Equals("demo@demo.com"))
                {
                    response.Code = 200;
                    response.Message = "Login Successfuly";
                    response.Result = new LoginResponse
                    {
                        Token = Guid.NewGuid().ToString(),
                    };
                }
                else
                {
                    response.Code = 404;
                    response.Message = "User Not Found";
                }
            }
            else
            {
                response.Code = 400;
                response.Message = "Invalid Login Request";
            }
            return response;
        }
    }
}
