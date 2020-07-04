using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Dto.CustomerOrder
{
    public class GetCustomerOrderDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Unit { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
    }
}
