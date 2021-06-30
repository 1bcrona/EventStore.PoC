using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.ValueObject;
using System;

namespace EventStore.Domain.Entity
{
    public class Order : BaseEntity<Guid>
    {
        #region Public Constructors

        public Order(Guid id)
        {
            Id = id;
        }

        public Order() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid OrderCustomerId { get; set; }
        public Guid OrderProductId { get; set; }
        public int Quantity { get; set; }
        public Price TotalPrice { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void AssignCustomerId(Guid c)
        {
            OrderCustomerId = c;
        }

        public void AssignProductId(Guid p)
        {
            OrderProductId = p;
        }

        #endregion Public Methods
    }
}