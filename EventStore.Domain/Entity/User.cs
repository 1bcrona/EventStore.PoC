using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Domain.Entity.Infrastructure;

namespace EventStore.Domain.Entity
{
    public class User : BaseEntity<Guid>
    {
        public User(Guid id)
        {
            this.Id = id;
        }

        public User() : this(Guid.NewGuid())
        {
        }

        public string UserName { get; set; }
    }
}
