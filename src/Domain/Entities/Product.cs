using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        [Key] public Guid Id { get; set; }
        [MaxLength(100)] public string Name { get; set; } = String.Empty;
        public int? Number { get; set; }
        public int? Quantity { get; set; }
        [MaxLength(200)] public string? Description { get; set; } = String.Empty;
        public decimal Price { get; set; }
    }
}
