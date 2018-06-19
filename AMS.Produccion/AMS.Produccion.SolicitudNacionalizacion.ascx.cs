namespace AMS.Produccion
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
    using AMS.Tools;
 
	/// <summary>
	///		Descripción breve de AMS_Produccion_SolicitudNacionalizacion.
	/// </summary>
	public partial class AMS_Produccion_SolicitudNacionalizacion : System.Web.UI.UserControl
	{
		#region Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
                Utils.llenarPrefijos(Response, ref ddlPrefDoc, "%","%","SN");
				ddlPrefDoc_SelectedIndexChanged(sender,e);
				CargarVehiculos();
				if(Request.QueryString["upd"]!=null)
                Utils.MostrarAlerta(Response, "Se ha creado la solicitud de nacionalización de los vehículos.");
			}
		}

		//Cargar Vehiculos
		private void CargarVehiculos()
		{
			DataSet dsVehiculos=new DataSet();
			DBFunctions.Request(dsVehiculos,IncludeSchema.NO,
				"SELECT MC.MCAT_VIN, MC.MCAT_MOTOR, PC.PCOL_DESCRIPCION, MC.PCAT_CODIGO "+
				"FROM MCATALOGOVEHICULO MC, MVEHICULO MV, PCOLOR PC "+
				"WHERE MC.MCAT_VIN=MV.MCAT_VIN AND MC.PCOL_CODIGO=PC.PCOL_CODIGO AND MV.TEST_TIPOESTA=3 "+
				"ORDER BY MC.PCAT_CODIGO, MC.MCAT_VIN;");
			dgEnsambles.DataSource=dsVehiculos.Tables[0];
			dgEnsambles.DataBind();
			ViewState["VEHICULOS"]=dsVehiculos.Tables[0];
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

		//Seleccionar
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			DataTable dtVehiculos=(DataTable)ViewState["VEHICULOS"];
			ArrayList sqlStrings=new ArrayList();
			string numDoc=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefDoc.SelectedValue+"'");		
			for(int n=0;n<dgEnsambles.Items.Count;n++)
				if(((CheckBox)dgEnsambles.Items[n].FindControl("chkUsarE")).Checked){
					sqlStrings.Add(
						"INSERT INTO DSOLICITUDNACIONALIZACIONCKD VALUES("+
						"'"+ddlPrefDoc.SelectedValue+"',"+numDoc+","+
						"'"+dtVehiculos.Rows[n]["MCAT_VIN"]+"',"+
						"CURRENT TIMESTAMP);");
					sqlStrings.Add(
						"UPDATE MVEHICULO "+
						"SET MVEH_FECHDISP='"+DateTime.Now.ToString("yyyy-MM-dd")+"', TEST_TIPOESTA=5 "+
						"WHERE MCAT_VIN='"+dtVehiculos.Rows[n]["MCAT_VIN"]+"';");
				}
			sqlStrings.Add("UPDATE pdocumento set pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='"+ddlPrefDoc.SelectedValue+"';");
			if(sqlStrings.Count==0)
			{
                Utils.MostrarAlerta(Response, "No seleccionó vehículos.");
				return;
			}

			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(""+indexPage+"?process=Produccion.SolicitudNacionalizacion&path="+Request.QueryString["path"]+"&upd=1");
			else
				lblInfo.Text += "<br>Error : Detalles <br>"+DBFunctions.exceptions;
		}

		protected void ddlPrefDoc_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lblNumDoc.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefDoc.SelectedValue+"'");		
		}
	}
}