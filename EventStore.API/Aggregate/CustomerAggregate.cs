using System;
using EventStore.API.Aggregate.Base;
using EventStore.Domain.Event.Impl;

namespace EventStore.API.Aggregate
{
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
}