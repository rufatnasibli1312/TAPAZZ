using DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.ComplaintValidator
{
    public class DeleteComplaintValidator :AbstractValidator<DeleteComplaintDto>
    {
        public DeleteComplaintValidator()
        {
            RuleFor(m => m.Id).NotNull().WithMessage("Id cannot be null").NotEqual(0)
           .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
