using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BuildMaterials.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility val = (Visibility)value;
            switch (val)
            {
                case Visibility.Visible: { return Visibility.Collapsed; }
                case Visibility.Collapsed: { return Visibility.Visible; }
                default: { return Visibility.Collapsed; }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility val = (Visibility)value;
            switch (val)
            {
                case Visibility.Visible: { return Visibility.Collapsed; }
                case Visibility.Collapsed: { return Visibility.Visible; }
                default: { return Visibility.Collapsed; }
            }
        }
    }
}
