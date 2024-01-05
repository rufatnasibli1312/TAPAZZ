using DTO.LocationDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.LocationValidator
{
    public class LocationToUpdateValidator : AbstractValidator<LocationToUpdateDTO>
    {
        public LocationToUpdateValidator()
        {
            RuleFor(location => location.Name)
            .NotEmpty().WithMessage("Location name cannot be empty")
            .MaximumLength(50).WithMessage("Location name cannot exceed 50 characters");
            RuleFor(m => m.Id)
               .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0");
        }
    }
}
