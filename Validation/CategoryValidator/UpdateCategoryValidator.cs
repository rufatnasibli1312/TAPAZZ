using DTO.CategoryDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.CategoryValidator
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>

    {
        public UpdateCategoryValidator()
        {
            RuleFor(m => m.Name)
          .NotEmpty().WithMessage("Category name cannot be empty")
          .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");

            RuleFor(m => m.ParentCategoryId)
     .NotNull().WithMessage("ParentCategoryId must not be null")
     .NotEqual(0).WithMessage("ParentCategoryId must be greater than 0");

            RuleFor(m => m.Id).NotNull().WithMessage("CategoryId must not be null").NotEqual(0)
                .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0");
        }
    }
}
