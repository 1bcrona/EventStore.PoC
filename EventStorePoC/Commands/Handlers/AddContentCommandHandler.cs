using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.PoC.Domain.Entity;
using EventStore.PoC.Domain.Event.Impl;
using EventStore.PoC.Domain.Event.Infrastructure;
using EventStore.PoC.Store.EventStore.Infrastructure;
using MediatR;

namespace EventStore.PoC.API.Commands.Handlers
{
    public class AddContentCommandHandler : IRequestHandler<AddContentCommand, bool>
    {
        private readonly IEventStore _DocumentStore;

        public AddContentCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        public async Task<bool> Handle(AddContentCommand request, CancellationToken cancellationToken)
        {
            var c = new Content
            {
                ContentCdnLink = new ContentCdnLink(request.Url),
                ContentMetadata = new ContentMetadata(request.Title)
            };

            var eventCollection = _DocumentStore.GetCollection();


            var events = new IEvent[]
            {
                new ContentCreated {AggregateId = c.Id, EntityId = c.Id, Data = c},
            };

            foreach (var @event in events)
            {
                await eventCollection.AddEvent(c.Id, @event);
            }

            return await Task.FromResult(true);
        }
    }
}