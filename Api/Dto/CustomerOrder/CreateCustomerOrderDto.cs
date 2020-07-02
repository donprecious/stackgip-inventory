using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Dto.CustomerOrder
{
    public class CreateCustomerOrderDto
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal Unit { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
       // public string Status { get; set; }
    }
}
