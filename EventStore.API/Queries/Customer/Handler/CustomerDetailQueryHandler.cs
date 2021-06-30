using EventStore.API.Model;
using EventStore.API.Model.Validation;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Queries.Customer.Handler
{
    public class CustomerDetailQueryHandler : IRequestHandler<CustomerDetailQuery, Domain.Entity.Customer>
    {
        #region Public Constructors

        public CustomerDetailQueryHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Private Properties

        private IEventStore _DocumentStore { get; }

        #endregion Private Properties

        #region Public Methods

        public async Task<Domain.Entity.Customer> Handle(CustomerDetailQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await new CustomerDetailQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var collection = await _DocumentStore.GetCollection();

            var content = await collection.Query<Domain.Entity.Customer>(request.CustomerId);

            if (!content.Active) return null; 

            return content;
        }

        #endregion Public Methods
    }
}