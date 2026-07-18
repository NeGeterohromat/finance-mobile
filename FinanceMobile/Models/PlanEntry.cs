using System;

namespace FinanceMobile.Models
{
    public class PlanEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public FinancialCategory Category { get; set; }
        public string PeriodId { get; set; } // К какой неделе относится план
        public decimal PlannedAmount { get; set; } // Ожидаемая сумма

        public PlanEntry(FinancialCategory category, string periodId, decimal plannedAmount)
        {
            Category = category;
            PeriodId = periodId;
            PlannedAmount = plannedAmount;
        }
    }
}