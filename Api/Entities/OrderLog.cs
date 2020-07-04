using DocumentFormat.OpenXml.Drawing.Charts;
using StackgipInventory.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Entities
{
    public class OrderLog:Entity
    {
        public int CustomerOrderId { get; set; }
        public CustomerOrder CustomerOrder { get; set; }
        public string Operation { get; set; }
    }
}
