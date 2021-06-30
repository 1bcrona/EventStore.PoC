using EventStore.API.Model.Response.Dto;
using MediatR;
using System;

namespace EventStore.API.Queries.Product
{
    public class ProductDetailQuery : IRequest<ProductDto>
    {
        #region Public Properties

        public Guid ProductId { get; set; }

        #endregion Public Properties
    }
}