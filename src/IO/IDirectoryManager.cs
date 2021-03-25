using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace CEo.IO
{
    public interface IDirectoryManager
    {
        Boolean DirectoryExists(String directoryPath);
        IDirectoryInfo CreateDirectory(String directoryPath);

        IEnumerable<String> EnumerateFiles(
            String directory,
            SearchOption? searchOptions = SearchOption.TopDirectoryOnly);
        IEnumerable<String> EnumerateFiles(
            String directory, String searchPattern,
            SearchOption? searchOptions = SearchOption.TopDirectoryOnly);
        IEnumerable<String> EnumerateFiles(
            String directory, String searchPattern,
            EnumerationOptions enumerationOptions);
    }

    public interface IDirectoryManagerOptions { }

    public class DirectoryManager : IDirectoryManager
    {
        Object _createDirectoryLock = new Object();

        public DirectoryManager(
            IDirectoryManagerOptions? options = default,
            ILogger? logger = default) :
                this(new FileSystem(), options, logger) { }
        public DirectoryManager(
            IFileSystem fileSystem,
            IDirectoryManagerOptions? options = default,
            ILogger? logger = default)
        {
            (Options, Logger) = (options ?? new DirectoryManagerOptions(), logger);
            FileSystem = fileSystem;
        }

        protected IFileSystem FileSystem { get; }
        protected IDirectoryManagerOptions Options { get; }
        protected ILogger? Logger { get; }

        public virtual Boolean DirectoryExists(String directoryPath) =>
            FileSystem.Directory.Exists(directoryPath);

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Locks, waits for existing <see cref="CreateDirectory" /> calls to finish,
        ///       then unlocks to check if there are existing directories.
        ///   </para>
        /// </remarks>
        /// <param name="directoryPath"></param>
        /// <returns>
        ///   <para>
        ///     A <see cref="IDirectoryInfo" /> of the created the directory,
        ///       or the current directory if <see cref="directoryPath" /> is empty.
        ///   </para>
        /// </returns>
        public virtual IDirectoryInfo CreateDirectory(String directoryPath)
        {
            var currentDirectory = FileSystem.Directory.GetCurrentDirectory();
            if (String.IsNullOrWhiteSpace(directoryPath))
                return FileSystem.Directory.CreateDirectory(".");

            var directoryExists = true;
            IDirectoryInfo createdDirectory;

            lock(_createDirectoryLock)
            {
                directoryExists = DirectoryExists(directoryPath);
                createdDirectory = FileSystem.Directory.CreateDirectory(directoryPath);
            }
            if (directoryExists) return createdDirectory;

            var createdPath = createdDirectory.FullName
                .Replace(currentDirectory, String.Empty);

            return createdDirectory;
        }

        // UNTESTED
        public virtual IEnumerable<String> EnumerateFiles(
            String directory,
            SearchOption? searchOption = SearchOption.TopDirectoryOnly) =>
            searchOption.HasValue ?
                FileSystem.Directory.EnumerateFiles(directory, "*", searchOption.Value) :
                FileSystem.Directory.EnumerateFiles(directory);

        // UNTESTED
        public virtual IEnumerable<String> EnumerateFiles(
            String directory, String searchPattern,
            SearchOption? searchOption = SearchOption.TopDirectoryOnly) =>
            searchOption.HasValue ?
                FileSystem.Directory.EnumerateFiles(
                    directory, searchPattern, searchOption.Value) :
                FileSystem.Directory.EnumerateFiles(directory, searchPattern);

        // UNTESTED
        public virtual IEnumerable<String> EnumerateFiles(
            String directory, String searchPattern,
            EnumerationOptions enumerationOptions) =>
                FileSystem.Directory.EnumerateFiles(
                    directory, searchPattern, enumerationOptions);
    }

    public record DirectoryManagerOptions : IDirectoryManagerOptions { }
}