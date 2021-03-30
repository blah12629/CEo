using FluentAssertions;
using System;
using System.Numerics;
using Xunit;

namespace CEo.Tests
{
    public class ByteExtensionsTests
    {
        [Fact]
        public void Join_ShouldReturn0_WhenValuesIsEmpty() =>
            new Byte[] { }.Join().Should().Be(0);

        public static Object[][] JoinTestsData => new Object[][]
        {
            new Object[] { (BigInteger)12629, new Byte[] { 126, 2, 9 } },
            new Object[] { (BigInteger)123123123, new Byte[] { 1, 23, 123, 1, 2, 3 } }
        };

        [Theory]
        [MemberData(nameof(JoinTestsData))]
        public void JoinTests(BigInteger expectedValue, Byte[] values) =>
            values.Join().Should().Be(expectedValue);
    }
}