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
	using Ajax;
	using AMS.Contabilidad;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
    using AMS.Tools;
	

	/// <summary>
	///		Descripción breve de AMS_Comercial_InterfaceContablePlanillas.
	/// </summary>
	public class AMS_Comercial_InterfaceContablePlanillas : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList ddlAno;
		protected System.Web.UI.WebControls.DropDownList ddlMes;
		protected System.Web.UI.WebControls.DropDownList ddlDiaInicial;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.Button CmdContabilizar;
		protected System.Web.UI.WebControls.DropDownList ddlDiaFinal;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected DataTable dtCabeceras;
		protected DataTable dtDetalles;
		protected System.Web.UI.WebControls.DataGrid dgMovs;
		protected DataTable dtMovs;
		protected DataSet dsInterface;
		protected double valorDebito=0,valorCredito=0;
		protected System.Web.UI.WebControls.Button btnCancelar;
		protected System.Web.UI.WebControls.LinkButton lnkExportarExcel;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdSvrErrores;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdnCont;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(ddlAno.Items.Count==0)Cargar();
			if(!IsPostBack){
				ViewState["PROCESO"]=Request.QueryString["prc"];
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  Se ha realizado el proceso.");
			}
			else{
				if(Session["dtCabeceras"]!=null)
					dtCabeceras=(DataTable)Session["dtCabeceras"];
				if(Session["dtDetalles"]!=null)
					dtDetalles=(DataTable)Session["dtDetalles"];
			}
		}
		private void Cargar()
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlAno,"SELECT pano_ano ,pano_detalle FROM pano");
			bind.PutDatasIntoDropDownList(ddlMes,"SELECT pmes_mes ,pmes_nombre FROM pmes where pmes_mes > 0 and pmes_mes <13");
			bind.PutDatasIntoDropDownList(ddlDiaInicial,"SELECT tdia_dia,tdia_nombre FROM tdia");
			bind.PutDatasIntoDropDownList(ddlDiaFinal,"SELECT tdia_dia,tdia_nombre FROM tdia");
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
			this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.CmdContabilizar.Click += new System.EventHandler(this.CmdContabilizar_Click);
			this.lnkExportarExcel.Click += new System.EventHandler(this.lnkExportarExcel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		protected void ConstruirGrilla()
		{
			DatasToControls.Aplicar_Formato_Grilla(dgMovs);
			Estructura_DtMovs();
			dtCabeceras=dsInterface.Tables[0];
			dtDetalles=dsInterface.Tables[1];

			for(int i=0;i<dtCabeceras.Rows.Count;i++)
			{
				DataRow[] hijos=dtDetalles.Select("PREFIJO='"+dtCabeceras.Rows[i][0].ToString()+"' AND NUMERO='"+dtCabeceras.Rows[i][1].ToString()+"'");
				valorCredito=valorDebito=0;
				if(hijos.Length!=0)
				{
					ConstruirHeader(dtCabeceras.Rows[i][0].ToString(),dtCabeceras.Rows[i][1].ToString(),dtCabeceras.Rows[i][2].ToString(),dtCabeceras.Rows[i][3].ToString(),dtCabeceras.Rows[i][4].ToString(),dtCabeceras.Rows[i][5].ToString(),dtCabeceras.Rows[i][6].ToString(),dtCabeceras.Rows[i][7].ToString(),dtCabeceras.Rows[i][8].ToString());
					for(int j=0;j<hijos.Length;j++)
					{
						if(!hijos[j][9].ToString().Equals("0"))//Debito
						{
							try
							{
								valorDebito=valorDebito+Math.Round(Convert.ToDouble(hijos[j][9].ToString().Substring(1)),2);
							}
							catch{valorDebito=valorDebito+0;}
						}
						if(!hijos[j][10].ToString().Equals("0"))//Credito
						{
							try
							{
								valorCredito=valorCredito+Math.Round(Convert.ToDouble(hijos[j][10].ToString().Substring(1)),2);
							}
							catch{valorCredito=valorCredito+0;}
						}
						ConstruirItems(hijos[j][2].ToString(),hijos[j][3].ToString(),hijos[j][4].ToString(),hijos[j][5].ToString(),hijos[j][6].ToString(),hijos[j][7].ToString(),hijos[j][8].ToString(),hijos[j][9].ToString(),hijos[j][10].ToString(),hijos[j][11].ToString(),dtCabeceras.Rows[i][0].ToString(),dtCabeceras.Rows[i][1].ToString());
					}
					ConstruirTotalComprobante();
				}
			}
			dgMovs.DataSource=dtMovs;
			dgMovs.DataBind();
			btnCancelar.Visible=CmdContabilizar.Visible = lnkExportarExcel.Visible = (dtMovs.Rows.Count>0);
			Session["dtCabeceras"]=dtCabeceras;
			Session["dtDetalles"]=dtDetalles;
		}

		protected void ConstruirItems(string cuenta,string prefijoRef,string numeroRef,string nit,string almacen,string centroCosto,string razon,string debito,string credito,string valorBase, string prefijoPadre, string numeroPadre)
		{
			if(cuenta == null || cuenta == String.Empty)
				hdSvrErrores.Value += "Por favor revise el comprobante de "+prefijoPadre+"-"+numeroPadre+" algun renglon no tiene cuenta\\n";
			if(almacen == null || almacen == String.Empty)
				hdSvrErrores.Value += "Por favor revise el comprobante de "+prefijoPadre+"-"+numeroPadre+" algun renglon no tiene almacen\\n";
			if(centroCosto == null || centroCosto == String.Empty)
				hdSvrErrores.Value += "Por favor revise el comprobante de "+prefijoPadre+"-"+numeroPadre+" algun renglon no tiene centro de costo\\n";
			DataRow fila;
			fila=dtMovs.NewRow();
			fila[0]=VerificarErrorConsulta(cuenta.Replace("'",""));
			fila[1]=VerificarErrorConsulta(prefijoRef)+"-"+VerificarErrorConsulta(numeroRef);
			fila[2]=VerificarErrorConsulta(nit);
			fila[3]=VerificarErrorConsulta(almacen.Replace("'",""));
			fila[4]=VerificarErrorConsulta(centroCosto.Replace("'",""));
			fila[5]=VerificarErrorConsulta(razon.Replace("'",""));
			fila[6]="<p style=\"TEXT-ALIGN: right\">"+debito+"</p>";
			fila[7]="<p style=\"TEXT-ALIGN: right\">"+credito+"</p>";
			fila[8]="<p style=\"TEXT-ALIGN: right\">"+valorBase+"</p>";
			dtMovs.Rows.Add(fila);
		}

		private string VerificarErrorConsulta(string valor)
		{
			string nuevoValor="";
			if(valor.IndexOf("Error")!=-1)
				nuevoValor="<p style=\"COLOR: red\">"+valor+"</p>";
			else
				nuevoValor=valor;
			return nuevoValor;
		}

		protected void ConstruirTotalComprobante()
		{
			DataRow fila,filaIntermedio;
			fila=dtMovs.NewRow();
			filaIntermedio=dtMovs.NewRow();
			fila[0]="<p style=\"COLOR: navy\">Total Comprobante</p>";
			if(Math.Round(valorDebito,2) != Math.Round(valorCredito,2))
				fila[5]="<p style=\"COLOR: red\">Comprobante Descuadrado</p>";
			else
			{
				fila[5]="<p style=\"COLOR: green\">Sumas Iguales</p>";
				hdnCont.Value="S";
			}
			fila[6]="<p style=\"TEXT-ALIGN: right\">"+Math.Round(valorDebito,2).ToString("C")+"</p>";
			fila[7]="<p style=\"TEXT-ALIGN: right\">"+Math.Round(valorCredito,2).ToString("C")+"</p>";
			dtMovs.Rows.Add(fila);
			dtMovs.Rows.Add(filaIntermedio);
		}

		protected void ConstruirHeader(string prefijo,string numero,string anio,string mes,string fecha,string razon,string procesado,string usuario,string valor)
		{
			DataRow fila,filaTitulos;
			fila=dtMovs.NewRow();
			filaTitulos=dtMovs.NewRow();
			fila[0]=prefijo;
			fila[1]=numero;
			fila[2]=anio;
			fila[3]=mes;
			fila[4]=fecha;
			fila[5]=razon;
			fila[6]=procesado;
			fila[7]=usuario;
			fila[8]=valor;
			filaTitulos[0]="<p style=\"COLOR: navy\">Cuenta</p>";
			filaTitulos[1]="<p style=\"COLOR: navy\">Referencia Pref-Num</p>";
			filaTitulos[2]="<p style=\"COLOR: navy\">Nit</p>";
			filaTitulos[3]="<p style=\"COLOR: navy\">Almacen</p>";
			filaTitulos[4]="<p style=\"COLOR: navy\">Centro Costo</p>";
			filaTitulos[5]="<p style=\"COLOR: navy\">Razón</p>";
			filaTitulos[6]="<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Débito</p>";
			filaTitulos[7]="<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Crédito</p>";
			filaTitulos[8]="<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Base</p>";
			dtMovs.Rows.Add(fila);
			dtMovs.Rows.Add(filaTitulos);
		}

		protected void Estructura_DtMovs()
		{
			dtMovs=new DataTable();
			dtMovs.Columns.Add("PREFIJO",typeof(string));//0
			dtMovs.Columns.Add("NUMERO",typeof(string));//1
			dtMovs.Columns.Add("AÑO",typeof(string));//2
			dtMovs.Columns.Add("MES",typeof(string));//3
			dtMovs.Columns.Add("FECHA DOC",typeof(string));//4
			dtMovs.Columns.Add("RAZON",typeof(string));//5
			dtMovs.Columns.Add("PROCESADO",typeof(string));//6
			dtMovs.Columns.Add("USUARIO",typeof(string));//7
			dtMovs.Columns.Add("VALOR",typeof(string));//8
		}
		
		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			int diaI,diaF,ano,mes,proceso;
			string almacen;
			//Proceso
			try{
				proceso=int.Parse(DBFunctions.SingleData("SELECT TPROC_CODIGO FROM DBXSCHEMA.TPROCESOS_CONTABLES_TRANSPORTES WHERE MODULO='"+Request.QueryString["prc"]+"';"));}
			catch{
                Utils.MostrarAlerta(Response, "  Proceso no valido");
				return;}
			//ANO
			try{
				ano=int.Parse(ddlAno.SelectedValue);}
			catch{
                Utils.MostrarAlerta(Response, "  Año no valido");
				return;}
			//MES
			try{
				mes=int.Parse(ddlMes.SelectedValue);}
			catch{
                Utils.MostrarAlerta(Response, "  Mes no valido");
				return;}
			//DIAS
			try{
				diaI=int.Parse(ddlDiaInicial.SelectedValue);
				diaF=int.Parse(ddlDiaFinal.SelectedValue);}
			catch
			{
                Utils.MostrarAlerta(Response, "  Dia inicial o final no valido");
				return;}
			if(diaI>diaF){
                Utils.MostrarAlerta(Response, "  El dia final es menor al dia inicial");
				return;
			}
			//ALMACEN
			almacen=DBFunctions.SingleData("SELECT PALMACEN_CONTABILIDAD FROM DBXSCHEMA.CTRANSPORTES;");
			if(almacen.Length==0){
                Utils.MostrarAlerta(Response, "  No se ha definido el almacen por defecto");
				return;
			}
			//Estan liquidadas todas las planillas?
			if(DBFunctions.RecordExist("SELECT * FROM DBXSCHEMA.MPLANILLAVIAJE WHERE FECHA_LIQUIDACION IS NULL AND (DAY(FECHA_PLANILLA)<="+diaF+" AND DAY(FECHA_PLANILLA)>="+diaI+" AND MONTH(FECHA_PLANILLA)="+mes+" AND YEAR(FECHA_PLANILLA)="+ano+");")){
                Utils.MostrarAlerta(Response, "  No se han despachado planillas creadas en el rango de fechas especificado");
				return;
			}
			//Conceptos tienen cuentas?
			if(DBFunctions.RecordExist("SELECT * FROM DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TCON_CODIGO NOT IN(SELECT TCON_CODIGO FROM DBXSCHEMA.TIMPUTACION_CONTABLE_TRANSPORTES);"))
			{
                Utils.MostrarAlerta(Response, "  Algunos conceptos de contabilidad no se han asociado a una cuenta");
				return;
			}
			//Consultar interface
			dsInterface=new DataSet();
			//Consultar encabezados[0] y detalles[1]
			DBFunctions.Request(dsInterface,IncludeSchema.NO,"CALL DBXSCHEMA.INTERFACE_PLANILLAS("+proceso+","+ano+","+mes+","+diaI+","+diaF+",'"+almacen+"','"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			ViewState["DIA_DESDE"]=diaI.ToString();
			ViewState["DIA_HASTA"]=diaF.ToString();
			ViewState["MES"]=mes.ToString();
			ViewState["ANO"]=ano.ToString();
			if(dsInterface.Tables[0].Rows.Count==0){
                Utils.MostrarAlerta(Response, "  No se han despachado planillas para el rango de fechas especificado");
				return;
			}
			ddlAno.Enabled=ddlDiaFinal.Enabled=ddlDiaInicial.Enabled=ddlMes.Enabled=false;
			btnConsultar.Visible=false;
			ConstruirGrilla();
		}

		private void btnVolver_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Comercial.InterfaceContable&path="+Request.QueryString["path"]);
		}

		private void CmdContabilizar_Click(object sender, System.EventArgs e)
		{
			InterfaceContable miInterface=new InterfaceContable();
			miInterface.Anio=Convert.ToInt32(ddlAno.SelectedValue);
			miInterface.DiaInicial=Convert.ToInt32(ddlDiaInicial.SelectedValue);
			miInterface.DiaFinal=Convert.ToInt32(ddlDiaFinal.SelectedValue);
			miInterface.Mes=Convert.ToInt32(ddlMes.SelectedValue);
			miInterface.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
			miInterface.Procesado=DateTime.Now.Date;
			miInterface.TablaCabecera=dtCabeceras;
			miInterface.TablaDetalles=dtDetalles;
			if(miInterface.GuardarInterface())
			{
				string sqlCont="UPDATE DBXSCHEMA.MPLANILLAVIAJE SET FECHA_INTERFACE='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE "+
				"FECHA_LIQUIDACION IS NOT NULL AND FECHA_INTERFACE IS NULL AND "+
				"(DAY(FECHA_PLANILLA)<="+int.Parse(ViewState["DIA_HASTA"].ToString())+" AND "+
				"DAY(FECHA_PLANILLA)>="+int.Parse(ViewState["DIA_DESDE"].ToString())+" AND "+
				"MONTH(FECHA_PLANILLA)="+int.Parse(ViewState["MES"].ToString())+" AND "+
				"YEAR(FECHA_PLANILLA)="+int.Parse(ViewState["ANO"].ToString())+");";
				DBFunctions.NonQuery(sqlCont);
				Response.Redirect(indexPage+"?process=Comercial.InterfaceContablePlanillas&path="+Request.QueryString["path"]+"&act=1&prc="+ViewState["PROCESO"]);
			}
			else
				lblError.Text=miInterface.Mensajes;
		}

		private void lnkExportarExcel_Click(object sender, System.EventArgs e)
		{
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename=interfase.xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			System.IO.StringWriter stringWrite = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
			dgMovs.RenderControl(htmlWrite);
			Response.Write(stringWrite.ToString());
			Response.End();
		}
	}
}
