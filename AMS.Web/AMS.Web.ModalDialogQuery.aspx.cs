// created on 27/09/2004 at 10:11
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;

namespace AMS.Web
{
	public partial class ModalDialogQuery : System.Web.UI.Page
	{
		
		protected DataTable consulta;
				
		public ModalDialogQuery()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}		
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			BindDatas();
		}

		private void InitializeComponent()
		{

		}
		
		protected void BindDatas()
		{
			//Parametros recibidos : 
			//NombreProcedimientoAlmacenado : nomSecProc
			//NumeroParametros : numKeysEnc
			//NombreParametro n: nomCajNegn
			//ParametroPosicion n : EntCajNegn			
			try
			{
				int cantidadParametros = Convert.ToInt32(Request.QueryString["numKeysEnc"]);
				Hashtable hsParams = new Hashtable();
				for(int i=1;i<=cantidadParametros;i++)
					hsParams.Add(Request.QueryString["nomCajNeg"+i],Request.QueryString["EntCajNeg"+i]);
				DataSet dsConsulta = new DataSet();
				DBFunctions.Request(dsConsulta,IncludeSchema.NO,"DBXSCHEMA."+Request.QueryString["nomSecProc"],hsParams.GetEnumerator());
				dgTable.DataSource = dsConsulta.Tables[0];
				dgTable.DataBind();
			}
			catch{Page.RegisterClientScriptBlock("status","<script>alert('Se ha presentado un error al realizar la consulta');window.close();</script>");}
		}
	}
}
