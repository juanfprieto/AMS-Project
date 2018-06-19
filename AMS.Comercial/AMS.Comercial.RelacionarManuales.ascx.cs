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
	///		Descripción breve de AMS_Comercial_RelacionarManuales.
	/// </summary>
	public class AMS_Comercial_RelacionarManuales : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Button btnSubir;
		protected System.Web.UI.HtmlControls.HtmlInputFile filUpl;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox procedimiento;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox descripcion;
		string NombreArchivo=null;
		private void Page_Load(object sender, System.EventArgs e)
		{
			
		}
		public void Relacionar_Click(object sender, System.EventArgs e)
		{
			if(filUpl.PostedFile.FileName.ToString()==string.Empty)
				Response.Write("<script language:javascript>alert('No ha seleccionado un archivo.');</script>");
			else
			{
				string[] file=filUpl.PostedFile.FileName.ToString().Split('\\');
				if((file[file.Length-1].Split('.'))[file[file.Length-1].Split('.').Length-1].ToUpper()!="HTM")
					Response.Write("<script language:javascript>alert('No es un archivo válido.');</script>");
				else
				{
					NombreArchivo=""+file[file.Length-1].ToString()+"";
					string query="INSERT INTO DBXSCHEMA.MMANUALES VALUES(DEFAULT,'"+procedimiento.Text.ToString()+"','"+NombreArchivo.ToString()+"')";
					DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MMANUALES VALUES(DEFAULT,'"+procedimiento.Text.ToString()+"','"+NombreArchivo.ToString()+"','"+descripcion.Text.ToString()+"')");
					Response.Write("<script language='javascript'>alert('Manual de :"+procedimiento.Text.ToString()+" Insertado Satisfactoriamente');</script>");
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
