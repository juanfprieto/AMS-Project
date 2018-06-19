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

namespace AMS.Tools
{
	/// <summary>
	/// Descripci�n breve de AMS_Tools_Impresion.
	/// </summary>
	public partial class Impresion : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			if(Request.QueryString["tip"]=="1")
			{
				pnl1.Visible=true;
				pnl2.Visible=false;
				this.Cargar_Datos_Tipo1();
			}
			else if(Request.QueryString["tip"]=="2")
			{
				pnl2.Visible=true;
				pnl1.Visible=false;
				this.Cargar_Datos_Tipo2();
			}
		}

		protected void Cargar_Datos_Tipo1()
		{
			if(Session["datimp"]!=null)
			{
				lbtabla.Text=((ArrayList)Session["datimp"])[0].ToString();
				lbcampo.Text=((ArrayList)Session["datimp"])[1].ToString();
				lbcomentario.Text=((ArrayList)Session["datimp"])[2].ToString();
				lbayuda.Text=((ArrayList)Session["datimp"])[3].ToString();
			}
		}

		protected void Cargar_Datos_Tipo2()
		{
			if(Session["datimp"]!=null)
			{
				dg.DataSource=(DataTable)Session["datimp"];
				dg.DataBind();
			}
		}

		#region C�digo generado por el Dise�ador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// M�todo necesario para admitir el Dise�ador. No se puede modificar
		/// el contenido del m�todo con el editor de c�digo.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
