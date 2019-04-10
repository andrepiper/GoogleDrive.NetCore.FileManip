using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;

namespace NetCore.FileManip.Lib.Services.Implementation
{
    public class GoogleDriveFunctions : IGoogleDriveFunctions
    {
        private readonly DriveService _service;

        public GoogleDriveFunctions(DriveService service)
        {
            _service = service;
        }

        public DriveService GetServiceObject()
        {
            return _service;
        }

        public class FilesCopyOptionalParms
        {
            /// Whether to ignore the domain's default visibility settings for the created file. Domain administrators can choose to make all uploaded files visible to the domain by default; this parameter bypasses that behavior for the request. Permissions are still inherited from parent folders.
            public bool? IgnoreDefaultVisibility { get; set; }
            /// Whether to set the 'keepForever' field in the new head revision. This is only applicable to files with binary content in Drive.
            public bool? KeepRevisionForever { get; set; }
            /// A language hint for OCR processing during image import (ISO 639-1 code).
            public string OcrLanguage { get; set; }
            /// Whether the requesting application supports Team Drives.
            public bool? SupportsTeamDrives { get; set; }

        }

        /// <summary>
        /// Creates a copy of a file and applies any requested updates with patch semantics. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/copy
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="fileId">The ID of the file.</param>
        /// <param name="body">A valid Drive v3 body.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>FileResponse</returns>
        public async Task<File> Upload(File body, MemoryStream stream, string mimetype)
        {
            try
            {
                FilesResource.CreateMediaUpload request;
                // Initial validation.
                var googleIdObject = await GenerateId();
                // Initial validation.
                if (_service == null)
                {
                    throw new ArgumentNullException("service");
                }
                else if (body == null)
                {
                    throw new ArgumentNullException("body");
                }
                else if (googleIdObject == null)
                {
                    throw new ArgumentNullException("id value is null");
                }
                // Building the initial request.
                body.Id = googleIdObject.Ids[0];
                using (stream)
                {
                    request = _service.Files.Create(
                        body,
                        stream,
                        mimetype);

                    request.Fields = "id";

                    await request.UploadAsync();
                }
                body.Id = request.Body.Id;
                return body;
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Copy failed.", ex);
            }
        }

        /// <summary>
        /// Creates a copy of a file and applies any requested updates with patch semantics. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/copy
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="fileId">The ID of the file.</param>
        /// <param name="body">A valid Drive v3 body.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>FileResponse</returns>
        public async Task<File> Copy(string fileId, File body, FilesCopyOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (fileId == null)
                    throw new ArgumentNullException(fileId);

                // Building the initial request.
                var request = _service.Files.Copy(body, fileId);

                // Applying optional parameters to the request.                
                request = (FilesResource.CopyRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(request, optional);

                // Requesting data.
                return await request.ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Copy failed.", ex);
            }
        }

        public class FilesCreateOptionalParms
        {
            /// Whether to ignore the domain's default visibility settings for the created file. Domain administrators can choose to make all uploaded files visible to the domain by default; this parameter bypasses that behavior for the request. Permissions are still inherited from parent folders.
            public bool? IgnoreDefaultVisibility { get; set; }
            /// Whether to set the 'keepForever' field in the new head revision. This is only applicable to files with binary content in Drive.
            public bool? KeepRevisionForever { get; set; }
            /// A language hint for OCR processing during image import (ISO 639-1 code).
            public string OcrLanguage { get; set; }
            /// Whether the requesting application supports Team Drives.
            public bool? SupportsTeamDrives { get; set; }
            /// Whether to use the uploaded content as indexable text.
            public bool? UseContentAsIndexableText { get; set; }

        }

        /// <summary>
        /// Creates a new file. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/create
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="body">A valid Drive v3 body.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>FileResponse</returns>
        public async Task<File> Create(File body, FilesCreateOptionalParms optional = null)
        {
            try
            {
                var googleIdObject = await GenerateId();
                // Initial validation.
                if (_service == null)
                {
                    throw new ArgumentNullException("service");
                }
                else if (body == null)
                {
                    throw new ArgumentNullException("body");
                }
                else if (googleIdObject == null)
                {
                    throw new ArgumentNullException("id value is null");
                }
                // Building the initial request.
                body.Id = googleIdObject.Ids[0];
                var request = _service.Files.Create(body);

                // Applying optional parameters to the request.                
                request = (FilesResource.CreateRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(request, optional);

                // Requesting data.
                return await request.ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Create failed.", ex);
            }
        }

        public class FilesDeleteOptionalParms
        {
            /// Whether the requesting application supports Team Drives.
            public bool? SupportsTeamDrives { get; set; }

        }

        /// <summary>
        /// Permanently deletes a file owned by the user without moving it to the trash. If the file belongs to a Team Drive the user must be an organizer on the parent. If the target is a folder, all descendants owned by the user are also deleted. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/delete
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="fileId">The ID of the file.</param>
        /// <param name="optional">Optional paramaters.</param>
        public async Task Delete(string fileId, FilesDeleteOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");
                if (fileId == null)
                    throw new ArgumentNullException(fileId);

                // Building the initial request.
                var request = _service.Files.Delete(fileId);

                // Applying optional parameters to the request.                
                request = (FilesResource.DeleteRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(request, optional);

                // Requesting data.
                await request.ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Delete failed.", ex);
            }
        }

        /// <summary>
        /// Permanently deletes all of the user's trashed files. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/emptyTrash
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        public async Task EmptyTrash()
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");

                // Make the request.
                await _service.Files.EmptyTrash().ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.EmptyTrash failed.", ex);
            }
        }

        /// <summary>
        /// Exports a Google Doc to the requested MIME type and returns the exported content. Please note that the exported content is limited to 10MB. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/export
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="fileId">The ID of the file.</param>
        /// <param name="mimeType">The MIME type of the format requested for this export.</param>
        public async Task Export(string fileId, string mimeType)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");
                if (fileId == null)
                    throw new ArgumentNullException(fileId);
                if (mimeType == null)
                    throw new ArgumentNullException(mimeType);

                // Make the request.
                await _service.Files.Export(fileId, mimeType).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Export failed.", ex);
            }
        }

        public class FilesGenerateIdsOptionalParms
        {
            /// The number of IDs to return.
            public int? Count { get; set; }
            /// The space in which the IDs can be used to create new files. Supported values are 'drive' and 'appDataFolder'.
            public string Space { get; set; }

        }

        /// <summary>
        /// Generates a set of file IDs which can be provided in create requests. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/generateIds
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>GeneratedIdsResponse</returns>
        public async Task<GeneratedIds> GenerateId(FilesGenerateIdsOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");

                // Building the initial request.
                var request = _service.Files.GenerateIds();
                request.Count = 1;
                // Applying optional parameters to the request.                
                request = (FilesResource.GenerateIdsRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(request, optional);

                // Requesting data.
                return await request.ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.GenerateIds failed.", ex);
            }
        }

        public class FilesGetOptionalParms
        {
            /// Whether the user is acknowledging the risk of downloading known malware or other abusive files. This is only applicable when alt=media.
            public bool? AcknowledgeAbuse { get; set; }
            /// Whether the requesting application supports Team Drives.
            public bool? SupportsTeamDrives { get; set; }

        }

        /// <summary>
        /// Gets a file's metadata or content by ID. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/get
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="fileId">The ID of the file.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>FileResponse</returns>
        public async Task<File> Get(string fileId, FilesGetOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");
                if (fileId == null)
                    throw new ArgumentNullException(fileId);

                // Building the initial request.
                var request = _service.Files.Get(fileId);

                // Applying optional parameters to the request.                
                request = (FilesResource.GetRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(request, optional);

                // Requesting data.
                return await request.ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Get failed.", ex);
            }
        }

        public class FilesListOptionalParms
        {
            /// Comma-separated list of bodies of items (files/documents) to which the query applies. Supported bodies are 'user', 'domain', 'teamDrive' and 'allTeamDrives'. 'allTeamDrives' must be combined with 'user'; all other values must be used in isolation. Prefer 'user' or 'teamDrive' to 'allTeamDrives' for efficiency.
            public string Corpora { get; set; }
            /// The source of files to list. Deprecated: use 'corpora' instead.
            public string Corpus { get; set; }
            /// Whether Team Drive items should be included in results.
            public bool? IncludeTeamDriveItems { get; set; }
            /// A comma-separated list of sort keys. Valid keys are 'createdTime', 'folder', 'modifiedByMeTime', 'modifiedTime', 'name', 'name_natural', 'quotaBytesUsed', 'recency', 'sharedWithMeTime', 'starred', and 'viewedByMeTime'. Each key sorts ascending by default, but may be reversed with the 'desc' modifier. Example usage: ?orderBy=folder,modifiedTime desc,name. Please note that there is a current limitation for users with approximately one million files in which the requested sort order is ignored.
            public string OrderBy { get; set; }
            /// The maximum number of files to return per page. Partial or empty result pages are possible even before the end of the files list has been reached.
            public int? PageSize { get; set; }
            /// The token for continuing a previous list request on the next page. This should be set to the value of 'nextPageToken' from the previous response.
            public string PageToken { get; set; }
            /// A query for filtering the file results. See the "Search for Files" guide for supported syntax.
            public string Q { get; set; }
            /// A comma-separated list of spaces to query within the corpus. Supported values are 'drive', 'appDataFolder' and 'photos'.
            public string Spaces { get; set; }
            /// Whether the requesting application supports Team Drives.
            public bool? SupportsTeamDrives { get; set; }
            /// ID of Team Drive to search.
            public string TeamDriveId { get; set; }

        }
        /// <summary>
        /// Lists or searches files. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/list
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>FileListResponse</returns>
        public async Task<FileList> List(FilesListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");

                // Building the initial request.
                FilesResource.ListRequest listRequest = _service.Files.List();
                listRequest.Fields = "files(id, name, webViewLink)";

                // Applying optional parameters to the request.                
                listRequest = (FilesResource.ListRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(listRequest, optional);

                // Requesting data.
                var files = await listRequest.ExecuteAsync();
                return files;
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.List failed.", ex);
            }
        }

        public class FilesUpdateOptionalParms
        {
            /// A comma-separated list of parent IDs to add.
            public string AddParents { get; set; }
            /// Whether to set the 'keepForever' field in the new head revision. This is only applicable to files with binary content in Drive.
            public bool? KeepRevisionForever { get; set; }
            /// A language hint for OCR processing during image import (ISO 639-1 code).
            public string OcrLanguage { get; set; }
            /// A comma-separated list of parent IDs to remove.
            public string RemoveParents { get; set; }
            /// Whether the requesting application supports Team Drives.
            public bool? SupportsTeamDrives { get; set; }
            /// Whether to use the uploaded content as indexable text.
            public bool? UseContentAsIndexableText { get; set; }

        }

        /// <summary>
        /// Updates a file's metadata and/or content with patch semantics. 
        /// Documentation https://developers.google.com/drive/v3/reference/files/update
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="fileId">The ID of the file.</param>
        /// <param name="body">A valid Drive v3 body.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>FileResponse</returns>
        public async Task<File> Update(string fileId, File body, FilesUpdateOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (fileId == null)
                    throw new ArgumentNullException(fileId);

                // Building the initial request.
                var request = _service.Files.Update(body, fileId);

                // Applying optional parameters to the request.                
                request = (FilesResource.UpdateRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Update failed.", ex);
            }
        }

        public class FilesWatchOptionalParms
        {
            /// Whether the user is acknowledging the risk of downloading known malware or other abusive files. This is only applicable when alt=media.
            public bool? AcknowledgeAbuse { get; set; }
            /// Whether the requesting application supports Team Drives.
            public bool? SupportsTeamDrives { get; set; }

        }

        /// <summary>
        /// Subscribes to changes to a file 
        /// Documentation https://developers.google.com/drive/v3/reference/files/watch
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service.</param>  
        /// <param name="fileId">The ID of the file.</param>
        /// <param name="body">A valid Drive v3 body.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>ChannelResponse</returns>
        public async Task<Channel> Watch(string fileId, Channel body, FilesWatchOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (_service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (fileId == null)
                    throw new ArgumentNullException(fileId);

                // Building the initial request.
                var request = _service.Files.Watch(body, fileId);

                // Applying optional parameters to the request.                
                request = (FilesResource.WatchRequest)GoogleDriveFunctionsHelper.ApplyOptionalParms(request, optional);

                // Requesting data.
                return await request.ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Watch failed.", ex);
            }
        }

    }

    public static class GoogleDriveFunctionsHelper
    {

        /// <summary>
        /// Using reflection to apply optional parameters to the request.  
        /// 
        /// If the optonal parameters are null then we will just return the request as is.
        /// </summary>
        /// <param name="request">The request. </param>
        /// <param name="optional">The optional parameters. </param>
        /// <returns></returns>
        public static object ApplyOptionalParms(object request, object optional)
        {
            if (optional == null)
                return request;

            System.Reflection.PropertyInfo[] optionalProperties = (optional.GetType()).GetProperties();

            foreach (System.Reflection.PropertyInfo property in optionalProperties)
            {
                // Copy value from optional parms to the request.  They should have the same names and datatypes.
                System.Reflection.PropertyInfo piShared = (request.GetType()).GetProperty(property.Name);
                if (property.GetValue(optional, null) != null) // TODO Test that we do not add values for items that are null
                    piShared.SetValue(request, property.GetValue(optional, null), null);
            }

            return request;
        }
    }
}
