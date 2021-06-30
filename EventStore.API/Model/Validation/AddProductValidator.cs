using EventStore.API.Commands.Product;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class AddProductValidator : AbstractValidator<AddProductCommand>
    {
        #region Public Constructors

        public AddProductValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.Title).NotNull().NotEmpty();
            RuleFor(command => command.Url).NotNull().NotEmpty();
            RuleFor(command => command.Price).NotNull();
            RuleFor(command => command.Price != null && command.Price.Currency != null);
            RuleFor(command => command.Price != null && command.Price.Value > 0);
            RuleFor(command => command.Stock).GreaterThan(0);
        }

        #endregion Public Constructors
    }
}