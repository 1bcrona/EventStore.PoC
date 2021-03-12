using System.Threading;
using System.Threading.Tasks;
using EventStore.Domain.Event.Impl;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;

namespace EventStore.API.Commands.User.Handler
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Domain.Entity.User>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public AddUserCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Domain.Entity.User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var u = new Domain.Entity.User { UserName = request.UserName };

            var eventCollection = await _DocumentStore.GetCollection();

            var events = new IEvent[]
            {
                new UserCreated {AggregateId = u.Id, EntityId = u.Id, Data = u}
            };

            foreach (var @event in events) await eventCollection.AddEvent(u.Id, @event);

            return await Task.FromResult(u);
        }

        #endregion Public Methods
    }
}