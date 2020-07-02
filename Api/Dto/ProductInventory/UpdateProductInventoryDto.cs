using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Dto.ProductInventory
{
    public class UpdateProductInventoryDto
    {
        public decimal Min { get; set; }            
        public decimal Max { get; set; }               
        public decimal AvailableUnit { get; set; }                   
    }
}
