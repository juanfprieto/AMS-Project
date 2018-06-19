namespace AMS.Comercial
{
	using System;
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
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	

	/// <summary>
	///		Descripción breve de AMS_Comercial_Manuales.
	/// </summary>
	public class AMS_Comercial_Manuales : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Button Consultar;
		protected System.Web.UI.WebControls.DropDownList Procedimientos;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox descripcion;
		//string NombreArchivo=null;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				
				int contador=0; 
				if(contador==0)
				{
					bind.PutDatasIntoDropDownList(Procedimientos,"Select MMAN_MANUAL,MMAN_NOMBRE from DBXSCHEMA.MMANUALES ");
					ListItem it=new ListItem("--Procedimiento--","0");
					Procedimientos.Items.Insert(0,it);
				}
				
				
			}
		}
		public void Consultar_Click(object sender, System.EventArgs e)
		{
			
			string url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"MANUALES/" ;
			string procedimiento=""+Procedimientos.SelectedValue.ToString()+"";
			url=url + procedimiento;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
		
		
		
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
			this.Procedimientos.SelectedIndexChanged += new System.EventHandler(this.Procedimientos_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Procedimientos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			descripcion.Text=DBFunctions.SingleData("Select MMAN_DESCRIPCION from DBXSCHEMA.MMANUALES WHERE MMAN_MANUAL='"+Procedimientos.SelectedValue.ToString()+"'");
		}
	}
}
