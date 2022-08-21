using MassTransitTest.Consumer.Operations.Interfaces;
using MassTransitTest.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MassTransitTest.Consumer.Operations
{
    public class AuthorOperations : IAuthorOperations
    {
        public MassTransitTestResponse Add(MassTransitTestRequest request)
        {
            var response = new MassTransitTestResponse();
            var author = JsonConvert.DeserializeObject<AuthorRequest>(request.RequestBody);
            if (author != null)
            {
                response.Code = 200;
                response.Message = "Author Successfully Registered";
                response.Result = new AuthorResponse
                {
                    Id = DateTime.Now.Millisecond,
                    Name = author.Name,
                };
            }
            else
            {
                response.Code = 400;
                response.Message = "Invalid Author Request";
            }
            return response;
        }

        [HttpPut]
        public MassTransitTestResponse Update(MassTransitTestRequest request)
        {
            var response = new MassTransitTestResponse();
            if (!String.IsNullOrEmpty(request.Id) && long.TryParse(request.Id, out _))
            {
                var author = JsonConvert.DeserializeObject<AuthorRequest>(request.RequestBody);
                if (author != null)
                {
                    response.Code = 200;
                    response.Message = "Author Successfully Updated";
                    response.Result = new AuthorResponse
                    {
                        Id = long.Parse(request.Id),
                        Name = author.Name + " Updated",
                    };
                }
                else
                {
                    response.Code = 400;
                    response.Message = "Invalid Author Request";
                }
            }
            else
            {
                response.Code = 404;
                response.Message = "Author Not Found";
            }
            return response;
        }

        public MassTransitTestResponse Detail(MassTransitTestRequest request)
        {
            var response = new MassTransitTestResponse();
            if (!String.IsNullOrEmpty(request.Id) && long.TryParse(request.Id, out _))
            {
                response.Code = 200;
                response.Message = "Success";
                response.Result = new AuthorResponse
                {
                    Id = long.Parse(request.Id),
                    Name = "Test Author " + request.Id,
                };
            }
            else
            {
                response.Code = 404;
                response.Message = "Author Not Found";
            }
            return response;
        }

        public MassTransitTestResponse Get(MassTransitTestRequest request)
        {
            var id = DateTime.Now.Millisecond;

            var response = new MassTransitTestResponse
            {
                Code = 200,
                Message = "Success",
                Result = new List<AuthorResponse>{
                    new AuthorResponse{
                        Id = id,
                        Name = "Test Author " + id,
                    },
                    new AuthorResponse{
                        Id = id + 1,
                        Name = "Test Author " + (id + 1),
                    },
                }
            };

            return response;
        }
    }
}
