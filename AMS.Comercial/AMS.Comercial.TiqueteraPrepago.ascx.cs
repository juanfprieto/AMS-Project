namespace AMS.Comercial
{
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Globalization;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.Mail;
	using System.Web.UI;
	using System;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Comercial_TiqueteraPrepago.
	/// </summary>
	public class AMS_Comercial_TiqueteraPrepago : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox numero;
		protected System.Web.UI.WebControls.TextBox fecha;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox inicio;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox fin;
		protected System.Web.UI.WebControls.Button Crear;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator2;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
			fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");

			}
		}
		public void Crear_Click(object sender, System.EventArgs e)
		{
			int Inicio=Convert.ToInt32(inicio.Text.ToString());
			int Fin=Convert.ToInt32(fin.Text.ToString());
			if(Fin < Inicio)
			{
				Response.Write("<script language='javascript'>alert('Secuencia Invalida');</script>");
			
			}
			else
			{
				string ExisteTQ=DBFunctions.SingleData("select TTIQ_CODIGO from DBXSCHEMA.TTIQUETERA_PREPAGO WHERE TTIQ_CODIGO="+numero.Text.ToString()+"");
				if(ExisteTQ.Equals("") || ExisteTQ.Equals(null))
				{
			
					int NumeroTiquetes=Fin-Inicio;
					DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.TTIQUETERA_PREPAGO VALUES("+numero.Text.ToString()+",'"+fecha.Text.ToString()+"',"+inicio.Text.ToString()+","+fin.Text.ToString()+","+inicio.Text.ToString()+","+NumeroTiquetes+",1) ");
					Response.Write("<script language='javascript'>alert('Creacion Exitosa');</script>");
					Label5.Text=DBFunctions.exceptions;
				}
				else
				{
					Response.Write("<script language='javascript'>alert('Tiquetera "+numero.Text.ToString()+" YA Existe,Seleccione otro Numero');</script>");
				}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
