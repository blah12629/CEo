using FluentAssertions;
using System;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace CEo.IO.Tests
{
    public class FileManagerTests : IClassFixture<FileSystemFixture>
    {
        public FileManagerTests(FileSystemFixture fileSystemFixture) 
        {
            FileSystem = fileSystemFixture.FileSystem;
            FileManager = new FileManager(FileSystem, default, default);
        }

        protected MockFileSystem FileSystem { get; }
        protected FileManager FileManager { get; }

        [Theory]
        [InlineData("deletable_file.png")]
        [InlineData("deletable_file.txt", @"deletable\deletable_file.txt")]
        public async Task DeleteAsync_ShouldDeleteFile_WhenExists(params String[] paths)
        {
            for (var i = 0; i < paths.Length; i ++)
            {
                FileManager.FileExists(paths[i]).Should().BeTrue();
                await FileManager.DeleteFileAsync(paths[i]);
                FileManager.FileExists(paths[i]).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData("abnormal_text.txt")]
        [InlineData("abnormal_text.txt", @"test_directory\test\jpg.png")]
        public async Task DeleteAsync_ShouldDeleteNothing_WhenFileDoesNotExist(
            params String[] paths)
        {
            for (var i = 0; i < paths.Length; i ++)
            {
                FileManager.FileExists(paths[i]).Should().BeFalse();
                await FileManager.DeleteFileAsync(paths[i]);
                FileManager.FileExists(paths[i]).Should().BeFalse();
            }
        }
    }
}