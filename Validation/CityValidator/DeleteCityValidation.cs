using DTO.LocationDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.LocationValidator
{
    public class DeleteCityValidation : AbstractValidator<DeleteLocationDto>
    {
        public DeleteCityValidation()
        {
            RuleFor(m => m.Id).NotNull().WithMessage("Id cannot be null").NotEqual(0)
               .WithMessage("Id must be greater than 0 ");
        }
    }
}
