using DTO.AccountDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.UserValidator
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithMessage("Id cannot be empty.");

            RuleFor(m => m.Fullname).NotEmpty().WithMessage("Fullname cannot be empty.");

            RuleFor(m => m.Address).NotEmpty().WithMessage("Address cannot be empty.");

            RuleFor(m => m.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber cannot be empty.")
                .Matches(@"^[0-9]+$").WithMessage("PhoneNumber should contain only digits.");


        }

    }
}
