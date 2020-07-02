using StackgipInventory.Data;
using StackgipInventory.Entities;
using StackgipInventory.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Repository
{
    public class OrderLogRepository : GenericRepository<OrderLog>,IOrderLogRepository
    {
        public OrderLogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
