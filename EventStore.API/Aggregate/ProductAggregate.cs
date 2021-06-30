using System;
using EventStore.API.Aggregate.Base;
using EventStore.Domain.Event.Impl;

namespace EventStore.API.Aggregate
{
    public class ProductAggregate : BaseAggregate<Domain.Entity.Product, Guid>
    {
        #region Public Methods

        public void Apply(ProductStockUpdated e)
        {
            if (Data == null) return;
            Data.Stock += e.Data;
        }

        public void Apply(ProductCreated e)
        {
            var product = new Domain.Entity.Product
            {
                Id = e.EntityId,
                ProductMetadata = e.Data.ProductMetadata,
                ProductLocation = e.Data.ProductLocation,
                Stock = (int)e.Data?.Stock,
                Price = e.Data.Price,
                Active = true
            };

            Data = product;
        }

        public void Apply(ProductDeleted e)
        {
            if (Data == null) return;
            Data.Active = false;
        }

        #endregion Public Methods
    }
}