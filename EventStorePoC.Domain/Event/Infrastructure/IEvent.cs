namespace EventStore.PoC.Domain.Event.Infrastructure
{
    public interface IEvent
    {
        #region Public Properties

        public object AggregateId { get; }
        object Data { get; }
        public object EntityId { get; }

        #endregion Public Properties
    }
}