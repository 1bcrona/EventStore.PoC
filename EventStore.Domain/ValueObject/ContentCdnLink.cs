using System;

namespace EventStore.Domain.ValueObject
{
    public class ContentCdnLink
    {
        #region Public Constructors

        public ContentCdnLink(string url)
        {
            Url = url;
        }

        private bool ValidateCdnLink(string url)
        {
            var normalizedUrl = (url ?? String.Empty).Trim();
            return !String.IsNullOrEmpty(normalizedUrl);
        }

        public ContentCdnLink() : this(null)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string Url { get; set; }

        #endregion Public Properties
    }
}