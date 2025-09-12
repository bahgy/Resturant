using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.DataBase
{
    public class ResturantDbContextFactory : IDesignTimeDbContextFactory<ResturantDbContext>
    {
        public ResturantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResturantDbContext>();
            optionsBuilder.UseSqlServer("Data Source=HOSSAM05;Initial Catalog=ResturantDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

            return new ResturantDbContext(optionsBuilder.Options);
        }
    }
}
