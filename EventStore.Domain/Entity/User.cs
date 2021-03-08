using EventStore.Domain.Entity.Infrastructure;
using System;

namespace EventStore.Domain.Entity
{
    public class User : BaseEntity<Guid>
    {
        #region Public Constructors

        public User(Guid id)
        {
            this.Id = id;
        }

        public User() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string UserName { get; set; }

        #endregion Public Properties
    }
}