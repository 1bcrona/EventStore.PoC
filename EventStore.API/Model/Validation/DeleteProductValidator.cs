using EventStore.API.Commands.Product;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        #region Public Constructors

        public DeleteProductValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.ProductId).NotNull().NotEmpty();
        }

        #endregion Public Constructors
    }
}