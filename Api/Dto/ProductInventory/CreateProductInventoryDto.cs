using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Dto.ProductInventory
{
    public class CreateProductInventoryDto
    {
        [Required]
        public int ProductId { get; set; }
        public decimal Min { get; set; } = 10;
        public decimal Max { get; set; } = 1000;
        public decimal AvailableUnit { get; set; }
    }
}
