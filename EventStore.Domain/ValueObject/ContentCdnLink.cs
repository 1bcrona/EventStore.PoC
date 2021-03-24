using System;
using System.Runtime.CompilerServices;

namespace EventStore.Domain.ValueObject
{
    public class ContentCdnLink
    {
        #region Public Properties

        public string Url { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public ContentCdnLink(string url)
        {
            ValidateCdnLink(url);
            Url = url;
        }


        public ContentCdnLink() : this(String.Empty)
        {
        }

        private bool ValidateCdnLink(string url)
        {
            var normalizedUrl = (url ?? string.Empty).Trim();
            return !string.IsNullOrEmpty(normalizedUrl);
        }

        #endregion Public Constructors
    }
}