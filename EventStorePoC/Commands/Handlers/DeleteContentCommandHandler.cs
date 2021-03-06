using EventStore.PoC.Domain.Event.Impl;
using EventStore.PoC.Domain.Event.Infrastructure;
using EventStore.PoC.Store.EventStore.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.PoC.API.Commands.Handlers
{
    public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand, bool>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public DeleteContentCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
        {
            var eventCollection = _DocumentStore.GetCollection();

            var events = new IEvent[]
            {
                new ContentDeleted() { AggregateId = request.ContentId, EntityId = request.ContentId, Data = "Content Deleted"},
            };

            foreach (var @event in events)
            {
                await eventCollection.AddEvent(request.ContentId, @event);
            }

            return await Task.FromResult(true);
        }

        #endregion Public Methods
    }
}