using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rsturant.DAL.Repo.Abstraction
{
    //public interface IGeniricRepo<T>
    //{
    //  public Task< T > Get(int? id);
    //    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);

    //    public Task< int> Add (T item);
    //  public Task< int> Update (T item);
    // public Task< int> Delete (T item);
    //}
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);

        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
        Task<int> SaveAsync();
    }

}
