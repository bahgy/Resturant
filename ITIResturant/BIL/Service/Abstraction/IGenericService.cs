using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.BLL.Service.Abstraction
{
    public interface IGenericService <T> where T : class
    {
        public Task<T> Get(int id);
        public Task<List<T>> GetAll();
        public Task<int> Add(T item);
        public Task<int> Update(T item);
        public Task<int> Delete(int id);
    }
}
