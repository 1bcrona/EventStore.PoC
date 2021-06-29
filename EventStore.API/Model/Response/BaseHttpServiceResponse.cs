namespace EventStore.API.Model.Response
{
    public class BaseHttpServiceResponse<TData>
    {
        #region Public Properties

        public TData Data { get; set; }
        public ErrorModel Error { get; set; }
        public bool HasError => Error != null;

        #endregion Public Properties
    }

    public class ErrorModel
    {
        #region Public Properties

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        #endregion Public Properties
    }
}
