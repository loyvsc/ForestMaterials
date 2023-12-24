namespace BuildMaterials.Extensions
{
    public static class ConvertExtensions
    {
        public static string ToMySQLDate(this DateTime obj) => $"{obj.Date.Year}-{obj.Date.Month}-{obj.Date.Day}";
    }
}
