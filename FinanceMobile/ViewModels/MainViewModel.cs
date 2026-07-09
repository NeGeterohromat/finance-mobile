using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanceMobile.Databases;
using FinanceMobile.DataStructs;
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

        private DatabaseService dbs = DatabaseService.DatabaseServiceInstance;


        [ObservableProperty]
        private string _lastTableOut = "";

        [RelayCommand]
        private void GetFromDB()
        {
            var data = dbs.LoadAllCells();

            Week w1 = new Week() { StartDay = DateTime.Today, EndDay = DateTime.Today.AddDays(6) };
            Week w2 = new Week() { StartDay = DateTime.Today.AddDays(7), EndDay = DateTime.Today.AddDays(7 + 6) };
            Week w3 = new Week() { StartDay = DateTime.Today.AddDays(14), EndDay = DateTime.Today.AddDays(14 + 6) };

            Cell00 = data["sect1"]["c1"][w1.StartDay].ToString();
            Cell01 = data["sect1"]["c1"][w2.StartDay].ToString();
            Cell02 = data["sect1"]["c1"][w3.StartDay].ToString();
            Cell10 = data["sect2"]["c2"][w1.StartDay].ToString();
            Cell11 = data["sect2"]["c2"][w2.StartDay].ToString();
            Cell12 = data["sect2"]["c2"][w3.StartDay].ToString();
        }

        // Этот атрибут автоматически создаст публичное свойство ToggleGreetingCommand
        [RelayCommand]
        private void ToggleGreeting()
        {
            // Переключаем текст на противоположный
            WelcomeMessage = WelcomeMessage == OriginalText
                ? AlternateText
                : OriginalText;

            Section s1 = new Section("sect1", 3);
            Section s2 = new Section("sect2", 3);
            Week w1 = new Week() { StartDay = DateTime.Today, EndDay = DateTime.Today.AddDays(6) };
            Week w2 = new Week() { StartDay = DateTime.Today.AddDays(7), EndDay = DateTime.Today.AddDays(7+6) };
            Week w3 = new Week() { StartDay = DateTime.Today.AddDays(14), EndDay = DateTime.Today.AddDays(14+6) };
            s1.AddCategory("c1");
            s2.AddCategory("c2");
            s1["c1", w1] = new Cell() { Value = double.Parse(Cell00) };
            s1["c1", w2] = new Cell() { Value = double.Parse(Cell01) };
            s1["c1", w3] = new Cell() { Value = double.Parse(Cell02) };
            s2["c2", w1] = new Cell() { Value = double.Parse(Cell10) };
            s2["c2", w2] = new Cell() { Value = double.Parse(Cell11) };
            s2["c2", w3] = new Cell() { Value = double.Parse(Cell12) };

            dbs.SaveAllCells(new() { s1, s2 });
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
