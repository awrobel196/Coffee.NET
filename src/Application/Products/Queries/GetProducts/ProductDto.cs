using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Queries.GetProducts
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int? Number { get; set; }
        public int? Quantity { get; set; }
        public string? Description { get; set; } = String.Empty;
        public decimal Price { get; set; }
    }
}
