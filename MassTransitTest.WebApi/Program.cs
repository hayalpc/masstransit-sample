using MassTransit;
using MassTransitTest.Middleware;
using MassTransitTest.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddSwaggerGen();

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
        cfg.Send<MassTransitTestRequest>(m => m.UseRoutingKeyFormatter(c => c.Message.RoutingKey));
        cfg.Publish<MassTransitTestRequest>(m =>
        {
            m.Durable = false;
            m.AutoDelete = true;
            m.ExchangeType = "direct";
        });

        cfg.UseNewtonsoftJsonSerializer();
        cfg.UseNewtonsoftJsonDeserializer();

        cfg.ConfigureEndpoints(context);
    });

    x.AddRequestClient<BookRequest>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();


app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseMiddleware<RequestMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}/{id?}");
    
});



/*
app.MapControllers();

app.UseMiddleware<RequestMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}/{id?}");
});

app.UseMvc();
*/
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();



//    endpoints.Map("api/{controller}/{action}/{id?}", endpoints.CreateApplicationBuilder()
//        .UseMiddleware<RequestMiddleware>()
//        .Build());


//});
if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    app.Run("http://0.0.0.0:5000");
}
