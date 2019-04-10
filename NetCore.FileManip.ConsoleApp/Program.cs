using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCore.FileManip.Lib.Services;
using NetCore.FileManip.Lib.Services.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;

namespace NetCore.FileManip.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine($"Process started {DateTime.Now}");

            var masterFolderId = "18ogAh4hzGItFTniz4Xtc4_UEdXY9O0UF";
            string[] driveScope =
            {
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile,
                DriveService.Scope.DriveAppdata,
                DriveService.Scope.DriveMetadata,
                DriveService.Scope.DriveMetadataReadonly,
                DriveService.Scope.DrivePhotosReadonly,
                DriveService.Scope.DriveScripts,
                DriveService.Scope.DriveReadonly
            };

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IGoogleService>(new GoogleService(new GoogleAuthenticationServices("auth.json"), driveScope))
                .BuildServiceProvider();

            //acess DI container
            var googleService = serviceProvider.GetService<IGoogleService>();

            //creating a folder
            var childFolder = await googleService.DriveInstance().Create(new File()
            {
                Name = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}",
                MimeType = "application/vnd.google-apps.folder",
                Description = $"Folder desc",
                Parents = new List<string>() { masterFolderId }
            });
            //upload file to master folder
            var pdfStream = new ConsoleUtils().downloadPDF();
            var pdfFileInMasterFolder = await googleService.DriveInstance().Upload(new File()
            {
                Name = "file.pdf",
                Parents = new List<string>() { masterFolderId }
            }, pdfStream, "application/pdf");
            //upload same file to child folder
            pdfStream = new ConsoleUtils().downloadPDF();
            var pdfFileInChildFolder = await googleService.DriveInstance().Upload(new File()
            {
                Name = "file.pdf",
                Parents = new List<string>() { childFolder.Id }
            }, pdfStream, "application/pdf");
            //create a new folder and copy file to another folder
            var childFolderCopy = await googleService.DriveInstance().Create(new File()
            {
                Name = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_BAK",
                MimeType = "application/vnd.google-apps.folder",
                Description = $"Folder desc",
                Parents = new List<string>() { masterFolderId }
            });
            var pdfFileCopied = await googleService.DriveInstance().Copy(pdfFileInChildFolder.Id, new File()
            {
                Name = "file_BAK.pdf",
                Parents = new List<string>() { childFolderCopy.Id }
            });

            Console.WriteLine($"Process finished {DateTime.Now}");
            Console.WriteLine($"Press enter to close application");
            Console.ReadLine();
        }
    }
}
