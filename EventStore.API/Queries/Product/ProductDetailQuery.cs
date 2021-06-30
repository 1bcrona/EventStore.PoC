using MediatR;
using System;
using EventStore.API.Model.Response.Dto;

namespace EventStore.API.Queries.Product
{
    public class ProductDetailQuery : IRequest<ProductDto>
    {
        #region Public Properties

        public Guid ProductId { get; set; }

        #endregion Public Properties
    }
}