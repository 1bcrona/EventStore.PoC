using System;
using MediatR;

namespace EventStore.API.Queries.Customer
{
    public class CustomerDetailQuery : IRequest<Domain.Entity.Customer>
    {
        #region Public Properties

        public Guid CustomerId { get; set; }

        #endregion Public Properties
    }
}