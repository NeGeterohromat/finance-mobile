using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanceMobile.DataStructs;
using System;
using System.Collections.Generic;

namespace FinanceMobile.ViewModels
{
    public partial class CreateCategoryViewModel : ViewModelBase
    {
        private Dictionary<string, string> selectedTypeToSectionName = new()
        {
            {"Расход",Budget.ExpensesName },
            {"Доход",Budget.IncomesName }
        };

        [ObservableProperty]
        private string _selectedType = "Расход";

        [ObservableProperty]
        private string _categoryName = "";

        public event Action? RequestClose;

        [RelayCommand]
        private void SelectType(string type) => SelectedType = type;

        [RelayCommand]
        private void Create()
        {
            if (string.IsNullOrWhiteSpace(CategoryName)) return;
            App.AppBudget.AddCategory(selectedTypeToSectionName[SelectedType], CategoryName);
            App.NavigateTo(new BudgetViewModel());
        }

        [RelayCommand]
        private void Close() => App.NavigateTo(new BudgetViewModel());
    }
}