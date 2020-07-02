using StackgipInventory.Entities;
using StackgipInventory.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Repository
{
    public interface IProductInventoryRepository:IGenericRepository<ProductInventory>
    {
        Task<ProductInventory> Get(int id);
    }
}
