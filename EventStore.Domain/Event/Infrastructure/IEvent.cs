using System;

namespace EventStore.Domain.Event.Infrastructure
{
    public interface IEvent
    {
        #region Public Properties

        public Guid AggregateId { get; }
        object Data { get; }
        public Guid EntityId { get; }

        #endregion Public Properties
    }
}