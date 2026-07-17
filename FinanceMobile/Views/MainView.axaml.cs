
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

            // Подписываемся на изменения и передаем новый ViewModel в ContentControl
            App.PageChanged += () =>
            {
                PageHost.Content = App.CurrentPage;
            };

            // Устанавливаем ViewModel как источник данных для этого экрана
            DataContext = new MainViewModel();
        }
    }
}