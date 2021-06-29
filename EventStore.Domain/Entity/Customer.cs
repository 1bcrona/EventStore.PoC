using EventStore.Domain.Entity.Infrastructure;
using System;

namespace EventStore.Domain.Entity
{
    public class Customer : BaseEntity<Guid>
    {
        #region Public Constructors

        public Customer(string name) : this(Guid.NewGuid())
        {
            Name = name;
        }

        public Customer() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        private Customer(Guid id)
        {
            Id = id;
        }

        #endregion Private Constructors

        #region Public Properties

        public string Address { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        #endregion Public Properties
    }
}