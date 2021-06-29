using EventStore.API.Queries.Order;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class OrderDetailQueryValidator : AbstractValidator<OrderDetailQuery>
    {
        #region Public Constructors

        public OrderDetailQueryValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.OrderId).NotNull().NotEmpty();
        }

        #endregion Public Constructors
    }
}