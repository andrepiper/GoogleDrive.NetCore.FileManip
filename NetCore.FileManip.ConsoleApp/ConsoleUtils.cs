using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCore.FileManip.ConsoleApp
{
    public class ConsoleUtils
    {
        public MemoryStream downloadPDF()
        {
            var netClient = new System.Net.WebClient();
            var data = netClient.DownloadData(new Uri("https://www.sagicorjamaica.com/Forms/Banking/SagicorBank_LoanApplication.pdf"));
            return new System.IO.MemoryStream(data);
        }
    }
}
