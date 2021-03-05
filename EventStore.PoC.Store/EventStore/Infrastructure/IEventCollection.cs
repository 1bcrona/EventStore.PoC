using System;
using System.Threading.Tasks;
using EventStore.PoC.Domain.Event.Infrastructure;
using EventStore.PoC.Store.EventStore.Impl.MartenDb;
using Marten.Linq.Parsing;

namespace EventStore.PoC.Store.EventStore.Infrastructure
{
    public interface IEventCollection
    {
        Task<object> AddEvent(IEvent @event);
        Task<object> AddEvents(IEvent[] events);

        Task<bool> AddEvent(object streamId, IEvent @event);
        Task<bool> AddEvents(object streamId, IEvent[] @event);

        Task<IEvent> ReadStream(string streamId);
        Task<IEvent> ReadStream(Guid streamId);
    }





}