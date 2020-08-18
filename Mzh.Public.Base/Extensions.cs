using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    public static class MzhExtensions
    {
        /// <summary>
        /// DataTable通过反射获取单个对象
        /// </summary>
        public static T ToSingleModel<T>(this DataTable data) where T : new()
        {
            if (data != null && data.Rows.Count > 0 && data.Rows.Count < 2)
                return data.GetList<T>(null, true).Single();
            return default(T);
        }
        public static List<T> GetList<T>(this DataTable data, string prefix, bool ignoreCase = true) where T : new()
        {
            List<T> t = new List<T>();
            int columnscount = data.Columns.Count;
            if (ignoreCase)
            {
                for (int i = 0; i < columnscount; i++)
                    data.Columns[i].ColumnName = data.Columns[i].ColumnName.ToUpper();
            }
            
            try
            {
                var properties = new T().GetType().GetProperties();

                var rowscount = data.Rows.Count;
                for (int i = 0; i < rowscount; i++)
                {
                    var model = new T();
                    foreach (var p in properties)
                    {
                        var keyName = prefix + p.Name + "";
                        if (ignoreCase)
                            keyName = keyName.ToUpper();
                        for (int j = 0; j < columnscount; j++)
                        {
                            if (data.Columns[j].ColumnName == keyName && data.Rows[i][j] != null)
                            {
                                string pval = data.Rows[i][j].ToString();
                                if (!string.IsNullOrEmpty(pval))
                                {
                                    try
                                    {
                                        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                        {
                                            p.SetValue(model, Convert.ChangeType(data.Rows[i][j], p.PropertyType.GetGenericArguments()[0]), null);
                                        }
                                        else
                                        {
                                            p.SetValue(model, Convert.ChangeType(data.Rows[i][j], p.PropertyType), null);
                                        }
                                    }
                                    catch (Exception x)
                                    {
                                        throw x;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    t.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }

        public static T TransObjFromTable<T>(this DataRow dr) where T : new()
        {
            Type target = typeof(T);
            var model = new T();
            foreach (var pp in target.GetProperties())
            {
                try
                {
                    pp.SetValue(model, dr.Field<object>(pp.Name));
                }
                catch
                {
                    continue;
                    //Log.Exception(ex.Message, ex);
                }
            }
            return model;
        }

        public static DateTime MonDay(this DateTime time)
        {
            var weekday = time.DayOfWeek;
            if(weekday == 0)
            {
                return time.AddDays(-6).Date;
            }
            else
            {
                return time.AddDays(-1*((int)weekday-1)).Date;
            }
        }

        public static DateTime FirstInMonth(this DateTime time)
        {
            return time.AddDays(1 - time.Day).Date;
        }
    }
}
