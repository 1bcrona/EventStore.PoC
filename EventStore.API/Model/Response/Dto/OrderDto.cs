using System;
using EventStore.Domain.Entity;
using EventStore.Domain.ValueObject;

namespace EventStore.API.Model.Response.Dto
{
    public class OrderDto
    {
        #region Public Properties
        public Guid OrderId { get; set; }
        public Price Amount { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        #endregion Public Properties
    }
}