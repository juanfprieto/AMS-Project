using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class BusquedaOrden : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				//bind.PutDatasIntoDropDownList(ddlPrefijo,"SELECT DISTINCT P.pdoc_codigo,P.pdoc_descripcion FROM dbxschema.pdocumento P,dbxschema.morden M WHERE P.pdoc_codigo=M.pdoc_codigo AND P.tdoc_tipodocu='OT' AND M.test_estado='F'");
                Utils.llenarPrefijos(Response, ref ddlPrefijo , "%", "%", "OT");
				bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mord_numeorde FROM morden WHERE pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"' AND test_estado='F'");
			}
		}
		
		protected void ddlPrefijo_IndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mord_numeorde FROM morden WHERE pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"'");
		}
		
		protected void btnAceptar_Click(object Sender,EventArgs e)
		{
			DataSet ds,ds1;
			double totalFacturas=0,totalNotas=0,cartera=0;
			if(ddlNumero.Items.Count!=0)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mfac_tipodocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante FROM dbxschema.mfacturacliente WHERE mnit_nit=(SELECT mnit_nit FROM dbxschema.morden WHERE pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"' AND mord_numeorde="+this.ddlNumero.SelectedValue+") AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 ");
				if(ds.Tables[0].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						if(ds.Tables[0].Rows[i][2].ToString()=="F")
							totalFacturas=totalFacturas+Convert.ToDouble(ds.Tables[0].Rows[i][3]);
						else if(ds.Tables[0].Rows[i][2].ToString()=="N")
							totalNotas=totalNotas+Convert.ToDouble(ds.Tables[0].Rows[i][3]);
					}
					cartera=totalFacturas-totalNotas;
					if(cartera>0)
					{
						Session["error"]=ds.Tables[0];
						ds1=new DataSet();
						DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"' AND MORD.mord_numeorde="+this.ddlNumero.SelectedValue+" AND MORD.test_estado='F'");
						Session["ds"]=ds1;
						Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=OT&error=V");
					}
					else if(cartera<=0)
					{
						ds1=new DataSet();
                        //DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"' AND MORD.mord_numeorde="+this.ddlNumero.SelectedValue+" AND MORD.test_estado='F'");
                        DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin, MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion,MORD.PDOC_CODIGO,MORD.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo = MCAT.pcat_codigo --AND   MCAT.mnit_nit = MNIT.mnit_nit AND   PCOL.pcol_codigo = MCAT.pcol_codigo AND   MORD.mnit_nit = MNIT.mnit_nit AND   MORD.MCAT_VIN = MCAT.MCAT_VIN AND   MORD.pdoc_codigo = '" + this.ddlPrefijo.SelectedValue + "' AND   MORD.mord_numeorde = " + this.ddlNumero.SelectedValue + " AND  MORD.test_estado = 'F'; ");
                        Session["ds"]=ds1;
						Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=OT");
					}
				}
				else
				{
					ds1=new DataSet();
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"' AND MORD.mord_numeorde="+this.ddlNumero.SelectedValue+" AND MORD.test_estado='F'");
					Session["ds"]=ds1;
					Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=OT");
				}
			}
			else
            Utils.MostrarAlerta(Response, "No hay ordenes de trabajo con ese prefijo");
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
