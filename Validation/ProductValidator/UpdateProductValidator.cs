using DAL.Data;
using DTO.ProductDto_s;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.ProductValidator
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {

        public MyDbContext _context { get; }

        public UpdateProductValidator(MyDbContext context)
        {
            _context = context;
            RuleFor(product => product.Id).NotEqual(0)
             .WithMessage("Id must be greater than 0");

            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Product name cannot be empty")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

            RuleFor(product => product.Description)
                .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters");

            RuleFor(product => product.Price).NotEqual(0)
               .WithMessage("Price must be greater than 0");

            RuleFor(product => product.LocationId)
                        .Must(BeValidLocationId).WithMessage("LocationId must be a valid entry in the database");

            RuleFor(product => product.CategoryId)
              .Must(BeValidCategoryId).WithMessage("CategoryId must be a valid entry in the database");

            RuleFor(product => product.newPhotos)
                .Must(photos => photos != null && photos.Count > 0).WithMessage("At least one new photo is required");

        }
        private bool BeValidLocationId(int locationId)
        {

            return _context.Locations.Any(l => l.Id == locationId);
        }

        private bool BeValidCategoryId(int categoryId)
        {

            return _context.Categories.Any(c => c.Id == categoryId);
        }
    }

}

