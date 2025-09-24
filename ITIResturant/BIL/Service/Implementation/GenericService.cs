using Microsoft.Identity.Client;
using Resturant.BLL.Service.Abstraction;
using Rsturant.DAL.Repo.Abstraction;
using Rsturant.DAL.Repo.Impelementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.BLL.Service.Impelementation
{
    public class GenericService<T>:IGenericService<T>where T : class
    { private IGenericRepo<T> _geniricRepo;
        private IProductRepo p_repo;

        public GenericService(IGenericRepo<T> geniricRepo) 
        {
        _geniricRepo = geniricRepo;
        }

        public GenericService(IProductRepo p_repo)
        {
            this.p_repo = p_repo;
        }

        public async Task<int> Add(T item)
        { 
           await _geniricRepo.AddAsync(item);
            return  await _geniricRepo.SaveAsync();
        }
        public async Task<int> Delete(int id)
        {
            await _geniricRepo.DeleteAsync(id);
            return await _geniricRepo.SaveAsync();
        }
        public async Task<int> Update(T item)
        { return await _geniricRepo.UpdateAsync(item);}
        
        public async Task<T> Get(int id)
        { var Entity=await _geniricRepo.GetByIdAsync(id);
            if (Entity == null)
                throw new Exception($"Entity With id = {id} Not Exissit");
            return Entity;
        }
        
        public async Task<List<T>> GetAll()
        {
            return await _geniricRepo.GetAllAsync();
        }
    }
}
