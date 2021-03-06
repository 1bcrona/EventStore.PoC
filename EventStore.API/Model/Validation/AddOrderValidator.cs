using EventStore.API.Commands.Order;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    {
        #region Public Constructors

        public AddOrderValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.CustomerId).NotNull().NotEmpty();
            RuleFor(command => command.ProductId).NotNull().NotEmpty();
            RuleFor(command => command.Quantity).NotNull().NotEmpty();
        }

        #endregion Public Constructors
    }
}