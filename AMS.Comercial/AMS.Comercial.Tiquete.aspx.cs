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

namespace AMS.Comercial
{
	public class Tiquete : System.Web.UI.Page
	{
		public string strPlanilla,strError,strNITEmpresa,strNombre,strDireccionEmpresa,strTelefonoEmpresa,strDireccionAlterna1,strDireccionAlterna2,strTexto1,strTexto2,strNumero,strConductor,strNombreEmpresa,strFecha,strBus,strNoPasajes,strPuestos,strOrigen,strValor,strDestino,strValorTotal,strTexto3,strTexto4,strTexto5;
		public string txtTiquete;
		
		public Tiquete()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(Request.QueryString["tq"]!=null)
				this.Generar_Tiquete(Request.QueryString["tq"]);
			/*Response.Clear();
			Response.ClearContent();
			Response.ClearHeaders();
			Response.ContentType = "text/plain";
			Response.Write("pruebassss");
			Response.End();*/
		}
		
			//Validar responsable
			//Info general
			//Consultar tiquete
			//Consultar ruta
			//Consultar planilla, viaje
			//Consultar puestos
			//Agencia de paso? No tener en cuenta el puesto
			
			//Response.Write(HttpContext.Current.User.Identity.Name.ToString().ToLower());

		private void InitializeComponent()
		{
		
		}

		protected void Generar_Tiquete(string numero)
		{
			//Leer Plantilla
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
                string urlPapel = ConfigurationManager.AppSettings["PathToPapeleria"] + "PlantillaTiquete.txt";
                strArchivo = File.OpenText(urlPapel);
				strLinea=strArchivo.ReadLine();
				//La primera linea puede contener el ancho del tiquete
				try
                {
					anchoTiquete=Int16.Parse(strLinea);
					strLinea=strArchivo.ReadLine();
                }
				catch
                {
                }
                while(strLinea!=null)
				{
					plantilla+=strLinea+nlchar;
					strLinea=strArchivo.ReadLine();
				}
				strArchivo.Close();
                Response.Write("<script language='javascript'>alert('Imprimiendo tiquete.');</script>");
			}
			catch
            {
				Response.Write("<script language='javascript'>alert('No se ha creado la plantilla de tiquetes, no se pudo imprimir.');</script>");
				return;
			}
			plantilla=plantilla.Replace("<RED>",redChar);


			
			
			//Validar responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			//Tex.WriteLine("1234567890123456789012345678901234567890");
			if(nitResponsable.Length==0){
				Response.Write("<script language='javascript'>alert('No tiene permiso.');</script>");
				return;
			//Info general
			
			//Consultar tiquete
			

			//Consultar ruta
			
			//Consultar planilla, viaje
			
			//Consultar puestos
			//Agencia de paso? No tener en cuenta el puesto
			}
			
			
			/*txtTiquete+=redChar+new String('-',anchoTiquete)+nlchar;
			if(dsInfoTiquete.Tables[0].Rows.Count>0){
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["NOMBRE_EMPRESA"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto("NIT. "+dsInfoTiquete.Tables[0].Rows[0]["NIT_EMPRESA"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TEXTO1"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TEXTO2"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TELEFONO_EMPRESA"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TEXTO3"].ToString(),' ')+nlchar;}
			txtTiquete+=redChar+new String('-',anchoTiquete)+nlchar;
			txtTiquete+="Factura cambiaria de transporte"+nlchar;*/
			//txtTiquete+="número  : "+strNumero+nlchar;
				//txtTiquete+=Tiquetes.CortarTexto("Nombre : "+Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["COMPRADOR"].ToString()))+nlchar;
				//txtTiquete+=Tiquetes.CortarTexto("         "+Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["MNIT_COMPRADOR"].ToString()))+nlchar;

			/*
			if(dsViaje.Tables[0].Rows[0]["PROGRAMACION"].ToString().Length>0)
				plantilla=plantilla.Replace("<FECHA_VIAJE>",DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
				//txtTiquete+="Fecha   :  "+DateTime.Now.ToString("dd/MM/yyyy hh:mm")+nlchar;
			else 
				plantilla=plantilla.Replace("<FECHA_VIAJE>",DateTime.Now.ToString("dd/MM/yyyy"));
			*/	
			//txtTiquete+="Fecha   :  "+DateTime.Now.ToString("dd/MM/yyyy ")+nlc har;
			//txtTiquete+="Bus     :  "+dsViaje.Tables[0].Rows[0]["NUMERO"].ToString()+" ("+dsViaje.Tables[0].Rows[0]["PLACA"].ToString()+")"+nlchar;
			//txtTiquete+=Tiquetes.AjustarTexto("Puesto  :  "+strPuestos)+nlchar;
			//txtTiquete+="Origen  :  "+dsRuta.Tables[0].Rows[0]["ORIGEN"].ToString()+nlchar;
			//txtTiquete+="Destino :  "+dsRuta.Tables[0].Rows[0]["DESTINO"]+nlchar;
			//txtTiquete+="Pasajes :  "+dsPuestos.Tables[0].Rows.Count.ToString()+nlchar;
			//txtTiquete+="Valor   :  "+precio.ToString("#,###")+nlchar;
			//txtTiquete+="Total   :  "+(dsPuestos.Tables[0].Rows.Count*precio).ToString("#,###")+nlchar;
			txtTiquete=AMS.Comercial.Plantillas.GenerarTiquete(numero,plantilla,nlchar,nitResponsable,anchoTiquete);
			//txtTiquete+="Vendedor: "+Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["VENDEDOR"].ToString())+nlchar;
			//txtTiquete+="Planilla: "+strPlanilla+nlchar+nlchar;
		}
	}
}

