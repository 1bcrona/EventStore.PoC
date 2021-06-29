using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.ValueObject;
using System;

namespace EventStore.Domain.Entity
{
    public class Product : BaseEntity<Guid>, IAggregateRoot
    {
        #region Public Constructors

        public Product(Guid id)
        {
            Id = id;
        }

        public Product() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public ProductLocation ProductLocation { get; set; }
        public ProductMetadata ProductMetadata { get; set; }
        public int Stock { get; set; }
        public Price Price { get; set; }

        #endregion Public Properties
    }
}