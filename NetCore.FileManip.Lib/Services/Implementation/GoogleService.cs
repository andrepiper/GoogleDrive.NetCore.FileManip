using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.FileManip.Lib.Services.Implementation
{
    public class GoogleService : IGoogleService
    {
        private readonly IGoogleAuthenticationServices _igoogleAuthenticationServices;
        private readonly DriveService _driveService;

        public GoogleService(IGoogleAuthenticationServices googleAuthenticationServices, string[] driveScope)
        {
            _igoogleAuthenticationServices = googleAuthenticationServices;
            if (_igoogleAuthenticationServices == null)
                throw new Exception($"GoogleAuthenticationServices instance is null");
            if (driveScope == null)
                throw new Exception($"No drive scope was specified");
            _driveService = _igoogleAuthenticationServices.AuthenticateDriveAccount(driveScope);
        }

        public GoogleDriveFunctions DriveInstance()
        {
            return new GoogleDriveFunctions(_driveService);
        }
    }
}
