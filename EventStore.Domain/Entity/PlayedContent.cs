using EventStore.Domain.Entity.Infrastructure;
using System;

namespace EventStore.Domain.Entity
{
    public class PlayedContent : BaseEntity<Guid>, IAggregateRoot
    {
        #region Public Constructors

        public PlayedContent(Guid id)
        {
            this.Id = id;
        }

        public PlayedContent() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Content ViewContent { get; set; }
        public User ViewedUser { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void AssignContent(Content c)
        {
            ViewContent = c;
        }

        public void AssignUser(User u)
        {
            ViewedUser = u;
        }

        #endregion Public Methods
    }
}