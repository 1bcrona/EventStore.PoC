using MediatR;
using System;
using EventStore.API.Model.Response.Dto;

namespace EventStore.API.Queries.Customer
{
    public class CustomerDetailQuery : IRequest<CustomerDto>
    {
        #region Public Properties

        public Guid CustomerId { get; set; }

        #endregion Public Properties
    }
}