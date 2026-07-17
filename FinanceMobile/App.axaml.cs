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

namespace FinanceMobile
{
    public partial class App : Application
    {
        // Текущий отображаемый экран
        public static ViewModelBase CurrentPage { get; private set; }

        // Событие, чтобы главное окно знало, что экран изменился
        public static event Action PageChanged;

        public static Budget AppBudget {  get; private set; }

        public override void Initialize()
        {
            AppBudget = Budget.BudgetInstance;
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
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
    }
}