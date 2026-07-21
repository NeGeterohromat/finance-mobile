using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanceMobile.DataStructs;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace FinanceMobile.ViewModels
{
    public partial class BudgetViewModel : ViewModelBase
    {
        private static readonly CultureInfo Ru = new("ru-RU");

        private DateTime _periodAnchor = new(2026, 7, 9);

        // Плановые и фактические суммы по группам (для сводных карточек сверху)
        private decimal _incomePlanned = 20000;
        private decimal _incomeActual;
        private decimal _expensePlanned = 8000;
        private decimal _expenseActual;

        [ObservableProperty]
        private string _selectedPeriod = "Неделя";

        [ObservableProperty]
        private string _selectedMode = "План";

        [ObservableProperty]
        private string _dateRangeText = "";

        [ObservableProperty]
        private string _incomeTotal = "";

        [ObservableProperty]
        private string _expenseTotal = "";

        [ObservableProperty]
        private string _balanceTotal = "";

        public ObservableCollection<CategoryGroupItem> Categories { get; } = new();

        public BudgetViewModel()
        {
            // TODO: сейчас данные захардкожены для вёрстки.
            // Позже заменить на реальную загрузку через FinanceMobile.Databases.Category / Operation
            Categories.Add(new CategoryGroupItem
            {
                IconData = "M2,5H22V20H2V5M20,18V7H4V18H20M17,12A2,2 0 0,0 15,10H10V15H15A2,2 0 0,0 17,13V12Z",
                Title = "Доходы",
                PlannedAmount = 20000,
                Items =
                {
                    new SubcategoryItem { Name = "Зарплата", PlannedAmount = 20000 },
                    new SubcategoryItem { Name = "Фриланс", PlannedAmount = 0 },
                    new SubcategoryItem { Name = "Инвестиции", PlannedAmount = 0 },
                }
            });

            Categories.Add(new CategoryGroupItem
            {
                IconData = "M17,18A2,2 0 0,1 19,20A2,2 0 0,1 17,22A2,2 0 0,1 15,20A2,2 0 0,1 17,18M7,18A2,2 0 0,1 9,20A2,2 0 0,1 7,22A2,2 0 0,1 5,20A2,2 0 0,1 7,18M7.17,14.75L7.2,14.63L8.1,13H15.55C16.3,13 16.96,12.59 17.3,11.97L21.16,4.96L19.42,4H19.41L18.31,6L15.55,11H8.53L4.27,2H1V4H3L6.6,11.59L5.25,14.04C5.09,14.35 5,14.67 5,15A2,2 0 0,0 7,17H19V15H7.42C7.29,15 7.17,14.89 7.17,14.75Z",
                Title = "Расходы",
                PlannedAmount = 8000,
                Items =
                {
                    new SubcategoryItem { Name = "Еда", PlannedAmount = 8000 },
                    new SubcategoryItem { Name = "Аренда", PlannedAmount = 0 },
                    new SubcategoryItem { Name = "Образование", PlannedAmount = 0 },
                    new SubcategoryItem { Name = "Кредит", PlannedAmount = 0 },
                }
            });

            UpdateDateRangeText();
            RefreshAll();
        }

        partial void OnSelectedPeriodChanged(string value) => UpdateDateRangeText();
        partial void OnSelectedModeChanged(string value) => RefreshAll();

        [RelayCommand]
        private void SelectPeriod(string period) => SelectedPeriod = period;

        [RelayCommand]
        private void SelectMode(string mode) => SelectedMode = mode;

        [RelayCommand]
        private void PreviousPeriod()
        {
            _periodAnchor = SelectedPeriod switch
            {
                "Неделя" => _periodAnchor.AddDays(-7),
                "Месяц" => _periodAnchor.AddMonths(-1),
                "Квартал" => _periodAnchor.AddMonths(-3),
                "Год" => _periodAnchor.AddYears(-1),
                _ => _periodAnchor
            };
            UpdateDateRangeText();
            // TODO: перезагрузить Categories/суммы под новый диапазон дат
        }

        [RelayCommand]
        private void NextPeriod()
        {
            _periodAnchor = SelectedPeriod switch
            {
                "Неделя" => _periodAnchor.AddDays(7),
                "Месяц" => _periodAnchor.AddMonths(1),
                "Квартал" => _periodAnchor.AddMonths(3),
                "Год" => _periodAnchor.AddYears(1),
                _ => _periodAnchor
            };
            UpdateDateRangeText();
            // TODO: перезагрузить Categories/суммы под новый диапазон дат
        }

        private void UpdateDateRangeText()
        {
            switch (SelectedPeriod)
            {
                case "Неделя":
                    var weekEnd = _periodAnchor.AddDays(6);
                    DateRangeText = _periodAnchor.Month == weekEnd.Month
                        ? $"{_periodAnchor:dd} - {weekEnd:dd MMMM}"
                        : $"{_periodAnchor:dd MMMM} - {weekEnd:dd MMMM}";
                    break;

                case "Месяц":
                    DateRangeText = Capitalize(_periodAnchor.ToString("MMMM yyyy", Ru));
                    break;

                case "Квартал":
                    int quarterStartMonth = ((_periodAnchor.Month - 1) / 3) * 3 + 1;
                    var qStart = new DateTime(_periodAnchor.Year, quarterStartMonth, 1);
                    var qEnd = qStart.AddMonths(3).AddDays(-1);
                    DateRangeText = $"{Capitalize(qStart.ToString("MMMM", Ru))} - {Capitalize(qEnd.ToString("MMMM yyyy", Ru))}";
                    break;

                case "Год":
                    DateRangeText = _periodAnchor.Year.ToString();
                    break;
            }
        }

        private static string Capitalize(string s)
            => string.IsNullOrEmpty(s) ? s : char.ToUpper(s[0], Ru) + s[1..];

        // Вызывается из окна "Создать категорию"
        public void AddCategory(string type, string categoryName)
        {
            string groupTitle = type == "Доход" ? "Доходы" : "Расходы";
            var group = Categories.FirstOrDefault(c => c.Title == groupTitle);
            if (group is null) return;

            if (group.Items.Any(i => i.Name == categoryName)) return;

            group.Items.Add(new SubcategoryItem { Name = categoryName, PlannedAmount = 0, ActualAmount = 0 });
            group.IsExpanded = true;
            RefreshAll();
        }

        // Вызывается из окна "Добавить запись" — добавляет к ФАКТУ, план не трогает
        public void AddOperation(string type, string amountText, string categoryName, string comment)
        {
            if (!decimal.TryParse(amountText.Replace(" ", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                return; // TODO: показать ошибку в форме

            string? groupTitle = type switch
            {
                "Доход" => "Доходы",
                "Расход" => "Расходы",
                _ => null // "Перевод" — TODO: отдельная логика для переводов между счетами
            };
            if (groupTitle is null) return;

            var group = Categories.FirstOrDefault(c => c.Title == groupTitle);
            if (group is null) return;

            var existing = group.Items.FirstOrDefault(i => i.Name == categoryName);
            if (existing is null)
            {
                existing = new SubcategoryItem { Name = categoryName, PlannedAmount = 0 };
                group.Items.Add(existing);
            }
            existing.ActualAmount += amount;
            group.ActualAmount += amount;

            if (groupTitle == "Доходы")
                _incomeActual += amount;
            else
                _expenseActual += amount;

            RefreshAll();
        }

        private void RefreshAll()
        {
            foreach (var group in Categories)
                group.Refresh(SelectedMode);

            RefreshSummary();
        }

        private void RefreshSummary()
        {
            switch (SelectedMode)
            {
                case "План":
                    IncomeTotal = AmountFormatter.Format(_incomePlanned);
                    ExpenseTotal = AmountFormatter.Format(_expensePlanned);
                    BalanceTotal = AmountFormatter.Format(_incomePlanned - _expensePlanned);
                    break;

                case "Факт":
                    IncomeTotal = AmountFormatter.Format(_incomeActual);
                    ExpenseTotal = AmountFormatter.Format(_expenseActual);
                    BalanceTotal = AmountFormatter.Format(_incomeActual - _expenseActual);
                    break;

                default: // Сравнение
                    IncomeTotal = $"{AmountFormatter.Format(_incomePlanned)} / {AmountFormatter.Format(_incomeActual)}";
                    ExpenseTotal = $"{AmountFormatter.Format(_expensePlanned)} / {AmountFormatter.Format(_expenseActual)}";
                    BalanceTotal = $"{AmountFormatter.Format(_incomePlanned - _expensePlanned)} / {AmountFormatter.Format(_incomeActual - _expenseActual)}";
                    break;
            }
        }
    }
}