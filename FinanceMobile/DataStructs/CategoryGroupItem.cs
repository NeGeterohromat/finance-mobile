using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace FinanceMobile.DataStructs
{
    public static class AmountFormatter
    {
        public static string Format(decimal amount) => $"{amount:N0}".Replace(",", " ") + " ₽";
    }

    // Одна подкатегория внутри группы (например, "Зарплата" внутри "Доходы")
    public partial class SubcategoryItem : ObservableObject
    {
        public string Name { get; set; } = "";

        // Реальные числа, из которых считается то, что видно на экране
        public decimal PlannedAmount { get; set; }
        public decimal ActualAmount { get; set; }

        [ObservableProperty]
        private string _amount = "—";

        // Пересчитывает отображаемый текст под текущий режим (План/Факт/Сравнение)
        public void Refresh(string mode)
        {
            Amount = mode switch
            {
                "План" => PlannedAmount == 0 ? "—" : AmountFormatter.Format(PlannedAmount),
                "Факт" => ActualAmount == 0 ? "—" : AmountFormatter.Format(ActualAmount),
                _ => $"{(PlannedAmount == 0 ? "—" : AmountFormatter.Format(PlannedAmount))} / {(ActualAmount == 0 ? "—" : AmountFormatter.Format(ActualAmount))}"
            };
        }
    }

    // Группа категорий верхнего уровня ("Доходы" или "Расходы") с возможностью раскрыть/свернуть
    public partial class CategoryGroupItem : ObservableObject
    {
        public string IconData { get; set; } = "";
        public string Title { get; set; } = "";

        public decimal PlannedAmount { get; set; }
        public decimal ActualAmount { get; set; }

        [ObservableProperty]
        private string _totalAmount = "—";

        [ObservableProperty]
        private bool _isExpanded;

        public ObservableCollection<SubcategoryItem> Items { get; set; } = new();

        [RelayCommand]
        private void Toggle() => IsExpanded = !IsExpanded;

        public void Refresh(string mode)
        {
            TotalAmount = mode switch
            {
                "План" => AmountFormatter.Format(PlannedAmount),
                "Факт" => AmountFormatter.Format(ActualAmount),
                _ => $"{AmountFormatter.Format(PlannedAmount)} / {AmountFormatter.Format(ActualAmount)}"
            };

            foreach (var item in Items)
                item.Refresh(mode);
        }
    }
}