using EventStore.Domain.Entity;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Queries.Handlers
{
    public class ContentDetailQueryHandler : IRequestHandler<ContentDetailQuery, Content>
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

        public async Task<Content> Handle(ContentDetailQuery request, CancellationToken cancellationToken)
        {
            var collection = _DocumentStore.GetCollection();

            var content = await collection.Query<Content>(request.ContentId);

            return content;
        }

        #endregion Public Methods
    }
}