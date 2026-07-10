using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMobile.Databases
{
    [Table("settings")]
    public class Setting
    {
        [Column("key")]
        public string Key { get; set; }

        [Column("value")]
        public string Value { get; set; }
    }
}
