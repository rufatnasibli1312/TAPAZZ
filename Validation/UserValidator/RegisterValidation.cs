using DTO.AccountDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.UserValidator
{
    public class RegisterValidation : AbstractValidator<RegisterModelDto>
    {
        public RegisterValidation()
        {
            RuleFor(m => m.Email).NotNull().WithMessage("Mail daxil edin ...");
            RuleFor(m => m.Email).EmailAddress().WithMessage("Mail duzgun daxil edilmedi (xxxxx@example.com) ...");
            RuleFor(p => p.Password).Matches(@"[A-Z]+").WithMessage("Sifrede minimum bir boyuk herf olmalidir...");
            RuleFor(p => p.Password).Matches(@"[a-z]+").WithMessage("Sifrede minimum bir kick herf olmalidir...");
            RuleFor(p => p.Password).Matches(@"[0-9]+").WithMessage("Sifrede minimum bir reqem  olmalidir.");

            RuleFor(x => x.Password).Matches(@"[\!\?\*\.]*$").WithMessage("Sifrede Bu simvollardan istifade ede bilmezsiniz (!? *.).");
            RuleFor(m => m.Address).NotNull().WithMessage("Adres bos qoyula bilmez");
            RuleFor(m => m.Fullname).NotNull().WithMessage("Ad bos qoyula bilmez");

        }

    }
}
