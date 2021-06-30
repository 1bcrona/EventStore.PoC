using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.Event.Impl;
using System;

namespace EventStore.API.Aggregate.Product
{
    public class BaseAggregate<T, K> where T : BaseEntity<K>
    {
        #region Public Properties

        public T Data { get; set; }
        public K Id { get; set; }

        #endregion Public Properties
    }

    public class CustomerAggregate : BaseAggregate<Domain.Entity.Customer, Guid>
    {
        #region Public Methods

        public void Apply(CustomerCreated e)
        {
            var customer = new Domain.Entity.Customer()
            {
                Id = e.EntityId,
                Name = e.Data?.Name,
                Surname = e.Data?.Surname,
                Address = e.Data?.Address,
                Active = true,
            };

            Data = customer;
        }

        public void Apply(CustomerDeleted e)
        {
            if (Data == null) return;
            Data.Active = false;
        }

        #endregion Public Methods
    }

    public class OrderAggregate : BaseAggregate<Domain.Entity.Order, Guid>
    {
        #region Public Methods

        public void Apply(OrderCreated e)
        {
            var order = new Domain.Entity.Order()
            {
                Id = e.EntityId,
                TotalPrice = e.Data.TotalPrice,
                Quantity = e.Data.Quantity,
                Active = true,
            };
            order.AssignCustomerId(e.Data.OrderCustomerId);
            order.AssignProductId(e.Data.OrderProductId);

            Data = order;
        }

        public void Apply(OrderDeleted e)
        {
            if (Data == null) return;
            Data.Active = false;
        }

        #endregion Public Methods
    }

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