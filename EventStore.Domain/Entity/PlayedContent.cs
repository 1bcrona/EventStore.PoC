using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Domain.Entity.Infrastructure;

namespace EventStore.Domain.Entity
{
    public class PlayedContent : BaseEntity<Guid>, IAggregateRoot
    {
        public PlayedContent(Guid id)
        {
            this.Id = id;
        }

        public PlayedContent() : this(Guid.NewGuid())
        {
        }

        public Content ViewContent { get; set; }
        public User ViewedUser { get; set; }

        public void AssignContent(Content c)
        {
            ViewContent = c;
        }

        public void AssignUser(User u)
        {
            ViewedUser = u;
        }

    }
}
