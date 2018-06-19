// created on 19/8/2004 
using System.IO;
using System.Data.Common;
using System.Configuration;
using System.Text;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
	public partial class Importer : System.Web.UI.UserControl 
	{
        		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(tablaAct, "Select NAME from SYSIBM.SYSTABLES WHERE ((NAME LIKE 'M%') OR (NAME LIKE 'P%')  OR (NAME LIKE 'D%') OR (NAME LIKE 'S%') OR (NAME LIKE 'T%')) AND CREATOR='DBXSCHEMA' ORDER BY NAME");
			}
		}
		
		protected  void actualizar(Object  Sender, EventArgs e)
		{
            DB2FunctionsImpl db2f = new DB2FunctionsImpl();
            db2f.OpenConnection();

			lb.Text="";

			//Primero debemos cargar el archivo y guardarlo en la carpeta donde se encuentran los archivos
			Stream s = fDocument.PostedFile.InputStream;
			string[] files = fDocument.PostedFile.FileName.Split('\\');
			fDocument.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsCobol"]+files[files.Length-1]);
			
            //Debemos traer las columnas de esta tabla y cual es tu tipo de datos
			string tabla = tablaAct.SelectedValue;
			string fileText = "";
			string[] lineas, datos;
			string errorEsp = "";
			bool errorCarga = false;
			bool errorConfiguracion = false;
			ArrayList sqlStrings = new ArrayList();
			DataSet tipoDatos = new DataSet();
            db2f.Request(tipoDatos, IncludeSchema.NO, "SELECT coltype FROM sysibm.syscolumns WHERE tbname='" + tabla + "' ORDER BY colno");
			
            //Ahora vamos a leer el contenido del archivo para almacenarlo en la base de datos
			try
			{
				StreamReader sr = new StreamReader(ConfigurationManager.AppSettings["PathToImportsCobol"]+files[files.Length-1]);
   	 			fileText = sr.ReadToEnd();
   				sr.Close();
			}
			catch(Exception er)
			{
				lb.Text = "Se ha presentado un error con el archivo a importar: <BR>"+er.ToString();
				errorCarga = true;
			}
			if(!errorCarga)
			{
				//Aqui vamos a empezar a cargar este archivo a la base de datos
				lineas = fileText.Split("\n".ToCharArray());
				for(int i=0; i<lineas.Length; i++)
				{
					datos = lineas[i].Split(",".ToCharArray());
					string insert = "INSERT INTO " + tabla + " VALUES(";
					string nulos="";
					if(datos.Length == tipoDatos.Tables[0].Rows.Count)
					{
						for(int j=0; j<datos.Length; j++)
						{
							if((tipoDatos.Tables[0].Rows[j].ItemArray[0].ToString().Trim()=="CHAR")||(tipoDatos.Tables[0].Rows[j].ItemArray[0].ToString().Trim()=="VARCHAR")||(tipoDatos.Tables[0].Rows[j].ItemArray[0].ToString().Trim()=="DATE")||(tipoDatos.Tables[0].Rows[j].ItemArray[0].ToString().Trim()=="TIME")||(tipoDatos.Tables[0].Rows[j].ItemArray[0].ToString().Trim()=="DATETIME")||(tipoDatos.Tables[0].Rows[j].ItemArray[0].ToString().Trim()=="TIMESTMP"))
							{
								nulos=datos[j].Trim();
								if(nulos.ToUpper()!="NULL")
									insert += "'" + datos[j].Trim() + "'";
								else
									insert += datos[j].Trim();
							}
							else
							{
								nulos=datos[j].Trim();
								if(nulos.ToUpper()!="NULL")
									insert += "" + datos[j].Trim() + "";
								else
									insert += datos[j].Trim();
							}
						   	if(j != datos.Length-1)
							   	insert += ", ";
						}
						insert += ")";
						sqlStrings.Add(insert);
					}
					else
					{
						errorConfiguracion = true;
						errorEsp += "<br>Error en :"+i.ToString()+" "+lineas[i];
					}
				}
			}

			if(errorConfiguracion)
			{
				lb.Text+="<BR>"+"Existe un error en la configuracion del archivo : "+errorEsp;
			}
			else
			{
                int exitosos=0;
                int errores = 0;
                StringBuilder good = new StringBuilder();
                StringBuilder bad = new StringBuilder();
                String lblText;

				for(int i=0;i<sqlStrings.Count;i++)
				{
                    String sql = sqlStrings[i].ToString();

                    try
                    {
                        db2f.NonQuery(sqlStrings[i].ToString());

                        good.Append(String.Format("<font color=#009900>Se ha conseguido realizar la operación : {0} </font><br>", sql));
                        exitosos++;
                    }
                    catch (Exception ex)
                    {
                        bad.Append(String.Format("<font color=red> Error ejecutando : {0} <br>Detalles : {1} </font><br><br>", sql, ex.Message));
                        errores++;
                    }
				}

                lblText = good.ToString() + 
                    bad.ToString() + 
                    String.Format("<br><br><font color=#009900>Se ha conseguido ingresar satisfactoriamente {0} filas</font><br>", exitosos) +
                    String.Format("<font color=red>Se han rechazado {0} filas</font>", errores);

                lb.Text += lblText;
			}

            db2f.CloseConnection();
		}
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}

}
