
using Avalonia;
using Avalonia.Controls;
using FinanceMobile.ViewModels;
using FinanceMobile;

namespace FinanceMobile.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            // Устанавливаем ViewModel как источник данных для этого экрана
            DataContext = new MainViewModel();
        }
    }
}