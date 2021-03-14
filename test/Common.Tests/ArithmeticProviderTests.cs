using FluentAssertions;
using System;
using Xunit;

namespace CEo.Tests
{
    public class ArithmeticProviderTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenTInvalid()
        {
            new Action(() => new ArithmeticProvider<UInt64>())
                .Should().Throw<ArgumentException>();

            new Action(() => new ArithmeticProvider<Object>())
                .Should().Throw<ArgumentException>();

            new Action(() => new ArithmeticProvider<FactAttribute>())
                .Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(2, 5, 2)]
        [InlineData(0, 1, 2)]
        [InlineData(1, 2, 2)]
        [InlineData(0, 0, 69)]
        public void Divide_ShouldDoIntegerDivision_WhenTUnsignedInteger(
            Object quotient, Object left, Object right)
        {
            var int32Provider = new ArithmeticProvider<Int32>();
            var uint32Provider = new ArithmeticProvider<UInt32>();

            int32Provider.Divide(Convert.ToInt32(left), Convert.ToInt32(right))
                .Should().Be(Convert.ToInt32(quotient));

            uint32Provider.Divide(Convert.ToUInt32(left), Convert.ToUInt32(right))
                .Should().Be(Convert.ToUInt32(quotient));
        }

        [Theory]
        [InlineData(-2, -5, 2)]
        [InlineData(0, 1, 2)]
        [InlineData(-1, -2, 2)]
        [InlineData(0, 0, -69)]
        public void Divide_ShouldDoIntegerDivision_WhenTSignedInteger(
            Object quotient, Object left, Object right)
        {
            var int32Provider = new ArithmeticProvider<Int32>();

            int32Provider.Divide(Convert.ToInt32(left), Convert.ToInt32(right))
                .Should().Be(Convert.ToInt32(quotient));
        }

        [Theory]
        [InlineData(-2.5f, -5, 2)]
        [InlineData(0.5f, 1, 2)]
        [InlineData(-1, -2, 2)]
        [InlineData(0, 0, -69)]
        public void Divide_ShouldDoFloatingPointDivision_WhenTFloatingPoint(
            Object quotient, Object left, Object right)
        {
            var singleProvider = new ArithmeticProvider<Single>();
            var doubleProvider = new ArithmeticProvider<Double>();

            singleProvider.Divide(Convert.ToSingle(left), Convert.ToSingle(right))
                .Should().Be(Convert.ToSingle(quotient));

            doubleProvider.Divide(Convert.ToDouble(left), Convert.ToDouble(right))
                .Should().Be(Convert.ToDouble(quotient));
        }

        [Theory]
        [InlineData(69.9d, -69.9d)]
        [InlineData(0, 0)]
        public void Negate_ShouldReturnNegatedValue_WhenTSignedValue(
            Object expectedValue, Object valueToNegate)
        {
            IArithmeticProvider int32Provider = new ArithmeticProvider<Int32>(),
                singleProvider = new ArithmeticProvider<Single>(),
                doubleProvider = new ArithmeticProvider<Double>();

            int32Provider.Negate(valueToNegate)
                .Should().Be(Convert.ToInt32(expectedValue));

            singleProvider.Negate(valueToNegate)
                .Should().Be(Convert.ToSingle(expectedValue));

            doubleProvider.Negate(valueToNegate)
                .Should().Be(Convert.ToDouble(expectedValue));
        }

        [Theory]
        [InlineData(4294967226, 69)]
        [InlineData(4294967295, 0)]
        public void Negate_ShouldReturnMaxMinusValue_WhenTUInt32(
            UInt32 expectedValue, UInt32 valueToNegate)
        {
            var uint32Provider = new ArithmeticProvider<UInt32>();
            uint32Provider.Negate(valueToNegate).Should().Be(expectedValue);
        }
    }
}