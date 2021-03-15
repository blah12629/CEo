using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace CEo.IO.Tests
{
    public class FileCheckerTests
    {
        protected MockFileSystem FileSystem { get; }
        protected FileChecker FileChecker { get; }

        public FileCheckerTests() : this(new MockFileSystem(
            new Dictionary<String, MockFileData>
            {
                [@"C:\normal_text.txt"] = new MockFileData("test txt file"),
                [@"C:\corrupt_files\corrupt_image.png"] = new MockFileData("test image file"),
                [@"C:\corrupt_files\corrupt_json.json"] = new MockFileData("test json file")
            },
            currentDirectory: @"C:\")) { }
        protected FileCheckerTests(MockFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            FileChecker = new FileChecker(fileSystem, new FileCheckerOptions(), default);
        }
        protected FileCheckerTests(MockFileSystem fileSystem, FileChecker fileChecker) :
            this(fileSystem) => FileChecker = fileChecker;

        [Theory]
        [InlineData(@"nonexistent_folder\corrupt_files\corrupt_image.png", @"C:\")]
        [InlineData(@"..\../directory/normal_text.txt", @"C:\corrupt_files\")]
        public void FileExists_ShouldReturnFalse_WhenRelativePathDoesNotExists(
            String relativePath, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            FileChecker.FileExists(relativePath).Should().BeFalse();
        }

        [Theory]
        [InlineData(@"..\corrupt_files\corrupt_image.png", @"C:\")]
        [InlineData(@"../normal_text.txt", @"C:\corrupt_files\")]
        public void FileExists_ShouldReturnTrue_WhenRelativePathExists(
            String relativePath, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            FileChecker.FileExists(relativePath).Should().BeTrue();
        }

        [Theory]
        [InlineData(@"nonexistent_folder\corrupt_files\corrupt_image.png", @"C:\")]
        [InlineData(@"currupt_json_2.json", @"C:\corrupt_files\")]
        public void FileExists_ShouldReturnFalse_WhenAbsolutePathDoesNotExists(
            String absolutePath, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            FileChecker.FileExists(absolutePath).Should().BeFalse();
        }

        [Theory]
        [InlineData(@"corrupt_files\corrupt_image.png", @"C:\")]
        [InlineData(@"corrupt_json.json", @"C:\corrupt_files\")]
        public void FileExists_ShouldReturnTrue_WhenAbsolutePathExists(
            String absolutePath, String currentDirectory)
        {
            FileSystem.Directory.SetCurrentDirectory(currentDirectory);
            FileChecker.FileExists(absolutePath).Should().BeTrue();
        }
    }
}