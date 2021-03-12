using System;
using EventStore.Domain.Entity.Infrastructure;

namespace EventStore.Domain.Entity
{
    public class PlayedContent : BaseEntity<Guid>, IAggregateRoot
    {
        #region Public Constructors

        public PlayedContent(Guid id)
        {
            Id = id;
        }

        public PlayedContent() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Content ViewedContent { get; set; }
        public User ViewedUser { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void AssignContent(Content c)
        {
            ViewedContent = c;
        }

        public void AssignUser(User u)
        {
            ViewedUser = u;
        }

        #endregion Public Methods
    }
}