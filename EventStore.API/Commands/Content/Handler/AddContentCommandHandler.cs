using EventStore.Domain.Event.Impl;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Domain.ValueObject;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Commands.Content.Handler
{
    public class AddContentCommandHandler : IRequestHandler<AddContentCommand, Domain.Entity.Content>
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

        public async Task<Domain.Entity.Content> Handle(AddContentCommand request, CancellationToken cancellationToken)
        {
            var c = new Domain.Entity.Content
            {
                ContentCdnLink = new ContentCdnLink(request.Url),
                ContentMetadata = new ContentMetadata(request.Title)
            };

            var eventCollection = await _DocumentStore.GetCollection();

            var events = new IEvent[]
            {
                new ContentCreated {AggregateId = c.Id, EntityId = c.Id, Data = c}
            };

            foreach (var @event in events) await eventCollection.AddEvent(c.Id, @event);

            return await Task.FromResult(c);
        }

        #endregion Public Methods
    }
}