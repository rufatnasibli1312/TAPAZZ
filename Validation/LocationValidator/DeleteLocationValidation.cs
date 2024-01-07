using DTO.LocationDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.LocationValidator
{
    public class DeleteLocationValidation : AbstractValidator<DeleteLocationDto>
    {
        public DeleteLocationValidation()
        {
            RuleFor(m => m.Id).NotNull().WithMessage("Id cannot be null").NotEqual(0)
               .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0");
        }
    }
}
