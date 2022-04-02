using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Product
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var products = _mapper.Map<List<ProductDto>>(entity);
            return products;

        }
    }
}
