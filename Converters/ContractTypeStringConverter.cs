using BuildMaterials.Models;
using System.Globalization;
using System.Windows.Data;

namespace BuildMaterials.Converters
{
    public class ContractTypeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ContactType)value)
            {
                case ContactType.Email:
                    {
                        return "Эл. почта";
                    }
                case ContactType.Phonenumber:
                    {
                        return "Номер телефона";
                    }
                    default:
                    {
                        return "";
                    }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (ContactType)value;
    }
}
