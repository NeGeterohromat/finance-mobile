using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.IO;
using System.Linq;
using FinanceMobile.DataStructs;

namespace FinanceMobile.Databases
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _db;

        //Singletone
        public static DatabaseService DatabaseServiceInstance { get; private set; } = new();

        private DatabaseService()
        {
            // Определяем путь к файлу БД. 
            // На Android это будет внутренняя папка приложения: /data/data/com.yourapp/files/
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "finance_app.db");

            // Открываем (или создаем) соединение
            _db = new SQLiteConnection(dbPath);

            // Создаем таблицы, если их еще нет
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // 1. Включаем поддержку внешних ключей (обязательно для REFERENCES)
            _db.Execute("PRAGMA foreign_keys = ON;");

            // 2. Создаем таблицы через ORM (каждый вызов - это отдельный SQL-запрос под капотом)
            _db.CreateTable<Account>();
            _db.CreateTable<Category>();
            _db.CreateTable<Operation>();
            _db.CreateTable<WeekPlan>();
            _db.CreateTable<Setting>();
            _db.CreateTable<PeriodicOperation>();

            // 3. Создаем индексы отдельными вызовами Execute
            _db.Execute("CREATE INDEX IF NOT EXISTS idx_operations_date ON operations(date);");
            _db.Execute("CREATE INDEX IF NOT EXISTS idx_operations_status ON operations(status);");

            var count = _db.ExecuteScalar<int>("SELECT COUNT(*) FROM categories;");
            var count2 = _db.Table<Category>().Count();
            var c = 454;
        }

        public void SaveOperation(Operation op)
        {
            _db.InsertOrReplace(op);
        }

        public void SavePeriodicOperation(PeriodicOperation op)
        {
            _db.InsertOrReplace(op);
        }

        public void SaveCategory(Category cat)
        {
            _db.InsertOrReplace(cat);
        }

        public string GetCategoryID(string name, string type)
        {
            return _db.Table<Category>()
              .Where(c => c.Name == name && c.Type == type)
              .FirstOrDefault()
              .Id;
        }

        public List<Operation> GetOperationList() => _db.Table<Operation>().ToList();

        public List<Category> GetCategoryList() => _db.Table<Category>().ToList();

        public List<PeriodicOperation> GetPeriodicoperationList() => _db.Table<PeriodicOperation>().ToList();
    }
}
