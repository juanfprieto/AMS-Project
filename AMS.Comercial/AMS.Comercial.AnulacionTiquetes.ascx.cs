namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Microsoft.Web.UI.WebControls;
	using AMS.DB;
	using AMS.Forms;
	using Ajax;
	using AMS.DBManager;
    using AMS.Tools;

	/// <summary>
	///	Descripción breve de AMS_Comercial_AnulacionTiquetes.
	/// </summary>
	public class AMS_Comercial_AnulacionTiquetes : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox txtTiquete;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlMotivo;
		protected System.Web.UI.WebControls.Button btnAnular;
		protected System.Web.UI.WebControls.Label lblError;
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlMotivo,
					"select TCON_CODIGO as valor, TCON_DESCRIPCION as texto from DBXSCHEMA.TCONCEPTO_ANULACION_TIQUETE ORDER BY TCON_DESCRIPCION;");
				ddlMotivo.Items.Insert(0,new ListItem("---seleccione---",""));
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "El tiquete ha sido anulado.");
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
			this.btnAnular.Click += new System.EventHandler(this.btnAnular_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnAnular_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue,anulacion=ddlMotivo.SelectedValue,tiqueteS;
			int tiquete;

			//Validaciones
			if(anulacion.Length==0){
                Utils.MostrarAlerta(Response, "Debe dar el motivo de la anulación.");
				return;}
			//Tiquete
			//try{
			//    tiquete=Convert.ToInt32(txtTiquete.Text);
			//    if(tiquete>=Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)+999999 ||tiquete<0)
			//    //if(tiquete<0)
			//        throw(new Exception());
			//}
			//catch{
			//    Response.Write("<script language='javascript'>alert('Debe dar un número de tiquete valido.');</script>");
			//    return;}

			tiqueteS = txtTiquete.Text;
			
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
            Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado.");
			//Validar tiquete
			//if(Tiquetes.lenTiquete-tiqueteS.Length<0){
			//    Response.Write("<script language='javascript'>alert('Debe dar un número de tiquete valido.');</script>");
			//    return;}
			//administrador del sistema adminck
			string adminck=DBFunctions.SingleData("select TTIPE_CODIGO from DBXSCHEMA.susuario where susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "';");
	

			if(tiqueteS.Length<=6){
                Utils.MostrarAlerta(Response, "El tiquete pertenece a papeleria manual de imprenta.");
				return;
			}

			//tiqueteS=new string('0',Tiquetes.lenTiquete-tiqueteS.Length)+tiqueteS;
			//Consultar numero de tiquete real (>1'000.000)
			
			DataSet dsTiquete=new DataSet();
			DBFunctions.Request(dsTiquete,IncludeSchema.NO,
				"Select * from DBXSCHEMA.MCONTROL_PAPELERIA where "+
				"TDOC_CODIGO='TIQ' AND RTRIM(CHAR(NUM_DOCUMENTO)) like('"+tiqueteS+"') AND TIPO_DOCUMENTO='V' AND "+
				"FECHA_ASIGNACION IS NOT NULL AND FECHA_USO IS NOT NULL AND FECHA_ANULACION IS NULL "+
				"ORDER BY NUM_DOCUMENTO DESC;");
			if(dsTiquete.Tables[0].Rows.Count==0){
                Utils.MostrarAlerta(Response, "No existe el tiquete virtual.");
				return;}

			string tiqueteR=dsTiquete.Tables[0].Rows[0]["NUM_DOCUMENTO"].ToString();
			dsTiquete=new DataSet();
			DBFunctions.Request(dsTiquete,IncludeSchema.NO,
				"SELECT * FROM DBXSCHEMA.MTIQUETE_VIAJE "+
				"WHERE TDOC_CODIGO='TIQ' AND TEST_CODIGO='V' AND NUM_DOCUMENTO="+tiqueteR+";");
			if(dsTiquete.Tables[0].Rows.Count==0){
                Utils.MostrarAlerta(Response, "No existe el tiquete vendido.O Anulado");
				return;}

			//Planilla
			string planilla=dsTiquete.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			string numViaje=DBFunctions.SingleData("SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla+" AND MAG_CODIGO="+agencia+" AND FECHA_LIQUIDACION IS NULL;");
			if(numViaje.Length==0){
                Utils.MostrarAlerta(Response, "La planilla no existe o ya ha sido liquidada.");
				return;}

			//Valida que no sea de un dia anterior el tiquete
			DateTime fechaRep = (DateTime)dsTiquete.Tables[0].Rows[0]["FECHA_REPORTE"];
			 if  (adminck != "AS") 
			{
			if (!(fechaRep.Year == DateTime.Now.Year && fechaRep.Month == DateTime.Now.Month && fechaRep.Day == DateTime.Now.Day))
			{
                Utils.MostrarAlerta(Response, "El tiquete pertenece a dias anteriores.");
				return;
			}
			}


			//valida fecha de cierre
			DataSet dsCierre=new DataSet();
			string cierre = DBFunctions.SingleData("select MAG_CODIGO from DBXSCHEMA.DCIERRE_DIARIO_AGENCIA Where mag_codigo=" + agencia + " and fecha_contabilizacion='" + DateTime.Now + "';");
			if (cierre != "")
			{
                Utils.MostrarAlerta(Response, "Ya se ha registrado la fecha de cierre de contabilizacion para este tiquete.");
				return;
			}

			//Actualizar tiquete
			ArrayList sqlUpd=new ArrayList();
			sqlUpd.Add("UPDATE dbxschema.mtiquete_viaje  set TEST_CODIGO='A' where TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+tiqueteR+";");
			sqlUpd.Add("INSERT INTO dbxschema.MTIQUETE_VIAJE_ANULADO values('TIQ',"+tiqueteR+","+anulacion+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
			sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET NUM_ANULACION=0, COD_ANULACION="+anulacion+",MNIT_ANULACION='"+nitResponsable+"', FECHA_ANULACION='"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"' WHERE TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+tiqueteR+";");
			//Vaciar puestos
			sqlUpd.Add("UPDATE DBXSCHEMA.MCONFIGURACIONPUESTO SET TDOC_CODIGO=NULL, NUM_DOCUMENTO=NULL, TEST_CODIGO='DN' WHERE TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+tiqueteR+";");

			if(DBFunctions.Transaction(sqlUpd)){
				if(Tools.Browser.IsMobileBrowser())
					Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process=Comercial.AnulacionTiquetes&path="+Request.QueryString["path"]+"&act=1");
				else
					Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.AnulacionTiquetes&path="+Request.QueryString["path"]+"&act=1");
			}
			else
				lblError.Text=DBFunctions.exceptions;
		}
	}
}
