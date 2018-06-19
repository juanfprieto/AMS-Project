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
	///		Descripción breve de AMS_Comercial_DevolucionPapeleriaAgencia.
	/// </summary>
	public class AMS_Comercial_DevolucionPapeleriaAgencia : System.Web.UI.UserControl
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
		protected System.Web.UI.WebControls.Button btnDevolver;
		protected System.Web.UI.WebControls.Label lblError;
		public string strActScript;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				string devolucion=DBFunctions.SingleData("SELECT MDEVOLUCION_NUMERO+1 FROM DBXSCHEMA.MDEVOLUCION_PAPELERIA ORDER BY MDEVOLUCION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
				if(devolucion.Length==0)devolucion="1";
				lblNumero.Text=devolucion;
				lblFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");

				Agencias.TraerAgenciasPrefijoUsuario(ddlAgencia);
				
				//TIPOS DE DOCUMENTOS
				DataSet dsTiposDoc=new DataSet();
				DBFunctions.Request(dsTiposDoc,IncludeSchema.NO,
					"select tdoc_codigo concat case when prefijo='S' then '|' else '' end as valor, tdoc_nombre as texto from DBXSCHEMA.TDOCU_TRANS WHERE PAPELERIA='S' ORDER BY TDOC_NOMBRE;");
				DataRow drC=dsTiposDoc.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsTiposDoc.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtTiposDocumentos",dsTiposDoc.Tables[0]);
				//CONCEPTOS DEVOLUCION
				DataSet dsTiposAnul=new DataSet();
				DBFunctions.Request(dsTiposAnul,IncludeSchema.NO,
					"select rtrim(char(tcon_codigo)) as valor,tcon_descripcion as texto from DBXSCHEMA.TCONCEPTO_DEVOLUCION_PAPELERIA ORDER BY TCON_DESCRIPCION;");
				drC=dsTiposAnul.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsTiposAnul.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtTiposDevolucion",dsTiposAnul.Tables[0]);
				MostrarTabla();
				if(Request.QueryString["act"]!=null && Request.QueryString["dvl"]!=null)
                Utils.MostrarAlerta(Response, "La papelería ha sido devuelta con el número de devolución " + Request.QueryString["dvl"] + "");
			}
		}
		//Mostrar grilla
		private void MostrarTabla()
		{
			DataTable dtDocumentos=new DataTable();
			//Tabla tiquetes normales
			dtDocumentos.Columns.Add("NUMERO",typeof(int));
			for(int n=1;n<=20;n++)
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
			this.btnDevolver.Click += new System.EventHandler(this.btnDevolver_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnDevolver_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue.Replace("|","").Trim();
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado.");
				return;
			}
			string devolucion=DBFunctions.SingleData("SELECT MDEVOLUCION_NUMERO+1 FROM DBXSCHEMA.MDEVOLUCION_PAPELERIA ORDER BY MDEVOLUCION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(devolucion.Length==0)devolucion="1";
			//Validar
			#region Validar Documentos
			ArrayList arrNumeros=new ArrayList();
			ArrayList arrTiposDocs=new ArrayList();
			ArrayList arrInicios=new ArrayList();
			ArrayList arrFines=new ArrayList();
			ArrayList arrFilas=new ArrayList();
			int linea=1,fila=1;
			long inicio=0, fin=0;
			bool info=false;
			string errores="";
			foreach(DataGridItem dgrI in dgrDocumentos.Items)
			{
				bool errLin=false;
				DropDownList ddlTipoDocumento=(DropDownList)dgrI.FindControl("ddlTipoDocumento");
				TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
				TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
				DropDownList ddlConceptoDevolucion=(DropDownList)dgrI.FindControl("ddlConceptoDevolucion");
				if(ddlTipoDocumento.SelectedValue.Length>0 || ddlConceptoDevolucion.SelectedValue.Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0)
				{
					string tipo=ddlTipoDocumento.SelectedValue.Trim().Replace("|","");
					string clase="M";
					string concepto=ddlConceptoDevolucion.SelectedValue.Trim();
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
						errores+="Debe seleccionar el concepto de devolución en la línea "+ fila+". ";errLin=true;}
					//Inicio
					try
					{
						inicio=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtInicioDocumento.Text.Trim());
						if(inicio<0)throw(new Exception());}
					catch
					{
						errores+="Numero inicial no válido en la línea "+fila+". ";errLin=true;}
					//Final
					try
					{
						fin=int.Parse(txtFinDocumento.Text.Trim());
						if(ddlTipoDocumento.SelectedValue.EndsWith("|") && fin>=Math.Pow(10,Comercial.Tiquetes.lenTiquete))throw(new Exception());
						fin=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+fin;
						if(fin<0)throw(new Exception());}
					catch
					{
						errores+="Numero final no válido en la línea "+fila+". ";errLin=true;}
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
					arrFilas.Add(fila);
					arrTiposDocs.Add(tipo);
					//Validos?
					if((fin-inicio+1)!=Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM MCONTROL_PAPELERIA WHERE TDOC_CODIGO='"+tipo+"' AND NUM_DOCUMENTO>="+inicio+" AND NUM_DOCUMENTO<="+fin+" AND TIPO_DOCUMENTO='"+clase+"' AND MAG_CODIGO="+agencia+" AND NUM_RECEPCION IS NOT NULL AND FECHA_RECEPCION IS NOT NULL AND NUM_DESPACHO IS NOT NULL AND FECHA_DESPACHO IS NOT NULL AND NUM_ASIGNACION IS NULL AND FECHA_ASIGNACION IS NULL AND MNIT_RESPONSABLE IS NULL AND MPLA_CODIGO IS NULL AND FECHA_USO IS NULL;"))){
						errores+="No se encontraron todos los documentos de la línea "+fila+" (deben estar despachados y no asignados). ";errLin=true;}
					//Guardar
					if(!errLin)
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
					else{
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
					TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
					TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
					DropDownList ddlConceptoDevolucion=(DropDownList)dgrI.FindControl("ddlConceptoDevolucion");
					if(ddlTipoDocumento.SelectedValue.Length>0 || ddlConceptoDevolucion.SelectedValue.Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0)
					{
						string tipo=ddlTipoDocumento.SelectedValue.Trim().Replace("|","");
						string clase="M";
						string concepto=ddlConceptoDevolucion.SelectedValue.Trim();
						int prefijo=0;
						if(ddlTipoDocumento.SelectedValue.EndsWith("|") && ddlAgencia.SelectedValue.EndsWith("|"))
							prefijo=Convert.ToInt16(agencia);
						inicio=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtInicioDocumento.Text.Trim());
						fin=(prefijo*(long)Math.Pow(10,Comercial.Tiquetes.lenTiquete))+int.Parse(txtFinDocumento.Text.Trim());
						/*for(long numeroDoc=inicio;numeroDoc<=fin;numeroDoc++)
							DBFunctions.NonQuery("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET NUM_DEVOLUCION="+devolucion+", FECHA_DEVOLUCION='"+DateTime.Now.ToString("yyyy-MM-dd")+"', MNIT_DEVOLUCION='"+nitResponsable+"', NUM_DESPACHO=NULL, MAG_CODIGO=NULL, FECHA_DESPACHO=NULL WHERE TDOC_CODIGO='"+tipo+"' AND NUM_DOCUMENTO="+numeroDoc+";");*/
						DBFunctions.NonQuery("call dbxschema.DEVAGEN_PAPEL('"+tipo+"',"+inicio+","+fin+",'M',"+devolucion+",'"+nitResponsable+"');");
						DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MDEVOLUCION_PAPELERIA VALUES("+devolucion+","+linea+","+agencia+",'"+tipo+"','"+clase+"',"+inicio+","+fin+","+concepto+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"');");
						linea++;
					}
				}
			#endregion

			if(!info)errores+="No ingresó información.";
			if(errores.Length==0)
				Response.Redirect(indexPage+"?process=Comercial.DevolucionPapeleriaAgencia&act=1&path="+Request.QueryString["path"]+"&dvl="+devolucion);
			else strActScript+="alert('"+errores+"');";
		}

		private void dgrDocumentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlTipo=(DropDownList)e.Item.FindControl("ddlTipoDocumento");
				DropDownList dlConceptoDevolucion=(DropDownList)e.Item.FindControl("ddlConceptoDevolucion");
				
				dlTipo.DataSource=ViewState["dtTiposDocumentos"];
				dlTipo.DataTextField="texto";
				dlTipo.DataValueField="valor";
				dlTipo.DataBind();
				
				dlConceptoDevolucion.DataSource=ViewState["dtTiposDevolucion"];
				dlConceptoDevolucion.DataTextField="texto";
				dlConceptoDevolucion.DataValueField="valor";
				dlConceptoDevolucion.DataBind();
			}
		}
	}
}
