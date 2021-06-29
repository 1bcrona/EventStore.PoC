using System;
using MediatR;

namespace EventStore.API.Queries.Order
{
    public class OrderDetailQuery : IRequest<Domain.Entity.Order>
    {
        #region Public Properties

        public Guid OrderId { get; set; }

        #endregion Public Properties
    }
}