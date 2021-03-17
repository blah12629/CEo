using FluentAssertions;
using System;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace CEo.IO.Tests
{
    public class DirectoryManagerTests : IClassFixture<FileSystemFixture>
    {
        protected MockFileSystem FileSystem { get; }
        protected DirectoryManager DirectoryManager { get; }

        public DirectoryManagerTests(FileSystemFixture fileSystemFixture)
        {
            FileSystem = fileSystemFixture.FileSystem;
            DirectoryManager = new DirectoryManager(FileSystem);
        }

        [Theory]
        [InlineData(@"test_directory\test", @"C:\")]
        [InlineData(@"corrupt_files", @"C:\")]
        [InlineData(@"..\corrupt_files", @"C:\test_directory\")]
        [InlineData(@"..\..\..\corrupt_files", @"C:\test_directory\")]
        public void DirectoryExists_ShouldReturnTrue_WhenDirectoryExists(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            DirectoryManager.DirectoryExists(path).Should().BeTrue();
        }

        [Theory]
        [InlineData(@"normal_text.txt", @"C:\")]
        public void DirectoryExists_ShouldReturnFalse_WhenPathIsFile(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            DirectoryManager.DirectoryExists(path).Should().BeFalse();
        }

        [Theory]
        [InlineData("nonexistent_path", @"C:\")]
        [InlineData(@"..\nonesistent_path\path.txt", @"C:\corrupt_files")]
        public void DirectoryExists_ShouldReturnFalse_WhenPathDoesNotExist(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            DirectoryManager.DirectoryExists(path).Should().BeFalse();
        }

        [Theory]
        [InlineData(@"..\test_directory\test\test2\test3", @"C:\corrupt_files")]
        public void CreateDirectory_ShouldCreateDirectoryRecursively(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            DirectoryManager.CreateDirectory(path).Should().NotBeNull();
            DirectoryManager.DirectoryExists(path).Should().BeTrue();
        }

        [Theory]
        [InlineData("", @"C:\")]
        [InlineData("\n\r\t ", "corrupt_files")]
        public void CreateDirectory_ShouldReturnCurrentDirectory_WhenPathIsEmptyOrWhiteSpace(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            DirectoryManager.CreateDirectory(path).FullName
                .Should().Be(FileSystem.Path.GetFullPath(currentDirectory));
        }
    }
}