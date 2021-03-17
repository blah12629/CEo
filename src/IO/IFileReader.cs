using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CEo.IO
{
    public interface IFileReader : IFileChecker
    {
        Task<Bitmap> ReadBitmapAsync(
            String bitmapPath,
            CancellationToken cancellationToken = default);

        Task<Image> ReadImageAsync(
            String imagePath,
            CancellationToken cancellationToken = default);

        Task<T> ReadJsonAsync<T>(
            String jsonPath,
            JsonSerializerOptions? serializerOptions = default,
            CancellationToken cancellationToken = default);

        Task<String> ReadStringAsync(
            String stringPath,
            CancellationToken cancellationToken = default);
    }

    public interface IFileReaderOptions : IFileCheckerOptions { }

    public class FileReader : IFileReader
    {
        public FileReader(
            IFileReaderOptions? options = default,
            ILogger? logger = default) :
                this(new FileSystem(), options, logger) { }
        public FileReader(
            IFileSystem fileSystem,
            IFileReaderOptions? options = default,
            ILogger? logger = default)
        {
            (Options, Logger) = (options ?? new FileReaderOptions(), logger);
            FileSystem = fileSystem;
            FileChecker = new FileChecker(FileSystem, Options, Logger);
        }
        public FileReader(
            IFileSystem fileSystem,
            IFileChecker fileChecker,
            IFileReaderOptions? options = default,
            ILogger? logger = default) :
                this(fileSystem, options, logger) =>
                    FileChecker = fileChecker;

        protected IFileSystem FileSystem { get; }
        protected IFileChecker FileChecker { get; }
        protected IFileReaderOptions Options { get; }
        protected ILogger? Logger { get; }

        public virtual Boolean FileExists(String filePath) =>
            FileChecker.FileExists(filePath);

        protected internal virtual Stream OpenRead(String filePath) =>
            FileSystem.File.OpenRead(filePath);

        public virtual async Task<Bitmap> ReadBitmapAsync(
            String bitmapPath,
            CancellationToken cancellationToken = default) =>
                (Bitmap)await ReadImageAsync(bitmapPath, cancellationToken);

        public virtual async Task<Image> ReadImageAsync(
            String imagePath,
            CancellationToken cancellationToken = default)
        {
            using var memory = new MemoryStream();

            using (var file = OpenRead(imagePath))
                await file.CopyToAsync(memory, cancellationToken);

            try
            {
                return Image.FromStream(memory);
            }
            catch(ArgumentException)
            {
                Logger?.LogError($"File `{ imagePath }` cannot be read as an image.");
                throw;
            }
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="jsonPath">
        ///   <para></para>
        /// </param>
        /// <param name="serializerOptions">
        ///   <para></para>
        /// </param>
        /// <param name="cancellationToken">
        ///   <para></para>
        /// </param>
        /// <typeparam name="T">
        ///   <para></para>
        /// </typeparam>
        /// <returns>
        ///   <remarks>
        ///     Will return null if the content of the file is null.
        ///   </remarks>
        /// </returns>
        public virtual async Task<T?> ReadJsonAsync<T>(
            String jsonPath,
            JsonSerializerOptions? serializerOptions = default,
            CancellationToken cancellationToken = default)
        {
            using var file = OpenRead(jsonPath);

            try
            {
                return await JsonSerializer.DeserializeAsync<T>(
                    file, serializerOptions, cancellationToken);
            }
            catch(Exception exception)
            {
                if (exception is JsonException || exception is NotSupportedException)
                    Logger?.LogError(
                        $@"File `{jsonPath}` cannot be " +
                        "deserialized to `{typeof(T).Name}`.");
                throw;
            }

        }

        // UNTESTED
        public virtual async Task<String> ReadStringAsync(
            String stringPath,
            CancellationToken cancellationToken = default)
        {
            using var file = OpenRead(stringPath);
            using var reader = new StreamReader(file);
            return await reader.ReadToEndAsync();
        }
    }

    public record FileReaderOptions : IFileReaderOptions { }
}