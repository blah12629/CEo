using Microsoft.Extensions.Logging;
using System;
using System.IO.Abstractions;

namespace CEo.IO
{
    public interface IFileChecker
    {
        Boolean FileExists(String fielPath);
    }

    public interface IFileCheckerOptions { }

    public class FileChecker : IFileChecker
    {
        public FileChecker(
            IFileCheckerOptions? options = default,
            ILogger? logger = default) :
                this(new FileSystem(), options, logger) { }
        public FileChecker(
            IFileSystem fileSystem,
            IFileCheckerOptions? options = default,
            ILogger? logger = default)
        {
            (Options, Logger) = (options ?? new FileCheckerOptions(), logger);
            FileSystem = fileSystem;
        }

        protected IFileSystem FileSystem { get; }
        protected IFileCheckerOptions Options { get; }
        protected ILogger? Logger { get; }

        public virtual Boolean FileExists(String filePath) =>
            FileSystem.File.Exists(filePath);
    }

    public record FileCheckerOptions : IFileCheckerOptions { }
}