﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Data;
using SAAPHelper.Helper;
using System.ComponentModel;
using System.Net;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;
using SAAPHelper.Constant;
using SAAPHelper.Models;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace SAAPHelper
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            //string value = "xSQL = xSQL & \" SELECT @sql += N'EXEC sp_refreshview ''' + [name] + N'''' + CHAR(10) FROM objects_schema\" & 'vbCrLf";
            string value = "\"File name 「\" & xRS!File_Name & \"」 is duplicated. Please select again.\"";
            //string Val = "「\"N'\" & INDATA & \"'\"」";

            var data = GetSpecialCharacters(value);
            //int IdxCmt = FuncHelper.GetStartIdxCommented(value);
            //string firstName  = CharactersHelper.CapitalizeFirstLetter("AAA 1234    CCCC 1");

            //JapanFileHelper.ConvertComment();


            string[] fileArray = Directory.GetFiles(Constants.pathFolder, "*.txt");
            foreach (string fileUrl in fileArray)
            {
                if (!fileUrl.Contains(Constants.OutputName) && !fileUrl.Contains(Constants.ConvertName))
                {
                    Console.WriteLine("fileName {0}", System.IO.Path.GetFileName(fileUrl));

                    string filename = System.IO.Path.GetFileName(fileUrl);
                    JapanFileHelper.GetJapaneseTextFile(fileUrl);
                    //JapanFileHelper.ConvertFile(fileUrl);
                }
            }

            Console.WriteLine("Successfully ");
            Console.ReadKey();
        }

        

    }
}
