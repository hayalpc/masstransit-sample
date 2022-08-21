using MassTransitTest.Core.Models;

namespace MassTransitTest.Consumer.Operations.Interfaces
{
    public interface IBookOperations : IBaseOperations
    {
        MassTransitTestResponse Add(MassTransitTestRequest request);
        MassTransitTestResponse Update(MassTransitTestRequest request);
        MassTransitTestResponse Detail(MassTransitTestRequest request);
        MassTransitTestResponse Get(MassTransitTestRequest request);
    }
}
