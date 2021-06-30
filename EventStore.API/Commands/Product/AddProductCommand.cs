using EventStore.API.Model.Response.Dto;
using EventStore.Domain.ValueObject;
using MediatR;

namespace EventStore.API.Commands.Product
{
    public class AddProductCommand : IRequest<ProductDto>
    {
        #region Public Properties

        public Price Price { get; set; }
        public int Stock { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        #endregion Public Properties
    }
}