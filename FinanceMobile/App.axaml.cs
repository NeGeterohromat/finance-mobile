using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FinanceMobile.DataStructs;
using FinanceMobile.ViewModels;
using FinanceMobile.Views;
using System;
using System.Linq;
using SQLitePCL;

namespace FinanceMobile
{
    public partial class App : Application
    {
        // Текущий отображаемый экран
        public static ViewModelBase CurrentPage { get; private set; }

        // Событие, чтобы главное окно знало, что экран изменился
        public static event Action PageChanged;

        public static DateTime UserInputDateTime { get; private set; }

        public static Budget AppBudget {  get; private set; }

        public override void Initialize()
        {
            AppBudget = Budget.BudgetInstance;

            // Пока захардкожено. Потом изменить.
            UserInputDateTime = DateTime.Now;

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // для первоначальной записи данных в бд. Если бд существует на устройстве,
            // то эти данные оттуда возьмутся сами и этот код нужно закомментировать.
            //App.AppBudget.AddCategory(Budget.IncomesName, "Зарплата");
            //App.AppBudget.AddCategory(Budget.IncomesName, "Фриланс");
            //App.AppBudget.AddCategory(Budget.IncomesName, "Инвестиции");
            //App.AppBudget.AddCategory(Budget.ExpensesName, "Еда");
            //App.AppBudget.AddCategory(Budget.ExpensesName, "Аренда");
            //App.AppBudget.AddCategory(Budget.ExpensesName, "Образование");
            //App.AppBudget.AddCategory(Budget.ExpensesName, "Кредит");
            //App.AppBudget.AddOperation(Budget.IncomesName, "Зарплата", DateTime.Now, 20000);
            //App.AppBudget.AddOperation(Budget.ExpensesName, "Еда", DateTime.Now, 8000);
            // до сюда

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    Content = new Views.BudgetView(),
                    Width = 400,
                    Height = 800
                };
            }/*
            else if (ApplicationLifetime is IActivityApplicationLifetime singleViewFactoryApplicationLifetime)
            {
                singleViewFactoryApplicationLifetime.MainViewFactory = () => new MainView 
                { 
                    Content = new Views.BudgetView(), 
                    DataContext = new Views.BudgetView() 
                };
            }*/
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView();

                NavigateTo(new BudgetViewModel());
            }

            base.OnFrameworkInitializationCompleted();
        }

        public static void NavigateTo(ViewModelBase viewModel)
        {
            CurrentPage = viewModel;
            PageChanged?.Invoke();
        }

        public static void AddDaysToUserDate(int days)
        {
            UserInputDateTime = UserInputDateTime.AddDays(days);
        }
    }
}