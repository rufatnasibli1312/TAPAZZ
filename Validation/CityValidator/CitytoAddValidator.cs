using DTO.LocationDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.LocationValidator
{
    public class CitytoAddValidator : AbstractValidator<CitytoAddDTO>
    {
        public CitytoAddValidator()
        {
            RuleFor(m => m.Name)
         .NotEmpty().WithMessage("City name cannot be empty")
         .MaximumLength(20).WithMessage("City name cannot exceed 20 characters");
        }
    }
}
