using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using FinanceMobile.ViewModels;

namespace FinanceMobile.Views
{
    public partial class AddEntryWindow : UserControl
    {
        public AddEntryWindow()
        {
            InitializeComponent();
            var vm = new AddEntryViewModel();
            //vm.RequestClose += () => Close();
            DataContext = vm;
        }
    }
}