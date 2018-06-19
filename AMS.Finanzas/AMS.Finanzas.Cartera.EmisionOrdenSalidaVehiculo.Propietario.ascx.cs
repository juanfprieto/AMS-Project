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
	public partial  class BusquedaPropietario : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			
		}
		
		protected void btnAceptar_Click(object Sender,EventArgs e)
		{
			DataSet ds,ds1;
			double totalFacturas=0,totalNotas=0,cartera=0;
			if(DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='"+this.tbNit.Text+"'"))
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mfac_tipodocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante FROM dbxschema.mfacturacliente WHERE mnit_nit='"+this.tbNit.Text+"' AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 ");
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
						DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MNIT.mnit_nit='"+this.tbNit.Text+"'");
						DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MVEH.test_tipoesta=40 AND MNIT.mnit_nit='"+this.tbNit.Text+"'");
						Session["ds"]=ds1;
						Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=NP&error=V");
					}
					else if(cartera==0 || cartera<0)
					{
						ds1=new DataSet();
						DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MNIT.mnit_nit='"+this.tbNit.Text+"'");
						DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MVEH.test_tipoesta=40 AND MNIT.mnit_nit='"+this.tbNit.Text+"'");
						Session["ds"]=ds1;
						Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=NP");
					}
				}
				else
				{
					ds1=new DataSet();
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MNIT.mnit_nit='"+this.tbNit.Text+"'");
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MVEH.test_tipoesta=40 AND MNIT.mnit_nit='"+this.tbNit.Text+"'");
					Session["ds"]=ds1;
					Response.Redirect(indexPage+"?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=NP");
				}
			}
			else
            Utils.MostrarAlerta(Response, "No hay datos de ese nit, asegurese que esta correcto.");
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
