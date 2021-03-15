// using Microsoft.Extensions.Logging;
// using System;
// using System.Drawing;
// using System.IO;
// using System.Text.Json;
// using System.Threading;
// using System.Threading.Tasks;

// namespace CEo.IO
// {
//     public interface IFileReader
//     {
//         Task<Bitmap> ReadBitmapAsync(
//             String bitmapPath,
//             CancellationToken cancellationToken = default);

//         Task<Image> ReadImageAsync(
//             String imagePath,
//             CancellationToken cancellationToken = default);

//         Task<T> ReadJsonAsync<T>(
//             String jsonPath,
//             JsonSerializerOptions serializerOptions = default,
//             CancellationToken cancellationToken = default);

//         Task<String> ReadStringAsync(
//             String stringPath,
//             CancellationToken cancellationToken = default);
//     }

//     internal class FileReader : IFileReader
//     {
//         public FileReader(ILogger logger)
//         {
//             Logger = logger;
//             FileChecker = new FileChecker(Logger);
//         }
//         public FileReader(IFileChecker fileChecker, ILogger logger) : this(logger) =>
//             FileChecker = fileChecker;

//         protected IFileChecker FileChecker { get; }
//         protected ILogger Logger { get; }

//         protected internal virtual FileStream OpenRead(String filePath)
//         {
//             if (filePath == default || !FileChecker.FileExists(filePath))
//             {
//                 Logger?.LogWarning($"File `{ filePath ?? "null" }` does not exists.");
//                 return default;
//             }

//             return File.OpenRead(filePath);
//         }

//         public virtual async Task<Bitmap> ReadBitmapAsync(
//             String bitmapPath,
//             CancellationToken cancellationToken = default) =>
//                 await ReadImageAsync(bitmapPath, cancellationToken) as Bitmap;

//         public virtual async Task<Image> ReadImageAsync(
//             String imagePath,
//             CancellationToken cancellationToken = default)
//         {
//             using var memory = new MemoryStream();

//             using(var file = OpenRead(imagePath))
//                 if (file == default) return default;
//                 else await file.CopyToAsync(memory, cancellationToken);

//             try
//             {
//                 return Image.FromStream(memory);
//             }
//             catch(ArgumentException)
//             {
//                 Logger?.LogError($"File `{ imagePath }` cannot be read as an image.");
//                 return default;
//             }
//         }

//         // Tests:
//         // - Empty file
//         // - Invalid json format
//         // - Uncastable types => probably from a dictionary to an array
//         // - Castable types => Test1 : Test0; json file is Test1, T is Test0
//         public virtual async Task<T> ReadJsonAsync<T>(
//             String jsonPath,
//             JsonSerializerOptions serializerOptions = default,
//             CancellationToken cancellationToken = default)
//         {
//             using var file = OpenRead(jsonPath);
//             if (file == default) return default(T);

//             try
//             {
//                 return await JsonSerializer
//                     .DeserializeAsync<T>(file, serializerOptions, cancellationToken);
//             }
//             catch (Exception exception)
//             {
//                 if (exception is JsonException || exception is NotSupportedException) {
//                     Logger?.LogError($@"File `{
//                         jsonPath }` cannot be deserialized to `{
//                         typeof(T).Name }`.");

//                     return default(T);
//                 }

//                 throw;
//             }
//         }

//         public virtual async Task<String> ReadStringAsync(
//             String stringPath,
//             CancellationToken cancellationToken = default)
//         {
//             using var file = OpenRead(stringPath);
//             if (file == default) return default;

//             using var reader = new StreamReader(file);
//             return await reader.ReadToEndAsync();
//         }
//     }
// }