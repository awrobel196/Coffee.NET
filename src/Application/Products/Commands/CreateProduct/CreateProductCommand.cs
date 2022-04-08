using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name can not be null ")]
        [MaxLength(200, ErrorMessage = "The Name cannot be longer than 100 characters")]
        public string? Name { get; set; }
        public int? Number { get; set; }
        public int? Quantity { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description can not be null ")]
        [MaxLength(200, ErrorMessage = "The Description cannot be longer than 200 characters")]
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request);
            entity.Id = Guid.NewGuid();

            await _context.Product.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
