using System;
using EventStore.API.Model;
using EventStore.API.Model.Validation;
using EventStore.Domain.Event.Impl;
using EventStore.Domain.ValueObject;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventStore.API.Aggregate;

namespace EventStore.API.Commands.Order.Handler
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Domain.Entity.Order>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public AddOrderCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Domain.Entity.Order> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new AddOrderValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var eventCollection = await _DocumentStore.GetCollection();
            var customerDetails = (await eventCollection.AggregateStream<CustomerAggregate>(request.CustomerId)).Data;

            if (customerDetails == null || !customerDetails.Active)
            {
                throw new ApiException("CUSTOMER_NOT_FOUND", "Customer can not be found in database.", HttpStatusCode.InternalServerError);
            }
            var productDetails = (await eventCollection.AggregateStream<ProductAggregate>(request.ProductId)).Data;

            if (productDetails == null || !productDetails.Active)
            {
                throw new ApiException("PRODUCT_NOT_FOUND", "Product can not be found in database.", HttpStatusCode.InternalServerError);
            }

            if (productDetails.Stock < request.Quantity)
            {
                throw new ApiException("INSUFFICIENT_AMOUNT", "Product stock is insufficient for this order.", HttpStatusCode.InternalServerError);
            }

            var order = new Domain.Entity.Order { Quantity = request.Quantity, TotalPrice = new Price() { Currency = productDetails.Price.Currency, Value = productDetails.Price.Value * request.Quantity } };

            order.AssignProductId(productDetails.Id);
            order.AssignCustomerId(customerDetails.Id);

            await eventCollection.AddEvent(order.Id, new OrderCreated { AggregateId = order.Id, EntityId = order.Id, Data = order });
            await eventCollection.AddEvent(productDetails.Id, new ProductStockUpdated() { AggregateId = productDetails.Id, EntityId = productDetails.Id, Data = order.Quantity * -1 });

            return order;
        }

        #endregion Public Methods
    }
}