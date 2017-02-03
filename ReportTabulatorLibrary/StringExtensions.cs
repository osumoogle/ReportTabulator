using System.Linq;

namespace ReportTabulatorLibrary
{
    public static class StringExtensions
    {
        public static bool IsOnlyDigits(this string value)
        {
            return value.All(c => c >= '0' && c <= '9');
        }
    }
}