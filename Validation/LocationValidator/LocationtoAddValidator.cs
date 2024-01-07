using DTO.LocationDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.LocationValidator
{
    public class LocationtoAddValidator : AbstractValidator<LocationtoAddDTO>
    {
        public LocationtoAddValidator()
        {
            RuleFor(m => m.Name)
         .NotEmpty().WithMessage("Location name cannot be empty")
         .MaximumLength(20).WithMessage("Location name cannot exceed 20 characters");
        }
    }
}
