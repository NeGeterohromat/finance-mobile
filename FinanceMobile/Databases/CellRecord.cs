using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.Databases
{
    public class CellRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string SectionName { get; set; }
        public string CategoryName { get; set; }
        public DateTime WeekStart { get; set; }
        public double Value { get; set; }
    }
}
