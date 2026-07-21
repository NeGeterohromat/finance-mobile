using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.Databases
{
    [Table("categories")]
    public class Category
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("color")]
        public string Color { get; set; }
    }
}
