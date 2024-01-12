using DTO.AccountDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.UserValidator
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserDto>
    {
        public DeleteUserValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithMessage("Id cannot be empty.");
        }
    }
}
