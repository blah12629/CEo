using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CEo.IO
{
    public interface IFileManager : IFileReader, IFileWriter
    {
        Task DeleteFileAsync(
            String filePath,
            CancellationToken cancellationToken = default);

        Task DeleteFilesAsync(
            IEnumerable<String> filePaths,
            CancellationToken cancellationToken = default);

        Task CopyToAsync(
            String source, String destination,
            CancellationToken cancellationToken = default);
    }

    public interface IFileManagerOptions : IFileReaderOptions, IFileWriterOptions { }

    public class FileManager : IFileManager
    {
        public FileManager(
            IFileManagerOptions? options = default,
            ILogger? logger = default) :
                this(new FileSystem(), options, logger) { }
        public FileManager(
            IFileSystem fileSystem,
            IFileManagerOptions? options = default,
            ILogger? logger = default) :
            this(fileSystem, default, default, default, options, logger) { }
        public FileManager(
            IFileSystem fileSystem,
            IDirectoryManager? directoryManager = default,
            IFileReader? fileReader = default,
            IFileWriter? fileWriter = default,
            IFileManagerOptions? options = default,
            ILogger? logger = default)
        {
            (Options, Logger) = (options ?? new FileManagerOptions(), logger);
            FileSystem = fileSystem;

            var fileChecker = new FileChecker(FileSystem, Options, Logger);
            DirectoryManager = directoryManager ??
                new DirectoryManager(FileSystem, Options, Logger);
            FileReader = fileReader ??
                new FileReader(FileSystem, fileChecker, Options, Logger);
            FileWriter = fileWriter ??
                new FileWriter(FileSystem, fileChecker, DirectoryManager, Options, Logger);
        }

        protected IFileSystem FileSystem { get; }
        protected IDirectoryManager DirectoryManager { get; }
        protected IFileReader FileReader { get; }
        protected IFileWriter FileWriter { get; }
        protected IFileManagerOptions Options { get; }
        protected ILogger? Logger { get; }

        public virtual Task DeleteFileAsync(
            String filePath,
            CancellationToken cancellationToken = default) =>
            Task.Run(() => FileSystem.File.Delete(filePath), cancellationToken);

        public virtual Task DeleteFilesAsync(
            IEnumerable<String> filePaths,
            CancellationToken cancellationToken = default) =>
            this.BuildAllAsync(filePaths
                .Select(path => new Func<Task>(() =>
                    DeleteFileAsync(path, cancellationToken)))
                .ToArray());

        public virtual async Task CopyToAsync(
            String source, String destination,
            CancellationToken cancellationToken = default)
        {
            DirectoryManager.CreateDirectory(FileSystem.Path.GetDirectoryName(destination));

            if (FileExists(destination))
                await DeleteFileAsync(destination, cancellationToken);

            await Task.Run(
                () => FileSystem.File.Copy(source, destination),
                cancellationToken);
        }

        public virtual Boolean FileExists(String filePath) =>
            FileReader.FileExists(filePath);

        public virtual Task<Bitmap> ReadBitmapAsync(
            String bitmapPath,
            CancellationToken cancellationToken = default) =>
                FileReader.ReadBitmapAsync(bitmapPath, cancellationToken);

        public virtual Task<Image> ReadImageAsync(
            String imagePath,
            CancellationToken cancellationToken = default) =>
                FileReader.ReadImageAsync(imagePath, cancellationToken);

        public virtual Task<T> ReadJsonAsync<T>(
            String jsonPath,
            JsonSerializerOptions? serializerOptions = default,
            CancellationToken cancellationToken = default) =>
                FileReader.ReadJsonAsync<T>(jsonPath, serializerOptions, cancellationToken);

        public virtual Task<String> ReadStringAsync(
            String stringPath,
            CancellationToken cancellationToken = default) =>
                FileReader.ReadStringAsync(stringPath, cancellationToken);

        public virtual Task WriteBitmapAsync(
            Bitmap bitmap, String bitmapPath,
            CancellationToken cancellationToken = default) =>
                FileWriter.WriteBitmapAsync(bitmap, bitmapPath, cancellationToken);

        public virtual Task WriteImageAsync(
            Image image, String imagePath,
            CancellationToken cancellationToken = default) =>
                FileWriter.WriteImageAsync(image, imagePath, cancellationToken);

        public virtual Task WriteJsonAsync<T>(
            T value, String jsonPath,
            JsonSerializerOptions? serializerOptions = default,
            CancellationToken cancellationToken = default) =>
                FileWriter.WriteJsonAsync<T>(
                    value, jsonPath, serializerOptions, cancellationToken);

        public virtual Task WriteStringAsync(
            String value, String stringPath,
            CancellationToken cancellationToken = default) =>
                FileWriter.WriteStringAsync(value, stringPath, cancellationToken);
    }

    public record FileManagerOptions : IFileManagerOptions
    {
        public Boolean CreateDirectories { get; init; } =
            IFileManagerOptions.DefaultCreateDirectories;

        public Boolean WarnOnOverwrite { get; init; } =
            IFileManagerOptions.DefaultWarnOnOverwrite;
    }
}