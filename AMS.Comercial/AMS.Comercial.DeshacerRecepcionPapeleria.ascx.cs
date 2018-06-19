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
	///		Descripci�n breve de AMS_Comercial_DeshacerRecepcionPapeleria.
	/// </summary>
	public class AMS_Comercial_DeshacerRecepcionPapeleria : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		protected System.Web.UI.WebControls.Button btnRecibir;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblNumero;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		public string strActScript="";
		public string strPrefijos="";
		public string strTalonariosScript="";
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				string eliminacion=DBFunctions.SingleData("SELECT MELIMINACION_NUMERO+1 FROM DBXSCHEMA.MELIMINACION_PAPELERIA ORDER BY MELIMINACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
				if(eliminacion.Length==0)eliminacion="1";
				lblNumero.Text=eliminacion;
				lblFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				//TIPOS DE DOCUMENTOS
				DataSet dsTiposDoc=new DataSet();
				DBFunctions.Request(dsTiposDoc,IncludeSchema.NO,
					"select tdoc_codigo concat case when prefijo='S' then '|' else '' end as valor, tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS WHERE PAPELERIA='S' ORDER BY TDOC_NOMBRE;");
				DataRow drC=dsTiposDoc.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsTiposDoc.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtTiposDocumentos",dsTiposDoc.Tables[0]);
				//Prefijos
				DataSet dsPrefijoDoc=new DataSet();
				DBFunctions.Request(dsPrefijoDoc,IncludeSchema.NO,
					"select mag_codigo as valor, mage_nombre as texto from DBXSCHEMA.MAGENCIA where prefijo='S' ORDER BY MAGE_NOMBRE;");
				drC=dsPrefijoDoc.Tables[0].NewRow();
				drC[0]="0";
				drC[1]="---no asignado---";
				dsPrefijoDoc.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtPrefijosDocumentos",dsPrefijoDoc.Tables[0]);
				MostrarTabla();
				ConsultarCapacidadTalonarios();
				if(Request.QueryString["act"]!=null && Request.QueryString["elm"]!=null)
                Utils.MostrarAlerta(Response, "La papeleria ha sido eliminada con el n�mero de eliminaci�n " + Request.QueryString["elm"] + ".");
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
			ViewState.Add("PREFIJOS",strPrefijos);
		}


		#region C�digo generado por el Dise�ador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		M�todo necesario para admitir el Dise�ador. No se puede modificar
		///		el contenido del m�todo con el editor de c�digo.
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
				DropDownList dlPrefijo=(DropDownList)e.Item.FindControl("ddlPrefijo");
				TextBox txtTalonarios=(TextBox)e.Item.FindControl("txtTalonarios");
				TextBox txtInicio=(TextBox)e.Item.FindControl("txtInicioDocumento");
				TextBox txtFin=(TextBox)e.Item.FindControl("txtFinDocumento");
				string objsScr="'"+dlTipo.ClientID+"','"+txtTalonarios.ClientID+"','"+txtInicio.ClientID+"','"+txtFin.ClientID+"'";
				dlTipo.DataSource=ViewState["dtTiposDocumentos"];
				dlTipo.DataTextField="texto";
				dlTipo.DataValueField="valor";
				dlTipo.DataBind();
				dlPrefijo.DataSource=ViewState["dtPrefijosDocumentos"];
				dlPrefijo.DataTextField="texto";
				dlPrefijo.DataValueField="valor";
				dlPrefijo.DataBind();
				txtTalonarios.Attributes.Add("onkeyup","Total("+objsScr+");");
				txtInicio.Attributes.Add("onkeyup","Total("+objsScr+");");
				dlTipo.Attributes.Add("onchange","VerPrefijo('"+dlTipo.ClientID+"','"+dlPrefijo.ClientID+"');Total("+objsScr+");");

				dlPrefijo.Style.Add("display","none");
				strPrefijos+="VerPrefijo('"+dlTipo.ClientID+"','"+dlPrefijo.ClientID+"');";
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
			string eliminacion=DBFunctions.SingleData("SELECT MELIMINACION_NUMERO+1 FROM DBXSCHEMA.MELIMINACION_PAPELERIA ORDER BY MELIMINACION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(eliminacion.Length==0)eliminacion="1";
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
				DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
				DropDownList ddlPrefijo=(DropDownList)dgrI.FindControl("ddlPrefijo");
				TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
				TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
				TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
				if((ddlTipoDocumento.SelectedValue.Length>0 || txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0))
				{
					int talonarios=0; 
					long inicio=0, fin=0, prefijo=0;
					string tipo=ddlTipoDocumento.SelectedValue.Replace("|","").Trim();
					if(ddlTipoDocumento.SelectedValue.EndsWith("|"))prefijo=Convert.ToInt16(ddlPrefijo.SelectedValue);
					string clase="M";
					info=true;
					//Tipo
					if(tipo.Length==0)
					{
						errores+="Debe seleccionar la clase de documento en la l�nea "+ fila+". ";errLin=true;}
					//Tipo
					if(clase.Length==0)
					{
						errores+="Debe seleccionar el tipo en la l�nea "+ fila+". ";errLin=true;}
					//Talonarios
					try
					{
						talonarios=int.Parse(txtTalonarios.Text.Trim());}
					catch
					{
						errores+="Cantidad de talonarios no v�lido en la l�nea "+fila+". ";errLin=true;}
					//Inicio
					try
					{
						inicio=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtInicioDocumento.Text.Trim());
						if(inicio<0)throw(new Exception());}
					catch
					{
						errores+="N�mero inicial no v�lido en la l�nea "+fila+". ";errLin=true;}
					//Fin
					try
					{
						fin=int.Parse(txtFinDocumento.Text.Trim());
						if(ddlTipoDocumento.SelectedValue.EndsWith("|") && fin>=Math.Pow(10,Comercial.Tiquetes.lenTiquete))throw(new Exception());
						fin=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+fin;
						if(fin<0)throw(new Exception());}
					catch
					{
						errores+="N�mero final no v�lido en la l�nea "+fila+". ";errLin=true;}
					//Inicio>Final?
					if(inicio>fin)
					{
						errores+="N�mero inicial mayor a n�mero final en la l�nea "+fila+". ";errLin=true;}
					//Rangos cruzados?
					for(int n=0;n<linea-1;n++)
					{
						long iniA=(long)arrInicios[n];
						long finA=(long)arrFines[n];
						if(tipo.Equals(arrTiposDocs[n]) && ((inicio<=finA && inicio>=iniA) || (fin<=finA && fin>=iniA) || (fin>=finA && inicio<=iniA)))
						{
							errores+="El rango de la l�nea "+fila+" se cruza con el rango de la fila "+arrFilas[n].ToString()+". ";errLin=true;}
					}
					//Maximo permitido
					int maxL=0;
					if(tipo.Length>0)
					{
						maxL=Convert.ToInt32(dsLimitesTipos.Tables[0].Select("tdoc_codigo='"+tipo+"'")[0]["MAX_TALONARIO"]);
						if((fin-inicio+1)>(maxL*talonarios))
						{
							errores+="Se super� el n�mero m�ximo de documentos permitidos por talonario en la l�nea "+fila+". ";errLin=true;}
					}
					arrInicios.Add(inicio);
					arrFines.Add(fin);
					arrTiposDocs.Add(tipo);
					arrFilas.Add(fila);
					//rangos en BD
					if((fin-inicio+1)!=Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM MCONTROL_PAPELERIA WHERE TDOC_CODIGO='"+tipo+"' AND NUM_DOCUMENTO>="+inicio+" AND NUM_DOCUMENTO<="+fin+" AND TIPO_DOCUMENTO='"+clase+"' AND MAG_CODIGO IS NULL AND NUM_RECEPCION IS NOT NULL AND FECHA_RECEPCION IS NOT NULL AND NUM_DESPACHO IS NULL AND FECHA_DESPACHO IS NULL AND NUM_ASIGNACION IS NULL AND FECHA_ASIGNACION IS NULL AND MNIT_RESPONSABLE IS NULL AND MPLA_CODIGO IS NULL AND FECHA_USO IS NULL;"))){
						errores+="No se encontraron todos los documentos de la l�nea "+fila+" (deben estar recepcionados, sin despachar ni utilizar). ";errLin=true;}
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
			if(errores.Length==0 && info)
			{
				linea=1;
				foreach(DataGridItem dgrI in dgrDocumentos.Items)
				{
					DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
					DropDownList ddlPrefijo=(DropDownList)dgrI.FindControl("ddlPrefijo");
					TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
					TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
					TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
					if((ddlTipoDocumento.SelectedValue.Length>0 || txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0))
					{
						int talonarios=0; 
						long inicio=0, fin=0, prefijo=0;
						string tipo=ddlTipoDocumento.SelectedValue.Replace("|","").Trim();
						if(ddlTipoDocumento.SelectedValue.EndsWith("|"))prefijo=Convert.ToInt16(ddlPrefijo.SelectedValue);
						string clase="M";
						info=true;
						talonarios=int.Parse(txtTalonarios.Text.Trim());
						inicio=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtInicioDocumento.Text.Trim());
						fin=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtFinDocumento.Text.Trim());
						//Guardar
						DBFunctions.NonQuery("call dbxschema.ELIMINACION_PAPEL('"+tipo+"',"+inicio+","+fin+",'M');");
						DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MELIMINACION_PAPELERIA VALUES("+eliminacion+","+linea+",'"+tipo+"','"+clase+"',"+talonarios+","+inicio+","+fin+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"');");
						linea++;
					}
				}
			}
			#endregion

			if(!info)errores+="No ingres� informaci�n.";
			if(errores.Length==0)
				Response.Redirect(indexPage+"?process=Comercial.DeshacerRecepcionPapeleria&act=1&path="+Request.QueryString["path"]+"&elm="+eliminacion);
			else strActScript+="alert('"+errores+"');";
		}
	}
}
