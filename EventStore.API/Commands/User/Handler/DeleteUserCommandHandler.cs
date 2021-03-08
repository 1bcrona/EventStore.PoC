using EventStore.Domain.Event.Impl;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Commands.User.Handler
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public DeleteUserCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var eventCollection = _DocumentStore.GetCollection();

            var events = new IEvent[]
            {
                new UserDeleted() { AggregateId = request.UserId, EntityId = request.UserId, Data = "User Deleted"},
            };

            foreach (var @event in events)
            {
                await eventCollection.AddEvent(request.UserId, @event);
            }

            return await Task.FromResult(true);
        }

        #endregion Public Methods
    }
}