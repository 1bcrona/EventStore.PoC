using EventStore.API.Model;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Model.Validation;
using EventStore.Domain.Event.Impl;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.API.Commands.Customer.Handler
{
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, CustomerDto>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public AddCustomerCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<CustomerDto> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new AddCustomerValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }

            var customer = new Domain.Entity.Customer
            {
                Name = request.Name,
                Surname = request.Surname,
                Address = request.Address
            };

            var eventCollection = await _DocumentStore.GetCollection();

            await eventCollection.AddEvent(customer.Id, new CustomerCreated { AggregateId = customer.Id, EntityId = customer.Id, Data = customer });

            return customer;
        }

        #endregion Public Methods
    }
}