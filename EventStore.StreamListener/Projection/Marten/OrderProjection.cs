using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using System;

namespace EventStore.StreamListener.Projection.Marten
{
    public class OrderProjection : BaseMartenProjection<Order, Guid>
    {
        #region Public Constructors

        public OrderProjection()
        {
            ProjectEvent<OrderCreated>(@event => @event.EntityId, (order, e) =>
            {
                order.Id = e.EntityId;
                order.AssignCustomerId(e.Data.OrderCustomerId);
                order.AssignProductId(e.Data.OrderProductId);
                order.TotalPrice = e.Data.TotalPrice;
                order.Quantity = e.Data.Quantity;
                order.Active = true;
            });

            ProjectEvent<OrderDeleted>(@event => @event.EntityId, (playedContent, _) =>
            {
                playedContent.Active = false;
            });
        }

        #endregion Public Constructors
    }
}