using System;
using MediatR;

namespace EventStore.API.Queries.Content
{
    public class ContentDetailQuery : IRequest<Domain.Entity.Content>
    {
        public Guid ContentId { get; set; }
    }
}
