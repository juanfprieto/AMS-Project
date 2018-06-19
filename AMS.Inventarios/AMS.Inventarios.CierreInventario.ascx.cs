
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
using AMS.Tools;
namespace AMS.Inventarios
{
	/// <summary>
	///		Descripción breve de AMS_Inventarios_CierreInventario.
	/// </summary>
	public partial class AMS_Inventarios_CierreInventario : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Button btnSeleccionar;
		#endregion Controles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                //bind.PutDatasIntoDropDownList(ddlPrefInventario, "SELECT PDOC_CODIGO FROM PDOCUMENTO WHERE TDOC_TIPODOCU='IF' and tvig_vigencia = 'V' ;");
                Utils.llenarPrefijos(Response, ref ddlPrefInventario , "%", "%", "IF");
				bind.PutDatasIntoDropDownList(ddlCentro, "SELECT pcen_codigo, pcen_nombre FROM pcentrocosto");
				bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor where pven_vigencia = 'V'");
				txtFechaProceso.Text = DateTime.Now.ToString("yyyy-MM-dd");
				ddlPrefInventario_SelectedIndexChanged(ddlPrefInventario,e);
				if(Request.QueryString["inv"]!=null)
                    Utils.MostrarAlerta(Response, "El inventario " + Request.QueryString["inv"] + " ha sido cerrado.");
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
		
		//Cambia el numero
		protected void ddlNumeroInventario_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DataSet dsItems=null;
			if(ddlNumInventario.Items.Count>0){
				CargarItems(ref dsItems);
				dgrItems.Visible=true;
				dgrItems.DataSource=dsItems.Tables[0];
				dgrItems.DataBind();}
			else
				dgrItems.Visible=false;
		}
		//Cargar Items
		private void CargarItems(ref DataSet dsItems){
			dsItems=new DataSet();
			DBFunctions.Request(dsItems,IncludeSchema.NO,
				"SELECT * "+
				"FROM DINVENTARIOFISICO "+
				"WHERE PDOC_CODIGO='"+ddlPrefInventario.SelectedValue+"' AND MINF_NUMEROINV="+ddlNumInventario.SelectedValue+" AND DINV_DIFERENCIA<>0;");
		}
		//Cambia el prefijo
		protected void ddlPrefInventario_SelectedIndexChanged(object sender, System.EventArgs e){
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNumInventario,"SELECT MINF_NUMEROINV FROM MINVENTARIOFISICO WHERE PDOC_CODIGO='"+ddlPrefInventario.SelectedValue+"' AND MINF_FECHACIERRE IS NULL;");
			ddlNumeroInventario_SelectedIndexChanged(ddlNumInventario,e);
		}

		//Cerrar Inventario
		protected void btnCerrar_Click(object sender, System.EventArgs e)
		{
			DataSet dsItems=null;
			uint numReferencia;
			DateTime fechaProceso;
			ArrayList sqlA=new ArrayList();
			int tm=50;
			double cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA;
			string codI = "";
			string codalmacen = "";
            int tarjeta = 0;
			string nnit = DBFunctions.SingleData("SELECT mnit_nit from cempresa;");
			string vend=ddlVendedor.SelectedItem.Value;
			string carg=DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vend+"'");
			string ccos=ddlCentro.SelectedValue;
			string pano = DBFunctions.SingleData("SELECT pano_ano FROM cinventario");
			
			//Validaciones
			if(DBFunctions.RecordExist("SELECT dinv_tarjeta from DINVENTARIOFISICO "+
				"WHERE DINV_CONTDEFINITIVO IS NULL AND PDOC_CODIGO='"+ddlPrefInventario.SelectedValue+"' AND MINF_NUMEROINV="+ddlNumInventario.SelectedValue+";"))
			{
                Utils.MostrarAlerta(Response, "El inventario no se puede cerrar porque existen tarjetas sin contar.");
				return;
			}
			if(ddlNumInventario.Items.Count==0 || ddlNumInventario.SelectedValue.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar el número y prefijo del inventario.");
				return;}
			try{
				numReferencia=Convert.ToUInt16(tbPrefNumRef.Text.Trim());
				if(tbPrefDocRef.Text.Trim().Length==0)throw(new Exception());}
			catch{
                Utils.MostrarAlerta(Response, "Debe ingresar un número y prefijo de referencia válidos.");
				return;}
			try{
				fechaProceso=Convert.ToDateTime(txtFechaProceso.Text);}
			catch{
                Utils.MostrarAlerta(Response, "Debe ingresar una fecha de proceso válida.");
				return;}
			
			Movimiento Mov=
				new Movimiento(
				ddlPrefInventario.SelectedValue,Convert.ToUInt16(ddlNumInventario.SelectedValue),
				tbPrefDocRef.Text.Trim(),numReferencia,
				tm,nnit,fechaProceso,vend,carg,ccos,"N");
			
			CargarItems(ref dsItems);
			
			foreach(DataRow dr in dsItems.Tables[0].Rows)
			{
				codI       = dr["DINV_MITE_CODIGO"].ToString();
				codalmacen = dr["DINV_PALM_ALMACEN"].ToString();
				cant       = Convert.ToDouble(dr["DINV_DIFERENCIA"]);//cantidfad facturada
				valU       = Convert.ToDouble(dr["DINV_COSTPROM"]);//valor unidad
                tarjeta    = Convert.ToInt16(dr["DINV_TARJETA"]);//valor unidad
				try{costP  = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitem WHERE pano_ano="+pano+" AND mite_codigo='"+codI+"'"));}
				catch{costP=0;}
				try{costPH = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitem WHERE pano_ano="+pano+" AND mite_codigo='"+codI+"'"));}
				catch{costPH=0;}
				try{costPA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitemalmacen WHERE pano_ano="+pano+" AND palm_almacen='"+dr["DINV_PALM_ALMACEN"]+"' AND mite_codigo='"+codI+"'"));}
				catch{costPA=0;}
				try{costPHA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitemalmacen WHERE pano_ano="+pano+" AND palm_almacen='"+dr["DINV_PALM_ALMACEN"]+"' AND mite_codigo='"+codI+"'"));}
				catch{costPHA=0;}
				try{invI = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitem WHERE pano_ano="+pano+" AND mite_codigo='"+codI+"'"));}
				catch{invI=0;}
				try{invIA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitemalmacen WHERE pano_ano="+pano+" AND palm_almacen='"+dr["DINV_PALM_ALMACEN"]+"' AND mite_codigo='"+codI+"'"));}
				catch{invIA=0;}
				pIVA=0; //iva
				pDesc=0; //descuento
				cantDev=0;//devolucion
				try
				{
					valP=Convert.ToDouble(DBFunctions.SingleData("SELECT msal_valpubl from MITEMS where mite_codigo='"+codI+"'"));}//Este campo no existe dentro de la base de datos ??????????//Se dice que tomar el valor de dite_valounit, pero esa tabla no tiene valores ademas dependen de un documento y numero de documento??? DUDAS
				catch
				{
					valP=0;}
				Mov.InsertaFila(codI,cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA,0,codalmacen,tarjeta);
				//codigo, cantidad, valUnitario, costoPromedio, costoPromedioAlmacen,porcentajeIVA,porcentajeDescuento,cantidadDevolucion,costopromediohistorico,costopromediohistoricoalmacen,valorPublico,inventiarioinicial, iunventarioinicialalmacen
				//0			1			2			3				4					5			6					7						8						9						  10			11					12	
			}
			
			sqlA.Add(
				"UPDATE MINVENTARIOFISICO SET MINF_FECHACIERRE='"+txtFechaProceso.Text+"' "+
				"WHERE PDOC_CODIGO='"+ddlPrefInventario.SelectedValue+"' AND MINF_NUMEROINV="+ddlNumInventario.SelectedValue+";");
			
			if(Mov.CerrarInventario(true,sqlA)){
				string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                Response.Redirect("" + indexPage + "?process=Inventarios.CierreInventario&path=" + Request.QueryString["path"] + "&inv=" + ddlPrefInventario.SelectedValue + "-" + ddlNumInventario.SelectedValue + "");
			}
			else
				lbInfo.Text += Mov.ProcessMsg;
		}
	}
}