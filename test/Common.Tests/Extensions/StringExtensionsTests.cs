using FluentAssertions;
using System;
using Xunit;

namespace CEo.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("\n\r\t", "\n\r\t")]
        [InlineData("This\nIS\ta test \tSentence", "This\nIS\taTest \tSentence")]
        public void FromPascalCaseToSentenceCase_ShouldReturnSentence(
            String expected, String input) =>
                input.FromPascalCaseToSentenceCase().Should().Be(expected);

        [Theory]
        [InlineData("", "")]
        [InlineData("\n\r\t", "\n\r\t")]
        [InlineData("this_is_a_test_sentence", "This\nIS\taTest \tSentence")]
        public void FromPascalCaseToSnakeCase_ShouldReturnSnakeCase(
            String expected, String input) =>
                input.FromPascalCaseToSnakeCase().Should().Be(expected);

        [Theory]
        [InlineData("", "")]
        [InlineData("\n\r\t", "\n\r\t")]
        [InlineData("This\nIS\ta Test \tSentence", "This\nIS\taTest \tSentence")]
        public void FromPascalCaseToTitleCase_ShouldReturnTitleCase(
            String expected, String input) =>
                input.FromPascalCaseToTitleCase().Should().Be(expected);

        [Theory]
        [InlineData("", "")]
        [InlineData("\n\r\t", "\n\r\t")]
        [InlineData("this_is_atest_sentence", "This\nIS\taTest \tSentence")]
        public void FromSentenceCaseToSnakeCase_ShouldReturnSnakeCase(
            String expected, String input) =>
                input.FromSentenceCaseToSnakeCase().Should().Be(expected);

        [Theory]
        [InlineData("", "", "asd")]
        [InlineData("\n\r\t", "\n\r\t", "asd")]
        [InlineData("This\nIS\taTestasdSentence", "This\nIS\taTest \tSentence", "asd")]
        public void ReplaceWhiteSpaceChains(
            String expected, String input, String replacement) =>
                input.ReplaceWhiteSpaceChains(replacement).Should().Be(expected);
    }
}