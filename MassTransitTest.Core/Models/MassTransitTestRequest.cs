namespace MassTransitTest.Core.Models
{
    public class MassTransitTestRequest
    {
        public string RoutingKey { get; set; }
        public string ModuleName { get; set; }
        public string OperationName { get; set; }
        public string Id { get; set; }
        public string RequestBody { get; set; }
    }
}
