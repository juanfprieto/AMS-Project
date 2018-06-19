using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.IO;
using AMS.DBManager;
using AMS.Forms;
using AMS.Tools;
using AMS.DB;
using Ajax;

namespace AMS.Comercial
{
	/// <summary>
	///		Descripción breve de AMS_Comercial_ConfiguracionBus.
	/// </summary>
	public class AMS_Comercial_ConfiguracionBus : System.Web.UI.UserControl
	{
		#region controles, variables

		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtFilas;
		protected System.Web.UI.WebControls.TextBox txtColumnas;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolder2;
		protected Table tb = new Table();
		protected Table tbimg = new Table();
		protected System.Web.UI.WebControls.Literal Literal1;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolder1;
		protected System.Web.UI.WebControls.Image Image1;
		protected int silla=0;
		protected System.Web.UI.WebControls.Button Button2;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolder3;
		protected System.Web.UI.WebControls.Image Image2;
		protected System.Web.UI.WebControls.DropDownList ddlConfiguracion;
		protected System.Web.UI.WebControls.Button btnModificar;
		protected System.Web.UI.WebControls.Button btnReconfigurar;
		protected System.Web.UI.WebControls.Button btnVolver;
		protected System.Web.UI.WebControls.Label lblError;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private enum tipoElemento{Vacio,Elemento,Herramienta,Caneca};
		#endregion
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Comercial.AMS_Comercial_ConfiguracionBus));		
			//Button2.Attributes.Add("onClick","if(confirm('Esta seguro que desea guardar esta configuracion Modelo?')){document.getElementById('"+Button2.ClientID+"').disabled = true;"+this.Page.GetPostBackEventReference(Button2)+";return true;}else {return false;}");	
			// Introducir aquí el código de usuario para inicializar la página
			if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlConfiguracion,"select mcon_cod,nombre from dbxschema.mconfiguracionbus order by nombre");
				ListItem it=new ListItem("--Seleccione--","");
				this.ddlConfiguracion.Items.Insert(0,it);
                if (Request.QueryString["act"] != null) Utils.MostrarAlerta(Response, "La configuracion ha sido actualizada.");
			}
			
		}
		
		[Ajax.AjaxMethod]
		public DataSet CargarConfig(string config)
		{
			DataSet Vins= new DataSet();
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mbus_filas as FILAS, mbus_columnas as COLUMNAS from dbxschema.mconfiguracionbus where mcon_cod="+config+";");
			return Vins;
			
		}

		[Ajax.AjaxMethod]
		public string ConstruirPrevioDinamico(string configConsulta)
		{
			return CreaTabla(configConsulta);
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
			this.btnReconfigurar.Click += new System.EventHandler(this.btnReconfigurar_Click);
			this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
			this.btnModificar.Click += new System.EventHandler(this.Button1_Click);
			this.Button2.Click += new System.EventHandler(this.Button2_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Reconfigurar configuracion
		private void btnReconfigurar_Click(object sender, System.EventArgs e)
		{
			int filas=0,columnas=0;
			//Validaciones
			//Nueva configuracion
			if(ddlConfiguracion.SelectedValue.Length>0)
			{
				try
				{
					filas=int.Parse(txtFilas.Text);
					columnas=int.Parse(txtColumnas.Text);
					if(filas<1 || columnas<1)
						throw(new Exception());
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar un número de filas y columnas válido para reconfigurar.");
					return;
				}
			}
			else
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar una configuracion.");
				return;
			}
			txtFilas.Text=filas.ToString();
			txtColumnas.Text=columnas.ToString();
			txtFilas.Enabled=false;
			txtColumnas.Enabled=false;
			btnVolver.Visible=true;
			btnReconfigurar.Visible=false;
			btnModificar.Visible=false;
			ddlConfiguracion.Enabled=false;
			CounstruirMatriz(filas,columnas,false);
		}

		//Configurar bus
		private void Button1_Click(object sender, System.EventArgs e)
		{
			int filas=0,columnas=0;
			string config=ddlConfiguracion.SelectedValue.Trim();
			//Validaciones
			//Nueva configuracion
			if(config.Length>0){
				try{
					filas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_filas from dbxschema.MCONFIGURACIONBUS where MCON_COD="+config));
					columnas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_columnas from dbxschema.MCONFIGURACIONBUS where MCON_COD="+config));
					if(filas<1 || columnas<1)
						throw(new Exception());
				}
				catch{
                    Utils.MostrarAlerta(Response, "Debe dar un número de filas y columnas válido para reconfigurar.");
					return;
				}
			}
			else{
                Utils.MostrarAlerta(Response, "Debe seleccionar una configuracion.");
				return;
			}
			txtFilas.Text=filas.ToString();
			txtColumnas.Text=columnas.ToString();
			txtFilas.Enabled=false;
			txtColumnas.Enabled=false;
			btnVolver.Visible=true;
			btnReconfigurar.Visible=false;
			btnModificar.Visible=false;
			ddlConfiguracion.Enabled=false;
			CounstruirMatriz(filas,columnas,true);
		}

		//Previsualizacion
		protected string CreaTabla(string config){
			string orden = string.Empty;
			try{
				Table prueba=new Table(); 
				int filas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_filas from dbxschema.MCONFIGURACIONBUS where MCON_COD="+config));
				int columnas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_columnas from dbxschema.MCONFIGURACIONBUS where MCON_COD="+config));
				if(filas<1 || columnas<1){
					return("Número de filas o columnas no válido, debe reconfigurar.");
				}
				for(int i=0;i<filas;i++){
					prueba.CellPadding=2;
					prueba.CellSpacing=4;
					prueba.BorderStyle=0;
					prueba.BorderWidth=3;
					prueba.BackColor=Color.White;
				
					TableRow Tablar = new TableRow();
					for(int j=0;j<columnas;j++){
						TableCell Tablac = new TableCell(); 
						Tablac.Height=35;
						Tablac.Width=20;
						Tablac.BorderWidth=3;
						string imagen=Convert.ToString(DBFunctions.SingleData("select tele_codigo from dbxschema.DCONFIGURACIONBUS where mcon_cod="+config+" and mpue_posfila="+i+" and mpue_poscolumna="+j+""));
						if(imagen.Equals("SP")){
							Tablac.Text="<div id='zone"+"_"+ i.ToString()+"_"+j.ToString()+"' style='background:#FFFFFF;height:100%;width:100%'><div id='puesto_"+i+"_"+j+"' style='background:#FFFFFF'><img src='../img/silla1.jpg'/></div></div>"; 
							Tablar.Cells.Add(Tablac);
						}
						else{
							if(imagen.Equals("BN")){
								Tablac.Text="<div id='zone"+"_"+ i.ToString()+"_"+j.ToString()+"' style='background:#FFFFFF;height:100%;width:100%'><div id='puesto_"+i+"_"+j+"' style='background:#FFFFFF'><img src='../img/baños.jpg'/></div></div>"; 
								Tablar.Cells.Add(Tablac);
							}
							else{
								if(imagen.Equals("SC")){
									Tablac.Text="<div id='zone"+"_"+ i.ToString()+"_"+j.ToString()+"' style='background:#FFFFFF;height:100%;width:100%'><div id='puesto_"+i+"_"+j+"' style='background:#FFFFFF'><img src='../img/silla4.jpg'/></div></div>"; 
									Tablar.Cells.Add(Tablac);
								}
								else{
									Tablac.Text="<div id='zone"+"_"+ i.ToString()+"_"+j.ToString()+"' style='background:#FFFFFF;height:100%;width:100%'></div>"; 
									Tablar.Cells.Add(Tablac);
								}
							}
						}
					}
					prueba.Rows.Add(Tablar);
				}
				StringBuilder SB= new StringBuilder();
				StringWriter SW= new StringWriter(SB);
				HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
				prueba.RenderControl(htmlTW);
				orden = SB.ToString();
			}
			catch(Exception ex){
				string strEx = ex.ToString();
			}
			return orden;
		}

		//Guardar modelo
		private void Button2_Click(object sender, System.EventArgs e){
			int filas=0,columnas=0;
			string config=ddlConfiguracion.SelectedValue;
			ArrayList sqlUpd=new ArrayList();
			filas=int.Parse(txtFilas.Text);
			columnas=int.Parse(txtColumnas.Text);
			if(filas<0||columnas<0)return;
			//Eliminar configuracion anterior
			sqlUpd.Add("delete from dbxschema.dconfiguracionbus where mcon_cod="+config+";");
			//Actualizar nueva configuracion
			sqlUpd.Add("update dbxschema.mconfiguracionbus set mbus_filas="+filas+", mbus_columnas="+columnas+" where mcon_cod="+config);
			string[,]x=new string[filas,columnas];
			string puesto,tp,tipo;
			int numepues=1;
			int invi,invj;
			for(int i=0;i<filas;i++){
				for(int j=0;j<columnas;j++){
					invi=filas-1-i;
					invj=j;
					x[invi,invj]=Request.Form["_ctl1:hdEstadoSilla_"+invi.ToString()+"_"+invj.ToString()];
					puesto="0";
					tp="PS";
					tipo = x[invi,invj];
					if(tipo.Equals("silla1") || tipo.Equals("Sil_2")){
						puesto=numepues.ToString();
						numepues++;
						tp="SP";}
					else if(tipo.Equals("baños") || tipo.Equals("Sil_3"))tp="BN";
						else if(tipo.Equals("silla4") || tipo.Equals("Sil_1"))tp="SC";
					sqlUpd.Add("insert into dbxschema.dconfiguracionbus values("+config+","+invi+","+invj+",'"+tp+"','"+puesto+"')");
				}
			}
			if(!DBFunctions.Transaction(sqlUpd)){
				lblError.Text=DBFunctions.exceptions;
				return;}
			Panel2.Visible=false;
			Response.Redirect("" + indexPage + "?process=Comercial.ConfiguracionBus&path="+Request.QueryString["path"]+"&act=1");
		}		


		//Registrar region arrastrar-soltar
		private void RegistrarRegion(tipoElemento tipoR, string fila, string columna, string color, string imagen, ref string script, ref TableRow tr, int ancho, int alto, int borde, string codigo, ref PlaceHolder plcHolder)
		{
			//int tipo=(int)tipoR;
			HtmlInputHidden hd = new HtmlInputHidden();
			TableCell tcimg = new TableCell(); 
			tcimg.Height=27;
			tcimg.Width=20;
			tcimg.BorderWidth=borde;
			tcimg.Text="";
			tcimg.BackColor=Color.White;
			switch(tipoR)
			{
				case tipoElemento.Caneca:
					tcimg.Text+="<div id='silla_"+codigo+"_"+0+"' style='background:#"+color+";height:"+alto+"px;width:"+ancho+"px'><img src='../img/"+imagen+"'/></div>";
					tcimg.ID="tdContainer_"+codigo;
					script+="dndMgr.registerDropZone(new Rico.Dropzone('silla_"+codigo+"_"+0+"'));";
				break;
				case tipoElemento.Elemento:
					
					tcimg.Text+="<div id='droponme"+"_"+fila+"_"+columna+"' style='background:#"+color+";height:"+alto+"px;width:"+ancho+"px'>";
					tcimg.Text+="<div id='Sil_"+fila+"_"+columna+"' style='background:#"+color+"'>";
					tcimg.Text+="<img src='../img/"+imagen+"'/></div></div>";
					tcimg.ID="tdContainer_"+fila+"_"+columna;
					script+="dndMgr.registerDropZone(new Rico.Dropzone('droponme"+"_"+fila+"_"+columna+"'));";
					script+="dndMgr.registerDraggable(new Rico.Draggable('"+tcimg.ClientID+"','Sil_"+fila+"_"+columna+"'));";
                    hd.ID = "hdEstadoSilla_"+fila+"_"+columna;
					hd.Value="Sil_"+codigo;
					plcHolder.Controls.Add(hd);
				break;
				case tipoElemento.Herramienta:
					tcimg.Text+="<div id='silla_"+codigo+"_0' style='background:#"+color+";height:"+alto+"px;width:"+ancho+"px'><img src='../img/"+imagen+"'/></div>";
					tcimg.ID="tdContainer_"+codigo;
					script+="dndMgr.registerDraggable(new Rico.Draggable('"+tcimg.ClientID+"','silla_"+codigo+"_0'));";
				break;
				case tipoElemento.Vacio:
					tcimg.ID="tdContainer_"+fila+"_"+columna;
					tcimg.Text+="<div id='droponme"+"_"+fila+"_"+columna+"' style='background:#"+color+";height:"+alto+"px;width:"+ancho+"px'></div>"; 
					script+="dndMgr.registerDropZone(new Rico.Dropzone('droponme"+"_"+fila+"_"+columna+"'));";
					hd.ID = "hdEstadoSilla_"+fila+"_"+columna;
					hd.Value="";
					plcHolder.Controls.Add(hd);
				break;
			}
			tr.Cells.Add(tcimg);
		}
		
		//Atras
		private void btnVolver_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Comercial.ConfiguracionBus&path="+Request.QueryString["path"]);
		}

		//Construir matriz
		void CounstruirMatriz(int filas, int columnas, bool cargar)
		{
			string registro;
			if(filas<1||columnas<1)
				cargar=false;
			registro="<script>";
			TableRow trimg = new TableRow();
			PlaceHolder dmHerramientas=new PlaceHolder();
			tbimg.BorderWidth=3;
			tbimg.BackColor=Color.White;
			//Registrar herramientas de diseño
				//Conductor
			RegistrarRegion(tipoElemento.Herramienta,"0","0","FFFFFF","silla4.jpg",ref registro,ref trimg,20,27,3,"1",ref dmHerramientas);
			//Pasajero
			RegistrarRegion(tipoElemento.Herramienta,"0","0","FFFFFF","silla1.jpg",ref registro,ref trimg,20,27,3,"2",ref dmHerramientas);
			//Baños
			RegistrarRegion(tipoElemento.Herramienta,"0","0","FFFFFF","baños.jpg",ref registro,ref trimg,20,27,3,"3",ref dmHerramientas);
			//Caneca
			RegistrarRegion(tipoElemento.Caneca,"0","0","FFFFFF","caneca.jpg",ref registro,ref trimg,20,27,3,"4",ref dmHerramientas);
			tbimg.Rows.Add(trimg);
			PlaceHolder2.Controls.Add(tbimg);

			//Manual
			if(!cargar){
				tb.CellPadding=2;
				tb.CellSpacing=4;
				tb.BorderStyle=0;
				tb.BorderWidth=3;
				tb.BackColor=Color.White;
				for(int i=0;i<filas;i++){
					TableRow tr = new TableRow();
					for(int j=0;j<columnas;j++){
						RegistrarRegion(tipoElemento.Vacio,i.ToString(),j.ToString(),"FFFFFF","",ref registro,ref tr,20,27,3,"",ref PlaceHolder3);
					}
					tb.Rows.Add(tr);
				}
				PlaceHolder1.Controls.Add(tb);
				registro +="</script>";
				Literal1.Text=registro;
				Panel2.Visible=true;
				Image2.Visible=true;
			}
			else
			{
				PlaceHolder2.Controls.Add(tbimg);
				for(int i=0;i<filas;i++)
				{
					tb.CellPadding=2;
					tb.CellSpacing=4;
					tb.BorderStyle=0;
					tb.BorderWidth=3;
					tb.BackColor=Color.White;
					TableRow tr = new TableRow();
					for(int j=0;j<columnas;j++)
					{
						string imagen=Convert.ToString(DBFunctions.SingleData("select tele_codigo from dbxschema.dconfiguracionbus where mcon_cod="+ddlConfiguracion.SelectedValue+" and mpue_posfila="+i+" and mpue_poscolumna="+j+""));
						if(imagen.Equals("SP"))
							RegistrarRegion(tipoElemento.Elemento,i.ToString(),j.ToString(),"FFFFFF","silla1.jpg",ref registro,ref tr,20,27,3,"2",ref PlaceHolder3);
						else if(imagen.Equals("BN"))
							RegistrarRegion(tipoElemento.Elemento,i.ToString(),j.ToString(),"FFFFFF","baños.jpg",ref registro,ref tr,20,27,3,"3",ref PlaceHolder3);
						else if(imagen.Equals("SC"))
							RegistrarRegion(tipoElemento.Elemento,i.ToString(),j.ToString(),"FFFFFF","silla4.jpg",ref registro,ref tr,20,27,3,"1",ref PlaceHolder3);
						else
							RegistrarRegion(tipoElemento.Vacio,i.ToString(),j.ToString(),"FFFFFF","",ref registro,ref tr,20,27,3,"",ref PlaceHolder3);
					}
					tb.Rows.Add(tr);
				}
				PlaceHolder1.Controls.Add(tb);
				registro +="</script>";
	
				Panel2.Visible=true;
				Image2.Visible=true;
			}
			Literal1.Text=registro;
		}
	}

}
