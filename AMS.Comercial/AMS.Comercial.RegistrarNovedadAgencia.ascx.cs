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
	///		Descripción breve de AMS_Comercial_RegistrarNovedadAgencia.
	/// </summary>
	public class AMS_Comercial_RegistrarNovedadAgencia : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList Agencias;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox fecha;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox responsable;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList asunto;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox observaciones;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox registro;
		protected System.Web.UI.WebControls.Button Registrar;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox solucion;
		protected System.Web.UI.WebControls.Label Label1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				if(Agencias.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(Agencias,"select MOFI_CODIGO,MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA");
					ListItem it=new ListItem("--Agencias--","0");
					Agencias.Items.Insert(0,it);
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
			this.Agencias.SelectedIndexChanged += new System.EventHandler(this.Agencias_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Agencias_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Panel1.Visible=true;
			DatasToControls bind = new DatasToControls();
			registro.Text=DBFunctions.SingleData("select MOFI_ENCARGADO from DBXSCHEMA.MOFICINA WHERE MOFI_CODIGO='"+Agencias.SelectedValue.ToString()+"'");
			if(asunto.Items.Count==0)
			{
				bind.PutDatasIntoDropDownList(asunto,"Select TNOVE_CODIGO,TNOVE_DESCRIPCION from DBXSCHEMA.TNOVEDADES_AGENCIA ORDER BY TNOVE_DESCRIPCION");
				ListItem it=new ListItem("--Asunto--","0");
				asunto.Items.Insert(0,it);
			}

		}

		public  void Registrar_Click(Object  Sender, EventArgs e)
		{
			string query="INSERT INTO DBXSCHEMA.MNOVEDADES_AGENCIA VALUES(DEFAULT,DEFAULT,'"+Agencias.SelectedValue.ToString()+"','"+fecha.Text.ToString()+"','"+registro.Text.ToString()+"','"+responsable.Text.ToString()+"',"+asunto.SelectedValue.ToString()+",'"+observaciones.Text.ToString()+"','"+solucion.Text.ToString()+"')";
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MNOVEDADES_AGENCIA VALUES(DEFAULT,DEFAULT,'"+Agencias.SelectedValue.ToString()+"','"+fecha.Text.ToString()+"','"+registro.Text.ToString()+"','"+responsable.Text.ToString()+"',"+asunto.SelectedValue.ToString()+",'"+observaciones.Text.ToString()+"','"+solucion.Text.ToString()+"')");
			Label9.Text=DBFunctions.exceptions; 
		}



	}
}
