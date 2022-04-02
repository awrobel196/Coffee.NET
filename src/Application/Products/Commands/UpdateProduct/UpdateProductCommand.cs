using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Products.Commands.DeleteProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<UpdateProductHelper>
    {
        public Guid Id { get; set; }
        public int? Quantity { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductHelper>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UpdateProductHelper> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Product.Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return new UpdateProductHelper(HttpStatusCode.NotFound, $"Product with id {request.Id} not found");
            }

            //If no description or quantity was specified when the product was updated, the values will not be updated
            product.Quantity = request.Quantity ?? product.Quantity;
            product.Description = request.Description ?? request.Description;

            if (await _context.SaveChangesAsync(cancellationToken) < 1)
            {
                return new UpdateProductHelper(HttpStatusCode.OK, $"The given values already exist in the database. The database has not been changed");
            }

            return new UpdateProductHelper(HttpStatusCode.OK, $"");
        }
    }
}
