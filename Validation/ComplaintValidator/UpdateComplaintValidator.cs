using DTO.ComplaintDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.ComplaintValidator
{
    public class UpdateComplaintValidator :AbstractValidator<UpdateComplaintDto>
    {
        public UpdateComplaintValidator()
        {
            RuleFor(m => m.ComplaintText)
            .NotEmpty().WithMessage("Complaint text cannot be empty")
            .MaximumLength(500).WithMessage("Complaint text cannot exceed 500 characters");
            RuleFor(m => m.Id)
          .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
