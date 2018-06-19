using System;
using System.Collections;
using System.Data;


namespace AMS.DB
{
	public interface IDBFunctions
	{
		string ConnectionString{get; set;}
		string Exceptions{get; set;}
		
		DataSet Request(DataSet ds, IncludeSchema isEnum, string sql);
        DataSet RequestGlobal(DataSet ds, IncludeSchema isEnum, string dataBase, string sql);

        DataSet Request(DataSet ds, IncludeSchema isEnum, string nombreProcedimiento, IDictionaryEnumerator parametros);
        //DataSet Insert(DataSet ds, int index);
        //DataSet Delete(DataSet ds, int index);
        //DataSet Update(DataSet ds, int index);
		
		int NonQuery(string sql);
        int NonQueryGlobal(string sql, string dataBase);
        int NonQuery(string sql, byte[] blob);
		string SingleData(string sql);
        string SingleDataGlobal(string sql);
				
		bool Transaction(ArrayList sql);

        bool RecordExist(string sql);

        /// <summary>
        /// Ejecuta una consulta y devuelve los resultados en un Arraylist lleno de Hashtable, un hash por fila del resultado. Los atributos consultados
        /// se pueden consultar desde el hashtable con su nombre en may�sculas. Ej.  string nit = hash["MNIT_NIT"];
        /// </summary>
        /// <param name="sql">El sql a ejecutar</param>
        /// <returns>Arraylist lleno de Hashtable </returns>
        ArrayList RequestAsCollection(string sql);
        /// <summary>
        /// Guarda un Hash lleno de los par�metros que deben almacenarse
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="hash">Par�metros a guardar</param>
        /// <returns>true si guard�, false si no</returns>
        /// 
        
        ArrayList RequestGlobalAsCollection(string sql);

        bool SaveHashtable(String tableName, Hashtable hash);
        /// <summary>
        /// Actualiza los datos de una tabla, dadas ciertas condiciones
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="hashData">Par�metros a actualizar</param>
        /// <param name="hashPk">Llaves o condiciones del Update</param>
        /// <returns>N�mero de filas afectadas</returns>
        int UpdateHashtable(string tableName, Hashtable hashData, Hashtable hashPk);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="hashPk">Llaves o condiciones del Delete</param>
        /// <returns>N�mero de filas afectadas</returns>
        int DeleteHashtable(string tableName, Hashtable hashPk);

        void CloseConnection();

    }

	public interface IDBFunctionsNew
	{
		int nonQuery(string sql);
		string singleData(string sql);
        ArrayList requestAsCollection(string sql);
		bool saveHashtable(String tableName, Hashtable hash);
	}
}
