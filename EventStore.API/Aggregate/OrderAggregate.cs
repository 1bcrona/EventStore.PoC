using EventStore.API.Aggregate.Base;
using EventStore.Domain.Event.Impl;
using System;

namespace EventStore.API.Aggregate
{
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
}