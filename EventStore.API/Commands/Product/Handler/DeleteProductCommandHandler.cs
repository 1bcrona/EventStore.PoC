using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventStore.API.Model;
using EventStore.API.Model.Validation;
using EventStore.Domain.Event.Impl;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;

namespace EventStore.API.Commands.Product.Handler
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public DeleteProductCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new DeleteProductValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var eventCollection = await _DocumentStore.GetCollection();

            await eventCollection.AddEvent(request.ProductId, new ProductDeleted { AggregateId = request.ProductId, EntityId = request.ProductId });

            return true;
        }

        #endregion Public Methods
    }
}