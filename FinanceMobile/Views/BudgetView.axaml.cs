using Avalonia.Controls;
using FinanceMobile.ViewModels;

namespace FinanceMobile.Views
{
    public partial class BudgetView : UserControl
    {
        public BudgetView()
        {
            InitializeComponent();
            DataContext = new BudgetViewModel();

            AddEntryButton.Click += (_, _) =>
            {
                App.NavigateTo(new AddEntryViewModel());
                /*
                var window = new AddEntryWindow();
                if (TopLevel.GetTopLevel(this) is Window owner)
                    window.ShowDialog(owner);
                else
                    window.Show();
                */
            };
        }
    }
}