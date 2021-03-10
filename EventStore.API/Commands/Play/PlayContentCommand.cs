using MediatR;
using System;

namespace EventStore.API.Commands
{
    public class PlayContentCommand : IRequest<bool>
    {
        #region Public Properties

        public Guid ContentId { get; set; }
        public Guid UserId { get; set; }

        #endregion Public Properties
    }
}