using MassTransitTest.Consumer.Operations.Interfaces;
using MassTransitTest.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MassTransitTest.Consumer.Operations
{
    public class BookOperations : IBookOperations
    {

        [HttpPost]
        public MassTransitTestResponse Add(MassTransitTestRequest request)
        {
            var response = new MassTransitTestResponse();
            var book = JsonConvert.DeserializeObject<BookRequest>(request.RequestBody);
            if (book != null)
            {
                response.Code = 200;
                response.Message = "Book Successfully Registered";
                response.Result = new BookResponse
                {
                    Id = DateTime.Now.Millisecond,
                    Author = book.Author,
                    BookName = book.BookName,
                    Price = 10,
                };
            }
            else
            {
                response.Code = 400;
                response.Message = "Invalid Book Request";
            }
            return response;
        }

        [HttpPut]
        public MassTransitTestResponse Update(MassTransitTestRequest request)
        {
            var response = new MassTransitTestResponse();
            if (!String.IsNullOrEmpty(request.Id) && long.TryParse(request.Id, out _))
            {
                var book = JsonConvert.DeserializeObject<BookRequest>(request.RequestBody);
                if (book != null)
                {
                    response.Code = 200;
                    response.Message = "Book Successfully Updated";
                    response.Result = new BookResponse
                    {
                        Id = long.Parse(request.Id),
                        Author = "New " + book.Author,
                        BookName = book.BookName + " Updated",
                        Price = 10,
                    };
                }
                else
                {
                    response.Code = 400;
                    response.Message = "Invalid Book Request";
                }
            }
            else
            {
                response.Code = 404;
                response.Message = "Book Not Found";
            }
            return response;
        }

        [HttpGet]
        public MassTransitTestResponse Detail(MassTransitTestRequest request)
        {
            var response = new MassTransitTestResponse();
            if (!String.IsNullOrEmpty(request.Id) && long.TryParse(request.Id, out _))
            {
                response.Code = 200;
                response.Message = "Success";
                response.Result = new BookResponse
                {
                    Id = long.Parse(request.Id),
                    Author = "Test Author " + request.Id,
                    BookName = "My Book " + request.Id,
                    Price = 10,
                };
            }
            else
            {
                response.Code = 404;
                response.Message = "Book Not Found";
            }
            return response;
        }

        [HttpGet]
        public MassTransitTestResponse Get(MassTransitTestRequest request)
        {
            var id = DateTime.Now.Millisecond;

            var response = new MassTransitTestResponse
            {
                Code = 200,
                Message = "Success",
                Result = new List<BookResponse>{
                    new BookResponse{
                        Id = id,
                        Author = "Test Author " + id,
                        BookName = "My Book " + id,
                        Price = 10,
                    },
                    new BookResponse{
                        Id = (id + 1),
                        Author = "Test Author " + (id + 1),
                        BookName = "My Book " + (id + 1),
                        Price = 12,
                    },
                }
            };

            return response;
        }
    }
}
