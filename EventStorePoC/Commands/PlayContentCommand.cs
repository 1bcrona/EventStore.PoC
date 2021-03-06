using MediatR;
using System;

namespace EventStore.PoC.API.Commands
{
    public class PlayContentCommand : IRequest<bool>
    {
        #region Public Properties

        public Guid ContentId { get; set; }

        #endregion Public Properties
    }
}