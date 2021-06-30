using EventStore.Domain.Entity;
using EventStore.Domain.ValueObject;
using System;

namespace EventStore.API.Model.Response.Dto
{
    public class ProductDto
    {
        #region Public Properties

        public string Id { get; set; }
        public Price Price { get; set; }
        public ProductLocation ProductLocation { get; set; }
        public ProductMetadata ProductMetadata { get; set; }
        public int Stock { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator ProductDto(Product product)
        {
            return new() { Id = product.Id == Guid.Empty ? null : product.Id.ToString(), Price = product.Price, ProductLocation = product.ProductLocation, ProductMetadata = product.ProductMetadata, Stock = product.Stock };
        }

        #endregion Public Methods
    }
}