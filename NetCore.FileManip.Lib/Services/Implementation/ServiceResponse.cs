using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.FileManip.Lib.Services.Implementation
{
    public class ServiceResponse<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public Exception Error { get; set; }
    }
}
