using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CEo.IO.Tests
{
    public class FileReaderTests : IClassFixture<FileSystemFixture>
    {
        protected MockFileSystem FileSystem { get; }
        protected FileReader FileReader { get; }

        public FileReaderTests(FileSystemFixture fileSystemFixture)
        {
            FileSystem = fileSystemFixture.FileSystem;
            FileReader = new FileReader(FileSystem);
        }

        [Theory]
        [InlineData(@"nonexistent_path\nonexistent_file", @"C:\")]
        [InlineData(@"..\..\nonexistent_path\nonexistent_file", @"C:\")]
        public void OpenRead_ShouldThrowFileNotFoundException_WhenFileDoesNotExist(
            String filePath, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            new Action(() => FileReader.OpenRead(filePath))
                .Should().Throw<FileNotFoundException>();
        }

        [Theory]
        [InlineData(@"C:\normal_text.txt")]
        [InlineData(@"C:\test_directory\test\png.png")]
        public void ReadImageAsync_ShouldThrowArgumentException_WhenReadingStringContents(String path) =>
            new Func<Task>(async () => await FileReader.ReadImageAsync(path))
                .Should().Throw<ArgumentException>();

        [Fact]
        public void ReadJsonAsync_ShouldThrowJsonException_WhenParsingArrayAsDictionary() =>
            new Func<Task>(async () => await FileReader
                .ReadJsonAsync<IDictionary<String, String>>("test_array.json"))
                    .Should().Throw<JsonException>();

        [Fact]
        public async Task ReadJsonAsync_ShouldReturnArray_WhenParsingArrayAsArray() =>
            (await FileReader.ReadJsonAsync<Int32[]>("test_array.json"))
                .Should().BeEquivalentTo(new[] { 0, 1, 2 });

        [Fact]
        public async Task ReadJsonAsync_ShouldReturnNull_WhenFileContainingNull() =>
            (await FileReader.ReadJsonAsync<Int32[]>("test_null.json")).Should().BeNull();
    }
}