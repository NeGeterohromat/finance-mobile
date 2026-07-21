using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.Databases
{
    [Table("accounts")]
    public class Account
    {
        [Column("id")]
        public string Id { get; set; }  = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("balance")]
        public double Balance { get; set; } = 0; // Инициализация вместо DEFAULT
    }
}
