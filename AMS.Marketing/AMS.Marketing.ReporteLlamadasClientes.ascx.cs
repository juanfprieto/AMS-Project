using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Marketing
{
	public partial  class ReporteLlamadasClientes : System.Web.UI.UserControl
	{
		public string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string Tipo;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
            if (Request.QueryString["tipo"] != null && Request.QueryString["tipo"].ToString().Equals("V"))
            {
                Tipo = "V"; //Reporte Seguimiento actividades Vendedores: S.A.M.
            }
            else
            {
                Tipo = "C"; //Reporte Seguimiento actividades Clientes
            }
            if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				toolsHolder.Visible=false;
				phReporte.Visible=false;
                bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO,PVEN_NOMBRE FROM DBXSCHEMA.PVENDEDOR where tvend_codigo in ('VM','VV','MC') ORDER BY PVEN_NOMBRE;");
                if (Tipo.Equals("C"))
                {
                    ddlVendedor.Items.Insert(0, new ListItem("--Todos--", "-1"));
                    ddlVendedor.Items.Insert(1, new ListItem("--Validar--", "-2"));
                }
                plcClave.Visible = Tipo.Equals("V");
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "La actividad ha sido registrada");
                if (Request.QueryString["nit"] != null)
                Utils.MostrarAlerta(Response, "Gestionó el cliente " + Request.QueryString["nit"] + "");
				if(Request.QueryString["ret"]!=null && Session["MARKFLT_VENDEDOR"]!=null)
				{
					fechaInicial.SelectedDate=(DateTime)Session["MARKFLT_FECHADESDE"];
					fechaInicial.TodaysDate=(DateTime)Session["MARKFLT_FECHADESDE"];
					fechaFinal.SelectedDate=(DateTime)Session["MARKFLT_FECHAHASTA"];
					fechaFinal.TodaysDate=(DateTime)Session["MARKFLT_FECHAHASTA"];
					ddlVendedor.SelectedIndex=ddlVendedor.Items.IndexOf(ddlVendedor.Items.FindByValue(Session["MARKFLT_VENDEDOR"].ToString()));
                    if (Tipo.Equals("V"))
                        txtClave.Text = Session["MARKFLT_CLAVE"].ToString();
					btnGenerar_Click(Sender,e);
				}
			}
		}
		
		protected void btnGenerarExcel_Click(object Sender,EventArgs e)
		{
			DataGrid dgExcel=null;
			int selExcel=Convert.ToInt16(ddlTipoExcel.SelectedValue);
			switch(selExcel)
			{
				case 1:dgExcel=dgReporte;break;
				case 2:dgExcel=dgCumple;break;
				case 3:dgExcel=dgAniversario;break;
				case 4:dgExcel=dgAct;break;
				case 5:dgExcel=dgContacto1;break;
				case 6:dgExcel=dgContacto2;break;
				case 7:dgExcel=dgContacto3;break;
				case 8:dgExcel=dgContacto4;break;
				case 9:dgExcel=dgContacto5;break;
				case 10:dgExcel=dgMantenimiento;break;
				case 11:dgExcel=dgContacto0;break;
                case 12:dgExcel=dgDocsVencen; break;
                case 13: dgExcel = dgrCredsVencen; break;
                case 14: dgExcel = dgrRecomendaciones; break;
			}
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename=ReporteSeguimiento."+ddlTipoExcel.SelectedItem.Text.Replace(" ","_")+".xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			System.IO.StringWriter stringWrite = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
			dgExcel.RenderControl(htmlWrite);
			Response.Write(stringWrite.ToString());
			Response.End();
		}
		protected void btnGenerar_Click(object Sender,EventArgs e)
		{
			string vendedor=ddlVendedor.SelectedValue;
            string password;
            string prefActividad;
			bool validar=false;
            if (Tipo.Equals("V")){
                password = DBFunctions.SingleData("SELECT PVEN_CLAVE FROM DBXSCHEMA.PVENDEDOR WHERE PVEN_CODIGO='" + vendedor + "';");
                if (!password.Equals(txtClave.Text)){
                    Utils.MostrarAlerta(Response, "Clave no válida");
                    return;
                }
            }
            if(Tipo.Equals("C")){
			    if(vendedor=="-1")vendedor="";
			    if(vendedor=="-2"){
				    validar=true;
				    vendedor="";
			    }
                Session.Clear();
            }
			dgReporte.DataBind();
			if(Session["dtReporte"]==null)
				this.toolsHolder.Visible=false;
			DataSet ds;
			if(fechaInicial.SelectedDate>fechaFinal.SelectedDate)
			{
                Utils.MostrarAlerta(Response, "La fecha inicial debe ser anterior o igual a la fecha final");
				return;
			}
			else
			{
                string nitEmpresa = DBFunctions.SingleData("select mnit_nit from cempresa");

				ds=new DataSet();
                prefActividad = "PC";
				//PROFESION
				string select0="SELECT MCLI.mnit_nit CC,MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') CONCAT ' ' concat MNIT.mnit_apellidos CONCAT ' ' concat COALESCE(MNIT.mnit_apellido2,'') AS Nombre,PPRO.ppro_nombre PROFESION,(CAST(PPRO.tdia_dia AS CHARACTER(2)) CONCAT ' de ' CONCAT PMES.pmes_nombre) AS Fecha, PV.PVEN_NOMBRE AS PVEN_NOMBRE,MNIT_EMAIL AS EMAIL,COALESCE(MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO ";
				string from="FROM dbxschema.mnit MNIT,dbxschema.pprofesion PPRO,dbxschema.mcliente MCLI,dbxschema.pmes PMES,dbxschema.tdia TDIA, dbxschema.pvendedor PV ";
				string where="WHERE MCLI.mnit_nit=MNIT.mnit_nit AND MCLI.ppro_codigo=PPRO.ppro_codigo AND MCLI.PVEN_CODIGO=PV.PVEN_CODIGO and "+
					(vendedor.Length>0?"PV.PVEN_CODIGO='"+vendedor+"' AND ":"")+				
					"PPRO.tdia_dia=TDIA.tdia_dia AND PPRO.pmes_mes=PMES.pmes_mes AND "+
					"PPRO.pmes_mes>="+fechaInicial.SelectedDate.Month+" AND PPRO.pmes_mes<="+fechaFinal.SelectedDate.Month+" AND "+
					"NOT(PPRO.pmes_mes="+fechaInicial.SelectedDate.Month+" AND PPRO.tdia_dia<"+fechaInicial.SelectedDate.Day+") AND "+
					"NOT(PPRO.pmes_mes="+fechaFinal.SelectedDate.Month+" AND PPRO.tdia_dia>"+fechaFinal.SelectedDate.Day+") ";
				if(validar || Tipo.Equals("V"))
					where+=" AND MCLI.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '"+fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"' and '"+fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"') ";
				else if(!Tipo.Equals("V"))
                    where+=" AND MCLI.mnit_nit not in( SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad +"' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') ";

                prefActividad = "CC";
                //CUMPLEAÑOS
				string select1="SELECT MCLI.mnit_nit CC,MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') CONCAT ' ' concat MNIT.mnit_apellidos CONCAT ' ' concat COALESCE(MNIT.mnit_apellido2,'') AS Nombre,(CAST(MCLI.tdia_dia AS CHARACTER(2)) CONCAT ' de ' CONCAT PMES.pmes_nombre) AS Fecha , PV.PVEN_NOMBRE AS PVEN_NOMBRE,MNIT_EMAIL AS EMAIL,COALESCE(MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO ";
				string from2="FROM dbxschema.mnit MNIT,dbxschema.mcliente MCLI,dbxschema.pmes PMES,dbxschema.tdia, dbxschema.pvendedor PV ";
				string where2="WHERE MCLI.mnit_nit=MNIT.mnit_nit AND MCLI.tdia_dia=TDIA.tdia_dia AND MCLI.pmes_mes=PMES.pmes_mes AND MCLI.PVEN_CODIGO=PV.PVEN_CODIGO AND "+
					(vendedor.Length>0?"PV.PVEN_CODIGO='"+vendedor+"' AND ":"")+
					"MCLI.pmes_mes>="+fechaInicial.SelectedDate.Month+" AND MCLI.pmes_mes<="+fechaFinal.SelectedDate.Month+" AND "+
					"NOT(MCLI.pmes_mes="+fechaInicial.SelectedDate.Month+" AND MCLI.tdia_dia<"+fechaInicial.SelectedDate.Day+") AND "+
					"NOT(MCLI.pmes_mes="+fechaFinal.SelectedDate.Month+" AND MCLI.tdia_dia>"+fechaFinal.SelectedDate.Day+") ";
                if (validar || Tipo.Equals("V"))
					where2+=" AND MCLI.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '"+fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"' and '"+fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"') ";
                else if (!Tipo.Equals("V"))
                    where2 += " AND MCLI.mnit_nit not in( SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') ";

                prefActividad = "AC";
                //ANIVERSARIOS
				string select2="SELECT MCLI.mnit_nit CC,"+
					"MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') CONCAT ' ' concat MNIT.mnit_apellidos CONCAT ' ' concat COALESCE(MNIT.mnit_apellido2,'') concat ' - ' concat coalesce(MCLI.MCLI_CONYUGE,'') AS Nombre,(CAST(MCLI.mcli_diaaniver AS CHARACTER(2)) CONCAT ' de ' CONCAT PMES.pmes_nombre) AS Fecha , PV.PVEN_NOMBRE AS PVEN_NOMBRE,"+
					"MNIT_EMAIL AS EMAIL,COALESCE(MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO ";
				string from3="FROM dbxschema.mnit MNIT,dbxschema.mcliente MCLI,dbxschema.pmes PMES,dbxschema.tdia TDIA, dbxschema.pvendedor PV ";
				string where3="WHERE MCLI.mnit_nit=MNIT.mnit_nit AND MCLI.mcli_diaaniver=TDIA.tdia_dia AND MCLI.mcli_mesaniver=PMES.pmes_mes AND MCLI.PVEN_CODIGO=PV.PVEN_CODIGO AND "+
					(vendedor.Length>0?"PV.PVEN_CODIGO='"+vendedor+"' AND ":"")+				
					"MCLI.mcli_mesaniver>="+fechaInicial.SelectedDate.Month+" AND MCLI.mcli_mesaniver<="+fechaFinal.SelectedDate.Month+" AND "+
					"NOT(MCLI.mcli_mesaniver="+fechaInicial.SelectedDate.Month+" AND MCLI.mcli_diaaniver<"+fechaInicial.SelectedDate.Day+") AND "+
					"NOT(MCLI.mcli_mesaniver="+fechaFinal.SelectedDate.Month+" AND MCLI.mcli_diaaniver>"+fechaFinal.SelectedDate.Day+") ";
                if (validar || Tipo.Equals("V"))
					where3+=" AND MCLI.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '"+fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"' and '"+fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"') ";
                else if (!Tipo.Equals("V"))
                    where3 += " AND MCLI.mnit_nit not in( SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') ";

                prefActividad = "AE";
                //DMARKETING
				string select3="SELECT "+
					"DMAR.mnit_nit CC,MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') CONCAT ' ' concat MNIT.mnit_apellidos CONCAT ' ' concat COALESCE(MNIT.mnit_apellido2,'') Nombre,"+
					"PACT.pact_nombmark AS ACTIVIDAD,CAST(DAY(DMAR.dmar_fechprox) AS CHARACTER(2)) CONCAT ' de ' CONCAT MONTHNAME(DMAR.dmar_fechprox) FECHA,pv.pven_nombre as pven_nombre,"+
					"MNIT_EMAIL AS EMAIL,COALESCE(MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO "+
					"FROM dbxschema.dmarketing DMAR,dbxschema.mnit MNIT,dbxschema.pactividadmarketing PACT, dbxschema.pvendedor pv "+
					"WHERE DMAR.mnit_nit=MNIT.mnit_nit AND DMAR.pact_codimark=PACT.pact_codimark AND dmar.PVEN_CODIGO=PV.PVEN_CODIGO AND "+
					(vendedor.Length>0?"PV.PVEN_CODIGO='"+vendedor+"' AND ":"")+
                    ((validar || Tipo.Equals("V")) ? " DMAR.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' and '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') AND " : " ") +
                    (!Tipo.Equals("V") ? " DMAR.mnit_nit not in(SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) AND " : " ") +
                    "DMAR.dmar_fechprox BETWEEN DATE('"+this.fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"') AND DATE('"+this.fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"') "+ 
					"ORDER BY DMAR.mnit_nit";

				DateTime fechaCI=fechaInicial.SelectedDate.AddDays(-2);
				DateTime fechaCF=fechaFinal.SelectedDate.AddDays(-2);
                prefActividad = "C2S";
				//CONTACTOS A 2 DIAS usando morden
                string select4 = 
                    "SELECT mc.mnit_nit AS CC, \n" +
                    "       mc.mcat_vin VIN, \n" +
                    "       mc.pcat_codigo concat ' / ' concat mc.mcat_vin concat ' / ' concat mc.mcat_placa AS VEHI, \n" +
                    "       CASE WHEN COALESCE(mo.mord_contacto,'') <> '' \n" +
                    "       THEN \n" +
                    "           mo.mord_contacto \n" +
                    "       ELSE \n" +
                    "           mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') \n" +
                    "       END AS NOMBRE, \n" + 
                    "       mo.fecha AS fecha, \n" +
                    "       pv.pven_codigo, \n" + 
                    "       PA.PALM_DESCRIPCION,  \n " +
                    "       mn.MNIT_EMAIL AS EMAIL, \n" +
                    "       COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, \n" +
                    "       pv.pven_nombre \n" +
                    "FROM dbxschema.mnit mn, \n" +
                    "     dbxschema.pvendedor pv, \n" +
                    "     dbxschema.mcatalogovehiculo mc, \n" +
                    "     dbxschema.cempresa cem, \n" +
                    "     DBXSCHEMA.PALMACEN PA, \n " +
                    "     (SELECT mcat_vin, pven_codigo, mnit_nit, mord_contacto, MAX(mord_fechliqu) AS fecha, PALM_ALMACEN FROM DBXSCHEMA.MORDEN " +
                    (nitEmpresa == "890802377" ? "WHERE pdoc_codigo <> '421' " : "") + // Esto es exclusivo de caldas motor
                    "GROUP BY mcat_vin, pven_codigo, mnit_nit,mord_contacto, palm_almacen) AS mo \n" +
                    "WHERE mc.mcat_vin = mo.mcat_vin \n" +
                    "AND   cem.mnit_nit <> mo.mnit_nit \n" +
                    "AND   mn.mnit_nit = mc.mnit_nit \n" +
                    "AND   pv.pven_codigo = mo.pven_codigo \n" +
                    "AND   PA.PALM_ALMACEN = MO.PALM_ALMACEN \n" +
                    "AND   mo.fecha BETWEEN '" + fechaCI.ToString("yyyy-MM-dd") + "' AND '" + fechaCF.ToString("yyyy-MM-dd") + "' \n" +
                    (vendedor.Length > 0 ? " AND PV.PVEN_CODIGO='" + vendedor + "' " : "") +
                    ((validar || Tipo.Equals("V")) ? " AND mc.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' and '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND  mc.mnit_nit NOT IN (SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad +"' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "ORDER BY cc";

                fechaCI =fechaInicial.SelectedDate.AddDays(-5);
				fechaCF=fechaFinal.SelectedDate.AddDays(-5);
                prefActividad = "E5";
				//CONTACTOS A 5 dias
                string select5 = "select mv.mcat_vin VIN," +
					"mv.mnit_nit AS CC, mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE, cast(mv.mveh_fechentr as date) as fecha, "+
                    "mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, pv.pven_nombre, PA.PALM_descripcion, " +
                    "mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO " +
					"from dbxschema.mvehiculo mv,dbxschema.mnit mn, dbxschema.mcatalogovehiculo mc, dbxschema.masignacionvehiculo ma,"+
                    "dbxschema.mpedidovehiculo mp, PALMACEN PA, " +
                    "DBXSCHEMA.MFACTURAPEDIDOVEHICULO MFPV, " +
                    "MFACTURACLIENTE MFC, dbxschema.pvendedor pv "+
					"where mn.mnit_nit=mv.mnit_nit and mc.mcat_vin=mv.mcat_vin and ma.mveh_inventario=mv.mveh_inventario and ma.pdoc_codigo=mp.pdoc_codigo "+
					(vendedor.Length>0?" AND PV.PVEN_CODIGO='"+vendedor+"' ":"")+
                    "AND   MFPV.MVEH_INVENTARIO = MV.MVEH_INVENTARIO " +
                    "AND MFC.PDOC_CODIGO = MFPV.PDOC_CODIGO AND MFC.MFAC_NUMEDOCU = MFPV.MFAC_NUMEDOCU " +
                    "AND PA.PALM_ALMACEN = MFC.PALM_ALMACEN " +
                    "AND mfac_tipclie = 'C' " +
                    "and mfc.palm_almacen = PA.palm_almacen" +
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti >= '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND  mv.mnit_nit not in (SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "and mv.mveh_fechentr between '"+fechaCI.ToString("yyyy-MM-dd")+" 00:00:00' and '"+fechaCF.ToString("yyyy-MM-dd")+" 00:00:00' "+
					"and ma.mped_numepedi=mp.mped_numepedi and pv.pven_codigo=mp.pven_codigo order by mv.mnit_nit;";
				
                fechaCI=fechaInicial.SelectedDate.AddMonths(-1);
				fechaCF=fechaFinal.SelectedDate.AddMonths(-1);
                prefActividad = "E30";
                //CONTACTOS A 30 dias
                string select6 = "select mv.mcat_vin VIN," +
					"mv.mnit_nit AS CC, mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE, cast(mv.mveh_fechentr as date) as fecha, "+
                    "mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, pv.pven_nombre, PA.PALM_descripcion, " +
					"mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO "+
					"from dbxschema.mvehiculo mv,dbxschema.mnit mn, dbxschema.mcatalogovehiculo mc, dbxschema.masignacionvehiculo ma,"+
                    "dbxschema.mpedidovehiculo mp, DBXSCHEMA.MFACTURAPEDIDOVEHICULO MFPV, " +
                    "PALMACEN PA, " +
                    "MFACTURACLIENTE MFC, dbxschema.pvendedor pv " +
					"where mn.mnit_nit=mv.mnit_nit and mc.mcat_vin=mv.mcat_vin and ma.mveh_inventario=mv.mveh_inventario " +
                    "AND MFPV.MVEH_INVENTARIO = MV.MVEH_INVENTARIO " +
                    "AND MFC.PDOC_CODIGO = MFPV.PDOC_CODIGO " +
                    "AND MFC.MFAC_NUMEDOCU = MFPV.MFAC_NUMEDOCU " +
                    "AND PA.PALM_ALMACEN = MFC.PALM_ALMACEN " +
                    "AND mfac_tipclie = 'C' " +
                    "AND mfc.palm_almacen = PA.palm_almacen " +
                    "and ma.pdoc_codigo=mp.pdoc_codigo "+
                  (vendedor.Length>0?" AND PV.PVEN_CODIGO='"+vendedor+"' ":"")+
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti >= '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND  mv.mnit_nit not in(SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "and mv.mveh_fechentr between '"+fechaCI.ToString("yyyy-MM-dd")+" 00:00:00' and '"+fechaCF.ToString("yyyy-MM-dd")+" 00:00:00' "+
					"and ma.mped_numepedi=mp.mped_numepedi and pv.pven_codigo=mp.pven_codigo order by mv.mnit_nit;";
				
                fechaCI=fechaInicial.SelectedDate.AddMonths(-2);
				fechaCF=fechaFinal.SelectedDate.AddMonths(-2);
                prefActividad = "E60";
				//CONTACTOS A 60 dias
                string select7 = "select mv.mcat_vin VIN," +
					"mv.mnit_nit AS CC, mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE, cast(mv.mveh_fechentr as date) as fecha, "+
                    "mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, pv.pven_nombre, pa.palm_descripcion," +
					"mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO "+
					"from dbxschema.mvehiculo mv,dbxschema.mnit mn, dbxschema.mcatalogovehiculo mc, dbxschema.masignacionvehiculo ma,"+
					"dbxschema.mpedidovehiculo mp, " +
                    "dbxschema.MFACTURAPEDIDOVEHICULO MFPV, " +
                    "dbxschema.PALMACEN PA, " +
                    "dbxschema.MFACTURACLIENTE MFC, " +
                    "dbxschema.pvendedor pv "+
					"where mn.mnit_nit=mv.mnit_nit and mc.mcat_vin=mv.mcat_vin and ma.mveh_inventario=mv.mveh_inventario " +
                    "AND   MFPV.MVEH_INVENTARIO = MV.MVEH_INVENTARIO " +
                    "AND MFC.PDOC_CODIGO = MFPV.PDOC_CODIGO " +
                    "AND MFC.MFAC_NUMEDOCU = MFPV.MFAC_NUMEDOCU " +
                    "AND PA.PALM_ALMACEN = MFC.PALM_ALMACEN " +
                    "AND mfac_tipclie = 'C' " +
                    "AND mfc.palm_almacen = PA.palm_almacen " +
                    "and ma.pdoc_codigo=mp.pdoc_codigo " +
					(vendedor.Length>0?" AND PV.PVEN_CODIGO='"+vendedor+"' ":"")+
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti >= '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND mv.mnit_nit not in(SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "and mv.mveh_fechentr between '"+fechaCI.ToString("yyyy-MM-dd")+" 00:00:00' and '"+fechaCF.ToString("yyyy-MM-dd")+" 00:00:00' "+
					"and ma.mped_numepedi=mp.mped_numepedi and pv.pven_codigo=mp.pven_codigo order by mv.mnit_nit;";
				
                fechaCI=fechaInicial.SelectedDate.AddMonths(-6);
				fechaCF=fechaFinal.SelectedDate.AddMonths(-6);
                prefActividad = "EM6";
				//CONTACTOS A 6 MESES
                string select8 = "select mv.mcat_vin VIN," +
					"mv.mnit_nit AS CC, mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE, cast(mv.mveh_fechentr as date) as fecha, "+
                    "mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, pv.pven_nombre, PA.PALM_descripcion," +
					"mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO "+
					"from dbxschema.mvehiculo mv,dbxschema.mnit mn, dbxschema.mcatalogovehiculo mc, dbxschema.masignacionvehiculo ma,"+
					"dbxschema.mpedidovehiculo mp, " +
                    "DBXSCHEMA.MFACTURAPEDIDOVEHICULO MFPV, " +
                    "dbxschema.PALMACEN PA, " +
                    "dbxschema.MFACTURACLIENTE MFC, " +
                    "dbxschema.pvendedor pv "+
					"where mn.mnit_nit=mv.mnit_nit and mc.mcat_vin=mv.mcat_vin and ma.mveh_inventario=mv.mveh_inventario " +
                    "AND MFPV.MVEH_INVENTARIO = MV.MVEH_INVENTARIO " +
                    "AND MFC.PDOC_CODIGO = MFPV.PDOC_CODIGO " +
                    "AND MFC.MFAC_NUMEDOCU = MFPV.MFAC_NUMEDOCU " +
                    "AND PA.PALM_ALMACEN = MFC.PALM_ALMACEN " +
                    "AND mfac_tipclie = 'C' " +
                    "AND mfc.palm_almacen = PA.palm_almacen " +
                    "and ma.pdoc_codigo=mp.pdoc_codigo "+
					(vendedor.Length>0?" AND PV.PVEN_CODIGO='"+vendedor+"' ":"")+
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti >= '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND mv.mnit_nit not in(SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "and mv.mveh_fechentr between '"+fechaCI.ToString("yyyy-MM-dd")+" 00:00:00' and '"+fechaCF.ToString("yyyy-MM-dd")+" 00:00:00' "+
					"and ma.mped_numepedi=mp.mped_numepedi and pv.pven_codigo=mp.pven_codigo order by mv.mnit_nit;";
				
                fechaCI=fechaInicial.SelectedDate.AddYears(-1);
				fechaCF=fechaFinal.SelectedDate.AddYears(-1);
                prefActividad = "EA1";
				//CONTACTOS A 1 año
                string select9 = "select mv.mcat_vin VIN," +
					"mv.mnit_nit AS CC, mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE, cast(mv.mveh_fechentr as date) as fecha, "+
                    "mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, pv.pven_nombre, PA.PALM_descripcion," +
					"mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO "+
					"from dbxschema.mvehiculo mv,dbxschema.mnit mn, dbxschema.mcatalogovehiculo mc, dbxschema.masignacionvehiculo ma,"+
					"dbxschema.mpedidovehiculo mp, " +
                    "DBXSCHEMA.MFACTURAPEDIDOVEHICULO MFPV, " +
                    "dbxschema.PALMACEN PA, " +
                    "dbxschema.MFACTURACLIENTE MFC, " +
                    "dbxschema.pvendedor pv "+
					"where mn.mnit_nit=mv.mnit_nit and mc.mcat_vin=mv.mcat_vin and ma.mveh_inventario=mv.mveh_inventario " +
                    "AND MFPV.MVEH_INVENTARIO = MV.MVEH_INVENTARIO " +
                    "AND MFC.PDOC_CODIGO = MFPV.PDOC_CODIGO " +
                    "AND MFC.MFAC_NUMEDOCU = MFPV.MFAC_NUMEDOCU " +
                    "AND PA.PALM_ALMACEN = MFC.PALM_ALMACEN " +
                    "AND mfac_tipclie = 'C' " +
                    "AND mfc.palm_almacen = PA.palm_almacen " +
                    "and ma.pdoc_codigo=mp.pdoc_codigo "+
					(vendedor.Length>0?" AND PV.PVEN_CODIGO='"+vendedor+"' ":"")+
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti >= '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND mv.mnit_nit not in(SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "and mv.mveh_fechentr between '"+fechaCI.ToString("yyyy-MM-dd")+" 00:00:00' and '"+fechaCF.ToString("yyyy-MM-dd")+" 00:00:00' "+
					"and ma.mped_numepedi=mp.mped_numepedi and pv.pven_codigo=mp.pven_codigo order by mv.mnit_nit;";

                DateTime fechaInicLlamada = Convert.ToDateTime(this.fechaInicial.SelectedDate);
                fechaInicLlamada = fechaInicLlamada.AddMonths(-1);
                string fechaIniLlama = fechaInicLlamada.ToString("yyyy-MM-dd");
                prefActividad = "MP";
                //KITS A APLICAR
                //  mc.mcat_numekiloprom*0.0333 = KILOMETRAJE PROMEDIO DIARIO del vehículo específico
                string select10 = "Select DISTINCT mc.mcat_vin VIN," +
					"mn.mnit_nit as CC, "+
					"mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE,"+
					"mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, pv.pven_nombre,"+
					"pc.pgru_grupo grupo, mc.pcat_codigo concat ' / ' concat mc.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI,mc.mcat_fechultikilo AS FECHA,"+
					"mc.mcat_venta, days(mc.mcat_fechultikilo)-days(mc.mcat_venta) dias_ultord_entrega, mc.mcat_numekiloprom*0.0333 kilo_prom_dia,"+
					"days('"+this.fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"')-days(mc.mcat_fechultikilo) dias_desde,"+
					"days('"+this.fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"')-days(mc.mcat_fechultikilo) dias_hasta,"+
					"int((mc.mcat_numekiloprom*0.033)*(days('"+this.fechaInicial.SelectedDate.ToString("yyyy-MM-dd")+"')-days(mc.mcat_fechultikilo))) kilos_desde,"+
					"int((mc.mcat_numekiloprom*0.033)*(days('"+this.fechaFinal.SelectedDate.ToString("yyyy-MM-dd")+"')-days(mc.mcat_fechultikilo)))+  CAST (mc.mcat_numeultikilo AS INT)   kilos_hasta,"+
					"pm.pman_kilometraje, pk.pkit_codigo concat ' - ' concat pk.pkit_nombre KIT, PV.PVEN_CODIGO AS PVEN_CODIGO "+
					"from "+
					"DBXSCHEMA.PCATALOGOVEHICULO pc, dbxschema.mcatalogovehiculo mc, DBXSCHEMA.MNIT mn, "+
   				    "dbxschema.pvendedor pv, morden mo1,  "+
					"(SELECT max(mord_creacion) fecha_kilo, mcat_vin  from DBXSCHEMA.morden group by mcat_vin) as mo, "+
                    "DBXSCHEMA.PKIT pk left join DBXSCHEMA.PMANTENIMIENTOPROGRAMADO pm on pk.pkit_codigo=pm.pkit_codigo " +  
                    "where "+
					"mo1.mord_creacion=mo.fecha_kilo and mo.mcat_vin=mo1.mcat_vin and "+
					"mc.pcat_codigo=pc.pcat_codigo and pk.pgru_grupo=pc.pgru_grupo and mc.mcat_vin=mo.mcat_vin and "+
                    "mn.mnit_nit=mc.mnit_nit and "+
					"pv.pven_codigo=mo1.pven_codigo and "+
					(vendedor.Length>0?"PV.PVEN_CODIGO='"+vendedor+"' AND ":" ")+
                     ((Tipo.Equals("C")) ?
                       "mn.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where mmar_numero IS Null AND dmi.dmar_fechacti between '" + fechaIniLlama + "' and '" + this.fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') AND " : " " +
                        "mn.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '" + this.fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' and '" + this.fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') AND ") +
                    "days(mc.mcat_fechultikilo)-days(mc.mcat_venta)>0 and " +
                    "( (coalesce(pm.pman_kilometraje,0) between decimal((mc.mcat_numekiloprom*0.0333)*(days('" + this.fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "')-days(mc.mcat_fechultikilo))+mc.mcat_numeultikilo)*0.99 and decimal((mc.mcat_numekiloprom*0.0333)*(days('" + this.fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "')-days(mc.mcat_fechultikilo))+mc.mcat_numeultikilo) )"+
                    " OR pm.pman_MESES = (year(CURRENT DATE)-year(mo.fecha_kilo))*12+month (current date )- month (mo.fecha_kilo)" +
                    " OR  (coalesce(pk.pkit_kilometr,0) between decimal((mc.mcat_numekiloprom*0.0333)*(days('" + this.fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "')-days(mc.mcat_fechultikilo))+mc.mcat_numeultikilo)*0.99 and decimal((mc.mcat_numekiloprom*0.0333)*(days('" + this.fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "')-days(mc.mcat_fechultikilo))+mc.mcat_numeultikilo) ) )"+
                    " and mc.MCAT_NUMEKILOPROM > 0 and mc.MCAT_NUMEKILOPROM < 5000 "+
                    " and mn.mnit_nit not in (select mnit_nit from cempresa) "+
	                " and mn.mnit_nit not in (select mnit_nit from PCASAMATRIZ) "+
	                " and mn.mnit_nit not in (select mnit_nit from PASEGURADORA) "+
                    " and mc.mcat_vin not in (select mcat_vin from morden where test_estado = 'A') "+
                    (!Tipo.Equals("V") ? " and mn.mnit_nit not in (SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "order by kilos_hasta;";

                fechaCI = fechaInicial.SelectedDate.AddDays(-10);
                fechaCF = fechaFinal.SelectedDate.AddDays(10);
                prefActividad = "DV";
                //DOCUMENTOS A VENCER
                string select11 =
                    "select mv.mcat_vin VIN," +
                    " mv.mnit_nit AS CC, mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE, " +
                    " mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, " +
                    " pv.pven_nombre, PV.PVEN_CODIGO AS PVEN_CODIGO, " +
                    " mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, " +
                    " PD.PDOC_NOMBRE AS DOCUMENTO, MD.MVEH_FECHENTREGA AS FECHA " +
                    "from mvehiculo mv, mnit mn, mcatalogovehiculo mc," +
                    " MVEHICULODOCUMENTO md, PDOCUMENTOVEHICULO pd, " +
                    " pvendedor pv, masignacionvehiculo ma,mpedidovehiculo mp "+
                    "where " +
                    " ma.mveh_inventario=mv.mveh_inventario and ma.pdoc_codigo=mp.pdoc_codigo and "+
                    " md.mcat_vin=mv.mcat_vin and md.pdoc_codidocu=pd.pdoc_codigo and " +
                    " ma.mped_numepedi=mp.mped_numepedi and pv.pven_codigo=mp.pven_codigo and "+
                    " mn.mnit_nit=mv.mnit_nit and mc.mcat_vin=mv.mcat_vin " +
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' and '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND mv.mnit_nit not in(SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    (vendedor.Length > 0 ? " AND PV.PVEN_CODIGO='" + vendedor + "' " : "") +
                    " and md.MVEH_FECHENTREGA between '" + fechaCI.ToString("yyyy-MM-dd") + " 00:00:00' and '" + fechaCF.ToString("yyyy-MM-dd") + " 00:00:00' " +
                    "order by MD.MVEH_FECHENTREGA;";
                
                fechaCI = fechaInicial.SelectedDate.AddDays(-10);
                fechaCF = fechaFinal.SelectedDate.AddDays(10);
                prefActividad = "CV";
                //CREDITOS A VENCER
                string select12 =
                    "select mv.mcat_vin VIN," +
                    " mp.mnit_nit AS CC, "+
                    " mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE,"+
                    " mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, " +
                    " mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS,"+
                    " mc.mcred_fechaprob + mcred_meses MONTHS FECHA, mc.mcred_meses MESES, mc.mcred_valoaprob VALOR, "+
                    " pv.pven_nombre "+
                    "from pvendedor pv, mpedidovehiculo mp, mnit mn, mcreditofinanciera mc, mcatalogovehiculo mc, mvehiculo mv, masignacionvehiculo ma " +
                    "where " +
                    " ma.pdoc_codigo=mp.pdoc_codigo and ma.mped_numepedi=mp.mped_numepedi and mv.mveh_inventario=ma.mveh_inventario and " +
                    " mc.mcat_vin=mv.mcat_vin and "+
                    " mp.mnit_nit=mn.mnit_nit and mc.pdoc_codipedi=mp.pdoc_codigo and mc.mped_numepedi=mp.mped_numepedi and "+
                    " mc.mcred_fechaprob is not null and mcred_meses  is not null and "+
                    " pv.pven_codigo=mp.pven_codigo "+
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti between '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' and '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND mv.mnit_nit not in(SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    (vendedor.Length > 0 ? " AND PV.PVEN_CODIGO='" + vendedor + "' " : "") +
                    " and (mc.mcred_fechaprob + mcred_meses MONTHS) between '" + fechaCI.ToString("yyyy-MM-dd") + " 00:00:00' and '" + fechaCF.ToString("yyyy-MM-dd") + " 00:00:00' " +
                    "order by FECHA;";

                DateTime fechaAuxIni = new DateTime();
                DateTime fechaAuxFin = new DateTime();

                if (fechaInicial.SelectedDate.Month > 1)
                    fechaAuxIni = new DateTime(fechaInicial.SelectedDate.Year, fechaInicial.SelectedDate.Month, fechaInicial.SelectedDate.Day);
                else
                    fechaAuxIni = new DateTime(fechaInicial.SelectedDate.Year , 12, fechaInicial.SelectedDate.Day);
              
                if (fechaFinal.SelectedDate.Month > 1)
                     fechaAuxFin = new DateTime(fechaFinal.SelectedDate.Year, fechaFinal.SelectedDate.Month, fechaFinal.SelectedDate.Day);
                else
                    fechaAuxFin = new DateTime(fechaFinal.SelectedDate.Year, 12, fechaFinal.SelectedDate.Day);

                string select13 =
                     @"select mord.mnit_nit as CC,    
                     mn.mnit_nombres concat ' ' concat mn.mnit_nombre2 concat ' ' concat mn.mnit_apellidos concat ' ' concat mn.mnit_apellido2 as NOMBRE,    
                     do.pdoc_codigo concat '-' concat do.mord_numeorde as ORDENTRABAJO,   
                     do.dobsot_fecha_procesado as FECHAOT,    
                     do.dobsot_observaciones as OBSERVACIONES,    
                     mord.mcat_vin as VIN,    
                     do.pven_codigo as PVEN_CODIGO,    
                     pt.ptem_descripcion as OPERACION    
                     from morden mord, dobservacionesot do, mnit mn, ptempario pt    
                     where do.pdoc_codigo = mord.pdoc_codigo and  do.mord_numeorde = mord.mord_numeorde and    
                     do.DOBSOT_OBSERVACIONES not like 'Observaciones' and trim(do.DOBSOT_OBSERVACIONES) not like '' and    
                     mn.mnit_nit = mord.mnit_nit and    
                     ( (do.dobsot_fecha_procesado > '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + @"' and do.dobsot_fecha_procesado < '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + @"') or    
                     (do.dobsot_fecha_procesado > '" + fechaAuxIni.ToString("yyyy-MM-dd") + @"' and do.dobsot_fecha_procesado < '" + fechaAuxFin.ToString("yyyy-MM-dd") + @"') )    
                     and do.ptem_operacion = pt.ptem_operacion    
                     union    
                     select dm.mnit_nit as CC,    
                     mn.mnit_nombres concat ' ' concat mn.mnit_nombre2 concat ' ' concat mn.mnit_apellidos concat ' ' concat mn.mnit_apellido2 as NOMBRE,    
                     dm.pdoc_codirefe concat '-' concat dm.pdoc_numerefe as ORDENTRABAJO,    
                     dm.dmar_fechacti as FECHAOT,    
                     CAST (dm.dmar_detalle AS VARCHAR(1000)) as OBSERVACIONES,    
                     mor.mcat_vin as VIN,    
                     dm.pven_codmar as PVEN_CODIGO,    
                     'Preliquidación' as OPERACION    
                     from dmarketing dm, mnit mn, morden mor    
                     where mn.mnit_nit = dm.mnit_nit and     
                     dm.pdoc_codirefe not like '' and    
                     mor.pdoc_codigo = dm.pdoc_codirefe and mor.mord_numeorde = dm.pdoc_numerefe and    
                     ( (dm.dmar_fechacti > '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + @"' and dm.dmar_fechacti < '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + @"') or    
                     (dm.dmar_fechacti > '" + fechaAuxIni.ToString("yyyy-MM-dd") + @"' and dm.dmar_fechacti < '" + fechaAuxFin.ToString("yyyy-MM-dd") + @"') );";

                string orderBy = " ORDER BY MCLI.mnit_nit";

                //VEHICULOS QUE VISITRARON TALLER HACE 1 AÑO
                string select14 = @"SELECT CC, PLACA, VIN, CATALOGO, ULTIMA_ENTRADA, PROPIETARIO, Dirección, Teléfono, CELULAR, Ciudad, PVEN_CODIGO FROM 
                                        (
                                        SELECT MC.MNIT_NIT AS CC,
                                               MC.MCAT_PLACA AS PLACA,
                                               MC.MCAT_VIN AS VIN,
                                               PC.PCAT_DESCRIPCION || '' || PC.PCAT_CODIGO AS Catalogo,
                                               MO.MORD_ENTRADA AS Ultima_Entrada,
                                               VN.NOMBRE AS Propietario,
                                               MN.MNIT_DIRECCION AS Dirección,
                                               MN.MNIT_TELEFONO AS Teléfono,
                                               MN.MNIT_CELULAR Celular,
                                               PC.PCIU_NOMBRE AS Ciudad,
                                               MO.PVEN_CODIGO AS PVEN_CODIGO,
	                                           CASE
	                                           WHEN 
	                                           (SUBSTR(PC.PCAT_DESCRIPCION,1,3))='MAZ'
	                                           THEN 0 ELSE 1 
	                                           END AS ORDER_CAT
                                        FROM MORDEN MO,
                                             MCATALOGOVEHICULO MC,
                                             PCATALOGOVEHICULO PC,
                                             VMNIT VN,
                                             MNIT MN,
                                             PCIUDAD PC
                                        WHERE (DAYS (CURRENT DATE) - DAYS (MORD_ENTRADA)) BETWEEN 360 AND 365
                                        AND   TTRA_CODIGO IN ('M','R')
                                        AND   MO.MCAT_VIN = MC.MCAT_VIN
                                        AND   MC.PCAT_CODIGO = PC.PCAT_CODIGO
                                        AND   MC.MNIT_NIT = VN.MNIT_nIT
                                        AND   MC.MNIT_NIT = MN.MNIT_nIT
                                        AND   MN.PCIU_CODIGO = PC.PCIU_CODIGO
                                        ) AS vehiculos ORDER BY ORDER_CAT;";

                //VEHICULOS QUE VISITRARON TALLER HACE 2 AÑO
                string select15 = @"SELECT CC, PLACA, VIN, CATALOGO, ULTIMA_ENTRADA, PROPIETARIO, Dirección, Teléfono, CELULAR, Ciudad, PVEN_CODIGO FROM 
                                        (
                                        SELECT MC.MNIT_NIT AS CC,
                                               MC.MCAT_PLACA AS PLACA,
                                               MC.MCAT_VIN AS VIN,
                                               PC.PCAT_DESCRIPCION || '' || PC.PCAT_CODIGO AS Catalogo,
                                               MO.MORD_ENTRADA AS Ultima_Entrada,
                                               VN.NOMBRE AS Propietario,
                                               MN.MNIT_DIRECCION AS Dirección,
                                               MN.MNIT_TELEFONO AS Teléfono,
                                               MN.MNIT_CELULAR Celular,
                                               PC.PCIU_NOMBRE AS Ciudad,
                                               MO.PVEN_CODIGO AS PVEN_CODIGO,
	                                           CASE
	                                           WHEN 
	                                           (SUBSTR(PC.PCAT_DESCRIPCION,1,3))='MAZ'
	                                           THEN 0 ELSE 1 
	                                           END AS ORDER_CAT
                                        FROM MORDEN MO,
                                             MCATALOGOVEHICULO MC,
                                             PCATALOGOVEHICULO PC,
                                             VMNIT VN,
                                             MNIT MN,
                                             PCIUDAD PC
                                        WHERE (DAYS (CURRENT DATE) - DAYS (MORD_ENTRADA)) BETWEEN 760 AND 765
                                        AND   TTRA_CODIGO IN ('M','R')
                                        AND   MO.MCAT_VIN = MC.MCAT_VIN
                                        AND   MC.PCAT_CODIGO = PC.PCAT_CODIGO
                                        AND   MC.MNIT_NIT = VN.MNIT_nIT
                                        AND   MC.MNIT_NIT = MN.MNIT_nIT
                                        AND   MN.PCIU_CODIGO = PC.PCIU_CODIGO
                                        ) AS vehiculos ORDER BY ORDER_CAT;";


                //CLIENTES POTENCIALES
                string select16 = @"SELECT VM.MNIT_NIT AS CC, VM.NOMBRE AS NOMBRE, DM.PRES_CODIGO AS CODIGO, DM.DMAR_DETALLE AS DETALLE, DM.DMAR_FECHPROX AS FECHA_CONTACTAR, DM.PVEN_CODIGO AS PVEN_CODIGO, 
                                    PV1.PVEN_NOMBRE AS VENDEDOR, PV2.PVEN_NOMBRE AS MERCADERISTA, MN.MNIT_TELEFONO AS TELEFONO, MN.MNIT_CELULAR AS CELULAR, PC.PCIU_NOMBRE AS CIUDAD
                                    FROM VMNIT VM, DMARKETING DM, MNIT MN, PCIUDAD PC, PVENDEDOR PV1, PVENDEDOR PV2
                                    WHERE PRES_CODIGO = 'PT' AND VM.MNIT_NIT = DM.MNIT_NIT AND MN.MNIT_NIT = DM.MNIT_NIT AND MN.PCIU_CODIGO = PC.PCIU_CODIGO
                                    AND PV1.PVEN_CODIGO = DM.PVEN_CODIGO AND PV2.PVEN_CODIGO = DM.PVEN_CODMAR and dm.dmar_fechacti between  '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + @"' and '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + @"';";


                //VEHICULOS QUE VISITRARON TALLER HACE 6 MESES
                string select17 = @"SELECT CC, PLACA, VIN, CATALOGO, ULTIMA_ENTRADA, PROPIETARIO, Dirección, Teléfono, CELULAR, Ciudad, PVEN_CODIGO FROM 
                                        (
                                        SELECT MC.MNIT_NIT AS CC,
                                               MC.MCAT_PLACA AS PLACA,
                                               MC.MCAT_VIN AS VIN,
                                               PC.PCAT_DESCRIPCION || '' || PC.PCAT_CODIGO AS Catalogo,
                                               MO.MORD_ENTRADA AS Ultima_Entrada,
                                               VN.NOMBRE AS Propietario,
                                               MN.MNIT_DIRECCION AS Dirección,
                                               MN.MNIT_TELEFONO AS Teléfono,
                                               MN.MNIT_CELULAR Celular,
                                               PC.PCIU_NOMBRE AS Ciudad,
                                               MO.PVEN_CODIGO AS PVEN_CODIGO,
	                                           CASE
	                                           WHEN 
	                                           (SUBSTR(PC.PCAT_DESCRIPCION,1,3))='MAZ'
	                                           THEN 0 ELSE 1 
	                                           END AS ORDER_CAT
                                        FROM MORDEN MO,
                                             MCATALOGOVEHICULO MC,
                                             PCATALOGOVEHICULO PC,
                                             VMNIT VN,
                                             MNIT MN,
                                             PCIUDAD PC
                                        WHERE (DAYS (CURRENT DATE) - DAYS (MORD_ENTRADA)) BETWEEN 180 AND 185
                                        AND   TTRA_CODIGO IN ('M','R')
                                        AND   MO.MCAT_VIN = MC.MCAT_VIN
                                        AND   MC.PCAT_CODIGO = PC.PCAT_CODIGO
                                        AND   MC.MNIT_NIT = VN.MNIT_nIT
                                        AND   MC.MNIT_NIT = MN.MNIT_nIT
                                        AND   MN.PCIU_CODIGO = PC.PCIU_CODIGO
                                        ) AS vehiculos ORDER BY ORDER_CAT;";

                fechaCI = fechaInicial.SelectedDate.AddDays(-3);
                fechaCF = fechaFinal.SelectedDate.AddDays(-3);
                prefActividad = "E5";
                //CONTACTOS A 3 dias
                string select18 = "select mv.mcat_vin VIN," +
                    "mv.mnit_nit AS CC, mn.mnit_nombres CONCAT ' ' CONCAT COALESCE(mn.mnit_nombre2,'') CONCAT ' ' concat mn.mnit_apellidos CONCAT ' ' concat COALESCE(mn.mnit_apellido2,'') AS NOMBRE, cast(mv.mveh_fechentr as date) as fecha, " +
                    "mc.pcat_codigo concat ' / ' concat mv.mcat_vin concat ' / ' concat mc.mcat_placa as VEHI, pv.pven_nombre, PA.PALM_descripcion, " +
                    "mn.MNIT_EMAIL AS EMAIL,COALESCE(mn.MNIT_TELEFONO,'') CONCAT ' - ' CONCAT COALESCE(mn.MNIT_CELULAR,'') AS TELS, PV.PVEN_CODIGO AS PVEN_CODIGO " +
                    "from dbxschema.mvehiculo mv,dbxschema.mnit mn, dbxschema.mcatalogovehiculo mc, dbxschema.masignacionvehiculo ma," +
                    "dbxschema.mpedidovehiculo mp, PALMACEN PA, " +
                    "DBXSCHEMA.MFACTURAPEDIDOVEHICULO MFPV, " +
                    "MFACTURACLIENTE MFC, dbxschema.pvendedor pv " +
                    "where mn.mnit_nit=mv.mnit_nit and mc.mcat_vin=mv.mcat_vin and ma.mveh_inventario=mv.mveh_inventario and ma.pdoc_codigo=mp.pdoc_codigo " +
                    (vendedor.Length > 0 ? " AND PV.PVEN_CODIGO='" + vendedor + "' " : "") +
                    "AND   MFPV.MVEH_INVENTARIO = MV.MVEH_INVENTARIO " +
                    "AND MFC.PDOC_CODIGO = MFPV.PDOC_CODIGO AND MFC.MFAC_NUMEDOCU = MFPV.MFAC_NUMEDOCU " +
                    "AND PA.PALM_ALMACEN = MFC.PALM_ALMACEN " +
                    "AND mfac_tipclie = 'C' " +
                    "and mfc.palm_almacen = PA.palm_almacen" +
                    ((validar || Tipo.Equals("V")) ? " AND mv.mnit_nit not in(select dmi.mnit_nit from dbxschema.dmarketing dmi where dmi.dmar_fechacti >= '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "') " : " ") +
                    (!Tipo.Equals("V") ? " AND  mv.mnit_nit not in (SELECT DISTINCT d.mnit_nit FROM dmarketing d, pactividadmarketing p WHERE d.pact_codimark = p.pact_codimark AND p.tact_tipoacti = '" + prefActividad + "' AND d.dmar_fechacti BETWEEN '" + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + "' AND '" + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + "' ) " : " ") +
                    "and mv.mveh_fechentr between '" + fechaCI.ToString("yyyy-MM-dd") + " 00:00:00' and '" + fechaCF.ToString("yyyy-MM-dd") + " 00:00:00' " +
                    "and ma.mped_numepedi=mp.mped_numepedi and pv.pven_codigo=mp.pven_codigo order by mv.mnit_nit;";

                //lb.Text=select+from+where+orderBy;
                //lb.Text=select2+from2+where2+orderBy2;
                DBFunctions.Request(ds,IncludeSchema.NO,select0+from+where+orderBy);
				DBFunctions.Request(ds,IncludeSchema.NO,select1+from2+where2+orderBy);
				DBFunctions.Request(ds,IncludeSchema.NO,select2+from3+where3+orderBy);
				DBFunctions.Request(ds,IncludeSchema.NO,select3);
				DBFunctions.Request(ds,IncludeSchema.NO,select5);
				DBFunctions.Request(ds,IncludeSchema.NO,select6);
				DBFunctions.Request(ds,IncludeSchema.NO,select7);
				DBFunctions.Request(ds,IncludeSchema.NO,select8);
				DBFunctions.Request(ds,IncludeSchema.NO,select9);
				DBFunctions.Request(ds,IncludeSchema.NO,select10);
				DBFunctions.Request(ds,IncludeSchema.NO,select4); //tabla(10)
                DBFunctions.Request(ds,IncludeSchema.NO,select11);
                DBFunctions.Request(ds,IncludeSchema.NO,select12);
                DBFunctions.Request(ds, IncludeSchema.NO,select13);
                DBFunctions.Request(ds, IncludeSchema.NO, select14);
                DBFunctions.Request(ds, IncludeSchema.NO, select15);
                DBFunctions.Request(ds, IncludeSchema.NO, select16);
                DBFunctions.Request(ds, IncludeSchema.NO, select17);
                DBFunctions.Request(ds, IncludeSchema.NO, select18);
                // incluir los vins de campañas

                lbTitulo.Text = "PROFESIONES DE LOS CLIENTES ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[0].Rows.Count+" encontrados)";
				if(ds.Tables[0].Rows.Count>0)
				{
					dgReporte.DataSource=ds.Tables[0];
					dgReporte.DataBind();
					dgReporte.Visible=true;
                    lbTitulo.ForeColor = Color.DarkBlue;
				}
				else{
					dgReporte.Visible=false;
                    lbTitulo.ForeColor = Color.Gray;
                }

                lbTitulo2.Text = "FECHAS DE CUMPLEAÑOS DE LOS CLIENTES ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[1].Rows.Count + " encontrados)";
				if(ds.Tables[1].Rows.Count>0)
				{
					dgCumple.DataSource=ds.Tables[1];
					dgCumple.DataBind();
					dgCumple.Visible=true;
                    lbTitulo2.ForeColor = Color.DarkBlue;
				}
				else{
					dgReporte.Visible=false;
                    lbTitulo2.ForeColor=Color.Gray;
                }

                lbTitulo3.Text = "FECHAS DE ANIVERSARIO DE LOS CLIENTES ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[2].Rows.Count + " encontrados)"; ;
                if (ds.Tables[2].Rows.Count > 0)
                {
                    dgAniversario.DataSource = ds.Tables[2];
                    dgAniversario.DataBind();
                    dgAniversario.Visible = true;
                    lbTitulo3.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgAniversario.Visible = false;
                    lbTitulo3.ForeColor = Color.Gray;
                }

                lbTitulo4.Text = "FECHAS DE ACTIVIDADES EXTRAS CON LOS CLIENTES ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[3].Rows.Count + " encontrados)";
                if (ds.Tables[3].Rows.Count > 0)
                {
                    dgAct.DataSource = ds.Tables[3];
                    dgAct.DataBind();
                    dgAct.Visible = true;
                    lbTitulo4.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgAct.Visible = false;
                    lbTitulo4.ForeColor = Color.Gray;
                }

                lbTitulo5.Text = "CONTACTOS ACTUALES A 5 DIAS ENTREGA VEHICULO VENDIDO ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[4].Rows.Count + " encontrados)";
                if (ds.Tables[4].Rows.Count > 0)
                {
                    dgContacto1.DataSource = ds.Tables[4];
                    dgContacto1.DataBind();
                    dgContacto1.Visible = true;
                    lbTitulo5.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgContacto1.Visible = false;
                    lbTitulo5.ForeColor = Color.Gray;
                }

                lbTitulo6.Text = "CONTACTOS ACTUALES A 30 DIAS ENTREGA VEHICULO VENDIDO ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[5].Rows.Count + " encontrados)";
                if (ds.Tables[5].Rows.Count > 0)
                {
                    dgContacto2.DataSource = ds.Tables[5];
                    dgContacto2.DataBind();
                    dgContacto2.Visible = true;
                    lbTitulo6.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgContacto2.Visible = false;
                    lbTitulo6.ForeColor = Color.Gray;
                }

                lbTitulo7.Text = "CONTACTOS ACTUALES A 60 DIAS ENTREGA VEHICULO VENDIDO ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[6].Rows.Count + " encontrados)";
                if (ds.Tables[6].Rows.Count > 0)
                {
                    dgContacto2.DataSource = ds.Tables[6];
                    dgContacto2.DataBind();
                    dgContacto2.Visible = true;
                    lbTitulo7.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgContacto2.Visible = false;
                    lbTitulo7.ForeColor = Color.Gray;
                }
                lbTitulo8.Text = "CONTACTOS ACTUALES A 6 MESES ENTREGA VEHICULO VENDIDO ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[7].Rows.Count + " encontrados)";
                if (ds.Tables[7].Rows.Count > 0)
                {
                    dgContacto4.DataSource = ds.Tables[7];
                    dgContacto4.DataBind();
                    dgContacto4.Visible = true;
                    lbTitulo8.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgContacto4.Visible = false;
                    lbTitulo8.ForeColor = Color.Gray;
                }
                lbTitulo9.Text = "CONTACTOS ACTUALES A 1 AÑO ENTREGA VEHICULO VENDIDO ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[8].Rows.Count + " encontrados)";
                if (ds.Tables[8].Rows.Count > 0)
                {
                    dgContacto5.DataSource = ds.Tables[8];
                    dgContacto5.DataBind();
                    dgContacto5.Visible = true;
                    lbTitulo9.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgContacto5.Visible = false;
                    lbTitulo9.ForeColor = Color.Gray;
                }
                lbTitulo10.Text = "PLANES DE MANTENIMIENTO PROGRAMADO A VEHICULOS ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[9].Rows.Count + " encontrados)";
                if (ds.Tables[9].Rows.Count > 0)
                {
                                       
                    dgMantenimiento.DataSource = ds.Tables[9];
                    dgMantenimiento.DataBind();
                    dgMantenimiento.Visible = true;
                    lbTitulo10.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgMantenimiento.Visible = false;
                    lbTitulo10.ForeColor = Color.Gray;
                }
                lbTitulo0.Text = "CONTACTOS ACTUALES A 2 DIAS SALIDA VEHICULO DEL TALLER ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[10].Rows.Count + " encontrados)";
                if (ds.Tables[10].Rows.Count > 0)
                {
                    dgContacto0.DataSource = ds.Tables[10];
                    dgContacto0.DataBind();
                    dgContacto0.Visible = true;
                    lbTitulo0.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgContacto0.Visible = false;
                    lbTitulo0.ForeColor = Color.Gray;
                }
                lblTitulo11.Text = "DOCUMENTOS DE VEHICULOS A VENCER ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[11].Rows.Count + " encontrados)";
                if (ds.Tables[11].Rows.Count > 0)
                {
                    dgDocsVencen.DataSource = ds.Tables[11];
                    dgDocsVencen.DataBind();
                    dgDocsVencen.Visible = true;
                    lblTitulo11.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgDocsVencen.Visible = false;
                    lblTitulo11.ForeColor = Color.Gray;
                }
                //lblTitulo12.Text = "CREDITOS DE VEHICULO A VENCER ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + Convert.ToInt16(ds.Tables[12].Rows.Count.ToString()) + " encontrados)";
                if (ds.Tables[12].Rows.Count > 0)
                {
                    dgrCredsVencen.DataSource = ds.Tables[12];
                    dgrCredsVencen.DataBind();
                    dgrCredsVencen.Visible = true;
                    lblTitulo12.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgrCredsVencen.Visible = false;
                    lblTitulo12.ForeColor = Color.Gray;
                }

                if (ds.Tables.Count >= 14)
                {
                    lblTitulo13.Text = "RECOMENDACIONES TECNICAS OT ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + Convert.ToInt16(ds.Tables[13].Rows.Count.ToString()) + " encontrados)";
                    if (ds.Tables[13].Rows.Count > 0)
                    {
                        dgrRecomendaciones.DataSource = ds.Tables[13];
                        dgrRecomendaciones.DataBind();
                        dgrRecomendaciones.Visible = true;
                        lblTitulo13.ForeColor = Color.DarkBlue;
                    }
                    else
                    {
                        dgrRecomendaciones.Visible = false;
                        lblTitulo13.ForeColor = Color.Gray;
                    }
                }

                lblTitulo14.Text = "VEHICULOS QUE VISITARON EL TALLER HACE 1 AÑO " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[14].Rows.Count + " encontrados)";
                if (ds.Tables[14].Rows.Count > 0)
                {
                    dgrvehiculosunano.DataSource = ds.Tables[14];
                    dgrvehiculosunano.DataBind();
                    dgrvehiculosunano.Visible = true;
                    lblTitulo14.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgrvehiculosunano.Visible = false;
                    lblTitulo14.ForeColor = Color.Gray;
                }

                lblTitulo15.Text = "VEHICULOS QUE VISITARON EL TALLER HACE 2 AÑOS " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[15].Rows.Count + " encontrados)";
                if (ds.Tables[15].Rows.Count > 0)
                {
                    dgrdosanos.DataSource = ds.Tables[15];
                    dgrdosanos.DataBind();
                    dgrdosanos.Visible = true;
                    lblTitulo15.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgrdosanos.Visible = false;
                    lblTitulo15.ForeColor = Color.Gray;
                }

                lblTitulo16.Text = "CLIENTES POTENCIALES " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[16].Rows.Count + " encontrados)";
                if (ds.Tables[16].Rows.Count > 0)
                {
                    dgrclipotencial.DataSource = ds.Tables[16];
                    dgrclipotencial.DataBind();
                    dgrclipotencial.Visible = true;
                    lblTitulo16.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgrclipotencial.Visible = false;
                    lblTitulo16.ForeColor = Color.Gray;
                }

                lblTitulo17.Text = "VEHICULOS QUE VISITARON EL TALLER HACE 6 MESES " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[17].Rows.Count + " encontrados)";
                if (ds.Tables[17].Rows.Count > 0)
                {
                    dgrseismeses.DataSource = ds.Tables[17];
                    dgrseismeses.DataBind();
                    dgrseismeses.Visible = true;
                    lblTitulo17.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgrdosanos.Visible = false;
                    lblTitulo17.ForeColor = Color.Gray;
                }

                lbTitulo19.Text = "CONTACTOS ACTUALES A 3 DIAS ENTREGA VEHICULO VENDIDO ENTRE " + fechaInicial.SelectedDate.ToString("yyyy-MM-dd") + " y " + fechaFinal.SelectedDate.ToString("yyyy-MM-dd") + " (" + ds.Tables[4].Rows.Count + " encontrados)";
                if (ds.Tables[18].Rows.Count > 0)
                {
                    dgdias.DataSource = ds.Tables[18];
                    dgdias.DataBind();
                    dgdias.Visible = true;
                    lbTitulo19.ForeColor = Color.DarkBlue;
                }
                else
                {
                    dgdias.Visible = false;
                    lbTitulo19.ForeColor = Color.Gray;
                }


                toolsHolder.Visible=true;
				lbRep.Visible=true;
				phReporte.Visible=true;
				Session["rep"]=RenderHtml();
				pnlExcel.Visible=true;
			}
			Session["MARKFLT_FECHADESDE"]=fechaInicial.SelectedDate;
			Session["MARKFLT_FECHAHASTA"]=fechaFinal.SelectedDate;
			Session["MARKFLT_VENDEDOR"]=ddlVendedor.SelectedValue;
            if(Tipo.Equals("V"))
                Session["MARKFLT_CLAVE"] = txtClave.Text;
		}
		
		protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
        	phReporte.RenderControl(htmlTW);
        	return SB.ToString();
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
   		  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
     		MyMail.To = tbEmail.Text;
			MyMail.Subject = "Proceso : Reporte de Actividades de los Clientes";
			MyMail.Body = (RenderHtml());
      		MyMail.BodyFormat = MailFormat.Html;
			try{
    	   		SmtpMail.Send(MyMail);}
    		catch(Exception e){
    	 	  lb.Text = e.ToString();
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
