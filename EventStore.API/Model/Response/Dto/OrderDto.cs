using EventStore.Domain.ValueObject;

namespace EventStore.API.Model.Response.Dto
{
    public class OrderDto
    {
        #region Public Properties

        public CustomerDto Customer { get; set; }
        public string Id { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public Price TotalPrice { get; set; }

        #endregion Public Properties
    }
}