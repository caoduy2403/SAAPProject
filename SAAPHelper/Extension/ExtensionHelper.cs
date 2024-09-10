using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace SAAPHelper.Constant
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// https://stackoverflow.com/questions/12834059/how-to-convert-datatable-to-object-type-list-in-c-sharp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> GetListByDataTable<T>(DataTable dt)
        {
            try
            {
                var result = (from row in dt.AsEnumerable()
                              select GetItem<T>(row)
                             ).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ExtensionHelper.GetListByDataTable", ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Convert List to Datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// https://ironpdf.com/blog/net-help/csharp-datatable-to-list-guide/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn col in dr.Table.Columns)
            {
                var prop = obj.GetType().GetProperty(col.ColumnName);
                if (prop != null && dr[col] != DBNull.Value)
                    prop.SetValue(obj, dr[col]);
            }
            return obj;
        }
    }
}
