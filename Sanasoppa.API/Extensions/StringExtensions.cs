using System.Text.RegularExpressions;

namespace Sanasoppa.API.Extensions
{
    public static class StringExtensions
    {
        public static string Sanitize(this string str)
        {
            string pattern = @"[^a-zA-Z0-9,.\söÖäÄåÅ:;]";
            string sanitized = Regex.Replace(str, pattern, "");
            return sanitized;
        }
    }
}
