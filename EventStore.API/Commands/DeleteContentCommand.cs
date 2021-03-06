using MediatR;
using System;

namespace EventStore.API.Commands
{
    public class DeleteContentCommand : IRequest<bool>
    {
        #region Public Properties

        public Guid ContentId { get; set; }

        #endregion Public Properties
    }
}