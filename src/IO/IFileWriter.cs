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
        internal const Boolean DefualtCreateDirectories = true,
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

        protected IFileSystem FileSystem { get; }
        protected IFileChecker FileChecker { get; }
        protected IDirectoryManager DirectoryManager { get; }
        protected IFileWriterOptions Options { get; }
        protected ILogger? Logger { get; }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="filePath">
        ///   <para></para>
        /// </param>
        /// <returns>
        ///   <para>
        ///     Null when <see cref="filePath" /> is a directory.
        ///   </para>
        /// </returns>
        protected internal virtual FileStream? OpenRead(String filePath)
        {
            if (DirectoryManager.DirectoryExists(filePath)) return default;

            if (Options.CreateDirectories)
            {
                // May return null
                var directoryPath = Path.GetDirectoryName(filePath);
            }

            // File.OpenRead()
            throw new NotImplementedException();
        }

        public virtual Task WriteBitmapAsync(
            Bitmap bitmap, String bitmapPath,
            CancellationToken cancellationToken = default) =>
                throw new NotImplementedException();

        public virtual Task WriteImageAsync(
            Image image, String imagePath,
            CancellationToken cancellationToken = default) =>
                throw new NotImplementedException();

        public virtual Task WriteJsonAsync<T>(
            T value, String jsonPath,
            JsonSerializerOptions? serializerOptions = default,
            CancellationToken cancellationToken = default) =>
                throw new NotImplementedException();

        public virtual Task WriteStringAsync(
            String value, String stringPath,
            CancellationToken cancellationToken = default) =>
                throw new NotImplementedException();
    }

    public record FileWriterOptions : IFileWriterOptions
    {
        public Boolean CreateDirectories { get; init; } =
            IFileWriterOptions.DefualtCreateDirectories;

        public Boolean WarnOnOverwrite { get; init; } =
            IFileWriterOptions.DefaultWarnOnOverwrite;
    }
}

// using Microsoft.Extensions.Logging;
// using System;
// using System.Drawing;
// using System.Drawing.Imaging;
// using System.IO;
// using System.Text.Json;
// using System.Threading;
// using System.Threading.Tasks;

// namespace CEo.IO
// {
//     public interface IFileWriter
//     {
//         Task WriteBitmapAsync(
//             Bitmap bitmap, String bitmapPath,
//             CancellationToken cancellationToken = default);

//         Task WriteImageAsync(
//             Image image, String imagePath,
//             CancellationToken cancellationToken = default);

//         Task WriteJsonAsync<T>(
//             T value, String jsonPath,
//             JsonSerializerOptions serializerOptions = default,
//             CancellationToken cancellationToken = default);

//         Task WriteStringAsync(
//             String value, String stringPath,
//             CancellationToken cancellationToken = default);
//     }

//     internal class FileWriter : IFileWriter
//     {
//         public FileWriter(IFileWriterSettings settings, ILogger logger)
//         {
//             (Settings, Logger) = (settings ?? new FileWriterSettings(), logger);
//             FileChecker = new FileChecker(Logger);
//         }
//         public FileWriter(
//             IFileChecker fileChecker,
//             IFileWriterSettings settings,
//             ILogger logger) :
//                 this(settings, logger) =>
//                     FileChecker = fileChecker;

//         protected IFileChecker FileChecker { get; }
//         protected IFileWriterSettings Settings { get; }
//         protected ILogger Logger { get; }

//         // Test:
//         // - Empty filePath, null filePath, nonexistentFilePath
//         // - Test if creates directory when options is true/false
//         protected virtual FileStream OpenWrite(String filePath)
//         {
//             var directoryName = Path.GetDirectoryName(filePath);

//             if (!Directory.Exists(directoryName)) {
//                 Logger?.LogWarning($"Directory `{ directoryName }` does not exist.");

//                 if (Settings.CreateDirectoriesOnWrite) {
//                     var directoryFullName = Directory
//                         .CreateDirectory(directoryName).FullName;

//                     Logger?.LogDebug($"Directory `{ directoryFullName }` created.");
//                 }
//             }

//             if (File.Exists(filePath) && Settings.WarnOnOverwrite)
//                 Logger?.LogWarning($"File `{ filePath }` already exists. Overwriting may occur.");

//             return File.OpenWrite(filePath);
//         }

//         // Tests:
//         // - Corrupt bitmap object
//         // - ImageFormat.MemoryBmp will throw a null exception for the encoder
//         public virtual async Task WriteBitmapAsync(
//             Bitmap bitmap, String bitmapPath,
//             CancellationToken cancellationToken = default)
//         {
//             using var memory = new MemoryStream();
//             if (bitmap.RawFormat.Guid == ImageFormat.MemoryBmp.Guid)
//                 bitmap.Save(memory, ImageFormat.Bmp);
//             else bitmap.Save(memory, bitmap.RawFormat);

//             using var file = OpenWrite(bitmapPath);
//             await file.WriteAsync(memory.ToArray(), cancellationToken);
//         }

//         public virtual Task WriteImageAsync(
//             Image image, String imagePath,
//             CancellationToken cancellationToken = default) =>
//                 WriteBitmapAsync((Bitmap)image, imagePath, cancellationToken);

//         // Tests:
//         // - null `value`
//         public virtual async Task WriteJsonAsync<T>(
//             T value, String jsonPath,
//             JsonSerializerOptions serializerOptions = default,
//             CancellationToken cancellationToken = default)
//         {
//             using var file = OpenWrite(jsonPath);
//             await JsonSerializer
//                 .SerializeAsync<T>(file, value, serializerOptions, cancellationToken);
//         }

//         public virtual async Task WriteStringAsync(
//             String value, String stringPath,
//             CancellationToken cancellationToken = default)
//         {
//             using var file = OpenWrite(stringPath);
//             using var writer = new StreamWriter(file);
//             await writer.WriteAsync(value.AsMemory(), cancellationToken);
//         }
//     }
// }