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
		
	public class GenerarPlanillaIntegrada: System.Web.UI.UserControl
	{
		protected DropDownList ddlFormap;
		protected TextBox txtCodSucu;
		protected string nombreArchivo = "PlanillaIntegrada"+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
		protected	string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];
		protected	StreamWriter sw;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.WebControls.TextBox TextBox3;
		protected System.Web.UI.WebControls.Button Button1;
		//protected	StreamWriter sw = File.CreateText(directorioArchivo+nombreArchivo);
		protected PlanillaPago comparacion = new PlanillaPago();
		
		
		string outputDatosAportante;
		
		
		
		protected void iniciarproceso()
		{
			sw = File.CreateText(directorioArchivo+nombreArchivo);
			this.iniciararchivo();
			this.datosAportante();
			
		}
		
		protected void iniciararchivo()
		{
			
		}
		
		
		protected void datosAportante()
		{
			
			//Es el encabezado del Archivo Magnetico
			//1. tipo registro obligaotrio 01
			outputDatosAportante+="01";
			//2.secuencia obligatorio
			outputDatosAportante+="00001";
			DataSet DatosEmpresa= new DataSet();
			DBFunctions.Request(DatosEmpresa,IncludeSchema.NO,"select C.cemp_nombre,T.tnit_tiponit ,MNIT.mnit_nit,MNIT.mnit_digito from dbxschema.mnit MNIT, dbxschema.cempresa C, dbxschema.tnit T where MNIT.mnit_nit=C.mnit_nit and MNIT.tnit_tiponit=T.tnit_tiponit");
			//3.nombre o razon social aportante
			outputDatosAportante+=comparacion.Completar_Campos(DatosEmpresa.Tables[0].Rows[0][0].ToString(),2," ",false);
			//4.tipo documento aportante
			outputDatosAportante+=comparacion.Completar_Campos(DatosEmpresa.Tables[0].Rows[0][1].ToString(),2," ",false);
			//5. Numero identificacion aportante
			outputDatosAportante+=comparacion.Completar_Campos(DatosEmpresa.Tables[0].Rows[0][2].ToString(),3," ",false);
			//6. digito de Verificacion aportante
			outputDatosAportante+=comparacion.Completar_Campos(DatosEmpresa.Tables[0].Rows[0][3].ToString(),1," ",false);
			//7. Forma de Presentacion U.unico C.consolidado S.sucursal.
			outputDatosAportante+=ddlFormap.SelectedValue;
			//8. Codigo de la sucursal Aportante(obligatorio si la f.p es S.)
			if (ddlFormap.SelectedValue=="S")
			{
				if (txtCodSucu.Text!="")
				{
					outputDatosAportante+=comparacion.Completar_Campos(txtCodSucu.Text,10,"$",true);
				}
				else
				{
                    Utils.MostrarAlerta(Response, "Campo Obligatorio si la forma de presentacion es Sucursal");
				}
			}
			else if (ddlFormap.SelectedValue!="S")
			{
				outputDatosAportante+=comparacion.Completar_Campos(txtCodSucu.Text,10,"&",true);
			}
			
			//9. 
			
			
			
			
			outputDatosAportante+="fin";
			sw.WriteLine(outputDatosAportante);
			
			//cerrrar archivo.
			sw.Close();
			
		}
		
		protected void generarArchivoPlano(object sender, EventArgs e)
		{
			this.iniciarproceso();
			            Utils.MostrarAlerta(Response, "" + comparacion.Completar_Campos("hola", 2, "/", false) + "");
			
		}
		
			protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				
							
				
				
				//this.Completar_Campos("hola",10,20);
			}
			
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



