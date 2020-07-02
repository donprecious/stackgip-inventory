using StackgipInventory.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Entities
{
    public class CustomerOrder:Entity
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public decimal Unit { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
    }
}
