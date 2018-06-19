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
	///		Descripción breve de AMS_Comercial_DespachoPapeleria.
	/// </summary>
	public class AMS_Comercial_DespachoPapeleria : System.Web.UI.UserControl
	{
		#region Controles
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		public string strActScript="";
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblNumero;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		protected System.Web.UI.WebControls.Button btnRecibir;
		protected System.Web.UI.WebControls.Label lblError;
		public string strTalonariosScript="";
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				string despacho=DBFunctions.SingleData("SELECT MDESPACHO_NUMERO+1 FROM DBXSCHEMA.MDESPACHO_PAPELERIA ORDER BY MDESPACHO_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
				if(despacho.Length==0)despacho="1";
				lblNumero.Text=despacho;
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
				//AGENCIAS	
				DataSet dsAgencias=new DataSet();
				DBFunctions.Request(dsAgencias,IncludeSchema.NO,
					"select rtrim(char(mag_codigo)) concat case when prefijo='S' then '|' else '' end as valor,mage_nombre as texto from DBXSCHEMA.MAGENCIA ORDER BY mage_nombre;");
				drC=dsAgencias.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsAgencias.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtAgencias",dsAgencias.Tables[0]);
				MostrarTabla();
				ConsultarAgenciasCapacidadTalonarios();
				if(Request.QueryString["act"]!=null && Request.QueryString["dsp"]!=null)
                Utils.MostrarAlerta(Response, "La papelería ha sido despachada con el número de despacho " + Request.QueryString["dsp"] + ".");
			}
		}
		//Consulta capacidad talonarios y agencias preasignadas para los javascripts
		private void ConsultarAgenciasCapacidadTalonarios()
		{
			DataSet dsTalonarios=new DataSet();
			DBFunctions.Request(dsTalonarios,IncludeSchema.NO,"SELECT TDOC_CODIGO, CANTIDAD_TIQUETERA,coalesce(MAG_PRESIGNADA,0) AS MAG_PRESIGNADA FROM DBXSCHEMA.TDOCU_TRANS;");
			string tdocs="var arrTdocs = new Array(";
			string talonarios="var arrTalonarios = new Array(";
			string agenciasPre="var arrAgenciasPre = new Array(";
			for(int n=0;n<dsTalonarios.Tables[0].Rows.Count;n++)
			{
				tdocs+="'"+dsTalonarios.Tables[0].Rows[n]["TDOC_CODIGO"]+"',";
				talonarios+=dsTalonarios.Tables[0].Rows[n]["CANTIDAD_TIQUETERA"]+",";
				agenciasPre+=dsTalonarios.Tables[0].Rows[n]["MAG_PRESIGNADA"]+",";
			}
			if(tdocs.EndsWith(","))tdocs=tdocs.Substring(0,tdocs.Length-1);
			if(talonarios.EndsWith(","))talonarios=talonarios.Substring(0,talonarios.Length-1);
			if(agenciasPre.EndsWith(","))agenciasPre=agenciasPre.Substring(0,agenciasPre.Length-1);
			tdocs+=");";
			talonarios+=");";
			agenciasPre+=");";
			strTalonariosScript=tdocs+talonarios+agenciasPre;
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

		private void dgrDocumentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlTipo=(DropDownList)e.Item.FindControl("ddlTipoDocumento");
				DropDownList dlAgencia=(DropDownList)e.Item.FindControl("ddlAgencia");
				TextBox txtTalonarios=(TextBox)e.Item.FindControl("txtTalonarios");
				TextBox txtInicio=(TextBox)e.Item.FindControl("txtInicioDocumento");
				TextBox txtFin=(TextBox)e.Item.FindControl("txtFinDocumento");
				string objsScr="'"+dlTipo.ClientID+"','"+txtTalonarios.ClientID+"','"+txtInicio.ClientID+"','"+txtFin.ClientID+"','"+dlAgencia.ClientID+"'";
				
				dlTipo.DataSource=ViewState["dtTiposDocumentos"];
				dlTipo.DataTextField="texto";
				dlTipo.DataValueField="valor";
				dlTipo.DataBind();
				
				dlAgencia.DataSource=ViewState["dtAgencias"];
				dlAgencia.DataTextField="texto";
				dlAgencia.DataValueField="valor";
				dlAgencia.DataBind();

				txtTalonarios.Attributes.Add("onkeyup","Total("+objsScr+");");
				txtInicio.Attributes.Add("onkeyup","Total("+objsScr+");");
				dlTipo.Attributes.Add("onchange","Total("+objsScr+");");
				dlAgencia.Attributes.Add("onchange","Total("+objsScr+");");
			}
		}

		private void btnRecibir_Click(object sender, System.EventArgs e)
		{
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado.");
				return;
			}
			string despacho=DBFunctions.SingleData("SELECT MDESPACHO_NUMERO+1 FROM DBXSCHEMA.MDESPACHO_PAPELERIA ORDER BY MDESPACHO_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(despacho.Length==0)despacho="1";
			//Validar
			#region Validar Documentos
			ArrayList arrInicios=new ArrayList();
			ArrayList arrFines=new ArrayList();
			ArrayList arrTiposDocs=new ArrayList();
			ArrayList arrFilas=new ArrayList();
			DataSet dsLimitesTipos=new DataSet();
			DBFunctions.Request(dsLimitesTipos,IncludeSchema.NO,"SELECT TDOC_CODIGO, MAX_TALONARIO FROM DBXSCHEMA.TDOCU_TRANS;");
			int linea=1,fila=1;
			bool info=false;
			string errores="";
			foreach(DataGridItem dgrI in dgrDocumentos.Items)
			{
				bool errLin=false;
				DropDownList ddlAgencia=(DropDownList)dgrI.FindControl("ddlAgencia");
				DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
				TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
				TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
				TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
				if(ddlAgencia.SelectedValue.Length>0 || ddlTipoDocumento.SelectedValue.Length>0 || txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0)
				{
					int talonarios=0;
					long inicio=0, fin=0, prefijo=0;
					string tipo=ddlTipoDocumento.SelectedValue.Trim().Replace("|","");;
					string clase="M";
					string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
					if(ddlTipoDocumento.SelectedValue.EndsWith("|") && ddlAgencia.SelectedValue.EndsWith("|"))prefijo=Convert.ToInt16(agencia);
					info=true;
					//Agencia
					if(agencia.Length==0)
					{
						errores+="Debe seleccionar la agencia en la línea "+ fila+". ";errLin=true;}
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
						errores+="Cantidad de talonarios no válida en la línea "+fila+". ";errLin=true;}
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
						if(ddlTipoDocumento.SelectedValue.EndsWith("|") && fin>=Math.Pow(10,Comercial.Tiquetes.lenTiquete))throw(new Exception());
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
					string prfAgencia="";
					//para verificar prefijo corresponde agencia
					if(prefijo>0)
					{
						long limMin=(long)Convert.ToInt16(agencia)*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete);
						long limMax=(long)(Convert.ToInt16(agencia)+1)*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete);
						limMin--;
						prfAgencia=" NUM_DOCUMENTO>"+limMin+" AND NUM_DOCUMENTO<"+limMax+" AND ";
					}
					long totalDB=Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE "+prfAgencia+" TDOC_CODIGO='"+tipo+"' AND TIPO_DOCUMENTO='"+clase+"' AND NUM_DOCUMENTO BETWEEN "+inicio+" AND "+fin+" AND NUM_DESPACHO IS NULL AND MAG_CODIGO IS NULL AND FECHA_DESPACHO IS NULL AND MNIT_RESPONSABLE IS NULL AND MPLA_CODIGO IS NULL AND FECHA_USO IS NULL;"));
					if(total>totalDB)
					{
						errores+="No se han recibido todos los documentos especificados en el rango de la línea "+fila+", no coinciden todos sus tipos o ya han sido despachados. ";errLin=true;}
					//Maximo permitido
					int maxL=0;
					if(tipo.Length>0)
					{
						maxL=Convert.ToInt32(dsLimitesTipos.Tables[0].Select("tdoc_codigo='"+tipo+"'")[0]["MAX_TALONARIO"]);
						if(total>(maxL*talonarios))
						{
							errores+="Se superó el número máximo de documentos permitidos por talonario en la línea "+fila+". ";errLin=true;}
					}
					//Guardar
					if(!errLin)
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
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
			ArrayList sqlIns=new ArrayList();
			if(errores.Length==0 && info)
				foreach(DataGridItem dgrI in dgrDocumentos.Items)
				{
					DropDownList ddlAgencia=(DropDownList)dgrI.FindControl("ddlAgencia");
					DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
					TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
					TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
					TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
					if(ddlAgencia.SelectedValue.Length>0 || ddlTipoDocumento.SelectedValue.Length>0 || txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0)
					{
						int talonarios=0;
						long inicio=0, fin=0, prefijo=0;
						string tipo=ddlTipoDocumento.SelectedValue.Trim().Replace("|","");;
						string clase="M";
						string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
						if(ddlTipoDocumento.SelectedValue.EndsWith("|") && ddlAgencia.SelectedValue.EndsWith("|"))prefijo=Convert.ToInt16(agencia);
						talonarios=int.Parse(txtTalonarios.Text.Trim());
						inicio=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtInicioDocumento.Text.Trim());
						fin=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtFinDocumento.Text.Trim());
						//existen  todos en la BD, con fecha despacho nulo?
						long total=fin-inicio+1;
						long finAct=inicio,inicioAct=inicio;
						//Dividir recepciones en paquetes de 1000 para evitar posibles errores de timeout
						while(finAct<=fin)
						{
							finAct=inicioAct+999;
							if(finAct>fin)finAct=fin;
							sqlIns.Add("call dbxschema.DESPACHO_PAPEL('"+tipo+"',"+inicioAct+","+finAct+",'M',"+despacho+","+agencia+");");
							inicioAct=finAct+1;
							if(inicioAct>fin)break;
						}
					
						DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MDESPACHO_PAPELERIA VALUES("+despacho+","+linea+","+agencia+",'"+tipo+"','"+clase+"',"+talonarios+","+inicio+","+fin+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"');");
						
						if(!DBFunctions.Transaction(sqlIns))
						{
							errores+="No se pudo despachar la papeleria: "+ddlTipoDocumento.SelectedItem.Text+" "+inicio+"-"+fin+"\\r\\n";
							lblError.Text+=DBFunctions.exceptions;
						}
						else
						{
							DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MDESPACHO_PAPELERIA VALUES("+despacho+","+linea+","+agencia+",'"+tipo+"','"+clase+"',"+talonarios+","+inicio+","+fin+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"');");
							ddlTipoDocumento.SelectedIndex=0;
							txtTalonarios.Text="";
							txtInicioDocumento.Text="";
							txtFinDocumento.Text="";
							ddlAgencia.SelectedIndex=0;
						}
						
						linea++;
					}
				}
			#endregion

			if(!info)errores+="No ingresó información.";
			if(errores.Length==0)
				Response.Redirect(indexPage+"?process=Comercial.DespachoPapeleria&act=1&path="+Request.QueryString["path"]+"&dsp="+despacho);
			else strActScript+="alert('"+errores+"');";
		}
	}
}

