using MassTransit;
using MassTransitTest.Consumer.Infrastructure;
using MassTransitTest.Consumer.Operations;
using MassTransitTest.Consumer.Operations.Interfaces;
using MassTransitTest.Core.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IBookOperations, BookOperations>();
builder.Services.AddScoped<IAuthorOperations, AuthorOperations>();
builder.Services.AddScoped<IUserOperations, UserOperations>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var RabbitMQConf = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>() ?? new RabbitMQOptions();

        cfg.Host(RabbitMQConf.Host, "/", h =>
        {
            h.Username(RabbitMQConf.Username);
            h.Password(RabbitMQConf.Password);
        });

        cfg.ReceiveEndpoint(RabbitMQConf.RoutingKey, ep =>
        {
            ep.ConfigureConsumeTopology = false;
            ep.UseMessageRetry(r => r.Interval(2, 100));

            ep.Consumer(() => new MassTransitTestConsumer(context));

            ep.Bind<MassTransitTestRequest>(x =>
            {

                x.Durable = false;
                x.AutoDelete = true;
                x.ExchangeType = "direct";
                x.RoutingKey = RabbitMQConf.RoutingKey;
            });
        });

        cfg.UseNewtonsoftJsonSerializer();
        cfg.UseNewtonsoftJsonDeserializer();

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    app.Run("http://0.0.0.0:5001");
}

