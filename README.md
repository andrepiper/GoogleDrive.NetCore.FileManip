# GoogleDrive.NetCore.FileManip
Google Drive files manipulator library (.NET CORE)

Embed Google Drive in your .NET core application when handling files in your backend applications. 
Google gives every user 15 GB of free storage for small startups, having that amount of free fast and secure storage is a major win. I wanted to store reports in daily, monthly and yearly folder for team mates to access on the fly from any device that has a Google Drive application installed.

* Creating a JSON service account file
     *  A. Create a new API project at https://console.developers.google.com/.
     *  B. Go to the Google API credentials page, click to “Create credentials” and select Service account key.
     *  C. On the same credentials page, click to manage service accounts.
     *  D. Create a new service account if you don’t already have one.
     *  E. Click to create a new key for this account, select JSON and it should download a JSON credential file for you.
     *  F. Save the credentials file to the root of your project (or subfolder) and in Visual Studio set the property Copy to Output Path to        “Copy Always”. Note that you shouldn’t make this file publicly available if it’s being used in your solution.
      * G. Copy the service account user’s email address and add give it permission to any folder of your choice. I call that the master              folder. Think of it like the equivalent of a C:\ drive. :-)
      
See sample usage below, using DI :

```
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine($"Process started {DateTime.Now}");

            var masterFolderId = "XYZ_gAh4hzGItFTniz4Xtc4_UEdXY9O0UF";
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
                .AddSingleton<IGoogleService>(new GoogleService(new GoogleAuthenticationServices("auth_file.json"), driveScope))
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
    
```
