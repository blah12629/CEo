using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CEo.IO
{
    public interface IFileWriter
    {
        Task WriteBitmapAsync(
            Bitmap bitmap, String bitmapPath,
            CancellationToken cancellationToken = default);

        Task WriteImageAsync(
            Image image, String imagePath,
            CancellationToken cancellationToken = default);

        Task WriteJsonAsync<T>(
            T value, String jsonPath,
            JsonSerializerOptions? serializerOptions = default,
            CancellationToken cancellationToken = default);

        Task WriteStringAsync(
            String value, String stringPath,
            CancellationToken cancellationToken = default);
    }

    public interface IFileWriterOptions : IFileCheckerOptions, IDirectoryManagerOptions
    {
        internal const Boolean DefaultCreateDirectories = true,
            DefaultWarnOnOverwrite = false;

        Boolean CreateDirectories { get; }
        Boolean WarnOnOverwrite { get; }
    }

    public class FileWriter : IFileWriter
    {
        public FileWriter(
            IFileWriterOptions? options = default,
            ILogger? logger = default) :
                this(new FileSystem(), options, logger) { }
        public FileWriter(
            IFileSystem fileSystem,
            IFileWriterOptions? options = default,
            ILogger? logger = default)
        {
            (Options, Logger) = (options ?? new FileWriterOptions(), logger);
            FileSystem = fileSystem;
            FileChecker = new FileChecker(FileSystem, Options, Logger);
            DirectoryManager = new DirectoryManager(FileSystem, Options, Logger);
        }
        public FileWriter(
            IFileSystem fileSystem,
            IFileChecker? fileChecker = default,
            IDirectoryManager? directoryManager = default,
            IFileWriterOptions? options = default,
            ILogger? logger = default) :
                this(fileSystem, options, logger)
        {
            if (fileChecker != default) FileChecker = fileChecker;
            if (directoryManager != default) DirectoryManager = directoryManager;
        }

        protected IFileSystem FileSystem { get; }
        protected IFileChecker FileChecker { get; }
        protected IDirectoryManager DirectoryManager { get; }
        protected IFileWriterOptions Options { get; }
        protected ILogger? Logger { get; }

        protected internal virtual Stream OpenWrite(String filePath)
        {
            if (DirectoryManager.DirectoryExists(filePath) ||
                filePath.EndsWith(FileSystem.Path.DirectorySeparatorChar) ||
                filePath.EndsWith(FileSystem.Path.AltDirectorySeparatorChar))
                    throw new ArgumentException($"Cannot open a directory as a file.");

            if (Options.CreateDirectories)
                DirectoryManager.CreateDirectory(
                    FileSystem.Path.GetDirectoryName(filePath) ?? String.Empty);

            // UNTESTED
            if (Options.WarnOnOverwrite && FileChecker.FileExists(filePath))
                Logger?.LogWarning(
                    $"File `{filePath}` already exists. " +
                    "Overwriting may occur.");

            return FileSystem.File.OpenWrite(filePath);
        }

        // UNTESTED: Output contents
        public virtual async Task WriteBitmapAsync(
            Bitmap bitmap, String bitmapPath,
            CancellationToken cancellationToken = default)
        {
            using var memory = new MemoryStream();
            if (bitmap.RawFormat.Guid == ImageFormat.MemoryBmp.Guid)
                bitmap.Save(memory, ImageFormat.Bmp);
            else bitmap.Save(memory, bitmap.RawFormat);

            using var file = OpenWrite(bitmapPath);
            await file.WriteAsync(memory.ToArray(), cancellationToken);
        }

        // UNTESTED: Output contents
        public virtual Task WriteImageAsync(
            Image image, String imagePath,
            CancellationToken cancellationToken = default) =>
                WriteBitmapAsync((Bitmap)image, imagePath, cancellationToken);

        public virtual async Task WriteJsonAsync<T>(
            T value, String jsonPath,
            JsonSerializerOptions? serializerOptions = default,
            CancellationToken cancellationToken = default)
        {
            using var file = OpenWrite(jsonPath);
            await JsonSerializer.SerializeAsync<T>(
                file, value, serializerOptions, cancellationToken);
        }

        public virtual async Task WriteStringAsync(
            String value, String stringPath,
            CancellationToken cancellationToken = default)
        {
            using var file = OpenWrite(stringPath);
            using var writer = new StreamWriter(file);
            await writer.WriteAsync(value.AsMemory(), cancellationToken);
        }
    }

    public record FileWriterOptions : IFileWriterOptions
    {
        public Boolean CreateDirectories { get; init; } =
            IFileWriterOptions.DefaultCreateDirectories;

        public Boolean WarnOnOverwrite { get; init; } =
            IFileWriterOptions.DefaultWarnOnOverwrite;
    }
}