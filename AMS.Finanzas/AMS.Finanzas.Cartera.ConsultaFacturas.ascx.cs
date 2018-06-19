using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.DBManager;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class ConsultaFacturas : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox TextBox2;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			
		}
		
		protected void rblFiltro_IndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(rblFiltro.SelectedValue=="N")
			{
				pnl1.Visible=true;
				//bind.PutDatasIntoDropDownList(ddl1,"SELECT DISTINCT mnit_nit FROM dbxschema.mfacturacliente ORDER BY mnit_nit");
                TextBox1.Attributes.Add("ondblclick", "ModalDialog(this,'SELECT DISTINCT  mf.mnit_nit,mn.mnit_nombres concat\\' \\'concat coalesce(mn.mnit_nombre2,\\'\\') concat\\' \\'concat mn.mnit_apellidos concat\\' \\'concat coalesce(mn.mnit_apellido2,\\'\\') AS NOMBRE FROM dbxschema.mnit mn,dbxschema.mfacturacliente mf where mf.mnit_nit=mn.mnit_nit', new Array(),1)");
				ddl1.Visible=false;
				TextBox1.Visible=true;
				lb1.Visible=true;
				lb1.Text="Escoja el nit : ";
				ddl2.Visible=lb2.Visible=false;
			}
			else if(rblFiltro.SelectedValue=="F")
			{
				pnl1.Visible=true;
            //  bind.PutDatasIntoDropDownList(ddl1, "SELECT DISTINCT p.pdoc_codigo FROM dbxschema.mfacturacliente m, pdocumento p where m.pdoc_codigo = p.pdoc_codigo ");
                bind.PutDatasIntoDropDownList(ddl1, "SELECT DISTINCT pdoc_codigo FROM dbxschema.mfacturacliente");
             //	bind.PutDatasIntoDropDownList(ddl2, "SELECT mfac_numedocu FROM dbxschema.mfacturacliente WHERE pdoc_codigo='"+this.ddl1.SelectedValue+"' AND tvig_vigencia <> 'C'  ORDER BY mfac_numedocu");
                bind.PutDatasIntoDropDownList(ddl2, "SELECT mfac_numedocu FROM dbxschema.mfacturacliente WHERE pdoc_codigo='" + this.ddl1.SelectedValue + "' ORDER BY mfac_numedocu");
                ddl1.Visible = true;
				lb1.Visible=true;
				TextBox1.Visible=false;
				ddl1.AutoPostBack=true;
				lb1.Text="Escoja el prefijo de la factura : ";
				lb2.Text="Escoja el número de la factura : ";
				ddl2.Visible=lb2.Visible=true;
			}
			btnBuscar.Visible=true;
		}
		
		protected void ddl1_IndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
		//	bind.PutDatasIntoDropDownList(ddl2,"SELECT mfac_numedocu FROM dbxschema.mfacturacliente WHERE pdoc_codigo='"+this.ddl1.SelectedValue+"' AND tvig_vigencia <> 'C'  ORDER BY mfac_numedocu");
            bind.PutDatasIntoDropDownList(ddl2, "SELECT mfac_numedocu FROM dbxschema.mfacturacliente WHERE pdoc_codigo='" + this.ddl1.SelectedValue + "' ORDER BY mfac_numedocu");
        }
		
		protected void btnBuscar_Click(object Sender,EventArgs e)
		{
			DataSet ds;
			if(rblFiltro.SelectedValue=="N")
			{
				string [] tb;
				tb=(TextBox1.Text.Split('-'));
				ds=new DataSet();
			
				DBFunctions.Request(ds,IncludeSchema.NO,
                @"SELECT MFAC.pdoc_codigo AS Prefijo,MFAC.mfac_numedocu AS Numero,MFAC.mnit_nit || ' - '|| NOMBRE AS Nit,CASE TDOC_tipodocu WHEN 'FC' THEN 'Factura' WHEN 'NC' THEN 'Nota (-)' END AS Tipo, mfac.mfac_factura concat ' - ' concat TVIG.tvig_nombvige AS Fecha_y_Vigencia,
	            replace(VARCHAR_FORMAT(MFAC.mfac_valofact + MFAC.mfac_valoiva +MFAC.mfac_valoflet + MFAC.mfac_valoivaflet-MFAC.mfac_valorete,'S999,999,999,999.99'),'+','') AS Valor,
	            replace(VARCHAR_FORMAT(MFAC.mfac_valoabon,'S999,999,999,999.99'),'+','')   AS Abonado,
	            MFAC.mfac_valofact + MFAC.mfac_valoiva + MFAC.mfac_valoflet + MFAC.mfac_valoivaflet - MFAC.mfac_valorete - MFAC.mfac_valoabon as SALDO 
	            FROM dbxschema.mfacturacliente MFAC,dbxschema.tvigencia TVIG,dbxschema.PDOCUMENTO AS PD,dbxschema.VMNIT AS VN 
	            WHERE MFAC.PDOC_CODIGO=PD.PDOC_CODIGO AND MFAC.tvig_vigencia=TVIG.tvig_vigencia AND MFAC.mnit_nit='" + tb[0].ToString() + @"' AND MFAC.mnit_nit= VN.MNIT_NIT
                ORDER BY MFAC.pdoc_codigo ;
	            ");
                dgBusqueda.DataSource=ds.Tables[0];
				dgBusqueda.DataBind();
			}
			else if(rblFiltro.SelectedValue=="F")
			{
				ds=new DataSet();
			//	DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MFAC.pdoc_codigo AS Prefijo,MFAC.mfac_numedocu AS Numero,MFAC.mnit_nit AS Nit,CASE TDOC_tipodocu WHEN 'FC' THEN 'Factura' WHEN 'NC' THEN 'Nota' END AS Tipo,TVIG.tvig_nombvige AS Vigencia,(MFAC.mfac_valofact + MFAC.mfac_valoiva + MFAC.mfac_valoflet + MFAC.mfac_valoivaflet - MFAC.mfac_valorete) AS Valor,MFAC.mfac_valoabon AS Abonado, (MFAC.mfac_valofact + MFAC.mfac_valoiva + MFAC.mfac_valoflet + MFAC.mfac_valoivaflet - MFAC.mfac_valorete - MFAC.mfac_valoabon) as SALDO FROM dbxschema.mfacturacliente MFAC, dbxschema.tvigencia TVIG, dbxschema.PDOCUMENTO AS PD WHERE MFAC.PDOC_CODIGO=PD.PDOC_CODIGO AND MFAC.tvig_vigencia=TVIG.tvig_vigencia AND MFAC.pdoc_codigo='"+this.ddl1.SelectedValue+"' AND mfac_numedocu="+this.ddl2.SelectedValue+"");
				DBFunctions.Request(ds,IncludeSchema.NO,
                @"SELECT MFAC.pdoc_codigo AS Prefijo,MFAC.mfac_numedocu AS Numero,MFAC.mnit_nit || ' - '|| NOMBRE AS Nit,CASE TDOC_tipodocu WHEN 'FC' THEN 'Factura' WHEN 'NC' THEN 'Nota' END AS Tipo, mfac.mfac_factura concat ' - ' concat TVIG.tvig_nombvige AS Fecha_y_Vigencia,
                replace(VARCHAR_FORMAT(MFAC.mfac_valofact + MFAC.mfac_valoiva + MFAC.mfac_valoflet + MFAC.mfac_valoivaflet - MFAC.mfac_valorete,'S999,999,999,999.99'),'+','') AS Valor,
                replace(VARCHAR_FORMAT(MFAC.mfac_valoabon,'S999,999,999,999.99'),'+','') AS Abonado, 
                MFAC.mfac_valofact + MFAC.mfac_valoiva + MFAC.mfac_valoflet + MFAC.mfac_valoivaflet - MFAC.mfac_valorete - MFAC.mfac_valoabon as SALDO 
                FROM dbxschema.mfacturacliente MFAC, dbxschema.tvigencia TVIG, dbxschema.PDOCUMENTO AS PD,dbxschema.VMNIT AS VN 
                WHERE MFAC.PDOC_CODIGO=PD.PDOC_CODIGO AND MFAC.tvig_vigencia=TVIG.tvig_vigencia AND MFAC.pdoc_codigo='" + this.ddl1.SelectedValue + @"' AND mfac_numedocu=" + this.ddl2.SelectedValue + @" AND MFAC.mnit_nit= VN.MNIT_NIT;
                ");
                dgBusqueda.DataSource=ds.Tables[0];
				dgBusqueda.DataBind();
			}
		}

        protected void Show_Hide_ChildGrid(object sender, EventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                row.FindControl("pnlOrders").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "../img/minus.png";

                string prefijo = dgBusqueda.DataKeys[row.RowIndex].Values[0].ToString();
                string numero = dgBusqueda.DataKeys[row.RowIndex].Values[1].ToString();
                GridView gvOrders = row.FindControl("gvOrders") as GridView;
                gvOrders.ToolTip = prefijo;
                DataSet ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT m.pdoc_codigo concat ' - ' concat pd.pdoc_nombre AS Documento, mfac_numedocu AS Numero_Documento, pdoc_coddocref concat ' - ' concat p.pdoc_nombre AS Documento_Relacionado,
                		        ddet_numedocu || ' - de Fecha ' || coalesce(cast(mcaj_fecha as char(10)),'') AS Numero_Documento_Relacionado, replace(VARCHAR_FORMAT(ddet_valodocu,'S999,999,999,999.99'),'+','') AS Valor 
                		        FROM dbxschema.pdocumento p, dbxschema.pdocumento pd, dbxschema.ddetallefacturacliente m
                                     left join mcaja mc on m.pdoc_coddocref = mc.pdoc_codigo and m.ddet_numedocu = mc.mcaj_numero  
                		        WHERE p.pdoc_codigo = m.pdoc_coddocref and m.pdoc_codigo = pd.pdoc_codigo
                		        and m.pdoc_codigo  ='" + prefijo + @"' AND m.mfac_numedocu="+ numero +@"
                    UNION 
                  SELECT m.pdoc_codigo concat ' - ' concat pd.pdoc_nombre AS Documento, M.mCRU_numeRO AS Numero_Documento, MFAC_codIGO concat ' - ' concat p.pdoc_nombre AS Documento_Relacionado,
                		        MFAC_numeRO || ' - de Fecha ' || coalesce(cast(mcRU_fecha as char(10)),'') AS Numero_Documento_Relacionado, replace(VARCHAR_FORMAT(MCRU_valoR,'S999,999,999,999.99'),'+','') AS Valor 
                		        FROM dbxschema.pdocumento p, dbxschema.pdocumento pd, dbxschema.dcrucedocumento m
                                     left join mcRUCEDOCUMENTO mc on m.pdoc_codIGO = mc.pdoc_codigo and m.MCRU_numeRO = mc.mcRU_numero  
                		        WHERE p.pdoc_codigo = m.MFAC_codIGO and m.pdoc_codigo = pd.pdoc_codigo
                		        and m.MFAC_codigo  ='" + prefijo + "' AND m.mfac_numeRO="+ numero +" ");

                gvOrders.DataSource = ds.Tables[0];
                gvOrders.DataBind();
            }
            else
            {
                row.FindControl("pnlOrders").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "../img/plus.png";
            }
        }

		protected void dgBusqueda_Item(object Sender,DataGridCommandEventArgs e)
		{
//            DataSet ds;
//            if(e.CommandName=="ver")
//            {
//                ds=new DataSet();
//                DBFunctions.Request(ds, IncludeSchema.NO,
//                @"SELECT m.pdoc_codigo concat ' - ' concat pd.pdoc_nombre AS Documento, mfac_numedocu AS Numero_Documento, pdoc_coddocref concat ' - ' concat p.pdoc_nombre AS Documento_Relacionado,
//		        ddet_numedocu || ' - de Fecha ' || coalesce(cast(mcaj_fecha as char(10)),'') AS Numero_Documento_Relacionado, replace(VARCHAR_FORMAT(ddet_valodocu,'S999,999,999,999.99'),'+','') AS Valor 
//		        FROM dbxschema.pdocumento p, dbxschema.pdocumento pd, dbxschema.ddetallefacturacliente m
//                     left join mcaja mc on m.pdoc_coddocref = mc.pdoc_codigo and m.ddet_numedocu = mc.mcaj_numero  
//		        WHERE p.pdoc_codigo = m.pdoc_coddocref and m.pdoc_codigo = pd.pdoc_codigo
//		        and m.pdoc_codigo  ='" + dgBusqueda.Items[e.Item.DataSetIndex].Cells[1].Text + @"' 
//                AND m.mfac_numedocu=" + dgBusqueda.Items[e.Item.DataSetIndex].Cells[2].Text + " ");
//                if(ds.Tables[0].Rows.Count==0)
//                    Utils.MostrarAlerta(Response, "No se Encuentran Abonos para esta Consulta");
//                else
//                {
//                    dgAbonos.DataSource=ds.Tables[0];
//                    dgAbonos.DataBind();
//                }
//            }
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
