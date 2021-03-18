using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CEo.IO
{
    public interface IFileManager : IFileReader, IFileWriter { }

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
            ILogger? logger = default)
        {
            (Options, Logger) = (options ?? new FileManagerOptions(), logger);
            FileSystem = fileSystem;

            var fileChecker = new FileChecker(FileSystem, Options, Logger);
            FileReader = new FileReader(FileSystem, fileChecker, Options, Logger);
            FileWriter = new FileWriter(FileSystem, fileChecker, default, Options, Logger);
        }

        protected IFileSystem FileSystem { get; }
        protected IFileReader FileReader { get; }
        protected IFileWriter FileWriter { get; }
        protected IFileManagerOptions Options { get; }
        protected ILogger? Logger { get; }

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