using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using FluentValidation;

namespace Application.Common.Validation
{
    public class UpdateProductCommandValidation : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidation()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty()
                .WithMessage("he Name cannot be null or empty")
                .MaximumLength(200).WithMessage("The Description cannot be longer than 200 characters");
        }
    }
}
