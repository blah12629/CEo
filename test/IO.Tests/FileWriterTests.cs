using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using System.Text.Json;
using Xunit;

namespace CEo.IO.Tests
{
    public class FileWriterTests : IClassFixture<FileSystemFixture>
    {
        public class MockFileWriterOptions : IFileWriterOptions
        {
            public Boolean CreateDirectories { get; set; } =
                IFileWriterOptions.DefaultCreateDirectories;
            public Boolean WarnOnOverwrite { get; set; } =
                IFileWriterOptions.DefaultWarnOnOverwrite;
        }

        protected MockFileSystem FileSystem { get; }
        protected FileWriter FileWriter { get; }
        protected MockFileWriterOptions Options { get; }

        public FileWriterTests(FileSystemFixture fileSystemFixture)
        {
            FileSystem = fileSystemFixture.FileSystem;
            Options = new MockFileWriterOptions();
            FileWriter = new FileWriter(FileSystem, Options);
        }

        [Theory]
        [InlineData("test_directory", @"C:\")]
        [InlineData(@"test_directory\test_file\", @"C:\")]
        [InlineData(@"..\test_directory/", @"C:\corrupt_files\")]
        public void OpenWrite_ShouldThrowArgumentException_WhenPathIsADirectory(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            new Action(() => FileWriter.OpenWrite(path))
                .Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(@"nonexistent_path\test.txt", @"C:\\")]
        public void OpenWrite_ShouldThrowDirectoryNotFoundException_WhenDirectoryDoesNotExistAndOptionIsFalse(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            Options.CreateDirectories = false;
            new Action(() => FileWriter.OpenWrite(path)).Should().Throw<DirectoryNotFoundException>();
        }

        [Theory]
        [InlineData(@"C:\directory\to", @"..\..\directory\to\create.dir", @"C:\test_directory\test\")]
        public void OpenWrite_ShouldCreateDirectories_WhenOptionsIsTrue(
            String newDirectory, String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            FileWriter.OpenWrite(path);
            FileSystem.Directory.Exists(newDirectory).Should().BeTrue();
        }

        [Fact]
        public async Task WriteBitmapAsync_ShouldSave_WhenImageFormatIsMemoryBmp()
        {
            var bitmap = new Bitmap(16, 16);
            var fileName = "memory_bitmap.bmp";
            bitmap.RawFormat.Should().Be(ImageFormat.MemoryBmp);
            await FileWriter.WriteBitmapAsync(bitmap, fileName);
            FileSystem.File.Exists(fileName).Should().BeTrue();
        }

        [Fact]
        public async Task WriteImageAsync_ShouldSave()
        {
            var image = new Bitmap(16, 16);
            var fileName = "test_image.gif";
            await FileWriter.WriteImageAsync(image, fileName);
            FileSystem.File.Exists(fileName).Should().BeTrue();
        }

        [Fact]
        public async Task WriteJsonAsync_ShouldSave()
        {
            var testObject = new Dictionary<String, String> {
                ["test1"] = "test1",
                ["test2"] = "test2"
            };
            var fileName = "test_json.json";
            await FileWriter.WriteJsonAsync(testObject, fileName);
            FileSystem.File.Exists(fileName).Should().BeTrue();

            var savedObject = JsonSerializer
                .Deserialize<IDictionary<String, String>>(
                    await FileSystem.File.ReadAllTextAsync(fileName)) ??
                new Dictionary<String, String>();
            savedObject["test1"].Should().Be("test1");
            savedObject["test2"].Should().Be("test2");
        }

        [Fact]
        public async Task WriteStringAsync_ShouldSave()
        {
            var testContent = "test contents";
            var fileName = "test_txt.txt";

            await FileWriter.WriteStringAsync(testContent, fileName);
            (await FileSystem.File.ReadAllTextAsync(fileName))
                .Should().Be(testContent);

        }
    }
}