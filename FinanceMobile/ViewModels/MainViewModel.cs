using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FinanceMobile.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        // Этот атрибут автоматически создает свойство WelcomeMessage 
        // и реализует уведомление об изменении (INotifyPropertyChanged)
        [ObservableProperty]
        private string _welcomeMessage = "Добро пожаловать в наше финансовое приложение";

        // Исходный и альтернативный тексты (удобно хранить в константах)
        private const string OriginalText = "Добро пожаловать в наше финансовое приложение";
        private const string AlternateText = "Рады видеть вас снова!";

        // Этот атрибут автоматически создаст публичное свойство ToggleGreetingCommand
        [RelayCommand]
        private void ToggleGreeting()
        {
            // Переключаем текст на противоположный
            WelcomeMessage = WelcomeMessage == OriginalText
                ? AlternateText
                : OriginalText;
        }
    }
}
