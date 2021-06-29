using EventStore.API.Queries.Product;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class ProductDetailQueryValidator : AbstractValidator<ProductDetailQuery>
    {
        #region Public Constructors

        public ProductDetailQueryValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.ProductId).NotNull().NotEmpty();
        }

        #endregion Public Constructors
    }
}