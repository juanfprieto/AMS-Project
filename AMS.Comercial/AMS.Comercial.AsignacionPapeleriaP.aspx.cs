// created on 01/03/2005 at 10:34using System;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS_Comercial_AsignacionPapeleria1.
	/// </summary>
	public class AMS_Comercial_AsignacionPapeleria1 : System.Web.UI.Page
	{
		public string txtAsignacion;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(Request.QueryString["asg"]!=null)
				this.Generar_Asignacion(Request.QueryString["asg"]);
		}

		protected void Generar_Asignacion(string numero)
		{
			string agencia,fecha,asignadoPor,asignadoA,nitDespachador;
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaAsignacionPapeleria.txt");
				strLinea=strArchivo.ReadLine();
				//La primera linea puede contener el ancho del tiquete
				try
				{
					anchoTiquete=Int16.Parse(strLinea);
					strLinea=strArchivo.ReadLine();}
				catch{}

				while(strLinea!=null)
				{
					plantilla+=strLinea+nlchar;
					strLinea=strArchivo.ReadLine();
				}
				strArchivo.Close();
			}
			catch
			{
                Utils.MostrarAlerta(Response, "No se ha creado la plantilla de asignacion de papeleria, no se pudo imprimir.");
				return;
			}
			plantilla=plantilla.Replace("<RED>",redChar);
			txtAsignacion="";

			DataSet dsAsignacion=new DataSet();
			DBFunctions.Request(dsAsignacion, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MASIGNACION_PAPELERIA WHERE MASIGNACION_NUMERO="+numero+";");
			if(dsAsignacion.Tables[0].Rows.Count==0)
			{
				plantilla="NO EXISTE LA ASIGNACIÓN";
				return;
			}

			agencia=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+dsAsignacion.Tables[0].Rows[0]["MAG_CODIGO"].ToString()+";");
			fecha=Convert.ToDateTime(dsAsignacion.Tables[0].Rows[0]["FECHA_REPORTE"]).ToString("yyyy-MM-dd");
			asignadoPor=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+dsAsignacion.Tables[0].Rows[0]["MNIT_RESPONSALE"].ToString()+"';");
			nitDespachador=DBFunctions.SingleData("SELECT MNIT_RESPONSABLE FROM DBXSCHEMA.MCONTROL_PAPELERIA where TDOC_CODIGO='"+dsAsignacion.Tables[0].Rows[0]["TDOC_CODIGO"].ToString()+"' AND NUM_DOCUMENTO="+dsAsignacion.Tables[0].Rows[0]["DOCUMENTO_INICIAL"].ToString()+" ;");
			asignadoA=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+nitDespachador+"';");
			if(nitDespachador.Length==0 || asignadoA.Length==0){
				plantilla="NO SE ENCONTRÓ LA PERSONA ASIGNADA";
				return;
			}
			
			/*txtAsignacion+=redChar+new String('-',anchoTiquete)+nlchar;
			txtAsignacion+=Tiquetes.CentrarTexto("ASIGNACION PAPELERIA",' ')+nlchar;
			txtAsignacion+=redChar+new String('-',anchoTiquete)+nlchar;
			txtAsignacion+="Num asignacion: "+numero+nlchar;*/
			plantilla=plantilla.Replace("<NUMERO>",numero);
			//txtAsignacion+="Agencia:"+nlchar;
			//txtAsignacion+=" "+agencia+nlchar;
			plantilla=plantilla.Replace("<AGENCIA>",agencia);
			//txtAsignacion+="Fecha:"+nlchar;
			//txtAsignacion+=" "+fecha+nlchar;
			plantilla=plantilla.Replace("<FECHA>",fecha);
			//txtAsignacion+="Asignado por:"+nlchar;
			//txtAsignacion+=" "+asignadoPor+nlchar;
			plantilla=plantilla.Replace("<ASIGNA_NOMBRE>",asignadoPor);
			//txtAsignacion+="Asignado a:"+nlchar;
			//txtAsignacion+=" "+asignadoA+nlchar+" "+nlchar;
			plantilla=plantilla.Replace("<ASIGNADO_NOMBRE>",asignadoA);

			txtAsignacion="";
			for(int n=0;n<dsAsignacion.Tables[0].Rows.Count;n++){
				txtAsignacion+=redChar+DBFunctions.SingleData("SELECT TDOC_NOMBRE FROM DBXSCHEMA.TDOCU_TRANS WHERE TDOC_CODIGO='"+dsAsignacion.Tables[0].Rows[0]["TDOC_CODIGO"].ToString()+"';").ToUpper()+":"+nlchar;
				txtAsignacion+=" Talonarios: "+dsAsignacion.Tables[0].Rows[0]["NUMERO_TALONARIOS"].ToString()+nlchar;
				txtAsignacion+=" Desde:      "+dsAsignacion.Tables[0].Rows[0]["DOCUMENTO_INICIAL"].ToString()+nlchar;
				txtAsignacion+=" Hasta:      "+dsAsignacion.Tables[0].Rows[0]["DOCUMENTO_FINAL"].ToString()+nlchar+nlchar;

			}
			plantilla=plantilla.Replace("<CONTENIDO>",txtAsignacion);

			/*txtAsignacion+=" "+nlchar+"Firma Encargado:"+nlchar+" "+nlchar+" "+nlchar+" "+nlchar;
			txtAsignacion+="Firma Responsable:"+nlchar+" "+nlchar;*/
			txtAsignacion=plantilla;
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
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
