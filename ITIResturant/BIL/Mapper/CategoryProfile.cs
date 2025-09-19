using Restaurant.BLL.ModelVM.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Entity → VM
            CreateMap<Category, CategoryVM>();

            // VM → Entity
            CreateMap<CategoryVM, Category>();
        }
    }
}
