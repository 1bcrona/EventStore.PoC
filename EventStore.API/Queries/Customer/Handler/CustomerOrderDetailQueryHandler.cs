using EventStore.API.Model;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Model.Validation;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Queries.Customer.Handler
{
    public class CustomerOrderDetailQueryHandler : IRequestHandler<CustomerOrderDetailQuery, List<OrderDto>>
    {
        #region Public Constructors

        public CustomerOrderDetailQueryHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Private Properties

        private IEventStore _DocumentStore { get; }

        #endregion Private Properties

        #region Public Methods

        public async Task<List<OrderDto>> Handle(CustomerOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await new CustomerOrderDetailQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var collection = await _DocumentStore.GetCollection();

            var orders = await collection.Query<Domain.Entity.Order>();

            orders = orders.Where(w => w.OrderCustomerId == request.CustomerId);

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var product = await collection.Query<Domain.Entity.Product>(order.OrderProductId);
                var customer = await collection.Query<Domain.Entity.Customer>(order.OrderCustomerId);
                var orderDto = new OrderDto() { Amount = order.Amount, Quantity = order.Quantity, Product = product, Customer = customer };
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        #endregion Public Methods
    }
}