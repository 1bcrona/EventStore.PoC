using EventStore.Domain.Entity;
using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using System;

namespace EventStore.StreamListener.Projection.Marten
{
    public class UserProjection : BaseMartenProjection<User, Guid>
    {
    }
}