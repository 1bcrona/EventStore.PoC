using System;
using System.Collections.Generic;
using System.Text;
using EventStore.PoC.Domain.Entity;
using EventStore.PoC.Domain.Event.Impl;
using Marten;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

namespace EventStore.PoC.StreamListener
{
    public class ContentAggregation : AggregateProjection<Content>
    {
        public ContentAggregation()
        {
            DeleteEvent<ContentDeleted>();
            Lifecycle = ProjectionLifecycle.Live;

        }

        public Content Create(ContentCreated contentCreated)
        {
            return new()
            {
                ContentCdnLink = contentCreated.Data.ContentCdnLink,
                ContentMetadata = contentCreated.Data.ContentMetadata,
                PlayCount = contentCreated.Data.PlayCount
            };
        }


    }
}
