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

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS_Comercial_Viaje.
	/// </summary>
	public class AMS_Comercial_Viaje : System.Web.UI.Page
	{
		public string txtViaje;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(Request.QueryString["pla"]!=null)
				this.Generar_Viaje(Request.QueryString["pla"]);
		}

		protected void Generar_Viaje(string numeroPlanilla)
		{
			string agencia,fecha,rutaP,numBus,placaBus,conductor,relevador,hora,propietario,nitPropietario;
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
			string numViaje, codRuta;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaViaje.txt");
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
				Response.Write("<script language='javascript'>alert('No se ha creado la plantilla de viajes, no se pudo imprimir.');</script>");
				return;
			}
			plantilla=plantilla.Replace("<RED>",redChar);
			txtViaje="";

			numViaje=DBFunctions.SingleData("SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numeroPlanilla+";");
			codRuta=DBFunctions.SingleData("SELECT MRUT_CODIGO FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numeroPlanilla+";");
			if(numViaje.Length==0 || codRuta.Length==0){
				txtViaje="NO EXISTE EL VIAJE";
				return;
			}

			DataSet dsViaje=new DataSet();
			DBFunctions.Request(dsViaje, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MVIAJE where mrut_codigo='"+codRuta+"' AND mviaje_numero="+numViaje+";");
			if(dsViaje.Tables[0].Rows.Count==0){
				txtViaje="NO EXISTE EL VIAJE";
				return;
			} 
			
			agencia=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+dsViaje.Tables[0].Rows[0]["MAG_CODIGO"].ToString()+";");
			fecha=Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd")+" ";
			hora= (Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"])/60).ToString("00")+":"+((Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"])%60)).ToString("00");
			placaBus=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			numBus=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placaBus+"';");
			conductor=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"].ToString()+"';");
			relevador=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"].ToString()+"';");
			rutaP=DBFunctions.SingleData("SELECT MRUT_DESCRIPCION FROM MRUTAS WHERE MRUT_CODIGO='"+codRuta+"';");
			nitPropietario=DBFunctions.SingleData("SELECT MNIT_NITPROPIETARIO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placaBus+"';");
			propietario=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+nitPropietario+"';");

			plantilla=plantilla.Replace("<NUMERO>",numViaje);
			plantilla=plantilla.Replace("<AGENCIA>",agencia);
			plantilla=plantilla.Replace("<RUTA_PRINCIPAL>",rutaP);
			plantilla=plantilla.Replace("<FECHA_SALIDA>",fecha+" "+hora);
			plantilla=plantilla.Replace("<BUS_NUMERO>",numBus);
			plantilla=plantilla.Replace("<BUS_PLACA>",placaBus);
			plantilla=plantilla.Replace("<CONDUCTOR_NOMBRE>",conductor);
			plantilla=plantilla.Replace("<CONDUCTOR_NIT>",dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"].ToString());
			plantilla=plantilla.Replace("<RELEVADOR_NOMBRE>",relevador);
			plantilla=plantilla.Replace("<RELEVADOR_NIT>",dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"].ToString());
			plantilla=plantilla.Replace("<PROPIETARIO_NOMBRE>",propietario);
			plantilla=plantilla.Replace("<PROPIETARIO_NIT>",nitPropietario);
			txtViaje=plantilla;
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
