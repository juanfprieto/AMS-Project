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
	using Ajax;
	using System.Configuration;
    using AMS.Tools;
	/// <summary>
	///		Descripción breve de AMS_Comercial_ModificarBusAfiliado.
	/// </summary>
	public class AMS_Comercial_ModificarBusAfiliado : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.TextBox txtVin;
		protected System.Web.UI.WebControls.TextBox txtCatalogo;
		protected System.Web.UI.WebControls.TextBox FechaIngreso;
		protected System.Web.UI.WebControls.TextBox NumBus;
		protected System.Web.UI.WebControls.DropDownList ddlestado;
		protected System.Web.UI.WebControls.DropDownList ddlplaca;
		protected System.Web.UI.WebControls.TextBox numeroviejo;
		string txtVinS,txtCatalogoS;
		public string idComponentes, idNumerosDocs, idEntidades, idDesdeFchs, idHastaFchs, idArchivos, idLinksImg,imgCambia;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox valor;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList categoria;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.WebControls.TextBox TextBox3;
		protected System.Web.UI.WebControls.TextBox ddlpropietario;
		protected System.Web.UI.WebControls.TextBox ddlasociado;
		protected System.Web.UI.WebControls.TextBox ddlconductor;
		protected System.Web.UI.WebControls.TextBox txtObservaciones;
		protected System.Web.UI.WebControls.TextBox txtReposicion;
		protected System.Web.UI.WebControls.TextBox txtPotencia;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.TextBox conductor2;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Button btnGuardarComponentes;
		protected System.Web.UI.WebControls.DataGrid dgrComponentes;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		protected System.Web.UI.WebControls.Button btnGuardarDocumentos;
		protected System.Web.UI.WebControls.DropDownList ddlConfiguracion;
		protected System.Web.UI.WebControls.TextBox ddlpropietarioa;
		protected System.Web.UI.WebControls.TextBox ddlasociadoa;
		protected System.Web.UI.WebControls.TextBox ddlconductora;
		protected System.Web.UI.WebControls.TextBox conductor2a;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox txtCapacidadC;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtCapacidad;
		protected System.Web.UI.WebControls.Button btnPropietarios;
		protected System.Web.UI.WebControls.Panel pnlBus;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Panel pnlPropietarios;
		protected System.Web.UI.WebControls.Label lblPlaca;
		protected System.Web.UI.WebControls.DataGrid dgrPropietarios;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Button btnCancelar;
		protected System.Web.UI.WebControls.Button btnGuardarP;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.TextBox txtFechaN;
		protected System.Web.UI.WebControls.Label Label26;
		protected System.Web.UI.WebControls.DataGrid dgrPropietariosHst;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(Comercial.AMS_Comercial_ModificarBusAfiliado));	
			// Introducir aquí el código de usuario para inicializar la página
			txtVinS=Request.Form[txtVin.UniqueID];
			txtCatalogoS=Request.Form[txtCatalogo.UniqueID];
            if (!IsPostBack)
			{

				DatasToControls bind=  new DatasToControls();
				int conteo=0;	
				if(conteo==0)
				{
						
					bind.PutDatasIntoDropDownList(ddlplaca,"select mcat_placa  from dbxschema.mbusafiliado where testa_codigo>=0 ORDER BY MCAT_PLACA;");
					ListItem it=new ListItem("--Placa--","");
					ddlplaca.Items.Insert(0,it);
				}
				bind.PutDatasIntoDropDownList(ddlestado,"select testa_codigo,testa_descripcion from DBXSCHEMA.testadobusafiliado where testa_codigo>=0");
				txtVin.Text=DBFunctions.SingleData("select mcat_vin from dbxschema.mbusafiliado where mcat_placa='"+ddlplaca.SelectedValue+"'");
				txtCatalogo.Text=DBFunctions.SingleData("select pcat_codigo from DBXSCHEMA.MCATALOGOVEHICULO where MCAT_PLACA='"+ddlplaca.SelectedValue+"'");
				bind.PutDatasIntoDropDownList(categoria,"Select mbus_categoria from DBXSCHEMA.MCATEGORIA_FONDO_REPOSICION_BUS");
				bind.PutDatasIntoDropDownList(ddlConfiguracion,"select MCON_COD,NOMBRE from DBXSCHEMA.MCONFIGURACIONBUS ORDER BY NOMBRE");
				
				//Cargar tabla componentes
				DataSet dtsComponentes = new DataSet();
				DBFunctions.Request(dtsComponentes,IncludeSchema.NO,"SELECT TC.TCOMP_TIPOCOMP AS TCOMP_TIPOCOMP, TC.TCOMP_NOMBCOMP AS TCOMP_NOMBCOMP, BC.VALOR AS VALOR FROM DBXSCHEMA.TCOMPONENTEBUS TC LEFT JOIN DBXSCHEMA.MBUS_COMPONENTE BC ON TC.TCOMP_TIPOCOMP=BC.TCOMP_TIPOCOMP AND BC.MCAT_PLACA='';");
				dgrComponentes.DataSource=dtsComponentes.Tables[0];
				dgrComponentes.DataBind();
				//Cargar tabla documentos
				DataSet dtsDocumentos = new DataSet();
				DBFunctions.Request(dtsDocumentos,IncludeSchema.NO,"SELECT TD.TDOC_TIPODOC AS TDOC_TIPODOC, TD.TDOC_NOMBDOC AS TDOC_NOMBDOC, BD.NUMERO AS NUMERO, BD.MNIT_ENTIDAD AS MNIT_ENTIDAD, BD.FECHA_DESDE AS FECHA_DESDE, BD.FECHA_HASTA AS FECHA_HASTA, BD.IMAGEN AS IMAGEN FROM DBXSCHEMA.TDOCUMENTOBUS TD LEFT JOIN DBXSCHEMA.MBUS_DOCUMENTO BD ON TD.TDOC_TIPODOC=BD.TDOC_TIPODOC AND BD.MCAT_PLACA='';");
				dgrDocumentos.DataSource=dtsDocumentos.Tables[0];
				dgrDocumentos.DataBind();
			
				//Guardar nombres campos componentes
				//valor
				idComponentes="";
				for(int nC=0;nC<dgrComponentes.Items.Count;nC++)
					idComponentes+=dgrComponentes.Items[nC].FindControl("txtValorComponente").ClientID+"?";
				idComponentes=idComponentes.TrimEnd('?');
				ViewState["idComponentes"]=idComponentes;
				
				//Guardar nombres campos documentos
				idLinksImg=idArchivos=idHastaFchs=idDesdeFchs=idEntidades=idNumerosDocs="";
				for(int nC=0;nC<dgrDocumentos.Items.Count;nC++){
					idNumerosDocs+=dgrDocumentos.Items[nC].FindControl("txtNumeroDocumento").ClientID+"?";
					idEntidades+=dgrDocumentos.Items[nC].FindControl("txtEntidad").ClientID+"?";
					idDesdeFchs+=dgrDocumentos.Items[nC].FindControl("txtFechaDesde").ClientID+"?";
					idHastaFchs+=dgrDocumentos.Items[nC].FindControl("txtFechaHasta").ClientID+"?";
					idArchivos+=dgrDocumentos.Items[nC].FindControl("txtImagen").ClientID+"?";
					idLinksImg+=dgrDocumentos.Items[nC].FindControl("lnkImagen").ClientID+"?";
				}
				idNumerosDocs=idNumerosDocs.TrimEnd('?');
				idEntidades=idEntidades.TrimEnd('?');
				idDesdeFchs=idDesdeFchs.TrimEnd('?');
				idHastaFchs=idHastaFchs.TrimEnd('?');
				idArchivos=idArchivos.TrimEnd('?');
				idLinksImg=idLinksImg.TrimEnd('?');
				ViewState["idNumerosDocs"]=idNumerosDocs;
				ViewState["idEntidades"]=idEntidades;
				ViewState["idDesdeFchs"]=idDesdeFchs;
				ViewState["idHastaFchs"]=idHastaFchs;
				ViewState["idArchivos"]=idArchivos;
				ViewState["idLinksImg"]=idLinksImg;

				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El bus ha sido modificado.");
			}
			//Cargar nombres de controles generados dinamicamente del viewstate
			idComponentes=ViewState["idComponentes"].ToString();
			idNumerosDocs=ViewState["idNumerosDocs"].ToString();
			idEntidades=ViewState["idEntidades"].ToString();
			idDesdeFchs=ViewState["idDesdeFchs"].ToString();
			idHastaFchs=ViewState["idHastaFchs"].ToString();
			idArchivos=ViewState["idArchivos"].ToString();
			idLinksImg=ViewState["idLinksImg"].ToString();
		}
		#region Ajax
		[Ajax.AjaxMethod]
		public DataSet CargarPlaca(string mplaca)
		{
			DataSet Vins= new DataSet();
			string sqlComponentes="SELECT TC.TCOMP_TIPOCOMP AS TCOMP_TIPOCOMP, TC.TCOMP_NOMBCOMP AS TCOMP_NOMBCOMP, BC.VALOR AS VALOR FROM DBXSCHEMA.TCOMPONENTEBUS TC LEFT JOIN DBXSCHEMA.MBUS_COMPONENTE BC ON TC.TCOMP_TIPOCOMP=BC.TCOMP_TIPOCOMP AND BC.MCAT_PLACA='"+mplaca+"';";
			string sqlDocumentos="SELECT TD.TDOC_TIPODOC AS TDOC_TIPODOC, TD.TDOC_NOMBDOC AS TDOC_NOMBDOC, BD.NUMERO AS NUMERO, BD.MNIT_ENTIDAD AS MNIT_ENTIDAD, BD.FECHA_DESDE AS FECHA_DESDE, BD.FECHA_HASTA AS FECHA_HASTA, BD.IMAGEN AS IMAGEN FROM DBXSCHEMA.TDOCUMENTOBUS TD LEFT JOIN DBXSCHEMA.MBUS_DOCUMENTO BD ON TD.TDOC_TIPODOC=BD.TDOC_TIPODOC AND BD.MCAT_PLACA='"+mplaca+"';";
			string sqlBus="select mb.mcat_vin as VIN, mb.pcat_codigo as CATALOGO,mb.fec_ingreso AS FECHA,mb.MBUS_NUMERO as NUMERO,mb.MBUS_VALOR AS VALOR,mb.MNIT_NITPROPIETARIO AS PROPIETARIO,mb.MNIT_ASOCIADO AS ASOCIADO, mb.MNIT_NITCHOFER AS CHOFER, mb.MNIT_SEGCONDUCTOR AS CHOFER2, mb.MBUS_REPOSICION AS REPOSICION, mb.MBUS_OBSERVACIONES AS OBSERVACIONES, mb.MBUS_POTENCIA AS POTENCIA, mb.MBUS_CATEGORIA AS CATEGORIA, mb.TESTA_CODIGO AS ESTADO, mb.MCON_COD AS CONFIG, np.MNIT_NOMBRES concat ' ' concat np.MNIT_APELLIDOS AS NOMPROPIETARIO, na.MNIT_NOMBRES concat ' ' concat na.MNIT_APELLIDOS AS NOMASOCIADO, nc1.MNIT_NOMBRES concat ' ' concat nc1.MNIT_APELLIDOS AS NOMCONDUCTOR1, nc2.MNIT_NOMBRES concat ' ' concat nc2.MNIT_APELLIDOS AS NOMCONDUCTOR2, CAPACIDAD_COMBUSTIBLE AS GALONAJE, CAPACIDAD_PASAJEROS AS PASAJEROS from dbxschema.mbusafiliado mb "+
			"LEFT JOIN dbxschema.mnit np ON np.MNIT_NIT=mb.MNIT_NITPROPIETARIO "+
			"LEFT JOIN dbxschema.mnit na ON na.MNIT_NIT=mb.MNIT_ASOCIADO "+
			"LEFT JOIN dbxschema.mnit nc1 ON nc1.MNIT_NIT=mb.MNIT_NITCHOFER "+
			"LEFT JOIN dbxschema.mnit nc2 ON nc2.MNIT_NIT=mb.MNIT_SEGCONDUCTOR "+
			"where mcat_placa='"+mplaca+"';";
			DBFunctions.Request(Vins,IncludeSchema.NO,sqlBus+sqlComponentes+sqlDocumentos);
			return Vins;
		}

		#endregion Ajax
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
			this.btnPropietarios.Click += new System.EventHandler(this.btnPropietarios_Click);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.btnGuardarComponentes.Click += new System.EventHandler(this.btnGuardarComponentes_Click);
			this.btnGuardarDocumentos.Click += new System.EventHandler(this.btnGuardarDocumentos_Click);
			this.dgrPropietarios.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgrPropietarios_ItemCommand);
			this.btnGuardarP.Click += new System.EventHandler(this.btnGuardarP_Click);
			this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region Botones
		private void btnGuardarComponentes_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			string valor;
			double valorR;
			int numComponentes=dgrComponentes.Items.Count;
			bool[]componentes=new bool[numComponentes];
			//Placa
			if(ddlplaca.SelectedValue.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar la placa.");
				return;
			}
			//Encargado
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0){
                Utils.MostrarAlerta(Response, "  El usuario actual (responsable) no tiene un NIT asociado.");
				return;
			}
			//Valores
			for(int nC=0;nC<numComponentes;nC++){
				valor=((TextBox)dgrComponentes.Items[nC].FindControl("txtValorComponente")).Text.Replace(",","").Trim();
				if(valor.Length>0)
					try{
						valorR=Double.Parse(valor);
						componentes[nC]=true;
					}
					catch{
                        Utils.MostrarAlerta(Response, "  El valor de un componente no es válido.");
						return;
					}
				else
					componentes[nC]=false;
			}
			//Borrar anteriores valores
			DBFunctions.NonQuery("delete from dbxschema.mbus_componente where MCAT_PLACA='"+ddlplaca.SelectedValue+"'");
			//Guardar nuevos valores
			for(int nC=0;nC<numComponentes;nC++)
				if(componentes[nC]){
					valorR=Double.Parse(((TextBox)dgrComponentes.Items[nC].FindControl("txtValorComponente")).Text.Replace(",","").Trim());
					DBFunctions.NonQuery("insert into dbxschema.mbus_componente values("+dgrComponentes.DataKeys[nC]+",'"+ddlplaca.SelectedValue+"',"+valorR+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"')");
				}
			imgCambia="<script language='javascript'>cargarPlacaDB(document.getElementById('"+ddlplaca.ClientID+"'));</script>";
		}

		//Guardar Documentos
		private void btnGuardarDocumentos_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			int numDocumentos=dgrDocumentos.Items.Count;
			bool[]documentos=new bool[numDocumentos];
			string numero,entidad,desde,hasta;
			//Placa
			if(ddlplaca.SelectedValue.Length==0)
			{
                Utils.MostrarAlerta(Response, "  Debe seleccionar la placa del vehículo.");
				return;
			}
			//Valores documento
			for(int nC=0;nC<numDocumentos;nC++)
			{
				numero=((TextBox)dgrDocumentos.Items[nC].FindControl("txtNumeroDocumento")).Text.Trim();
				entidad=((TextBox)dgrDocumentos.Items[nC].FindControl("txtEntidad")).Text.Trim();
				if(numero.Length+entidad.Length>0){
					if(numero.Length==0){
                        Utils.MostrarAlerta(Response, "  Debe ingresar el número de los documentos que quiere actualizar.");
						return;
					}
					if(entidad.Length==0){
                        Utils.MostrarAlerta(Response, "  Debe ingresar el número de la entidad de los documentos que quiere actualizar.");
						return;
					}
					documentos[nC]=true;
				}
				else
					documentos[nC]=false;
			}
			//Guardar nuevos documentos
			for(int nC=0;nC<numDocumentos;nC++){
				numero=((TextBox)dgrDocumentos.Items[nC].FindControl("txtNumeroDocumento")).Text.Trim();
				entidad=((TextBox)dgrDocumentos.Items[nC].FindControl("txtEntidad")).Text.Trim();
				desde=((TextBox)dgrDocumentos.Items[nC].FindControl("txtFechaDesde")).Text.Trim();
				hasta=((TextBox)dgrDocumentos.Items[nC].FindControl("txtFechaHasta")).Text.Trim();
				desde=(desde.Length>0)?"'"+desde+"'":"NULL";
				hasta=(hasta.Length>0)?"'"+hasta+"'":"NULL";
				if(documentos[nC]){
					string sqlDoc="";
					//Actualizar si existe o insertar si no
					if(DBFunctions.RecordExist("SELECT * FROM DBXSCHEMA.MBUS_DOCUMENTO WHERE TDOC_TIPODOC="+dgrDocumentos.DataKeys[nC]+" AND MCAT_PLACA='"+ddlplaca.SelectedValue+"'"))
						sqlDoc="UPDATE DBXSCHEMA.MBUS_DOCUMENTO SET NUMERO='"+numero+"', MNIT_ENTIDAD='"+entidad+"', FECHA_DESDE="+desde+", FECHA_HASTA="+hasta+" WHERE TDOC_TIPODOC="+dgrDocumentos.DataKeys[nC]+" AND MCAT_PLACA='"+ddlplaca.SelectedValue+"'";
					else
						sqlDoc="INSERT INTO DBXSCHEMA.MBUS_DOCUMENTO VALUES("+dgrDocumentos.DataKeys[nC]+",'"+ddlplaca.SelectedValue+"','"+numero+"','"+entidad+"',"+desde+","+hasta+",'')";
						DBFunctions.NonQuery(sqlDoc);
				}
			}
			//Guardar imagenes
			for(int nC=0;nC<numDocumentos;nC++){
				if(documentos[nC]){
					HttpPostedFile pstFile=((System.Web.UI.HtmlControls.HtmlInputFile)dgrDocumentos.Items[nC].FindControl("txtImagen")).PostedFile;
					if(pstFile.FileName!=""){
						string imgType=pstFile.ContentType.ToLower();
						if(!imgType.EndsWith("jpeg") && !imgType.EndsWith("gif")){
                            Utils.MostrarAlerta(Response, "  Imágen no válida debe estar en formato jpg o gif.");
							return;
						}
						imgType=imgType.EndsWith("jpeg")?"jpg":"gif";
						//Guardar Archivo
						string arcImagen=ddlplaca.SelectedValue+"-"+dgrDocumentos.DataKeys[nC]+"."+imgType;
						pstFile.SaveAs(ConfigurationManager.AppSettings["MainPath"]+@"\img\DOC_VEHICULOS\"+arcImagen);
						//Actualizar registro
						DBFunctions.NonQuery("UPDATE DBXSCHEMA.MBUS_DOCUMENTO SET IMAGEN='"+arcImagen+"' WHERE TDOC_TIPODOC="+dgrDocumentos.DataKeys[nC]+" AND MCAT_PLACA='"+ddlplaca.SelectedValue+"'");
					}
				}
			}
			imgCambia="<script language='javascript'>cargarPlacaDB(document.getElementById('"+ddlplaca.ClientID+"'));</script>";
		}

		//Actualizar
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			int NumBusS,NumBusA,valorN=0,pasajeros;
			double galonaje;
			//int ini=Convert.ToInt32(DBFunctions.SingleData("select TCAT_RANGOI from DBXSCHEMA.TCAT_BUS WHERE TCAT_CODIGO='"+categoria.SelectedValue.ToString()+"'"));
			//int fin=Convert.ToInt32(DBFunctions.SingleData("select TCAT_RANGOF from DBXSCHEMA.TCAT_BUS WHERE TCAT_CODIGO='"+categoria.SelectedValue.ToString()+"'"));
			string asociado,chofer1,chofer2,reposicion;
			
			//Validaciones
			//Placa
			if(ddlplaca.SelectedValue.Length==0)
			{
                Utils.MostrarAlerta(Response, "  Debe seleccionar la placa del vehículo.");
				return;
			}
			//NITS
			if(ddlpropietario.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "  Debe dar el nit del propietario.");
				return;
			}
			asociado=((ddlasociado.Text.Length>0)?("'"+ddlasociado.Text+"'"):"NULL");
			chofer1=((ddlconductor.Text.Length>0)?("'"+ddlconductor.Text+"'"):"NULL");
			chofer2=((conductor2.Text.Length>0)?("'"+conductor2.Text+"'"):"NULL");
			reposicion=((txtReposicion.Text.Length>0)?("'"+txtReposicion.Text+"'"):"NULL");

			//Numero de bus
			try
			{
				NumBusA=Convert.ToInt32(numeroviejo.Text.Replace(",",""));
				if(NumBus.Text.Length>0)
					NumBusS=Convert.ToInt32(NumBus.Text.Replace(",",""));
				else
					NumBusS=NumBusA;
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  'Número de bus no válido.");
				return;
			}
			//Pasajeros
			try{
				pasajeros=Math.Abs(Convert.ToInt32(txtCapacidad.Text.Replace(",","")));
			}
			catch{
                Utils.MostrarAlerta(Response, "  Capacidad de pasajeros no válida.");
				return;
			}
			//Valor
			try
			{
				valorN=Convert.ToInt32(valor.Text.Replace(",",""));}
			catch
			{
                Utils.MostrarAlerta(Response, "  Valor de vehículo no válido.");
				return;
			}
			//Galonaje
			try
			{
				galonaje=Convert.ToDouble(txtCapacidadC.Text.Replace(",",""));}
			catch
			{
                Utils.MostrarAlerta(Response, "  Capacidad de combustible no válida.");
				return;
			}
			//Revisar categoria
			//if(!(valorN >= ini && valorN <= fin))
			//{
			//	Response.Write("<script language='javascript'>alert('Categoria incorrecta o valor incorrecto.');</script>");
			//	return;
			//}
			//Revisar que no se ha utilizado el numero si es nuevo
			if(NumBusA!=NumBusS)
				if(DBFunctions.RecordExist("SELECT MBUS_NUMERO from DBXSCHEMA.MBUS_NUMERO_INTERNO where MBUS_NUMERO="+NumBusS+" ") || DBFunctions.RecordExist("SELECT MBUS_NUMERO from DBXSCHEMA.MBUSAFILIADO where MBUS_NUMERO="+NumBusS+" "))
				{
                    Utils.MostrarAlerta(Response, "  El número del bus ya ha sido utilizado.");
					return;
				}
			string propietario=DBFunctions.SingleData("select MNIT_NITPROPIETARIO from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+ddlplaca.SelectedValue+"'").ToString();
			
			ArrayList sqlB=new ArrayList();
			sqlB.Add("UPDATE dbxschema.MBUSAFILIADO SET MNIT_NITPROPIETARIO='"+ddlpropietario.Text.ToString()+"',MNIT_ASOCIADO="+asociado+",MNIT_NITCHOFER="+chofer1+",MNIT_SEGCONDUCTOR="+chofer2+",FEC_INGRESO='"+FechaIngreso.Text.ToString()+"',TESTA_CODIGO="+ddlestado.SelectedValue.ToString()+",MBUS_VALOR="+valorN.ToString("0")+",MBUS_CATEGORIA='"+categoria.SelectedValue.ToString()+"',MCON_COD="+ddlConfiguracion.SelectedValue+",MBUS_NUMERO="+NumBusS+",MBUS_POTENCIA='"+txtPotencia.Text+"', MBUS_OBSERVACIONES='"+txtObservaciones.Text+"',MBUS_REPOSICION="+reposicion +", CAPACIDAD_COMBUSTIBLE="+galonaje.ToString("0")+", capacidad_pasajeros="+pasajeros.ToString("0")+" WHERE MCAT_PLACA='"+ddlplaca.SelectedValue.ToString()+"';");

			//Almacenar numero de bus en historial
			if(NumBusA!=NumBusS)
				sqlB.Add("insert into dbxschema.MBUS_NUMERO_INTERNO values ("+NumBusS+",'"+ddlplaca.SelectedValue+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"')");
			
			if(DBFunctions.Transaction(sqlB))
				Response.Redirect(indexPage+"?process=Comercial.ModificarBusAfiliado&act=1&path="+Request.QueryString["path"]);
			else
				lblError.Text=DBFunctions.exceptions;
		}

		#endregion Botones

		#region Propietarios
		private void btnPropietarios_Click(object sender, System.EventArgs e)
		{
			if(ddlplaca.SelectedValue.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar un bus.");
				return;
			}
			lblPlaca.Text=ddlplaca.SelectedValue;
			//Cargar propietarios
			DataSet dsPropietarios = new DataSet();
			DBFunctions.Request(dsPropietarios,IncludeSchema.NO,
				"SELECT MNIT_NIT, PORCENTAJE, CASE WHEN PRINCIPAL='S' THEN 1 ELSE 0 END AS PRINCIPAL, FECHA_PROPIETARIO FROM DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS WHERE MCAT_PLACA='"+ddlplaca.SelectedValue+"' ORDER BY PRINCIPAL DESC, PORCENTAJE DESC;"+
				"SELECT MNIT_NIT, PORCENTAJE, FECHA_PROPIETARIO FROM DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS_HST WHERE MCAT_PLACA='"+ddlplaca.SelectedValue+"' AND OPERACION='N' ORDER BY FECHA_PROPIETARIO DESC, PORCENTAJE ASC;");
			dgrPropietarios.DataSource=dsPropietarios.Tables[0];
			dgrPropietarios.DataBind();
			dgrPropietariosHst.DataSource=dsPropietarios.Tables[1];
			dgrPropietariosHst.DataBind();
			ViewState["PROPIETARIOS_ORIGINALES"]=dsPropietarios.Tables[0];
			ViewState["PROPIETARIOS_NUEVOS"]=dsPropietarios.Tables[0];
			txtFechaN.Text=Convert.ToDateTime(DBFunctions.SingleData("SELECT FECHA_PROPIETARIO FROM DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS WHERE MCAT_PLACA='"+ddlplaca.SelectedValue+"';")).ToString("yyyy-MM-dd");
			pnlBus.Visible=false;
			pnlPropietarios.Visible=true;
		}

		private void btnCancelar_Click(object sender, System.EventArgs e)
		{
			pnlBus.Visible=true;
			pnlPropietarios.Visible=false;
		}

		private void dgrPropietarios_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataTable dtPropietrariosAct=(DataTable)ViewState["PROPIETARIOS_NUEVOS"];
			if(!ActualizarPorcentajes())return;
			if(e.CommandName=="Agregar")
			{
				DataRow drN;
				string nit=((TextBox)e.Item.FindControl("txtNITN")).Text.Trim();
				double porcentaje=0;

				if(nit.Length==0)
				{
                    Utils.MostrarAlerta(Response, "  Debe dar el NIT.");
					return;}
				if(dtPropietrariosAct.Select("MNIT_NIT='"+nit+"'").Length>0)
				{
                    Utils.MostrarAlerta(Response, "  Ya existe el NIT, modifiquelo.");
					return;}

				try
				{
					porcentaje=Convert.ToDouble(((TextBox)e.Item.FindControl("txtPorcPropN")).Text);
					if(porcentaje<=0||porcentaje>100)throw(new Exception());}
				catch
				{
                    Utils.MostrarAlerta(Response, "  Porcentaje no válido");
					return;}
				drN=dtPropietrariosAct.NewRow();
				drN["MNIT_NIT"]=nit;
				drN["PORCENTAJE"]=porcentaje;
				drN["PRINCIPAL"]=0;
				dtPropietrariosAct.Rows.Add(drN);
			}
			else
			{
				dtPropietrariosAct.Rows.Remove(dtPropietrariosAct.Rows[e.Item.ItemIndex]);
			}
			ViewState["PROPIETARIOS_NUEVOS"]=dtPropietrariosAct;
			dgrPropietarios.DataSource=dtPropietrariosAct;
			dgrPropietarios.DataBind();
		}
		private bool ActualizarPorcentajes()
		{
			DataTable dtPropietrariosAct=(DataTable)ViewState["PROPIETARIOS_NUEVOS"];				
			int n=0;
			double porcentaje=0;
			foreach(DataGridItem dgiAct in dgrPropietarios.Items)
			{
				if(dgiAct.ItemType==ListItemType.AlternatingItem || dgiAct.ItemType==ListItemType.Item)
				{
					try
					{
						porcentaje=Convert.ToDouble(((TextBox)dgiAct.FindControl("txtPorcProp")).Text);
						if(porcentaje<=0||porcentaje>100)throw(new Exception());}
					catch
					{
                        Utils.MostrarAlerta(Response, "  Porcentaje no válido en el NIT " + dtPropietrariosAct.Rows[n]["MNIT_NIT"].ToString() + ".");
						return(false);}
					dtPropietrariosAct.Rows[n]["PRINCIPAL"]=(((CheckBox)dgiAct.FindControl("chkPrincipal")).Checked?1:0);
					dtPropietrariosAct.Rows[n]["PORCENTAJE"]=porcentaje;
					n++;
				}
			}
			ViewState["PROPIETARIOS_NUEVOS"]=dtPropietrariosAct;
			return(true);
		}

		private void btnGuardarP_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlAct=new ArrayList();
			string placa=ddlplaca.SelectedValue;
			//DataTable dtPropietrariosOrig;
			DataTable dtPropietrariosNuev=(DataTable)ViewState["PROPIETARIOS_NUEVOS"];
			bool hayPropietario=false;
			string nitPropietario="";
			int n;
			double totalPorcentaje=0;
			DateTime fechaActualizacion=new DateTime();

			//Fecha valida?
			try
			{
				fechaActualizacion=Convert.ToDateTime(txtFechaN.Text);
				if(fechaActualizacion>DateTime.Now) throw(new Exception());
				if(!DBFunctions.RecordExist("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE mcat_placa='"+ddlplaca.SelectedValue+"' AND FEC_INGRESO<='"+fechaActualizacion.ToString("yyyy-MM-dd")+"';")) throw(new Exception());
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  Fecha no valida, debe ser menor o igual a la fecha actual y mayor o igual a la fecha de afiliación del bus.");
				return;
			}
			
			
			//Verificar porcentajes
			if(!ActualizarPorcentajes())return;

			//Verificar propietarios
			n=0;
			double porcAux=0;
			foreach(DataGridItem dgrI in dgrPropietarios.Items){
				if(dgrI.ItemType==ListItemType.AlternatingItem || dgrI.ItemType==ListItemType.Item){
					if(((CheckBox)dgrI.FindControl("chkPrincipal")).Checked){
						if(hayPropietario){
                            Utils.MostrarAlerta(Response, "  Solo puede existir un propietario principal.");
							return;
						}
						hayPropietario=true;
						nitPropietario=dtPropietrariosNuev.Rows[n]["MNIT_NIT"].ToString();
					}
					porcAux=Convert.ToDouble(((TextBox)dgrI.FindControl("txtPorcProp")).Text);
					if(porcAux<1){
                        Utils.MostrarAlerta(Response, "  Porcentaje no válido.");
						return;
					}
					totalPorcentaje+=porcAux;
					n++;
				}
			}
			if(totalPorcentaje<99||totalPorcentaje>100){
                Utils.MostrarAlerta(Response, "  El total de porcentajes debe ser igual a 100%.");

				return;
			}
			if(!hayPropietario){
                Utils.MostrarAlerta(Response, "  Debe dar un propietario principal.");
				return;
			}

			//Reemplazar los actuales si la fecha es mas reciente a la ultima registrada
			if(!DBFunctions.RecordExist("SELECT FECHA_PROPIETARIO FROM DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS WHERE MCAT_PLACA='"+placa+"' AND FECHA_PROPIETARIO>='"+fechaActualizacion.ToString("yyyy-MM-dd")+"';")){
				//Borrar los ultimos registrados
				sqlAct.Add("DELETE FROM DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS WHERE MCAT_PLACA='"+placa+"';");
				//Guardar nuevos ultimos valores registrados
				foreach(DataRow drN in dtPropietrariosNuev.Rows)
					sqlAct.Add("INSERT INTO DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS VALUES('"+placa+"','"+drN["MNIT_NIT"]+"',"+drN["PORCENTAJE"]+",'"+((int)drN["PRINCIPAL"]==1?"S":"N")+"','"+fechaActualizacion.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL);");
				sqlAct.Add("UPDATE DBXSCHEMA.MBUSAFILIADO SET MNIT_NITPROPIETARIO='"+nitPropietario+"' WHERE MCAT_PLACA='"+placa+"';");
			}

			//Guardar historial
			//Sacar los que esten en la fecha actual
			sqlAct.Add("UPDATE DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS_HST SET OPERACION='S', FECHA_SALE='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE MCAT_PLACA='"+placa+"' AND FECHA_PROPIETARIO='"+fechaActualizacion.ToString("yyyy-MM-dd")+"';");
			//Ingresar los de la fecha como nuevos
			foreach(DataRow drN in dtPropietrariosNuev.Rows)
				sqlAct.Add("INSERT INTO DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS_HST VALUES(DEFAULT,'"+placa+"','"+drN["MNIT_NIT"]+"',"+drN["PORCENTAJE"]+",'"+((int)drN["PRINCIPAL"]==1?"S":"N")+"','N','"+fechaActualizacion.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',NULL);");
			
			//Actualizar
			if(DBFunctions.Transaction(sqlAct)){
				//ESTABLECER PROPIETARIO TITULAR
                Utils.MostrarAlerta(Response, "  Los propietarios del bus han sido modificados.");
				ddlpropietario.Text=nitPropietario;
				ddlpropietarioa.Text=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 from MNIT where MNIT_NIT='"+nitPropietario+"';");
				pnlBus.Visible=true;
				pnlPropietarios.Visible=false;
			}
			else
				lblError.Text=DBFunctions.exceptions;
		}
		#endregion Propietarios
	}
}
