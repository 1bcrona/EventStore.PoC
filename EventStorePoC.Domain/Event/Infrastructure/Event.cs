namespace EventStore.PoC.Domain.Event.Infrastructure
{
    public abstract class Event<T> : IEvent where T : class
    {
        public T Data { get; set; }
        public object AggregateId { get; set; }
        public object EntityId { get; set; }
        object IEvent.Data => Data;
    }
}