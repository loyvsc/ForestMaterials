using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BuildMaterials.Converters
{
    public class MaterialResponseStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((bool)value)
            {
                case true: { return "Является"; }
                case false: { return "Не является"; }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
    }
}
