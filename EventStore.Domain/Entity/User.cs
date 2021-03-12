using System;
using EventStore.Domain.Entity.Infrastructure;

namespace EventStore.Domain.Entity
{
    public class User : BaseEntity<Guid>
    {
        #region Public Properties

        public string UserName { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public User(Guid id)
        {
            Id = id;
        }

        public User() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors
    }
}