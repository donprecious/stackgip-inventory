using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Dto.ProductInventory
{
    public class GetProductInventoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal AvailableUnit { get; set; }
    }
}
