using EventStore.Domain.Entity.Infrastructure;
using System;
using System.Security.Principal;

namespace EventStore.Domain.Entity
{
    public class User : BaseEntity<Guid>
    {
        private User(Guid id)
        {
            Id = id;
        }

        public User(string name) : this(Guid.NewGuid())
        {
            ValidateUserName(name);
            UserName = name;
        }

        public User() : this(Guid.NewGuid())
        {
        }

        public string UserName { get; set; }

        public void ValidateUserName(string name)
        {
            var normalizedUrl = (name ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(normalizedUrl))
            {
                throw new Exception("NOT_A_VALID_USER_NAME");
            }

        }
    }
}