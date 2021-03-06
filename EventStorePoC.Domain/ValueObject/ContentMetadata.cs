namespace EventStore.PoC.Domain.ValueObject
{
    public class ContentMetadata
    {
        #region Public Constructors

        public ContentMetadata(string title)
        {
            Title = title;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Title { get; set; }

        #endregion Public Properties
    }
}