using Avalonia.Controls;
using FinanceMobile.ViewModels;

namespace FinanceMobile.Views
{
    public partial class AddEntryWindow : Window
    {
        public AddEntryWindow()
        {
            InitializeComponent();
            var vm = new AddEntryViewModel();
            vm.RequestClose += () => Close();
            DataContext = vm;
        }
    }
}