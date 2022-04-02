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
            RuleFor(x => x.Name).NotNull().NotEmpty()
                .WithMessage("he Name cannot be null or empty")
                .MaximumLength(100).WithMessage("The Name cannot be longer than 100 characters");

            RuleFor(x => x.Price).NotNull()
                .WithMessage("The Price cannot be null or empty");

            RuleFor(x => x.Description).MaximumLength(200).
                WithMessage("The Name cannot be longer than 200 characters");
        }
    }
}
