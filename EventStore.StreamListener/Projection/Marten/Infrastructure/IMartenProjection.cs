using EventStore.Store.EventStore.Infrastructure;

namespace EventStore.StreamListener.Projection.Marten.Infrastructure
{
    public interface IMartenProjection<out T, TKey> : IEventProjection<T, TKey>
    {
    }
}
