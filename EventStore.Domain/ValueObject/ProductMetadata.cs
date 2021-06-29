namespace EventStore.Domain.ValueObject
{
    public class ProductMetadata
    {
        #region Public Constructors

        public ProductMetadata(string title)
        {
            Title = title;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Title { get; set; }


        #endregion Public Properties
    }
}