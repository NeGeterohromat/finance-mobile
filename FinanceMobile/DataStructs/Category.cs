using FinanceMobile.Databases;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.DataStructs
{
    public class Category
    {
        public string Name { get; private set; }
        private Dictionary<Week, Cell> cells;
        private Dictionary<Week, List<Operation>> operations;
        public Category(int cellCount, string name, DateTime? weekStart = null)
        {
            Name = name;
            cells = new();
            operations = new();
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
        }

        public Week AddOperation(Operation op, Week? guessedWeek = null)
        {
            if (guessedWeek is null)
            {
                foreach (var week in cells.Keys)
                    if (AddOperationAtWeekIfPossible(op, week))
                        return week;
            }
            else if (AddOperationAtWeekIfPossible(op, (Week)guessedWeek))
                return (Week)guessedWeek;
            
            throw new InvalidOperationException($"Operation date {op.Date} is out of table");
        }

        private bool AddOperationAtWeekIfPossible(Operation op, Week week)
        {
            if (week.StartDay <= op.Date && op.Date < week.EndDay)
            {
                if (!operations.ContainsKey(week))
                    operations[week] = new();
                operations[week].Add(op);
                return true;
            }
            return false;
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
