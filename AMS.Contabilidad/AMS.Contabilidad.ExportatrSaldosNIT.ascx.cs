namespace AMS.Contabilidad
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.Globalization;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Mail;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Tools;
	using AMS.Forms;

	/// <summary>
	///		Descripción breve de AMS_Contabilidad_ExportatrSaldosNIT.
	/// </summary>
	public partial class AMS_Contabilidad_ExportatrSaldosNIT : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList( ddlAno, "SELECT pano_ano FROM pano WHERE PANO_ANO IN (SELECT DISTINCT PANO_ANO FROM MSaldoCuenta) ORDER BY 1" );
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

		}
		#endregion

		protected void btnGenerar_Click(object sender, System.EventArgs e)
		{
			//Validar
			int ano;
			string procedimiento="";
			try{ano=int.Parse(ddlAno.SelectedValue);}
            catch
            {
                Utils.MostrarAlerta(Response, "Año no valido"); return;
            
            }
			if(txtInicial.Text.Length==0||txtFinal.Text.Length==0)
            {
                Utils.MostrarAlerta(Response, "Debe dar las cuentas"); return; 
            }
                
			DataSet dsSaldos=new DataSet();
			procedimiento=(rdbOpcion.SelectedValue=="0")?"SALDOS_NIT":"SALDOS_TERCEROS_CUENTA";
			DBFunctions.Request(dsSaldos,IncludeSchema.NO,"call dbxschema."+procedimiento+"("+ano+",'"+txtInicial.Text+"','"+txtFinal.Text+"');");
			if(dsSaldos.Tables[0].Rows.Count==0)
            {
                Utils.MostrarAlerta(Response, "No se encontró información");return;
            }
			dgrSaldos.DataSource=dsSaldos.Tables[0];
			dgrSaldos.DataBind();

			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename=saldosNIT.xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			System.IO.StringWriter stringWrite = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

			dgrSaldos.RenderControl(htmlWrite);
			Response.Write(stringWrite.ToString());
			Response.End();
		}
	}
}
