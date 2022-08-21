using MassTransitTest.Core.Models;

namespace MassTransitTest.Consumer.Operations.Interfaces
{
    public interface IUserOperations : IBaseOperations
    {
        MassTransitTestResponse Login(MassTransitTestRequest request);
    }
}
