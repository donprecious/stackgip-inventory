using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackgipInventory.Data;
using StackgipInventory.Entities;
using StackgipInventory.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Repository
{
    public class ProductInventoryRepository:GenericRepository<ProductInventory>,IProductInventoryRepository
    {
        public ProductInventoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<ProductInventory> Get(int productId)
        {
            return await _dbContext.ProductInventories
                .Where(a => a.ProductId == productId && a.IsDeleted != true).FirstOrDefaultAsync();
        }
    }
}
