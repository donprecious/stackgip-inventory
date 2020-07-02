﻿using StackgipInventory.Entities;
using StackgipInventory.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackgipInventory.Repository
{
    public interface ICustomerOrderRepository:IGenericRepository<CustomerOrder>
    {
        Task<IEnumerable<CustomerOrder>> Get(int userId);
    }
}