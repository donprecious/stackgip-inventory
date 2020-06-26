using StackgipInventory.Data;

namespace StackgipInventory.Seeder
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        public Seed(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SeedPosts()
        {
       
        }

        public void Run()
        {
            //SeedSkills();
        }
    }
}
