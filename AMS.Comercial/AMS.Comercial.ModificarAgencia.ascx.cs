namespace AMS.Comercial
{
	using System;
	using System.Data;
    using System.Collections;
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
	///		Descripción breve de AMS_Comercial_ModificarAgencia.
	/// </summary>
	public class AMS_Comercial_ModificarAgencia : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox txtCodigo;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox txtNombre;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlCiudad;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtDireccion;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtTelefono;
		protected System.Web.UI.WebControls.Label Label15,lblError;
		protected System.Web.UI.WebControls.TextBox txtEncargado;
		protected System.Web.UI.WebControls.TextBox txtEncargadoa;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtPorcComision;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList rdbIVA;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.DropDownList ddlRegIVA;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.DropDownList rdbRotacion;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.DropDownList rdbActivo;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlSistema;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlPrefijo;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtNIT;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.DropDownList ddlChequeo;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_ModificarAgencia));	
			if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlAgencia,"select mag_codigo,rtrim(char(mag_codigo)) concat ' - ' concat mage_nombre from DBXSCHEMA.magencia");
				ListItem itm=new ListItem("Teclee nueva agencia ----->","");
				ddlAgencia.Items.Insert(0,itm);
				bind.PutDatasIntoDropDownList(ddlCiudad,"select pciu_codigo,pciu_nombre concat'(' concat pciu_codigoonal concat ')' from DBXSCHEMA.pciudad order by pciu_orden,pzon_zona,pciu_nombre");
				bind.PutDatasIntoDropDownList(ddlRegIVA,"select treg_regiiva,treg_nombre from DBXSCHEMA.tregimeniva");
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  La agencia ha sido modificada.");
			}
		}

		[Ajax.AjaxMethod]
		public DataSet CargarAgencia(string magencia)
		{
			DataSet Vins= new DataSet();
			string sqlAgencia="select ma.MAG_CODIGO COD,ma.MAGE_NOMBRE NOM, ma.PCIU_CODIGO AS CIU, ma.MAGE_DIRECCION AS DIR, ma.MAGE_TELEFONO AS TEL, ma.MNIT_ENCARGADO AS NIT, ma.MAGE_PORC_COMISION AS COMI, ma.MAGE_IVA AS TIVA, ma.TREG_IVA AS RIVA, ma.MAGE_ROTACION AS PROT, ma.MAGE_ESTADO AS EST, ma.MAGE_FECHA AS FCH, mn.MNIT_NOMBRES concat ' ' concat mn.MNIT_APELLIDOS AS NOMNIT, ma.MAGE_SISTEMA AS SIS, ma.PREFIJO AS PREF, ma.MNIT_EMPRESA as NITEMP, ma.CHEQUEO AS CHEQUEO FROM DBXSCHEMA.MAGENCIA ma, DBXSCHEMA.MNIT mn WHERE ma.MNIT_ENCARGADO=mn.MNIT_NIT AND ma.MAG_CODIGO="+magencia+";";
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

		//Modificar agencia
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			double porcComision;
			int codigoV=0;
			string codigo, nombre, nit;
			bool nueva=false;
			//Validaciones
			codigo=ddlAgencia.SelectedValue;
			nombre=txtNombre.Text.Trim();
			nit=txtNIT.Text.Trim();
			//Nueva?
			if(codigo.Length==0){
				codigo=txtCodigo.Text.Trim().ToUpper();
				nueva=true;
				if(codigo.Length==0)
				{
                    Utils.MostrarAlerta(Response, "  Debe dar el código de la nueva agencia.");
					return;}
				if(DBFunctions.RecordExist("SELECT MAG_CODIGO from DBXSCHEMA.MAGENCIA where MAG_CODIGO="+codigo))
				{
                    Utils.MostrarAlerta(Response, "  El nuevo código ya ha sido registrado.");
					return;}
			}
			//Codigo
			try	{
				codigoV=int.Parse(codigo);
				if(codigoV<1000)throw(new Exception());
			}
			catch{
                Utils.MostrarAlerta(Response, "  El código debe ser un número entero de 4 digitos.");
				return;
			}
			//Nombre
			if(nombre.Length==0){
                Utils.MostrarAlerta(Response, "  Debe dar el nombre de la agencia.");
				return;}
			//NIT
			if(nit.Length==0){
                Utils.MostrarAlerta(Response, "  Debe dar el nit de la agencia.");
				return;}
			//Encargado
			if(txtEncargado.Text.Length==0){
                Utils.MostrarAlerta(Response, "  Debe dar el nit del encargado.");
				return;
			}
			//% comision
			try{
				porcComision=double.Parse(txtPorcComision.Text.Replace(",",""));
				if(porcComision<0||porcComision>100)throw(new Exception());
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  El porcentaje de comisión no es válido.");
				return;}
			//IVA
			if(rdbIVA.SelectedIndex<0){
                Utils.MostrarAlerta(Response, "  Debe especificar si hay IVA.");
				return;}
			//Rotacion
			if(rdbRotacion.SelectedIndex<0){
                Utils.MostrarAlerta(Response, "  Debe especificar si hay rotación.");
				return;}
			//Rotacion
			if(rdbActivo.SelectedIndex<0){
                Utils.MostrarAlerta(Response, "  Debe especificar si la agencia está activa.");
				return;}
			//Eliminar cod anterior
            ArrayList sqlB = new ArrayList();
			if(nueva)
                sqlB.Add("insert into dbxschema.MAGENCIA values (" + codigoV + ",'" + nombre + "','" + ddlCiudad.SelectedValue + "','" + txtDireccion.Text.Trim() + "','" + txtTelefono.Text.Trim() + "','" + txtEncargado.Text + "'," + porcComision.ToString("0.##") + ",'" + rdbIVA.SelectedValue + "','" + ddlRegIVA.SelectedValue + "','" + rdbRotacion.SelectedValue + "','" + rdbActivo.SelectedValue + "','" + ddlSistema.SelectedValue + "','" + ddlPrefijo.SelectedValue + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + nit + "','" + ddlChequeo.SelectedValue + "',NULL)");
			else
				sqlB.Add("update dbxschema.MAGENCIA set MAGE_NOMBRE='"+nombre+"',PCIU_CODIGO='"+ddlCiudad.SelectedValue+"',MAGE_DIRECCION='"+txtDireccion.Text.Trim()+"',MAGE_TELEFONO='"+txtTelefono.Text.Trim()+"',MNIT_ENCARGADO='"+txtEncargado.Text+"',MAGE_PORC_COMISION="+porcComision.ToString("0.##")+",MAGE_IVA='"+rdbIVA.SelectedValue+"',TREG_IVA='"+ddlRegIVA.SelectedValue+"',MAGE_ROTACION='"+rdbRotacion.SelectedValue+"',MAGE_ESTADO='"+rdbActivo.SelectedValue+"',PREFIJO='"+ddlPrefijo.SelectedValue+"',MAGE_SISTEMA='"+ddlSistema.SelectedValue+"',MAGE_FECHA='"+DateTime.Now.ToString("yyyy-MM-dd")+"',MNIT_EMPRESA='"+nit+"', CHEQUEO='"+ddlChequeo.SelectedValue+"' WHERE MAG_CODIGO="+codigo+"");
            if (DBFunctions.Transaction(sqlB))
                Response.Redirect(indexPage + "?process=Comercial.ModificarAgencia&path=" + Request.QueryString["path"] + "&act=1");
            else
                lblError.Text = DBFunctions.exceptions;
		}
	}
}
