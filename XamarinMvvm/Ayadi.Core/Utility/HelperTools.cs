

using System.Text.RegularExpressions;

namespace Ayadi.Core.Utility
{
    public static class HelperTools
    {
        public static bool ValidateEmail(string str)
        {
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        public static string RemoveHTMLtags(string text)
        {
            return Regex.Replace(text, "<.*?>", string.Empty);
        }

    }
}
