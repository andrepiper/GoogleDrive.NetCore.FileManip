using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using NetCore.FileManip.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCore.FileManip.Lib.Services.Implementation
{
    public class GoogleAuthenticationServices: IGoogleAuthenticationServices
    {
        private readonly GoogleAuthFile _googleAuthFile;
        private readonly string _authJsonFileLocation;

        /// <summary>
        /// Constructor GoogleDriveService
        /// </summary>
        /// <param name="authJsonFileLocation"></param>
        public GoogleAuthenticationServices(string authJsonFileLocation)
        {
            //not optional 
            if(string.IsNullOrEmpty(authJsonFileLocation))
            {
                throw new Exception("Param authJsonFileLocation cannot be null");
            }
            _authJsonFileLocation = authJsonFileLocation;
            _googleAuthFile = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleAuthFile>(File.ReadAllText(authJsonFileLocation));
        }

        public DriveService AuthenticateDriveAccount(string[] accessScope)
        {
            var service = new DriveService();
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream(_authJsonFileLocation, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                         .CreateScoped(accessScope);
                }
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _googleAuthFile.project_id,
                });
            }
            catch (Exception error)
            {
                throw error;
            }
            return service;
        }
    }
}
