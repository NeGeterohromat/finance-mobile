using Avalonia.Controls;
using FinanceMobile.ViewModels;

namespace FinanceMobile.Views
{
    public partial class BudgetView : UserControl
    {
        public BudgetView()
        {
            InitializeComponent();
            var vm = new BudgetViewModel();
            DataContext = vm;

            AddEntryButton.Click += (_, _) =>
            {
                App.NavigateTo(new AddEntryViewModel());
            };

            AddCategoryButton.Click += (_, _) =>
            {
                App.NavigateTo(new CreateCategoryViewModel());
            };
        }
    }
}