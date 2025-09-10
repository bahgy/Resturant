using BIL.Model_VM.Table;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Service.Abstraction
{
    public interface ITableService
    {
        (bool, string) Create(CreateTableVM tableVM);
        (bool, string) Edit(int tableId, EditTableVM tableVM);
        bool Delete(int tableId);
        List<Table> GetAll();
        Table GetById(int tableId);
        List<Table> GetAllActiveTables();
    }
}
