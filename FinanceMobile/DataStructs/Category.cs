using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.DataStructs
{
    public class Category
    {
        public string Name { get; private set; }
        private Dictionary<Week, Cell> cells;
        public Category(int cellCount, string name, DateTime? weekStart = null)
        {
            Name = name;
            cells = new();
            // If default not set, default is today
            if (weekStart is null)
                weekStart = DateTime.Today;
            for (int i = 0;  i < cellCount; i++) 
            {
                cells[new Week() { StartDay = (DateTime)weekStart, EndDay = ((DateTime)weekStart).AddDays(6) }] = new Cell();
                weekStart = ((DateTime)weekStart).AddDays(7);
            }
        }

        public Cell this[Week week]
        {
            get {  return cells[week]; }
            set { 
                if (cells.ContainsKey(week)) 
                    cells[week] = value;
                else
                    throw new InvalidOperationException($"Week does not exists in Category {Name}");
            }
        }

        public IEnumerable<(Week,Cell)> GetCells()
        {
            foreach (var pair in cells)
            {
                yield return (pair.Key, pair.Value);
            }
        }
    }
}
