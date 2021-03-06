namespace EventStore.Domain.Event.Infrastructure
{
    public abstract class Event<T> : IEvent where T : class
    {
        #region Public Properties

        public object AggregateId { get; set; }
        public T Data { get; set; }
        object IEvent.Data => Data;
        public object EntityId { get; set; }

        #endregion Public Properties
    }
}