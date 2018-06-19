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
	///		Descripción breve de AMS_Comercial_PlanillaManual.
	/// </summary>
	public class AMS_Autorizacion_Conceptos : System.Web.UI.UserControl
	{
		#region Controles
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label lblError;

		
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Panel pnlAdicion;
		protected System.Web.UI.WebControls.Button btnAdicionar;
		protected System.Web.UI.WebControls.Button btnBuscar;
		protected System.Web.UI.WebControls.Button btnSalir;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.DropDownList ddlCargo1;
		protected System.Web.UI.WebControls.DropDownList ddlCargo;
		protected System.Web.UI.WebControls.DropDownList ddlConcepto1;
		protected System.Web.UI.WebControls.DropDownList ddlConcepto;
		protected System.Web.UI.WebControls.Button btnSeleccionar;
		
		protected System.Web.UI.WebControls.Panel pnlBuscar;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label lblAgencia;
		protected System.Web.UI.WebControls.Label lblNombreAgencia;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblCargo;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblConcepto;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.TextBox txtValorAutorizado;
		protected System.Web.UI.WebControls.Button BtnModificar;
		protected System.Web.UI.WebControls.Button BtnBorrar;
		protected System.Web.UI.WebControls.Button BtnRegresar;
		protected System.Web.UI.WebControls.Panel pnlActualizar;
		protected System.Web.UI.WebControls.DataGrid dgrAutorizaciones;
		protected System.Web.UI.WebControls.Panel pnlAutorizaciones;
		
		protected System.Web.UI.WebControls.DataGrid dgrAutorizacion;
		protected System.Web.UI.WebControls.TextBox txtValorAutorizacion;
		int codigo_menu = 20000;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			if (!IsPostBack)
			{
				ViewState["Operacion"] ="";
								
				if (DBFunctions.RecordExist("select operacion from DBXSCHEMA.SAUTORIZACIONES_USUARIO where SMEN_CODIGO = "+codigo_menu+" and SUSU_CODIGO =  (SELECT SUSU_CODIGO FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"') and operacion = 'A';"))
					btnAdicionar.Enabled=true;
				else
					btnAdicionar.Enabled=false;
				
				if (DBFunctions.RecordExist("select operacion from DBXSCHEMA.SAUTORIZACIONES_USUARIO where SMEN_CODIGO = "+codigo_menu+" and SUSU_CODIGO =  (SELECT SUSU_CODIGO FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"') and operacion = 'B';"))
					btnBuscar.Enabled=true;
				else
					btnBuscar.Enabled=false;
				if(Request.QueryString["Regresar"]== "A")
				{
                    Utils.MostrarAlerta(Response, "las autorizaciones fueron adicionadas.");
				}

				if (Request.QueryString["Regresar"]==null)
				{
					//Agencias
					//SeleccionarConsulta();
					
				}
				else
				
					if (Request.QueryString["Regresar"]== "1")
				{
					ViewState["PAGINA"]= Request.QueryString["pagina"];
					ViewState["QUERY"] = Request.QueryString["sql"];
					SeleccionarConsulta();
					MostarPagina();
					pnlAutorizaciones.Visible=true;
				}	
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
			this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
			this.btnBuscar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion Código generado por el Diseñador de Web Forms
		
				
		//Cambia la ruta principal
		private void ddlAgencia1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			//Tabla con agencias
			
		}
		

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			pnlBuscar.Visible=true;
			ViewState["Operacion"]= "B";
			//Agencias
			
			Agencias.TraerAgencias(ddlAgencia1);
			ddlAgencia.Items.Insert(0,new ListItem("Todas","0"));
			ddlAgencia.DataBind();
			//Cargo
			DataRow drCargo;
			DataSet dsCargos=new DataSet();
			DBFunctions.Request(dsCargos,IncludeSchema.NO,
				"Select PCAR_CODIGO  AS VALOR,PCAR_DESCRIPCION AS TEXTO from DBXSCHEMA.PCARGOS_TRANSPORTES ORDER BY PCAR_DESCRIPCION ;");
			drCargo=dsCargos.Tables[0].NewRow();
			drCargo[0]=0;
			drCargo[1]="Todos";
			dsCargos.Tables[0].Rows.InsertAt(drCargo,0);
			ddlCargo1.DataSource=dsCargos.Tables[0];
			ddlCargo1.DataTextField="texto";
			ddlCargo1.DataValueField="valor";
			ddlCargo1.DataBind();
			//Conceptos
			DataRow drConcepto;
			DataSet dsConceptos=new DataSet();
			DBFunctions.Request(dsConceptos,IncludeSchema.NO,
				"Select TCON_CODIGO AS VALOR,NOMBRE AS TEXTO from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TDOC_CODIGO = 'ANT' ORDER BY NOMBRE;");
			drConcepto=dsConceptos.Tables[0].NewRow();
			drConcepto[0]=0;
			drConcepto[1]="Todos";
			dsConceptos.Tables[0].Rows.InsertAt(drConcepto,0);
			ddlConcepto1.DataSource=dsConceptos.Tables[0];
			ddlConcepto1.DataTextField="texto";
			ddlConcepto1.DataValueField="valor";
			ddlConcepto1.DataBind();
			pnlBuscar.Visible=true;
		}
		private void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			BuscarRegistros();
		}
		private void BuscarRegistros()
		{
			SeleccionarConsulta();
			//Validar
			
			int agencia=Int16.Parse(ddlAgencia.SelectedValue);
			int concepto=Int16.Parse(ddlConcepto.SelectedValue);
			int cargo=Int16.Parse(ddlCargo.SelectedValue);
						
			string filtro="";
			//Consulta
			string sqlC="Select A.MAG_CODIGO,mage_nombre,CTA.PCAR_CODIGO,PCAR_DESCRIPCION,CT.TCON_CODIGO,CT.NOMBRE AS NOMBRE_CONCEPTO,VALOR_MAXIMO_AUTORIZACION,FECHA_REPORTE "+
				" from DBXSCHEMA.PCARGOS_TRANSPORTES CTA,DBXSCHEMA.MAGENCIA A,DBXSCHEMA.TCONCEPTOS_TRANSPORTES ct,DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE ACT "+
				" where A.MAG_CODIGO = ACT.MAG_CODIGO and CTA.PCAR_CODIGO  = ACT.PCAR_CODIGO and CT.TCON_CODIGO  =  ACT.TCON_CODIGO ";
							
			if(!ddlConcepto.SelectedValue.Equals("0"))filtro+=" AND ACT.TCON_CODIGO ="+ddlConcepto.SelectedValue+" ";
			if(!ddlAgencia.SelectedValue.Equals("0"))filtro+=" AND ACT.MAG_CODIGO="+ddlAgencia.SelectedValue+" ";
			if(!ddlCargo.SelectedValue.Equals("0"))filtro+=" AND ACT.PCAR_CODIGO ="+ddlCargo.SelectedValue+" ";
			
						
			sqlC+=filtro;
			
			sqlC+=" ORDER BY A.MAG_CODIGO, ACT.PCAR_CODIGO,ACT.TCON_CODIGO ;";
			ViewState["QUERY"]=sqlC;
			ViewState["PAGINA"] = 0;
			//Consulta
			dgrAutorizaciones.CurrentPageIndex=0;
			DataSet dsAutorizaciones=new DataSet();
			DBFunctions.Request(dsAutorizaciones, IncludeSchema.NO,sqlC);
			dgrAutorizaciones.DataSource=dsAutorizaciones.Tables[0];
			dgrAutorizaciones.DataBind();
			pnlAutorizaciones.Visible=true;
			
		}
		private void dgrAutorizaciones_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			ViewState["PAGINA"]= e.NewPageIndex;
			MostarPagina();
		}
		// public int PageCount() { int UltimaPagina = PageCount; }
		private void MostarPagina()
		{
			dgrAutorizaciones.CurrentPageIndex=  Convert.ToInt16(ViewState["PAGINA"]);
			DataSet dsAutorizaciones=new DataSet();
			DBFunctions.Request(dsAutorizaciones, IncludeSchema.NO,ViewState["QUERY"].ToString());
			dgrAutorizaciones.DataSource=dsAutorizaciones.Tables[0];
			dgrAutorizaciones.DataBind();
		}

				
		protected void dgActualizarAutorizacion(object sender, DataGridCommandEventArgs e) 
		{
			//string campos=ViewState["CAMPOS"].ToString();
			int ind=e.Item.DataSetIndex;

			
			if ((e.CommandName == "Actualizar") || (e.CommandName == "Borrar") || (e.CommandName == "Consultar"))
			{
				DataSet dsAutorizaciones=new DataSet();
				DBFunctions.Request(dsAutorizaciones, IncludeSchema.NO,ViewState["QUERY"].ToString());			  		
				//	int documento =Convert.ToInt32(dsAnticipos.Tables[0].Rows[ind]["NUM_DOCUMENTO"]);
				//	string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
				
				
				//if(Request.QueryString["processTitle"] != null)
				//	processTitle = Request.QueryString["processTitle"];
				//Response.Redirect(indexPage+ "?process=Comercial.IngresosGastos" + "&path="+Request.QueryString["path"]+ "&comando=" +e.CommandName + "&Documento=" +documento + "&sql="+ViewState["QUERY"] + "&pagina="+ViewState["PAGINA"]);
			}
			
		}
		
		#region AdicionarAutorizaciones
		protected void btnAdicionar_Click(Object sender, EventArgs e)
		{
			ViewState["Operacion"]= "A";
			DataSet dsAgencias=new DataSet();
			DBFunctions.Request(dsAgencias,IncludeSchema.NO,
				"select rtrim(char(mag_codigo)) as valor, mage_nombre as texto from DBXSCHEMA.magencia;");
			DataRow drDA=dsAgencias.Tables[0].NewRow();
			drDA[0]="";
			drDA[1]="---seleccione---";
			dsAgencias.Tables[0].Rows.InsertAt(drDA,0);
			pnlAdicion.Visible=true;
			
		}
		private void btnAdicionar_autorizacion_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlUpd=new ArrayList();
			ArrayList arrAutorizaziones=new ArrayList();
			
			string errores="";
			int fila=1;
			foreach(DataGridItem dgrI in dgrAutorizacion.Items)
			{
				bool errLin=false;
				int linea=1;
				TextBox txtNumPago=(TextBox)dgrI.FindControl("txtNumPago");
				DropDownList ddlConcepto=(DropDownList)dgrI.FindControl("ddlConcepto");
				DropDownList ddlCargo=(DropDownList)dgrI.FindControl("ddlCcargo");
				TextBox txtValorAutorizacion=(TextBox)dgrI.FindControl("txtValorAutorizacion");
				
				if(txtValorAutorizacion.Text.Trim().Length>0 || ddlConcepto.SelectedValue.Length>0 || ddlCargo.SelectedValue.Length>0)
				{
					
					string concepto=ddlConcepto.SelectedValue;
					string cargo=ddlCargo.SelectedValue;
					double valor=0;
					//Concepto
					if(concepto.Length==0)
					{
						errores+="Concepto de la Autorizacion no válido en la línea "+fila+". ";
						errLin=true;}
					//Concepto
					if(cargo.Length==0)
					{
						errores+="Cargo de la Autorizacion no válido en la línea "+fila+". ";
						errLin=true;}
					//Repetido?
					string llave = concepto+cargo;
					if(!errLin && arrAutorizaziones.Contains(llave))
					{
						errores+="la Autorizacion ya existe en la línea "+fila+" ya ha sido ingresado. ";errLin=true;}
					else
					{
						arrAutorizaziones.Add(llave);
					}
					
					//Precio
					try
					{
						valor=double.Parse(txtValorAutorizado.Text.Replace(",","").Trim());
						if(valor<=0)throw(new Exception());}
					catch
					{
						errores+="Valor de laaurorizacion no es válido en la línea "+fila+". ";
						errLin=true;
					}

					//Verificar repetido
					if(!DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE WHERE MAG_CODIGO = "+ddlAgencia.SelectedValue+" AND PCAR_CODIGO = "+ddlCargo.SelectedValue+",TCON_CODIGO = "+ddlConcepto.SelectedValue+";"))
					{
						errores+="Ya existe la Autorizacion de la línea "+fila+". ";
						errLin=true;
					}
					if(!errLin)
					{
						
						//Insertar Autorizacion
						sqlUpd.Add("INSERT INTO DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE  values("+ddlAgencia.SelectedValue+","+ddlCargo.SelectedValue+","+ddlConcepto.SelectedValue+","+valor+",'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
						
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
						linea++;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";
					}
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}
		
		
			if(errores.Length==0)
			{
				
				string Comando = "Adicionar";
				
				if(!DBFunctions.Transaction(sqlUpd))
					lblError.Text=DBFunctions.exceptions;
			}
			else 
            Utils.MostrarAlerta(Response, "las autorizaciones fueron adicionadas.'" + errores + "'");
				//strActScript+="alert('"+errores+"');";
			
		}	

		#endregion AdicionarAutorizaciones

		private void MostarGrillaAdicion()
		{
			//Adicion Autorizaciones
			DataTable dtAutorizacion=new DataTable();
			dtAutorizacion.Columns.Add("AGENCIA",typeof(int));
			
			for(int n=1;n<=10;n++)
			{
				DataRow drT=dtAutorizacion.NewRow();
				drT[0]=n;
				dtAutorizacion.Rows.Add(drT);
			}
			dgrAutorizacion.DataSource=dtAutorizacion;
			dgrAutorizacion.DataBind();
		
		}
		private  void SeleccionarConsulta()
		{
		}
	}
		
}
