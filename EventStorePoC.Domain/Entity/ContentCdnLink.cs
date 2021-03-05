using System;
using EventStore.PoC.Domain.Entity.Infrastructure;

namespace EventStore.PoC.Domain.Entity
{
    public class ContentCdnLink
    {
        public ContentCdnLink(string url)
        {
            Url = url;
        }

        public ContentCdnLink() : this(null)
        {
        }

        public string Url { get; set; }

    }
}