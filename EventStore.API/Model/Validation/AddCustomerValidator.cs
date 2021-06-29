﻿using EventStore.API.Commands.Customer;
using FluentValidation;

namespace EventStore.API.Model.Validation
{
    public class AddCustomerValidator : AbstractValidator<AddCustomerCommand>
    {
        public AddCustomerValidator()
        {
            RuleFor(command => command).NotNull();
            RuleFor(command => command.Name).NotNull().NotEmpty();
            RuleFor(command => command.Surname).NotNull().NotEmpty();
            RuleFor(command => command.Address).NotNull().NotEmpty();

        }
    }
}