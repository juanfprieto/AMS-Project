namespace AMS.Comercial
{
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Globalization;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.Mail;
	using System.Web.UI;
	using System;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Comercial_PapeleriaAlmacen.
	/// </summary>
	public class AMS_Comercial_PapeleriaAlmacen : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.RadioButtonList RadioButtonList1;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.Button recepcionar;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Panel Panel3;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.TextBox fechaRe;
		protected System.Web.UI.WebControls.TextBox fechaRetorno;
		protected System.Web.UI.WebControls.DropDownList DocRece;
		protected System.Web.UI.WebControls.DropDownList DocRet;
		protected System.Web.UI.WebControls.TextBox IniSecRE;
		protected System.Web.UI.WebControls.TextBox FinSecRE;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.TextBox responsableRET;
		protected System.Web.UI.WebControls.DropDownList agenciaRET;
		protected System.Web.UI.WebControls.TextBox IniSecRET;
		protected System.Web.UI.WebControls.TextBox FinSecRET;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.TextBox fechaDes;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList agencia;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList despacho;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label UltimaSecuencia;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox IniDes;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox FinDes;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox Responsable;
		protected System.Web.UI.WebControls.Button despachar;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label SecDispon;
		protected System.Web.UI.WebControls.Label Label1;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				
				
			}
		}

		
		public void Despachar_OnClick(object sender, System.EventArgs e)
		{
			//envio de papeleria
			int Inicio=Convert.ToInt32(IniDes.Text.ToString());
			int Fin =Convert.ToInt32(FinDes.Text.ToString());
			int FinSec=Convert.ToInt32(DBFunctions.SingleData("select MPAL_FINSEC from DBXSCHEMA.MPAPELERIA_ALMACEN WHERE TTIPO_CODIGO="+despacho.SelectedValue.ToString()+""));
			int InSec=Convert.ToInt32(DBFunctions.SingleData("select MPAL_INISEC from DBXSCHEMA.MPAPELERIA_ALMACEN WHERE TTIPO_CODIGO="+despacho.SelectedValue.ToString()+""));
			string []UltimSe=DBFunctions.SingleData("select MPAL_ULTIMSEC from DBXSCHEMA.MPAPELERIA_ALMACEN WHERE TTIPO_CODIGO="+despacho.SelectedValue.ToString()+"").Split('-');
			int UltimSecIni=Convert.ToInt32(UltimSe[0]);
			int UltimSecFin=Convert.ToInt32(UltimSe[1]);

			if(UltimSe.Equals("0-0"))
			{
			}
			else
			{
				if((Fin > FinSec || Inicio < InSec) || (Inicio < UltimSecFin )|| (Fin < UltimSecIni || Fin < UltimSecFin  ))
				{
                    Utils.MostrarAlerta(Response, "  Secuencia Invalida");

				}
				else
				{
					
					string SecuenciaInsercion=IniDes.Text.ToString()+"-"+FinDes.Text.ToString();
					DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MPAPEL_AGENCIA VALUES(DEFAULT,'"+fechaDes.Text.ToString()+"','"+agencia.SelectedValue.ToString()+"',"+despacho.SelectedValue.ToString()+",'"+SecuenciaInsercion.ToString()+"','0-0',8)");
					DBFunctions.NonQuery("UPDATE DBXSCHEMA.MPAPELERIA_ALMACEN SET MPAL_ULTIMSEC='"+SecuenciaInsercion.ToString()+"' WHERE TTIPO_CODIGO="+despacho.SelectedValue.ToString()+" ");
                    Utils.MostrarAlerta(Response, "  Recepcion Realizada Satisfactoriamente");
					Response.Redirect("" + indexPage + "?process=Comercial.PapeleriaAlmacen");
				}
			
			

			}
		}

		public void Recepcionar_OnClick(object sender, System.EventArgs e)
		{
			//recepcion de papeleria almacen
			string UltimaSecuenciaAlmacen=null;
			string Existe=DBFunctions.SingleData("select * from DBXSCHEMA.MPAPELERIA_ALMACEN where TTIPO_CODIGO="+DocRece.SelectedValue.ToString()+" ");
			if(Existe.Equals("") || Existe.Equals(null))
			{
				UltimaSecuenciaAlmacen="0-0";
				DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MPAPELERIA_ALMACEN VALUES(DEFAULT,'"+fechaRe.Text.ToString()+"',"+DocRece.SelectedValue.ToString()+","+IniSecRE.Text.ToString()+","+FinSecRE.Text.ToString()+",2,'"+UltimaSecuenciaAlmacen.ToString()+"')");
                Utils.MostrarAlerta(Response, "  Recepcion Realizada Satisfactoriamente");
			}
			else
			{
				DBFunctions.NonQuery("UPDATE DBXSCHEMA.MPAPELERIA_ALMACEN SET MPAL_FINSEC="+FinSecRE.Text.ToString()+" WHERE TTIPO_CODIGO="+DocRece.SelectedValue.ToString()+" ");
                Utils.MostrarAlerta(Response, "  Inventario Actualizado Satisfactoriamente");
				Response.Redirect("" + indexPage + "?process=Comercial.PapeleriaAlmacen");
			
			}
			
		}
		public void Recepcionar2_OnClick(object sender, System.EventArgs e)
		{
			//devolucion 
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MDEV_PAPEL VALUES(DEFAULT,'"+fechaRetorno.Text.ToString()+"','"+agenciaRET.SelectedValue.ToString()+"',"+DocRet.SelectedValue.ToString()+","+IniSecRET.Text.ToString()+","+FinSecRET.Text.ToString()+",'"+responsableRET.Text.ToString()+"')");
            Utils.MostrarAlerta(Response, "  Recepcion Realizada Satisfactoriamente");
			Response.Redirect("" + indexPage + "?process=Comercial.PapeleriaAlmacen");
		}
		private void RadioButtonList1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=  new DatasToControls();
			///despacho
			if(RadioButtonList1.SelectedItem.Value.Equals("1"))
			{
				Panel2.Visible=false;
				Panel3.Visible=false;
				Panel1.Visible=true;
				Panel2.Height=0;
				Panel2.Width=0;
				Panel3.Height=0;
				Panel3.Width=0;
				
				
				
					fechaDes.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
					int contOfi=0;
					if(contOfi==0)
					{
						bind.PutDatasIntoDropDownList(agencia,"Select MOFI_CODIGO,MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA");
						ListItem it=new ListItem("--Agencia--","0");
						agencia.Items.Insert(0,it);
					}
					int contDespacho=0;
					if(contDespacho==0)
					{
						bind.PutDatasIntoDropDownList(despacho,"Select TTIPO_CODIGO,TTIPO_DESCRIPCION from DBXSCHEMA.TTIPO_DESPACHO");
						ListItem it=new ListItem("--Despacho--","0");
						despacho.Items.Insert(0,it);

					
				}
			}
			////fin despacho
			////recepcion almacen
			if(RadioButtonList1.SelectedItem.Value.Equals("2"))
			{
				Panel1.Visible=false;
				Panel3.Visible=false;
				Panel1.Height=0;
				Panel1.Width=0;
				Panel3.Height=0;
				Panel3.Width=0;
				Panel2.Visible=true;
				fechaRe.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				int contDespacho=0;
				if(contDespacho==0)
				{
					bind.PutDatasIntoDropDownList(DocRece,"Select TTIPO_CODIGO,TTIPO_DESCRIPCION from DBXSCHEMA.TTIPO_DESPACHO");
					ListItem it=new ListItem("--Documento--","0");
					DocRece.Items.Insert(0,it);

				}

			}
			////fin recepcion almacen
			////recepcion devoluciones
			if(RadioButtonList1.SelectedItem.Value.Equals("3"))
			{
				Panel1.Visible=false;
				Panel2.Visible=false;
				Panel2.Height=0;
				Panel2.Width=0;
				Panel1.Height=0;
				Panel1.Width=0;
				Panel3.Visible=true;
				fechaRetorno.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				
				int contRet=0;
				if(contRet==0)
				{
					bind.PutDatasIntoDropDownList(DocRet,"Select TTIPO_CODIGO,TTIPO_DESCRIPCION from DBXSCHEMA.TTIPO_DESPACHO");
					ListItem it=new ListItem("--Documento--","0");
					DocRet.Items.Insert(0,it);

				}
				int contAgeRET=0;
				if(contAgeRET==0)
				{
					bind.PutDatasIntoDropDownList(agenciaRET,"Select MOFI_CODIGO,MOFI_DESCRIPCION from DBXSCHEMA.MOFICINA");
					ListItem it=new ListItem("--Agencia--","0");
					agenciaRET.Items.Insert(0,it);
				}

			
			}
		//fin recepcion devoluciones
		}

		private void agencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=  new DatasToControls();
			Responsable.Text=DBFunctions.SingleData("select MOFI_ENCARGADO from DBXSCHEMA.MOFICINA WHERE MOFI_CODIGO='"+agencia.SelectedValue.ToString()+"'").ToString();
		}

		private void agencia2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=  new DatasToControls();
			responsableRET.Text=DBFunctions.SingleData("select MOFI_ENCARGADO from DBXSCHEMA.MOFICINA WHERE MOFI_CODIGO='"+agenciaRET.SelectedValue.ToString()+"'").ToString();

		}
		public void despacho_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string UlSecuencia=DBFunctions.SingleData("select MPAL_ULTIMSEC from DBXSCHEMA.MPAPELERIA_ALMACEN WHERE TTIPO_CODIGO="+despacho.SelectedValue.ToString()+" ").ToString();
			string []SecDisponible=UlSecuencia.Split('-');
			if(UlSecuencia.Equals("0-0"))
			{
				IniDes.Text=DBFunctions.SingleData("select MPAL_INISEC from DBXSCHEMA.MPAPELERIA_ALMACEN WHERE TTIPO_CODIGO="+despacho.SelectedValue.ToString()+" ");
				FinDes.Text="0";
			}
			else
			{
				IniDes.Text=SecDisponible[1].ToString();
				FinDes.Text="0";
			}
			UltimaSecuencia.Text=UlSecuencia.ToString();
			string Secuencia=DBFunctions.SingleData("Select RTRIM(CAST(MPAL_INISEC AS CHARACTER(10)))CONCAT '-' CONCAT RTRIM(CAST(MPAL_FINSEC AS CHARACTER(10))) from DBXSCHEMA.MPAPELERIA_ALMACEN WHERE TTIPO_CODIGO="+despacho.SelectedValue.ToString()+"");
			SecDispon.Text=Secuencia.ToString();
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
			this.RadioButtonList1.SelectedIndexChanged += new System.EventHandler(this.RadioButtonList1_SelectedIndexChanged);
			this.agenciaRET.SelectedIndexChanged += new System.EventHandler(this.agencia2_SelectedIndexChanged);
			this.agencia.SelectedIndexChanged += new System.EventHandler(this.agencia_SelectedIndexChanged);
			this.despacho.SelectedIndexChanged += new System.EventHandler(this.despacho_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		
		

		
	}
}
