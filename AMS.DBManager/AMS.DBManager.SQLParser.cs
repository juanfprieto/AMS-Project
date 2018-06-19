using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AMS.DBManager
{
    public class SQLParser
    {
        public static string dbConnectionType = ConfigurationManager.AppSettings["DBConnectionType"];

        //Obtiene la Estructura de la TABLA. [Columna, Remark, Indice Columna, Tipo de Dato]
        public static string GetTableStructure(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT name, remarks, colno, coltype FROM sysibm.syscolumns WHERE tbname='" + tabla + "' ORDER BY colno ASC";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"SELECT
                        a.COLUMN_NAME as NAME,
                        (ROW_NUMBER()  OVER(ORDER BY a.ORDINAL_POSITION ASC))-1 AS COLNO,
                        isnull(b.REMARK, '') as REMARKS,
                        UPPER(a.DATA_TYPE) as COLTYPE
                        FROM INFORMATION_SCHEMA.COLUMNS as a,
                        (
                        select 
                        sc.name  as COLNAME,
                        sep.value AS REMARK
                        from sys.tables st
                        inner join sys.columns sc on st.object_id = sc.object_id
                        left join sys.extended_properties sep on st.object_id = sep.major_id
                        and sc.column_id = sep.minor_id
                        and sep.name = 'MS_Description'
                        where st.name = '" + tabla + @"'
                        ) as b
                        where a.TABLE_NAME = '" + tabla + @"' and a.COLUMN_NAME = b.COLNAME
                        order by a.ORDINAL_POSITION;";
            }
            
            return query;
        }

        //Obtiene las llaves Foraneas de la TABLA. [Nombre Columna foranea, Tabla referencia, Nombre llave foranea]
        public static string GetTableForeign(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='" + tabla + "';";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                //se agrega espacio al fkcolnames para mantener logica del db2..
                query = @"select ' ' + k.COLUMN_NAME as FKCOLNAMES, k2.TABLE_NAME as REFTBNAME, k2.COLUMN_NAME  as PKCOLNAMES
                        from INFORMATION_SCHEMA.KEY_COLUMN_USAGE k , INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS r,
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE k2
                        where k.TABLE_NAME='" + tabla + @"' and k.CONSTRAINT_NAME = r.CONSTRAINT_NAME
                        and k2.CONSTRAINT_NAME = r.UNIQUE_CONSTRAINT_NAME;";
            }

            return query;
        }

        //Obtiene las llaves Primarias de la TABLA. [Llave primaria]
        public static string GetTablePrimary(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT NAME FROM SYSIBM.SYSCOLUMNS WHERE TBNAME = '" + tabla.ToUpper().Trim() + "' AND KEYSEQ > 0 ORDER BY KEYSEQ ASC ";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"select column_name as NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where CONSTRAINT_NAME in (
                        (select name from sys.indexes where object_id = OBJECT_ID('" + tabla + "')) ) order by ORDINAL_POSITION;";
            }

            return query;
        }

        // Obtiene el nombre de una Columna de la TABLA basado en la posicion indice. [Nombre de columna segun Indice]
        public static string GetTableColumnByIndex(string tabla, int posicion)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT COLNAME FROM SYSCAT.COLUMNS WHERE TABNAME = '" + tabla + "' and COLNO = " + posicion + " order by colno;";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                //para sql server se hace +1 a la posicion ya que inicia desde 1 y no 0 como DB2
                query = "select column_name as COLNAME from INFORMATION_SCHEMA.COLUMNS where table_name='" + tabla + "' and ORDINAL_POSITION = " + (posicion + 1);
            }

            return query;
        }

        // Obtiene la estructura de la TABLA con parametros adicionales 
        //[Remark, Columna, Tipo de Dato, tamaño Campo, Nuleable, Escala, Valor Default, Generado]
        public static string GetTableStructureExtended(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='" + tabla + "'";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"SELECT
                        isnull(b.REMARK, '') as REMARKS,
                        a.COLUMN_NAME as NAME,
                        UPPER(a.DATA_TYPE) as COLTYPE,
                        isnull(a.CHARACTER_MAXIMUM_LENGTH, 15) as LENGTH,
                        substring(a.IS_NULLABLE, 1, 1) as NULLS,
                        COALESCE(a.NUMERIC_PRECISION, 0) as SCALE,
                        COALESCE(REPLACE(REPLACE(a.COLUMN_DEFAULT, '(', ''), ')', ''), '') as [DEFAULT],
                        '' as GENERATED
                        FROM INFORMATION_SCHEMA.COLUMNS as a,
                        (
                        select 
                        sc.name  as COLNAME,
                        sep.value AS REMARK
                        from sys.tables st
                        inner join sys.columns sc on st.object_id = sc.object_id
                        left join sys.extended_properties sep on st.object_id = sep.major_id
                        and sc.column_id = sep.minor_id
                        and sep.name = 'MS_Description'
                        where st.name = '" + tabla + @"'
                        ) as b
                        where a.TABLE_NAME = '" + tabla + @"' and a.COLUMN_NAME = b.COLNAME
                        order bY NAME;";
            }

            return query;
        }

        //Obtiene valores extendidos de llaves Foraneas de la TABLA
        //[Nombre Columna foranea, Tabla referencia, Nombre Columna llave foranea, Nombre llave foranea, Referencia primaria de llave foranea]
        public static string GetTableForeignExtended(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT fkcolnames, reftbname, pkcolnames, relname, refkeyname FROM sysibm.sysrels WHERE tbname = '" + tabla + "'";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                //Se agregan espacios iniciales a fkcolnames y pkcolnames para mantener integridad con DB2 que los trae asi.
                query = @"select
                        ' ' + kc.COLUMN_NAME as FKCOLNAMES,
                        tc2.TABLE_NAME as REFTBNAME,
                        ' ' + kc2.COLUMN_NAME as PKCOLNAMES,
                        tc.CONSTRAINT_NAME as RELNAME,
                        rc.UNIQUE_CONSTRAINT_NAME as REFKEYNAME
                        from INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc,
                        INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc,
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE kc,
                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2,
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE kc2
                        where tc.table_name = '" + tabla + @"' and tc.CONSTRAINT_TYPE = 'FOREIGN KEY'
                        and tc.CONSTRAINT_NAME = rc.CONSTRAINT_NAME
                        and kc.CONSTRAINT_NAME = rc.CONSTRAINT_NAME
                        and tc2.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME
                        and kc2.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME;";
            }

            return query;
        }
        
        //Obtiene valores de llaves foraneas con columna de secuencia para llaves foraneas compuestas de la TABLA.
        //[nombres Llave foranea, columna foranea, secuencia de llave]
        public static string GetTableForeignKeySequence(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT constname, colname, colseq FROM sysibm.syskeycoluse WHERE tbname = '" + tabla + "' AND constname NOT IN(SELECT name FROM sysibm.sysindexes WHERE tbname = '" + tabla + "')";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"Select
                        C.CONSTRAINT_NAME as CONSTNAME,
                        CU.COLUMN_NAME as COLNAME,
                        CU.ORDINAL_POSITION as COLSEQ
                        FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
                        INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                        INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
                        INNER JOIN (SELECT i1.TABLE_NAME, i2.COLUMN_NAME
                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                        WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                        ) PT ON PT.TABLE_NAME = PK.TABLE_NAME and FK.TABLE_NAME = '" + tabla + "';";
            }

            return query;
        }

        //Obtiene Nombre de columna foranea de la TABLA segun el nombre de la llave foranea. 
        //[Nombre columna Foranea]
        public static string GetKeyColumnByForeignName(string keyName, string colSeq)
        {
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT colname FROM sysibm.syskeycoluse WHERE constname = '" + keyName + "' AND colseq = " + colSeq;
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"select COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                        where CONSTRAINT_NAME='" + keyName + "' and ORDINAL_POSITION=" + colSeq;
            }

            return query;
        }

        //Obtiene Nombre de columna de la TABLA segun el indice de la TABLA. 
        //[Nombre columna]
        public static string GetColumnNameByTableIndex(string tabla, string colSeq)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT NAME FROM sysibm.syscolumns WHERE tbname='" + tabla + "' AND colno=" + colSeq;
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where 
                        TABLE_NAME = '" + tabla + "'  and ORDINAL_POSITION = " + colSeq;
            }

            return query;
        }

        //Obtiene Tipo de Dato de columna foranea de la TABLA segun el nombre de la llave foranea. 
        //[Tipo Dato columna foranea]
        public static string GetDataTypeByForeignName(string tabla, string keyName, string colSeq)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT coltype FROM sysibm.syscolumns WHERE name=(SELECT colname FROM " +
                           "sysibm.syskeycoluse WHERE constname='" + keyName + "' AND colseq=" + colSeq + ") AND tbname='" + tabla + "'";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"select UPPER(DATA_TYPE) as COLTYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tabla + "'" +
                        "and COLUMN_NAME = (select COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                        "where CONSTRAINT_NAME='" + keyName + "' and ORDINAL_POSITION = " + colSeq + ");";
            }

            return query;
        }
  
        //Obtiene todas las Columnas de la TABLA. [Columnas Tabla]
        public static string GetColumnsTable(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT NAME FROM sysibm.SYSCOLUMNS where TBNAME = '" + tabla + "' order by colno;";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"select COLUMN_NAME as NAME from INFORMATION_SCHEMA.COLUMNS where 
                        TABLE_NAME = '" + tabla + "'  ORDER BY ORDINAL_POSITION";
            }

            return query;
        }

        //Obtiene las llaves primarias de la TABLA en relacion a TABLA foranea.
        //[Nombre columnas primarias]
        public static string GetPrimaryKeysTable(string tabla, string tablaRef)
        {
            tabla = tabla.ToUpper().Trim();
            tablaRef = tablaRef.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT PKCOLNAMES FROM SYSIBM.SYSRELS WHERE TBNAME='" + tabla + "' AND REFTBNAME='" + tablaRef + "';";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"select COLUMN_NAME as PKCOLNAMES from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where CONSTRAINT_NAME in (
                        (select name from sys.indexes where object_id = OBJECT_ID('" + tablaRef + "')) ) order by ORDINAL_POSITION;";
            }

            return query;
        }

        //Obtiene las TABLAS hijas relacionadas a la TABLA parametro. Esta relacion debe estar en STABLA_DEPENDENCIAS.
        //[Nombre tabla hija, Descripcion tabla, LLave primaria tabla]
        public static string GetChildTables(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT DISTINCT ST.NAME AS NOMBRE, ST.REMARKS AS DESCRIPCION, SIH.COLNAMES AS LLAVE_PRIMARIA " +
                            "FROM SYSIBM.SYSRELS SR, SYSIBM.SYSTABLES ST, SYSIBM.SYSINDEXES SIH " +
                            "WHERE SR.CREATOR='DBXSCHEMA' AND SR.REFTBNAME='" + tabla + "' AND ST.NAME=SR.TBNAME AND " +
                            "SIH.TBNAME=ST.NAME AND SIH.UNIQUERULE='P' AND " +
                            "UPPER(ST.NAME) IN(SELECT UPPER(TABLA) FROM DBXSCHEMA.STABLA_DEPENDENCIAS);";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                query = @"select 
                        tc.TABLE_NAME as NOMBRE,
                        ep.value as DESCRIPCION,
                        '+' + (select kk.COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE kk
                        where kk.table_name=tc.TABLE_NAME and kk.CONSTRAINT_NAME=(
                        select tt.CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS tt
                        where tt.table_name=tc.TABLE_NAME and tt.CONSTRAINT_TYPE='PRIMARY KEY'))
                        as LLAVE_PRIMARIA
                        from INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc,
                        INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc,
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE kc,
                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2,
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE kc2
                        ,sys.extended_properties ep
                        where tc2.table_name='" + tabla + @"'
                        and tc.CONSTRAINT_TYPE='FOREIGN KEY'
                        and tc.CONSTRAINT_NAME = rc.CONSTRAINT_NAME
                        and kc.CONSTRAINT_NAME = rc.CONSTRAINT_NAME
                        and tc2.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME
                        and kc2.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME
                        and tc.TABLE_NAME IN (SELECT UPPER(TABLA) FROM STABLA_DEPENDENCIAS)
                        and ep.major_id = OBJECT_ID(tc.table_name) 
                        and ep.name = 'MS_Description' and ep.minor_id=0;";
            }

            return query;
        }

        //Obtiene una fila de un campo con las llaves Primarias de la TABLA concatenadas con '+'.
        //[Concatenado de llave Rpimaria Tabla]
        public static string GetPrimaryKeysString(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT COLNAMES FROM SYSIBM.SYSINDEXES WHERE TBNAME='" + tabla + "' AND UNIQUERULE='P'";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                //se aplica 3 left joins para traer las llaves en una sola fila en un solo campo. Ya que Sql Server no trae
                //las llaves en una misma fila como DB2 si lo hace.
                query = @"select COALESCE(colname1, '') + COALESCE(colname2, '') + COALESCE(colname3, '')  as COLNAME from
                        (select '+' + COLUMN_NAME as COLNAME1, CONSTRAINT_NAME  from INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                        where TABLE_NAME = '" + tabla + @"' and CONSTRAINT_NAME like 'PK_%' and ORDINAL_POSITION = 1) as a
                        left join
                        (select '+' + COLUMN_NAME as COLNAME2, CONSTRAINT_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                        where TABLE_NAME = '" + tabla + @"' and CONSTRAINT_NAME like 'PK_%' and  ORDINAL_POSITION = 2) as b
                        on a.CONSTRAINT_NAME = b.CONSTRAINT_NAME
                        left join
                        (select '+' + COLUMN_NAME as COLNAME3, CONSTRAINT_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                        where TABLE_NAME = '" + tabla + @"' and CONSTRAINT_NAME like 'PK_%' and  ORDINAL_POSITION = 3) as c
                        on b.CONSTRAINT_NAME = c.CONSTRAINT_NAME; ";
            }

            return query;
        }

        public static string GetTableNameDescription(string tabla)
        {
            tabla = tabla.ToUpper().Trim();
            string query = "";

            if (dbConnectionType == "DB2")
            {
                query = "SELECT remarks FROM SYSIBM.SYSTABLES WHERE name = '" + tabla + "'";
            }
            else if (dbConnectionType == "SQLSERVER")
            {
                //se aplica 3 left joins para traer las llaves en una sola fila en un solo campo. Ya que Sql Server no trae
                //las llaves en una misma fila como DB2 si lo hace.
                query = "select value from sys.extended_properties where " +
                        "major_id = OBJECT_ID('" + tabla + "') and minor_id = 0 and name = 'MS_Description';";
            }

            return query;
        }


    }
}