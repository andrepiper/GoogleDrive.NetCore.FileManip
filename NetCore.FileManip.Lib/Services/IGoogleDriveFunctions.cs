using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static NetCore.FileManip.Lib.Services.Implementation.GoogleDriveFunctions;

namespace NetCore.FileManip.Lib.Services
{
    public interface IGoogleDriveFunctions
    {
        Task<File> Copy(string fileId, File body, FilesCopyOptionalParms optional = null);
        Task<File> Create(File body, FilesCreateOptionalParms optional = null);
        Task Delete(string fileId, FilesDeleteOptionalParms optional = null);
        Task EmptyTrash();
        Task Export(string fileId, string mimeType);
        Task<GeneratedIds> GenerateId(FilesGenerateIdsOptionalParms optional = null);
        Task<File> Get(string fileId, FilesGetOptionalParms optional = null);
        Task<FileList> List(FilesListOptionalParms optional = null);
        Task<File> Update(string fileId, File body, FilesUpdateOptionalParms optional = null);
        Task<Channel> Watch(string fileId, Channel body, FilesWatchOptionalParms optional = null);
        DriveService GetServiceObject();
    }
}
