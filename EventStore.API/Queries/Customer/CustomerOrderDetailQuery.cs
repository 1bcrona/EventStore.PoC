using EventStore.API.Model.Response.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace EventStore.API.Queries.Customer
{
    public class CustomerOrderDetailQuery : IRequest<List<OrderDto>>
    {
        #region Public Properties

        public Guid CustomerId { get; set; }

        #endregion Public Properties
    }
}