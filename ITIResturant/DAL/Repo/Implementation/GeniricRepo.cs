using Microsoft.EntityFrameworkCore;

using Rsturant.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rsturant.DAL.Repo.Impelementation
{
    ////public class GeniricRepo<T>:IGeniricRepo<T> where T : class
    ////{
    ////    // ********** Generic Repo for Crude Opertation to Less the code ************
    ////    private ResturantDbContext Db;
    ////    public GeniricRepo(ResturantDbContext db)
    ////    {
    ////        this.Db = db;
    ////    }

    ////    public async Task <int> Add(T item)
    ////    {
    ////        Db.Set<T>().Add(item);
    ////        return await Db.SaveChangesAsync();
    ////    }
    ////    public async Task<int> Delete(T item)
    ////    {
    ////        Db.Set<T>().Remove(item);
    ////        return await  Db.SaveChangesAsync() ;
    ////    }
    ////    public async Task<int> Update(T item) 
    ////    { 
    ////        Db.Set<T>().Update(item);
    ////        return await  Db.SaveChangesAsync();

    ////    }
    ////    public async Task <T> Get(int? id)
    ////    {
    ////      return await Db.Set<T>().FindAsync(id);
    ////    }
    ////    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    ////    {
    ////        IQueryable<T> query = Db.Set<T>();

    ////        if (filter != null)
    ////        {
    ////            query = query.Where(filter);
    ////        }

    ////        return await query.ToListAsync();
    ////    }

    ////    //public async Task< HashSet<T>> GetAll()
    ////    //{   
    ////    //    return  await Db.Set<T>().ToHashSetAsync<T>();
    ////    //}

    ////}
    ///
    public class GeniricRepo<T> :IGenericRepo<T> where T : class
    {
        protected readonly RestaurantDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GeniricRepo(RestaurantDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task<int> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await SaveAsync(); // Id هيتملأ بعد SaveAsync()
        }

        public  async Task< int >UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await SaveAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
            return await SaveAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
