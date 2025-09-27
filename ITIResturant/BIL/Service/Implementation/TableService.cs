


namespace Restaurant.BLL.Service.Implementation
{
    public class TableService : ITableService
    {
        private readonly ITableRepo _tableRepo;
        public TableService(ITableRepo tableRepo)  
        {
            _tableRepo = tableRepo;
        }

        public (bool, string) Create(CreateTableVM tableVM)
        {
            try
            {
                var table = new Table
                {
                    TableNumber = tableVM.TableNumber,
                    Capacity = tableVM.Capacity,
                    IsActive = tableVM.IsActive
                };

                var result = _tableRepo.Create(table);
                return result
                    ? (false, "Table created successfully")
                    : (true, "Failed to create table");
            }
            catch (Exception ex)
            {
                return (true, ex.Message);
            }
        }

        public (bool, string) Edit(int tableId, EditTableVM tableVM)
        {
            try
            {
                var newTable = new Table
                {
                    TableNumber = tableVM.TableNumber,
                    Capacity = tableVM.Capacity,
                    IsActive = tableVM.IsActive
                };

                var result = _tableRepo.Edit(tableId, newTable);
                return result
                    ? (false, "Table updated successfully")
                    : (true, "Failed to edit table");
            }
            catch (Exception ex)
            {
                return (true, ex.Message);
            }
        }

        public bool Delete(int tableId)
        {
            return _tableRepo.Delete(tableId);
        }

        public List<Table> GetAll()
        {
            return _tableRepo.GetAll();
        }

        public Table GetById(int tableId)
        {
            return _tableRepo.GetById(tableId);
        }

        public List<Table> GetAllActiveTables()
        {
            return _tableRepo.GetAll().Where(t => t.IsActive).ToList();
        }
    }
}
