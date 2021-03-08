using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Queries.Content.Handler
{
    public class ContentDetailQueryHandler : IRequestHandler<ContentDetailQuery, Domain.Entity.Content>
    {
        #region Public Constructors

        public ContentDetailQueryHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Private Properties

        private IEventStore _DocumentStore { get; set; }

        #endregion Private Properties

        #region Public Methods

        public async Task<Domain.Entity.Content> Handle(ContentDetailQuery request, CancellationToken cancellationToken)
        {
            var collection = _DocumentStore.GetCollection();

            var content = await collection.Query<Domain.Entity.Content>(request.ContentId);

            return content;
        }

        #endregion Public Methods
    }
}