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
	/// Descripción breve de AMS_Comercial_Giro.
	/// </summary>
	public class AMS_Comercial_Giro : System.Web.UI.Page
	{
		public string txtGiro;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(Request.QueryString["gir"]!=null)
				this.Generar_Giro(Request.QueryString["gir"]);
		}

		protected void Generar_Giro(string numero)
		{
			string agenciaO,agenciaD,fecha,entregaNit,entregaNombre,recibeNit,recibeNombre,ResponsableNombre,valor, costo,iva,total;
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaGiro.txt");
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
                Utils.MostrarAlerta(Response, "No se ha creado la plantilla de Giros, no se pudo imprimir.");
				return;
			}
			plantilla=plantilla.Replace("<RED>",redChar);

			DataSet dsGiro=new DataSet();
			DBFunctions.Request(dsGiro, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MGIROS WHERE NUM_DOCUMENTO="+numero+";");
			if(dsGiro.Tables[0].Rows.Count==0)
			{
				plantilla="NO EXISTE EL GIRO";
				return;
			}

			//Validar responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "NO TIENE NIT EL USUARIO--> CREELO EN NITS");
				return;
			}

			agenciaO=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+dsGiro.Tables[0].Rows[0]["MAG_AGENCIA_ORIGEN"].ToString()+";");
			agenciaD=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+dsGiro.Tables[0].Rows[0]["MAG_AGENCIA_DESTINO"].ToString()+";");
			fecha=Convert.ToDateTime(dsGiro.Tables[0].Rows[0]["FECHA_RECIBE"]).ToString("yyyy/MM/dd");
			entregaNit=dsGiro.Tables[0].Rows[0]["MNIT_EMISOR"].ToString();
			entregaNombre=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+entregaNit+"';");
			ResponsableNombre=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+nitResponsable+"';");
			recibeNit=dsGiro.Tables[0].Rows[0]["MNIT_DESTINATARIO"].ToString();
			recibeNombre=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+recibeNit+"';");			
			valor=Convert.ToDouble(dsGiro.Tables[0].Rows[0]["VALOR_GIRO"]).ToString("###,###,##0");
			costo=Convert.ToDouble(dsGiro.Tables[0].Rows[0]["COSTO_GIRO"]).ToString("###,###,##0");
			iva=Convert.ToDouble(dsGiro.Tables[0].Rows[0]["VALOR_IVA"]).ToString("###,###,##0");
			total=(Convert.ToDouble(dsGiro.Tables[0].Rows[0]["VALOR_GIRO"])+Convert.ToDouble(dsGiro.Tables[0].Rows[0]["COSTO_GIRO"])+Convert.ToDouble(dsGiro.Tables[0].Rows[0]["VALOR_IVA"])).ToString("###,###,##0");

			plantilla=plantilla.Replace("<NUMERO>",numero);
			plantilla=plantilla.Replace("<FECHA>",fecha);
			plantilla=plantilla.Replace("<AGENCIA_ORIGEN>",agenciaO);
			plantilla=plantilla.Replace("<AGENCIA_DESTINO>",agenciaD);

			plantilla=plantilla.Replace("<ENTREGA_NOMBRE>",entregaNombre);
			plantilla=plantilla.Replace("<ENTREGA_NIT>",entregaNit);
			plantilla=plantilla.Replace("<RECIBE_NOMBRE>",recibeNombre);
			plantilla=plantilla.Replace("<RECIBE_NIT>",recibeNit);

			plantilla=plantilla.Replace("<VALOR_GIRO>",valor);
			plantilla=plantilla.Replace("<COSTO_GIRO>",costo);
			plantilla=plantilla.Replace("<IVA_GIRO>",iva);
			plantilla=plantilla.Replace("<TOTAL_GIRO>",total);
			
			int longitud = ResponsableNombre.Length;
			int lineas = longitud/Tiquetes.anchoTiquete;
			string txtNombre="";
			int subindice = 0;
			for(int i=1;i<=lineas;i++)
			{
				txtNombre+=Tiquetes.ReemplazarTexto(ResponsableNombre.Substring(subindice,Tiquetes.anchoTiquete).ToString())+nlchar;
				subindice += Tiquetes.anchoTiquete;
			}
			if (subindice < longitud)
				txtNombre+=Tiquetes.ReemplazarTexto(ResponsableNombre.Substring(subindice,longitud - subindice).ToString())+nlchar;
			plantilla=plantilla.Replace("<RESPONSABLE>",txtNombre);
			txtGiro=plantilla;
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
