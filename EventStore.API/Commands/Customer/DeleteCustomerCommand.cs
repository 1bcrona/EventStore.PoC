using System;
using MediatR;

namespace EventStore.API.Commands.Customer
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        #region Public Properties

        public Guid CustomerId { get; set; }

        #endregion Public Properties
    }
}