using Microsoft.EntityFrameworkCore;

namespace StackgipInventory.Data
{
    public class ApplicationDbContext :  DbContext
    {




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {  }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
      
    }
}
