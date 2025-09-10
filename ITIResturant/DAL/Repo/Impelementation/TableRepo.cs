using DAL.DataBase;
using DAL.Entities;
using DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.Impelementation
{
    public class TableRepo : ITableRepo
    {
        private readonly ResturantDbContext Db;
        public TableRepo(ResturantDbContext context)
        {
            Db = context;  
        }

        // إنشاء طاولة جديدة
        public bool Create(Table table)
        {
            try
            {
                var result = Db.Tables.Add(table);
                Db.SaveChanges();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // حذف طاولة
        public bool Delete(int tableId)
        {
            try
            {
                var table = Db.Tables.FirstOrDefault(t => t.Id == tableId);
                if (table == null) return false;

                Db.Tables.Remove(table);
                Db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // تعديل الطاولة
        public bool Edit(int tableId, Table newTable)
        {
            try
            {
                var oldTable = Db.Tables.FirstOrDefault(t => t.Id == tableId);
                if (oldTable == null) return false;

                oldTable.TableNumber = newTable.TableNumber;
                oldTable.Capacity = newTable.Capacity;
                oldTable.IsActive = newTable.IsActive;

                Db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // جلب كل الطاولات
        public List<Table> GetAll()
        {
            try
            {
                return Db.Tables.ToList();
            }
            catch
            {
                return new List<Table>();
            }
        }

        // جلب طاولة بالـ Id
        public Table GetById(int tableId)
        {
            try
            {
                return Db.Tables.FirstOrDefault(t => t.Id == tableId);
            }
            catch
            {
                return null;
            }
        }
    }
}
