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

            product.Quantity = request.Quantity;
            product.Description = request.Description;

            if (await _context.SaveChangesAsync(cancellationToken) < 1)
            {
                return new UpdateProductHelper(HttpStatusCode.InternalServerError, $"Error while save changes in database");
            }

            return new UpdateProductHelper(HttpStatusCode.OK, $"");
        }
    }
}
