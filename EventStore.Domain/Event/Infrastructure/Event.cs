using System;

namespace EventStore.Domain.Event.Infrastructure
{
    public abstract class Event<T> : IEvent
    {
        #region Public Properties

        public Guid AggregateId { get; set; }
        public T Data { get; set; }
        object IEvent.Data => Data;
        public Guid EntityId { get; set; }

        #endregion Public Properties
    }
}