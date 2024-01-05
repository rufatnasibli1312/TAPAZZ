using DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.FavouriteValidator
{
    public class FavouriteAddValidator : AbstractValidator<FavouriteAddDto>
    {
        public FavouriteAddValidator()
        {
            RuleFor(m => m.ProductId)
           .GreaterThan(0).WithMessage("ProductId must be greater than 0");
        }
    }
}
