using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Reportes;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class SolicitarPedido : System.Web.UI.UserControl
	{
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
		protected DataSet ds;
		protected Table tabPreHeader,tabFirmas;
		protected string reportTitle="Formato de Pedido";	
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlNIT,"SELECT distinct t1.MNIT_NIT, t2.mnit_nombres concat ' ' concat t2.mnit_apellidos FROM MPEDIDOITEM as t1, MPROVEEDOR t3, mnit as t2 where t1.mnit_nit=t2.mnit_nit and t3.mnit_nit=t1.mnit_nit");
				bind.PutDatasIntoDropDownList(ddlPeds,"SELECT pped_codigo concat ' - ' concat cast(mped_numepedi as character(30)) FROM mpedidoitem WHERE mnit_nit='"+ddlNIT.SelectedValue+"' AND mped_claseregi='P'");
			}
			btnGuarda.Visible=false;
			toolsHolder.Visible=false;
			dgItems.Visible=false;
		}
		
		public void CambiaNIT(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();		
			bind.PutDatasIntoDropDownList(ddlPeds,"SELECT pped_codigo concat ' - ' concat cast(mped_numepedi as character(30)) FROM mpedidoitem WHERE mnit_nit='"+ddlNIT.SelectedValue+"' AND mped_claseregi='P'");
		}
		
		public void Reiniciar(object sender, System.EventArgs e)
		{
			btnGuarda.Visible = toolsHolder.Visible = btnReiniciar.Visible = false;
			ddlNIT.Enabled = ddlPeds.Enabled = btnCarga.Enabled = true;
			plDownload.Controls.Clear();
		}
		
		public void Cargar(object sender, System.EventArgs e)
		{
			plDownload.Controls.Clear();
			string preP = "", numP = "";
			string sa = ddlPeds.SelectedValue.ToString();
			preP = sa.Substring(0,sa.LastIndexOf("-")).Trim();
			numP = sa.ToString().Substring(sa.LastIndexOf("-")+1).Trim();
			ds = new DataSet();
			DBFunctions.Request(ds, IncludeSchema.NO,"select t1.MPED_CLASREGI as ped_tipo, t1.PPED_CODIGO as ped_ref, t1.MPED_NUMEPEDI as ped_num, DBXSCHEMA.EDITARREFERENCIAS(t1.mite_codigo,t4.plin_tipo) as mite_codigo, t2.mite_nombre as mite_nombre, t1.dped_cantpedi as mite_cant, t3.msal_costprom as mite_precio, t1.piva_porciva as mite_piva, (t1.dped_cantpedi*t3.msal_costprom) + (t1.piva_porciva*(t1.dped_cantpedi*t3.msal_costprom))/100 as mite_total from dpedidoitem as t1, mitems as t2, msaldoitem as t3, plineaitem as t4 where t1.mite_codigo=t2.mite_codigo and t3.mite_codigo=t2.mite_codigo and t1.pped_codigo='"+preP+"' and t1.mped_numepedi="+numP+" and t3.pano_ano="+DBFunctions.SingleData("SELECT pano_ano from cinventario")+" and t2.plin_codigo=t4.plin_codigo");
			//             										0                            1						   	2							3							        4								5								6							7										8                                                        
			int n;
			if(ds.Tables[0].Rows.Count<=0)
			{
                Utils.MostrarAlerta(Response, "No hay items en el pedido!");
				return;
			}
			double st=0,t=0,iva=0,at,ia;
			for(n=0;n<ds.Tables[0].Rows.Count;n++)
			{
				at=Convert.ToDouble(ds.Tables[0].Rows[n][5])*Convert.ToDouble(ds.Tables[0].Rows[n][6]);
				st+=at;
				ia=(Convert.ToDouble(ds.Tables[0].Rows[n][7])/100)*at;
				iva+=ia;
				t+=at+ia;
				ds.Tables[0].Rows[n][0]=(n+1).ToString();
			}
			DataRow nr = ds.Tables[0].NewRow();
			nr[0] = "Subtotal:";
			nr[8] = st;
			ds.Tables[0].Rows.Add(nr);
			nr = ds.Tables[0].NewRow();
			nr[0] = "IVA:";
			nr[8] = iva;
			ds.Tables[0].Rows.Add(nr);
			nr = ds.Tables[0].NewRow();
			nr[0] = "Total:";
			nr[8] = t;
			ds.Tables[0].Rows.Add(nr);
			dgItems.Visible = true;
			dgItems.DataSource = ds.Tables[0];
			dgItems.DataBind();
			DatasToControls.JustificacionGrilla(dgItems,ds.Tables[0]);
			btnGuarda.Visible = toolsHolder.Visible = btnReiniciar.Visible = true;
			ddlNIT.Enabled = ddlPeds.Enabled = btnCarga.Enabled = false;
    	    string []pr = new string[2];
			tabPreHeader = new Table();
			tabFirmas = new Table();
			pr[0] = "Nit: "+ddlNIT.SelectedItem.Text;
			pr[1] = DBFunctions.SingleData("SELECT t1.MNIT_DIRECCION concat ' - ' concat t2.pciu_nombre concat ' - ' concat t1.mnit_telefono FROM MNIT as t1, pciudad as t2 where t2.PCIU_CODIGO=t1.PCIU_CODIGO and MNIT_NIT='"+ddlNIT.SelectedValue+"'");
	       	Press frontEnd = new Press(new DataSet(), reportTitle);
			frontEnd.PreHeader(tabPreHeader, dgItems.Width, pr);
			frontEnd.Firmas(tabFirmas,dgItems.Width);
	       	StringBuilder SB = new StringBuilder();
    	   	StringWriter SW = new StringWriter(SB);
       		HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
	       	tabPreHeader.RenderControl(htmlTW);
    	   	dgItems.RenderControl(htmlTW);
       		tabFirmas.RenderControl(htmlTW);
			string strRep;
    	   	strRep = SB.ToString();
			Session.Clear();
			Session["Rep"] = strRep;		
		}
	
		//Guardar archivo
		protected void Guardar(object sender, System.EventArgs e)
		{
			Cargar(sender,e);
			plDownload.Controls.Clear();
			//Traemos el directorio donde se guardara el PathToDownloads
			string preP, numP;
			string nombreArchivo = "SolicitudPedido"+this.ddlNIT.SelectedValue+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
			preP = this.ddlPeds.SelectedValue.Substring(0,this.ddlPeds.SelectedValue.LastIndexOf("-")).Trim();
			numP = this.ddlPeds.SelectedValue.Substring(this.ddlPeds.SelectedValue.LastIndexOf("-")+1).Trim();
			string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];
			//Traemos los campos que componen el formato para este proveedor
			DataSet dsFormato = new DataSet();
			DBFunctions.Request(dsFormato,IncludeSchema.NO,"SELECT mfpi_opcdpedite, mfpi_secuencia FROM mformpeditemprov WHERE mnit_nit='"+ddlNIT.SelectedValue+"' ORDER BY mfpi_secuencia ASC");
			if(dsFormato.Tables[0].Rows.Count>0)
			{
				string whereMPI = " pped_codigo='"+preP+"' AND mped_numepedi="+numP+"";
				string whereMPR = " mnit_nit='"+ddlNIT.SelectedValue+"'";
				StreamWriter sw = File.CreateText(directorioArchivo+nombreArchivo);
				DataSet dsItems = new DataSet();
				DBFunctions.Request(dsItems,IncludeSchema.NO,"SELECT mite_codigo FROM dpedidoitem WHERE pped_codigo='"+preP+"' AND mped_numepedi="+numP+"");
				for(int i=0;i<dsItems.Tables[0].Rows.Count;i++)
				{
					string output = "";
					for(int j=0;j<dsFormato.Tables[0].Rows.Count;j++)
					{
						if(dsFormato.Tables[0].Rows[j][0].ToString().IndexOf('@') == -1)
						{
							if(dsFormato.Tables[0].Rows[j][0].ToString() == "SEC")
								output += i.ToString() + "\t";
							else
								output += dsFormato.Tables[0].Rows[j][0].ToString() + "\t";
						}
						else
						{
							if(dsFormato.Tables[0].Rows[j][0].ToString().IndexOf("MPEDIDOITEM") != -1)
								output += DBFunctions.SingleData("SELECT "+(dsFormato.Tables[0].Rows[j][0].ToString().Split('@'))[0].Replace("*","'")+" FROM MPEDIDOITEM WHERE"+whereMPI) + "\t";
							else if(dsFormato.Tables[0].Rows[j][0].ToString().IndexOf("MPROVEEDOR") != -1)
								output += DBFunctions.SingleData("SELECT "+(dsFormato.Tables[0].Rows[j][0].ToString().Split('@'))[0].Replace("*","'")+" FROM MPROVEEDOR WHERE"+whereMPR) + "\t";
							else if(dsFormato.Tables[0].Rows[j][0].ToString().IndexOf("DPEDIDOITEM") != -1)
								output += DBFunctions.SingleData("SELECT "+(dsFormato.Tables[0].Rows[j][0].ToString().Split('@'))[0].Replace("*","'")+" FROM DPEDIDOITEM WHERE"+whereMPI+" AND mite_codigo='"+dsItems.Tables[0].Rows[i][0]+"'") + "\t";
							else if(dsFormato.Tables[0].Rows[j][0].ToString().IndexOf("MITEMS") != -1)
								output += DBFunctions.SingleData("SELECT "+(dsFormato.Tables[0].Rows[j][0].ToString().Split('@'))[0].Replace("*","'")+" FROM MITEMS WHERE mite_codigo='"+dsItems.Tables[0].Rows[i][0]+"'") + "\t";
						}
					}
					sw.WriteLine(output);
				}
				sw.Close();
				//Ahora creamos el link que nos permite ver o descargar el archivo
				HyperLink hplDwn = new HyperLink();
				hplDwn.Text = nombreArchivo;
				hplDwn.NavigateUrl = "../dwl/"+nombreArchivo+"";
				hplDwn.Target = "_blank";
				plDownload.Controls.Add(new LiteralControl("Archivo de Solicitud : "));
				plDownload.Controls.Add(hplDwn);
			}
			else
                Utils.MostrarAlerta(Response, "No Existe Un Formato Para Este Proveedor!");
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			Cargar(Sender,E);
            Utils.MostrarAlerta(Response, "" + Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, dgItems) + "");
		}
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.Load+=new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}

