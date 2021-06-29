using EventStore.Domain.Entity.Infrastructure;
using System;
using EventStore.Domain.ValueObject;

namespace EventStore.Domain.Entity
{
    public class Order : BaseEntity<Guid>, IAggregateRoot
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

        public Product OrderProduct { get; set; }
        public Customer OrderCustomer { get; set; }
        public int Quantity { get; set; }

        public Price Amount { get; set; }

        #endregion Public Properties

        #region Public Methods



        public void AssignProduct(Product p)
        {
            OrderProduct = p;
        }

        public void AssignCustomer(Customer c)
        {
            OrderCustomer = c;
        }

        #endregion Public Methods
    }
}