using EventStore.API.Commands.Customer;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.CustomerId).NotNull().NotEmpty();

        }
    }
}