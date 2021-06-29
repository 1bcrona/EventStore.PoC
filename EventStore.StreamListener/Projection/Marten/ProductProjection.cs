using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStore.StreamListener.Projection.Marten
{
    public class ProductProjection : BaseMartenProjection<Product, Guid>
    {
        #region Public Constructors

        public ProductProjection()
        {
            ProjectEvent<OrderPlaced>(@event => @event.Data.OrderProduct.Id, (product, e) =>
            {
                product.Stock -= e.Data.Quantity;
            });

            ProjectEvent<ProductCreated>(@event => @event.EntityId, (product, e) =>
            {
                product.Id = e.EntityId;
                product.ProductMetadata = e.Data.ProductMetadata;
                product.ProductLocation = e.Data.ProductLocation;
                product.Stock = e.Data?.Stock ?? 0;
                product.Price = e.Data.Price;
                product.Active = true;
            });

            ProjectEvent<ProductDeleted>(@event => @event.EntityId, (product, _) =>
            {
                product.Active = false;
            });
        }



        #endregion Public Constructors
    }
}