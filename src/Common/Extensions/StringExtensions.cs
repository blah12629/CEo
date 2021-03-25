using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace System
{
    public static partial class StringExtensions
    {
        static String _multipleWhiteSpacesPattern = @"(?<!^)(?<!\s)\s\s+(?!\s)(?!$)";

        // https://stackoverflow.com/a/1211435
        public static String FromPascalCaseToSentenceCase(this String pascalString) =>
            Regex.Replace(pascalString, "[a-z][A-Z]", match =>
                $"{ match.Value[0] } { char.ToLower(match.Value[1]) }");

        public static String FromPascalCaseToSnakeCase(this String pascalString) =>
            pascalString.FromPascalCaseToSentenceCase()
                .FromSentenceCaseToSnakeCase();

        public static String FromPascalCaseToTitleCase(this String pascalString) =>
            Regex.Replace(pascalString, "[a-z][A-Z]", match =>
                $"{ match.Value[0] } { char.ToUpper(match.Value[1]) }");

        public static String FromSentenceCaseToSnakeCase(this String sentenceString) =>
            Regex.Replace(
                sentenceString.ReplaceWhiteSpaceChains("_").ToLower(),
                @"(?<!\s)\s(?!\s)",
                match => "_");

        public static String ReplaceWhiteSpaceChains(this String value, String replacement) =>
            Regex.Replace(value, _multipleWhiteSpacesPattern, match => replacement);

        public static String GetLongestSubstring(this IEnumerable<String> values)
        {
            if (!values.Any() || values.Any(value => String.IsNullOrEmpty(value)))
                return String.Empty;

            var valuesSorted = values.OrderBy(value => value.Length).ToList().AsReadOnly();
            var longestSubstring = String.Empty;

            for (var i = 0; i < valuesSorted[0].Length; i ++)
            {
                longestSubstring += valuesSorted[0][i];

                for (var j = 0; j < valuesSorted.Count; j ++)
                    if (valuesSorted[j][0 .. (i + 1)] != longestSubstring)
                        return longestSubstring[.. (longestSubstring.Length - 1)];
            }

            return longestSubstring;
        }
    }
}