

namespace Restaurant.BLL.Service.Abstraction
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
