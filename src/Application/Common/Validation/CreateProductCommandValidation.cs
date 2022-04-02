using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Products.Commands.CreateProduct;
using FluentValidation;

namespace Application.Common.Validation
{
    public class CreateProductCommandValidation : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidation()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.Description).MaximumLength(200);
        }
    }
}
