using EventStore.Domain.Entity;
using EventStore.Domain.ValueObject;
using System;
using System.ComponentModel.Design;

namespace EventStore.API.Model.Response.Dto
{
    public abstract class BaseDto
    {
        public abstract bool Active { get; set; }
    }
    public class ProductDto : BaseDto
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
            return product == null ? null : new ProductDto
            {
                Id = product.Id == Guid.Empty ? null : product.Id.ToString(),
                Price = product.Price,
                ProductLocation = product.ProductLocation,
                ProductMetadata = product.ProductMetadata,
                Stock = product.Stock,
                Active = product.Active
            };
        }

        #endregion Public Methods

        public override bool Active { get; set; }
    }
}