using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Domain.Entity;
using EventStore.Domain.ValueObject;

namespace EventStore.API.Model.Response.Dto
{
    public class ProductDto
    {
        public string Id { get; set; }
        public Price Price { get; set; }
        public ProductLocation ProductLocation { get; set; }
        public ProductMetadata ProductMetadata { get; set; }
        public int Stock { get; set; }


        public static implicit operator ProductDto(Product product)
        {
            return new() { Id = product.Id == Guid.Empty ? null : product.Id.ToString(), Price = product.Price, ProductLocation = product.ProductLocation, ProductMetadata = product.ProductMetadata, Stock = product.Stock };
        }
    }
}
