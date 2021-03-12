using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Store.EventStore.Infrastructure;

namespace EventStore.StreamListener.Projection.Marten
{
    public interface IMartenProjection<T, TKey> : IEventProjection
    {
    }
}
