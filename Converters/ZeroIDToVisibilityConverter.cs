using BuildMaterials.BD;
using BuildMaterials.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BuildMaterials.Converters
{
    public class ZeroIDToVisibilityConverter : IValueConverter
    {
        private ITable unsetValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ITable)
            {
                unsetValue = (ITable)value;
                return (value as ITable).ID==0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }

            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => unsetValue;
    }
}
