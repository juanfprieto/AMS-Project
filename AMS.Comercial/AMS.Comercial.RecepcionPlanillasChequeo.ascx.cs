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
	///		Descripción breve de AMS_Comercial_RecepcionPlanillasChequeo.
	/// </summary>
	public class AMS_Comercial_RecepcionPlanillasChequeo : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblNumero;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		protected System.Web.UI.WebControls.Button btnRecibir;
		protected System.Web.UI.WebControls.Label lblError;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		public string strActScript;
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				string recepcion=DBFunctions.SingleData("SELECT MRECEPCION_NUMERO+1 FROM DBXSCHEMA.MRECEPCION_PAPELERIA ORDER BY MRECEPCION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
				if(recepcion.Length==0)recepcion="1";
				lblNumero.Text=recepcion;
				lblFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				MostrarTabla();
				ViewState["CANTIDAD_TALONARIOS"]=DBFunctions.SingleData("SELECT CANTIDAD_TIQUETERA FROM DBXSCHEMA.TDOCU_TRANS WHERE TDOC_CODIGO='PLACHQ';");
				if(Request.QueryString["act"]!=null && Request.QueryString["rec"]!=null)
					Response.Write("<script language='javascript'>alert('La papeleria ha sido creada con el número de recepción "+Request.QueryString["rec"]+".');</script>");
			}
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
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario actual (responsable) no tiene un NIT asociado.');</script>");
				return;
			}
			string recepcion=DBFunctions.SingleData("SELECT MRECEPCION_NUMERO+1 FROM DBXSCHEMA.MRECEPCION_PAPELERIA ORDER BY MRECEPCION_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(recepcion.Length==0)recepcion="1";
			//Validar
			#region Documentos
			ArrayList arrInicios=new ArrayList();
			ArrayList arrFines=new ArrayList();
			ArrayList arrFilas=new ArrayList();
			ArrayList sqlUpd=new ArrayList();
			DataSet dsLimitesTipos=new DataSet();
			int limiteTipo=Convert.ToInt32(DBFunctions.SingleData("SELECT MAX_TALONARIO FROM DBXSCHEMA.TDOCU_TRANS WHERE TDOC_CODIGO='PLACHQ';"));
			int linea=1,fila=1;
			bool info=false;
			string errores="";
			foreach(DataGridItem dgrI in dgrDocumentos.Items)
			{
				bool errLin=false;
				TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
				TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
				TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
				if(txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0)
				{
					int talonarios=0; 
					int inicio=0, fin=0;
					string tipo="PLACHQ";
					string clase="M";
					info=true;
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
						inicio=int.Parse(txtInicioDocumento.Text.Trim());
						if(inicio<0)throw(new Exception());}
					catch
					{
						errores+="Número inicial no válido en la línea "+fila+". ";errLin=true;}
					//Fin
					try
					{
						fin=int.Parse(txtFinDocumento.Text.Trim());
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
						int iniA=(int)arrInicios[n];
						int finA=(int)arrFines[n];
						if((inicio<=finA && inicio>=iniA) || (fin<=finA && fin>=iniA) || (fin>=finA && inicio<=iniA))
						{
							errores+="El rango de la línea "+fila+" se cruza con el rango de la fila "+arrFilas[n].ToString()+". ";errLin=true;}
					}
					//Maximo permitido
					int maxL=0;
					if(tipo.Length>0)
					{
						maxL=limiteTipo;
						if((fin-inicio+1)>(maxL*talonarios))
						{
							errores+="Se superó el número máximo de documentos permitidos por talonario en la línea "+fila+". ";errLin=true;}
					}
					arrInicios.Add(inicio);
					arrFines.Add(fin);
					arrFilas.Add(fila);
					//rangos en BD
					if(DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='"+tipo+"' AND (NUM_DOCUMENTO BETWEEN "+inicio+" AND "+fin+");"))
					{
						errores+="Ya se han generado documentos dentro del rango definido en la línea "+fila+". ";errLin=true;}
					//Guardar
					if(!errLin)
					{
						DBFunctions.NonQuery("call dbxschema.RECEPCION_PAPEL('PLACHQ',"+inicio+","+fin+",'M',"+recepcion+");");
						sqlUpd.Add("INSERT INTO DBXSCHEMA.MRECEPCION_PAPELERIA VALUES("+recepcion+","+linea+",'"+tipo+"','"+clase+"',"+talonarios+","+inicio+","+fin+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"');");
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
					Response.Redirect(indexPage+"?process=Comercial.RecepcionPlanillasChequeo&act=1&path="+Request.QueryString["path"]+"&rec="+recepcion);
				else
					lblError.Text=DBFunctions.exceptions;}
			else strActScript+="alert('"+errores+"');";
		}

		private void dgrDocumentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				TextBox txtTalonarios=(TextBox)e.Item.FindControl("txtTalonarios");
				TextBox txtInicio=(TextBox)e.Item.FindControl("txtInicioDocumento");
				TextBox txtFin=(TextBox)e.Item.FindControl("txtFinDocumento");
				string objsScr="'"+txtTalonarios.ClientID+"','"+txtInicio.ClientID+"','"+txtFin.ClientID+"'";
				txtTalonarios.Attributes.Add("onkeyup","Total("+objsScr+");");
				txtInicio.Attributes.Add("onkeyup","Total("+objsScr+");");
			}
		}
	}
}
