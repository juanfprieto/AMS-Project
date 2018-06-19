using System;
using System.Configuration;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using AMS.DB;
using AMS.Forms;
using eWorld.UI;
using AMS.Tools;
using AMS.Documentos;

namespace AMS.Contabilidad
{
	public partial class CapEdiCom : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataGrid dgInserts;
		//protected DropDownList compCOBOL;
		protected PlaceHolder comprobHolder;
		protected TextBox textBox4;
		protected OdbcDataReader dr1;
		protected DataTable dtInserts;
		protected DataRow dr;
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
		protected int i, j;
		#endregion

		#region Eventos
		protected void Page_Init(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
                if (Request.QueryString["pref"] != null && Request.QueryString["num"]!= null)
                {
                    FormatosDocumentos formatoRecibo = new FormatosDocumentos();
                    formatoRecibo.Prefijo = Request.QueryString["pref"];
                    formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pref"] + "'");
                    formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["num"].ToString());
                    if (formatoRecibo.Cargar_Formato())
                        Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                    if(Request.QueryString["falloDoc"] == "1")
                    {
                        Utils.MostrarAlerta(Response, "Se ha realizado el proceso correctamente!! pero por un eror desconocido no ha sido posible archivar los documentos soporte, por favor contacte a ECAS");
                    }
                }

				DatasToControls bind = new DatasToControls();
                string yr   = DBFunctions.SingleData("SELECT pano_ano FROM ccontabilidad");
                Int16 año = Convert.ToInt16(yr);
                bind.PutDatasIntoDropDownList(year, "SELECT pano_ano, PANO_DETALLE FROM pano where pano_ano >= "+año+" order by 1 desc");
                DatasToControls.EstablecerDefectoDropDownList(year, yr);
                bind.PutDatasIntoDropDownList(month, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 0 AND pmes_mes != 13 order by 1");
				string mt   = DBFunctions.SingleData("SELECT pmes_mes FROM ccontabilidad");
				DatasToControls.EstablecerDefectoDropDownList(month,DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes="+mt+""));
				//month.SelectedIndex=month.Items.IndexOf(new ListItem(mt,mt));
				maVig.Text  = "  "+month.SelectedItem+" - "+yr+" ";
                // Descarta los documentos que no esten vigentes y tambien los que NO son contables como las ordenes de trabajo o producción, cotizaciones y pedidos
                bind.PutDatasIntoDropDownList(typeDoc, "SELECT pdoc_codigo,pdoc_codigo concat ' - ' concat pdoc_nombre FROM pdocumento where tvig_vigencia='V' and tdoc_tipodocu not in ('CT','CV','PC','PV','PI','OP','OT') order by pdoc_CODIGO");
                //Utils.llenarPrefijos(Response, ref typeDoc, "%", "%", "%");
                //bind.PutDatasIntoDropDownList(typePlant, "SELECT pdoc_codigo,pdoc_codigo concat ' - ' concat pdoc_nombre FROM pdocumento where tvig_vigencia is null or tvig_vigencia='V' order by pdoc_nombre");
                Utils.llenarPrefijos(Response, ref typePlant, "%", "%", "%");
                bind.PutDatasIntoDropDownList(compPlant, "SELECT mcom_numedocu FROM mcomprobante WHERE pdoc_codigo='" + typePlant.SelectedValue + "'");
				string pathToImports = ConfigurationManager.AppSettings["PathToImports"] + "comprobantes";
				//DatasToControls bind4 = new DatasToControls("");
				//bind.BindFromDirIntoLB(compCOBOL, pathToImports);
				IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				DateTime nd=DateTime.Now;
				try{nd=new DateTime(Convert.ToInt16(year.SelectedValue),Convert.ToInt16(month.SelectedValue),1);}catch{};
				//Button1.Attributes.Add("onclick","return ValidarFechaIngresoComprobante();");
                /*  validador de vigencia de documentos
                    PDOC_CODIGO ||'-'||PDOC_NOMBRE ||' LA RESOLUCION DE FACTURACION ESTA POR ACABARSE, FALTAN ' || (PDOC_NUMEFINA - PDOC_ULTIDOCU) ||' DOCUMENTOS' 
                    FROM PDOCUMENTO WHERE (PDOC_NUMEFINA - PDOC_ULTIDOCU) IN (100,75,50,25,15,10,5,4,3,2,1) AND TDOC_TIPODOCU = 'FC';

                    SELECT PDOC_CODIGO||'-'|| PDOC_NOMBRE||' LA FECHA RESOLUCION DE FACTURACION ESTA POR VENCERSE, FALTAN ', DAYS(CURRENT DATE) -DAYS(PDOC_FINFECHRESOFACT) , ' DIAS' 
                    FROM PDOCUMENTO WHERE (DAYS(CURRENT DATE) -DAYS(PDOC_FINFECHRESOFACT)) IN (30,20,15,10,5,4,3,2,1) AND TDOC_TIPODOCU = 'FC';

                 */
            }
		}

        protected void NewComp(Object Sender, EventArgs E)
        {
            string mensajeError = "";
            // validamos la consistencia de las cuantas automáticas de ccontabilidad
            string cuentaUtilidad = DBFunctions.SingleData("select mcue_codipuc from ccontabilidad");
            if (cuentaUtilidad.Substring(0, 4) != "3605")
            {
                mensajeError += "Cuenta Real de utilidad del ejercicio NO está configurada del grupo 3605nn \\n";
            }
            cuentaUtilidad = DBFunctions.SingleData("select mcue_codipucacum from ccontabilidad");
            if (cuentaUtilidad.Substring(0, 4) != "3705")
            {
                mensajeError += "Cuenta Real de utilidad acumulado NO está configurada del grupo 3705nn \\n";
            }
            cuentaUtilidad = DBFunctions.SingleData("select mcue_codipucnomi from ccontabilidad");
            if (cuentaUtilidad.Substring(0, 4) != "5905")
            {
                mensajeError += "Cuenta Nominal de la utilidad NO está configurada del grupo 5905nn \\n";
            }
            try
            { 
                cuentaUtilidad = DBFunctions.SingleData("select mcue_codipuc_AJUSPESO from ccontabilidad");
            }
            catch
            {
                cuentaUtilidad = "";
                mensajeError += "NO se ha creado el campo para definir la cuenta de AJUSTE AL PESO, llamar a ECAS ! \\n";
            }
            if (cuentaUtilidad.Length == 0 || (cuentaUtilidad.Substring(0, 1) != "4" && cuentaUtilidad.Substring(0, 1) != "5"))
            {
                mensajeError += "Cuenta de AJUSTE AL PESO NO está configurada del grupo 4nnn o 5nnn \\n";
            }

            string cuentasNIIf = @"SELECT MCUE_CODIPUC, MCUE_CODIPUCniif FROM MCUENTA 
                                    WHERE (TCUE_CODIGO = 'F' AND MCUE_CODIPUCNIIF NOT IN (SELECT MCUE_CODIPUC FROM MCUENTA WHERE TIMP_CODIGO <> 'N' ))
                                       OR (MCUE_CODIPUC <> MCUE_CODIPUCNIIF AND TCUE_CODIGO <> 'F' AND MCUE_CODIPUCNIIF <> '' AND MCUE_CODIPUCNIIF IS NOT NULL);";
            DataSet dscuentasNiif = new DataSet();
            DBFunctions.Request(dscuentasNiif, IncludeSchema.NO, cuentasNIIf);
            if (dscuentasNiif.Tables.Count > 0)
            {
                for (i = 0; i < dscuentasNiif.Tables[0].Rows.Count; i++)
                {
                    mensajeError += "La cuenta " + dscuentasNiif.Tables[0].Rows[i][0] + " está mal Configurada, NO es SOLO FISCAL o la cuenta NIIF EQUIVALENTE " + dscuentasNiif.Tables[0].Rows[i][1] + " NO es válida, modifique el PUC !! \\n";
                }
            }


            // validación de los saldos de las cuentas vs los auxiliares
            DataSet dsAuxiliares = new DataSet();
            string auxiliares = @"select * from (SELECT M.MCUE_CODIPUC, MS.PANO_ANO, MS.PMES_MES, MSAL_VALODEBI - MSAL_VALOCRED AS BALANCE,  
                                 SUM (MAUX_VALODEBI - MAUX_VALOCRED) AS AUXILIAR,  
                                 (MSAL_VALODEBI - MSAL_VALOCRED) - SUM(MAUX_VALODEBI - MAUX_VALOCRED) AS DIFER  
                                 FROM CCONTABILIDAD CC, MCUENTA M, MSALDOCUENTA MS left join MAUXILIARCUENTA MA  
                                 on Ms.MCUE_CODIPUC = MA.MCUE_CODIPUC  AND MS.PANO_ANO = MA.PANO_ANO AND MS.PMES_MES = MA.PMES_MES 
                                 WHERE M.MCUE_CODIPUC = MS.MCUE_CODIPUC AND M.TIMP_CODIGO = 'A' AND MS.PANO_ANO >= CC.PANO_ANO
                                 GROUP BY M.MCUE_CODIPUC,MS.PANO_ANO , MS.PMES_MES, MSAL_VALODEBI, MSAL_VALOCRED  
                                  ) a where a.difer <> 0 ORDER BY MCUE_CODIPUC; ";
            //  la condicion 
            //  AND MS.Pmes_mes > 0 
            //  VA ANTES DEL GROUP BY Y es temporal en fujiyama mientras se suben los saldos INICIALES correctos y cuadrados
         
            DBFunctions.Request(dsAuxiliares, IncludeSchema.NO, auxiliares);
            if (dsAuxiliares.Tables.Count > 0)
            {
                for (i = 0; i < dsAuxiliares.Tables[0].Rows.Count; i++)
                {
                    mensajeError += "Auxiliar descuadrado.. Cuenta " + dsAuxiliares.Tables[0].Rows[i][0] + "   " + dsAuxiliares.Tables[0].Rows[i][1] + "   " + dsAuxiliares.Tables[0].Rows[i][2] + "  Mayorice !!! \\n";
                }
            }

            // validación de los niveles del puc
            DataSet dsnivelesPuc = new DataSet();
            string nivelesPuc = @"SELECT * FROM (select * from mcuenta where substr(mcue_codipuc,2,1) = ''            and (tniv_CODIGO <> 1 OR TIMP_CODIGO <> 'N')  
                                 or substr(mcue_codipuc,3,1) = '' and substr(mcue_codipuc,2,1) <> ''  and (tniv_CODIGO <> 2 OR TIMP_CODIGO <> 'N')  
                                 or substr(mcue_codipuc,5,1) = '' and substr(mcue_codipuc,3,1) <> ''  and (tniv_CODIGO <> 3 OR TIMP_CODIGO <> 'N')  
                                 or substr(mcue_codipuc,7,1) = '' and substr(mcue_codipuc,5,1) <> ''  and tniv_codigo <> 4  
                                 or substr(mcue_codipuc,9,1) = '' and substr(mcue_codipuc,7,1) <> ''  and tniv_codigo <> 5  
                                 or substr(mcue_codipuc,11,1) = '' and substr(mcue_codipuc,9,1) <> '' and tniv_codigo <> 6  
                                 or substr(mcue_codipuc,13,1) = '' and substr(mcue_codipuc,11,1) <> '' and tniv_codigo <> 7  
                                 or substr(mcue_codipuc,15,1) = '' and substr(mcue_codipuc,13,1) <> '' and tniv_codigo <> 8 
                                 or substr(mcue_codipuc,15,1) <> ''                                    and tniv_codigo <> 9   
                                    UNION
                                select cm.* from mcuenta cm, mcuenta ci 
								 where (cm.mcue_codipuc = substr(ci.mcue_codipuc,1,4) and substr(ci.mcue_codipuc,5,2) <> '' and cm.TIMP_CODIGO <> 'N')
								    or (cm.mcue_codipuc = substr(ci.mcue_codipuc,1,6) and substr(ci.mcue_codipuc,7,2) <> '' and cm.TIMP_CODIGO <> 'N')
								    or (cm.mcue_codipuc = substr(ci.mcue_codipuc,1,8) and substr(ci.mcue_codipuc,9,2) <> '' and cm.TIMP_CODIGO <> 'N')
									or (cm.mcue_codipuc = substr(ci.mcue_codipuc,1,10) and substr(ci.mcue_codipuc,11,2) <> '' and cm.TIMP_CODIGO <> 'N')
                                    or (cm.mcue_codipuc = substr(ci.mcue_codipuc,1,12) and substr(ci.mcue_codipuc,13,2) <> '' and cm.TIMP_CODIGO <> 'N')
                                    or (cm.mcue_codipuc = substr(ci.mcue_codipuc,1,14) and substr(ci.mcue_codipuc,15,2) <> '' and cm.TIMP_CODIGO <> 'N')
                                ) AS A								 
                                order by 1; ";
            DBFunctions.Request(dsnivelesPuc, IncludeSchema.NO, nivelesPuc);
            if (dsnivelesPuc.Tables.Count > 0)
            {
                for (i = 0; i < dsnivelesPuc.Tables[0].Rows.Count; i++)
                {
                    mensajeError += "Nivel Imputación " + dsnivelesPuc.Tables[0].Rows[i][2] + " ó Imputabilidad Contable " + dsnivelesPuc.Tables[0].Rows[i][3] +  ", errado en Cuenta " + dsnivelesPuc.Tables[0].Rows[i][0] + " " + dsnivelesPuc.Tables[0].Rows[i][1] + "  Corrija el PUC !!! \\n";
                }
            }

            // validación de las clases de cuenta (real - nominal)
            DataSet dsclasesPuc = new DataSet();
            string clasePuc = @"select * from mcuenta where substr(mcue_codipuc,1,1) IN ('1','2','3') AND TCLA_CODIGO <> 'R' 
						        OR substr(mcue_codipuc,1,1) IN ('4','5','6','7') AND TCLA_CODIGO <> 'N' order by 1;  ";
            DBFunctions.Request(dsclasesPuc, IncludeSchema.NO, clasePuc);
            if (dsclasesPuc.Tables.Count > 0)
            {
                for (i = 0; i < dsclasesPuc.Tables[0].Rows.Count; i++)
                {
                    mensajeError += "Clase de Cuenta Contable " + dsclasesPuc.Tables[0].Rows[i][10] + " errada en Cuenta " + dsclasesPuc.Tables[0].Rows[i][0] + " " + dsclasesPuc.Tables[0].Rows[i][1] + "   Corrija el PUC !!! \\n";
                }
            }

            // validación de la jerarquia de cuentas MADRES con cuentas HIJAS
            DataSet DScuentasHijas = new DataSet();
            string cuentasHijas = @"select * from (
                                 select mh.mcue_codipuc as hija, MH.MCUE_NOMBRE, timp_nombre, mp.mcue_codipuc as mama, MH.MCUE_NOMBRE as NOM
                                   from TIMPUTACONTABILIDAD t, mcuenta mh
	                                 left join mcuenta mp on substr(mh.mcue_codipuc,1,8) = mp.mcue_codipuc 
                                 where MH.timp_codigo = t.timp_codigo
                                   union
                                 select mh.mcue_codipuc as hija, MH.MCUE_NOMBRE, timp_nombre, mp.mcue_codipuc as mama, MH.MCUE_NOMBRE as NOM
                                   from TIMPUTACONTABILIDAD t, mcuenta mh
	                                 left join mcuenta mp on substr(mh.mcue_codipuc,1,6) = mp.mcue_codipuc 
                                 where MH.timp_codigo = t.timp_codigo
                                   union
                                 select mh.mcue_codipuc as hija, MH.MCUE_NOMBRE, timp_nombre, mp.mcue_codipuc as mama, MH.MCUE_NOMBRE as NOM
                                   from TIMPUTACONTABILIDAD t, mcuenta mh
	                                 left join mcuenta mp on substr(mh.mcue_codipuc,1,4) = mp.mcue_codipuc 
                                 where MH.timp_codigo = t.timp_codigo
                                   union
                                 select mh.mcue_codipuc as hija, MH.MCUE_NOMBRE, timp_nombre, mp.mcue_codipuc as mama, MH.MCUE_NOMBRE as NOM
                                   from TIMPUTACONTABILIDAD t, mcuenta mh
	                                 left join mcuenta mp on substr(mh.mcue_codipuc,1,2) = mp.mcue_codipuc 
                                 where MH.timp_codigo = t.timp_codigo
                                   union
                                 select mh.mcue_codipuc as hija, MH.MCUE_NOMBRE, timp_nombre, mp.mcue_codipuc as mama, MH.MCUE_NOMBRE as NOM
                                   from TIMPUTACONTABILIDAD t, mcuenta mh
	                                 left join mcuenta mp on substr(mh.mcue_codipuc,1,1) = mp.mcue_codipuc 
                                 where MH.timp_codigo = t.timp_codigo) as a
                                where mama is null	 
                                order by 1 ; ";
            DBFunctions.Request(DScuentasHijas, IncludeSchema.NO, cuentasHijas);
            if (DScuentasHijas.Tables.Count > 0)
            {
                for (i = 0; i < DScuentasHijas.Tables[0].Rows.Count; i++)
                {
                    mensajeError += "Cuenta en el P U C  " + DScuentasHijas.Tables[0].Rows[i][0] + "   " + DScuentasHijas.Tables[0].Rows[i][1] + "   " + DScuentasHijas.Tables[0].Rows[i][2] + "  SIN cuenta PADRE,    Corrija el PUC !!! \\n";
                }
            }

            // validación de cuentas NO IMPUTABLES que tienen saldo
            DataSet DScuentasSaldo = new DataSet();
            string cuentasSaldo = @"select ms.* from mcuenta m, msaldocuenta ms, ccontabilidad cc
                                    where m.mcue_codipuc = ms.mcue_codipuc and (ms.msal_valodebi <> 0 or msal_valocred <> 0) and timp_codigo = 'N' AND ms.PANO_ANO >= cc.pano_ano
                                    ORDER BY 3,1;";
            DBFunctions.Request(DScuentasSaldo, IncludeSchema.NO, cuentasSaldo);
            if (DScuentasSaldo.Tables.Count > 0)
            {
                for (i = 0; i < DScuentasSaldo.Tables[0].Rows.Count; i++)
                {
                    mensajeError += "Cuenta NO Imputable " + DScuentasSaldo.Tables[0].Rows[i][2] + " , tiene movimiento, debe RECLASIFICAR ESTA CUENTA y volver a CREARLA !!! ";
                }
            }

            // validación de vigencias y rangos de prefijos de FACTURACION
            DataSet DSprefijos = new DataSet();
            string prefijos = @"SELECT * FROM (select P.PDOC_CODIGO, PDOC_NOMBRE, DAYS(PDOC_FINFECHRESOFACT) - DAYS(CURRENT DATE) AS DIAS,  PDOC_NUMEFINA - PDOC_ULTIDOCU AS RANGO 
                                    FROM pdocumentohecho ph, pdocumento p
                                    where p.pdoc_codigo = ph.pdoc_codigo and tdoc_tipodocu = 'FC') AS A
                                 WHERE abs(DIAS) IN (30,20,10,5,4,3,2,1) OR abs(RANGO) IN (100,50,25,10,5,4,3,2,1)  
                                    ORDER BY 1,2;";
            DBFunctions.Request(DSprefijos, IncludeSchema.NO, prefijos);
            if (DSprefijos.Tables.Count > 0)
            {
                for (i = 0; i < DSprefijos.Tables[0].Rows.Count; i++)
                {
                    mensajeError += "Al prefijo de Facturación " + DSprefijos.Tables[0].Rows[i][0] + " " +DSprefijos.Tables[0].Rows[i][1] + " le quedan " + DSprefijos.Tables[0].Rows[i][2] + " días de vigencia y " + DSprefijos.Tables[0].Rows[i][3] + " numeros de rango disponible. Actualice esta resolución, o desvincule este prefijo si ya no lo aplica !!! ";
                }
            }

            if (mensajeError.Length > 0)
            {
                Utils.MostrarAlerta(Response, mensajeError);
                return;
            }
         
            //Debemos validar que la fecha del comprobante no sea menor a mes y año vigente
            //Creamos un DateTime con el primer dia de la fecha y mes vigente
            DateTime fechaVigente  = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT pano_ano FROM ccontabilidad") + "-" + DBFunctions.SingleData("SELECT pmes_mes FROM ccontabilidad") + "-01");
            DateTime fechaEscogida = cpFechaComp.SelectedDate;
            if (fechaEscogida < fechaVigente)
            {
                if (!DBFunctions.RecordExist("SELECT TTIPE_CODIGO FROM SUSUARIO WHERE TTIPE_CODIGO = 'AS' AND SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name + "' "))
                {
                    Utils.MostrarAlerta(Response, "Fecha menor al Mes y Año Vigente");
                    return;
                }
                else
                    Utils.MostrarAlerta(Response, "Sr(a) Contador(a), la Fecha del documnento es menor a la vigencia contable, su perfíl si permite este proceso");

            }
            if ((year.SelectedValue.ToString() != fechaEscogida.Year.ToString()) || (month.SelectedValue.ToString() != fechaEscogida.Month.ToString()))
                {
                    Utils.MostrarAlerta(Response, "Mes o Año Selecionados diferentes a la Fecha Seleccionada");
                }
                else
                {
                    string y = year.SelectedValue.ToString();
                    string m = month.SelectedItem.Value;
                    string indexPage  = ConfigurationManager.AppSettings["MainIndexPage"];
                    string referencia = typeDoc.SelectedItem.Text;

                    Session["archivos"] = null;
                    if (chkPlant.Checked)
                    {
                        if (compPlant.Items.Count > 0)
                            Response.Redirect("" + indexPage + "?process=Contabilidad.CompGrid&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&year=" + y + "&month=" + m + "&date=" + cpFechaComp.SelectedDate + "&typeNum=" + typeDoc.SelectedValue + "&consecutivo=" + rblConsecutivo.SelectedValue + "&ref=" + referencia + "&action=new" + "&plant=" + chkPlant.Checked + "&tPlant=" + typePlant.SelectedItem.Value + "&cPlant=" + compPlant.SelectedItem.Value);
                        else
                            infoLabel.Text = "No existe ningún comprobante plantilla";
                    }
                    else
                    {
                        Response.Redirect("" + indexPage + "?process=Contabilidad.CompGrid&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&year=" + y + "&month=" + m + "&date=" + cpFechaComp.SelectedDate + "&typeNum=" + typeDoc.SelectedValue + "&consecutivo=" + rblConsecutivo.SelectedValue + "&ref=" + referencia + "&action=new" + "&plant=" + chkPlant.Checked);
                    }
                }
            }
        


		protected void Cambio_ItemP(Object sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			string parametro = typePlant.SelectedItem.Text;
			compPlant.Items.Clear();
			bind.PutDatasIntoDropDownList(compPlant,"SELECT mcom_numedocu FROM mcomprobante WHERE pdoc_codigo='"+typePlant.SelectedValue+"';");
		}

		#endregion

		#region Metodos
		protected string Cargar_Archivo_ComprobanteXML(string nombreArchivo)
		{
			int i,j;
			string error = "";
			//primero debemos leer el schema xml correspondiente a los comprobantes
			//primero se trae el nombre del xml schema
			string nombreArchivoSchema = ConfigurationManager.AppSettings["PathToSchemas"]+"Comprobante.xsd";
			DataSet dsXml = new DataSet();
			dsXml.ReadXmlSchema(nombreArchivoSchema);
			//Ahora tratamos de leer el archivo xml que contiene el comprobante a importar
			try
            {
				dsXml.ReadXml(nombreArchivo, XmlReadMode.ReadSchema);
			}
			catch(Exception e)
            {
				error = "Documento XML No Valido : <br>"+e.ToString();
			}
			//Debemos revisar si es un el archivo que cargo es valido o no de acuerdo al schema
			if(dsXml.Tables[1].Rows.Count==0)
				error = "Documento XML No Valido";
			else
			{
				//Aqui vamos a cargar los comprobantes que vengan dentro del documento xml
				DataRow[] filas;
				for(i=0;i<dsXml.Tables[1].Rows.Count;i++)
				{
					if(!DBFunctions.RecordExist("SELECT * FROM mcomprobante WHERE pdoc_codigo='"+dsXml.Tables[1].Rows[i]["prefijo"].ToString()+"' AND mcom_numedocu="+dsXml.Tables[1].Rows[i]["numero"].ToString()+""))
					{
						decimal debitos=0, creditos=0;
						filas = dsXml.Tables[2].Select("comprobanteEspecifico_Id = "+dsXml.Tables[1].Rows[i]["comprobanteEspecifico_Id"].ToString()+"");
						InitDataTable();
						for(j=0;j<filas.Length;j++)
						{
							debitos += Convert.ToDecimal(filas[j]["debito"].ToString().Replace(",", "."));
							creditos += Convert.ToDecimal(filas[j]["credito"].ToString().Replace(",", "."));
							DataRow row = dtInserts.NewRow();
							row[0] = filas[j][0];
							row[1] = filas[j][1];
							row[2] = filas[j][2];
							row[3] = filas[j][3];
							row[4] = filas[j][4];
							row[5] = filas[j][5];
							row[6] = filas[j][6];
							row[7] = filas[j][7];
							row[8] = filas[j][8];
							row[9] = filas[j][9];
							dtInserts.Rows.Add(row);
						}
						if((debitos - creditos) == 0)
						{
							Comprobante cp = new Comprobante();
							cp.Source = dtInserts;
							cp.Type   = dsXml.Tables[1].Rows[i]["prefijo"].ToString();
		 					cp.Number = dsXml.Tables[1].Rows[i]["numero"].ToString();
			 				cp.Year   = dsXml.Tables[1].Rows[i]["ano"].ToString();
			 				cp.Month  = dsXml.Tables[1].Rows[i]["mes"].ToString();
			 				cp.Detail = dsXml.Tables[1].Rows[i]["razon"].ToString();
		 					cp.Date   = dsXml.Tables[1].Rows[i]["fecha"].ToString();
		 					cp.Value  = debitos.ToString();
                            ArrayList sqlStrings = new ArrayList();
                            if (cp.CommitValues(ref sqlStrings))
                            {
                                infoLabel.Text += "el comprobante: " + dsXml.Tables[1].Rows[i]["numero"].ToString() + " se ha importado<br>";
                                if (DBFunctions.Transaction(sqlStrings))
                                    infoLabel.Text += "<BR>guardado";
                                else
                                    infoLabel.Text += "Error al grabar.";
                            }
                            else
                                infoLabel.Text += "<br>el comprobante: " + dsXml.Tables[1].Rows[i]["numero"].ToString() + " no pudo ser importado<br>" + cp.ProcessMsg;
						}
						else
							infoLabel.Text += "el comprobante: " + dsXml.Tables[1].Rows[i]["numero"].ToString() + " no tiene sumas iguales<br>";
					}
					else
						infoLabel.Text += "<br>El comprobante de tipo "+dsXml.Tables[1].Rows[i]["prefijo"].ToString()+" y "+dsXml.Tables[1].Rows[i]["numero"].ToString()+" numero ya existe en la base de datos";
				}
			}
			return error;
		}

        protected bool CheckVals(string[] datas, string what)
		{
			bool correct = false;
			int checks = 0;
			//DbFunctions check = new DbFunctions();

			if(what == "head")
			{
				checks = 0;
				string prefijo =  datas[0].Split(":".ToCharArray())[1].Trim() ;
				if(DBFunctions.RecordExist( "select pdoc_codigo from pdocumento where pdoc_codigo = '" + prefijo + "'"))
				{
					checks++;
					//infoLabel.Text += datas[0].Split(":".ToCharArray())[1].Trim() + " existe en pdocumento.<br>";
				}
				else
					infoLabel.Text += "error: " + datas[0].Split(":".ToCharArray())[1].Trim() + " no existe como tipo de documento.<br>";

				//if(!check.CheckRecord("mcomprobante", "mcom_numedocu", datas[1].Trim()))
                if (DBFunctions.RecordExist("select mcom_numedocu from mcomprobante where pdoc_codigo = '" + prefijo + "' and mcom_numedocu = '" + datas[1].Trim() + "'"))
				{
					checks++;
					//infoLabel.Text += datas[1].Trim() + " est disponible en mcomprobante.<br>";
				}
				else
					infoLabel.Text += "error: " + datas[1].Trim() + " ya existe como consecutivo.<br>";

				if(checks == 2)
					correct = true;
			}
			if(what == "body")
			{
				checks = 0;
				//if(check.CheckRecord("mcuenta", "mcue_codipuc", "'" + datas[0].Trim() + "'"))
				if(DBFunctions.RecordExist("select mcue_codipuc from mcuenta where mcue_codipuc = " + "'" + datas[0].Trim() + "'"))
				{
					checks++;
					//infoLabel.Text += datas[0].Trim() + " existe en mcuenta<br>";
				}
				else
					infoLabel.Text += "error: " + datas[0].Trim() + " no existe en el plan de cuentas<br>";
				//if(check.CheckRecord("mnit", "mnit_nit", "'" + datas[1].Trim() + "'"))
				if(DBFunctions.RecordExist("select mnit_nit from mnit where mnit_nit =" + "'" + datas[1].Trim() + "'"))
				{
					checks++;
					//infoLabel.Text += datas[1].Trim() + " existe en mnit<br>";
				}
				else
					infoLabel.Text += "error: " + datas[1].Trim() + " no es un NIT vlido<br>";
				//if(check.CheckRecord("palmacen", "palm_almacen", "'" + datas[5].Trim() + "'"))
                if (DBFunctions.RecordExist("select palm_almacen from palmacen where tvig_vigencia='V' and palm_almacen =" + "'" + datas[5].Trim() + "'"))
				{
					checks++;
					//infoLabel.Text += datas[5].Trim() + " existe en palmacen<br>";
				}
				else
					infoLabel.Text += "error: " + datas[5].Trim() + " no es una bodega vlido<br>";
				//if(check.CheckRecord("pcentrocosto", "pcen_codigo", "'" + datas[6].Trim() + "'"))
				if(DBFunctions.RecordExist("select pcen_codigo from pcentrocosto where pcen_codigo =" + "'" + datas[6].Trim() + "'"))
				{
					checks++;
					//infoLabel.Text += datas[6].Trim() + " existe en pcentrocosto<br>";
				}
				else
					infoLabel.Text += "error: " + datas[6].Trim() + " no es un centro de costo vlido<br>";

				if(checks == 4)
					correct = true;
			}

			return correct;
		}
        protected void InitDataTable()
        {
            lbFields.Clear();
            lbFields.Add("cuenta");//0
            lbFields.Add("nit");//1
            lbFields.Add("pref");//2
            lbFields.Add("docref");//3
            lbFields.Add("detalle");//4
            lbFields.Add("sede");//5
            lbFields.Add("ccosto");//6
            lbFields.Add("debito");//7
            lbFields.Add("credito");//8
            lbFields.Add("base");//9

            types.Clear();
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(string));
            types.Add(typeof(double));
            types.Add(typeof(double));
            types.Add(typeof(double));

            dtInserts = new DataTable();
            for (i = 0; i < lbFields.Count; i++)
                dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
            //infoLabel.Text +="<br>"+(string)lbFields[i];
        }

		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Init);

		}
		#endregion

	}
}
