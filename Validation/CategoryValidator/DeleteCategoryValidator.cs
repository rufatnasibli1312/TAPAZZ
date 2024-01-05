﻿using DTO.CategoryDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.CategoryValidator
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryDTO>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(m => m.Id)
               .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0");
        }
    }
}
