using Microsoft.Extensions.Logging;
using System;
using System.IO.Abstractions;

namespace CEo.IO
{
    public interface IFileChecker
    {
        Boolean FileExists(String fielPath);
    }

    public class FileChecker : IFileChecker
    {
        public FileChecker(IFileCheckerOptions options, ILogger? logger)
        {
            (Options, Logger) = (options, logger);
            FileSystem = new FileSystem();
        }
        public FileChecker(
            IFileSystem fileSystem, IFileCheckerOptions options, ILogger? logger) :
                this(options, logger) =>
                    FileSystem = fileSystem;

        protected IFileSystem FileSystem { get; }
        protected IFileCheckerOptions Options { get; }
        protected ILogger? Logger { get; }

        public virtual Boolean FileExists(String filePath) =>
            FileSystem.File.Exists(filePath);
    }

    public interface IFileCheckerOptions { }

    public record FileCheckerOptions : IFileCheckerOptions { }
}