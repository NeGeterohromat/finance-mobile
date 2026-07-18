using System;

namespace FinanceMobile.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public FinancialCategory Category { get; set; } // Привязанная статья
        public string PeriodId { get; set; } // Идентификатор периода (недели)
        public decimal Amount { get; set; } // Сумма
        public string? Comment { get; set; } // Необязательное пояснение
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Действие для депозита ("Пополнить"/"Снять") или займа ("Получить"/"Погасить")
        public string? OperationType { get; set; } 

        public Transaction(FinancialCategory category, string periodId, decimal amount, string? operationType = null, string? comment = null)
        {
            Category = category;
            PeriodId = periodId;
            Amount = amount;
            OperationType = operationType;
            Comment = comment;
        }
    }
}