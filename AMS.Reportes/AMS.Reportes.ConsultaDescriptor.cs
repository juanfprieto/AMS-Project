using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using AMS.Tools;
using AMS.DB;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
//using AjaxControlToolkit;
using System.Drawing;
using AMS.Forms;

namespace AMS.Reportes
{
    [Serializable]
    public class ConsultaDescriptor
    {
        public ArrayList Columnas { get; set; }
        public ArrayList Filtros { get; set; }
        public ArrayList AgrupaCol { get; set; }
        public DataTable Datos { get; set; }
        public string Sql { get; set; }
        public string Agrupar { get; set; }

        private IConsultaHelper helper;
        private string SqlReady;
        private string idReporte;
        private bool usaGlobal = false;
        public ConsultaDescriptor(string idReporte, bool uGlobal)
        {
            usaGlobal = uGlobal;
            try
            {
                // llena filtros y llena sql
                switch (ConfigurationManager.AppSettings["DBConnectionType"])
                {
                    case "DB2": helper = new ConsultaHelperDB2(uGlobal);
                        break;
                    default: helper = new ConsultaHelperDB2(uGlobal);
                        break;
                }
                
                this.idReporte = idReporte;
                Filtros = helper.ObtenerFiltrosReporte(idReporte);
                DataSet dsReporte = helper.ObtenerSQL(idReporte);
                Sql     = dsReporte.Tables[0].Rows[0][0].ToString();
                Agrupar = dsReporte.Tables[0].Rows[0][1].ToString();
            }
            catch { };
        }
        /// <summary>
        /// Al momento de generar el reporte, se reemplazan los valores de _?_ por los de _'_ (sin rayas al piso)
        /// Además se reemplazan los filtros en el sql. Los valores a reemplazar están establecidos por @n, donde n es
        /// el número del filtro [1,n]. Si la consulta tiene el formato antiguo (en donde los parámetros no estaban enumerados
        /// sino que solamente se reemplazaban una vez dependiendo del orden de aparición) se reemplaza @ por orden de aparición
        /// </summary>
        public void GenerarReporte(Hashtable filtrosData )
        {
            AplicarFiltros(filtrosData);

            DataSet ds = new DataSet();
            
            if(!SqlReady.ToUpper().Contains("BEGIN "))
                ds = DBFunctions.Request(ds, IncludeSchema.NO, SqlReady);
            else
                ds = DBFunctions.Request(ds, IncludeSchema.YES, SqlReady);

            if (ds.Tables.Count == 0)
                throw new Exception("Existe un Problema con la consulta del reporte");

            Datos = ds.Tables[0];

            GenerarColumnas(); 
        }

        private void GenerarColumnas()
        {
            Columnas = new ArrayList();
            Hashtable mascaras = helper.ObtenerMascaras(idReporte);

            string[] idAguparCols = Agrupar.Split(',');

            int colIndex = 1;
            int contGrup = 0;
            foreach (DataColumn dc in Datos.Columns)
            {
                Type tipo = dc.DataType;
                TipoCampo tipoCampo = TipoCampo.Cadena;
                TipoCampo tipoFiltro = (TipoCampo) (mascaras[colIndex] != null ? mascaras[colIndex] : TipoCampo.Cadena);

                if (tipo == typeof(System.DateTime)) tipoCampo = TipoCampo.Fecha;
                else if (tipo == typeof(System.Decimal)) tipoCampo = TipoCampo.Numero;
                else if (tipo == typeof(System.Double)) tipoCampo = TipoCampo.Numero;
                else if (tipo == typeof(System.Int16)) tipoCampo = TipoCampo.Numero;
                else if (tipo == typeof(System.Int32)) tipoCampo = TipoCampo.Numero;
                else if (tipo == typeof(System.Int64)) tipoCampo = TipoCampo.Numero;
                else if (tipo == typeof(System.String)) tipoCampo = TipoCampo.Cadena;

                //if (tipoFiltro != TipoCampo.Cadena) tipoCampo = tipoFiltro;
                string agrupa = "0";
                if (contGrup < idAguparCols.Length && idAguparCols[contGrup] == Convert.ToString(colIndex))
                {
                    agrupa = "1";
                    contGrup++;
                }

                //Obtener tamaño promedio del campo.
                int tamanoCampo = 0;
                int muestra = 1;
                for(int y=0; y < 10 && y < Datos.Rows.Count; y++)
                {
                    if(Datos.Rows[y][colIndex - 1].ToString().Length > 0)
                    {
                        tamanoCampo += Datos.Rows[y][colIndex-1].ToString().Length;
                        muestra++;
                    }
                }
                try
                {
                    tamanoCampo = tamanoCampo / muestra;
                }catch
                {
                    tamanoCampo = 0;
                }
                //Definicion de celda para Grid en base al tamaño promedio del campo.
                if (tamanoCampo >= 20)
                    tamanoCampo = 190;
                else if (tamanoCampo >= 16)
                    tamanoCampo = 175;
                else if (tamanoCampo >= 14)
                    tamanoCampo = 150;
                else if (tamanoCampo >= 12)
                    tamanoCampo = 130;
                else if (tamanoCampo >= 6)
                    tamanoCampo = 100;
                else
                    tamanoCampo = 75;

                ColumnaDescriptor col = new ColumnaDescriptor()
                {
                    Nombre = dc.ColumnName,
                    TipoCampo = tipoCampo,
                    TipoFiltro = tipoFiltro,
                    Agrupa = agrupa,
                    TamanoPromedio = tamanoCampo
                };

                Columnas.Add(col);
                colIndex++;
            }
        }

        private void AplicarFiltros(Hashtable filtrosData)
        {
            SqlReady = Sql;

            // llenamos los filtros por defecto
            SqlReady = SqlReady.Replace("@USER", HttpContext.Current.User.Identity.Name.ToLower());

            //llenamos los filtros con el formato nuevo
            int i = 1;
            foreach (FiltroDescriptor filtro in Filtros)
            {
                SqlReady = SqlReady.Replace("@" + i, filtrosData[filtro.Id].ToString());
                i++;
            }
            //llenamos los filtros con el formato viejo
            string[] sqlParts = SqlReady.Split('@');
            string sqlFin = "";
            for (i = 0; i < sqlParts.Length; i++)
            {
                string sqlP = sqlParts[i];

                if (i < Filtros.Count)
                {
                    int id = ((FiltroDescriptor)Filtros[i]).Id;
                    if (filtrosData[id].ToString() != "")
                        sqlFin += sqlP + filtrosData[id].ToString();
                    else
                        sqlFin += sqlP + "**" + i;
                }
                else //ultimo
                {
                    sqlFin += sqlP;
                    SqlReady = sqlFin;
                }
            }

            //Proceso para permitir filtros vacios
            SqlReady = SqlReady.Replace("\n", " ").Replace("  ", " ").Replace("\r", " ").Replace("  ", " ");
            for(int k=0; k < Filtros.Count; k++)
            {
                int tamanoBusqueda = SqlReady.IndexOf("**"+k);
                if (tamanoBusqueda == -1)
                    tamanoBusqueda = 1;

                string sqlCadena = SqlReady.Substring(0, tamanoBusqueda);
                if (sqlCadena != "" && tamanoBusqueda > 1)
                {
                    string[] arraySqlCadena = sqlCadena.Split(' ');
                    if (arraySqlCadena[arraySqlCadena.Length - 1].Contains('?') )
                        SqlReady = SqlReady.Replace(arraySqlCadena[arraySqlCadena.Length-2] + " ?**" + k + "?", " = " + arraySqlCadena[arraySqlCadena.Length-3]);
                    else
                        SqlReady = SqlReady.Replace(arraySqlCadena[arraySqlCadena.Length - 2] + " **" + k, " = " + arraySqlCadena[arraySqlCadena.Length - 3]);
                }
            }

            SqlReady = SqlReady.Replace("?", "'");
        }

    }
}