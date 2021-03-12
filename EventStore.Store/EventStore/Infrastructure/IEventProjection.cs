using System;

namespace EventStore.Store.EventStore.Infrastructure
{
    public interface IEventProjection
    {
        public event EventHandler<object> ProjectionUpdated;
    }
}
