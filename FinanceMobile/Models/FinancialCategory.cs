using System;

namespace FinanceMobile.Models
{
    public class FinancialCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } // Например, "Еда", "Зарплата"
        public FinancialType Type { get; set; }
        public string? Comment { get; set; } // Опциональный комментарий

        // Опционально для займа: если долг уже существовал при создании
        public decimal? InitialLoanAmount { get; set; } 

        public FinancialCategory(string name, FinancialType type, decimal? initialLoanAmount = null, string? comment = null)
        {
            Name = name;
            Type = type;
            InitialLoanAmount = initialLoanAmount;
            Comment = comment;
        }
    }
}