using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventStore.API.Model;
using EventStore.API.Model.Validation;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;

namespace EventStore.API.Queries.Order.Handler
{
    public class OrderDetailQueryHandler : IRequestHandler<OrderDetailQuery, Domain.Entity.Order>
    {
        #region Public Constructors

        public OrderDetailQueryHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Private Properties

        private IEventStore _DocumentStore { get; }

        #endregion Private Properties

        #region Public Methods

        public async Task<Domain.Entity.Order> Handle(OrderDetailQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await new OrderDetailQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }


            var collection = await _DocumentStore.GetCollection();

            var content = await collection.Query<Domain.Entity.Order>(request.OrderId);

            return content;
        }

        #endregion Public Methods
    }
}