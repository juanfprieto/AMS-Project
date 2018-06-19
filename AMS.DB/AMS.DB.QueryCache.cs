using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
//using AMS.DB;
using System.Collections;
using System.Data;

namespace AMS.DB
{
    public class QueryCache
    {
        private static Hashtable data = new Hashtable();

        public static DataSet getResult(string sql)
        {
            sql = sql.ToUpper();
            if (!data.ContainsKey(sql))
            {
                DataSet ds = new DataSet();
                data.Add(sql, DBFunctions.Request(ds, IncludeSchema.NO, sql));
            }

            return (DataSet) data[sql];
        }

        public static void addData(string sql)
        {
            sql = sql.ToUpper();
            DataSet ds = new DataSet();
            data.Add(sql, DBFunctions.Request(ds, IncludeSchema.NO, sql));
        }

        public static void removeData(string table)
        {
            table = table.ToUpper();
            ArrayList keys = new ArrayList();

            foreach (string sql in data.Keys)
                if (sql.Contains(table)) keys.Add(sql);

            foreach (string sql in keys)
                data.Remove(sql);
        }

        public static void removeQuery(string query)
        {
            if (query == null) return;

            query = query.ToUpper();

            if(data[query] != null)
                data.Remove(query);
        }
    }
}