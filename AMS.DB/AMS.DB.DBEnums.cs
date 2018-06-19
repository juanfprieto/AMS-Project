using System.IO;
using System;

namespace AMS.DB
{
	public enum DBAdapters
	{
		DB2,
		MYSQL,
		SQLSERVER,
		ORACLE,
		ODBC,
        WS
	}
	
	public enum SchemaSupport
	{
		YES,
		NO
	}
	
	public enum IncludeSchema
	{
		YES,
		NO
	}
}
