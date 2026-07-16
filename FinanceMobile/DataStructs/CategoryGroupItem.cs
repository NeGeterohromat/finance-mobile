using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace FinanceMobile.DataStructs
{
    // Одна подкатегория внутри группы (например, "Зарплата" внутри "Доходы")
    public class SubcategoryItem
    {
        public string Name { get; set; } = "";
        public string Amount { get; set; } = "";
    }

    // Группа категорий верхнего уровня ("Доходы" или "Расходы") с возможностью раскрыть/свернуть
    public partial class CategoryGroupItem : ObservableObject
    {
        public string IconData { get; set; } = "";
        public string Title { get; set; } = "";
        public string TotalAmount { get; set; } = "";

        [ObservableProperty]
        private bool _isExpanded;

        public ObservableCollection<SubcategoryItem> Items { get; set; } = new();

        [RelayCommand]
        private void Toggle() => IsExpanded = !IsExpanded;
    }
}