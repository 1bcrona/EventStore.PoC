using System;
using System.Collections.Generic;
using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Store.EventStore.Infrastructure;
using Marten.Events.Projections;
using Marten.Schema;

namespace EventStore.StreamListener.Projection.Marten.Infrastructure
{
    public class BaseMartenProjection<T, TKey> : ViewProjection<T, TKey>, IEventProjection<T, TKey> where T : BaseEntity<TKey>
    {
        #region Public Events


        #endregion Public Events

        #region Public Properties

        [Identity]
        public Guid Id { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void AddEvent<TEvent>(Func<TEvent, TKey> keySelector, Action<T, TEvent> action) where TEvent : class, IEvent
        {
            ProjectEvent(keySelector, action);
        }

        public void AddEvent<TEvent>(Func<TEvent, List<TKey>> keySelector, Action<T, TEvent> action) where TEvent : class, IEvent
        {
            ProjectEvent(keySelector, action);
        }

        #endregion Public Methods
    }
}