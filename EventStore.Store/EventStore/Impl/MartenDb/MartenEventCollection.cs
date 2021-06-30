using EventStore.Store.EventStore.Infrastructure;
using Marten;
using Marten.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Domain.Entity.Infrastructure;
using IEvent = EventStore.Domain.Event.Infrastructure.IEvent;

namespace EventStore.Store.EventStore.Impl.MartenDb
{
    public class MartenEventCollection : IEventCollection
    {
        #region Private Fields

        private readonly Marten.DocumentStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public MartenEventCollection(Marten.DocumentStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<object> AddEvent(IEvent @event)
        {
            var aggregateId = Guid.NewGuid();
            var result = await AddEventInternal(aggregateId, @event);

            if (result) return aggregateId;

            return null;
        }

        async Task<bool> IEventCollection.AddEvent(object streamId, IEvent @event)
        {
            var guidOrStringType = streamId.ToString();

            return await AddEventInternal(guidOrStringType, @event);
        }

        public async Task<object> AddEvents(IEvent[] events)
        {
            var aggregateId = Guid.NewGuid();
            var result = await AddEventsInternal(aggregateId, events);

            if (result) return aggregateId;

            return null;
        }

        public async Task<bool> AddEvents(object streamId, IEvent[] @event)
        {
            var guidOrStringType = streamId.ToString();
            return await AddEventsInternal(guidOrStringType, @event);
        }

        public async Task<IEnumerable<T>> Query<T>() where T : IEntity
        {
            return await QueryInternal<T>();
        }

        public async Task<T> Query<T>(object id) where T : IEntity
        {
            return id switch
            {
                int => await QueryInternal<T>(Convert.ToInt32(id)),
                long => await QueryInternal<T>(Convert.ToInt64(id)),
                string => await QueryInternal<T>(id.ToString()),
                Guid => await QueryInternal<T>(Guid.Parse(id.ToString() ?? string.Empty)),
                _ => throw new Exception("NOT_A_VALID_ID")
            };
        }

        public async Task<IEvent> ReadStream(Guid streamId)
        {
            return await ReadStreamInternal(streamId);
        }

        public async Task<T> AggregateStream<T>(Guid streamId) where T : class
        {
            using var session = this._DocumentStore.DirtyTrackedSession(IsolationLevel.Serializable);

            var item =await session.Events.AggregateStreamAsync<T>(streamId);

            return item;

        }

        public async Task<IEvent> ReadStream(string streamId)
        {
            return await ReadStreamInternal(streamId);
        }

        private async Task<T> QueryInternal<T>(Guid id) where T : IEntity
        {
            using var session = _DocumentStore.DirtyTrackedSession(IsolationLevel.Serializable);
            var item = await session.LoadAsync<T>(id);
            if (item == null) return default;
            return !item.Active ? default : item;
        }

        private async Task<T> QueryInternal<T>(string id) where T : IEntity
        {
            using var session = _DocumentStore.DirtyTrackedSession(IsolationLevel.Serializable);
            var item = await session.LoadAsync<T>(id);
            if (item == null) return default;
            return !item.Active ? default : item;
        }

        private async Task<T> QueryInternal<T>(int id) where T : IEntity
        {
            using var session = _DocumentStore.DirtyTrackedSession(IsolationLevel.Serializable);
            var item = await session.LoadAsync<T>(id);
            if (item == null) return default;
            return !item.Active ? default : item;
        }

        private async Task<T> QueryInternal<T>(long id) where T : IEntity
        {
            using var session = _DocumentStore.LightweightSession();
            var item = await session.LoadAsync<T>(id);
            if (item == null) return default;
            return !item.Active ? default : item; ;
        }

        private async Task<IEnumerable<T>> QueryInternal<T>() where T : IEntity
        {
            using var session = _DocumentStore.DirtyTrackedSession(IsolationLevel.Serializable);
            return await session.Query<T>().Where(w => w.Active).ToListAsync();
        }

        private async Task<IEvent> ReadStreamInternal(GuidOrStringType streamId)
        {
            using var session = _DocumentStore.DirtyTrackedSession(IsolationLevel.Serializable);
            streamId ??= Guid.NewGuid();

            if (_DocumentStore.Events.StreamIdentity == StreamIdentity.AsGuid)
                return await session.Events.AggregateStreamAsync<IEvent>((Guid)streamId);
            return await session.Events.AggregateStreamAsync<IEvent>((string)streamId);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<bool> AddEventInternal(GuidOrStringType streamId, IEvent @event)
        {
            using var session = _DocumentStore.LightweightSession(IsolationLevel.Serializable);
            streamId ??= Guid.NewGuid();

            if (_DocumentStore.Events.StreamIdentity == StreamIdentity.AsGuid)
                session.Events.Append((Guid)streamId, @event);
            else
                session.Events.Append((string)streamId, @event);
            await session.SaveChangesAsync();

            return true;
        }

        private async Task<bool> AddEventsInternal(GuidOrStringType streamId, IEvent[] events)
        {
            using var session = _DocumentStore.LightweightSession();
            streamId ??= Guid.NewGuid();

            if (_DocumentStore.Events.StreamIdentity == StreamIdentity.AsGuid)
                session.Events.Append((Guid)streamId, events?.Cast<object>());
            else
                session.Events.Append((string)streamId, events?.Cast<object>());

            await session.SaveChangesAsync();

            return true;
        }

        #endregion Private Methods
    }
}