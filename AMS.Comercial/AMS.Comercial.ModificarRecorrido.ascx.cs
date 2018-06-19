namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
	using System.Configuration;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ModificarRecorrido.
	/// </summary>
	public class AMS_Comercial_ModificarRecorrido : System.Web.UI.UserControl
	{
		#region Variables, controles

		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.ListBox lstSubRutas;
		protected System.Web.UI.WebControls.Button btnSubir;
		protected System.Web.UI.WebControls.Button btnBajar;
		protected System.Web.UI.WebControls.Button btnAgregar;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Button btnQuitar;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label lblDesc;
		protected System.Web.UI.WebControls.Label lblOrigen;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Panel pnlRuta;
		protected System.Web.UI.WebControls.Panel pnlSubrutas;
		protected System.Web.UI.WebControls.DropDownList ddlCiudad;
		protected System.Web.UI.WebControls.Label lblDestinoR;
		protected System.Web.UI.WebControls.Label lblDestino;
		protected System.Web.UI.WebControls.Label lblOrigenR;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.CheckBox chkInversa,chkRutas;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlRuta,"select mrut_codigo,mrut_codigo from DBXSCHEMA.mrutas WHERE mrut_clase=2 order by mrut_codigo;");
				bind.PutDatasIntoDropDownList(ddlCiudad,"select pciu_codigo,pciu_nombre concat' (' concat pciu_codigoonal concat ')' from DBXSCHEMA.pciudad order by pciu_orden,pzon_zona,pciu_nombre");
				ListItem itm=new ListItem("---Ruta---","");
				ddlRuta.Items.Insert(0,itm);
				btnGuardar.Attributes.Add("onclick","return(confirm('Se reasignarán las rutas asociadas utilizando las ciudades seleccionadas como referencia. ¿Desea continuar?'));");
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El recorrido ha sido modificado, por favor verifique que las rutas asociadas al recorrido son las correctas.");
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
			this.ddlRuta.SelectedIndexChanged += new System.EventHandler(this.ddlRuta_SelectedIndexChanged);
			this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
			this.btnSubir.Click += new System.EventHandler(this.btnSubir_Click);
			this.btnBajar.Click += new System.EventHandler(this.btnBajar_Click);
			this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Traer Subrutas de la ruta seleccionada
		private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string mruta=ddlRuta.SelectedValue.Trim();
			string origen="",destino="";
			DataSet dsRutas=new DataSet();
			DatasToControls bind=  new DatasToControls();
			pnlRuta.Visible=pnlSubrutas.Visible=false;
			lblDesc.Text=lblDestino.Text=lblOrigen.Text="";
			
			//Consultar info de ruta principal
			DBFunctions.Request(dsRutas,IncludeSchema.NO,
				"select rt.MRUT_DESCRIPCION, "+
				"co.PCIU_NOMBRE concat' (' concat co.pciu_codigoonal concat ')', "+
				"cd.PCIU_NOMBRE  concat' (' concat cd.pciu_codigoonal concat ')', "+
				"co.pciu_codigo,cd.pciu_codigo "+
				"from DBXSCHEMA.mrutas rt, DBXSCHEMA.pciudad cd, DBXSCHEMA.PCIUDAD co "+
				"WHERE co.PCIU_CODIGO=rt.PCIU_COD AND cd.PCIU_CODIGO=rt.PCIU_CODDES AND rt.MRUT_CODIGO='"+mruta+"';");
			if(dsRutas.Tables[0].Rows.Count>0){
				lblDesc.Text=dsRutas.Tables[0].Rows[0][0].ToString();
				lblOrigenR.Text=lblOrigen.Text=dsRutas.Tables[0].Rows[0][1].ToString();
				lblDestinoR.Text=lblDestino.Text=dsRutas.Tables[0].Rows[0][2].ToString();
				origen=dsRutas.Tables[0].Rows[0][3].ToString();
				destino=dsRutas.Tables[0].Rows[0][4].ToString();
				ViewState["ORIGEN"]=origen;
				ViewState["DESTINO"]=destino;
				pnlRuta.Visible=pnlSubrutas.Visible=true;
			}

			//Ciudades del recorrido
			dsRutas=new DataSet();
			DBFunctions.Request(dsRutas,IncludeSchema.NO,
				"select mrc.pciu_codigo as valor, pc.PCIU_NOMBRE concat' (' concat pc.pciu_codigoonal concat ')' as texto, mrc.secuencia "+
				"from dbxschema.mruta_ciudad mrc, dbxschema.pciudad pc "+
				"where mrc.pciu_codigo=pc.pciu_codigo and mrc.mrut_codigo='"+mruta+"' and mrc.pciu_codigo<>'"+origen+"' and mrc.pciu_codigo<>'"+destino+"' "+
				"order by mrc.secuencia;");
			ViewState["SubRutas"]=dsRutas;
			lstSubRutas.DataSource=dsRutas;
			lstSubRutas.DataTextField="texto";
			lstSubRutas.DataValueField="valor";
			lstSubRutas.DataBind();

		}

		//Guardar recorrido
		private void btnAgregar_Click(object sender, System.EventArgs e)
		{
			string mruta=ddlRuta.SelectedValue.Trim();
			string ciudadN=ddlCiudad.SelectedValue;
			string origen=ViewState["ORIGEN"].ToString();
			string destino=ViewState["DESTINO"].ToString();
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			if(mruta.Length==0)
			{
                Utils.MostrarAlerta(Response, "  Debe seleccionar una ruta principal.");
				return;
			}
			if(ciudadN.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar una ciudad.");
				return;
			}
			if(ciudadN.Equals(origen)){
                Utils.MostrarAlerta(Response, "  La ciudad de origen no se puede agregar como parte del recorrido.");
				return;
			}
			if(ciudadN.Equals(destino)){
                Utils.MostrarAlerta(Response, "  La ciudad de destino no se puede agregar como parte del recorrido.");
				return;
			}
			if(lstSubRutas.Items.FindByValue(ciudadN)==null){
				DataRow dr=dsSubRutas.Tables[0].NewRow();
				dr[0]=ciudadN;
				dr[1]=ddlCiudad.SelectedItem.Text;
				dsSubRutas.Tables[0].Rows.Add(dr);
			}
			else
			{
                Utils.MostrarAlerta(Response, "  Ya agrego la ciudad.");
				return;
			}
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
		}

		//Subir subruta de orden
		private void btnSubir_Click(object sender, System.EventArgs e){
			int i=lstSubRutas.SelectedIndex;
			string codA,nomA;
			if(i<=0 || lstSubRutas.Items.Count==0)
				return;
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			codA=dsSubRutas.Tables[0].Rows[i][0].ToString();
			nomA=dsSubRutas.Tables[0].Rows[i][1].ToString();
			dsSubRutas.Tables[0].Rows[i][0]=dsSubRutas.Tables[0].Rows[i-1][0];
			dsSubRutas.Tables[0].Rows[i][1]=dsSubRutas.Tables[0].Rows[i-1][1];
			dsSubRutas.Tables[0].Rows[i-1][0]=codA;
			dsSubRutas.Tables[0].Rows[i-1][1]=nomA;
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
			lstSubRutas.SelectedIndex=i-1;
		}

		//Bajar subruta de orden
		private void btnBajar_Click(object sender, System.EventArgs e){
			int i=lstSubRutas.SelectedIndex;
			string codA,nomA;
			if(i<0 || i==lstSubRutas.Items.Count-1 || lstSubRutas.Items.Count==0)
				return;
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			codA=dsSubRutas.Tables[0].Rows[i][0].ToString();
			nomA=dsSubRutas.Tables[0].Rows[i][1].ToString();
			dsSubRutas.Tables[0].Rows[i][0]=dsSubRutas.Tables[0].Rows[i+1][0];
			dsSubRutas.Tables[0].Rows[i][1]=dsSubRutas.Tables[0].Rows[i+1][1];
			dsSubRutas.Tables[0].Rows[i+1][0]=codA;
			dsSubRutas.Tables[0].Rows[i+1][1]=nomA;
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
			lstSubRutas.SelectedIndex=i+1;
		}

		//Guardar
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			//Guardar ciudades
			int n=0;
			string rutaInversa="";
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			string rutaP=ddlRuta.SelectedValue;
			if(rutaP.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar la ruta principal.");
				return;}
			
			//Inversa?
			if(chkInversa.Checked){
				//Existe la ruta inversa?
				rutaInversa=DBFunctions.SingleData(
					"SELECT MRI.MRUT_CODIGO "+
					"FROM DBXSCHEMA.MRUTAS MRI, DBXSCHEMA.MRUTAS MRP "+
					"WHERE MRP.MRUT_CODIGO='"+rutaP+"' AND MRP.PCIU_COD=MRI.PCIU_CODDES AND MRI.PCIU_COD=MRP.PCIU_CODDES;");
				if(rutaInversa.Length==0){
                    Utils.MostrarAlerta(Response, "  No existe la ruta inversa a la ruta principal seleccionada.");
					return;}
			}

			ArrayList sqlUpd=new ArrayList();
			
			//Borrar actuales
			sqlUpd.Add("DELETE FROM DBXSCHEMA.MRUTA_CIUDAD WHERE MRUT_CODIGO='"+rutaP+"';");
			if(chkInversa.Checked){
				sqlUpd.Add("UPDATE DBXSCHEMA.MRUTAS SET MRUT_CLASE=2 WHERE MRUT_CODIGO='"+rutaInversa+"';");
				sqlUpd.Add("DELETE FROM DBXSCHEMA.MRUTA_CIUDAD WHERE MRUT_CODIGO='"+rutaInversa+"';");
			}

			//Insertar nuevos
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_CIUDAD VALUES ('"+rutaP+"','"+ViewState["ORIGEN"].ToString()+"',1);");
			for(n=0;n<lstSubRutas.Items.Count;n++)
				sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_CIUDAD VALUES ('"+rutaP+"','"+lstSubRutas.Items[n].Value+"',"+(n+2)+");");
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_CIUDAD VALUES ('"+rutaP+"','"+ViewState["DESTINO"].ToString()+"',"+(n+2)+");");
			
			//Inversas
			if(chkInversa.Checked){
				sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_CIUDAD VALUES ('"+rutaInversa+"','"+ViewState["DESTINO"].ToString()+"',1);");
				for(n=0;n<lstSubRutas.Items.Count;n++)
					sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_CIUDAD VALUES ('"+rutaInversa+"','"+lstSubRutas.Items[(lstSubRutas.Items.Count-1)-n].Value+"',"+(n+2)+");");
				sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_CIUDAD VALUES ('"+rutaInversa+"','"+ViewState["ORIGEN"].ToString()+"',"+(n+2)+");");
			}

			//Modificar tabla de recorrido (temporal)
			//Borrar actuales
			if(chkRutas.Checked)
			{
			sqlUpd.Add("DELETE FROM DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL='"+rutaP+"';");
			if(chkInversa.Checked)
				sqlUpd.Add("DELETE FROM DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL='"+rutaInversa+"';");
			}
			
			//Insertar nuevos
			/*sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_INTERMEDIA "+
				"SELECT '"+rutaP+"',MR.MRUT_CODIGO,row_number() over () "+
				"FROM DBXSCHEMA.PCIUDAD PCO, DBXSCHEMA.PCIUDAD PCD, DBXSCHEMA.MRUTAS MR, DBXSCHEMA.MRUTA_CIUDAD MRCO, DBXSCHEMA.MRUTA_CIUDAD MRCD "+
				"WHERE "+
				"PCO.PCIU_CODIGO IN (SELECT MCA1.PCIU_CODIGO FROM DBXSCHEMA.MRUTA_CIUDAD MCA1 WHERE MCA1.MRUT_CODIGO='"+rutaP+"') "+
				"AND "+
				"PCD.PCIU_CODIGO IN (SELECT MCA2.PCIU_CODIGO FROM DBXSCHEMA.MRUTA_CIUDAD MCA2 WHERE MCA2.MRUT_CODIGO='"+rutaP+"') "+
				"AND "+
				"MRCO.PCIU_CODIGO=PCO.PCIU_CODIGO "+
				"AND "+
				"MRCD.PCIU_CODIGO=PCD.PCIU_CODIGO "+
				"AND "+
				"MR.PCIU_COD=PCO.PCIU_CODIGO "+
				"AND "+
				"MR.PCIU_CODDES=PCD.PCIU_CODIGO "+
				"AND "+
				"MRCO.SECUENCIA<MRCD.SECUENCIA "+
				"order by MRCO.SECUENCIA,MRCD.SECUENCIA;");*/
			if(chkRutas.Checked)
			{
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_INTERMEDIA "+
				"SELECT '"+rutaP+"',MR.MRUT_CODIGO,row_number() over () "+
				"FROM DBXSCHEMA.PCIUDAD PCO, DBXSCHEMA.PCIUDAD PCD, DBXSCHEMA.MRUTAS MR, DBXSCHEMA.MRUTA_CIUDAD MRCO, DBXSCHEMA.MRUTA_CIUDAD MRCD "+
				"WHERE  "+
				"MRCO.PCIU_CODIGO=PCO.PCIU_CODIGO AND "+
				"MRCD.PCIU_CODIGO=PCD.PCIU_CODIGO AND "+
				"MRCO.MRUT_CODIGO='"+rutaP+"' AND MRCD.MRUT_CODIGO='"+rutaP+"' AND "+
				"MR.PCIU_COD=PCO.PCIU_CODIGO AND MR.PCIU_CODDES=PCD.PCIU_CODIGO AND "+
				"MRCO.SECUENCIA<MRCD.SECUENCIA "+
				"order by MRCO.SECUENCIA,MRCD.SECUENCIA;");
			
			//Insertar nuevos Inversa
				if(chkInversa.Checked)
				{
				sqlUpd.Add("INSERT INTO DBXSCHEMA.MRUTA_INTERMEDIA "+
				"SELECT '"+rutaInversa+"',MR.MRUT_CODIGO,row_number() over () "+
				"FROM DBXSCHEMA.PCIUDAD PCO, DBXSCHEMA.PCIUDAD PCD, DBXSCHEMA.MRUTAS MR, DBXSCHEMA.MRUTA_CIUDAD MRCO, DBXSCHEMA.MRUTA_CIUDAD MRCD "+
				"WHERE  "+
				"MRCO.PCIU_CODIGO=PCO.PCIU_CODIGO AND "+
				"MRCD.PCIU_CODIGO=PCD.PCIU_CODIGO AND "+
				"MRCO.MRUT_CODIGO='"+rutaInversa+"' AND MRCD.MRUT_CODIGO='"+rutaInversa+"' AND "+
				"MR.PCIU_COD=PCO.PCIU_CODIGO AND MR.PCIU_CODDES=PCD.PCIU_CODIGO AND "+
				"MRCO.SECUENCIA<MRCD.SECUENCIA "+
				"order by MRCO.SECUENCIA,MRCD.SECUENCIA;");
			}
			}

			if(!DBFunctions.Transaction(sqlUpd))
			{
				lblError.Text=DBFunctions.exceptions;
				return;}
			//Rutas ambiguas asociadas?
			string repsP="",repsI="";
			if(chkRutas.Checked){
			repsP=DBFunctions.SingleData(
				"select reps.cont from ("+
				"Select count(*) as cont,po.pciu_nombre as origen, pd.pciu_nombre as destino from DBXSCHEMA.MRUTA_INTERMEDIA MI,DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PO, DBXSCHEMA.PCIUDAD PD "+
				"where MI.MRUTA_PRINCIPAL='"+rutaP+"' AND MR.MRUT_CODIGO=MI.mruta_secundaria AND PO.PCIU_CODIGO=mr.pciu_cod and PD.PCIU_CODIGO=mr.pciu_coddes "+
				"group by mr.pciu_cod,po.pciu_nombre,mr.pciu_coddes,pd.pciu_nombre) as reps "+
				"where reps.cont>1;");
			if(chkInversa.Checked)
				repsI=DBFunctions.SingleData(
					"select reps.cont from ("+
					"Select count(*) as cont,po.pciu_nombre as origen, pd.pciu_nombre as destino from DBXSCHEMA.MRUTA_INTERMEDIA MI,DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PO, DBXSCHEMA.PCIUDAD PD "+
					"where MI.MRUTA_PRINCIPAL='"+rutaInversa+"' AND MR.MRUT_CODIGO=MI.mruta_secundaria AND PO.PCIU_CODIGO=mr.pciu_cod and PD.PCIU_CODIGO=mr.pciu_coddes "+
					"group by mr.pciu_cod,po.pciu_nombre,mr.pciu_coddes,pd.pciu_nombre) as reps "+
					"where reps.cont>1;");
			}

			Response.Redirect(indexPage+"?process=Comercial.ModificarRecorrido&path="+Request.QueryString["path"]+"&act=1"+(repsP.Length>0?"&repp=1":"")+(repsI.Length>0?"&repi=1":""));
		}

		//Quitar elemento
		private void btnQuitar_Click(object sender, System.EventArgs e){
			int i=lstSubRutas.SelectedIndex;
			if(i<0 || lstSubRutas.Items.Count==0)
				return;
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			dsSubRutas.Tables[0].Rows.Remove(dsSubRutas.Tables[0].Rows[i]);
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
		}

	}
}
