using DTO.CategoryDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.CategoryValidator
{
    public class CategoryToAddValidator :AbstractValidator<CategoryToAddDto>
    {
        public CategoryToAddValidator()
        {
            RuleFor(m => m.Name)
           .NotEmpty().WithMessage("Category name cannot be empty")
           .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");

            RuleFor(m => m.ParentCategoryId).NotNull().WithMessage("ParentCategoryId cannot null").NotEqual(0)
                .WithMessage("ParentCategoryId must be greater than 0 ");
        }
    }
}
