using EventStore.API.Model;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Model.Validation;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Queries.Order.Handler
{
    public class OrderDetailQueryHandler : IRequestHandler<OrderDetailQuery, OrderDto>
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

        public async Task<OrderDto> Handle(OrderDetailQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await new OrderDetailQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var collection = await _DocumentStore.GetCollection();

            var order = await collection.Query<Domain.Entity.Order>(request.OrderId);

            if (order == null)
            {
                throw new ApiException("ORDER_NOT_FOUND", "Order can not be found", HttpStatusCode.InternalServerError);
            }

            var product = await collection.Query<Domain.Entity.Product>(order.OrderProductId);
            var customer = await collection.Query<Domain.Entity.Customer>(order.OrderCustomerId);

            var orderDto = new OrderDto()
            { Amount = order.Amount, Quantity = order.Quantity, Product = product, Customer = customer };

            return orderDto;
        }

        #endregion Public Methods
    }
}