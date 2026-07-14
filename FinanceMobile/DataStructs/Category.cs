using FinanceMobile.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceMobile.DataStructs
{
    public class Category
    {
        public string Name { get; private set; }
        private SortedOperationList operations;
        public Category(string name)
        {
            Name = name;
            operations = new();
        }

        public void AddOperation(Operation op)
        {
            operations.Add(op);
        }

        public IEnumerable<Operation> GetOperations(DateTime? dateStart = null, DateTime? dateEnd = null)
        { 
            foreach (var op in operations.Get(dateStart, dateEnd))
                yield return op;
        }
    }
}
