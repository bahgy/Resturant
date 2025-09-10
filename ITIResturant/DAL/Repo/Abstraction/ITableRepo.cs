using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.Abstraction
{
    public interface ITableRepo
    {
        bool Create(Table table);
        bool Edit(int tableId, Table newTable);
        bool Delete(int tableId);
        List<Table> GetAll();
        Table GetById(int tableId);
    }
}
