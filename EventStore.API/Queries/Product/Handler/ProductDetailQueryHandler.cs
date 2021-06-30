using System;
using EventStore.API.Model;
using EventStore.API.Model.Validation;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventStore.API.Aggregate.Product;
using EventStore.API.Model.Response.Dto;

namespace EventStore.API.Queries.Product.Handler
{
    public class ProductDetailQueryHandler : IRequestHandler<ProductDetailQuery, ProductDto>
    {
        #region Public Constructors
        public ProductDetailQueryHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Private Properties

        private IEventStore _DocumentStore { get; }

        #endregion Private Properties

        #region Public Methods

        public async Task<ProductDto> Handle(ProductDetailQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await new ProductDetailQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var collection = await _DocumentStore.GetCollection();

            var product = (await collection.AggregateStream<ProductAggregate>(request.ProductId)).Data;

            if (product == null) return null;
            return !product.Active ? null : product;
        }

        #endregion Public Methods
    }
}