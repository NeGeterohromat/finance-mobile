using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace FinanceMobile.ViewModels
{
    public partial class AddEntryViewModel : ViewModelBase
    {
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

        // TODO: заменить на реальные счета/категории из FinanceMobile.Databases
        public ObservableCollection<string> AccountOptions { get; } = new()
        {
            "Карта", "Наличные", "Депозит"
        };

        public ObservableCollection<string> CategoryOptions { get; } = new()
        {
            "Еда", "Здоровье", "Аренда", "Транспорт"
        };

        public bool IsTransfer => SelectedType == "Перевод";
        public bool IsNotTransfer => !IsTransfer;

        public event Action? RequestClose;

        partial void OnSelectedTypeChanged(string value)
        {
            OnPropertyChanged(nameof(IsTransfer));
            OnPropertyChanged(nameof(IsNotTransfer));
        }

        [RelayCommand]
        private void SelectType(string type) => SelectedType = type;

        [RelayCommand]
        private void ToggleRecurring() => IsRecurring = !IsRecurring;

        [RelayCommand]
        private void Add()
        {
            // TODO: сохранить операцию в БД через FinanceMobile.Databases.Operation
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void Close() => RequestClose?.Invoke();
    }
}