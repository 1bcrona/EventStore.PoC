using EventStore.API.Queries.Order;
using EventStore.API.Queries.Product;
using EventStore.API.Queries.Product.Handler;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class OrderDetailQueryValidator : AbstractValidator<OrderDetailQuery>
    {
        public OrderDetailQueryValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.OrderId).NotNull().NotEmpty();

        }
    }
}