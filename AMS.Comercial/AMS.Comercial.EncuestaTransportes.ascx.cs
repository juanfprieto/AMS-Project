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
	//Realizado por:
	//Ing.German Lopera
	//2007-04-26

	/// <summary>
	///		Descripción breve de AMS_Comercial_EncuestaTransportes.
	/// </summary>
	public class AMS_Comercial_EncuestaTransportes : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label codencuesta;
		protected System.Web.UI.WebControls.Label fechaencuesta;
		protected System.Web.UI.WebControls.TextBox nombre;
		protected System.Web.UI.WebControls.TextBox telefono;
		protected System.Web.UI.WebControls.DropDownList servicio;
		protected System.Web.UI.WebControls.DropDownList calser;
		protected System.Web.UI.WebControls.TextBox opservi;
		protected System.Web.UI.WebControls.DropDownList calaten;
		protected System.Web.UI.WebControls.TextBox opaten;
		protected System.Web.UI.WebControls.TextBox opgen;
		protected System.Web.UI.WebControls.Button Guardar;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
	    int consecutivo=0;
		private void Page_Load(object sender, System.EventArgs e)
		{
			
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				string consecutivoS=DBFunctions.SingleData("select max(MENCU_COD) from DBXSCHEMA.MENCUESTA_TRANSPORTES");
				if(consecutivoS.Equals(null) || consecutivoS.Equals(""))
				{
					consecutivo=1;
				}
				else
				{
					consecutivo=Convert.ToInt32(DBFunctions.SingleData("select max(MENCU_COD) from DBXSCHEMA.MENCUESTA_TRANSPORTES"))+1;
				}
				codencuesta.Text=consecutivo.ToString();
				fechaencuesta.Text=DateTime.Now.ToString("yyyy-MM-dd");
				
				bind.PutDatasIntoDropDownList(servicio,"Select MSEREN_DESC from DBXSCHEMA.MSERENCUESTA ");//carga el pais de origen
                			
			}
		}
		public void Grabar_Click(object sender, System.EventArgs e)
		{
			
			string fecha=DateTime.Now.ToString("yyyy-MM-dd");
			string nombreF=nombre.Text.ToString();
			int telefonoF=Convert.ToInt32(telefono.Text.ToString());
			int codserv=Convert.ToInt32(DBFunctions.SingleData("select MSEREN_CODIGO from DBXSCHEMA.MSERENCUESTA WHERE MSEREN_DESC='"+servicio.SelectedValue.ToString()+"' "));
			double califserv=Convert.ToDouble(calser.SelectedValue.ToString());
			string opservicio=opservi.Text.ToString();
			double calatenser=Convert.ToDouble(calaten.SelectedValue.ToString());
			string opiaten=opaten.Text.ToString();
			string opigene=opgen.Text.ToString();
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MENCUESTA_TRANSPORTES VALUES(DEFAULT,'"+fecha.ToString()+"','"+nombreF.ToString()+"',"+telefonoF+","+codserv+","+califserv+",'"+opservicio.ToString()+"',"+calatenser+",'"+opiaten.ToString()+"','"+opigene.ToString()+"'  )");
			//Label4.Text=DBFunctions.exceptions;  //labael para mostrar las excepciones al momento de insertar o modificar la base de datos.
			//Response.Write("<script language='javascript'>alert('Encuesta Creada Satisfactoriamente');</script>");
			//en este evento se guarda toda la informacion del formulario de remesas en la base de datos
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
