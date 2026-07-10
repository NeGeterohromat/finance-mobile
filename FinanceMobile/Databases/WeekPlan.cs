using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.Databases
{
    [Table("week_plans")]
    public class WeekPlan
    {
        [Column("week_start")]
        public DateTime WeekStart { get; set; }

        [Column("income_plan")]
        public double IncomePlan { get; set; } = 0;

        [Column("expense_plan")]
        public double ExpensePlan { get; set; } = 0;

        [Column("income_actual")]
        public double IncomeActual { get; set; } = 0;

        [Column("expense_actual")]
        public double ExpenseActual { get; set; } = 0;
    }
}
