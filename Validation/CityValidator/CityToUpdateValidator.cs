using DTO.LocationDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.LocationValidator
{
    public class CityToUpdateValidator : AbstractValidator<CityToUpdateDTO>
    {
        public CityToUpdateValidator()
        {
            RuleFor(location => location.Name)
            .NotEmpty().WithMessage("City name cannot be empty")
            .MaximumLength(50).WithMessage("City name cannot exceed 50 characters");

            RuleFor(m => m.Id).NotNull().WithMessage("Id cannot be null").NotEqual(0)
                .WithMessage("Id must be greater than 0 ");
        }
    }
}
