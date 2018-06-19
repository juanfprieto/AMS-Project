using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;



namespace AMS.Nomina
{
	public class ArchivoRetefuente : System.Web.UI.UserControl 
	{
		protected DataGrid DataGridRfte;
		protected System.Web.UI.WebControls.Button btnAceptar;
		protected HtmlInputFile fDocument;
		
		
		protected void AceptarArchivo(Object Sender, EventArgs E)
		{
			
			int i;
			string[] files = fDocument.PostedFile.FileName.Split('\\');
			if((files[files.Length-1].Split('.'))[1].Trim().ToUpper()=="XLS")
			{
				
				fDocument.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"]+files[files.Length-1]);
				//Ahora llamamos la clase que me permite traer los datos de una hoja de excel mediante ADO
				ExcelFunctions excelManager = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"]+files[files.Length-1]);
				DataSet ds = new DataSet();
				DataSet pretefuente = new DataSet();
				ds = excelManager.Request(ds,IncludeSchema.NO,"SELECT * FROM TablaRetefuente");
				DBFunctions.Request(pretefuente,IncludeSchema.NO,"select * from dbxschema.pretefuente;");
				if(ds.Tables.Count>0)
				{
					
					if(ds.Tables[0].Columns.Count == 5)
					{
						//mirar si la tabla pretefuente tiene algo
						if (pretefuente.Tables[0].Rows.Count>=0)
						{
                            Utils.MostrarAlerta(Response, "Se borrara la tabla anterior de Retencion en la Fuente");
							//borrar toda la tabla para guardar de nuevo
							DBFunctions.NonQuery("delete from dbxschema.pretefuente");
						}
                        Utils.MostrarAlerta(Response, "cargo archivo bien");
						for (i=0;i<ds.Tables[0].Rows.Count;i++)
						{
							
							DBFunctions.NonQuery("insert into pretefuente values(default,"+double.Parse(ds.Tables[0].Rows[i][0].ToString())+","+double.Parse(ds.Tables[0].Rows[i][2].ToString())+","+double.Parse(ds.Tables[0].Rows[i][3].ToString())+","+double.Parse(ds.Tables[0].Rows[i][4].ToString())+")");
							
						}
						
						
						DataGridRfte.DataSource= ds.Tables[0];
						DataGridRfte.DataBind();
												
					}
					else
                    Utils.MostrarAlerta(Response, "El archivo no tiene el numero de columnas indicado " + ds.Tables[0].Rows.Count.ToString() + ". Revise Por Favor!");
				}
				else
                Utils.MostrarAlerta(Response, "No se ha podido cargar el archivo, no se ha dado el nombre correcto al rango de celdas. Revise Por Favor!");
			}
			else
            Utils.MostrarAlerta(Response, "Tipo de Archivo Invalido!");
	
	
		}
		
		
		
		
		
		
		
		
		protected void Page_Load(object sender , EventArgs e)
		{
			
		}
		//protected HtmlInputFile fDocument;
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	
	
	}
	
	
		
	
	
	
}
