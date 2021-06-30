using EventStore.API.Model;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Model.Validation;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventStore.API.Aggregate;

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

            var order = (await collection.AggregateStream<OrderAggregate>(request.OrderId)).Data;

            if (order == null)
            {
                throw new ApiException("ORDER_NOT_FOUND", "Order can not be found", HttpStatusCode.InternalServerError);
            }

            if (!order.Active) return null;


            var product = (await collection.AggregateStream<ProductAggregate>(order.OrderProductId)).Data;
            var customer = (await collection.AggregateStream<CustomerAggregate>(order.OrderCustomerId)).Data;

            var orderDto = new OrderDto()
            {
                Id = order.Id.ToString(),
                TotalPrice = order.TotalPrice,
                Quantity = order.Quantity,
                Product = product,
                Customer = customer
            };

            return orderDto;
        }

        #endregion Public Methods
    }
}