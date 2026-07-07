using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

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

        // Свойства для ячеек таблицы (3 колонки x 5 строк = 15 ячеек)
        [ObservableProperty] private string _cell00 = "";
        [ObservableProperty] private string _cell01 = "";
        [ObservableProperty] private string _cell02 = "";

        [ObservableProperty] private string _cell10 = "";
        [ObservableProperty] private string _cell11 = "";
        [ObservableProperty] private string _cell12 = "";

        [ObservableProperty] private string _cell20 = "";
        [ObservableProperty] private string _cell21 = "";
        [ObservableProperty] private string _cell22 = "";

        [ObservableProperty] private string _cell30 = "";
        [ObservableProperty] private string _cell31 = "";
        [ObservableProperty] private string _cell32 = "";

        [ObservableProperty] private string _cell40 = "";
        [ObservableProperty] private string _cell41 = "";
        [ObservableProperty] private string _cell42 = "";


        [ObservableProperty]
        private string _lastTableOut = "";

        // Этот атрибут автоматически создаст публичное свойство ToggleGreetingCommand
        [RelayCommand]
        private void ToggleGreeting()
        {
            // Переключаем текст на противоположный
            WelcomeMessage = WelcomeMessage == OriginalText
                ? AlternateText
                : OriginalText;
        }

        // Этот метод вызывается автоматически при изменении Cell00
        // Имя метода всегда: On<ИмяСвойства>Changed
        partial void OnCell00Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell01Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell02Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell10Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell11Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell12Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell20Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell21Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell22Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell30Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell31Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell32Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell40Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell41Changed(string value)
        {
            LastTableOut = value;
        }
        partial void OnCell42Changed(string value)
        {
            LastTableOut = value;
        }
    }
}
