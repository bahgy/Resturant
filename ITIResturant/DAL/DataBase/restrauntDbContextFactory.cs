using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.DataBase
{
    public class ResturantDbContextFactory : IDesignTimeDbContextFactory<ResturantDbContext>
    {
        public ResturantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResturantDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-VCK5CED;Database=ResturantDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");

            return new ResturantDbContext(optionsBuilder.Options);
        }
    }
}
