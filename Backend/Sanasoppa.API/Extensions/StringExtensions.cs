using System.Text.RegularExpressions;

namespace Sanasoppa.API.Extensions
{
    public static class StringExtensions
    {
        public static string Sanitize(this string str)
        {
            string pattern = @"[^\w\s.,:;+\-/&'öÖäÄåÅ]";
            string sanitized = Regex.Replace(str, pattern, "", RegexOptions.NonBacktracking);
            return sanitized;
        }
    }
}
