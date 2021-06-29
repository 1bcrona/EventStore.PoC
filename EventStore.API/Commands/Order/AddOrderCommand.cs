using System;
using EventStore.Domain.ValueObject;
using MediatR;

namespace EventStore.API.Commands.Order
{
    public class AddOrderCommand : IRequest<Domain.Entity.Order>
    {
        #region Public Properties

        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }

        public int Quantity { get; set; }

        #endregion Public Properties
    }
}