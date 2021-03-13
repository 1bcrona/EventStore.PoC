using EventStore.Domain.Event.Infrastructure;
using System;
using System.Collections.Generic;

namespace EventStore.Store.EventStore.Infrastructure
{
    public interface IEventProjection
    {
        #region Public Events

        public event EventHandler<object> ProjectionUpdated;

        #endregion Public Events
    }

    public interface IEventProjection<out T, TKey> : IEventProjection
    {
        #region Public Methods

        void AddEvent<TEvent>(Func<TEvent, TKey> keySelector, Action<T, TEvent> action) where TEvent : class, IEvent;

        void AddEvent<TEvent>(Func<TEvent, List<TKey>> keySelector, Action<T, TEvent> action) where TEvent : class, IEvent;

        #endregion Public Methods
    }
}