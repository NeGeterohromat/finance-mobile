using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.DataStructs
{
    public class Deposite : Section
    {
        public const string Refill = "Refill";
        public const string Percents = "Percents";
        public const string WriteDowns = "WriteDowns";

        public Deposite(string name) : base(name)
        {
            AddCategory(Refill);
            AddCategory(Percents);
            AddCategory(WriteDowns);
        }

        // Счёт на депозите всегда считается с самого начала этого депозита.
        public double GetDepositeResult(DateTime? dateEnd = null, bool isPlanned = false)
        {
            return GetCategorySum(Refill, null, dateEnd, isPlanned) + GetCategorySum(Percents, null, dateEnd, isPlanned)
                - GetCategorySum(WriteDowns, null, dateEnd, isPlanned);
        }
    }
}
