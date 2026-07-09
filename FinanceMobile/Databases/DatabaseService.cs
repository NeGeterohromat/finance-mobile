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

        public DatabaseService()
        {
            // Определяем путь к файлу БД. 
            // На Android это будет внутренняя папка приложения: /data/data/com.yourapp/files/
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "finance_app.db");

            // Открываем (или создаем) соединение
            _db = new SQLiteConnection(dbPath);

            // Создаем таблицу, если её еще нет
            _db.CreateTable<CellRecord>();
        }

        // Сохранить все ячейки (удаляет старые и записывает новые)
        public void SaveAllCells(List<Section> sections)
        {
            _db.RunInTransaction(() =>
            {
                _db.Table<CellRecord>().Delete(); // Очищаем таблицу

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

        // Загрузить все ячейки
        public Dictionary<string,Dictionary<string,(DateTime,double)>> LoadAllCells()
        {
            var records = _db.Table<CellRecord>().ToList();
            var sections = new Dictionary<string, Dictionary<string, (DateTime, double)>>();
            foreach (var record in records)
            {
                if (!sections.ContainsKey(record.SectionName)) sections[record.SectionName] = new();

                if (!sections[record.SectionName].ContainsKey(record.CategoryName)) sections[record.SectionName][record.CategoryName] = new();

                sections[record.SectionName][record.CategoryName] = (record.WeekStart,record.Value);
            }
            return sections;
        }
    }
}
