using EventStore.Domain.Entity;
using EventStore.Domain.Event.Infrastructure;

namespace EventStore.Domain.Event.Impl
{
    public class ProductCreated : Event<Product>
    {
    }
}