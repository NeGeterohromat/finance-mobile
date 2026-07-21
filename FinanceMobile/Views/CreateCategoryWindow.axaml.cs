using Avalonia.Controls;
using FinanceMobile.ViewModels;

namespace FinanceMobile.Views
{
    public partial class CreateCategoryWindow : Window
    {
        public CreateCategoryWindow(BudgetViewModel budgetViewModel)
        {
            InitializeComponent();
            var vm = new CreateCategoryViewModel(budgetViewModel);
            vm.RequestClose += () => Close();
            DataContext = vm;
        }
    }
}