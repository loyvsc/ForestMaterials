using BuildMaterials.Models;
using System.Text.RegularExpressions;

namespace BuildMaterials.Helpers
{
    public class PhoneNumberInputHelper : NotifyPropertyChangedBase
    {
        private string? _phone;

        public PhoneNumberInputHelper()
        {
            MaxLength = 17;
        }

        public string? Phone
        {
            get => _phone;
            set
            {
                if (value == _phone) return;
                _phone = value;
                PhoneMask();
                OnPropertyChanged();
            }
        }

        public int MaxLength { get; set; }

        public async Task PhoneMask()
        {
            char[] charsToTrim = { '+', '(', ')', '-', ' ' };
            var newVal = Phone.Trim(charsToTrim);

            switch (newVal.Length)
            {
                case 3:
                    Phone = Regex.Replace(newVal, @"(\d{3})", "+$1");
                    break;
                case 5:
                    Phone = Regex.Replace(newVal, @"(\d{3})(\d{2})", "+$1($2)");
                    break;
                case 10:
                    Phone = Regex.Replace(newVal, @"(\d{3})(.{1})(\d{2})(.{1})(\d{3})",
                                         "+$1$2$3$4$5");
                    break;
                case 12:
                    Phone = Regex.Replace(newVal, @"(\d{3})(.{1})(\d{2})(.{1})(\d{3})(\d{2})",
                                         "+$1$2$3$4$5-$6-");
                    break;
            }
        }
    }
}
