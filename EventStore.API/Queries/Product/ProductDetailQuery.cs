using MediatR;
using System;

namespace EventStore.API.Queries.Product
{
    public class ProductDetailQuery : IRequest<Domain.Entity.Product>
    {
        #region Public Properties

        public Guid ProductId { get; set; }

        #endregion Public Properties
    }
}