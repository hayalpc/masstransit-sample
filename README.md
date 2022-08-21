# MassTransit-Samples
These examples provide unbound requests with MassTransit. 
Web Api doesn't know backend address. 

Communications provide via Masstransit RabbitMQ.
Web Api and backend application counts doesn't important.

- Update RabbitMQ options in appsettings.json
```
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest",
    "RoutingKey": "masstransittest"
  }
```
- If you want to run another IP or Port, you have to change Run option in Program.cs
```
if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    app.Run("http://0.0.0.0:5000");
}
```

# Build And Run
```
cd MassTransitTest.WebApi
dotnet build
dotnet run

cd MassTransitTest.Consumer
dotnet build
dotnet run
```
