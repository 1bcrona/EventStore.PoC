using EventStore.Domain.Event.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.Domain.Entity.Infrastructure;
using Remotion.Linq.Clauses;

namespace EventStore.Store.EventStore.Infrastructure
{
    public interface IEventCollection
    {
        #region Public Methods

        Task<object> AddEvent(IEvent @event);

        Task<bool> AddEvent(object streamId, IEvent @event);

        Task<object> AddEvents(IEvent[] events);

        Task<bool> AddEvents(object streamId, IEvent[] @event);

        Task<IEnumerable<T>> Query<T>() where T : IEntity;

        Task<T> Query<T>(object id) where T : IEntity;

        Task<IEvent> ReadStream(string streamId);

        Task<IEvent> ReadStream(Guid streamId);

        Task<T> AggregateStream<T>(Guid streamId) where T : class;

        #endregion Public Methods
    }
}