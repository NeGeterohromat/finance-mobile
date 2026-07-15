using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.Databases
{
    [Table("recurring_rules")]
    public class PeriodicOperation
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public string Id { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("interval_days")]
        public int IntervalDays { get; set; }

        [Column("type")]
        public string Type { get; set; }
        /*
        [Column("status")]
        public string Status { get; set; }
        */
        [Column("category_id")]
        public string CategoryId { get; set; } // Связь работает только логически в C#

        [Column("account_id")]
        public string AccountId { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [Column("description")]
        public string Description { get; set; } = ""; // Инициализация вместо DEFAULT ''
    }
}
