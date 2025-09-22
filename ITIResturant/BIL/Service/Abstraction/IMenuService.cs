using Restaurant.BLL.ModelVM.MenuVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Abstraction
{
    public interface IMenuService
    {
        Task<List<MenuVM>> GetMenuAsync();
    }
}
