
namespace Restaurant.DAL.Repo.Abstraction
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
