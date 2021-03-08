using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Queries.User.Handler
{
    public class UserDetailQueryHandler : IRequestHandler<UserDetailQuery, Domain.Entity.User>
    {
        #region Public Constructors

        public UserDetailQueryHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Private Properties

        private IEventStore _DocumentStore { get; set; }

        #endregion Private Properties

        #region Public Methods

        public async Task<Domain.Entity.User> Handle(UserDetailQuery request, CancellationToken cancellationToken)
        {
            var collection = _DocumentStore.GetCollection();

            var content = await collection.Query<Domain.Entity.User>(request.UserId);

            return content;
        }

        #endregion Public Methods
    }
}