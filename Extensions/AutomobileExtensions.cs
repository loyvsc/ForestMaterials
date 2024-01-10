using System.Text.RegularExpressions;

namespace BuildMaterials.Extensions
{
    public static class AutomobileExtensions
    {
        private static readonly Regex regex = new Regex("[0-9]{4} [A-Z]{2}-[1-7]");

        public static bool IsAutomobileRegistrationNumber(this string value) => regex.IsMatch(value);
    }
}
