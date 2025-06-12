using System;
using System.Globalization;
using System.Windows.Data;

namespace EConstruction.Converters // Створити цю папку та додати файл
{
    /// <summary>
    /// Конвертер, що перетворює булеве значення на кут повороту (для іконок).
    /// Зазвичай використовується для іконок "розгорнути/згорнути".
    /// </summary>
    public class BooleanToAngleConverter : IValueConverter
    {
        public double TrueAngle { get; set; } = 90;  // Кут при значенні True
        public double FalseAngle { get; set; } = 0; // Кут при значенні False

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? TrueAngle : FalseAngle;
            }
            return FalseAngle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
