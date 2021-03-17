using FluentAssertions;
using System;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace CEo.IO.Tests
{
    public class FileWriterTests : IClassFixture<FileSystemFixture>
    {
        public class MockFileWriterOptions : IFileWriterOptions
        {
            public Boolean CreateDirectories { get; set; } =
                IFileWriterOptions.DefualtCreateDirectories;
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
        [InlineData(@"..\test_directory\", @"C:\corrupt_files\")]
        public void OpenRead_ShouldReturnNull_WhenPathIsADirectory(
            String path, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            FileWriter.OpenRead(path).Should().BeNull();
        }

        // [Theory]
        // public void OpenRead_ShouldReturnNull_WhenDirectoryDoesNotExistAndOptionIsFalse(
        //     String path, String currentDirectory)
        // {
        //     FileSystem.Directory.SetCurrentDirectory(currentDirectory);
        //     Options.CreateDirectories = false;
        //     FileWriter.OpenRead(path).Should().BeNull();
        // }
    }
}