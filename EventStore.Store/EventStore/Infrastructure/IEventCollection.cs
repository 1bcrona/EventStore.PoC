using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.Event.Infrastructure;
using System;
using System.Collections.Generic;
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

        Task<T> AggregateStream<T>(Guid streamId) where T : class;

        Task<IEnumerable<T>> Query<T>() where T : IEntity;

        Task<T> Query<T>(object id) where T : IEntity;

        Task<IEvent> ReadStream(string streamId);

        Task<IEvent> ReadStream(Guid streamId);

        #endregion Public Methods
    }
}