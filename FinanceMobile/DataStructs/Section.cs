using FinanceMobile.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace FinanceMobile.DataStructs
{
    public class Section
    {
        public const string TotalsName = "totals";
        public string Name { get; private set; }

        public int CategoriesCount { get { return categoriesActual.Count; } }
        private Dictionary<string,Category> categoriesActual;
        private Dictionary<string, Category> categoriesPlanned;

        public Section(string name)
        {
            Name = name;
            categoriesActual = new();
            categoriesPlanned = new();
        }

        public void AddCategory(string name) 
        {
            if (categoriesActual.ContainsKey(name)) throw new InvalidOperationException($"Category with name {name} already exists");
            
            categoriesActual[name] = new Category(name);
            categoriesPlanned[name] = new Category(name);
        }

        public void AddOperation(string categoryName, Operation op, bool isPlanned = false)
        {
            var categories = isPlanned ? categoriesPlanned : categoriesActual;
            if (!categories.TryGetValue(categoryName, out var cat)) throw new InvalidOperationException($"Editable Category with name {categoryName} does not exists");

            cat.AddOperation(op);
        }

        public IEnumerable<Operation> GetOperations(string categoryName, DateTime? dateStart = null, DateTime? dateEnd = null, bool isPlanned = false)
        {
            var categories = isPlanned ? categoriesPlanned : categoriesActual;
            return categories[categoryName].GetOperations(dateStart, dateEnd);
        }

        public double GetCategorySum(string categoryName, DateTime? dateStart = null, DateTime? dateEnd = null, bool isPlanned = false) =>
            GetOperations(categoryName,dateStart,dateEnd,isPlanned).Select(op=>op.Value).Sum();

        public double GetSum(DateTime? dateStart = null, DateTime? dateEnd = null, bool isPlanned = false) => 
            (isPlanned ? categoriesPlanned : categoriesActual).Values
            .SelectMany(c => c.GetOperations(dateStart, dateEnd))
            .Select(op => op.Value)
            .Sum();
    }
}
