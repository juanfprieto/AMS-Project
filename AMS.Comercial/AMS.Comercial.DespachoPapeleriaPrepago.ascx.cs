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
	///		Descripci�n breve de AMS_Comercial_DespachoPapeleriaPrepago.
	/// </summary>
	public class AMS_Comercial_DespachoPapeleriaPrepago : System.Web.UI.UserControl
	{
		
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblNumero;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.DataGrid dgrDocumentos;
		protected System.Web.UI.WebControls.Button btnRecibir;
		protected System.Web.UI.WebControls.Label lblError;
		public string strActScript;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				string despacho=DBFunctions.SingleData("SELECT MDESPACHO_NUMERO+1 FROM DBXSCHEMA.MDESPACHO_PAPELERIA ORDER BY MDESPACHO_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
				if(despacho.Length==0)despacho="1";
				lblNumero.Text=despacho;
				lblFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				//Descuento prepago
				double porcentajePrepago;
				try{
					porcentajePrepago=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_PORCENTAJE FROM DBXSCHEMA.TPORCENTAJESTRANSPORTES WHERE CLAVE='PREPAGO';"));
					ViewState["PorcentajePrepago"]=porcentajePrepago.ToString("##0.##");}
				catch{
					ViewState["PorcentajePrepago"]="0";}
				MostrarTabla();
				ViewState["CANTIDAD_TALONARIOS"]=DBFunctions.SingleData("SELECT CANTIDAD_TIQUETERA FROM DBXSCHEMA.TDOCU_TRANS WHERE TDOC_CODIGO='TIQPRE';");
				if(Request.QueryString["act"]!=null && Request.QueryString["dsp"]!=null)
                Utils.MostrarAlerta(Response, "La papeler�a ha sido despachada con el n�mero de despacho " + Request.QueryString["dsp"] + ".");
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
			#region Documentos
			ArrayList arrInicios=new ArrayList();
			ArrayList arrFines=new ArrayList();
			ArrayList arrFilas=new ArrayList();
			ArrayList sqlUpd=new ArrayList();
			int limiteTipo=Convert.ToInt32(DBFunctions.SingleData("SELECT MAX_TALONARIO FROM DBXSCHEMA.TDOCU_TRANS WHERE TDOC_CODIGO='TIQPRE';"));
			int linea=1,fila=1;
			bool info=false;
			string errores="";
			double descuento=0;
			foreach(DataGridItem dgrI in dgrDocumentos.Items){
				bool errLin=false;
				TextBox txtRuta=(TextBox)dgrI.FindControl("txtRuta");
				TextBox txtTalonarios=(TextBox)dgrI.FindControl("txtTalonarios");
				TextBox txtInicioDocumento=(TextBox)dgrI.FindControl("txtInicioDocumento");
				TextBox txtFinDocumento=(TextBox)dgrI.FindControl("txtFinDocumento");
				TextBox txtDescuento=(TextBox)dgrI.FindControl("txtDescuento");
				if(txtRuta.Text.Trim().Length>0 || txtTalonarios.Text.Trim().Length>0 || txtInicioDocumento.Text.Trim().Length>0 || txtFinDocumento.Text.Trim().Length>0){
					int talonarios=0;
					int inicio=0, fin=0;
					string tipo="TIQPRE";
					string ruta=txtRuta.Text.Trim().ToUpper();
					string clase="M";
					if(ruta.Length>0 && ruta.Length<7)ruta=ruta+new string(' ',7-ruta.Length);
					txtRuta.Text=ruta;
					info=true;
					//Ruta
					if(!DBFunctions.RecordExist("SELECT MRUT_CODIGO FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+ruta+"';")){
						errores+="No existe la ruta de la l�nea "+ fila+". ";errLin=true;}
					//Talonarios
					try{
						talonarios=int.Parse(txtTalonarios.Text.Trim());}
					catch
					{
						errores+="Cantidad de talonarios no v�lida en la l�nea "+fila+". ";errLin=true;}
					//Inicio
					try
					{
						inicio=int.Parse(txtInicioDocumento.Text.Trim());
						if(inicio<0)throw(new Exception());}
					catch{
						errores+="N�mero inicial no v�lido en la l�nea "+fila+". ";errLin=true;}
					//Fin
					try
					{
						fin=int.Parse(txtFinDocumento.Text.Trim());
						if(fin<0)throw(new Exception());}
					catch
					{
						errores+="N�mero final no v�lido en la l�nea "+fila+". ";errLin=true;}
					//Inicio>Final?
					if(inicio>fin){
						errores+="N�mero inicial mayor a n�mero final en la l�nea "+fila+". ";errLin=true;}
					//Descuento
					try{
						descuento=Convert.ToDouble(txtDescuento.Text.Trim().Replace(",",""));
						if(descuento<0 || descuento>100)throw(new Exception());}
					catch{
						errores+="Valor de descuento no v�lido en la l�nea "+fila+". ";errLin=true;}
					//Rangos cruzados?
					for(int n=0;n<linea-1;n++){
						int iniA=(int)arrInicios[n];
						int finA=(int)arrFines[n];
						if((inicio<=finA && inicio>=iniA) || (fin<=finA && fin>=iniA) || (fin>=finA && inicio<=iniA)){
							errores+="El rango de la l�nea "+fila+" se cruza con el rango de la fila "+arrFilas[n].ToString()+". ";errLin=true;}
					}
					arrInicios.Add(inicio);
					arrFines.Add(fin);
					arrFilas.Add(fila);
					//existen  todos en la BD, con fecha despacho nulo?
					int total=fin-inicio+1;
					int totalDB=Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='"+tipo+"' AND TIPO_DOCUMENTO='"+clase+"' AND NUM_DOCUMENTO BETWEEN "+inicio+" AND "+fin+" AND NUM_DESPACHO IS NULL AND MAG_CODIGO IS NULL AND FECHA_DESPACHO IS NULL AND MNIT_RESPONSABLE IS NULL AND MPLA_CODIGO IS NULL AND FECHA_USO IS NULL;"));
					if(total>totalDB){
						errores+="No se han recibido todos los documentos especificados en el rango de la l�nea "+fila+", no coinciden todos sus tipos o ya han sido despachados. ";errLin=true;}
					//Maximo permitido
					int maxL=0;
					if(tipo.Length>0){
						maxL=limiteTipo;
						if(total>(maxL*talonarios)){
							errores+="Se super� el n�mero m�ximo de documentos permitidos por talonario en la l�nea "+fila+". ";errLin=true;}
					}
					//Guardar
					if(!errLin){
						for(int nD=inicio;nD<=fin;nD++)
							sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET NUM_DESPACHO="+despacho+", MRUT_CODIGO='"+ruta+"', PORCENTAJE_DESCUENTO="+descuento+", FECHA_DESPACHO='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE TDOC_CODIGO='"+tipo+"' AND NUM_DOCUMENTO="+nD+";");
						sqlUpd.Add("INSERT INTO DBXSCHEMA.MDESPACHO_PAPELERIA VALUES("+despacho+","+linea+",NULL,'"+tipo+"','"+clase+"',"+talonarios+","+inicio+","+fin+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"');");
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;}
					else{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";}
					linea++;
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}
			#endregion
			if(!info)errores+="No ingres� informaci�n.";
			if(errores.Length==0)
			{
				if(DBFunctions.Transaction(sqlUpd))
					Response.Redirect(indexPage+"?process=Comercial.DespachoPapeleriaPrepago&act=1&path="+Request.QueryString["path"]+"&dsp="+despacho);
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
