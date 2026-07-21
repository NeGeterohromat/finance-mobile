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
                var window = new AddEntryWindow(vm);
                if (TopLevel.GetTopLevel(this) is Window owner)
                    window.ShowDialog(owner);
                else
                    window.Show();
            };

            AddCategoryButton.Click += (_, _) =>
            {
                var window = new CreateCategoryWindow(vm);
                if (TopLevel.GetTopLevel(this) is Window owner)
                    window.ShowDialog(owner);
                else
                    window.Show();
            };
        }
    }
}