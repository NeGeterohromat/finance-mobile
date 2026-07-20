using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanceMobile.DataStructs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinanceMobile.ViewModels
{
    public partial class AddEntryViewModel : ViewModelBase
    {
        private Dictionary<string, string> selectedTypeToSectionName = new()
        {
            {"Расход",Budget.ExpensesName },
            {"Доход",Budget.IncomesName }
        };
        private Dictionary<string, int> daysInDatePeriod = new()
        {
            { "Не повторять", -1},
            {"Ежедневно", 1},
            {"Еженедельно", 7},
            {"Ежемесячно", 31 }
        };

        public ObservableCollection<string> DatePeriods { get; } = new()
        {
            "Не повторять",
            "Ежедневно",
            "Еженедельно",
            "Ежемесячно"
        };

        [ObservableProperty]
        private string _selectedPeriod = "Не повторять";

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

        [ObservableProperty]
        private string _dateInISOFormat = "";

        // TODO: заменить на реальные счета/категории из FinanceMobile.Databases
        public ObservableCollection<string> AccountOptions { get; private set; } = new()
        {
            "Карта", "Наличные", "Депозит"
        };

        public ObservableCollection<string> CategoryOptions { get; private set; } = new();

        public bool IsTransfer => SelectedType == "Перевод";
        public bool IsNotTransfer => !IsTransfer;

        public event Action? RequestClose;

        public AddEntryViewModel()
        {
            UpdateCategoryOptions();
        }

        partial void OnSelectedTypeChanged(string value)
        {
            OnPropertyChanged(nameof(IsTransfer));
            OnPropertyChanged(nameof(IsNotTransfer));

            UpdateCategoryOptions();
        }

        private void UpdateCategoryOptions()
        {
            if (SelectedType == "Перевод") return;

            CategoryOptions.Clear();

            foreach(var name in App.AppBudget.GetCategoryNames(selectedTypeToSectionName[SelectedType]))
                CategoryOptions.Add(name);
        }

        [RelayCommand]
        private void SelectType(string type) => SelectedType = type;

        [RelayCommand]
        private void ToggleRecurring() => IsRecurring = !IsRecurring;

        [RelayCommand]
        private void Add()
        {
            // TODO: сохранить операцию в БД через FinanceMobile.Databases.Operation
            //RequestClose?.Invoke();

            var periodInDays = -1;
            var isPlanned = false;
            if (IsRecurring)
            {
                periodInDays = daysInDatePeriod[SelectedPeriod];
            }
            var sectionName = selectedTypeToSectionName[SelectedType];
            var categoryName = SelectedCategory;
            var value = double.Parse(Amount);
            var date = DateTime.Parse(DateInISOFormat);
            App.AppBudget.AddOperation(sectionName, categoryName, date, value, isPlanned, periodInDays);
            App.NavigateTo(new BudgetViewModel());
        }
        /*
        [RelayCommand]
        private void Close() => RequestClose?.Invoke();
        */

        [RelayCommand]
        private void Close()
        {
            App.NavigateTo(new BudgetViewModel());
        }
    }
}