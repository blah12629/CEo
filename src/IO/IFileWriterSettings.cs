// using static CEo.IO.IFileWriterSettings;
// using System;

// namespace CEo.IO
// {
//     public interface IFileWriterSettings
//     {
//         internal const Boolean DefaultCreateDirectoriesOnWrite = true,
//             DefaultWarnOnOverwrite = true;

//         Boolean CreateDirectoriesOnWrite { get; set; }
//         Boolean WarnOnOverwrite { get; set; }
//     }

//     public class FileWriterSettings : IFileWriterSettings
//     {
//         public Boolean CreateDirectoriesOnWrite { get; set; } = DefaultCreateDirectoriesOnWrite;
//         public Boolean WarnOnOverwrite { get; set; } = DefaultWarnOnOverwrite;
//     }
// }