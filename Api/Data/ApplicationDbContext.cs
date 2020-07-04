using Microsoft.EntityFrameworkCore;
using StackgipInventory.Entities;

namespace StackgipInventory.Data
{
    public class ApplicationDbContext :  DbContext
    {
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<OrderLog> OrderLogs { get; set; }
        public virtual DbSet<ProductInventory> ProductInventories { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {  }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

      
    }
}
