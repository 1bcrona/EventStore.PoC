using MediatR;
using System;

namespace EventStore.API.Commands.Product
{
    public class DeleteProductCommand : IRequest<bool>
    {
        #region Public Properties

        public Guid ProductId { get; set; }

        #endregion Public Properties
    }
}