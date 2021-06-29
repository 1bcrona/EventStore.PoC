using MediatR;
using System;

namespace EventStore.API.Commands.Order
{
    public class AddOrderCommand : IRequest<Domain.Entity.Order>
    {
        #region Public Properties

        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        #endregion Public Properties
    }
}