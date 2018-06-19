using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using IBM.Data.DB2;
using AMS.CriptoServiceProvider;

namespace AMS.Tools
{
	/// <summary>
	///		Descripción breve de AMS_Tools_ExcelImporter.
	/// </summary>
	public partial class ExcelImporter : System.Web.UI.UserControl
	{
		protected string err="";

		protected void Page_Load(object sender, System.EventArgs e)
		{
            //lbError.Text += " Cualquier error con cualquier" + "<br />" + " fila <br>  del archivo se registrará en cuanto termine el proceso";
            lbError.Text += "";
            lb.Text = "";
            if (!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				if(Request.QueryString["tbl"]!=null)
				{
					bind.PutDatasIntoDropDownList(ddltabla,"SELECT NAME from SYSIBM.SYSTABLES WHERE CREATOR='DBXSCHEMA' AND NAME='"+Request.QueryString["tbl"].ToUpper().Trim()+"'");
					if(ddltabla.Items.Count==0)
                    Utils.MostrarAlerta(Response, "Parámetro no válido, tabla inexistente.");
				}
				else
					bind.PutDatasIntoDropDownList(ddltabla, "SELECT NAME from SYSIBM.SYSTABLES WHERE ((NAME LIKE 'M%') OR (NAME LIKE 'D%') OR (NAME LIKE 'P%') OR (NAME LIKE 'R%')  OR (NAME LIKE 'T%'))  AND CREATOR='DBXSCHEMA' ORDER BY NAME");
				if(Request.QueryString["ext"]!=null)
                    Utils.MostrarAlerta(Response, "Proceso Exitoso.\\n Se ha logrado ingresar la totalidad de los registros");
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void SubirArchivo(object sender, System.EventArgs e)
		{
            lb.Text = "";
			if(filUpl.PostedFile.FileName.ToString()==string.Empty)
                Utils.MostrarAlerta(Response, "No ha seleccionado un archivo");
			else
			{
				string[] file=filUpl.PostedFile.FileName.ToString().Split('\\');
                string extencionArchivo = (file[file.Length-1].Split('.'))[file[file.Length-1].Split('.').Length-1].ToUpper();
                if (extencionArchivo != "XLSX")
                    Utils.MostrarAlerta(Response, "No es un archivo de Excel de formato xlsx");
                else
                {
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT TBNAME, NAME, COLTYPE, COLNO, NULLS FROM SYSIBM.SYSCOLUMNS WHERE TBNAME='" + ddltabla.SelectedValue + "' ORDER BY COLNO");
                    filUpl.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"] + file[file.Length - 1]);
                    ExcelFunctions exc = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"] + file[file.Length - 1]);
                    exc.Request(ds1, IncludeSchema.NO, "SELECT * FROM " + ddltabla.SelectedValue + "");
                    if (ds1.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count == ds1.Tables[0].Columns.Count)
                        {
                            for (int i = 0; i < ds1.Tables[0].Columns.Count; i++)
                            {
                                ds1.Tables[0].Columns[i].ColumnName = ds.Tables[0].Rows[i][1].ToString();
                            }
                            ds1.Tables[0].Rows[0].Delete();
                            ds1.Tables[0].Rows[0].AcceptChanges();


                            if (((Button)sender).Text.Equals(btnSubidaRapida.Text))
                            {
                                //if(ddltabla.SelectedValue.Contains("MITEM") || ddltabla.SelectedValue.Contains("MPRECIOITEM"))
                                //{
                                //    Utils.MostrarAlerta(Response, "Este proceso no es válido ni con la tabla maestra de ITEMS o ninguna tabla derivada de la maestra de items. \n Intente oprimiendo el botón Revisar y Subir");
                                //    return;
                                //}
                                if (insertTable(ds.Tables[0], ds1.Tables[0]))
                                {
                                    Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Tools.ExcelImporter&ext=1");
                                }
                                else
                                {
                                    lb.Text = "Falló el proceso de inserción de datos, no se guardó ningún registro. \\n Revise el archivo Excel \\n o Intente con el botón Revisar y Subir";
                                    return;
                                }
                            }
                            else
                            {
                                if (!Commit(Armar_Inserts(ds, ds1)))
                                {
                                    Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Tools.ExcelImporter&ext=1");
                                }
                            }
                        }
                        else
                        {
                            Utils.MostrarAlerta(Response, "el número de columnas del archivo excel no es el mismo al número de columnas de la tabla principal. \\n Verifique que siguió correctamente las instrucciones para subir el archivo Excel.");
                        }
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "No se encontró el archivo Excel. Verifique que siguió correctamente las instrucciones para subir el archivo Excel y que haya seleccionado correctamente la tabla a subir.");
                    }
                    
				}
			}
		}

        public string Analizar_Campo(string campo, string tipo, string nulo, ref string err, string nombre, int i, ref int cont)
		{
            string valor = "";

            if(campo.Trim() != "")
            {
                switch (tipo.Trim())
                {
                    case "BIGINT":
                    case "INTEGER":
                    case "SMALLINT":
                        try
                        {
                            valor = Convert.ToInt32(campo).ToString();
                        }
                        catch
                        {
                            err += "<p style=\"COLOR: red\"> El valor " + campo + " como valor numérico ha sido mal digitado, por favor revise.(sin puntos ni comas), Por favor revise la Fila número " + (i + 2) + " en su archivo Excel</p>" + "<br>";
                            cont ++;
                        }
                        break;
                    case "DECIMAL":
                        try
                        {
                            valor = Convert.ToDouble(campo.Replace(",",".")).ToString();
                        }
                        catch
                        {
                            err += "<p style=\"COLOR: red\"> El valor " + campo + " como valor numérico ha sido mal digitado, por favor revise.(sin puntos ni comas), Por favor revise la Fila número " + (i + 2) + " en su archivo Excel</p>" + "<br>";
                            cont++;
                        }
                        break;
                    case "DOUBLE":
                        try
                        {
                            valor = Convert.ToDouble(campo.Replace(",", ".")).ToString();
                        }catch
                        {
                            err += "<p style=\"COLOR: red\"> El valor " + campo + " como valor numérico ha sido mal digitado, por favor revise.(solo un punto decimal, sin comas), Por favor revise la Fila número " + (i + 2) + " en su archivo Excel</p>" + "<br>";
                            cont++;
                        }
                        break;
                    case "CHAR":
                    case "LONGVAR":
                    case "VARCHAR":
                        valor = "'" + campo + "'";
                        break;
                    case "DATE":
                        try
                        {
                            valor = "'" + Convert.ToDateTime(campo).ToString("yyyy-MM-dd") + "'";
                        }
                        catch
                        {
                            err += "<p style=\"COLOR: red\"> En el Campo: " + nombre + " Se esta asignando un valor erróneo de FECHA, Por favor revise la Fila número " + (i + 2) + " en su archivo Excel</p> <br>";
                            cont ++;
                        }
                        break;
                    case "TIME":
                        valor = "'" + Convert.ToDateTime(campo).ToString("HH:mm:ss") + "'";
                        break;
                    case "TIMESTMP":
                        valor = "'" + Convert.ToDateTime(campo).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        break;
                }
            }
            else
            {
                switch(nulo)
                {
                    case "N":
                        err += "<p style=\"COLOR: red\"> Campo: " + nombre + " Se esta asignando un valor nulo a un campo que no puede ser nulo, Por favor revise la Fila número " + (i + 2) + " en su archivo Excel</p><br> ";
                        cont ++;
                        break;
                    default:
                        valor = "null";
                        break;
                }
            }
            return valor;
		}

        protected bool insertTable(DataTable table1, DataTable table2)
        {
            if(ddltabla.SelectedValue == "MITEMS")
            {
                //CAMBIAR REFERENCIA POR MEDIO DE LA LINEA
                for (int i = 0; i < table2.Rows.Count; i++)
                {
                    string codItAlma = "";
                    Referencias.Guardar(table2.Rows[i][0].ToString().Replace('"', ' ').Trim(), ref codItAlma, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + table2.Rows[i][2].ToString().Trim() + "'"));
                    table2.Rows[i][0] = codItAlma;
                }
            }
            //Insert
            bool rta = false;
            string servidor = ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
            string database = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
            string usuario = ConfigurationManager.AppSettings["UID"];
            string password = ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];

            string timeout = ConfigurationManager.AppSettings["ConnectionTimeout"];
            string port = ConfigurationManager.AppSettings["DataBasePort"];
            AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd = miCripto.DescifrarCadena(password);
            string connectionString = "Server=" + servidor + ":" + port + ";DataBase=" + database + ";UID=" + usuario + ";PWD=" + newPwd + "";


            //IBM.Data.DB2.DB2BulkCopy dbBulkcopy = new IBM.Data.DB2.DB2BulkCopy(connectionString, IBM.Data.DB2.DB2BulkCopyOptions.KeepIdentity);
            using (var dbBulkcopy = new IBM.Data.DB2.DB2BulkCopy(connectionString, IBM.Data.DB2.DB2BulkCopyOptions.Default))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                if (table1.Rows.Count == table2.Columns.Count)
                {
                    foreach (DataColumn col in table2.Columns)
                    {
                        dbBulkcopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    dbBulkcopy.BulkCopyTimeout = 600;
                    dbBulkcopy.DestinationTableName = ddltabla.SelectedValue;

                    try
                    {
                        dbBulkcopy.WriteToServer(table2);
                        dbBulkcopy.Close();
                        DBFunctions.closeConnection();
                        rta = true;
                    }
                    catch (Exception z)
                    {
                        dbBulkcopy.Close();
                        DBFunctions.closeConnection();
                        rta = false;
                        Utils.MostrarAlerta(Response, "No se ingresó ningún registro debido a que el archivo Excel presenta fallas. \n Por favor revise su archivo o pruebe con el botón Revisar y Subir");
                        lbError.Text = z.Message;
                    }
                }
            }
            return rta;
        }
        
        private ArrayList Armar_Inserts(DataSet ds, DataSet ds1)
        {
            DataTable tabla1 = ds1.Tables[0];
            DataTable tabla2 = ds.Tables[0];
            ArrayList sqls = new ArrayList();
            int cont = 0;
            DataSet dsLineas = new DataSet();
            DBFunctions.Request(dsLineas, IncludeSchema.NO, "SELECT PLIN_TIPO,PLIN_CODIGO FROM DBXSCHEMA.PLINEAITEM; SELECT MITE_CODIGO FROM MITEMS;");
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                string sql = "INSERT INTO " + ddltabla.SelectedValue + " VALUES(";
                bool validaMitem = true;
                //crear dataset para las lineas
                
                for (int j = 0; j < ds1.Tables[0].Columns.Count; j++)
                {
                    if (j != ds1.Tables[0].Columns.Count - 1)
                    {
                        if (ddltabla.SelectedValue == "MITEMS" && j == 0)
                        {
                            DataRow[] laLinea = dsLineas.Tables[0].Select("PLIN_CODIGO = '" + ds1.Tables[0].Rows[i]["PLIN_CODIGO"] + "'");
                            string refer = "";
                            if(laLinea.Length > 0)
                                if (!Referencias.Guardar(ds1.Tables[0].Rows[i]["MITE_CODIGO"].ToString(), ref refer, laLinea[0].ItemArray[0].ToString()))
                                {
                                    err += "<p style=\"COLOR: red\"> Item Inválido " + ds1.Tables[0].Rows[i]["MITE_CODIGO"].ToString() + " Por favor revise la Fila número " + (i + 2) + " en su archivo Excel</p><br>";
                                    validaMitem = false;
                                    break;
                                }
                                else
                                {
                                    /*if (dsLineas.Tables[1].Select("MITE_CODIGO = '" + refer + "'").Length > 0)
                                    {
                                        err += "<p style=\"COLOR: red\"> Item Inválido " + ds1.Tables[0].Rows[i]["MITE_CODIGO"].ToString() + ", Existe en la Base de Datos, Por favor revise su excel en la linea " + (i+2) + "</p><br>";
                                    }*/
                                    sql += Analizar_Campo(refer, ds.Tables[0].Rows[j][2].ToString(), ds.Tables[0].Rows[j][4].ToString(), ref err, ds1.Tables[0].Columns[j].ColumnName, i, ref cont) + ",";
                                }
                            else
                            {
                                err += "<p style=\"COLOR: red\"> Linea Inválida " + ds1.Tables[0].Rows[i]["PLIN_CODIGO"].ToString() + " Por favor revise la Fila número " + (i + 2) + " en su archivo Excel</p><br>";
                                validaMitem = false;
                                break;
                            }
                        }
                        else
                            sql += Analizar_Campo(ds1.Tables[0].Rows[i][j].ToString(), ds.Tables[0].Rows[j][2].ToString(), ds.Tables[0].Rows[j][4].ToString(), ref err, ds1.Tables[0].Columns[j].ColumnName, i, ref cont) + ",";
                    }
                    else
                        sql += Analizar_Campo(ds1.Tables[0].Rows[i][j].ToString(), ds.Tables[0].Rows[j][2].ToString(), ds.Tables[0].Rows[j][4].ToString(), ref err, ds1.Tables[0].Columns[j].ColumnName, i, ref cont) + ")";
                }
                //if (cont < 1)
                //{
                //    sqls.Add(sql);
                //    cont = 0;
                //}
                if(validaMitem)
                    sqls.Add(sql);

            }
            //if (cont == 0)
                //return null;

            return sqls;
        }

        protected bool Commit(ArrayList sql)
        {
            bool error = false;
            if (err != "" || sql == null)
            {
                lbError.Text = err;
                return true;
            }
            else if(err != "")
            {
                lbError.Text = err;
                return true;
            }
            string mensajes = "";
            int contB = 0, contM = 0;
            
            lb.Text = "";
            
            int j;
            for (int i = 0; i < sql.Count; i++)
            {
                j = i + 2;
                //lb.Text+=sql[i].ToString()+"<br>";
                //Debug.Print(sql[i].ToString());
                if (DBFunctions.NonQuery(sql[i].ToString()) == 1)
                {
                    mensajes += "<p style=\"COLOR: green\">Bien ejecutando " + sql[i].ToString() + "<font size='4px'> fila excel: " + j + "</font></p>";
                    contB++;
                }
                else
                {
                    
                    mensajes += "<p style=\"COLOR: red\">Error ejecutando " + sql[i].ToString() + " <font size='4px'>fila excel: " + j + "</font></p>";
                    contM++;
                    error = true;
                }
            }
            if (error == true)
            {
                lb.Text += "El proceso ha terminado con errores.<br> Se han logrado ingresar " + contB + " registros. <br>Se han rechazado " + contM + " registros. <br>Por favor revise las lineas en color rojo para mas detalles<br>";
                lb.Text += mensajes;
            }
            
            return error;
        }
    }
}
