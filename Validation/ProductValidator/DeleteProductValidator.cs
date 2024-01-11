using DTO.ProductDto_s;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.ProductValidator
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductDto>
    {
        public DeleteProductValidator()
        {
            RuleFor(product => product.Id).NotEqual(0)
                .WithMessage("Id must be greater than 0 ");
        }
    }
}
