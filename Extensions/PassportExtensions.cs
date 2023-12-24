using System.Text.RegularExpressions;

namespace BuildMaterials.Extensions
{
    public static class PassportExtensions
    {
        public static bool CheckPassportNumber(string passportNumber)
        {
            Regex regex = new Regex(@"^[A-Z]{2}\d{7}$");
            return regex.IsMatch(passportNumber);
        }
    }
}
