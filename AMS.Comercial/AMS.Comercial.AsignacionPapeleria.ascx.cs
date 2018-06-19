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
	///		Descripción breve de AMS_Comercial_AsignacionPapeleria.
	/// </summary>
	public class AMS_Comercial_AsignacionPapeleria : System.Web.UI.UserControl
	{
		#region Controles
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		public string strActScript="";
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblNumero;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		protected System.Web.UI.WebControls.Button btnRecibir;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		public string strTalonariosScript="";
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				string asignacion=DBFunctions.SingleData("SELECT MASIGNACION_NUMERO+1 FROM DBXSCHEMA.MASIGNACION_PAPELERIA ORDER BY MASIGNACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
				if(asignacion.Length==0)asignacion="1";
				Agencias.TraerAgenciasPrefijoUsuario(ddlAgencia);

				lblNumero.Text=asignacion;
				lblFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				//TIPOS DE DOCUMENTOS
				DataSet dsTiposDoc=new DataSet();
				DBFunctions.Request(dsTiposDoc,IncludeSchema.NO,
					"select tdoc_codigo concat case when prefijo='S' then '|' else '' end as valor,tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS WHERE PAPELERIA='S' ORDER BY TDOC_NOMBRE;");
				DataRow drC=dsTiposDoc.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsTiposDoc.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtTiposDocumentos",dsTiposDoc.Tables[0]);
				MostrarTabla();
				ConsultarCapacidadTalonarios();
				if(Request.QueryString["act"]!=null && Request.QueryString["asg"]!=null){
                    Utils.MostrarAlerta(Response, "La asignación ha sido creada con el número de asignación " + Request.QueryString["asg"]);
                    Response.Write("<script language:javascript>w=window.open('../aspx/AMS.Comercial.AsignacionPapeleria.aspx?asg=" + Request.QueryString["asg"] + "','ASIGPAPELERIA" + Request.QueryString["asg"] + "','width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no');</script>");
                    
                    //Utils.MostrarAlerta(Response, "La asignación ha sido creada con el número de asignación " + Request.QueryString["asg"] + ".');" +
                    //    "window.open('../aspx/AMS.Comercial.AsignacionPapeleria.aspx?asg=" + Request.QueryString["asg"] + "','ASIGPAPELERIA" + Request.QueryString["asg"] + "',\"width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no\"");
				}
			}
		}
		private void ConsultarCapacidadTalonarios()
		{
			DataSet dsTalonarios=new DataSet();
			DBFunctions.Request(dsTalonarios,IncludeSchema.NO,"SELECT TDOC_CODIGO, CANTIDAD_TIQUETERA FROM DBXSCHEMA.TDOCU_TRANS;");
			string tdocs="var arrTdocs = new Array(";
			string talonarios="var arrTalonarios = new Array(";
			for(int n=0;n<dsTalonarios.Tables[0].Rows.Count;n++)
			{
				tdocs+="'"+dsTalonarios.Tables[0].Rows[n]["TDOC_CODIGO"]+"',";
				talonarios+=dsTalonarios.Tables[0].Rows[n]["CANTIDAD_TIQUETERA"]+",";
			}
			if(tdocs.EndsWith(","))tdocs=tdocs.Substring(0,tdocs.Length-1);
			if(talonarios.EndsWith(","))talonarios=talonarios.Substring(0,talonarios.Length-1);
			tdocs+=");";
			talonarios+=");";
			strTalonariosScript=tdocs+talonarios;
			ViewState.Add("CAPACIDADTALONARIOS",strTalonariosScript);
		}

		//Mostrar grilla
		private void MostrarTabla()
		{
			DataTable dtDocumentos=new DataTable();
			//Tabla tiquetes normales
			dtDocumentos.Columns.Add("NUMERO",typeof(int));
			for(int n=1;n<=10;n++)
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
			this.dgrDocumentos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrDocumentos_ItemDataBound);
			this.btnRecibir.Click += new System.EventHandler(this.btnRecibir_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnRecibir_Click(object sender, System.EventArgs e)
		{
			//Agencia
			if(ddlAgencia.Items.Count==0){
                Utils.MostrarAlerta(Response, "El usuario actual no tiene agencias asignadas.");

				return;
			}
			string agencia=ddlAgencia.SelectedValue.Replace("|","").Trim();
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado");
				return;
			}
			string despachador=txtResponsable.Text.Trim();
			//Usuario
			if(!DBFunctions.RecordExist("SELECT MNIT.MNIT_NIT from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES PC  WHERE MP.MAG_CODIGO="+agencia+" AND MP.MNIT_NIT=MNIT.MNIT_NIT AND MNIT.MNIT_NIT='"+despachador+"' AND PC.PCAR_CODIGO=MP.PCAR_CODIGO AND PC.PCAR_FILTRO='D';"))
			{
                Utils.MostrarAlerta(Response, "El usuario al que asigna la papeleria no pertenece a la agencia.");
				return;
			}
			string asignacion=DBFunctions.SingleData("SELECT MASIGNACION_NUMERO+1 FROM DBXSCHEMA.MASIGNACION_PAPELERIA ORDER BY MASIGNACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(asignacion.Length==0)asignacion="1";
			
            //Validar
			#region Validar Documentos
			ArrayList arrInicios=new ArrayList();
			ArrayList arrFines=new ArrayList();
			ArrayList arrTiposDocs=new ArrayList();
			ArrayList arrFilas=new ArrayList();
			ArrayList sqlUpd=new ArrayList();
			DataSet dsLimitesTipos=new DataSet();
			DBFunctions.Request(dsLimitesTipos,IncludeSchema.NO,"SELECT TDOC_CODIGO, MAX_TALONARIO FROM DBXSCHEMA.TDOCU_TRANS;");
			int linea=1,fila=1;
			bool info=false;
			string errores="";
			foreach(DataGridItem dgrI in dgrDocumentos.Items)
			{
				bool errLin=false;
				DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
				TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
				TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
				TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
				if(ddlTipoDocumento.SelectedValue.Length>0 || txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0)
				{
					int talonarios=0;
					long inicio=0, fin=0;
					string tipo=ddlTipoDocumento.SelectedValue.Trim().Replace("|","");
					string clase="M";
					int prefijo=0;
					if(ddlTipoDocumento.SelectedValue.EndsWith("|") && ddlAgencia.SelectedValue.EndsWith("|"))
						prefijo=Convert.ToInt16(agencia);
					info=true;
					//Tipo
					if(tipo.Length==0)
					{
						errores+="Debe seleccionar la clase de documento en la línea "+ fila+". ";errLin=true;}
					//Tipo
					if(clase.Length==0)
					{
						errores+="Debe seleccionar el tipo en la línea "+ fila+". ";errLin=true;}
					//Talonarios
					try
					{
						talonarios=int.Parse(txtTalonarios.Text.Trim());}
					catch
					{
						errores+="Cantidad de talonarios no válido en la línea "+fila+". ";errLin=true;}
					//Inicio
					try
					{
						inicio=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtInicioDocumento.Text.Trim());
						if(inicio<0)throw(new Exception());}
					catch
					{
						errores+="Número inicial no válido en la línea "+fila+". ";errLin=true;}
					//Fin
					try
					{
						fin=int.Parse(txtFinDocumento.Text.Trim());
						if(ddlTipoDocumento.SelectedValue.EndsWith("|") && fin>=Math.Pow(10, Comercial.Tiquetes.lenTiquete))throw(new Exception());
						fin=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+fin;
						if(fin<0)throw(new Exception());}
					catch
					{
						errores+="Número final no válido en la línea "+fila+". ";errLin=true;}
					//Inicio>Final?
					if(inicio>fin)
					{
						errores+="Número inicial mayor a número final en la línea "+fila+". ";errLin=true;}
					//Rangos cruzados?
					for(int n=0;n<linea-1;n++)
					{
						long iniA=(long)arrInicios[n];
						long finA=(long)arrFines[n];
						if(tipo.Equals(arrTiposDocs[n]) && ((inicio<=finA && inicio>=iniA) || (fin<=finA && fin>=iniA) || (fin>=finA && inicio<=iniA)))
						{
							errores+="El rango de la línea "+fila+" se cruza con el rango de la fila "+arrFilas[n].ToString()+". ";errLin=true;}
					}
					arrInicios.Add(inicio);
					arrFines.Add(fin);
					arrTiposDocs.Add(tipo);
					arrFilas.Add(fila);
					//existen  todos en la BD, con fecha despacho nulo?
					long total=fin-inicio+1;
					long totalDB=Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='"+tipo+"' AND TIPO_DOCUMENTO='"+clase+"' AND MAG_CODIGO="+agencia+" AND NUM_DOCUMENTO BETWEEN "+inicio+" AND "+fin+" AND NUM_DESPACHO IS NOT NULL AND FECHA_DESPACHO IS NOT NULL AND NUM_ASIGNACION IS NULL AND FECHA_ASIGNACION IS NULL AND MNIT_RESPONSABLE IS NULL AND MPLA_CODIGO IS NULL AND NUM_ANULACION IS NULL AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL;"));
					if(total>totalDB)
					{
						errores+="No se han despachado a la agencia todos los documentos especificados en el rango de la línea "+fila+", no coinciden todos sus tipos o ya han sido asignados. ";errLin=true;}
					//Maximo permitido
					int maxL=0;
					if(tipo.Length>0){
						maxL=Convert.ToInt32(dsLimitesTipos.Tables[0].Select("tdoc_codigo='"+tipo+"'")[0]["MAX_TALONARIO"]);
						if(total>(maxL*talonarios))
						{
							errores+="Se superó el número máximo de documentos permitidos por talonario en la línea "+fila+". ";errLin=true;}
					}
					//Guardar
					if(!errLin)
					{
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

			#region Guardar Documentos
			linea=1;
			if(info && errores.Length==0)
				foreach(DataGridItem dgrI in dgrDocumentos.Items)
				{
					DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
					TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
					TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
					TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
					if( ddlTipoDocumento.SelectedValue.Length>0 || txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0)
					{
						int talonarios=0;
						long inicio=0, fin=0;
						string tipo=ddlTipoDocumento.SelectedValue.Trim().Replace("|","");
						string clase="M";
						int prefijo=0;
						if(ddlTipoDocumento.SelectedValue.EndsWith("|") && ddlAgencia.SelectedValue.EndsWith("|"))
							prefijo=Convert.ToInt16(agencia);
						info=true;
						talonarios=int.Parse(txtTalonarios.Text.Trim());
						inicio=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtInicioDocumento.Text.Trim());
						fin=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtFinDocumento.Text.Trim());
						/*for(long nD=inicio;nD<=fin;nD++)
							DBFunctions.NonQuery("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET MNIT_RESPONSABLE='"+despachador+"', NUM_ASIGNACION="+asignacion+", FECHA_ASIGNACION='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE TDOC_CODIGO='"+tipo+"' AND NUM_DOCUMENTO="+nD+";");*/
						DBFunctions.NonQuery("call dbxschema.ASIGNAR_PAPEL('"+tipo+"',"+inicio+","+fin+",'M',"+asignacion+",'"+despachador+"');");
						DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MASIGNACION_PAPELERIA VALUES("+asignacion+","+linea+","+agencia+",'"+tipo+"','"+clase+"',"+talonarios+","+inicio+","+fin+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+despachador+"');");
						linea++;
					}
				}
			#endregion

			if(!info)errores+="No ingresó información.";
			if(errores.Length==0)
				Response.Redirect(indexPage+"?process=Comercial.AsignacionPapeleria&act=1&path="+Request.QueryString["path"]+"&asg="+asignacion);
			else strActScript+="alert('"+errores+"');";
		}

		private void dgrDocumentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlTipo=(DropDownList)e.Item.FindControl("ddlTipoDocumento");
				TextBox txtTalonarios=(TextBox)e.Item.FindControl("txtTalonarios");
				TextBox txtInicio=(TextBox)e.Item.FindControl("txtInicioDocumento");
				TextBox txtFin=(TextBox)e.Item.FindControl("txtFinDocumento");
				string objsScr="'"+dlTipo.ClientID+"','"+txtTalonarios.ClientID+"','"+txtInicio.ClientID+"','"+txtFin.ClientID+"'";
				dlTipo.DataSource=ViewState["dtTiposDocumentos"];
				dlTipo.DataTextField="texto";
				dlTipo.DataValueField="valor";
				dlTipo.DataBind();
				txtTalonarios.Attributes.Add("onkeyup","Total("+objsScr+");");
				txtInicio.Attributes.Add("onkeyup","Total("+objsScr+");");
				dlTipo.Attributes.Add("onchange","Total("+objsScr+");");
			}
		}
	}
}
