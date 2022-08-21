namespace MassTransitTest.Core.Models
{
    public class RabbitMQOptions
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoutingKey { get; set; }
    }
}
