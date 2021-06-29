using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.API.Commands.Product;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class AddProductValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.Title).NotNull().NotEmpty();
            RuleFor(command => command.Url).NotNull().NotEmpty();
            RuleFor(command => command.Price).NotNull();
            RuleFor(command => command.Price.Currency).NotNull();
            RuleFor(command => command.Price.Value).GreaterThan(0);
            RuleFor(command => command.Stock).NotNull();
        }
    }
}
