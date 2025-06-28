using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace EConstruction.Converters
{
    public class DecimalToCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {
                // Форматуємо як валюту з двома знаками після коми.
                // Ви можете налаштувати культуру або використовувати параметр для символу валюти,
                // але для простоти зараз використовуємо поточну культуру або "C2" для USD.
                return decimalValue.ToString("C2", CultureInfo.CurrentCulture);
            }
            return value; // Повертаємо оригінальне значення, якщо це не Decimal
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // У цьому випадку зворотне перетворення може бути не потрібне,
            // але якщо потрібно, реалізуйте логіку парсингу рядка в Decimal.
            // Наприклад:
            if (value is string stringValue && decimal.TryParse(stringValue.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, ""), NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal result))
            {
                return result;
            }
            return value;
        }
    }
}