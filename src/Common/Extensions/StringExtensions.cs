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
    }
}