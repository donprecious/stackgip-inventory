using DocumentFormat.OpenXml.Bibliography;
using StackgipInventory.Data;
using StackgipInventory.Entities;
using StackgipInventory.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Repository
{
    public class CustomerOrderRepository:GenericRepository<CustomerOrder>,ICustomerOrderRepository
    {
        public CustomerOrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<CustomerOrder>> Get(int userId)
        {
            return _dbContext.CustomerOrders.Where(a => a.CustomerId == userId && a.IsDeleted != true).ToList();
        }
    }
}
