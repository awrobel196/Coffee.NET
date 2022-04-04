using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<HttpResponseHelper>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, HttpResponseHelper>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<HttpResponseHelper> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Product.Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                string msg = $"Try delete product with non exist id {request.Id}";
                Log.Warning(msg);
                return new HttpResponseHelper(HttpStatusCode.NotFound, msg);
            }

            _context.Product.Remove(product);

            if (await _context.SaveChangesAsync(cancellationToken) < 1)
            {
                string msg = "Error while save changes in database";
                Log.Warning(msg);
                return new HttpResponseHelper(HttpStatusCode.InternalServerError, msg);
            }

            return new HttpResponseHelper(HttpStatusCode.OK);
        }
    }
}
