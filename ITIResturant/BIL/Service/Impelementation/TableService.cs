using Restaurant.BLL.Model_VM.Table;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Repo.Abstraction;
using Restaurant.DAL.Repo.Impelementation;


namespace Restaurant.BLL.Service.Impelementation
{
    public class TableService : ITableService
    {
        private readonly ITableRepo _tableRepo;
        public TableService(ITableRepo tableRepo)  
        {
            _tableRepo = tableRepo;
        }

        // إنشاء طاولة
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

        // تعديل الطاولة
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

        // حذف الطاولة
        public bool Delete(int tableId)
        {
            return _tableRepo.Delete(tableId);
        }

        // جلب كل الطاولات
        public List<Table> GetAll()
        {
            return _tableRepo.GetAll();
        }

        // جلب طاولة بالـ Id
        public Table GetById(int tableId)
        {
            return _tableRepo.GetById(tableId);
        }

        // جلب كل الطاولات النشطة فقط
        public List<Table> GetAllActiveTables()
        {
            return _tableRepo.GetAll().Where(t => t.IsActive).ToList();
        }
    }
}
