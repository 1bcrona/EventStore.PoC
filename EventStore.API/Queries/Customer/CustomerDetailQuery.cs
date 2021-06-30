using EventStore.API.Model.Response.Dto;
using MediatR;
using System;

namespace EventStore.API.Queries.Customer
{
    public class CustomerDetailQuery : IRequest<CustomerDto>
    {
        #region Public Properties

        public Guid CustomerId { get; set; }

        #endregion Public Properties
    }
}