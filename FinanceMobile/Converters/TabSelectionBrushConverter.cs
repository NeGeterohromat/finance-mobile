using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace FinanceMobile.Converters
{
    // Использование в XAML:
    // Background="{Binding SelectedPeriod, Converter={StaticResource TabBrush}, ConverterParameter='Неделя|#FFFFFF|Transparent'}"
    // ConverterParameter формата: "значение_вкладки|цвет_если_выбрана|цвет_если_не_выбрана"
    public class TabSelectionBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is not string p) return Brushes.Transparent;

            var parts = p.Split('|');
            if (parts.Length != 3) return Brushes.Transparent;

            var targetValue = parts[0];
            var selectedColor = parts[1];
            var unselectedColor = parts[2];

            bool isSelected = value?.ToString() == targetValue;
            return Brush.Parse(isSelected ? selectedColor : unselectedColor);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}