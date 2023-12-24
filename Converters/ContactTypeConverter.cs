using BuildMaterials.Models;
using System.Globalization;
using System.Windows.Data;

namespace BuildMaterials.Converters
{
    public class ContactTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (ContactType)value;
    }
}
