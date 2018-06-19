namespace AMS.Automotriz
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
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Automotriz_GestorDocVehiculos.
	/// </summary>
	public partial class AMS_Automotriz_GestorDocVehiculos : System.Web.UI.UserControl
	{
		//JFSC 11022008 Poner en comentario
	    //string CodigoCatalogo=null;
		string nit=null;
		string NombreFoto=null;
		string propietario=null;
		string url=null;
		protected void Page_Load(object sender, System.EventArgs e)

		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                //bind.PutDatasIntoDropDownList(VehiDrop,"select MDOCVEH_PLACA from DBXSCHEMA.MDOC_VEHICULOS ORDER BY MDOCVEH_PLACA");
                bind.PutDatasIntoDropDownList(VehiDrop, "SELECT MCAT_VIN FROM MVEHICULODOCUMENTO ORDER BY MCAT_VIN;");
            }
			imglupa.Attributes.Add("OnClick","ModalDialog("+VehiDrop.ClientID+ ",'SELECT MCAT_VIN FROM MVEHICULODOCUMENTO ORDER BY MCAT_VIN;' ,new Array() );");
		}
		public void Generar_Click(object sender, System.EventArgs e)
		{
			Panel1.Visible=true;
            //nit=DBFunctions.SingleData("select MNIT_NIT from DBXSCHEMA.MCATALOGOVEHICULO WHERE MCAT_PLACA ='"+VehiDrop.SelectedValue.ToString()+"' ");
            nit = DBFunctions.SingleData("select MNIT_NIT from DBXSCHEMA.MCATALOGOVEHICULO WHERE MCAT_VIN ='" + VehiDrop.SelectedValue.ToString() + "' ");
            propietario =DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nit+"'");
			PropLabel.Text=propietario.ToString();
			VehiLabel.Text=VehiDrop.SelectedValue.ToString();
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
            //NombreFoto="Default.jpg";
            NombreFoto = DBFunctions.SingleData("SELECT MVEH_RUTA FROM MVEHICULODOCUMENTO WHERE MCAT_VIN ='" + VehiDrop.SelectedValue.ToString() + "'");
            url =url + NombreFoto;
			//imagen.ImageUrl=url;
		
		}
		public void Cargar1_Click(object sender, System.EventArgs e)
		{
            //NombreFoto=DBFunctions.SingleData("select MDOCVEH_SOAT from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
            DataSet NombreFoto = new DataSet();
            DBFunctions.Request(NombreFoto, IncludeSchema.NO, "SELECT MVEH_RUTA FROM MVEHICULODOCUMENTO WHERE MCAT_VIN ='" + VehiDrop.SelectedValue.ToString() + "'");
            int j = 0;
            for (j=0; j<NombreFoto.Tables[0].Rows.Count; j++)

            {
                url =System.Configuration.ConfigurationManager.AppSettings["Uploads"];
			    url=url + NombreFoto.Tables[0].Rows[j][0];
			    Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=668,WIDTH=709');</script>");
                //imagen.ImageUrl=url;
            }


        }
		public void Cargar2_Click(object sender, System.EventArgs e)
		{
			NombreFoto=DBFunctions.SingleData("select MDOCVEH_TPROPIEDAD from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
			url=url + NombreFoto;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
			//imagen.ImageUrl=url;

		
		}
		public void Cargar3_Click(object sender, System.EventArgs e)
		{
			NombreFoto=DBFunctions.SingleData("select MDOCVEH_PGARANTIA from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
			url=url + NombreFoto;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
			//imagen.ImageUrl=url;

		
		}
		public void Cargar4_Click(object sender, System.EventArgs e)
		{
			NombreFoto=DBFunctions.SingleData("select MDOCVEH_DIMPORTACION from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
			url=url + NombreFoto;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
			//imagen.ImageUrl=url;

		
		}
		public void Cargar5_Click(object sender, System.EventArgs e)
		{
			NombreFoto=DBFunctions.SingleData("select MDOCVEH_IMPRONTAS from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
			url=url + NombreFoto;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
			//imagen.ImageUrl=url;
			
		
		}
		public void Cargar6_Click(object sender, System.EventArgs e)
		{
			NombreFoto=DBFunctions.SingleData("select MDOCVEH_PBATERIA from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
			url=url + NombreFoto;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
			//imagen.ImageUrl=url;

		
		}
		public void Cargar7_Click(object sender, System.EventArgs e)
		{
			NombreFoto=DBFunctions.SingleData("select MDOCVEH_FACTURA from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
			url=url + NombreFoto;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
			//imagen.ImageUrl=url;

		
		}
		public void Cargar8_Click(object sender, System.EventArgs e)
		{
			NombreFoto=DBFunctions.SingleData("select MDOCVEH_IEVEH from DBXSCHEMA.MDOC_VEHICULOS WHERE MDOCVEH_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			url=System.Configuration.ConfigurationManager.AppSettings["PathToImages"] +"DOC_VEHICULOS/" ;
			url=url + NombreFoto;
			Response.Write("<script language:javascript>w=window.open('"+url.ToString()+"','','menubar=0,resizable=1,HEIGHT=1024,WIDTH=768');</script>");
			//imagen.ImageUrl=url;

		
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

		}
		#endregion

		
	}
}