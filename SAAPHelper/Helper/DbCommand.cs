using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SAAPHelper.Helper
{
    public static class DbCommand
    {
        private static string _strConnection = "Data Source = azipdevsql.database.windows.net;Initial Catalog = VietnamSAAP; User Id = vietnamsaapuser;Password = d8AmJgXxvv9Z;";

        /// <summary>
        ///  Execute Table
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetWithCommand(string sql)
        {

            using (SqlConnection conn = new SqlConnection(_strConnection))
            {
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    da.SelectCommand.CommandTimeout = 120;
                    DataSet ds = new DataSet("DataSet");
                    da.Fill(ds);

                    return ds;
                }
            }
        }

        /// <summary>
        /// Execute DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTableWithCommand(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_strConnection))
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.SelectCommand.CommandTimeout = 120;
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        return ds.Tables[0];

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DbCommand] [ExecuteDataTableWithCommand] [{0}]", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql)
        {

            using (SqlConnection conn = new SqlConnection(_strConnection))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 120;
                    object result = cmd.ExecuteScalar();
                    conn.Close();

                    return result;
                }
            }
        }

        public static object ExecuteScalarReplace(string txtConvert, string before_name, string after_name)
        {

            using (SqlConnection conn = new SqlConnection(_strConnection))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SAAP_AName_Replace_V3", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@textConvert", SqlDbType.NVarChar).Value = txtConvert;
                cmd.Parameters.AddWithValue("@before_name", SqlDbType.NVarChar).Value = before_name;
                cmd.Parameters.AddWithValue("@after_name", SqlDbType.NVarChar).Value = after_name;
                object result = cmd.ExecuteScalar();
                conn.Close();

                return result;
            }
        }
    }
}
