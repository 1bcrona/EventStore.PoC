using MediatR;

namespace EventStore.API.Commands.Customer
{
    public class AddCustomerCommand : IRequest<Domain.Entity.Customer>
    {
        #region Public Properties

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }

        #endregion Public Properties
    }
}