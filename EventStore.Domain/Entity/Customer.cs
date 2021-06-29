using EventStore.Domain.Entity.Infrastructure;
using System;
using System.Security.Principal;

namespace EventStore.Domain.Entity
{
    public class Customer : BaseEntity<Guid>
    {
        private Customer(Guid id)
        {
            Id = id;
        }

        public Customer(string name) : this(Guid.NewGuid())
        {
            Name = name;
        }

        public Customer() : this(Guid.NewGuid())
        {
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }


    }
}