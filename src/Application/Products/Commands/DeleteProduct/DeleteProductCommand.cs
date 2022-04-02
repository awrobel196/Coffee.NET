using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<DeleteProductHelper>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductHelper>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DeleteProductHelper> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Product.Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return new DeleteProductHelper(HttpStatusCode.NotFound, $"Product with id {request.Id} not found");
            }

            _context.Product.Remove(product);

            if (await _context.SaveChangesAsync(cancellationToken) < 1)
            {
                return new DeleteProductHelper(HttpStatusCode.InternalServerError, $"Error while save changes in database");
            }

            return new DeleteProductHelper(HttpStatusCode.OK, $"");
        }
    }
}
