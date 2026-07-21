using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace FinanceMobile.ViewModels
{
    public partial class CreateCategoryViewModel : ViewModelBase
    {
        private readonly BudgetViewModel _budgetViewModel;

        [ObservableProperty]
        private string _selectedType = "Расход";

        [ObservableProperty]
        private string _categoryName = "";

        public event Action? RequestClose;

        public CreateCategoryViewModel(BudgetViewModel budgetViewModel)
        {
            _budgetViewModel = budgetViewModel;
        }

        [RelayCommand]
        private void SelectType(string type) => SelectedType = type;

        [RelayCommand]
        private void Create()
        {
            if (string.IsNullOrWhiteSpace(CategoryName)) return;
            _budgetViewModel.AddCategory(SelectedType, CategoryName.Trim());
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void Close() => RequestClose?.Invoke();
    }
}