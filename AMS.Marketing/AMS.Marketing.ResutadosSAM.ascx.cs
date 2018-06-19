namespace AMS.Marketing
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.Mail;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text;
	using System.IO;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Marketing_ResutadosSAM.
	/// </summary>
	public partial class AMS_Marketing_ResutadosSAM : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlResultado,"SELECT pres_codigo, pres_descripcion from DBXSCHEMA.PRESULTADOACTIVIDAD order by pres_descripcion;");
				ddlResultado.Items.Insert(0,new ListItem("--Todos--",""));
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

		protected void btnGenerarExcel_Click(object sender, System.EventArgs e)
		{
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename=ResultadosSAM.xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			System.IO.StringWriter stringWrite = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
			dgReporte.RenderControl(htmlWrite);
			Response.Write(stringWrite.ToString());
			Response.End();
		}

		protected void btnGenerar_Click(object sender, System.EventArgs e)
		{
			if(fechaInicial.SelectedDate>=fechaFinal.SelectedDate)
			{
                Utils.MostrarAlerta(Response, "La fecha inicial debe ser anterior a la fecha final");
				return;
			}
			DataSet dsResultado=new DataSet();
			string sqlR="SELECT MN.mnit_nit CC,MN.mnit_nombres CONCAT ' ' CONCAT COALESCE(MN.mnit_nombre2,'') CONCAT ' ' concat MN.mnit_apellidos CONCAT ' ' concat COALESCE(MN.mnit_apellido2,'') AS Nombre,"+
			"PV.PVEN_NOMBRE AS PVEN_NOMBRE,MN.MNIT_EMAIL AS EMAIL,COALESCE(MN.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(MN.MNIT_CELULAR,'') AS TELS, DM.DMAR_FECHACTI AS FECHA, "+
			"PA.PACT_NOMBMARK AS PACT_NOMBMARK, PR.PRES_DESCRIPCION PRES_DESCRIPCION, DM.DMAR_DETALLE DMAR_DETALLE "+
			"FROM DBXSCHEMA.MNIT MN,DBXSCHEMA.PVENDEDOR PV, DBXSCHEMA.DMARKETING DM,DBXSCHEMA.PRESULTADOACTIVIDAD PR, DBXSCHEMA.PACTIVIDADMARKETING PA "+
			"WHERE DM.MNIT_NIT=MN.MNIT_NIT AND DM.PVEN_CODIGO=PV.PVEN_CODIGO AND PR.PRES_CODIGO=DM.PRES_CODIGO AND PA.PACT_CODIMARK=DM.PACT_CODIMARK "+
			"and DMAR_FECHACTI BETWEEN '"+fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"' AND '"+fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"' "+
			(ddlResultado.SelectedValue.Length>0?" and DM.PRES_CODIGO='"+ddlResultado.SelectedValue+"' ":"")+
			"ORDER BY CC;";
			DBFunctions.Request(dsResultado,IncludeSchema.NO,sqlR);
			dgReporte.DataSource=dsResultado.Tables[0];
			dgReporte.DataBind();
			pnlExcel.Visible=true;
			phReporte.Visible=true;
			lbRep.Text="REPORTE RESULTADOS S.A.M ENTRE "+fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+" y "+fechaFinal.SelectedDate.ToString("yyyy-MM-dd");
		}
	}
}
