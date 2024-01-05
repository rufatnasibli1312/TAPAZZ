using DTO.AccountDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.UserValidator
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(m => m.Email).NotNull().WithMessage("Email daxil edin ...");
            RuleFor(m => m.Email).EmailAddress().WithMessage("Email Hesabi Duzgun Daxil edilmedi ...");
            RuleFor(m => m.Password).NotNull().WithMessage("Shifre Daxil edilmedi ...");
            
        }
    }
}
