using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BuildMaterials.Converters
{
    public class NDSConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value+"%";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
    }
}
