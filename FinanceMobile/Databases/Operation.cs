using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.Databases
{
    [Table("operations")]
    public class Operation
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public string Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; } 

        [Column("type")]
        public string Type { get; set; }

        [Column("status")]
        public string Status { get; set; }

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
