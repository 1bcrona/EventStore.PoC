using EventStore.API.Commands.Product;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.ProductId).NotNull().NotEmpty();
        }
    }
}