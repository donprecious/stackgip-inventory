using StackgipInventory.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Entities
{
    public class ProductInventory:Entity
    {
        public int ProductId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal AvailableUnit { get; set; }
    }
}
