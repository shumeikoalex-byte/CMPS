using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EConstruction.Converters // Створити цю папку та додати файл
{
    /// <summary>
    /// Конвертер, що перетворює рядок на Visibility.
    /// Якщо рядок null або порожній, повертає Collapsed; інакше Visible.
    /// Використовується для перемикання між Path і Emoji іконками.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && !string.IsNullOrEmpty(s))
            {
                return Visibility.Visible; // Якщо рядок не порожній, робимо видимим
            }
            return Visibility.Collapsed; // Якщо рядок порожній, приховуємо
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Конвертер, що перетворює рядок на Visibility.
    /// Якщо рядок null або порожній, повертає Visible; інакше Collapsed.
    /// Використовується для відображення Emoji, коли PathData відсутній.
    /// </summary>
    public class InverseStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && !string.IsNullOrEmpty(s))
            {
                return Visibility.Collapsed; // Якщо рядок не порожній, приховуємо
            }
            return Visibility.Visible; // Якщо рядок порожній, робимо видимим
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
