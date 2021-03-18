using FluentAssertions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CEo.Tests
{
    public class Extensions
    {
        public interface IMockCommand
        {
            void ExecuteBuildTest();
            void ExecuteBuildAllTest();
        }

        [Fact]
        public void Build_ShouldExecuteAllActions()
        {
            var command = Substitute.For<IMockCommand>();
            var reference = new Object();

            reference.Build(
                () => command.ExecuteBuildTest(),
                () => command.ExecuteBuildTest())
            .Should().BeSameAs(reference);
            command.Received(2).ExecuteBuildTest();
        }

        [Fact]
        public async Task BuildAllAsync_ShouldExecuteAllActions()
        {
            var command = Substitute.For<IMockCommand>();
            var reference = new Object();

            (await reference.BuildAllAsync(
                () => Task.Run(() => command?.ExecuteBuildAllTest()),
                async () =>
                {
                    await Task.Delay(500);
                    await Task.Run(() => command?.ExecuteBuildAllTest());
                },
                () => Task.Run(() => command?.ExecuteBuildAllTest()),
                () => Task.Run(() => command?.ExecuteBuildAllTest()),
                () => Task.Run(() => command?.ExecuteBuildAllTest())))
            .Should().BeSameAs(reference);

            command.Received(5).ExecuteBuildAllTest();
        }
    }
}