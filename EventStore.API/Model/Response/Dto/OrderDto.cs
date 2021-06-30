using System;
using EventStore.Domain.Entity;
using EventStore.Domain.ValueObject;

namespace EventStore.API.Model.Response.Dto
{
    public class OrderDto
    {
        #region Public Properties
        public string Id { get; set; }
        public Price TotalPrice { get; set; }
        public CustomerDto Customer { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }

        #endregion Public Properties
    }
}