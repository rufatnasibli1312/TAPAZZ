using DTO.FavouriteDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.FavouriteValidator
{
    public class DeleteFavouriteValidator : AbstractValidator<DeleteFavouriteDto>
    {
        public DeleteFavouriteValidator()
        {
            RuleFor(m => m.Id).NotEqual(0)
           .WithMessage("Id must be greater than 0 ");
        }
    }
}
