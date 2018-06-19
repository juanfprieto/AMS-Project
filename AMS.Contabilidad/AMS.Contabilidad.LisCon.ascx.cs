using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.Reportes;
using AMS.DB;
using System.Data.Odbc;
using AMS.Tools;

namespace AMS.Contabilidad{
public partial class LisCon:System.Web.UI.UserControl
{
    //protected System.Web.UI.WebControls.RadioButton RadioOpcion;
	protected string reportTitle="Lista Consecutivo";
    protected string proceso;
    protected DataSet ds = new DataSet();
	protected void Page_Load(object sender, System.EventArgs e)
    {  
        this.ClearChildViewState();

        //llama el modo 
        proceso = Request.QueryString["proceso"];
        
		if(!IsPostBack)
        {  
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList( year, "SELECT pano_ano FROM pano where PANO_ANO IN (SELECT DISTINCT PANO_ANO FROM MSaldoCuenta) ORDER BY 1 DESC");
            bind.PutDatasIntoDropDownList(month, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 0 AND pmes_mes != 13 ORDER BY 1");
            if (proceso == "Tesoreria")
            {
                bind.PutDatasIntoDropDownList(typeDoc, "SELECT pdoc_codigo, pdoc_nombre CONCAT ' - ' CONCAT pdoc_codigo FROM pdocumento where tdoc_tipodocu in ('AT','CB','CS','DF','DE',EC','EN','ND','NR','RF','TC') order by 2");
            }
            else
                bind.PutDatasIntoDropDownList(typeDoc, "SELECT pdoc_codigo, pdoc_codigo CONCAT ' - ' CONCAT  pdoc_nombre FROM pdocumento order by 1");
            if (typeDoc.Items.Count > 1)
                typeDoc.Items.Insert(0, "Seleccione:..");
        }
	}

	protected void Report_ItemDataBound(object sender, DataGridItemEventArgs e){
 		if(e.Item.ItemType != ListItemType.Header){
 			e.Item.Cells[6].HorizontalAlign = HorizontalAlign.Right;
 		}
 	}

    protected void opcion_cambio(object sender, System.EventArgs e)
    {
        lblTipoComp.Visible = typeDoc.Visible = (RadioOpcion.SelectedValue != "TC");
        if (RadioOpcion.SelectedValue == "TC")
        {
            lblTipoComp.Visible = false;
            typeDoc.Visible = false;
        }
    }

	protected void Consulta_Click(object  Sender ,  EventArgs e){
        DataSet ds =new DataSet();
        string []pr=new string[2];
		pr[0]=pr[1]="";
		Press frontEnd = new Press(new DataSet(), reportTitle);
		frontEnd.PreHeader(tabPreHeader, dg.Width, pr);
		frontEnd.Firmas(tabFirmas,dg.Width);

        string sql = @"select m.PDOC_CODIGO, m.MCOM_NUMEDOCU, cast(MCOM_FECHA as char(10)), MCOM_RAZON, cast(MCOM_FECHAPROC as char(10)), MCOM_USUARIO,  
                   coalesce(sum(dcue_VALOdebi),123456789) as total_debito, coalesce(sum(dcue_VALOcred),987654321) as total_credito,  
                   coalesce(sum(dcue_VALOdebi),123456789) - coalesce(sum(dcue_VALOcred),987654321) as DIFERENCIA, 
                   coalesce(sum(dcue_NIIFdebi),123456789) as total_debito_NIIF, coalesce(sum(dcue_NIIFcred),987654321) as total_credito_NIIF, 
                   coalesce(sum(dcue_NIIFdebi),123456789) - coalesce(sum(dcue_NIIFcred),987654321) as DIFERENCIA_NIIF 
                    from MCOMPROBANTE m left join dcuenta d on m.pdoc_CODIGO = D.pdoc_CODIGO AND M.MCOM_NUMEDOCU = D.MCOM_NUMEDOCU  
                   where PMES_MES = "+ month.SelectedValue +" AND PANO_ANO ="+ year.SelectedValue +" ";
           
     //              0             1            2             3            4                5         6
        CultureInfo bz = new CultureInfo("en-us");
		if (RadioOpcion.SelectedValue=="UTC")
		   	sql += " AND M.PDOC_CODIGO = '" + typeDoc.SelectedValue + "' ";
		sql+=" GROUP BY m.PDOC_CODIGO, m.MCOM_NUMEDOCU, MCOM_FECHA, MCOM_RAZON, MCOM_FECHAPROC, MCOM_USUARIO "+
             " ORDER BY M.PDOC_CODIGO, M.MCOM_NUMEDOCU";
        ViewState["CONSULTAExcel"] = sql; 
//      hola.Text+=sql+"<br>";
        DBFunctions.Request(ds, IncludeSchema.NO,sql);
        string[] titleFields = { "Cod Doc.", "Nº Comprobante", "Fecha", "Razón", "Fecha Proc.", "Usuario", "Valor Débito", "Valor Crédito", "Vr Diferencia", "Valor Débito NIIF", "Valor Crédito NIIF", "Vr Diferencia NIIF" };
        string[] dataFields  = { "Cod Doc.", "Nº Comprobante", "Fecha", "Razón", "Fecha Proc.", "Usuario", "Valor Débito", "Valor Crédito", "Vr Diferencia", "Valor Débito NIIF", "Valor Crédito NIIF", "Vr Diferencia NIIF" };
        int i;
        for(i=0;i<titleFields.Length;i++)
        {
            BoundColumn bc = new BoundColumn();
            bc.HeaderText  = titleFields[i];
            bc.DataField   = dataFields[i];
            bc.DataFormatString = "{0:N}";
            dg.Columns.Add(bc);
        }
        DataTable dtRep = new DataTable();
        DataRow dr;
        for(i=0;i<titleFields.Length;i++)
            dtRep.Columns.Add(new DataColumn(dataFields[i],typeof(string)));
        string ca="0";
		int nd=0,fal=0;
		//JFSC 11022008 Poner en comentario por no ser usado:
		//ct=0,
		double pra=0,prt=0;
        if(ds.Tables[0].Rows.Count>0)
        {
            ca=ds.Tables[0].Rows[0].ItemArray[0].ToString();
        	pra=Convert.ToDouble(ds.Tables[0].Rows[0].ItemArray[1]);
            for(i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                prt=Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[1]);
                if (ca!=ds.Tables[0].Rows[i].ItemArray[0].ToString())
                {
                    ca = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                    pra = Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[1]);
                }
            	if(prt-pra>1)
                { 
                    fal++;
					dr=dtRep.NewRow();
            		if(prt-pra>2)
                    {
	                	dr[0]=(pra+1).ToString()+" - "+(prt-1).ToString();
            			dr[1]="FALTAN!";
                    }
            		else
                    {
	                	dr[0]=(pra+1).ToString();
            			dr[1]="FALTA!";            				
            		}
                    dtRep.Rows.Add(dr);
                }
                nd++;
                pra=prt;
                dr=dtRep.NewRow();
                dr[0] = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                dr[1] = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                dr[2] = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                dr[3] = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                dr[4] = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                dr[5] = ds.Tables[0].Rows[i].ItemArray[5].ToString();
                dr[6] = ((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[6])).ToString("N");
                dr[7] = ((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[7])).ToString("N");
                dr[8] = ((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[8])).ToString("N");
                dr[9] = ((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[9])).ToString("N");
                dr[10] = ((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[10])).ToString("N");
                dr[11] = ((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[11])).ToString("N");
                dtRep.Rows.Add(dr);
            }
            dr=dtRep.NewRow();
            dr[0] = "TOTALES:";
            dr[1] = nd.ToString();
			dr[2] = "Reg.Faltantes";
			dr[3] = fal.ToString();
            dtRep.Rows.Add(dr);
        }
        dg.DataSource = dtRep;
        dg.DataBind();
        StringBuilder SB = new StringBuilder();
        StringWriter  SW = new StringWriter(SB);
        HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
        tabPreHeader.RenderControl(htmlTW);
        dg.RenderControl(htmlTW);
        tabFirmas.RenderControl(htmlTW);
        string strRep;
        strRep  = SB.ToString();
		Session.Clear();
		Session["Rep"]=strRep;
		toolsHolder.Visible = true;
	}


    protected void ImprimirExcelGrid(Object Sender, EventArgs e)
    {
        try
        {
            ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTAExcel"].ToString());
            DateTime fecha = DateTime.Now;
            string nombreArchivo = "ConsultaTabla" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
            base.Response.Clear();
            base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
            base.Response.Charset = "Unicode";
            base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            base.Response.ContentType = "application/vnd.xls";
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            //ds = (DataSet)ViewState["dataGrid"];
            DataGrid dgAux = new DataGrid();
            dgAux.DataSource = ds;
            dgAux.DataBind();
            dgAux.RenderControl(htmlWrite);

            base.Response.Write(stringWrite.ToString());
            base.Response.End();
        }
        catch (Exception ex)
        {
            lblResult.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
            return;
        }
    }

	public void SendMail(Object Sender, ImageClickEventArgs E){
     	Consulta_Click(Sender, E);
     	Utils.MostrarAlerta(Response, Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, dg));
		dg.EnableViewState=false;
	}

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
