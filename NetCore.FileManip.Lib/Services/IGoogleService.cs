using Google.Apis.Sheets.v4;
using NetCore.FileManip.Lib.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.FileManip.Lib.Services
{
    public interface IGoogleService
    {
        GoogleDriveFunctions DriveInstance();
    }
}
