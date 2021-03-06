using EventStore.PoC.Domain.Entity;
using EventStore.PoC.Domain.Event.Impl;
using EventStore.PoC.Domain.Event.Infrastructure;
using EventStore.PoC.Domain.ValueObject;
using EventStore.PoC.Store.EventStore.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.PoC.API.Commands.Handlers
{
    public class AddContentCommandHandler : IRequestHandler<AddContentCommand, Content>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public AddContentCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Content> Handle(AddContentCommand request, CancellationToken cancellationToken)
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

            return await Task.FromResult(c);
        }

        #endregion Public Methods
    }
}