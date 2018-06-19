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
	///		Descripción breve de AMS_Comercial_ModificarRuta.
	/// </summary>
	public class AMS_Comercial_ModificarRuta : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox txtDescripcion;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.DropDownList ddlClase;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlCiudadOrigen;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.DropDownList ddlCiudadDestino;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtDistancia;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtHrsRecorrido;
		protected System.Web.UI.WebControls.TextBox txtMnsRecorrido;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.TextBox txtValorMin;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtValorMax;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtValorSugerido;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.TextBox txtCodigo;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtValPeso;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtValVol;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox TxtValorConduce;
		protected System.Web.UI.WebControls.Label Label11;
		//protected System.Web.UI.WebControls.CheckBox chkParadero;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
        protected System.Web.UI.WebControls.TextBox Txtsinsegurida;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_ModificarRuta));	
			if (!IsPostBack){
				DatasToControls bind=  new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlRuta,"select mrut_codigo,mrut_codigo concat': ' concat co.pciu_nombre concat' - ' concat cd.pciu_nombre as texto "+
													  "from DBXSCHEMA.mrutas r,DBXSCHEMA.pciudad co,DBXSCHEMA.pciudad cd "+
													  "where co.pciu_codigo = pciu_cod and cd.pciu_codigo = pciu_coddes order by mrut_codigo;");
				ListItem itm=new ListItem("Teclee nueva ruta ----->","");
				ddlRuta.Items.Insert(0,itm);
				bind.PutDatasIntoDropDownList(ddlClase,"select tcls_codigo,tcls_descripcion from DBXSCHEMA.tclaseruta tcls_codigo");
				bind.PutDatasIntoDropDownList(ddlCiudadOrigen,"select pciu_codigo,pciu_nombre concat'(' concat pciu_codigoonal concat ')' from DBXSCHEMA.pciudad order by pciu_orden,pzon_zona,pciu_nombre");
				bind.PutDatasIntoDropDownList(ddlCiudadDestino,"select pciu_codigo,pciu_nombre concat'(' concat pciu_codigoonal concat ')' from DBXSCHEMA.pciudad order by pciu_orden,pzon_zona,pciu_nombre");
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  La ruta ha sido modificada.");
			}
		}
		[Ajax.AjaxMethod]
		public DataSet CargarRuta(string mruta)
		{
			DataSet Vins= new DataSet();
            string sqlRuta = "select MRUT_DESCRIPCION AS DESC, MRUT_CLASE AS CLASE, PCIU_COD AS ORIG, PCIU_CODDES AS DEST, MRUT_DISTANCIA AS DIST, MRUT_TIEMPO AS TIEMPO, MRUT_VALMAX AS VMAX, MRUT_VALMIN AS VMIN, MRUT_VALSUG AS VSUG, VALOR_PESO AS VPESO, VALOR_VOLUMEN AS VVOL,VALOR_CONDUCE AS VCONDUCE, MRUT_PARADERO AS PARADERO, mrut_sancionsegusocial AS SANCION FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='" + mruta + "';";
			DBFunctions.Request(Vins,IncludeSchema.NO,sqlRuta);
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

		//Modificar ruta
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			double distancia,tiempo;
			string codigo, descripcion,paradero;
			int minutos, horas;
			int valmin,valmax,valsugerido,valpeso,valvolumen;
			double ValorConduce;
			bool nueva=false;
			//Validaciones
			codigo=ddlRuta.SelectedValue;
			descripcion=txtDescripcion.Text.Trim();
			//paradero=chkParadero.Checked?"S":"N";
            paradero = "N";
			//Nueva?
			if(codigo.Length==0){
				codigo=txtCodigo.Text.Trim().ToUpper();
				nueva=true;
				if(codigo.Length==0){
                    Utils.MostrarAlerta(Response, "  Debe dar el código de la nueva ruta.");
					return;}
				if(DBFunctions.RecordExist("SELECT MRUT_CODIGO from DBXSCHEMA.MRUTAS where MRUT_CODIGO='"+codigo+"'")){
                    Utils.MostrarAlerta(Response, "  El nuevo código ya ha sido registrado.");
					return;}
			}
			if(descripcion.Length==0){
                Utils.MostrarAlerta(Response, "  Debe dar la descripción de la ruta.");
				return;}
			if(ddlCiudadDestino.SelectedValue.Equals(ddlCiudadOrigen.SelectedValue)){
                Utils.MostrarAlerta(Response, "  El origen es igual al destino.");
				return;}
			try{
				distancia=double.Parse(txtDistancia.Text);}
			catch{
                Utils.MostrarAlerta(Response, "  Distancia no válida.");
				return;}
			try{
				minutos=int.Parse(txtMnsRecorrido.Text);
				horas=int.Parse(txtHrsRecorrido.Text);
				if(minutos<0||horas<0)throw(new Exception());
				tiempo=(double)horas+((double)minutos)/60;
			}
			catch{
                Utils.MostrarAlerta(Response, "  Tiempo de recorrido aproximado no válido.");
				return;}
			try{
				valmin=int.Parse(txtValorMin.Text.Replace(",",""));
				valmax=int.Parse(txtValorMax.Text.Replace(",",""));
				valsugerido=int.Parse(txtValorSugerido.Text.Replace(",",""));
				if(valmin>valmax||valsugerido>valmax||valsugerido<valmin)throw(new Exception());
				valpeso=int.Parse(txtValPeso.Text.Replace(",",""));
				valvolumen=int.Parse(txtValVol.Text.Replace(",",""));
			}
			catch{
                Utils.MostrarAlerta(Response, "  Uno de los valores no es válido.");
				return;}
			try
			{
				ValorConduce=double.Parse(TxtValorConduce.Text.Replace(",",""));}
			catch
			{
                Utils.MostrarAlerta(Response, "  Valor Conduce no válida.");
				return;}
			if(!nueva){
				//Cambiar a clase intermedia?
				if(ddlClase.SelectedValue.Equals("1") && DBFunctions.RecordExist("SELECT MRUTA_PRINCIPAL FROM MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL='"+codigo+"';")){
                    Utils.MostrarAlerta(Response, "  No puede cambiar la clase de ruta a intermedia pues tiene rutas asociadas en su recorrido.");
					return;
				}
			}
			//Eliminar cod anterior
			string sqlRuta;
			if(nueva)
                sqlRuta = "insert into dbxschema.MRUTAS values ('" + codigo + "','" + descripcion + "'," + ddlClase.SelectedValue + ",'" + ddlCiudadOrigen.SelectedValue + "','" + ddlCiudadDestino.SelectedValue + "'," + distancia.ToString("0.##") + "," + tiempo.ToString("0.##") + "," + valmax.ToString("0.##") + "," + valmin.ToString("0.##") + "," + valsugerido.ToString("0.##") + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + valpeso.ToString("0.##") + "," + valvolumen.ToString("0.##") + "," + ValorConduce + ",'" + paradero + "'," + Txtsinsegurida.Text + ");";
			else
				sqlRuta="update dbxschema.MRUTAS set MRUT_DESCRIPCION='"+descripcion+"',MRUT_CLASE="+ddlClase.SelectedValue+",PCIU_COD='"+ddlCiudadOrigen.SelectedValue+"',PCIU_CODDES='"+ddlCiudadDestino.SelectedValue+"',MRUT_DISTANCIA="+distancia.ToString("0.##")+",MRUT_TIEMPO="+tiempo.ToString("0.##")+",MRUT_VALMAX="+valmax.ToString("0.##")+",MRUT_VALMIN="+valmin.ToString("0.##")+",MRUT_VALSUG="+valsugerido.ToString("0.##")+",MRUT_FECHA='"+DateTime.Now.ToString("yyyy-MM-dd")+"', VALOR_PESO="+valpeso.ToString("0.##")+", VALOR_VOLUMEN="+valvolumen.ToString("0.##")+",Valor_conduce="+ValorConduce+", MRUT_PARADERO='"+paradero+"'"+",MRUT_SANCIONSEGUSOCIAL="+Txtsinsegurida.Text+" WHERE MRUT_CODIGO='"+codigo+"';";
			DBFunctions.NonQuery(sqlRuta);
			Response.Redirect(indexPage+"?process=Comercial.ModificarRuta&path="+Request.QueryString["path"]+"&act=1");		
		}
	}
}
