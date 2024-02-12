using BuildMaterials.Models;
using System.Text.RegularExpressions;

namespace BuildMaterials.Helpers
{
    public partial class PhoneNumberInputHelper : NotifyPropertyChangedBase
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
                if (value != _phone)
                {
                    _phone = value;
                    PhoneMask();
                    OnPropertyChanged();
                }
            }
        }

        public string? PhoneWithoutMask => Phone?.Trim(new char[] { '+', '(', ')', '-', ' ' });

        public int MaxLength { get; set; }

        public async Task PhoneMask()
        {
            var newVal = PhoneWithoutMask;

            switch (newVal.Length)
            {
                case 3:
                    _phone = Regex3().Replace(newVal, "+$1");
                    break;
                case 5:
                    _phone = Regex5().Replace(newVal, "+$1($2)");
                    break;
                case 10:
                    _phone = Regex10().Replace(newVal, "+$1$2$3$4$5");
                    break;
                case 12:
                    _phone = Regex12().Replace(newVal, "+$1$2$3$4$5-$6-");
                    break;
            }
        }

        [GeneratedRegex("(\\d{3})")]
        private static partial Regex Regex3();
        [GeneratedRegex("(\\d{3})(\\d{2})")]
        private static partial Regex Regex5();
        [GeneratedRegex("(\\d{3})(.{1})(\\d{2})(.{1})(\\d{3})")]
        private static partial Regex Regex10();
        [GeneratedRegex("(\\d{3})(.{1})(\\d{2})(.{1})(\\d{3})(\\d{2})")]
        private static partial Regex Regex12();
    }
}
