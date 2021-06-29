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

namespace EventStore.API.Commands.Product.Handler
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Domain.Entity.Product>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public AddProductCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Domain.Entity.Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new AddProductValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var product = new Domain.Entity.Product
            {
                ProductLocation = request.Url != null ? new ProductLocation(request.Url) : null,
                ProductMetadata = request.Title != null ? new ProductMetadata(request.Title) : null,
                Price = request.Price,
                Stock = request.Stock
            };

            var eventCollection = await _DocumentStore.GetCollection();

            await eventCollection.AddEvent(product.Id, new ProductCreated { AggregateId = product.Id, EntityId = product.Id, Data = product });

            return product;
        }

        #endregion Public Methods
    }
}