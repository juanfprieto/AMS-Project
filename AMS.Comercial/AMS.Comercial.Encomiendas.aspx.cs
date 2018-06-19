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
	/// 	Descripción breve de AMS_Comercial_Encomiendas.
	/// </summary>
	public class AMS_Comercial_Encomiendas : System.Web.UI.Page
	{
		public string txtEncomienda;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(Request.QueryString["enc"]!=null)
				this.Generar_Encomienda(Request.QueryString["enc"]);
		}

		protected void Generar_Encomienda(string numero)
		{
			string agenciaO,ruta,fecha,entregaNit,entregaCel,placa,numBus,entregaNombre,recibeNit,recibeCel,recibeNombre,descripcion,unidades,peso,volumen,valDeclarado,costo,iva,total,responsableNit,responsableNombre;
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaEncomienda.txt");
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
                Utils.MostrarAlerta(Response, "No se ha creado la plantilla de Encomiendas, no se pudo imprimir.");
				return;
			}
			plantilla=plantilla.Replace("<RED>",redChar);

			DataSet dsEncomienda=new DataSet();
			DBFunctions.Request(dsEncomienda, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MENCOMIENDAS WHERE NUM_DOCUMENTO="+numero+";");
			if(dsEncomienda.Tables[0].Rows.Count==0)
			{
				plantilla="NO EXISTE LA ENCOMIENDA";
				return;
			}

			agenciaO=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+dsEncomienda.Tables[0].Rows[0]["MAG_RECIBE"].ToString()+";");
			ruta=DBFunctions.SingleData("SELECT MRUT_DESCRIPCION FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+dsEncomienda.Tables[0].Rows[0]["MRUT_CODIGO"].ToString()+"';");
			fecha=Convert.ToDateTime(dsEncomienda.Tables[0].Rows[0]["FECHA_RECIBE"]).ToString("yyyy/MM/dd");
			placa=dsEncomienda.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			numBus=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';");
			entregaNit=dsEncomienda.Tables[0].Rows[0]["MNIT_EMISOR"].ToString();
			entregaNombre=DBFunctions.SingleData("SELECT MPAS_NOMBRES CONCAT ' ' CONCAT MPAS_APELLIDOS FROM DBXSCHEMA.MPASAJERO where MPAS_NIT='"+entregaNit+"';");
			entregaCel=DBFunctions.SingleData("SELECT MPAS_TELEFONO  FROM DBXSCHEMA.MPASAJERO where MPAS_NIT='"+entregaNit+"';");
			recibeNit=dsEncomienda.Tables[0].Rows[0]["MNIT_DESTINATARIO"].ToString();
            recibeNombre = DBFunctions.SingleData("SELECT MPAS_NOMBRES CONCAT ' ' CONCAT MPAS_APELLIDOS FROM DBXSCHEMA.MPASAJERO where MPAS_NIT='" + recibeNit + "';");
            recibeCel=DBFunctions.SingleData("SELECT MPAS_TELEFONO  FROM DBXSCHEMA.MPASAJERO where MPAS_NIT='"+recibeNit+"';");
 		    responsableNit=dsEncomienda.Tables[0].Rows[0]["MNIT_RESPONSABLE_RECIBE"].ToString();
			responsableNombre=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+responsableNit+"';");	
			
			unidades=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["UNIDADES"]).ToString("###,###,##0");
			peso=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["PESO"]).ToString("###,###,##0");
			volumen=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VOLUMEN"]).ToString("###,###,##0");
			valDeclarado=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VALOR_AVALUO"]).ToString("###,###,##0");
			costo=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["COSTO_ENCOMIENDA"]).ToString("###,###,##0");
			iva=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VALOR_IVA"]).ToString("###,###,##0");
			total=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VALOR_TOTAL"]).ToString("###,###,##0");

			plantilla=plantilla.Replace("<NUMERO>",numero);
			plantilla=plantilla.Replace("<FECHA>",fecha);
			plantilla=plantilla.Replace("<PLANILLA>",dsEncomienda.Tables[0].Rows[0]["MPLA_CODIGO"].ToString());
			plantilla=plantilla.Replace("<DOCUMENTO>",dsEncomienda.Tables[0].Rows[0]["NUM_DOC_REFERENCIA"].ToString());
			plantilla=plantilla.Replace("<AGENCIA>",agenciaO);
			plantilla=plantilla.Replace("<RUTA>",ruta);
			plantilla=plantilla.Replace("<BUS_NUMERO>",numBus);
			plantilla=plantilla.Replace("<BUS_PLACA>",placa);

			plantilla=plantilla.Replace("<ENTREGA_NOMBRE>",entregaNombre);
			plantilla=plantilla.Replace("<ENTREGA_NIT>",entregaNit);
			plantilla=plantilla.Replace("<ENTREGA_CEL>",entregaCel);

			plantilla=plantilla.Replace("<RECIBE_NOMBRE>",recibeNombre);
			plantilla=plantilla.Replace("<RECIBE_NIT>",recibeNit);
			plantilla=plantilla.Replace("<RECIBE_CEL>",recibeCel);
			//descripcion=Tiquetes.CortarTexto(dsEncomienda.Tables[0].Rows[0]["DESCRIPCION_CONTENIDO"].ToString(),anchoTiquete);
			descripcion = dsEncomienda.Tables[0].Rows[0]["DESCRIPCION_CONTENIDO"].ToString();
			int longitud = descripcion.Length;
			int lineas = longitud/Tiquetes.anchoTiquete;
			string txtDescripcion="";
			int subindice = 0;
			for(int i=1;i<=lineas;i++)
			{
				txtDescripcion+=Tiquetes.ReemplazarTexto(descripcion.Substring(subindice,Tiquetes.anchoTiquete).ToString())+nlchar;
				subindice += Tiquetes.anchoTiquete;
			}
			if (subindice < longitud)
				txtDescripcion+=Tiquetes.ReemplazarTexto(descripcion.Substring(subindice,longitud - subindice).ToString())+nlchar;
				
			plantilla=plantilla.Replace("<DESCRIPCION>",txtDescripcion);
			//plantilla=plantilla.Replace("<DESCRIPCION>",descripcion);
			plantilla=plantilla.Replace("<UNIDADES>",unidades);
			plantilla=plantilla.Replace("<PESO>",peso);
			plantilla=plantilla.Replace("<VOLUMEN>",volumen);


			plantilla=plantilla.Replace("<VALOR_DECLARADO>",valDeclarado);
			plantilla=plantilla.Replace("<COSTO_ENCOMIENDA>",costo);
			plantilla=plantilla.Replace("<IVA_ENCOMIENDA>",iva);
			plantilla=plantilla.Replace("<TOTAL_ENCOMIENDA>",total);
			plantilla = plantilla.Replace("<NITRESPONSABLE>",responsableNit);
			plantilla = plantilla.Replace("<RESPONSABLE>",responsableNombre);

			
			txtEncomienda=plantilla;
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
