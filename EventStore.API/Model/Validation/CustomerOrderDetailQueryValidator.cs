using EventStore.API.Queries.Customer;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class CustomerOrderDetailQueryValidator : AbstractValidator<CustomerOrderDetailQuery>
    {
        #region Public Constructors

        public CustomerOrderDetailQueryValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.CustomerId).NotNull().NotEmpty();
        }

        #endregion Public Constructors
    }
}