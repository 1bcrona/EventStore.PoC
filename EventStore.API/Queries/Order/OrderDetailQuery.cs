using EventStore.API.Model.Response.Dto;
using MediatR;
using System;

namespace EventStore.API.Queries.Order
{
    public class OrderDetailQuery : IRequest<OrderDto>
    {
        #region Public Properties

        public Guid OrderId { get; set; }

        #endregion Public Properties
    }
}