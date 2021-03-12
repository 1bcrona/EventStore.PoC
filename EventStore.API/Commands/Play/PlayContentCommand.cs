using System;
using MediatR;

namespace EventStore.API.Commands.Play
{
    public class PlayContentCommand : IRequest<bool>
    {
        #region Public Properties

        public Guid ContentId { get; set; }
        public Guid UserId { get; set; }

        #endregion Public Properties
    }
}