namespace EventStore.PoC.Domain.Event.Infrastructure
{
    public interface IEvent
    {
        public object AggregateId { get; }
        public object EntityId { get; }
        object Data { get; }
    }
}