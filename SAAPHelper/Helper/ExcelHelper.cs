﻿using SAAPHelper.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SAAPHelper.Models;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace SAAPHelper.Helper
{
    public class ExcelHelper
    {
        public static void SaveAs(string pFileName, List<DataExcelModel> list)
        {
            try
            {
                string fileName = String.Empty;
                string[] text = pFileName.Split('-');
                fileName = text[0];

                XLWorkbook workbook = new XLWorkbook();
                DataTable dt = list.ToDataTable();

                dt.TableName = fileName;
                workbook.Worksheets.Add(dt);
                string excelPath = Path.Combine(Constants.pathFolder, fileName + ".xlsx");
                workbook.SaveAs(excelPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ExcelHelper.SaveAs ", ex.ToString());
            }
        }
    }
}
