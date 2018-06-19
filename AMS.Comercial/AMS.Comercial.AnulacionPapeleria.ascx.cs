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
	///		Descripción breve de AMS_Comercial_AnulacionPapeleria.
	/// </summary>
	public class AMS_Comercial_AnulacionPapeleria : System.Web.UI.UserControl
	{
		#region Controles
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblNumero;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Panel pnlunoauno;
		protected System.Web.UI.WebControls.Button btnDocumento;
		protected System.Web.UI.WebControls.Button btnRango;
		protected System.Web.UI.WebControls.Button btnAnular;
		protected System.Web.UI.WebControls.Button btnBuscar;
		protected System.Web.UI.WebControls.TextBox txtNumeroInicial;
		protected System.Web.UI.WebControls.TextBox txtNumeroFinal;
		protected System.Web.UI.WebControls.DropDownList ddlConceptoAnular;
		protected System.Web.UI.WebControls.Button btnAnularRango;
		protected System.Web.UI.WebControls.Panel pnlRango;
		protected System.Web.UI.WebControls.Panel pnlBuscar;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlTipos;
		protected System.Web.UI.WebControls.DataGrid dgrAnulacion;
		
		public string strActScript;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				string anulacion=DBFunctions.SingleData("SELECT MANULACION_NUMERO+1 FROM DBXSCHEMA.MANULACION_PAPELERIA ORDER BY MANULACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
				if(anulacion.Length==0)anulacion="1";
				lblNumero.Text=anulacion;
				lblFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				//AGENCIAS
				Agencias.TraerAgenciasPrefijoUsuario(ddlAgencia);
				//TIPOS DE DOCUMENTOS
				DataSet dsTiposDoc=new DataSet();
				DBFunctions.Request(dsTiposDoc,IncludeSchema.NO,
					"select tdoc_codigo concat case when prefijo='S' then '|' else '' end as valor,tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS WHERE PAPELERIA='S' ORDER BY TDOC_NOMBRE;");
				DataRow drC=dsTiposDoc.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsTiposDoc.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtTiposDocumentos",dsTiposDoc.Tables[0]);
				//CONCEPTOS ANULACION
				DataSet dsTiposAnul=new DataSet();
				DBFunctions.Request(dsTiposAnul,IncludeSchema.NO,
					"select rtrim(char(tcon_codigo)) as valor,tcon_descripcion as texto from DBXSCHEMA.TCONCEPTO_ANULACION_PAPELERIA ORDER BY TCON_DESCRIPCION;");
				drC=dsTiposAnul.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsTiposAnul.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtTiposAnulacion",dsTiposAnul.Tables[0]);

				
				if(Request.QueryString["act"]!=null && Request.QueryString["anl"]!=null)
                Utils.MostrarAlerta(Response, "La papelería ha sido anulada con el número de anulación " + Request.QueryString["anl"] + ".");
			}
		}

		//Mostrar grilla
		private void MostrarTabla()
		{
			DataTable dtDocumentos=new DataTable();
			//Tabla tiquetes normales
			dtDocumentos.Columns.Add("NUMERO",typeof(int));
			for(int n=1;n<=15;n++)
			{
				DataRow drT=dtDocumentos.NewRow();
				drT[0]=n;
				dtDocumentos.Rows.Add(drT);
			}
			dgrDocumentos.DataSource=dtDocumentos;
			dgrDocumentos.DataBind();
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
			this.btnDocumento.Click += new System.EventHandler(this.btnDocumento_Click);
			this.btnRango.Click += new System.EventHandler(this.btnRango_Click);
			this.dgrDocumentos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrDocumentos_ItemDataBound);
			this.btnAnular.Click += new System.EventHandler(this.btnAnular_Click);
			this.btnAnularRango.Click += new System.EventHandler(this.btnAnularRango_Click);
			this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		private void btnDocumento_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue.Replace("|","").Trim();
			pnlunoauno.Visible=true;
			pnlRango.Visible=false;
			pnlBuscar.Visible=false;
			MostrarTabla();
		}
		
		private void btnRango_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue.Replace("|","").Trim();
			pnlunoauno.Visible=false;
			pnlRango.Visible=false;
			pnlBuscar.Visible=true;
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado.");
				return;
			}
			string anulacion=DBFunctions.SingleData("SELECT MANULACION_NUMERO+1 FROM DBXSCHEMA.MANULACION_PAPELERIA ORDER BY MANULACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(anulacion.Length==0)anulacion="1";
			
			//TIPOS DE DOCUMENTOS
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlTipos,
				"select tdoc_codigo concat case when prefijo='S' then '|' else '' end as valor,tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS WHERE PAPELERIA='S' ORDER BY TDOC_NOMBRE;");
			ListItem itm1=new ListItem("---Seleccione---","");
			ddlTipos.Items.Insert(0,itm1);			
			//CONCEPTOS ANULACION
			bind.PutDatasIntoDropDownList(ddlConceptoAnular,
				"select rtrim(char(tcon_codigo)) as valor,tcon_descripcion as texto from DBXSCHEMA.TCONCEPTO_ANULACION_PAPELERIA ORDER BY TCON_DESCRIPCION;");
				
			//ListItem itm2=new ListItem("---Seleccione---","");
			ddlConceptoAnular.Items.Insert(0,itm1);
								
		}
		private void btnBuscar_Click(object sender, System.EventArgs e)
		{
			long numeroInicial= 0;
			long numeroFinal = 0;
			int prefijo=0;
			string agencia=ddlAgencia.SelectedValue.Replace("|","").Trim();
			if(ddlTipos.SelectedValue.EndsWith("|") && ddlAgencia.SelectedValue.EndsWith("|"))
				prefijo=Convert.ToInt16(agencia);
			if(ddlAgencia.SelectedValue.Length==0)
			{	
                Utils.MostrarAlerta(Response, "Debe dar la agencia.");
				return;
			}
			if(ddlTipos.SelectedValue.Length==0)
			{	
                Utils.MostrarAlerta(Response, "Debe Escoger Tipo Documento.");
				return;
			}
				
			if(txtNumeroInicial.Text.Length==0)
			{	
                Utils.MostrarAlerta(Response, "Debe Numero Inicial Rango.");
				return;
			}
			
			if(txtNumeroFinal.Text.Length==0)
			{	
                Utils.MostrarAlerta(Response, "Debe Numero Inicial Rango.");
				return;
			}
			//Numero Inicial
			try
			{
				numeroInicial=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtNumeroInicial.Text.Trim());
				if (numeroInicial<1)throw(new Exception());}
			catch
			{
                Utils.MostrarAlerta(Response, "Error Numero Inicial Rango.");
				return;}

			//Numero Final
			try
			{
				numeroFinal=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtNumeroFinal.Text.Trim());
				if(numeroFinal<1)throw(new Exception());}
			catch
			{
                Utils.MostrarAlerta(Response, "Error Numero Final Rango.");
				return;}
			
			if (numeroInicial > numeroFinal)
			{
                Utils.MostrarAlerta(Response, "Numero Inicial > Numero Final");
				return;
			}
			ViewState["numeroInicial"] = numeroInicial;
			ViewState["numeroFinal"]   =  numeroFinal;
			if(ddlConceptoAnular.SelectedValue.Length==0)
			{	
                Utils.MostrarAlerta(Response, "Debe Escoger Concepto Anulacion.");
				return;
			}
			
						
			string tipo=ddlTipos.SelectedValue.Trim().Replace("|","");
			
			string concepto=ddlConceptoAnular.SelectedValue.Trim();
			pnlRango.Visible=true;
			DataSet dsPapeleria=new DataSet();
			DBFunctions.Request(dsPapeleria,IncludeSchema.NO,"select  TIPO_DOCUMENTO,NUM_DOCUMENTO,MAG_CODIGO,NUM_RECEPCION,FECHA_RECEPCION, NUM_DESPACHO,FECHA_DESPACHO,FECHA_ASIGNACION,MNIT_RESPONSABLE " +
															   " from DBXSCHEMA.MCONTROL_PAPELERIA " +
															   "	WHERE TDOC_CODIGO = '"+tipo+"' " +  
															   "	AND MAG_CODIGO  = "+agencia+" " +
															   "	AND NUM_DOCUMENTO >= "+numeroInicial+" " + 
															   "    AND NUM_DOCUMENTO <= "+numeroFinal+" " +   
															   "    AND FECHA_ANULACION IS NULL " +
															   "    AND FECHA_USO IS NULL  ");
			//DBFunctions.Request(dsPapeleria,IncludeSchema.NO,"CALL DBXSCHEMA.ANULACION_PAPELERIA('"+tipo+"',"+numeroInicial+","+numeroFinal+","+agencia+");");
			if(dsPapeleria.Tables[0].Rows.Count<=0) 
			{
				lblError.Text += "No hay Documentos en el rango / " + DBFunctions.exceptions;
				pnlunoauno.Visible=false;
				pnlRango.Visible=false;
				pnlBuscar.Visible=false;
				return;
			}
			lblError.Text= "Anular : Oprima el boton ";
			dgrAnulacion.DataSource=dsPapeleria.Tables[0].DefaultView;
			dgrAnulacion.DataBind();
		}
		private void dgrDocumentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlTipo=(DropDownList)e.Item.FindControl("ddlTipoDocumento");
				DropDownList dlConceptoAnulacion=(DropDownList)e.Item.FindControl("ddlConceptoAnulacion");
				
				dlTipo.DataSource=ViewState["dtTiposDocumentos"];
				dlTipo.DataTextField="texto";
				dlTipo.DataValueField="valor";
				dlTipo.DataBind();
				
				dlConceptoAnulacion.DataSource=ViewState["dtTiposAnulacion"];
				dlConceptoAnulacion.DataTextField="texto";
				dlConceptoAnulacion.DataValueField="valor";
				dlConceptoAnulacion.DataBind();
			}
		}
		private void btnAnularRango_Click(object sender, System.EventArgs e)
		{	
			ArrayList sqlUpd=new ArrayList();
			string agencia=ddlAgencia.SelectedValue.Replace("|","").Trim();
			string concepto=ddlConceptoAnular.SelectedValue.Trim();
			long numeroInicial= Convert.ToInt64(ViewState["numeroInicial"]);
			long numeroFinal = Convert.ToInt64(ViewState["numeroFinal"]);
			string tipo=ddlTipos.SelectedValue.Trim().Replace("|","");
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado.");
				return;
			}
			string anulacion=DBFunctions.SingleData("SELECT MANULACION_NUMERO+1 FROM DBXSCHEMA.MANULACION_PAPELERIA ORDER BY MANULACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(anulacion.Length==0)anulacion="1";
			
			sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET NUM_ANULACION="+anulacion+", FECHA_ANULACION='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', COD_ANULACION="+concepto+", MNIT_ANULACION='"+nitResponsable+"' " +
				" WHERE TDOC_CODIGO='"+tipo+"' 	AND MAG_CODIGO  = "+agencia+"  AND NUM_DOCUMENTO >="+numeroInicial+" and NUM_DOCUMENTO <="+numeroFinal+"  AND FECHA_ANULACION IS NULL  AND FECHA_USO IS NULL ;");
			
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MANULACION_PAPELERIA  "+
                       " select NUM_ANULACION,rownumber() over() linea,MAG_CODIGO,TDOC_CODIGO,'M',NUM_DOCUMENTO,COD_ANULACION, current date, MNIT_ANULACION "+
                       " from DBXSCHEMA.MCONTROL_PAPELERIA "+
                       " WHERE NUM_ANULACION = "+anulacion+" and TDOC_CODIGO='"+tipo+"' AND MAG_CODIGO  = "+agencia+"  AND NUM_DOCUMENTO >="+numeroInicial+" and NUM_DOCUMENTO <="+numeroFinal+"  ;");
											
			if(DBFunctions.Transaction(sqlUpd))
				Response.Redirect(indexPage+"?process=Comercial.AnulacionPapeleria&act=1&path="+Request.QueryString["path"]+"&anl="+anulacion);
			else
				lblError.Text=DBFunctions.exceptions;
		}
		private void btnAnular_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue.Replace("|","").Trim();
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado.");
				return;
			}
			string anulacion=DBFunctions.SingleData("SELECT MANULACION_NUMERO+1 FROM DBXSCHEMA.MANULACION_PAPELERIA ORDER BY MANULACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(anulacion.Length==0)anulacion="1";
			//Validar
			#region Documentos
			ArrayList arrNumeros=new ArrayList();
			ArrayList arrTiposDocs=new ArrayList();
			ArrayList sqlUpd=new ArrayList();
			int linea=1,fila=1;
			long numeroDoc=0;
			bool info=false;
			string errores="";
			foreach(DataGridItem dgrI in dgrDocumentos.Items)
			{
				bool errLin=false;
				DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
				TextBox txtNumeroDocumento=(TextBox)dgrI.FindControl("txtNumeroDocumento");
				DropDownList ddlConceptoAnulacion=(DropDownList)dgrI.FindControl("ddlConceptoAnulacion");
				if(ddlTipoDocumento.SelectedValue.Length>0 || ddlConceptoAnulacion.SelectedValue.Length>0 || txtNumeroDocumento.Text.Trim().Length>0)
				{
					string tipo=ddlTipoDocumento.SelectedValue.Trim().Replace("|","");
					string clase="M";
					string concepto=ddlConceptoAnulacion.SelectedValue.Trim();
					int prefijo=0;
					if(ddlTipoDocumento.SelectedValue.EndsWith("|") && ddlAgencia.SelectedValue.EndsWith("|"))
						prefijo=Convert.ToInt16(agencia);
					info=true;
					//Tipo
					if(tipo.Length==0)
					{
						errores+="Debe seleccionar la clase de documento en la línea "+ fila+". ";errLin=true;}
					//Clase
					if(clase.Length==0)
					{
						errores+="Debe seleccionar el tipo en la línea "+ fila+". ";errLin=true;}
					//Concepto
					if(concepto.Length==0)
					{
						errores+="Debe seleccionar el concepto de anulación en la línea "+ fila+". ";errLin=true;}
					//Numero
					try
					{
						numeroDoc=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtNumeroDocumento.Text.Trim());
						if(numeroDoc<1)throw(new Exception());}
					catch
					{
						errores+="Número de documento no válido en la línea "+fila+". ";errLin=true;}
					//Repetido?
					for(int n=0;n<linea-1;n++){
						string tipoO=arrTiposDocs[n].ToString();
						long numO=(long)arrNumeros[n];
						if(tipo.Equals(tipoO) && numeroDoc==numO){
							errores+="Ya ingresó el documento de la línea "+fila+". ";errLin=true;break;}
					}
					//Verificar papeleria
					DataSet dsVerifica=new DataSet();
					
					DBFunctions.Request(dsVerifica,IncludeSchema.NO,"CALL DBXSCHEMA.VERIFICA_ANULA_PAPELERIA('"+tipo+"',"+numeroDoc+","+agencia+");");
					int Ok = Convert.ToInt32(dsVerifica.Tables[0].Rows[0]["OK"]);				
					string Resultado =  dsVerifica.Tables[0].Rows[0]["RESULTADO"].ToString();
					if (Ok == 0)
					{
						errores+=" "+Resultado+" en la linea "+fila+". ";
						errLin=true;
					}

					/*
					if(!DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM MCONTROL_PAPELERIA WHERE TDOC_CODIGO='"+tipo+"' AND NUM_DOCUMENTO="+numeroDoc+" AND TIPO_DOCUMENTO='"+clase+"' AND MAG_CODIGO="+agencia+" AND NUM_RECEPCION IS NOT NULL AND FECHA_RECEPCION IS NOT NULL AND NUM_DESPACHO IS NOT NULL AND FECHA_DESPACHO IS NOT NULL AND NUM_ANULACION IS NULL AND MNIT_ANULACION IS NULL AND FECHA_ANULACION IS NULL AND MPLA_CODIGO IS NULL AND FECHA_USO IS NULL;")){
						errores+="No se encontró el documento de la línea "+fila+" (debe estar despachado y no utilizado). ";errLin=true;}
					*/
					arrNumeros.Add(numeroDoc);
					arrTiposDocs.Add(tipo);
					
					//Guardar
					if(!errLin)
					{
						sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET NUM_ANULACION="+anulacion+", FECHA_ANULACION='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', COD_ANULACION="+concepto+", MNIT_ANULACION='"+nitResponsable+"' WHERE TDOC_CODIGO='"+tipo+"' AND NUM_DOCUMENTO="+numeroDoc+";");
						sqlUpd.Add("INSERT INTO DBXSCHEMA.MANULACION_PAPELERIA VALUES("+anulacion+","+linea+","+agencia+",'"+tipo+"','"+clase+"',"+numeroDoc+","+concepto+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"');");
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";}
					linea++;
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}
			#endregion
			if(!info)errores+="No ingresó información.";
			if(errores.Length==0)
			{
				if(DBFunctions.Transaction(sqlUpd))
					Response.Redirect(indexPage+"?process=Comercial.AnulacionPapeleria&act=1&path="+Request.QueryString["path"]+"&anl="+anulacion);
				else
					lblError.Text=DBFunctions.exceptions;}
			else strActScript+="alert('"+errores+"');";
		}
	}
}
