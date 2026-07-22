using Avalonia.Controls;
using FinanceMobile.ViewModels;

namespace FinanceMobile.Views
{
    public partial class CreateCategoryWindow : UserControl
    {
        public CreateCategoryWindow()
        {
            InitializeComponent();
            var vm = new CreateCategoryViewModel();
            DataContext = vm;
        }
    }
}