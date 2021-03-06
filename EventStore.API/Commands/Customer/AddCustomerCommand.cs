using EventStore.API.Model.Response.Dto;
using MediatR;

namespace EventStore.API.Commands.Customer
{
    public class AddCustomerCommand : IRequest<CustomerDto>
    {
        #region Public Properties

        public string Address { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        #endregion Public Properties
    }
}