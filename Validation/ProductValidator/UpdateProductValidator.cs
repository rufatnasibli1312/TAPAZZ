using DTO.ProductDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.ProductValidator
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(product => product.Id)
               .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Product name cannot be empty")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

            RuleFor(product => product.Description)
                .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters");

            RuleFor(product => product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(product => product.LocationId)
                .GreaterThan(0).WithMessage("LocationId must be greater than 0");

            RuleFor(product => product.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0");

            RuleFor(product => product.newPhotos)
                .Must(photos => photos != null && photos.Count > 0).WithMessage("At least one new photo is required");
        }
    }
}
