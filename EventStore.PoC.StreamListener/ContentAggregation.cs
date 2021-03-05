using System;
using System.Collections.Generic;
using System.Text;
using EventStore.PoC.Domain.Entity;
using EventStore.PoC.Domain.Event.Impl;
using Marten;
using Marten.Events.Projections;

namespace EventStore.PoC.StreamListener
{
    public class ContentAggregation : ViewProjection<Content, Guid>
    {
        public object Id { get; }
        public ContentAggregation()
        {
            ProjectEvent<ContentCreated>(Persist);
            DeleteEvent<ContentDeleted>();
        }

        private void Persist(Content arg1, ContentCreated arg2)
        {
            arg1.ContentCdnLink = arg2.Data.ContentCdnLink;
            arg1.ContentMetadata = arg2.Data.ContentMetadata;
            arg1.PlayCount = arg2.Data.PlayCount;
            arg1.SetId(arg2.AggregateId.ToString());
        }


    }
}
