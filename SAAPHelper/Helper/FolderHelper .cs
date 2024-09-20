using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using SAAPHelper.Constant;

namespace SAAPHelper.Helper
{
    public class FolderHelper
    {
        public static bool FileExists(string pUrl = "")
        {
            string _url = Constants.pathFolder;

            if (!string.IsNullOrEmpty(pUrl))
            {
                _url = pUrl;
            }

            bool result = File.Exists(_url);

            return result;
        }

        public static void DeleteAllFiles(string pUrl = "")
        {
            string[] fileArray = Directory.GetFiles(Constants.pathFolder, "*.txt");
            foreach (string fileUrl in fileArray)
            {
                Console.WriteLine("File Name:  {0}", fileUrl);
                if (File.Exists(fileUrl))
                {
                    File.Delete(fileUrl);
                }
            }
        }
        
    }
}
