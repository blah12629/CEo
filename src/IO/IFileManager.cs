// using Microsoft.Extensions.Logging;
// using System;
// using System.Drawing;
// using System.Text.Json;
// using System.Threading;
// using System.Threading.Tasks;

// namespace CEo.IO
// {
//     public interface IFileManager : IFileChecker, IFileReader, IFileWriter { }

//     public class FileManager : IFileManager
//     {
//         public FileManager(IFileManagerSettings settings, ILogger logger)
//         {
//             (Settings, Logger) = (settings ?? new FileManagerSettings(), logger);
//             FileChecker = new FileChecker(Logger);
//             FileReader = new FileReader(FileChecker, Logger);
//             FileWriter = new FileWriter(FileChecker, Settings, Logger);
//         }

//         public FileManager(
//             IFileChecker fileChecker,
//             IFileManagerSettings settings, ILogger logger) :
//                 this(settings, logger)
//         {
//             FileChecker = fileChecker;
//             FileReader = new FileReader(FileChecker, Logger);
//             FileWriter = new FileWriter(FileChecker, Settings, Logger);
//         }

//         protected IFileChecker FileChecker { get; }
//         protected IFileReader FileReader { get; }
//         protected IFileWriter FileWriter { get; }
//         protected IFileManagerSettings Settings { get; }
//         protected ILogger Logger { get; }

//         public virtual Boolean FileExists(String filePath) =>
//             FileChecker.FileExists(filePath);

//         public virtual Task<Bitmap> ReadBitmapAsync(
//             String bitmapPath,
//             CancellationToken cancellationToken = default)    =>
//                 FileReader.ReadBitmapAsync(bitmapPath, cancellationToken);

//         public virtual Task<Image> ReadImageAsync(
//             String imagePath,
//             CancellationToken cancellationToken = default) =>
//                 FileReader.ReadImageAsync(imagePath, cancellationToken);

//         public virtual Task<T> ReadJsonAsync<T>(
//             String jsonPath,
//             JsonSerializerOptions serializerOptions = default,
//             CancellationToken cancellationToken = default)=>
//                 FileReader.ReadJsonAsync<T>(jsonPath, serializerOptions, cancellationToken);

//         public virtual Task<String> ReadStringAsync(
//             String stringPath,
//             CancellationToken cancellationToken = default) =>
//                 FileReader.ReadStringAsync(stringPath, cancellationToken);

//         public virtual Task WriteBitmapAsync(
//             Bitmap bitmap, String bitmapPath,
//             CancellationToken cancellationToken = default) =>
//                 FileWriter.WriteBitmapAsync(bitmap, bitmapPath, cancellationToken);

//         public virtual Task WriteImageAsync(
//             Image image, String imagePath,
//             CancellationToken cancellationToken = default ) =>
//                 FileWriter.WriteImageAsync(image, imagePath, cancellationToken);

//         public virtual Task WriteJsonAsync<T>(
//             T value, String jsonPath,
//             JsonSerializerOptions serializerOptions = default,
//             CancellationToken cancellationToken = default) =>
//                 FileWriter.WriteJsonAsync<T>(value, jsonPath, serializerOptions, cancellationToken);

//         public virtual Task WriteStringAsync(
//             String value, String stringPath,
//             CancellationToken cancellationToken = default) =>
//                 FileWriter.WriteStringAsync(value, stringPath, cancellationToken);
//     }
// }