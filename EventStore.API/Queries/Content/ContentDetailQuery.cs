using System;
using MediatR;

namespace EventStore.API.Queries.Content
{
    public class ContentDetailQuery : IRequest<Domain.Entity.Content>
    {
        #region Public Properties

        public Guid ContentId { get; set; }

        #endregion Public Properties
    }
}