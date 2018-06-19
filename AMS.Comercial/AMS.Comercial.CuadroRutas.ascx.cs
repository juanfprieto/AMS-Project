namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
	using Ajax;
	using System.Configuration;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_CuadroRutas.
	/// </summary>
	public class AMS_Comercial_CuadroRutas : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlHora;
		protected System.Web.UI.WebControls.DropDownList ddlMinuto;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlProgramacion;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.CheckBox chkLunes;
		protected System.Web.UI.WebControls.CheckBox chkMartes;
		protected System.Web.UI.WebControls.CheckBox chkMiercoles;
		protected System.Web.UI.WebControls.CheckBox chkJueves;
		protected System.Web.UI.WebControls.CheckBox chkViernes;
		protected System.Web.UI.WebControls.CheckBox chkSabado;
		protected System.Web.UI.WebControls.CheckBox chkDomingo;
		protected System.Web.UI.WebControls.Label Label1;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_CuadroRutas));	
			if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlRuta,"select mrut_codigo,mrut_codigo from DBXSCHEMA.mrutas WHERE MRUT_CLASE=2 order by mrut_codigo;");
				ListItem itm=new ListItem("---Ruta---","");
				ddlRuta.Items.Insert(0,itm);
				bind.PutDatasIntoDropDownList(ddlProgramacion,"select tprog_tipoprog,tprog_nombre from DBXSCHEMA.tprogramacionruta");
				itm=new ListItem("---Programacion---","");
				ddlProgramacion.Items.Insert(0,itm);
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "El cuadro de rutas ha sido actualizado.");
				for(int i=0;i<24;i++)
					ddlHora.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				for(int i=0;i<60;i++)
					ddlMinuto.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
			}
		}

		[Ajax.AjaxMethod]
		public DataSet CargarCuadro(string ruta, int hora, int minuto, string programacion)
		{
			DataSet Vins= new DataSet();
			int mins=hora*60+minuto;
			string sqlAgencia="select rt.MRUT_DESCRIPCION AS DESC, co.PCIU_NOMBRE AS ORIG, cd.PCIU_NOMBRE AS DEST from DBXSCHEMA.mrutas rt, DBXSCHEMA.pciudad cd, DBXSCHEMA.PCIUDAD co WHERE co.PCIU_CODIGO=rt.PCIU_COD AND cd.PCIU_CODIGO=rt.PCIU_CODDES AND rt.MRUT_CODIGO='"+ruta+"';";
			sqlAgencia+="select MRUT_LUNES AS LUN,MRUT_MARTES AS MAR, MRUT_MIERCOLES AS MIE, MRUT_JUEVES AS JUE, MRUT_VIERNES AS VIE, MRUT_SABADO AS SAB, MRUT_DOMINGO AS DOM FROM DBXSCHEMA.MRUTA_CUADRO WHERE MRUT_CODIGO='"+ruta+"' AND MRUT_HORA="+mins+" AND TPROG_TIPOPROG="+programacion+";";
			DBFunctions.Request(Vins,IncludeSchema.NO,sqlAgencia);
			return Vins;
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
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Guardar cuadro
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			string mruta=ddlRuta.SelectedValue.Trim();
			string programacion=ddlProgramacion.SelectedValue.Trim();
			string sqlP="";
			int mins;
			if(mruta.Length==0||programacion.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar la ruta y el tipo de programacion.");
				return;
			}
			//Hora
			try{
				mins=int.Parse(ddlHora.SelectedValue)*60+int.Parse(ddlMinuto.SelectedValue);
			}
			catch{
                Utils.MostrarAlerta(Response, "Hora no valida.");
				return;
			}
			//Eliminar anteriores
			sqlP="delete from dbxschema.mruta_cuadro WHERE MRUT_CODIGO='"+mruta+"' AND MRUT_HORA="+mins+" AND TPROG_TIPOPROG="+programacion+";";
			DBFunctions.NonQuery(sqlP);
			//Insertar actual
			sqlP="insert into dbxschema.mruta_cuadro values('"+mruta+"',"+mins+","+programacion+",'"+(chkLunes.Checked?"S":"N")+"','"+(chkMartes.Checked?"S":"N")+"','"+(chkMiercoles.Checked?"S":"N")+"','"+(chkJueves.Checked?"S":"N")+"','"+(chkViernes.Checked?"S":"N")+"','"+(chkSabado.Checked?"S":"N")+"','"+(chkDomingo.Checked?"S":"N")+"');";
			DBFunctions.NonQuery(sqlP);
			Response.Redirect(indexPage+"?process=Comercial.CuadroRutas&path="+Request.QueryString["path"]+"&act=1");
		}
	}
}
