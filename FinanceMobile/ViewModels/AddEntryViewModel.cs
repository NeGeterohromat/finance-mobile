using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinanceMobile.ViewModels
{
    public partial class AddEntryViewModel : ViewModelBase
    {
        private readonly BudgetViewModel? _budgetViewModel;

        [ObservableProperty]
        private string _selectedType = "Расход";

        [ObservableProperty]
        private string _amount = "";

        [ObservableProperty]
        private string _selectedAccount = "";

        [ObservableProperty]
        private string _selectedFromAccount = "";

        [ObservableProperty]
        private string _selectedToAccount = "";

        [ObservableProperty]
        private string _selectedCategory = "";

        [ObservableProperty]
        private string _comment = "";

        [ObservableProperty]
        private bool _isRecurring;

        public ObservableCollection<string> AccountOptions { get; } = new()
        {
            "Карта", "Наличные", "Депозит"
        };

        // Берём реальные категории из BudgetViewModel, а не хардкодим —
        // список зависит от того, выбран "Расход" или "Доход"
        public ObservableCollection<string> CategoryOptions
        {
            get
            {
                if (_budgetViewModel is null) return new ObservableCollection<string>();

                string groupTitle = SelectedType == "Доход" ? "Доходы" : "Расходы";
                var group = _budgetViewModel.Categories.FirstOrDefault(c => c.Title == groupTitle);
                var names = group?.Items.Select(i => i.Name) ?? Enumerable.Empty<string>();
                return new ObservableCollection<string>(names);
            }
        }

        public bool IsTransfer => SelectedType == "Перевод";
        public bool IsNotTransfer => !IsTransfer;

        public event Action? RequestClose;

        public AddEntryViewModel(BudgetViewModel? budgetViewModel = null)
        {
            _budgetViewModel = budgetViewModel;
        }

        partial void OnSelectedTypeChanged(string value)
        {
            OnPropertyChanged(nameof(IsTransfer));
            OnPropertyChanged(nameof(IsNotTransfer));
            OnPropertyChanged(nameof(CategoryOptions));
        }

        [RelayCommand]
        private void SelectType(string type) => SelectedType = type;

        [RelayCommand]
        private void ToggleRecurring() => IsRecurring = !IsRecurring;

        [RelayCommand]
        private void Add()
        {
            if (string.IsNullOrWhiteSpace(Amount)) return; // TODO: показать ошибку валидации в форме
            _budgetViewModel?.AddOperation(SelectedType, Amount, SelectedCategory, Comment);
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void Close() => RequestClose?.Invoke();
    }
}