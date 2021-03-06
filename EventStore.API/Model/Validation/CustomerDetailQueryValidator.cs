using EventStore.API.Queries.Customer;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class CustomerDetailQueryValidator : AbstractValidator<CustomerDetailQuery>
    {
        #region Public Constructors

        public CustomerDetailQueryValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.CustomerId).NotNull().NotEmpty();
        }

        #endregion Public Constructors
    }
}