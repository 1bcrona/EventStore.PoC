using EventStore.Domain.ValueObject;
using MediatR;

namespace EventStore.API.Commands.Product
{
    public class AddProductCommand : IRequest<Domain.Entity.Product>
    {
        #region Public Properties

        public string Title { get; set; }
        public string Url { get; set; }

        public int Stock { get; set; }

        public Price Price { get; set; }
        #endregion Public Properties
    }
}