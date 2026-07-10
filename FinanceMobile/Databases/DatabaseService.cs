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
            // Выполняем ваш точный SQL-скрипт
            _db.Execute(@"CREATE TABLE IF NOT EXISTS accounts (
                id      TEXT PRIMARY KEY,
                name    TEXT NOT NULL,
                type    TEXT NOT NULL,
                balance REAL NOT NULL DEFAULT 0
            );

            CREATE TABLE IF NOT EXISTS categories (
                id    TEXT PRIMARY KEY,
                name  TEXT NOT NULL,
                type  TEXT NOT NULL,
                color TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS operations (
                id          TEXT PRIMARY KEY,
                date        TEXT NOT NULL,
                type        TEXT NOT NULL,
                status      TEXT NOT NULL,
                category_id TEXT NOT NULL REFERENCES categories(id),
                account_id  TEXT NOT NULL REFERENCES accounts(id),
                amount      REAL NOT NULL,
                description TEXT NOT NULL DEFAULT ''
            );

            CREATE INDEX IF NOT EXISTS idx_operations_date ON operations(date);
            CREATE INDEX IF NOT EXISTS idx_operations_status ON operations(status);

            CREATE TABLE IF NOT EXISTS week_plans (
                week_start     TEXT PRIMARY KEY,
                income_plan    REAL NOT NULL DEFAULT 0,
                expense_plan   REAL NOT NULL DEFAULT 0,
                income_actual  REAL NOT NULL DEFAULT 0,
                expense_actual REAL NOT NULL DEFAULT 0
            );

            CREATE TABLE IF NOT EXISTS settings (
                key   TEXT PRIMARY KEY,
                value TEXT NOT NULL
            );");
        }

        // Сохранить все ячейки (удаляет старые и записывает новые)
        /*
        public void SaveAllCells(List<Section> sections)
        {
            _db.RunInTransaction(() =>
            {
                _db.DeleteAll<CellRecord>(); // Очищаем таблицу

                foreach (var sect in sections)
                {
                    foreach (var cellData in sect.GetCellsWithoutTotals())
                    {
                        _db.Insert(new CellRecord
                        {
                            SectionName = sect.Name,
                            CategoryName = cellData.Item1,
                            WeekStart = cellData.Item2.StartDay,
                            Value = cellData.Item3.Value
                        });
                    }
                }
            });
        }
        */
        

        // Загрузить все ячейки
        /*
        public Dictionary<string,Dictionary<string,Dictionary<DateTime,double>>> LoadAllCells()
        {
            var records = _db.Table<CellRecord>().ToList();
            var sections = new Dictionary<string, Dictionary<string, Dictionary<DateTime, double>>>();
            foreach (var record in records)
            {
                if (!sections.ContainsKey(record.SectionName)) sections[record.SectionName] = new();

                if (!sections[record.SectionName].ContainsKey(record.CategoryName)) sections[record.SectionName][record.CategoryName] = new();

                sections[record.SectionName][record.CategoryName][record.WeekStart] = record.Value;
            }
            return sections;
        }
        */
    }
}
