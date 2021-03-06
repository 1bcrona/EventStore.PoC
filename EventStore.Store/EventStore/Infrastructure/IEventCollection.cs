using EventStore.Domain.Event.Infrastructure;
using System;
using System.Threading.Tasks;

namespace EventStore.Store.EventStore.Infrastructure
{
    public interface IEventCollection
    {
        #region Public Methods

        Task<object> AddEvent(IEvent @event);

        Task<bool> AddEvent(object streamId, IEvent @event);

        Task<object> AddEvents(IEvent[] events);

        Task<bool> AddEvents(object streamId, IEvent[] @event);

        Task<IEvent> ReadStream(string streamId);

        Task<IEvent> ReadStream(Guid streamId);

        #endregion Public Methods
    }
}