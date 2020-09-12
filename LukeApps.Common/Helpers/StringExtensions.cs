using System.Text;

namespace LukeApps.Common.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Get Substring from the left
        /// </summary>
        /// <param name="value"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static string Left(this string value, int l) =>
     (l <= value.Length && l > 0) ? value.Substring(0, l) : value;

        /// <summary>
        /// Get Substring from the right
        /// </summary>
        /// <param name="value"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string Right(this string value, int r) =>
             (r <= value.Length && r > 0) ? value.Substring(value.Length - r) : value;

        /// <summary>
        /// Inserts a Space between words if Capital Letter is used (Works with Camel Case)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="preserveAcronyms"></param>
        /// <returns></returns>
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}