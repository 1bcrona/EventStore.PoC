using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.ValueObject;
using System;

namespace EventStore.Domain.Entity
{
    public class Product : BaseEntity<Guid>
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

        public Price Price { get; set; }
        public ProductLocation ProductLocation { get; set; }
        public ProductMetadata ProductMetadata { get; set; }
        public int Stock { get; set; }

        #endregion Public Properties
    }
}