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
        [ObservableProperty]
        private string _selectedPeriod = "Неделя";

        [ObservableProperty]
        private string _selectedMode = "План";

        [ObservableProperty]
        private string _dateRangeText = "09 - 16 Июля";

        [ObservableProperty]
        private string _incomeTotal = "20 000 ₽";

        [ObservableProperty]
        private string _expenseTotal = "8 000 ₽";

        [ObservableProperty]
        private string _balanceTotal = "12 000 ₽";

        public ObservableCollection<CategoryGroupItem> Categories { get; } = new();

        public BudgetViewModel()
        {
            // пока период выдачи захардкожен, выдаётся сумма всех операций.
            DateTime? start = null;
            DateTime? end = null;

            // для первоначальной записи данных в бд. Если бд существует на устройстве,
            // то эти данные оттуда возьмутся сами и этот код нужно закомментировать.
            App.AppBudget.AddCategory(Budget.IncomesName, "Зарплата");
            App.AppBudget.AddCategory(Budget.IncomesName, "Фриланс");
            App.AppBudget.AddCategory(Budget.IncomesName, "Инвестиции");
            App.AppBudget.AddCategory(Budget.ExpensesName, "Еда");
            App.AppBudget.AddCategory(Budget.ExpensesName, "Аренда");
            App.AppBudget.AddCategory(Budget.ExpensesName, "Образование");
            App.AppBudget.AddCategory(Budget.ExpensesName, "Кредит");
            App.AppBudget.AddOperation(Budget.IncomesName, "Зарплата", DateTime.Now, 20000);
            App.AppBudget.AddOperation(Budget.ExpensesName, "Еда", DateTime.Now, 8000);
            // до сюда

            Categories.Add(new CategoryGroupItem
            {
                IconData = "M2,5H22V20H2V5M20,18V7H4V18H20M17,12A2,2 0 0,0 15,10H10V15H15A2,2 0 0,0 17,13V12Z",
                Title = "Доходы",
                TotalAmount = App.AppBudget.GetSectionSum(Budget.IncomesName, start, end).ToString("C", new CultureInfo("ru-RU")),
                Items = new ObservableCollection<SubcategoryItem>(App.AppBudget.GetCategoryNames(Budget.IncomesName)
                    .Select(c => {
                        var categorySum = App.AppBudget.GetCategorySum(Budget.IncomesName, c, start, end);
                        var sumText = "-";
                        if (categorySum != 0) 
                            sumText = categorySum.ToString("C", new CultureInfo("ru-RU"));

                        return new SubcategoryItem
                        {
                            Name = c,
                            Amount = sumText
                        };
                    }))                    
                /*
                {
                    new SubcategoryItem { Name = "Зарплата", Amount = "20 000 ₽" },
                    new SubcategoryItem { Name = "Фриланс", Amount = "—" },
                    new SubcategoryItem { Name = "Инвестиции", Amount = "—" },
                }*/
            });

            Categories.Add(new CategoryGroupItem
            {
                IconData = "M17,18A2,2 0 0,1 19,20A2,2 0 0,1 17,22A2,2 0 0,1 15,20A2,2 0 0,1 17,18M7,18A2,2 0 0,1 9,20A2,2 0 0,1 7,22A2,2 0 0,1 5,20A2,2 0 0,1 7,18M7.17,14.75L7.2,14.63L8.1,13H15.55C16.3,13 16.96,12.59 17.3,11.97L21.16,4.96L19.42,4H19.41L18.31,6L15.55,11H8.53L4.27,2H1V4H3L6.6,11.59L5.25,14.04C5.09,14.35 5,14.67 5,15A2,2 0 0,0 7,17H19V15H7.42C7.29,15 7.17,14.89 7.17,14.75Z",
                Title = "Расходы",
                TotalAmount = App.AppBudget.GetSectionSum(Budget.ExpensesName, start, end).ToString("C", new CultureInfo("ru-RU")),
                Items = new ObservableCollection<SubcategoryItem>(App.AppBudget.GetCategoryNames(Budget.ExpensesName)
                    .Select(c => {
                        var categorySum = App.AppBudget.GetCategorySum(Budget.ExpensesName, c, start, end);
                        var sumText = "-";
                        if (categorySum != 0)
                            sumText = categorySum.ToString("C", new CultureInfo("ru-RU"));

                        return new SubcategoryItem
                        {
                            Name = c,
                            Amount = sumText
                        };
                    }))
                /*
                {
                    new SubcategoryItem { Name = "Еда", Amount = "8 000 ₽" },
                    new SubcategoryItem { Name = "Аренда", Amount = "—" },
                    new SubcategoryItem { Name = "Образование", Amount = "—" },
                    new SubcategoryItem { Name = "Кредит", Amount = "—" },
                }
                */
            });
        }

        [RelayCommand]
        private void SelectPeriod(string period) => SelectedPeriod = period;

        [RelayCommand]
        private void SelectMode(string mode) => SelectedMode = mode;

        [RelayCommand]
        private void PreviousPeriod()
        {
            // TODO: сдвиг диапазона дат назад в зависимости от SelectedPeriod
        }

        [RelayCommand]
        private void NextPeriod()
        {
            // TODO: сдвиг диапазона дат вперёд
        }

        [RelayCommand]
        private void AddCategory()
        {
            // TODO: открыть модалку "Создание категории" — сверстаем следующим шагом
        }
    }
}