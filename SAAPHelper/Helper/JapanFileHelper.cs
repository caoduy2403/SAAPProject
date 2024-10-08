﻿using SAAPHelper.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SAAPHelper.Models;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace SAAPHelper.Helper
{
    public class JapanFileHelper
    {

        public static void HandleJapaneseTextFile(bool isTranslated = false)
        {
            DateTime StartTime = DateTime.Now;
            Console.WriteLine("====================[HandleJapaneseTextFile - START]==================== [{0}] \n", StartTime.ToString("MM/dd/yyyy hh:mm:ss"));

            string[] fileArray = Directory.GetFiles(Constants.pathFolder, "*.txt");
            foreach (string fileUrl in fileArray)
            {
                if (!fileUrl.Contains(Constants.OutputName)
                    && !fileUrl.Contains(Constants.ConvertName)
                    && !fileUrl.Contains(Constants.CommentName))
                {
                    string filename = System.IO.Path.GetFileName(fileUrl);
                    GetJapaneseTextFile(fileUrl, isTranslated);
                }
            }

            DateTime EndTime = DateTime.Now;
            Console.WriteLine("=====================[HandleJapaneseTextFile - END]===================== [{0}] [{1}] ", EndTime.ToString("MM/dd/yyyy hh:mm:ss"), (DateTime.Now - StartTime));
        }

        public static void HandleConvertFile()
        {
            DateTime StartTime = DateTime.Now;
            Console.WriteLine("====================[HandleConvertFile - START]==================== [{0}] \n", StartTime.ToString("MM/dd/yyyy hh:mm:ss"));
           
            string[] fileArray = Directory.GetFiles(Constants.pathFolder, "*.txt");
            foreach (string fileUrl in fileArray)
            {
                if (!fileUrl.Contains(Constants.OutputName) 
                    && !fileUrl.Contains(Constants.ConvertName) 
                    && !fileUrl.Contains(Constants.CommentName))
                {
                    Console.WriteLine("File Name [{0}]", fileUrl);
                    ConvertFile(fileUrl);
                }
            }

            DateTime EndTime = DateTime.Now;
            Console.WriteLine("=====================[HandleConvertFile - END]===================== [{0}] [{1}] ", EndTime.ToString("MM/dd/yyyy hh:mm:ss"), (DateTime.Now - StartTime));
        }

        public static void GetJapaneseTextFile(string fileUrl, bool isTranslated = false)
        {
            string fileUrlname = Path.GetFileName(fileUrl);
            string ext = Path.GetExtension(fileUrl);

            string filename = fileUrlname.Replace(ext, "");
            string OutputFile = filename + Constants.OutputName + ext;

            // Create a Regex
            Regex rg = new Regex(Constants.pattern);

            IEnumerable<string> fileContents = File.ReadLines(fileUrl);
            string outFile = Path.Combine(Constants.pathFolder, OutputFile);

            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            List<string> lstConversion = new List<string>();
            DataTable dtResult = GetANAMEConversion();

            using (StreamWriter writer = new StreamWriter(outFile))
            {
                List<DataExcelModel> dataExcel = new List<DataExcelModel>();
                int LineCode = 0;
                int No = 1;
                DateTime StartTime = DateTime.Now;

                int countLine = fileContents.Count() - 1;

                foreach (string line in fileContents)
                {
                    LineCode++;
                    Console.WriteLine("[File Name] {0} [Line Code] #{1} [TimeSpan] {2}\n", filename, LineCode, (DateTime.Now - StartTime));
                    int currentLineCursor = Console.CursorTop;
                  
                    if (LineCode == countLine)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                    }

                    if (FuncHelper.chkTextIsNotCommented(line))
                    {
                        string txtConvert = line;

                        // get the index from the start of the first comment
                        int IdxCmt = FuncHelper.GetStartIdxCommented(line);
                        string ConvertLast = string.Empty;

                        if (IdxCmt > -1)
                        {
                            txtConvert = txtConvert.Substring(0, IdxCmt);
                        }

                        string[] words = txtConvert.Trim().Split(' ');
                        foreach (string word in words)
                        {
                            string[] text = word.Split(Constants.delimiterChars);
                            foreach (var subline in text)
                            {
                                string sub_text = subline.Trim();
                                int IdxSubcmt = line.IndexOf(sub_text);
                                sub_text = sub_text.Replace("\"'", "");
                                sub_text = sub_text.Replace("' \"", "");
                                sub_text = sub_text.Replace("\"", "");

                                if (!rg.IsMatch(sub_text))
                                {
                                    continue;
                                }

                                //check in database
                                bool isExisted = dtResult.AsEnumerable().Any(x => x.Field<string>("before_name") == sub_text);

                                if (!isExisted) // rg.IsMatch(sub_text) && !isExisted
                                {
                                    //check in current file
                                    bool isExistedConversion = lstConversion.Any(x => x.Equals(sub_text));

                                    //Write to a file
                                    if (!isExistedConversion)
                                    {
                                        lstConversion.Add(sub_text);
                                        string textExcel = sub_text + "\t@EXCEL@" + line;
                                        writer.WriteLine(textExcel);

                                        DataExcelModel dataExcelModel = new DataExcelModel();

                                        string transText = string.Empty;
                                        if (isTranslated)
                                        {
                                            //Add data to Export
                                            transText = TranslateHelper.TranslateText(sub_text);
                                            dataExcelModel.Variable = CharactersHelper.CapitalizeFirstLetterWithVarible(transText);
                                            dataExcelModel.Message = CharactersHelper.CapitalizeFirstLetterWithMessage(transText);
                                        }

                                        dataExcelModel.No = No;
                                        dataExcelModel.JA = sub_text;
                                        dataExcelModel.EN = transText;
                                        dataExcelModel.LineText = line;
                                        dataExcelModel.LineCode = LineCode.ToString();

                                        dataExcel.Add(dataExcelModel);
                                        No++;
                                    }
                                    else
                                    {
                                        var obj = dataExcel.FirstOrDefault(x => x.JA == sub_text);
                                        if (obj != null)
                                        {
                                            obj.LineCode = obj.LineCode + "\r\n" + LineCode.ToString();
                                            obj.LineText = obj.LineText + "\r\n" + line;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                if (dataExcel.Count > 0)
                {
                    //Export to Excel File
                    ExcelHelper.SaveAs(filename, dataExcel);
                }
            }
        }

        public static void ConvertFile(string fileUrl)
        {
            string filename = Path.GetFileName(fileUrl);
            string ext = Path.GetExtension(fileUrl);
            string OutputFile = filename.Replace(ext, "") + Constants.ConvertName + ext;

            // Create a Regex
            Regex rg = new Regex(Constants.pattern);

            IEnumerable<string> fileContents = File.ReadLines(fileUrl);
            string outFile = Path.Combine(Constants.pathFolder, OutputFile);

            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            DataTable dtResult = GetANAMEConversion();
            using (StreamWriter writer = new StreamWriter(outFile))
            {
                int LineCode = 0;
                foreach (string line in fileContents)
                {
                    LineCode++;
                    Console.WriteLine("[ConvertFile] - [File Name] {0} [Line Code] {1}\n", filename, LineCode);
                    int currentLineCursor = Console.CursorTop;
                    Console.SetCursorPosition(0, Console.CursorTop - 2);

                    string txtConvert = string.Empty;
                    txtConvert = line;

                    if (FuncHelper.chkTextIsNotCommented(txtConvert))
                    {
                        int IdxCmt = FuncHelper.GetStartIdxCommented(line);
                        string ConvertLast = string.Empty;

                        if (IdxCmt > -1)
                        {
                            ConvertLast = txtConvert.Substring(IdxCmt);
                            txtConvert = txtConvert.Substring(0, IdxCmt);
                        }

                        foreach (DataRow dr in dtResult.Rows)
                        {
                            string before_name = dr["before_name"].ToString();
                            string after_name = dr["after_name"].ToString();
                            //bool IsReplaced = false;

                            int idxSubtext = line.IndexOf(before_name); //StringComparison.OrdinalIgnoreCase

                            if ((IdxCmt == -1 || (IdxCmt > idxSubtext)) && idxSubtext > -1)
                            {
                                //Regex.Replace(txtConvert, before_name, after_name, RegexOptions.IgnoreCase);
                                txtConvert = txtConvert.Replace(before_name, after_name);
                                //IsReplaced = true;
                            }

                            #region Convert To Half Width
                            ////Convert To Half Width
                            //bool isHalf = JapanCharactersHandler.IsContainedHalfWidth(txtConvert);
                            //bool IsRedWord = Constants._redFlag.Contains(before_name);

                            //if (!IsReplaced || IsRedWord)
                            //{
                            //    bool bConvert_before_name = false;
                            //    bool isHalfbefore_name = JapanCharactersHandler.IsContainedHalfWidth(before_name);

                            //    //check half width text
                            //    if (isHalfbefore_name)
                            //    {
                            //        string full_before_name = JapanCharactersHandler.ConvertHalfToFull(before_name);
                            //        int idxFull = line.IndexOf(full_before_name);

                            //        if ((IdxCmt == -1 || IdxCmt > idxFull) && idxFull > -1)
                            //        {
                            //            txtConvert = txtConvert.Replace(full_before_name, after_name);
                            //            bConvert_before_name = true;
                            //        }
                            //    }

                            //    if (!bConvert_before_name)
                            //    {
                            //        string convert_before_name = JapanCharactersHandler.ConvertFullToHalf(before_name);
                            //        int idxSubConvert = line.IndexOf(convert_before_name);

                            //        if (isHalf)
                            //        {
                            //            if ((IdxCmt == -1 || IdxCmt > idxSubConvert) && idxSubConvert > -1)
                            //            {
                            //                txtConvert = txtConvert.Replace(convert_before_name, after_name);
                            //            }
                            //        }
                            //    }
                            //} 
                            #endregion

                            //if (!rg.IsMatch(txtConvert)) //!isHalf
                            //{
                            //    break;
                            //}
                        }

                        if (!string.IsNullOrEmpty(ConvertLast))
                        {
                            txtConvert = txtConvert + ConvertLast;
                        }
                    }

                    writer.WriteLine(txtConvert);
                }
            }
        }

        public static string GetConversionFileName(string id, string form_name,string file_name = "")
        {
            string result = id + "." + form_name + "_" + file_name + ExtensionFile.TEXT;
            return result;
        }

        public static void ExportFile(ExportModel model)
        {
            string listID = model.listID;
            bool isBefore = model.isBefore;
            bool isAfter = model.isAfter;
            bool isCommented = model.isCommented;
            bool isTranslatedFile = model.isTranslatedFile;

            DateTime StartTime = DateTime.Now;
            Console.WriteLine("====================[ExportFile - START]==================== [{0}] \n", StartTime.ToString("MM/dd/yyyy hh:mm:ss"));

            DataTable dtConversion = getSAAPConversion(listID);

            foreach (DataRow drConver in dtConversion.Rows)
            {
                string id = drConver["id"].ToString();
                string form_name = drConver["form_name"].ToString();
                string before_source = drConver["Before_source"].ToString();
                string After_source = drConver["After_source"].ToString();
               
                string transText = TranslateHelper.TranslateText(form_name);
                form_name = CharactersHelper.CapitalizeFirstLetter(transText);

                #region Before Name
                //Before Name
                if (isBefore)
                {
                   
                    string beforeName = GetConversionFileName(id, form_name, "BeforeName");
                    string outBeforeName = Path.Combine(Constants.pathFolder, beforeName);

                    Console.WriteLine("[Before Name] {0} \n", beforeName);
                    if (File.Exists(outBeforeName))
                    {
                        File.Delete(outBeforeName);
                    }

                    using (StreamWriter writerBeforeName = new StreamWriter(outBeforeName))
                    {
                        writerBeforeName.WriteLine(before_source);
                    }
                }
                #endregion

                #region After Name
                //After Name
                if (isAfter)
                {
                    string afterName = GetConversionFileName(id, form_name, "AfterName");
                    string outAfterName = Path.Combine(Constants.pathFolder, afterName);

                    Console.WriteLine("[After Name] {0} \n", afterName);

                    if (File.Exists(outAfterName))
                    {
                        File.Delete(outAfterName);
                    }

                    using (StreamWriter writerAfterName = new StreamWriter(outAfterName))
                    {

                        writerAfterName.WriteLine(After_source);
                    }
                }
                #endregion

                #region Replace Commented
                //Replace Commented
                if (isCommented)
                {
                    string filename = Path.GetFileName(form_name);
                    string OutputFile = GetConversionFileName(id, filename, Constants.CommentName);
                    string consoleName = GetConversionFileName(id, filename,string.Empty);

                    Console.WriteLine("[Replace Commented] {0} \n", OutputFile);

                    // Create a Regex
                    Regex rg = new Regex(Constants.pattern);

                    string[] arrBefore = before_source.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
                    string[] fileContents = After_source.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
                    string outFile = Path.Combine(Constants.pathFolder, OutputFile);

                    if (File.Exists(outFile))
                    {
                        File.Delete(outFile);
                    }

                    //DateTime StartTime  = DateTime.Now;

                    DataTable dtResult = GetANAMEConversion();
                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        int LineCode = 0;
                        foreach (string line in fileContents)
                        {
                            //Console.WriteLine("[ExportFile] - [File Name] {0} [Line Code] #{1} [TimeSpan] {2}\n", consoleName, LineCode, (DateTime.Now - StartTime));

                            string txtConvert = string.Empty;
                            string txtBefore = string.Empty;

                            txtConvert = line;
                            txtBefore = arrBefore[LineCode];


                            int IdxCmt = FuncHelper.GetStartIdxCommented(line);
                            int IdxBefore = FuncHelper.GetStartIdxCommented(txtBefore);

                            //&& IdxBefore !=-1
                            if (IdxCmt != -1 && IdxBefore !=-1)
                            {
                                if (IdxCmt == 0)
                                {
                                    txtConvert = txtBefore;
                                }
                                else
                                {
                                    string txtComment = string.Empty;
                                    string txtBeforeComment = string.Empty;

                                    txtComment = txtConvert.Substring(IdxCmt);
                                    txtConvert = txtConvert.Substring(0, IdxCmt);

                                    txtBeforeComment = txtBefore.Substring(IdxBefore);

                                    if (!string.IsNullOrEmpty(txtBeforeComment))
                                    {
                                        txtConvert = txtConvert + txtBeforeComment;
                                    }
                                }
                            }

                            writer.WriteLine(txtConvert);
                            LineCode++;
                        }
                    }
                }

                #endregion

                #region Translate To English
                if (isTranslatedFile)
                {
                    // Create a Regex
                    Regex rg = new Regex(Constants.pattern);
                    string transBeforeName = GetConversionFileName(id, form_name, Constants.ConvertName);
                    string[] fileContents = before_source.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
                    string outFile = Path.Combine(Constants.pathFolder, transBeforeName);

                    if (File.Exists(outFile))
                    {
                        File.Delete(outFile);
                    }

                    DataTable dtResult = GetANAMEConversion();
                    using (StreamWriter writer = new StreamWriter(outFile))
                    {
                        int LineCode = 0;
                        int count = fileContents.Count() - 1;
                        foreach (string line in fileContents)
                        {
                            LineCode++;
                            Console.WriteLine("[ConvertFile] - [File Name] {0} [Line Code] #{1} [TimeSpan] = {2} \n", transBeforeName, LineCode, (DateTime.Now - StartTime));
                            int currentLineCursor = Console.CursorTop;
                            Console.SetCursorPosition(0, Console.CursorTop - 2);

                            //if (LineCode == count)
                            //{
                            //    Console.SetCursorPosition(0, Console.CursorTop - 1);
                            //}
                            //else
                            //{
                            //    Console.SetCursorPosition(0, Console.CursorTop - 2);
                            //}

                            string txtConvert = string.Empty;
                            txtConvert = line;

                            if (FuncHelper.chkTextIsNotCommented(txtConvert))
                            {
                                int IdxCmt = FuncHelper.GetStartIdxCommented(line);
                                string ConvertLast = string.Empty;

                                if (IdxCmt > -1)
                                {
                                    ConvertLast = txtConvert.Substring(IdxCmt);
                                    txtConvert = txtConvert.Substring(0, IdxCmt);
                                }

                                foreach (DataRow dr in dtResult.Rows)
                                {
                                    string before_name = dr["before_name"].ToString();
                                    string after_name = dr["after_name"].ToString();

                                    int idxSubtext = line.IndexOf(before_name); //StringComparison.OrdinalIgnoreCase

                                    if ((IdxCmt == -1 || (IdxCmt > idxSubtext)) && idxSubtext > -1)
                                    {
                                        txtConvert = txtConvert.Replace(before_name, after_name);
                                    }
                                }

                                if (!string.IsNullOrEmpty(ConvertLast))
                                {
                                    txtConvert = txtConvert + ConvertLast;
                                }
                            }

                            writer.WriteLine(txtConvert);
                        }
                    }
                }
                #endregion

                
            }

            DateTime EndTime = DateTime.Now;
            Console.WriteLine("====================[ExportFile - END]===================== [{0}] [{1}] ", EndTime.ToString("MM/dd/yyyy hh:mm:ss"), (DateTime.Now - StartTime));
        }

        private static DataTable getSAAPConversion(string listID = "")
        {
            DataTable dtResult = new DataTable();

            string sql = "";
            sql = sql + " SELECT [ID] ";
            sql = sql + "       ,[form_name] ";
            sql = sql + "       ,[Before_source] ";
            sql = sql + "       ,[After_source] ";
            sql = sql + " FROM [dbo].[ANAME_conversion] ";
            sql = sql + " Where ISnull(After_source,N'') != N'' ";

            if(!string.IsNullOrEmpty(listID))
            {
                string replaceStr = " And [ID] IN (@listID@) ";
                replaceStr = replaceStr.Replace("@listID@", listID);
                sql = sql + replaceStr;
            }
          
            //sql = sql + " And [ID] IN (1,2,3,4,5,8,9,10,12,13,14,15,16,17,18,19,20,21,23,24,25,26,27) ";
            sql = sql + " OrDer By [ID] ";
            dtResult = DbCommand.ExecuteDataTableWithCommand(sql);

            return dtResult;
        }

        private static DataTable GetANAMEConversion()
        {
            DataTable dtResult = new DataTable();

            string sql = string.Empty;
          
            sql = sql + "select [before_name]";
            sql = sql + "		,[after_name]";
            sql = sql + "from [dbo].[SAAP_AName_Conversion]";
            //sql = sql + " Where temp.after_name LIKE N'%and%' ";
            sql = sql + "order by Len(before_name) desc";

            dtResult = DbCommand.ExecuteDataTableWithCommand(sql);

            return dtResult;
        }

        private static string ReplaceJapanese_CS_AS_WS(string txtConvert, string before_name, string after_name)
        {
            object result = new object();

            if (after_name.Contains("_Check") && after_name.Contains("NULL"))
            {
                bool VALE = false;
                if (VALE)
                { 
                }
            }

            //string sql = "Declare @変換前ソース nvarchar(max) \n" +
            //             "Set @変換前ソース = REPLACE(N'@txtConvert@',N'@before_name@'  COLLATE Japanese_CS_AS_WS,N'@after_name@') \n" +
            //             "Select @変換前ソース as result \n";

            //sql = sql.Replace("@txtConvert@", txtConvert);
            //sql = sql.Replace("@before_name@", before_name);
            //sql = sql.Replace("@after_name@", after_name);

            result = DbCommand.ExecuteScalarReplace(txtConvert, before_name, after_name);

            return result.ToString(); 
        }
    }
}
