using System;
using EventStore.PoC.Domain.Entity.Infrastructure;

namespace EventStore.PoC.Domain.Entity
{
    public class ContentMetadata
    {
        public ContentMetadata(string title)
        {
            Title = title;
        }


        public string Title { get; set; }


    }
}