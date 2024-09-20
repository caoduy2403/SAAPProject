using System;
using System.Text.RegularExpressions;
using System.IO;
using SAAPHelper.Helper;
using SAAPHelper.Constant;

namespace SAAPHelper
{
    internal class Program
    {
       
        static void Main(string[] args)
        {

            DateTime StartTime = DateTime.Now;
            Console.WriteLine("====================[START]==================== [{0}] \n", StartTime.ToString("MM/dd/yyyy hh:mm:ss"));
            
            JapanFileHelper.RemoveComment(false, false, true);

            string[] fileArray = Directory.GetFiles(Constants.pathFolder, "*.txt");
            foreach (string fileUrl in fileArray)
            {
                if (!fileUrl.Contains(Constants.OutputName) && !fileUrl.Contains(Constants.ConvertName))
                {
                    Console.WriteLine("File Name {0}", System.IO.Path.GetFileName(fileUrl));
                    string filename = System.IO.Path.GetFileName(fileUrl);
                    //JapanFileHelper.GetJapaneseTextFile(fileUrl);
                    //JapanFileHelper.ConvertFile(fileUrl);
                }
            }

            DateTime EndTime = DateTime.Now;
            Console.WriteLine("=====================[END]===================== [{0}] [{1}] ", EndTime.ToString("MM/dd/yyyy hh:mm:ss"), (DateTime.Now - StartTime));
            Console.WriteLine("Successfully ");
            Console.ReadKey();
        }
    }
}
