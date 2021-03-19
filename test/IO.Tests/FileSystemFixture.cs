using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace CEo.IO.Tests
{
    public class FileSystemFixture
    {
        public MockFileSystem FileSystem { get; }

        public FileSystemFixture() => FileSystem = new MockFileSystem(
            new Dictionary<String, MockFileData>
            {
                [@"C:\deletable_file.png"] = new MockFileData("deletable 0"),
                [@"C:\deletable_file.txt"] = new MockFileData("deletable 1"),
                [@"C:\deletable\deletable_file.txt"] = new MockFileData("deletable 2"),
                [@"C:\normal_text.txt"] = new MockFileData("test txt file"),
                [@"C:\test_array.json"] = new MockFileData("[ 0, 1, 2 ]"),
                [@"C:\test_null.json"] = new MockFileData("null"),
                [@"C:\test_directory\test\png.png"] = new MockFileData("test corrupt png"),
                [@"C:\corrupt_files\corrupt_image.png"] = new MockFileData("test image file"),
                [@"C:\corrupt_files\corrupt_json.json"] = new MockFileData("test json file")
            },
            currentDirectory: @"C:\");
    }
}