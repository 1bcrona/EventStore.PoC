using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using System;

namespace EventStore.StreamListener.Projection.Marten
{
    public class CustomerProjection : BaseMartenProjection<Customer, Guid>
    {
        #region Public Constructors

        public CustomerProjection()
        {
            ProjectEvent<CustomerCreated>(@event => @event.EntityId, (customer, e) =>
            {
                customer.Id = e.EntityId;
                customer.Name = e.Data?.Name;
                customer.Surname = e.Data?.Surname;
                customer.Address = e.Data?.Address;
                customer.Active = true;
            });

            ProjectEvent<CustomerDeleted>(@event => @event.EntityId, (customer, _) =>
                {
                    customer.Active = false;
                }
                );
        }

        #endregion Public Constructors
    }
}