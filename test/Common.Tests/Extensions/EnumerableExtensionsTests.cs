using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CEo.Tests
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void Split_ShouldReturnEmpty_WhenInputIsEmpty() =>
            new String[0].Split(69).Should().BeEmpty();

        [Fact]
        public void Split_ShouldDivideEvenly_WhenSplitSizeDividesCount() =>
            new Int32[] { 0, 1, 2 }.Split(1).Should().BeEquivalentTo(
                new Int32[][] { new Int32[] { 0 }, new Int32[] { 1 }, new Int32[] { 2 } });

        [Fact]
        public void Split_ShouldFillExistingArrays_BeforeInitializingNew_WhenSpliSizeDoesNotDivideCount() =>
            new Int32[] { 0, 1, 2, 3, 4 }.Split(3).Should().BeEquivalentTo(
                new Int32[][] { new Int32[] { 0, 1, 2 }, new Int32[] { 3, 4 } });

        [Theory]
        [InlineData(new Int32[0] { }, new Int32[] { 0, 1 })]
        [InlineData(new Int32[] { 0 }, new Int32[] { 0, 1 })]
        public void ElementsAt_ShouldThrowOutOfRangeException_WhenIndexIsGreaterThanCount(
            IEnumerable<Int32> collection, IEnumerable<Int32> indices) =>
                new Action(() => collection.ElementsAt(indices).ToList())
                    .Should().Throw<ArgumentOutOfRangeException>();

        [Theory]
        [InlineData(new Int32[] { 1, 9 }, new Int32[] { 1, 2, 6, 2, 9 }, new Int32[] { 0, 4 })]
        [InlineData(new Int32[] { 6, 9 }, new Int32[] { 6, 9 }, new Int32[] { 0, 1 })]
        public void ElementsAt_ShouldReturnElementsAt_WhenIndicesAreLessThanCount(
            IEnumerable<Int32> expected,
            IEnumerable<Int32> collection,
            IEnumerable<Int32> indices) =>
                collection.ElementsAt(indices)
                    .Should().BeEquivalentTo(expected);
    }
}