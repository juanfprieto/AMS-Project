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

	/// <summary>
	///		Descripción breve de AMS_Comercial_RelacionarPlanilla.
	/// </summary>
	public class AMS_Comercial_RelacionarPlanilla : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlTipoAsociar;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Panel pnlPlanilla;
		protected System.Web.UI.WebControls.Label lblTipo;
		protected System.Web.UI.WebControls.Label lblTipoDesc;
		protected System.Web.UI.WebControls.DropDownList ddlNumeroTipo;
		protected System.Web.UI.WebControls.Button btnAsociar;
		protected System.Web.UI.WebControls.Panel pnlInfoElemento;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Panel pnlTipoAsociar;
		protected System.Web.UI.WebControls.Label lblTipoExp;
		protected System.Web.UI.WebControls.Label lblTipoInfo;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Panel pnlElemento;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtNumeroDocumento;
		protected System.Web.UI.WebControls.Label Label35;
		protected System.Web.UI.WebControls.TextBox txtNumeroPlanilla;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			if (!IsPostBack){
				
				if(Convert.ToInt32(Request.QueryString["recarga"])==1)
				{
					//Se guardo la informacion y se muestra la rta.
					Response.Write("<script language='javascript'>alert('Se ha relacionado el elemento a la planilla.');</script>");
					
				}
				Agencias.TraerAgenciasUsuario(ddlAgencia);
							
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
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
			this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
			this.ddlTipoAsociar.SelectedIndexChanged += new System.EventHandler(this.ddlTipoAsociar_SelectedIndexChanged);
			this.ddlNumeroTipo.SelectedIndexChanged += new System.EventHandler(this.ddlNumeroTipo_SelectedIndexChanged);
			this.btnAsociar.Click += new System.EventHandler(this.btnAsociar_Click);
			this.txtNumeroDocumento.TextChanged += new System.EventHandler(this.txtNumeroDocumento_TextChanged);
			
			this.Load += new System.EventHandler(this.Page_Load);
			


		}
		#endregion

		//Cambia la agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			string agencia=ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlPlanilla.Visible=false;
			pnlElemento.Visible=false;
			pnlInfoElemento.Visible=false;
			pnlTipoAsociar.Visible=true;
			ddlTipoAsociar.SelectedIndex=0;
			//Cambia la ruta
			if(agencia.Length==0)
			{
				pnlTipoAsociar.Visible=false;
				return;
			}
		}

		//Cambia el tipo
		private void ddlTipoAsociar_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string FechaHoy  = DateTime.Now.ToString("yyyy-MM-dd");
			string agencia=ddlAgencia.SelectedValue;
			string tipo=ddlTipoAsociar.SelectedValue;
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			DatasToControls bind=new DatasToControls();
			pnlPlanilla.Visible=true;
			//txtNumeroDocumento.Visible=true;
			pnlElemento.Visible=false;
			pnlInfoElemento.Visible=false;
			//Cambia la ruta
			if(agencia.Length==0)
			{
				pnlPlanilla.Visible=false;
                txtNumeroDocumento.Visible=false;
				ddlNumeroTipo.Items.Clear();
			    

				return;
			}
			
		}
		
		 
		private void txtNumeroDocumento_TextChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			string tipo=ddlTipoAsociar.SelectedValue;
			string planilla=txtNumeroDocumento.Text;
			DatasToControls bind=new DatasToControls();
			pnlElemento.Visible=true;
			pnlInfoElemento.Visible=false;
						
			if(!validarCampo(planilla))return;
		
			//validar que si corresponda la planilla.
			if(DBFunctions.RecordExist("select mp.mpla_codigo from dbxschema.mplanillaviaje mp,"+
				" DBXSCHEMA.MCONTROL_PAPELERIA cp, dbxschema.mviaje mv,dbxschema.mbusafiliado mb where "+
				" mp.mpla_codigo = cp.NUM_DOCUMENTO and cp.TDOC_CODIGO = 'PLA' and mp.mag_codigo="+agencia+" AND "+
				" (mp.fecha_liquidacion is null or TIPO_DOCUMENTO = 'M') and mp.fecha_interface is null and "+
				" mb.mcat_placa=mv.mcat_placa and mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo "+
				" and  mp.mpla_codigo="+planilla+";"))
			{ 
			    
						           
				//Cambia la ruta
				if(planilla.Length==0)
				{
					pnlElemento.Visible=false;
					ddlNumeroTipo.Items.Clear();
					return;
				}
				string RutaPrincipal = DBFunctions.SingleData("select mv.mrut_codigo from  dbxschema.mplanillaviaje mp, dbxschema.mviaje mv "+
					"where mp.mpla_codigo = "+planilla+" and "+
					"mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo;");
				lblTipo.Text=tipo+":";
				lblTipoDesc.Text=tipo+":";
				ddlNumeroTipo.Items.Clear();
				if(tipo.Equals("Giros"))
				{
					lblTipoExp.Text="Numero: Ag.Origen: Ag.Destino: Emisor: Destinatario";
					bind.PutDatasIntoDropDownList(ddlNumeroTipo,
						"select mg.num_documento,"+
						"rtrim(char(mg.num_documento)) concat ': ' concat maO.mage_nombre concat ': ' concat "+
						"maD.mage_nombre concat ': ' concat "+
						"ne.MNIT_NOMBRES concat ' ' concat ne.MNIT_APELLIDOS concat ': ' concat "+
						"nd.MNIT_NOMBRES concat ' ' concat nd.MNIT_APELLIDOS "+
						"from DBXSCHEMA.mgiros mg, DBXSCHEMA.magencia maO, DBXSCHEMA.magencia maD, "+
						"DBXSCHEMA.mnit ne, DBXSCHEMA.mnit nd,DBXSCHEMA.MRUTAS mr,DBXSCHEMA.MRUTA_intermedia ri  "+
						"where mpla_codigo is null and test_codigo='A' and mg.mag_agencia_origen="+ddlAgencia.SelectedValue+" and "+
						"RI.MRUTA_PRINCIPAL='"+RutaPrincipal+"' AND RI.MRUTA_SECUNDARIA=mg.mrut_codigo and mr.mrut_codigo=mg.mrut_codigo and "+
						"ne.mnit_nit=mg.mnit_emisor and nd.mnit_nit=mg.mnit_destinatario and "+
						"maD.mag_codigo=mg.mag_agencia_destino and maO.mag_codigo=mg.mag_agencia_origen order by mg.num_documento;");
					ddlNumeroTipo.Items.Insert(0, new ListItem("---seleccione---",""));
				}
				else
				{
					if(tipo.Equals("Encomiendas"))
					{
						lblTipoExp.Text="Numero: Agencia Origen: Ciudad Destino: Emisor: Destinatario";
						bind.PutDatasIntoDropDownList(ddlNumeroTipo,
							"select me.num_documento, rtrim(char(me.num_documento)) concat ': ' concat maO.mage_nombre concat ': ' "+
							"concat pcD.pciu_nombre concat ': ' concat ne.MNIT_NOMBRES "+
							"concat ' ' concat ne.MNIT_APELLIDOS concat ': ' concat nd.MNIT_NOMBRES "+
							" concat ' ' concat nd.MNIT_APELLIDOS "+
							"from DBXSCHEMA.mencomiendas me, DBXSCHEMA.magencia maO,DBXSCHEMA.mnit ne, "+
							"DBXSCHEMA.mnit nd, DBXSCHEMA.pciudad pcD, DBXSCHEMA.MRUTAS mr,DBXSCHEMA.MRUTA_intermedia ri  "+
							"where me.mpla_codigo is null and test_codigo='A' and me.mag_recibe="+ddlAgencia.SelectedValue+" and "+
							"RI.MRUTA_PRINCIPAL='"+RutaPrincipal+"' AND RI.MRUTA_SECUNDARIA=me.mrut_codigo and mr.mrut_codigo=me.mrut_codigo and "+
							"ne.mnit_nit=me.mnit_emisor and nd.mnit_nit=me.mnit_destinatario and pcD.pciu_codigo=mr.pciu_coddes and "+
							"maO.mag_codigo=me.mag_recibe order by me.num_documento;");
						ddlNumeroTipo.Items.Insert(0, new ListItem("---seleccione---",""));
					
					}
					else
					{
						if(tipo.Equals("Anticipos/servicios"))
						{
							lblTipoExp.Text="Tipo-Numero: Placa: Concepto: Destinatario";
							bind.PutDatasIntoDropDownList(ddlNumeroTipo,
								"select rtrim(char(mg.num_documento)) concat '-' concat mg.tdoc_codigo as tpStr,"+
								"tdoc_codigo concat '-' concat rtrim(char(mg.num_documento)) concat ': ' concat "+
								"mg.mcat_placa concat ': ' concat tg.nombre concat ': ' concat "+
								"nd.MNIT_NOMBRES concat ' ' concat nd.MNIT_APELLIDOS "+
								"from DBXSCHEMA.mgastos_transportes mg, "+
								"DBXSCHEMA.mnit nd, DBXSCHEMA.TCONCEPTOS_TRANSPORTES tg where "+
								"mg.mpla_codigo is null and mg.test_codigo='A' and mg.mag_codigo="+ddlAgencia.SelectedValue+" and "+
								"nd.mnit_nit=mg.mnit_responsable_recibe and "+
								"mg.TCON_CODIGO=tg.TCON_CODIGO order by tpStr;");
							ddlNumeroTipo.Items.Insert(0, new ListItem("---seleccione---",""));
						
						}
					}
				}
			}
			else
			{ddlNumeroTipo.Items.Clear();
				return;
			}
			
		}
		
		private void ddlNumeroTipo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			string tipo=ddlTipoAsociar.SelectedValue;
			string planilla=txtNumeroDocumento.Text;
			string numero=ddlNumeroTipo.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlInfoElemento.Visible=true;
			lblTipoInfo.Text="";
			if(!validarCampo(planilla))return;

			//Cambia la ruta
			if(numero.Length==0)
			{
				pnlInfoElemento.Visible=false;
				return;
			}
			string strInfo="";
			//Cargar informacion del elemento
			if(tipo.Equals("Giros"))
			{
				strInfo=DBFunctions.SingleData(
					"SELECT '<BR>&nbsp;ORIGEN: ' concat maO.mage_nombre concat '<br>' concat "+
					"'&nbsp;DESTINO: ' concat  maD.mage_nombre concat '<br><br>' concat "+
					"'&nbsp;EMISOR: <br>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat mg.mnit_emisor concat '<br>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat ne.MNIT_NOMBRES concat ' ' concat ne.MNIT_APELLIDOS concat '<BR>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Direccion: ' concat mg.mnit_emisor_direccion concat '<BR>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Telefono: ' concat mg.mnit_emisor_telefono concat '<BR>' concat "+
					"'<br>&nbsp;RECEPTOR: <br>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat mg.mnit_destinatario concat '<br>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat nd.MNIT_NOMBRES concat ' ' concat nd.MNIT_APELLIDOS concat '<BR>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Direccion: ' concat mg.mnit_destinatario_direccion concat '<BR>' concat "+
					"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Telefono: ' concat mg.mnit_destinatario_telefono concat '<BR><BR>' concat "+
					"'&nbsp;VALOR: ' concat char(int(mg.valor_giro)) "+
					"from DBXSCHEMA.mgiros mg, DBXSCHEMA.magencia maO, DBXSCHEMA.magencia maD, "+
					"DBXSCHEMA.mnit ne, DBXSCHEMA.mnit nd where mg.num_documento="+numero +" and "+
					"ne.mnit_nit=mg.mnit_emisor and nd.mnit_nit=mg.mnit_destinatario and "+
					"maD.mag_codigo=mg.mag_agencia_destino and maO.mag_codigo=mg.mag_agencia_origen;");
			}
			else
			{
				if(tipo.Equals("Encomiendas"))
				{
					strInfo=DBFunctions.SingleData(
						"SELECT '<BR>&nbsp;ORIGEN: ' concat maO.mage_nombre concat '<br>' concat "+
						"'&nbsp;CIUDAD DESTINO: ' concat  pcD.pciu_nombre concat '<br><br>' concat "+
						"'&nbsp;EMISOR: <br>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat me.mnit_emisor concat '<br>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat ne.MNIT_NOMBRES concat ' ' concat ne.MNIT_APELLIDOS concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Direccion: ' concat me.mnit_emisor_direccion concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Telefono: ' concat me.mnit_emisor_telefono concat '<BR>' concat "+
						"'<br>&nbsp;RECEPTOR: <br>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat me.mnit_destinatario concat '<br>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat nd.MNIT_NOMBRES concat ' ' concat nd.MNIT_APELLIDOS concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Direccion: ' concat me.mnit_destinatario_direccion concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Telefono: ' concat me.mnit_destinatario_telefono concat '<BR><BR>' concat "+
						"'&nbsp;ENCOMIENDA:<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Descripcion: ' concat me.descripcion_contenido concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Unidades: ' concat char(int(me.unidades)) concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Peso: ' concat char(int(me.peso)) concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Volumen: ' concat char(int(me.volumen)) concat '<BR>' concat "+
						"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Valor Declarado: ' concat char(int(me.valor_avaluo)) concat '<BR>' "+
						"from DBXSCHEMA.mencomiendas me, DBXSCHEMA.magencia maO, DBXSCHEMA.pciudad pcD, DBXSCHEMA.mrutas mr, "+
						"DBXSCHEMA.mnit ne, DBXSCHEMA.mnit nd where me.num_documento="+numero +" and "+
						"ne.mnit_nit=me.mnit_emisor and nd.mnit_nit=me.mnit_destinatario and "+
						"mr.mrut_codigo=me.mrut_codigo and pcD.pciu_codigo=mr.pciu_coddes and "+
						"maO.mag_codigo=me.mag_recibe;");
				}
				else
				{
					if(tipo.Equals("Anticipos/servicios"))
					{
						string []prt=numero.Split('-');
						strInfo=DBFunctions.SingleData("SELECT '<BR>&nbsp;PLACA: ' concat mg.MCAT_PLACA concat '<br>' concat "+
							"'&nbsp;CLASE: ' concat  mg.TDOC_CODIGO concat '<br>' concat "+
							"'&nbsp;CONCEPTO: ' concat  tg.nombre concat '<br><br>' concat "+
							"'&nbsp;RECIBIO: <br>' concat "+
							"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat mg.mnit_responsable_recibe concat '<br>' concat "+
							"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' concat nr.MNIT_NOMBRES concat ' ' concat nr.MNIT_APELLIDOS concat '<BR><BR>' concat "+
							"'&nbsp;GASTO:<BR>' concat "+
							"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Descripcion: ' concat mg.descripcion concat '<BR>' concat "+
							"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cantidad Consumo: ' concat char(int(mg.cantidad_consumo)) concat '<BR>' concat "+
							"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Valor Unidad: ' concat char(int(mg.valor_unidad)) concat '<BR>' concat "+
							"'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Valor Total: ' concat char(int(mg.valor_total_autorizado)) concat '<BR>' "+
							"from DBXSCHEMA.mgastos_transportes mg, "+
							"DBXSCHEMA.mnit nr, DBXSCHEMA.TCONCEPTOS_TRANSPORTES tg where mg.num_documento="+prt[0] +" and mg.TDOC_CODIGO='"+prt[1]+"' and "+
							"nr.mnit_nit=mg.mnit_responsable_recibe and "+
							"mg.TCON_CODIGO=tg.TCON_CODIGO;");
					}
				}
			}
			lblTipoInfo.Text=strInfo;
		}

		public bool validarCampo(String str)
		{	int numero;
			try{numero=Convert.ToInt32(str);return true;}
			catch{Response.Write("<script language='javascript'>alert('Número inicial de documento no válido.');</script>");return false;}
		}


		private void btnAsociar_Click(object sender, System.EventArgs e)
		{
			string tipo=ddlTipoAsociar.SelectedValue;
			string planilla=txtNumeroDocumento.Text;
			string numero=ddlNumeroTipo.SelectedValue;
			string placa;
			if(!validarCampo(planilla))return;
			if(!validarCampo(numero))return;
			ArrayList sqlAct=new ArrayList();
			if(tipo.Length==0)return;
			if(tipo.Equals("Giros"))
			{
				sqlAct.Add(
					"UPDATE DBXSCHEMA.mgiros SET mpla_codigo="+planilla+" "+
					"where num_documento="+numero+" and mpla_codigo is null;");
			}
			else
			{
				if(tipo.Equals("Encomiendas"))
				{
					placa=DBFunctions.SingleData("Select mcat_placa from DBXSCHEMA.MVIAJE mv, DBXSCHEMA.mplanillaviaje mp where mp.MRUT_CODIGO=mv.MRUT_CODIGO and mp.MVIAJE_NUMERO=mv.MVIAJE_NUMERO and mp.mpla_codigo="+planilla+";");
					sqlAct.Add(
						"UPDATE DBXSCHEMA.mencomiendas SET mpla_codigo="+planilla+", mcat_placa='"+placa+"' "+
						"where num_documento="+numero+" and mpla_codigo is null;");
				}
				else
				{
					if(tipo.Equals("Anticipos/servicios"))
					{
						string []prt=numero.Split('-');
						sqlAct.Add(
							"UPDATE DBXSCHEMA.mgastos_transportes SET test_codigo='P', mpla_codigo="+planilla+" "+
							"where num_documento="+prt[0]+" and TDOC_CODIGO='"+prt[1]+"' and mpla_codigo is null;");
					}
				}
			}
			string fecha_liquidacion = DBFunctions.SingleData("select coalesce(char(fecha_liquidacion),'') from dbxschema.mplanillaviaje where mpla_codigo="+planilla+";");
			if (fecha_liquidacion.Length !=0)
				sqlAct.Add("CALL DBXSCHEMA.ACTUALIZA_PLANILLA("+planilla+",'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");;
		
			if(!DBFunctions.Transaction(sqlAct)){
				lblError.Text=DBFunctions.exceptions;
				return;
			}
			Response.Write("<script language='javascript'>alert('Se ha relacionado el elemento a la planilla.');</script>");
			Response.Redirect(indexPage+ "?process=Comercial.RelacionarPlanilla" + "&recarga=1");
		}
	}

}
