using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.Event.Infrastructure;
using Marten.Events.Projections;

namespace EventStore.StreamListener.Projection.Marten
{
    public class BaseMartenProjection<T, TKey> : ViewProjection<T, TKey> where T : BaseEntity<TKey>
    {
        public void AddEvent<TEvent>(Func<TEvent, TKey> idSelector, Action<T, TEvent> action) where TEvent : class, IEvent
        {
            ProjectEvent(idSelector, action);
        }
    }
}
