// created on 09/12/2004 at 15:35

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Timers;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class HojaVida : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string HabeasData = "";


        protected void Page_Load(object sender, System.EventArgs e)
		{
			lb.Text = "";
			if(!IsPostBack)
			{
                HabeasData = Request.QueryString["habeas"];
                ViewState["HabeasData"] = HabeasData;
                DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlColor,"SELECT pcol_codigo, pcol_descripcion from dbxschema.pcolor order by pcol_descripcion;");
				ddlColor.Items.Insert(0,new ListItem("--todos--",""));
				bind.PutDatasIntoDropDownList(ddlServicio,"SELECT TSER_TIPOSERV, TSER_NOMBSERV from dbxschema.TSERVICIOVEHICULO order by TSER_NOMBSERV;");
				ddlServicio.Items.Insert(0,new ListItem("--todos--",""));
                if (Request.QueryString["vinE"] != null)
                {
                    txtVin.Text = Request.QueryString["vinE"].ToString();
                    Consultar_Hoja(sender, e);
                }
			}
		}	
		
		protected  void Consultar_Hoja(Object  Sender, EventArgs e)
		{
			string sql="SELECT * FROM mcatalogovehiculo WHERE";
			int ano=0;
			double kiloV=-1,kiloU=-1,kiloP=-1;
			dgrCatalogos.Visible=false;
			//VALIDACIONES
			try{
				if(txtAno.Text.Length>0)
					ano=int.Parse(txtAno.Text);
            }
			catch{
                Utils.MostrarAlerta(Response, "Año del modelo no valido");
            }
			try{
				if(txtKilometrajeV.Text.Length>0)kiloV=double.Parse(txtKilometrajeV.Text);
				if(txtKilometrajeU.Text.Length>0)kiloU=double.Parse(txtKilometrajeU.Text);
				if(txtKilometrajeP.Text.Length>0)kiloP=double.Parse(txtKilometrajeP.Text);
			}
			catch{
                Utils.MostrarAlerta(Response, "Uno de los kilometrajes no es valido");
            }
			//Filtros
			if(txtVin.Text.Length>0)    sql+=" MCAT_VIN='"+txtVin.Text+"' AND";
			if(txtPlaca.Text.Length>0)  sql+=" MCAT_PLACA='"+txtPlaca.Text+"' AND";
			if(txtMotor.Text.Length>0)  sql+=" MCAT_MOTOR='"+txtMotor.Text+"' AND";
			if(txtNit.Text.Length>0)    sql+=" MNIT_NIT='"+txtNit.Text+"' AND";
			if(txtSerie.Text.Length>0)  sql+=" MCAT_SERIE='"+txtSerie.Text+"' AND";
			if(txtChasis.Text.Length>0) sql+=" MCAT_CHASIS='"+txtChasis.Text+"' AND";
			if(ddlColor.SelectedValue.Length>0)sql+=" PCOL_CODIGO='"+ddlColor.SelectedValue+"' AND";
			if(ano>0)                   sql+=" MCAT_ANOMODE="+ano+" AND";
			if(ddlServicio.SelectedValue.Length>0)sql+=" TSER_TIPOSERV='"+ddlServicio.SelectedValue+"' AND";
			if(txtSeguro.Text.Length>0) sql+=" MCAT_VENCSEGUOBLI='"+txtSeguro.Text+"' AND";
			if(txtConcesionario.Text.Length>0)sql+=" MCAT_CONCVEND='"+txtConcesionario.Text+"' AND";
			if(txtVenta.Text.Length>0)  sql+=" MCAT_VENTA='"+txtVenta.Text+"' AND";
			if(kiloV>-1)                sql+=" MCAT_NUMEKILOVENT="+kiloV+" AND";
			if(txtRadio.Text.Length>0)  sql+=" MCAT_NUMERORADIO='"+txtRadio.Text+"' AND";
			if(kiloU>-1)                sql+=" MCAT_NUMEULTIKILO="+kiloU+" AND";
			if(kiloP>-1)                sql+=" MCAT_NUMEKILOPROM="+kiloP+" AND";
			if(txtCategoria.Text.Length>0)sql+=" MCAT_CATEGORIA='"+txtCategoria.Text+"' AND";
			if(sql.EndsWith("AND"))     sql=sql.Substring(0,sql.Length-3);
			if(sql.EndsWith("WHERE"))
			{
                Utils.MostrarAlerta(Response, "No ingreso parametros de busqueda");
				return;
			}
			
			DataSet dsCatalogos=new DataSet();
			DBFunctions.Request(dsCatalogos,IncludeSchema.NO,sql);
			
			if(dsCatalogos.Tables[0].Rows.Count==0)
			{
                Utils.MostrarAlerta(Response, "No se encontraron resultados");
				return;
			}
            HabeasData = ViewState["HabeasData"].ToString() ;
            if (dsCatalogos.Tables[0].Rows.Count==1)
				Response.Redirect("" + indexPage + "?process=Automotriz.HojaVida.ConsultarHoja&placa="+dsCatalogos.Tables[0].Rows[0]["MCAT_PLACA"].ToString()+"&vin=" + dsCatalogos.Tables[0].Rows[0]["MCAT_VIN"].ToString() + "&habeas=" + HabeasData + "");

			dgrCatalogos.DataSource=dsCatalogos.Tables[0];
			dgrCatalogos.DataBind();
			dgrCatalogos.Visible=true;

		}
		protected void dgTable_Procesos(object sender, DataGridCommandEventArgs e) 
		{
			if(e.CommandName == "Edit")
			{
                HabeasData = ViewState["HabeasData"].ToString();
                Response.Redirect("" + indexPage + "?process=Automotriz.HojaVida.ConsultarHoja&placa=" + dgrCatalogos.DataKeys[e.Item.ItemIndex].ToString() + "&habeas=" + HabeasData + "");
			}
		}
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    

		}
		#endregion

	}
}
