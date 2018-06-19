namespace AMS.Contabilidad
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
	using AMS.Tools;


	/// <summary>
	///		Descripción breve de AMS_Contabilidad_PatronesHechosEcono.
	///		en este formulario se muestran las tablas de donde se sacan los datos para construir
	///		un hecho economico,despues de ubicar la tabla se da la opcion de vizualizar cada elemento de la tabla
	///		para ser seleccionado y crear las referencias de campos

	/// </summary>
	public class AMS_Contabilidad_PatronesHechosEcono : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox Aplicación;
		protected System.Web.UI.WebControls.DropDownList CodigoDocumento;
		protected System.Web.UI.WebControls.DropDownList NumeroDocumento;
		protected System.Web.UI.WebControls.DropDownList CodigoDocRefe;
		protected System.Web.UI.WebControls.DropDownList NumeroDocRef;
		protected System.Web.UI.WebControls.DropDownList TablaNit;
		protected System.Web.UI.WebControls.Label AplicacionLabel;
		protected System.Web.UI.WebControls.Label CodigoDocRLabel;
		protected System.Web.UI.WebControls.Label NitLabel;
		protected System.Web.UI.WebControls.Label CodigoDocLabel;
		protected System.Web.UI.WebControls.Label NumeroDocLabel;
		protected System.Web.UI.WebControls.Label NumeroDRefLabel;
		protected System.Web.UI.WebControls.Label TablaNitLabel;
		protected System.Web.UI.WebControls.Label ParamLabel;
		protected System.Web.UI.WebControls.Label DetallesLabel;
		protected System.Web.UI.WebControls.DropDownList Nit;
		protected System.Web.UI.WebControls.Button GRABAR;
		protected System.Web.UI.WebControls.Label CampoSolTabRefNitLabel;
		protected System.Web.UI.WebControls.DropDownList CampoSolTabRefNit;
		protected System.Web.UI.WebControls.Label TablaRefSedeLabel;
		protected System.Web.UI.WebControls.DropDownList TablaRefSede;
		protected System.Web.UI.WebControls.Label CampoLLaveTabRefSedeLabel;
		protected System.Web.UI.WebControls.DropDownList CampoLLaveTabRefSede;
		protected System.Web.UI.WebControls.Label CampoSolTabRefSedeLabel;
		protected System.Web.UI.WebControls.DropDownList CampoSolTabRefSede;
		protected System.Web.UI.WebControls.Label TablaRefCentroCostoLabel;
		protected System.Web.UI.WebControls.DropDownList TablaRefCentroCosto;
		protected System.Web.UI.WebControls.Label CampoLLaveTabRefCentroCostolabel;
		protected System.Web.UI.WebControls.DropDownList CampoLLaveTabRefCentroCosto;
		protected System.Web.UI.WebControls.Label CampoSolTabRefCentroCostoLabel;
		protected System.Web.UI.WebControls.DropDownList CampoSolTabRefCentroCosto;
		protected System.Web.UI.WebControls.Label TablaRefRazonLabel;
		protected System.Web.UI.WebControls.DropDownList TablaRefRazon;
		protected System.Web.UI.WebControls.Label CampoLLaveTabRefRazonLabel;
		protected System.Web.UI.WebControls.DropDownList CampoLLaveTabRefRazon;
		protected System.Web.UI.WebControls.Label CampoSolTbaRefRazonLabel;
		protected System.Web.UI.WebControls.DropDownList CampoSolTbaRefRazon;
		protected System.Web.UI.WebControls.Label naturalezaLabel;
		protected System.Web.UI.WebControls.DropDownList Naturaleza;
		protected System.Web.UI.WebControls.Label CampoLLaveTbaRefValorImputacionLabel;
		protected System.Web.UI.WebControls.DropDownList CampoLLaveTbaRefValorImputacion;
		protected System.Web.UI.WebControls.Label TablaRefValorImputacionLabel;
		protected System.Web.UI.WebControls.DropDownList TablaRefValorImputacion;
		protected System.Web.UI.WebControls.Label CampoSolTabRefValorImputacionLabel;
		protected System.Web.UI.WebControls.DropDownList CampoSolTabRefValorImputacion;
		protected System.Web.UI.WebControls.Label SumatoriaLabel;
		protected System.Web.UI.WebControls.DropDownList Sumatoria;
		protected System.Web.UI.WebControls.Label FormulaLabel;
		protected System.Web.UI.WebControls.TextBox Formula;
		protected System.Web.UI.WebControls.Label ConsecutivoLabel;
		protected System.Web.UI.WebControls.TextBox Consecutivo;
		protected System.Web.UI.WebControls.Label DocumentoLabel;
		protected System.Web.UI.WebControls.DropDownList Documento;
		protected System.Web.UI.WebControls.Label CampoLLaveNitLabel;
		protected System.Web.UI.WebControls.DropDownList CampoLLaveNit;
		protected System.Web.UI.WebControls.DropDownList TablaDetalle;
		protected System.Web.UI.WebControls.Label TablaDetalleLabel;
		protected System.Web.UI.WebControls.Label CuentaLabel;
		protected System.Web.UI.WebControls.DropDownList Cuenta;
		protected System.Web.UI.HtmlControls.HtmlTable Detalles;
		//JFSC 11022008 Poner en comentario
		//string documento = null;		
		//string cuenta = null;
		double Aplic = 0;
		string codigodoc = null;
		string numerodoc = null;
		string codigodocref = null;
		string numerodocref = null;
		//JFSC 11022008 Poner en comentario
		//string nit = null;
		string tablanit = null;
		//JFSC 11022008 Poner en comentario
		//string campollavenit = null;
		//string camposoltabrefnit = null;
		string campollavetabrefsede = null;
		string camposoltabrefsede = null;
		string campollavetabrefcentrocosto = null;
		string camposoltabrefcentrocosto = null;
		string campollavetabrefrazon = null;
		string camposoltbarefrazon = null;
		string naturaleza = null;
		string campollavetbarefvalorimputacion = null;
		string camposoltabrefvalorimputacion = null;
		//JFSC 11022008 Poner en comentario
		//string sumatoria = null;
		string formula = null;
		protected System.Web.UI.WebControls.Label Label1;
		int consecutivo=0;
	

		private void Page_Load(object sender, System.EventArgs e)
		{
		
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(Documento,"Select PDOC_CODIGO,PDOC_CODIGO concat '--' concat PDOC_NOMBRE from DBXSCHEMA.PDOCUMENTO");
				bind.PutDatasIntoDropDownList(Cuenta,"SELECT MCUE_CODIPUC,MCUE_CODIPUC concat '--' concat MCUE_NOMBRE as cuenta  from DBXSCHEMA.MCUENTA ORDER BY MCUE_CODIPUC");
				consecutivo=Convert.ToInt32(DBFunctions.SingleData("Select max(PPUC_SECUENCIA) from DBXSCHEMA.PPUCHECHOSDETALLE"));
				consecutivo=consecutivo+1;
				bind.PutDatasIntoDropDownList(TablaDetalle,"Select STAB_NOMBRE  from DBXSCHEMA.STABLA");
                bind.PutDatasIntoDropDownList(Nit, "Select MNIT_NIT,MNIT_NIT concat '--' concat MNIT_APELLIDOS || ' ' || coalesce(MNIT_APELLIDO2,'') concat ' ' concat MNIT_NOMBRES || ' ' || coalesce(MNIT_NOMBRE2,'') as Nit from DBXSCHEMA.MNIT ORDER BY MNIT_NIT");
				bind.PutDatasIntoDropDownList(TablaNit,"SELECT DISTINCT TBNAME FROM sysibm.sysrels where TBNAME ='MNIT'");
				tablanit=TablaNit.SelectedValue.ToString();//se muestran los campos de la tabla nit
				bind.PutDatasIntoDropDownList(CampoLLaveNit,"SELECT name FROM sysibm.syscolumns WHERE tbname= 'MNIT'");
				bind.PutDatasIntoDropDownList(CampoSolTabRefNit,"SELECT name FROM sysibm.syscolumns WHERE tbname= 'MNIT'");			
				bind.PutDatasIntoDropDownList(Naturaleza,"SELECT TNAT_CODIGO,TNAT_NOMBRE from DBXSCHEMA.TNATURALEZACONTABILIDAD");
				bind.PutDatasIntoDropDownList(TablaRefSede,"SELECT DISTINCT TBNAME FROM sysibm.sysrels");		
				bind.PutDatasIntoDropDownList(TablaRefCentroCosto,"SELECT DISTINCT TBNAME FROM sysibm.sysrels");			
				bind.PutDatasIntoDropDownList(TablaRefRazon,"SELECT DISTINCT TBNAME FROM sysibm.sysrels");		
				bind.PutDatasIntoDropDownList(TablaRefValorImputacion,"SELECT DISTINCT TBNAME FROM sysibm.sysrels");
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
			
		}
		private void InitializeComponent()
		{
			this.Consecutivo.TextChanged += new System.EventHandler(this.Consecutivo_TextChanged);
			this.Aplicación.TextChanged += new System.EventHandler(this.Aplicación_TextChanged);
			this.TablaDetalle.SelectedIndexChanged += new System.EventHandler(this.TablaDetalle_SelectedIndexChanged);
			this.TablaRefSede.SelectedIndexChanged += new System.EventHandler(this.TablaRefSede_SelectedIndexChanged);
			this.TablaRefCentroCosto.SelectedIndexChanged += new System.EventHandler(this.TablaRefCentroCosto_SelectedIndexChanged);
			this.TablaRefRazon.SelectedIndexChanged += new System.EventHandler(this.TablaRefRazon_SelectedIndexChanged);
			this.Naturaleza.SelectedIndexChanged += new System.EventHandler(this.Naturaleza_SelectedIndexChanged);
			this.TablaRefValorImputacion.SelectedIndexChanged += new System.EventHandler(this.TablaRefValorImputacion_SelectedIndexChanged);
			this.Sumatoria.SelectedIndexChanged += new System.EventHandler(this.Sumatoria_SelectedIndexChanged);
			this.Formula.TextChanged += new System.EventHandler(this.Formula_TextChanged);
			this.GRABAR.Click += new System.EventHandler(this.Grabar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		
		
		private void TablaDetalle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(CodigoDocumento,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaDetalle.SelectedValue.ToString()+"'");
			codigodoc=CodigoDocumento.SelectedValue.ToString();//carga el campo del codigo del doc
			bind.PutDatasIntoDropDownList(NumeroDocumento,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaDetalle.SelectedValue.ToString()+"'");
			numerodoc=NumeroDocumento.SelectedValue.ToString();//carga el campo del numero del doc
			bind.PutDatasIntoDropDownList(CodigoDocRefe,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaDetalle.SelectedValue.ToString()+"'");
			codigodocref=CodigoDocRefe.SelectedValue.ToString();//carga el campo del codigo del documento de referencia
			bind.PutDatasIntoDropDownList(NumeroDocRef,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaDetalle.SelectedValue.ToString()+"'");
			numerodocref=NumeroDocRef.SelectedValue.ToString();//carga el numero del documento de referencia
		
		}

		private void TablaRefSede_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(CampoLLaveTabRefSede,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefSede.SelectedValue.ToString()+"'");
			campollavetabrefsede=CampoLLaveTabRefSede.SelectedValue.ToString();//se busca el campo llave d ela tabla sede	
			bind.PutDatasIntoDropDownList(CampoSolTabRefSede,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefSede.SelectedValue.ToString()+"'");
			camposoltabrefsede=CampoSolTabRefSede.SelectedValue.ToString();//se busca elcampo solicitado de la tabla de sede
		}

		private void TablaRefCentroCosto_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(CampoLLaveTabRefCentroCosto,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefCentroCosto.SelectedValue.ToString()+"'");
			campollavetabrefcentrocosto=CampoLLaveTabRefCentroCosto.SelectedValue.ToString();
			bind.PutDatasIntoDropDownList(CampoSolTabRefCentroCosto,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefCentroCosto.SelectedValue.ToString()+"'");
			camposoltabrefcentrocosto=CampoSolTabRefCentroCosto.SelectedValue.ToString();

		}

		private void TablaRefRazon_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(CampoLLaveTabRefRazon,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefRazon.SelectedValue.ToString()+"'");
			campollavetabrefrazon=CampoLLaveTabRefRazon.SelectedValue.ToString();
			bind.PutDatasIntoDropDownList(CampoSolTbaRefRazon,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefRazon.SelectedValue.ToString()+"'");
			camposoltbarefrazon=CampoSolTbaRefRazon.SelectedValue.ToString();
		}

		private void TablaRefValorImputacion_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(CampoLLaveTbaRefValorImputacion,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefValorImputacion.SelectedValue.ToString()+"'");
			campollavetbarefvalorimputacion=CampoLLaveTbaRefValorImputacion.SelectedValue.ToString();
			bind.PutDatasIntoDropDownList(CampoSolTabRefValorImputacion,"SELECT name FROM sysibm.syscolumns WHERE tbname= '"+TablaRefValorImputacion.SelectedValue.ToString()+"'");
			camposoltabrefvalorimputacion=CampoSolTabRefValorImputacion.SelectedValue.ToString();
		
		}

		private void Sumatoria_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(Sumatoria.SelectedValue.Equals("SI"))
				naturaleza="S";
			if(Sumatoria.SelectedValue.Equals("NO"))
				naturaleza="N";
			
		}

		private void Naturaleza_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			if(Naturaleza.SelectedValue.Equals("CREDITO"))
				naturaleza="C";
			else if(Naturaleza.SelectedValue.Equals("DEBITO"))
				    naturaleza="D";
		}

		private void Formula_TextChanged(object sender, System.EventArgs e)
		{
		
			formula=Formula.Text;// aqui se guarda la formula final
		}

		private void Consecutivo_TextChanged(object sender, System.EventArgs e)
		{
			consecutivo=Convert.ToInt32(DBFunctions.SingleData("Select max(PPUC_SECUENCIA) from DBXSCHEMA.PPUCHECHOSDETALLE"));
			consecutivo=consecutivo+1;
	
		}
		private void Aplicación_TextChanged(object sender, System.EventArgs e)
		{
		Aplic=Convert.ToDouble(Aplicación.Text);
		}


		public void Grabar_Click(object sender, System.EventArgs e)
		{
			if(DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.PPUCHECHOSDETALLE(PDOC_CODIGO,PPUC_SECUENCIA,MCUE_CODIPUC,PPUC_PORCAPLI,PPUC_TABLADETALLE,PPUC_CAMPOLLAVE,PPUC_NUMELLAVE,PPUC_CODIREFE,PPUC_NUMEREFE,PPUC_NIT,PPUC_NIT_TABLAREF,PPUC_NIT_CAMPOREF,PPUC_NIT_CAMPOVALOR,PPUC_ALMACEN,PPUC_ALMACEN_TABLAREF,PPUC_ALMACEN_CAMPOREF,PPUC_ALMACEN_CAMPOVALOR,PPUC_CENTROCOSTO,PPUC_CENTROCOSTO_TABLAREF,PPUC_CENTROCOSTO_CAMPOREF,PPUC_CENTROCOSTO_CAMPOVALOR,PPUC_DETALLE,PPUC_DETALLE_TABLAREF,PPUC_DETALLE_CAMPOREF,PPUC_DETALLE_CAMPOVALOR,PPUC_NATURALEZA,PPUC_VALOR,PPUC_VALOR_TABLAREF,PPUC_VALOR_CAMPOREF,PPUC_VALOR_CAMPOVALOR,TRES_SINO,PPUC_FORMULA)VALUES ('"+Documento.SelectedValue+"',"+DBFunctions.SingleData("Select max(PPUC_SECUENCIA)+1 from DBXSCHEMA.PPUCHECHOSDETALLE")+",'"+Cuenta.SelectedValue+"',"+Aplicación.Text+",'"+TablaDetalle.SelectedValue+"','"+CodigoDocumento.SelectedValue+"','"+NumeroDocumento.SelectedValue+"','"+CodigoDocRefe.SelectedValue+"','"+NumeroDocRef.SelectedValue+"','"+Nit.SelectedValue+"','"+TablaNit.SelectedValue+"','"+CampoLLaveNit.SelectedValue+"','"+CampoSolTabRefNit.SelectedValue+"','null','"+TablaRefSede.SelectedValue.ToString()+"','"+CampoLLaveTabRefSede.SelectedValue+"','"+CampoSolTabRefSede.SelectedValue+"','null','"+TablaRefCentroCosto.SelectedValue.ToString()+"','"+CampoLLaveTabRefCentroCosto.SelectedValue+"','"+CampoSolTabRefCentroCosto.SelectedValue+"','null','"+TablaRefRazon.SelectedValue.ToString()+"','"+CampoLLaveTabRefRazon.SelectedValue+"','"+CampoSolTbaRefRazon.SelectedValue+"','"+Naturaleza.SelectedValue+"','null','"+TablaRefValorImputacion.SelectedValue.ToString()+"','"+CampoLLaveTbaRefValorImputacion.SelectedValue+"','"+CampoSolTabRefValorImputacion.SelectedValue+"','"+Sumatoria.SelectedValue+"','"+Formula.Text+"')")==1)
			{
				Label1.Text="Bien ingresando : "+"INSERT INTO DBXSCHEMA.PPUCHECHOSDETALLE(PDOC_CODIGO,PPUC_SECUENCIA,MCUE_CODIPUC,PPUC_PORCAPLI,PPUC_TABLADETALLE,PPUC_CAMPOLLAVE,PPUC_NUMELLAVE,PPUC_CODIREFE,PPUC_NUMEREFE,PPUC_NIT,PPUC_NIT_TABLAREF,PPUC_NIT_CAMPOREF,PPUC_NIT_CAMPOVALOR,PPUC_ALMACEN,PPUC_ALMACEN_TABLAREF,PPUC_ALMACEN_CAMPOREF,PPUC_ALMACEN_CAMPOVALOR,PPUC_CENTROCOSTO,PPUC_CENTROCOSTO_TABLAREF,PPUC_CENTROCOSTO_CAMPOREF,PPUC_CENTROCOSTO_CAMPOVALOR,PPUC_DETALLE,PPUC_DETALLE_TABLAREF,PPUC_DETALLE_CAMPOREF,PPUC_DETALLE_CAMPOVALOR,PPUC_NATURALEZA,PPUC_VALOR,PPUC_VALOR_TABLAREF,PPUC_VALOR_CAMPOREF,PPUC_VALOR_CAMPOVALOR,TRES_SINO,PPUC_FORMULA)VALUES ('"+Documento.SelectedValue+"',"+DBFunctions.SingleData("Select max(PPUC_SECUENCIA)+1 from DBXSCHEMA.PPUCHECHOSDETALLE")+",'"+Cuenta.SelectedValue+"',"+Aplicación.Text+",'"+TablaDetalle.SelectedValue+"','"+CodigoDocumento.SelectedValue+"','"+NumeroDocumento.SelectedValue+"','"+CodigoDocRefe.SelectedValue+"','"+NumeroDocRef.SelectedValue+"','"+Nit.SelectedValue+"','"+TablaNit.SelectedValue+"','"+CampoLLaveNit.SelectedValue+"','"+CampoSolTabRefNit.SelectedValue+"','null','"+TablaRefSede.SelectedValue.ToString()+"','"+CampoLLaveTabRefSede.SelectedValue+"','"+CampoSolTabRefSede.SelectedValue+"','null','"+TablaRefCentroCosto.SelectedValue.ToString()+"','"+CampoLLaveTabRefCentroCosto.SelectedValue+"','"+CampoSolTabRefCentroCosto.SelectedValue+"','null','"+TablaRefRazon.SelectedValue.ToString()+"','"+CampoLLaveTabRefRazon.SelectedValue+"','"+CampoSolTbaRefRazon.SelectedValue+"','"+Naturaleza.SelectedValue+"','null','"+TablaRefValorImputacion.SelectedValue.ToString()+"','"+CampoLLaveTbaRefValorImputacion.SelectedValue+"','"+CampoSolTabRefValorImputacion.SelectedValue+"','"+Sumatoria.SelectedValue+"','"+Formula.Text+"')";
			}
			else
				Label1.Text="Mal Ingresando : "+"INSERT INTO DBXSCHEMA.PPUCHECHOSDETALLE(PDOC_CODIGO,PPUC_SECUENCIA,MCUE_CODIPUC,PPUC_PORCAPLI,PPUC_TABLADETALLE,PPUC_CAMPOLLAVE,PPUC_NUMELLAVE,PPUC_CODIREFE,PPUC_NUMEREFE,PPUC_NIT,PPUC_NIT_TABLAREF,PPUC_NIT_CAMPOREF,PPUC_NIT_CAMPOVALOR,PPUC_ALMACEN,PPUC_ALMACEN_TABLAREF,PPUC_ALMACEN_CAMPOREF,PPUC_ALMACEN_CAMPOVALOR,PPUC_CENTROCOSTO,PPUC_CENTROCOSTO_TABLAREF,PPUC_CENTROCOSTO_CAMPOREF,PPUC_CENTROCOSTO_CAMPOVALOR,PPUC_DETALLE,PPUC_DETALLE_TABLAREF,PPUC_DETALLE_CAMPOREF,PPUC_DETALLE_CAMPOVALOR,PPUC_NATURALEZA,PPUC_VALOR,PPUC_VALOR_TABLAREF,PPUC_VALOR_CAMPOREF,PPUC_VALOR_CAMPOVALOR,TRES_SINO,PPUC_FORMULA)VALUES ('"+Documento.SelectedValue+"',"+DBFunctions.SingleData("Select max(PPUC_SECUENCIA)+1 from DBXSCHEMA.PPUCHECHOSDETALLE")+",'"+Cuenta.SelectedValue+"',"+Aplicación.Text+",'"+TablaDetalle.SelectedValue+"','"+CodigoDocumento.SelectedValue+"','"+NumeroDocumento.SelectedValue+"','"+CodigoDocRefe.SelectedValue+"','"+NumeroDocRef.SelectedValue+"','"+Nit.SelectedValue+"','"+TablaNit.SelectedValue+"','"+CampoLLaveNit.SelectedValue+"','"+CampoSolTabRefNit.SelectedValue+"','null','"+TablaRefSede.SelectedValue.ToString()+"','"+CampoLLaveTabRefSede.SelectedValue+"','"+CampoSolTabRefSede.SelectedValue+"','null','"+TablaRefCentroCosto.SelectedValue.ToString()+"','"+CampoLLaveTabRefCentroCosto.SelectedValue+"','"+CampoSolTabRefCentroCosto.SelectedValue+"','null','"+TablaRefRazon.SelectedValue.ToString()+"','"+CampoLLaveTabRefRazon.SelectedValue+"','"+CampoSolTbaRefRazon.SelectedValue+"','"+Naturaleza.SelectedValue+"','null','"+TablaRefValorImputacion.SelectedValue.ToString()+"','"+CampoLLaveTbaRefValorImputacion.SelectedValue+"','"+CampoSolTabRefValorImputacion.SelectedValue+"','"+Sumatoria.SelectedValue+"','"+Formula.Text+"')<br> Detalles : "+DBFunctions.exceptions;
		}
	}
}
