using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Implementation
{
    public class DeliveryRepo: IDeliveryRepo
    {
        private readonly RestaurantDbContext _dbContext;
        public DeliveryRepo(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            return await _dbContext.Deliveries
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
