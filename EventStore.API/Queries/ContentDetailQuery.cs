using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Domain.Entity;
using MediatR;

namespace EventStore.API.Queries
{
    public class ContentDetailQuery : IRequest<Content>
    {
        public Guid ContentId { get; set; }
    }
}
