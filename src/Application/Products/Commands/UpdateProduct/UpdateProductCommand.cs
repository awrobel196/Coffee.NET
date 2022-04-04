using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Products.Commands.DeleteProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<HttpResponseHelper>
    {
        public Guid Id { get; set; }
        public int? Quantity { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, HttpResponseHelper>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<HttpResponseHelper> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Product.Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                string msg = $"Product with id {request.Id} not found";
                Log.Warning(msg);
                return new HttpResponseHelper(HttpStatusCode.NotFound, msg);
            }

          
            product.Quantity = request.Quantity ?? product.Quantity;
            product.Description = request.Description ?? request.Description;

            if (await _context.SaveChangesAsync(cancellationToken) < 1)
            {
                string msg = $"The given values already exist in the database. The database has not been changed";
                Log.Warning(msg);
                return new HttpResponseHelper(HttpStatusCode.OK, msg);
            }

            return new HttpResponseHelper(HttpStatusCode.OK);
        }
    }
}
