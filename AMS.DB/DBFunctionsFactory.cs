using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.DB
{
    class DBFunctionsFactory
    {

        internal static IDBFunctions CreateDBFunctions(DBAdapters dba)
        {
            IDBFunctions func = null;

            switch(dba)
            {
                case DBAdapters.DB2: 
                    func = new DB2Functions();
                    break;
                case DBAdapters.SQLSERVER:
                    func = new SQLServerFunctions();
                    break;
                case DBAdapters.ODBC: 
                    func = new OdbcFunctions();
                    break;
                case DBAdapters.WS:
                    func = new WSFunctions();
                    break;

            }

            return func;
        }
    }
}
