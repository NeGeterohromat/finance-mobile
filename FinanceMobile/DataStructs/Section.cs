using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FinanceMobile.DataStructs
{
    public class Section
    {
        public const string TotalsName = "totals";
        public string Name { get; private set; }

        public int WeeksCount { get; private set; }
        public DateTime WeeksStart { get; private set; }
        public int CategoriesCount { get { return categories.Count; } }
        private Dictionary<string,Category> categories;

        private Category totals;

        public Section(string name, int weeksCount, DateTime? weeksStart)
        {
            Name = name;
            categories = new();
            WeeksCount = weeksCount;
            if (weeksStart is null)
                WeeksStart = DateTime.Today;
            else
                WeeksStart = (DateTime)weeksStart;
            totals = new Category(weeksCount, TotalsName, WeeksStart);
        }

        public void AddCategory(string name) 
        {
            if (categories.ContainsKey(name)) throw new InvalidOperationException($"Category with name {name} already exists");
            categories[name] = new Category(WeeksCount, name, WeeksStart);
        }

        public Cell this[string name, Week week]
        {
            get 
            { 
                if (name == TotalsName) 
                    return totals[week];
                return categories[name][week];
            }
            set
            {
                if (!categories.TryGetValue(name, out var category)) throw new InvalidOperationException($"Editable Category with name {name} does not exists");
                
                var lastCell = category[week];
                // the category itself checks whether a week exists in the category
                category[week] = value;

                RecalculateTotals(week, value, lastCell);
            }
        }

        public IEnumerable<(string,Week,Cell)> GetCellsWithoutTotals()
        {
            foreach (var pair in categories) 
            {
                foreach (var cellPair in pair.Value.GetCells())
                {
                    yield return (pair.Key, cellPair.Item1, cellPair.Item2);
                }
            }
            /*
            foreach (var cellPair in totals.GetCells())
            {
                yield return (TotalsName,cellPair.Item1,cellPair.Item2);
            }
            */
        } 

        private void RecalculateTotals(Week week, Cell newCell, Cell lastCell)
        {
            totals[week] = new Cell() { Value = totals[week].Value + newCell.Value - lastCell.Value };
        }
    }
}
