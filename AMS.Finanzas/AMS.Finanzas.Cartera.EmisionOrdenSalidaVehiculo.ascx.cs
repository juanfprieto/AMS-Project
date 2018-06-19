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
using Ajax;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial class EmisionOrdenSalida : System.Web.UI.UserControl
	{
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected string prefOT,numOT,cat,vin;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(EmisionOrdenSalida));
			if(!IsPostBack)
			{
				Session.Clear();
				if(Request["err"]!=null && Request["err"]=="1")
                    Utils.MostrarAlerta(Response, "La consulta no ha retornado información. Intente con otro filtro");
				else if(Request["err"]!=null && Request["err"]=="2")
                    Utils.MostrarAlerta(Response, "Este vehículo no tiene ordenes de trabajo asociadas y tampoco aparece como un vehículo nuevo. Verifique sus datos");
				else if(Request["err"]!=null && Request["err"]=="3")
                Utils.MostrarAlerta(Response, "Este vehículo no tiene ordenes de trabajo asociadas y tampoco aparece como un vehículo nuevo. Verifique sus datos");
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlCatalogo,"SELECT pcat_codigo,pcat_codigo ||' - '|| pcat_descripcion FROM pcatalogovehiculo ORDER BY  pcat_codigo");
				bind.PutDatasIntoDropDownList(ddlVIN,"SELECT mcat_vin FROM mcatalogovehiculo WHERE pcat_codigo='"+ddlCatalogo.SelectedValue+"' ORDER BY mcat_vin");
				//bind.PutDatasIntoDropDownList(ddlPrefijo,"SELECT DISTINCT P.pdoc_codigo,P.pdoc_descripcion FROM dbxschema.pdocumento P,dbxschema.morden M WHERE P.pdoc_codigo=M.pdoc_codigo AND P.tdoc_tipodocu='OT' AND M.test_estado='F'");
                Utils.llenarPrefijos(Response, ref ddlPrefijo  , "%", "%", "OT");
				bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mord_numeorde FROM morden WHERE pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"' AND test_estado='F' ORDER BY mord_numeorde");
			}
			prefOT=Request.Form[ddlPrefijo.UniqueID];
			numOT=Request.Form[ddlNumero.UniqueID];
			cat=Request.Form[ddlCatalogo.UniqueID];
			vin=Request.Form[ddlVIN.UniqueID];
		}

        protected void mostrarPlaceholder(object Sender, EventArgs e)
        {
            plhVinVehiculo.Visible = plhPlacaVehiculo.Visible = plhOrden.Visible = plhNitPropietario.Visible = false;

            if (rbCat.Checked) plhVinVehiculo.Visible = true;
            else if(rbPla.Checked) plhPlacaVehiculo.Visible = true;
            else if(rbOT.Checked) plhOrden.Visible = true;
            else plhNitPropietario.Visible = true;
        }

        protected void Ejecutar_Proceso(object Sender,EventArgs e)
		{
			if(((Button)Sender).CommandName=="CV")
			{
				CalcularCarteraCatalogo(cat,vin);
			}
			else if(((Button)Sender).CommandName=="PV")
			{
				CalcularCarteraPlaca(tbPlaca.Text);
			}
			else if(((Button)Sender).CommandName=="OT")
			{
				CalcularCarteraOrden(prefOT,numOT);
			}
			else if(((Button)Sender).CommandName=="NP")
			{
				CalcularCarteraNit(tbNit.Text);
			}
		}

		[Ajax.AjaxMethod]
		public DataSet CambiarCatalogo(string cat)
		{
			DataSet ds=new DataSet();
			return DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mcat_vin AS VIN FROM mcatalogovehiculo WHERE pcat_codigo='"+cat+"' ORDER BY mcat_vin");
		}

		[Ajax.AjaxMethod]
		public DataSet CambiarOrden(string pref)
		{
			DataSet ds=new DataSet();
			return DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mord_numeorde AS NUMERO FROM morden WHERE pdoc_codigo='"+pref+"' AND test_estado='F' ORDER BY mord_numeorde");
		}

		private void CalcularCarteraCatalogo(string cat,string vin)
		{
			double cartera=0,totalFacturas=0,totalNotas=0;
			DataSet ds=new DataSet();
			DataSet ds1=new DataSet();
			string ruta="";
			if(cbCartera.Checked)
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mfac_tipodocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante FROM dbxschema.mfacturacliente WHERE mnit_nit=(SELECT mnit_nit FROM dbxschema.mcatalogovehiculo WHERE pcat_codigo='"+cat+"' AND mcat_vin='"+vin+"') AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 ");
				if(ds.Tables[0].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						if(ds.Tables[0].Rows[i][2].ToString()!="N")
							totalFacturas=totalFacturas+Convert.ToDouble(ds.Tables[0].Rows[i][3]);
						else if(ds.Tables[0].Rows[i][2].ToString()=="N")
							    totalNotas=totalNotas+Convert.ToDouble(ds.Tables[0].Rows[i][3]);
					}
					cartera=totalFacturas-totalNotas;
					if(cartera>0)
					{
						Session["error"]=ds.Tables[0];
						ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=CV&error=V";
					}
					else if(cartera==0 || cartera<0)
						ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=CV";
				}
				else
				{
					bool existeVehSinEnt=DBFunctions.RecordExist("SELECT * FROM mvehiculo WHERE mcat_vin='"+vin+"' AND test_tipoesta<>60");
					bool existeOTFac=DBFunctions.RecordExist("SELECT * FROM morden WHERE mcat_vin='"+vin+"' AND test_estado<>'E'");
					if(existeVehSinEnt || existeOTFac)
						ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=CV";
					else
                    {
                        Utils.MostrarAlerta(Response, "Este vehículo no tiene ordenes de trabajo asociadas y tampoco aparece como un vehículo nuevo. Verifique sus datos");
                        return;

                    }
				}
			}
			else
			{
				bool existeVehSinEnt=DBFunctions.RecordExist("SELECT * FROM mvehiculo WHERE mcat_vin='"+vin+"' AND test_tipoesta<>60");
				bool existeOTFac=DBFunctions.RecordExist("SELECT * FROM morden WHERE  mcat_vin='"+vin+"' AND test_estado<>'E'");
				if(existeVehSinEnt || existeOTFac)
					ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=CV"+ConstruirGet(cbCartera.Checked,cbMensaje.Checked);
				else
					ruta="?process=Cartera.EmisionOrdenSalidaVehiculo&err=3"+ConstruirGet(cbCartera.Checked,cbMensaje.Checked);
			}
			DBFunctions.Request(ds1,IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion,MORD.PDOC_CODIGO, MORD.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.mcat_vin='" + vin+"' AND MORD.test_estado='F'");
			DBFunctions.Request(ds1,IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion,MO.PDOC_CODIGO, MO.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH LEFT JOIN MORDEN MO ON (MVEH.MCAT_VIN = MO.MCAT_VIN) WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MVEH.mcat_vin='" + vin+"' AND MVEH.test_tipoesta=40");
			Session["ds"]=ds1;
			Response.Redirect(indexPage+ruta);
		}

		private void CalcularCarteraPlaca(string placa)
		{
			DataSet ds=new DataSet();
			DataSet ds1=new DataSet();
			double totalFacturas=0,totalNotas=0,cartera=0;
            string placaCat = DBFunctions.SingleData("SELECT distinct mcat_placa FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "'").Trim();
            string placaVeh = DBFunctions.SingleData("SELECT distinct C.mcat_placa FROM dbxschema.mcatalogovehiculo C,dbxschema.mvehiculo V WHERE V.mcat_vin=C.mcat_vin AND V.test_tipoesta=40 AND C.mcat_placa='" + placa + "'").Trim();
            bool existePlacaCatalogo = placaCat == "" ? false : true;  //esto no porque pueden venir placa vacias y vendría en true... DBFunctions.RecordExist("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
            bool existePlacaVehiculo = placaVeh == "" ? false: true; //esto no porque pueden venir placa vacias y vendría en true... DBFunctions.RecordExist("SELECT C.mcat_placa FROM dbxschema.mcatalogovehiculo C,dbxschema.mvehiculo V WHERE V.mcat_vin=C.mcat_vin AND V.test_tipoesta=40 AND C.mcat_placa='"+placa+"'");
            string ruta="";
			if(cbCartera.Checked)
			{
				//Si existen ambas eso quiere decir que el vehiculo fue vendido
				if(existePlacaCatalogo && existePlacaVehiculo)
				{
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mfac_tipodocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante FROM dbxschema.mfacturacliente WHERE mnit_nit=(SELECT mnit_nit FROM dbxschema.mcatalogovehiculo WHERE mcat_placa='"+placa+"') AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 ");
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
							ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=PV&error=V";
						}
						else if(cartera==0 || cartera<0)
							    ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=PV";
					}
					else
						ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=PV";
                    DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion, MORD.PDOC_CODIGO, MORD.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MCAT.mcat_placa='" + placa + "'");
                    DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion, MO.PDOC_CODIGO,MO.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH LEFT JOIN MORDEN MO ON (MVEH.MCAT_VIN = MO.MCAT_VIN) WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MCAT.mcat_placa='" + placa + "' AND MVEH.test_tipoesta=40");
                    //DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MCAT.mcat_placa='"+placa+"'");
                    //DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND  MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MCAT.mcat_placa='"+placa+"' AND MVEH.test_tipoesta=40");
                    Session["ds"]=ds1;
					Response.Redirect(indexPage+ruta);
				}
				else if(existePlacaCatalogo && !existePlacaVehiculo)
				{
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mfac_tipodocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante FROM dbxschema.mfacturacliente WHERE mnit_nit=(SELECT mnit_nit FROM dbxschema.mcatalogovehiculo WHERE mcat_placa='"+placa+"') AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 ");
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
							ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=PV&error=V";
						}
						else if(cartera==0 || cartera<0)
							    ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=PV";
					}
					else
						ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=PV";
                    //DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MCAT.mcat_placa='"+placa+"'");
                    DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion, MORD.PDOC_CODIGO, MORD.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MCAT.mcat_placa='" + placa + "'");
                    //DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND  MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MCAT.mcat_placa='"+placa+"' AND MVEH.test_tipoesta=40");
                    DBFunctions.Request(ds1,IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion, MO.PDOC_CODIGO,MO.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH LEFT JOIN MORDEN MO ON (MVEH.MCAT_VIN = MO.MCAT_VIN) WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MCAT.mcat_placa='" + placa+"' AND MVEH.test_tipoesta=40");
					Session["ds"]=ds1;
					Response.Redirect(indexPage+ruta);
				}
				else
                    Utils.MostrarAlerta(Response, "No hay datos de esa placa o no se ingresó ninguna placa.Por favor Revise.");
			}
			else
			{
				ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=PV"+ConstruirGet(cbCartera.Checked,cbMensaje.Checked);
				DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND  MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MCAT.mcat_placa='"+placa+"'");
				DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MCAT.mcat_placa='"+placa+"' AND MVEH.test_tipoesta=40");
				Session["ds"]=ds1;
				Response.Redirect(indexPage+ruta);
			}
		}

		private void CalcularCarteraOrden(string pref,string num)
		{
			DataSet ds,ds1;
			double totalFacturas=0,totalNotas=0,cartera=0;
			string ruta="";
			ds=new DataSet();
			if(cbCartera.Checked)
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mfac_tipodocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante FROM dbxschema.mfacturacliente WHERE mnit_nit=(SELECT mnit_nit FROM dbxschema.morden WHERE pdoc_codigo='"+pref+"' AND mord_numeorde="+num+") AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 ");
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
						ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=OT&error=V";
					}
					else if(cartera==0 || cartera<0)
						    ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=OT";
				}
				else
					ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=OT";
				ds1=new DataSet();
                DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion,MORD.PDOC_CODIGO, MORD.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.pdoc_codigo='" + pref + "' AND MORD.mord_numeorde= " + num + " AND MORD.test_estado='F'");
                //DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.pdoc_codigo='"+pref+"' AND MORD.mord_numeorde="+num+" AND MORD.test_estado='F'");
                Session["ds"]=ds1;
				Response.Redirect(indexPage+ruta);
			}
			else
			{
				ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=OT"+ConstruirGet(cbCartera.Checked,cbMensaje.Checked);
				ds1=new DataSet();
				//DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.pdoc_codigo='"+pref+"' AND MORD.mord_numeorde="+num+" AND MORD.test_estado='F'");
                DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion,MORD.PDOC_CODIGO, MORD.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.pdoc_codigo='" + pref + "' AND MORD.mord_numeorde= " + num + " AND MORD.test_estado='F'");
                Session["ds"]=ds1;
				Response.Redirect(indexPage+ruta);
			}
		}

		private void CalcularCarteraNit(string nit)
		{
			DataSet ds,ds1;
			double totalFacturas=0,totalNotas=0,cartera=0;
			string ruta="";
			if(cbCartera.Checked)
			{
                if (DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='" + nit + "'"))
                {
                    ds = new DataSet();
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pdoc_codigo,mfac_numedocu,mfac_tipodocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante FROM dbxschema.mfacturacliente WHERE mnit_nit='" + nit + "' AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 ");
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i][2].ToString() == "F")
                                totalFacturas = totalFacturas + Convert.ToDouble(ds.Tables[0].Rows[i][3]);
                            else if (ds.Tables[0].Rows[i][2].ToString() == "N")
                                    totalNotas = totalNotas + Convert.ToDouble(ds.Tables[0].Rows[i][3]);
                        }
                        cartera = totalFacturas - totalNotas;
                        if (cartera > 0)
                        {
                            Session["error"] = ds.Tables[0];
                            ruta = "?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=NP&error=V";
                        }
                        else if (cartera == 0 || cartera < 0)
                                ruta = "?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=NP";
                    }

                    ds1 = new DataSet();
                    DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion, MORD.PDOC_CODIGO, MORD.MORD_NUMEORDE  FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MNIT.mnit_nit='" + nit + "'");
                    DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion, MO.PDOC_CODIGO, MO.MORD_NUMEORDE FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH LEFT JOIN MORDEN MO ON MVEH.MCAT_VIN = MO.MCAT_VIN WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MVEH.test_tipoesta=40 AND MNIT.mnit_nit='" + nit + "'");
                    if (ds1.Tables[0].Rows.Count == 0 && ds1.Tables[1].Rows.Count == 0)
                    {
                        Utils.MostrarAlerta(Response, "No existen órdenes para ese NIT, revise sus datos.");
                        return;
                    }
                    else
                        ruta = "?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=NP";


                    Session["ds"]=ds1;
					Response.Redirect(indexPage+ruta);
				}
				else
                Utils.MostrarAlerta(Response, "No hay datos de ese nit, asegurese que ingresó el NIT correcto.");
			}
			else
			{
				ruta="?process=Cartera.EmisionOrdenSalidaVehiculo.Emision&tipo=NP"+ConstruirGet(cbCartera.Checked,cbMensaje.Checked);
				ds1=new DataSet();
				DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.morden MORD WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MORD.mcat_vin=MCAT.mcat_vin AND MORD.mnit_nit=MNIT.mnit_nit AND MORD.test_estado='F' AND MNIT.mnit_nit='"+nit+"'");
				DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT DISTINCT MNIT.mnit_nit,MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_apellidos,PCAT.pcat_codigo||'-'||6PCAT.pcat_descripcion,MCAT.mcat_vin,MCAT.mcat_placa,MCAT.mcat_motor,PCOL.pcol_descripcion FROM dbxschema.mcatalogovehiculo MCAT,dbxschema.pcatalogovehiculo PCAT,dbxschema.pcolor PCOL,dbxschema.mnit MNIT,dbxschema.mvehiculo MVEH WHERE PCAT.pcat_codigo=MCAT.pcat_codigo AND MCAT.mnit_nit=MNIT.mnit_nit AND PCOL.pcol_codigo=MCAT.pcol_codigo AND MVEH.mcat_vin=MCAT.mcat_vin AND MVEH.mnit_nit=MNIT.mnit_nit AND MVEH.test_tipoesta=40 AND MNIT.mnit_nit='"+nit+"'");
				Session["ds"]=ds1;
				Response.Redirect(indexPage+ruta);
			}
		}

		/// <summary>
		/// Función para construir unas variables get dependiendo de los valores llegados en los checkbox
		/// </summary>
		/// <param name="verificar">bool true si el checkbox de verificar cartera esta chequeado, de lo contrario false</param>
		/// <param name="mensaje">bool true si el checkbox de mostrar mensaje esta chequeado, de lo contrario false</param>
		/// <returns>string variables construidas</returns>
		private string ConstruirGet(bool verificar,bool mensaje)
		{
			string miGet="";
			//Si el checkbox de verficar esta chequeado
			if(verificar)
				miGet="&verf=1&mens=0";
				//Si el checkbox de verificar no esta chequeado y el de mensajes tampoco
			else if(!verificar && !mensaje)
				miGet="&verf=0&mens=0";
				//Si el checkbox de verificar no esta chequeado y el de mensajes si
			else if(!verificar && mensaje)
				miGet="&verf=0&mens=1";
			return miGet;
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
