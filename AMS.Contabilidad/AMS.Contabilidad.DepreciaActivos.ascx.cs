// created on 02/09/2004 at 9:33
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
using AMS.Tools;
using System.IO;

namespace AMS.Contabilidad
{
	public partial class DepreciaActivos : System.Web.UI.UserControl
	{
		protected string mesSeleccionado,fechaComprobante;
		protected DataTable comprobante, dtComprobanteNiif;
		protected double paag,total,totalNiif,valorMinimo;
		protected DataSet activosFijos;
        protected Label LBvalorMinimo;
		
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
                Session.Clear();
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ano, "SELECT pano_ano FROM pano where pano_ano >= (select pano_ano from ccontabilidad) order by 1");
				bind.PutDatasIntoDropDownList(mes,"SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 13");
                txtPrefijoComprobante.Text = DBFunctions.SingleData("SELECT p.pdoc_codigo concat ' - ' concat p.pdoc_nombre from pdocumento p, ccontabilidad c where c.CCON_PREFNIIF = p.pdoc_codigo;");
                if(txtPrefijoComprobante.Text == "")
                {
                    Utils.MostrarAlerta(Response, "Por favor parametrice el documento para contabilizar la depreciacion y el deterioro en la configuración de contabilidad. -CCON_PREFNIIF-");
                    return;
                };
                string ValorMinimoString = DBFunctions.SingleData("SELECT ccon_valominidrep FROM ccontabilidad");
                LBvalorMinimo = new Label();
                try
                {
                    valorMinimo = Convert.ToDouble(ValorMinimoString);
                }
                catch
                {
                    Utils.MostrarAlerta(Response, "Por favor parametrice el valor mínimo de la depreciación en la configuración de contabilidad");
                    return;
                };

                //Session["valorMinimo"] = valorMinimo;
                LBvalorMinimo.Text="El Valor Mínimo de Depreciación es : "+this.valorMinimo.ToString("C");
	
                //		valorPaag.Text="Valor del PAAG para el mes elegido es : "+DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes=(SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+mes.SelectedItem.ToString()+"')");
			}
		}
		
		//
		
		protected  void  efectuar_depreciacion(Object  Sender, EventArgs e)
		{
            
			lb.Text="";
            string procesoTipo = ((Button)Sender).Text;
            Session.Clear();
            //if (Session["comprobanteLEGAL"] != null)
            //{
                procesoTipo = "Efectuar LEGAL";
            //}

            ViewState["proceso"] = procesoTipo;
            if (!this.ExisteComprobante(procesoTipo))
			{
		        //paag = System.Convert.ToDouble(DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes=(SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+mes.SelectedItem.ToString()+"')"));
                paag = 0;  // ahora los ajustes por inflacion estan derogados
                this.PrepararTabla();
                this.LlenarComprobante();
                int numComprobante = Convert.ToInt32(this.ConsecutivoComprobante());
                //string codComprobante = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + tComprobante.SelectedItem.ToString() + "'");
                string codComprobante = DBFunctions.SingleData("select CCON_PREFNIIF from ccontabilidad");

                for (int cont = 1; cont <= 2; cont++)
                {
                    ArrayList header = new ArrayList();
                    header.Add(codComprobante);//tipo
                    header.Add(numComprobante);//consecutivo
                    header.Add(ano.SelectedItem.ToString());//año
                    header.Add(mesSeleccionado);//mes
                    header.Add(fechaComprobante);//fecha

                    if (cont == 2) //Niif
                    {
                        header.Add("DEPRECIACION NIIF DE ACTIVOS FIJOS DEL MES DE " + mes.SelectedItem.ToString().ToUpper());//Razon
                        header.Add(totalNiif.ToString());//total Niif	
                        Session["header"] = header;
                    }
                    else if (cont == 1) //Legal
                    {
                        header.Add("DEPRECIACION LEGAL DE ACTIVOS FIJOS DEL MES DE " + mes.SelectedItem.ToString().ToUpper());//Razon
                        header.Add(total.ToString());//total Legal
                        Session["headerLEGAL"] = header;
                    }

            //        numComprobante++;
                }

                //int legal = 0;
                //DataTable dtLegal = new DataTable();
                //ArrayList arrayheader = new ArrayList();
                //DataSet dsActivosFijos = new DataSet();
                //if (Session["comprobanteLEGAL"] == null)
                //{
                //    legal = 1;
                //}
                //else
                //{
                //    dtLegal = (DataTable)Session["comprobanteLEGAL"];
                //    arrayheader = (ArrayList)Session["headerLEGAL"];
                //    dsActivosFijos.Tables.Add(((DataTable)Session["activosFijosLEGAL"]).Copy());
                //}

                Session["comprobante"] = dtComprobanteNiif;
                Session["comprobanteLEGAL"] = comprobante;                
				Session["activosFijos"] = activosFijos.Tables[0];
                Session["activosFijosLEGAL"] = activosFijos.Tables[0];

                Session["paag"] = paag;
				guardar.Visible = true;
                guardar1.Visible = true;
                guardar2.Visible = true;
                Lbcredito.Visible = true;
                Lbdebito.Visible = true;
                LbcreditoNIIF.Visible = true;
                LbdebitoNIIF.Visible = true;
                BtnImprimirExcel.Visible = true;
                //if (legal == 1)
                //{
                //Session["comprobanteLEGAL"] = comprobante;
                //Session["activosFijosLEGAL"] = activosFijos.Tables[0];
                //efectuar_depreciacion(Sender, e);
                //}
                //else
                //{
                //    Session["comprobanteLEGAL"] = dtLegal;
                //    Session["headerLEGAL"] = arrayheader;
                //    Session["activosFijosLEGAL"] = dsActivosFijos.Tables[0];
                //}
            }
			else
			{
                this.PrepararTabla();
                comprobanteG.DataSource = comprobante;
                comprobanteG.DataBind();
                
				lb.Text="Este proceso ya fue realizado para el mes seleccionado";
			}
			
		}

        protected void guardar_comprobanteMixto(Object Sender, EventArgs e)
        {
            int tipo = 0;
            guardar_comprobante(tipo);
        }
        protected void guardar_comprobanteLegal(Object Sender, EventArgs e)
        {
            int tipo = 1;
            guardar_comprobante(tipo);
        }

        protected void guardar_comprobanteNiif(Object Sender, EventArgs e)
        {
            int tipo = 2;
            guardar_comprobante(tipo);
        }

        protected void guardar_comprobante(int tipo)
        {

            ArrayList sqlStrings = new ArrayList();
            String[] arrayPrefijos = new String[2];
            bool primeraParte = false;
            Hashtable h_dactivosfijos = new Hashtable();
            mesSeleccionado = DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='" + mes.SelectedItem.ToString() + "'");
            fechaComprobante = ano.SelectedItem.ToString() + "-" + mesSeleccionado + "-" + DBFunctions.SingleData("SELECT pmes_dias FROM pmes WHERE pmes_mes=" + mesSeleccionado + "");

            int ini;
            int fin;
            if (tipo == 0)  // ambas depreciaciones
            {
                ini = 1;
                fin = 2;
            }
            else if (tipo == 1)  // deprecia solo LEGAL
            {
                    ini = 1;
                    fin = 1;
            }
                else   // deprecia solo NIIF
                {
                    ini = 2;
                    fin = 2;
                }

            for (int cont = ini; cont <= fin; cont++)
            {
                //Guardamos el comprobante
                ArrayList header = new ArrayList();

                //header = (ArrayList)Session["header"];
                comprobante = new DataTable();
                //comprobante = ((DataTable)Session["comprobante"]).Copy();
                //Hacemos los cambios correspondientes en la Tabla mactivofijo
                DataTable activosFijos = new DataTable();
                //activosFijos = ((DataTable)Session["activosFijos"]).Copy();

                int incrementoNiif = 0;
                DataSet dsActivos = new DataSet();
                if (cont == 1)
                {
                    header = (ArrayList)Session["headerLEGAL"];
                    comprobante = ((DataTable)Session["comprobanteLEGAL"]).Copy();
                    //activosFijos = ((DataTable)Session["activosFijosLEGAL"]).Copy();
                    DBFunctions.Request(dsActivos, IncludeSchema.NO,
                             @"SELECT mafj_descripcion,pcco_centcost,mafj_fechinic,mafj_valohist, 
                              mafj_valomejora,mafj_depracum,mafj_cuotas,mafj_cuendepr,mafj_cuengastdepr,  
                              mafj_cuenactifijo,mafj_codiacti, 0 as valoreci, tdep_tipodepr, mafj_horautil FROM mactivofijo  
                              WHERE  (mafj_valohist + mafj_valomejora) > mafj_depracum  AND mafj_fechinic < '" + fechaComprobante + "' AND TVIG_VIGENCIA = 'V' ");
                    ViewState["proceso"] = "Efectuar LEGAL";
                    if (tipo == 0)  // AMBAS DEPRECIACIONES LEGAL Y NIIF
                        incrementoNiif = 1;
                }
                else
                {
                    header = (ArrayList)Session["header"];
                    comprobante = ((DataTable)Session["comprobante"]).Copy();
                    //activosFijos = ((DataTable)Session["activosFijos"]).Copy();
                    DBFunctions.Request(dsActivos, IncludeSchema.NO,
                             @"SELECT mafj_descripcion, pcco_centcost, mafj_fechinic, mafj_valoactniif,
                              mafj_valomejoraniif, mafj_depracumniif, mafj_cuotasniif, mafj_cuenNIIFdepr, mafj_cuengastdeprNIIF,
                              mafj_cuenNIIFacti, mafj_codiacti, mafj_valoreciniif, tdep_tipodepr, mafj_horautil FROM mactivofijo
                              WHERE(mafj_valoactniif + mafj_valomejoraniif) > mafj_depracumniif  AND mafj_fechinic < '" + fechaComprobante + "' AND TVIG_VIGENCIA = 'V' ");
                    ViewState["proceso"] = "Efectuar NIIF";
                }

                activosFijos = dsActivos.Tables[0].Copy();

                //Este proceso se comentó ya que no se requiere el calculo NIIF por diferencia...
                //if (ViewState["proceso"].ToString() == "Efectuar NIIF")
                //    comprobante = ValorContabilizacionDiferenciaNiif(comprobante, header[0].ToString(), header[2].ToString(), header[3].ToString());

                Comprobante comp = new Comprobante(comprobante);
                comp.Type   = header[0].ToString();
                if (cont == 1)
                    comp.Number = header[1].ToString();
                else
                    comp.Number = Convert.ToUInt32((Convert.ToUInt32(header[1].ToString())+ incrementoNiif)).ToString();
                comp.Year   = header[2].ToString();
                comp.Month  = header[3].ToString();
                comp.Date   = header[4].ToString();
                comp.Detail = header[5].ToString();
                comp.User   = HttpContext.Current.User.Identity.Name.ToLower();
                comp.Value  = header[6].ToString();
                comp.Consecutivo = false;
                comp.TipoProceso = ViewState["proceso"].ToString() == "Efectuar LEGAL" ? "0" : "1";

                sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu = pdoc_ultidocu + 1 + " + incrementoNiif + " WHERE pdoc_codigo ='" + comp.Type + "'");

                //if (comp.CommitValues())
                if (comp.CommitValues(ref sqlStrings))
                    primeraParte = true;
                else
                    lb.Text += comp.ProcessMsg;

                paag = System.Convert.ToDouble(Session["paag"]);

                DataSet dsDetallesActivoFijo = new DataSet();
                DBFunctions.Request(dsDetallesActivoFijo, IncludeSchema.NO, "select DAFJ_CODIACTI  from dactivofijo where pano_ano =" + header[2].ToString() + " and pmes_mes = " + header[3].ToString());

                string ValorMinimoString = DBFunctions.SingleData("SELECT ccon_valominidrep FROM ccontabilidad");
                valorMinimo = Convert.ToDouble(ValorMinimoString);


                //Actualizacion detalles activo fijo
                for (int i = 0; i < activosFijos.Rows.Count; i++)
                {
                    double valorResidualNIIF = 0;
                    if (ViewState["proceso"].ToString() == "Efectuar NIIF")
                    {
                        valorResidualNIIF = System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[11]);
                    }

                    //double amortizacion = (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[3]) + System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[4]) - valorResidualNIIF) / (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[6]));
                    double amortizacion = 0;
                    if (activosFijos.Rows[i].ItemArray[12].ToString() == "L")  //Tipo de depreciacion: LINEA RECTA
                        amortizacion = (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[3]) + System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[4]) - valorResidualNIIF) / (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[6]));
                    else if (activosFijos.Rows[i].ItemArray[12].ToString() == "H")  //Tipo de depreciacion: HORAS TRABAJADAS
                            amortizacion = ((System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[3]) + System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[4]) - valorResidualNIIF) / (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[13]))) * 25000; //valor de prueba de horas por mes

                    double saldoAmt = (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[3])) - (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[5]));
                    double valor_Activo = (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[3])) + (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[4]));

                    if (valor_Activo <= valorMinimo)
                    {
                        amortizacion = valor_Activo;
                    }
                    else if (saldoAmt < amortizacion)
                            amortizacion = saldoAmt;

                    //lb.Text+="<BR>Valor de amortizacion que se suma a mafj_depracum="+amortizacion.ToString()+" para el activo "+activosFijos.Rows[i].ItemArray[0].ToString();
                    //sqlStrings.Add("UPDATE mactivofijo SET mafj_depracum = mafj_depracum +"+amortizacion.ToString()+" WHERE mafj_descripcion='"+activosFijos.Rows[i].ItemArray[0].ToString()+"'");

                    if (ViewState["proceso"].ToString() == "Efectuar NIIF" && tipo != 1)
                        sqlStrings.Add("UPDATE mactivofijo SET mafj_depracumniif = mafj_depracumniif +" + amortizacion.ToString() + " WHERE mafj_codiacti='" + activosFijos.Rows[i].ItemArray[10].ToString() + "'");
                    else if (tipo != 2)
                            sqlStrings.Add("UPDATE mactivofijo SET mafj_depracum = mafj_depracum +" + amortizacion.ToString() + " WHERE mafj_codiacti='" + activosFijos.Rows[i].ItemArray[10].ToString() + "'");

                    //double inflacionAcumulada = System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[14].ToString());
                    //double valor = ((inflacionAcumulada)+System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[3].ToString())+System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[4].ToString()))*(paag/100);
                    double valor = (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[3].ToString()) + System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[4].ToString())) * (paag / 100);

                    //lb.Text+="<BR>valor del ajuste por inflacion que se suma a mafj_inflacum="+valor.ToString()+" del activo "+activosFijos.Rows[i].ItemArray[0].ToString();
                    //no se usa inflacion
                    //sqlStrings.Add("UPDATE mactivofijo SET mafj_inflacum = mafj_inflacum + "+valor.ToString()+" WHERE mafj_descripcion='"+activosFijos.Rows[i].ItemArray[0].ToString()+"'");
                    //double valorInflacionDepreciacionAcumulada = (System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[15].ToString())+System.Convert.ToDouble(activosFijos.Rows[i].ItemArray[5].ToString()))*(paag/100);
                    //lb.Text+="<BR>valor del ajuste de inflacion de la depreciacion que se suma a mafj_infldepracum="+valorInflacionDepreciacionAcumulada+"para el activo "+activosFijos.Rows[i].ItemArray[0].ToString();
                    //sqlStrings.Add("UPDATE mactivofijo SET mafj_infldepracum = mafj_infldepracum +"+valorInflacionDepreciacionAcumulada.ToString()+" WHERE mafj_descripcion='"+activosFijos.Rows[i].ItemArray[0].ToString()+"'");				

                    DataRow[] drActivoFijo = dsDetallesActivoFijo.Tables[0].Select("DAFJ_CODIACTI = '" + activosFijos.Rows[i].ItemArray[10].ToString() + "'");
                    if (h_dactivosfijos.ContainsKey(activosFijos.Rows[i].ItemArray[10].ToString()) == true || drActivoFijo.Length > 0)  //(drActivoFijo.Length > 0)
                    {
                        if (ViewState["proceso"].ToString() == "Efectuar NIIF" && tipo != 1)
                            sqlStrings.Add("UPDATE DACTIVOFIJO SET DAFJ_VALODEPRNIIF = " + saldoAmt.ToString() + " WHERE DAFJ_CODIACTI = '" + activosFijos.Rows[i].ItemArray[10].ToString() + "' " +
                                           "AND PANO_ANO = " + header[2].ToString() + " AND PMES_MES = " + header[3].ToString() + ";");
                        else if (tipo != 2)
                                sqlStrings.Add("UPDATE DACTIVOFIJO SET DAFJ_VALODEPR = " + saldoAmt.ToString() + " WHERE DAFJ_CODIACTI = '" + activosFijos.Rows[i].ItemArray[10].ToString() + "' " +
                                       "AND PANO_ANO = " + header[2].ToString() + " AND PMES_MES = " + header[3].ToString() + ";");
                    }
                    else
                    {
                        if (ViewState["proceso"].ToString() == "Efectuar NIIF" && tipo != 1)
                        { 
                            sqlStrings.Add("INSERT INTO dactivofijo VALUES('" + activosFijos.Rows[i].ItemArray[10].ToString() + "'," + header[2].ToString() + "," + header[3].ToString() + "," + saldoAmt.ToString() + "," + valor.ToString() + ",0)");
                            h_dactivosfijos.Add(activosFijos.Rows[i].ItemArray[10].ToString(), activosFijos.Rows[i].ItemArray[10].ToString());
                        }
                        else if (tipo != 2)
                            {  
                                sqlStrings.Add("INSERT INTO dactivofijo VALUES('" + activosFijos.Rows[i].ItemArray[10].ToString() + "'," + header[2].ToString() + "," + header[3].ToString() + "," + valor.ToString() + "," + saldoAmt.ToString() + ",0)");
                                h_dactivosfijos.Add(activosFijos.Rows[i].ItemArray[10].ToString(), activosFijos.Rows[i].ItemArray[10].ToString());
                            }
                    }
                }

                arrayPrefijos[cont-1] = header[0].ToString() + "-" + header[1].ToString();
            }

            if (primeraParte)
            {
                if (DBFunctions.Transaction(sqlStrings))
                {
                    Utils.MostrarAlerta(Response, "Proceso completo! Se han generado los comprobantes Legal: " + arrayPrefijos[0] + "  y Niif: " + arrayPrefijos[1]);
                    lb.Text = "<BR>Se Ha Logrado Realizar El Proceso con Exito"; //DBFunctions.exceptions + ;
                }
                else
                    lb.Text += DBFunctions.exceptions + "<BR>Se ha presentado un inconveniente al grabar el Proceso";
            }
            else
                lb.Text += "<BR>Se ha presentado un inconveniente con el Proceso";
        }

        /*
        protected DataTable ValorContabilizacionDiferenciaNiif(DataTable comprobanteNiif, string codigoPref, string yearComp, string monthComp)
        {
            DataTable comprobanteNiifAux = new DataTable();
            DataSet dsMovimientoComprobanteLegal = new DataSet();
            string numeroPref = DBFunctions.SingleData("SELECT mcom_numedocu FROM mcomprobante where  mcom_razon LIKE '%DEPRECIACION DE ACTIVOS%' and pano_ano=" + yearComp + " AND pmes_mes=" + monthComp);
            DBFunctions.Request(dsMovimientoComprobanteLegal, IncludeSchema.NO, "select mcue_codipuc, dcue_codirefe, dcue_valodebi, dcue_valocred from dcuenta where pdoc_codigo='" + codigoPref + "' and mcom_numedocu=" + numeroPref);

            for (int p = 0; p < comprobanteNiif.Rows.Count; p++)
            {
                string cuentaPUC = comprobanteNiif.Rows[p][0].ToString();
                string codigoReferencia = comprobanteNiif.Rows[p][10].ToString();
                DataRow[] drComprobanteLegal = dsMovimientoComprobanteLegal.Tables[0].Select("MCUE_CODIPUC='" + cuentaPUC + "' AND DCUE_CODIREFE='" + codigoReferencia + "'");
                if (drComprobanteLegal.Length > 0)
                {
                    double valorDeb = Convert.ToDouble(drComprobanteLegal[0][2]);
                    double valorCred = Convert.ToDouble(drComprobanteLegal[0][3]);
                    double valorDebNiif = Convert.ToDouble(comprobanteNiif.Rows[p][7]);
                    double valorCredNiif = Convert.ToDouble(comprobanteNiif.Rows[p][8]);
                    double total = 0;
                    if (valorDeb != 0)
                    {
                        //Resta Debitos
                        total = valorDebNiif - valorDeb;
                        if (total >= 0)
                            comprobanteNiif.Rows[p][7] = Math.Round(total,0);
                        else
                        {
                            comprobanteNiif.Rows[p][8] = Math.Round(total, 0);
                            comprobanteNiif.Rows[p][7] = 0;
                        }
                    }
                    else 
                    { 
                        //Resta Creditos
                        total = valorCredNiif - valorCred;
                        if (total >= 0)
                            comprobanteNiif.Rows[p][8] = Math.Round(total, 0);
                        else
                        {
                            comprobanteNiif.Rows[p][7] = Math.Round(total, 0);
                            comprobanteNiif.Rows[p][8] = 0;
                        }
                    }
                }
            


            return comprobanteNiif;
        }
		*/

		//FUNCION QUE VERIFICA QUE NO EXISTA EL COMPROBANTE DE DEPRECIACION DEL MES SELECCIONADO
        protected bool ExisteComprobante(string procesoTipo)
		{
			bool existe = false;
			mesSeleccionado = DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+mes.SelectedItem.ToString()+"'");
			bool existeCompLegal = DBFunctions.RecordExist("SELECT * FROM mcomprobante WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes="+mesSeleccionado+" AND mcom_razon LIKE '%DEPRECIACION DE ACTIVOS%'");
            bool existeCompNiif = DBFunctions.RecordExist("SELECT * FROM mcomprobante WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes="+mesSeleccionado+" AND mcom_razon LIKE '%DEPRECIACION NIIF DE ACTIVOS%'");

            if (procesoTipo == "Efectuar NIIF")
            {
                if (!existeCompLegal)
                    existe = true;
                else
                    if (existeCompNiif)
                        existe = true;
            }
            else 
            {
                if(existeCompLegal)
                    existe = true;
            }

            if (Session["comprobanteLEGAL"] != null)
                existe = false;

			return existe;
		}		
		//FIN DE LA FUNCION DE VERIFICACION DE COMPROBANTE

        protected void PrepararTabla()
		{
            
            comprobante = new DataTable();
            dtComprobanteNiif = new DataTable();

            //0.Se crea la columna que almacena los codigos de las cuentas afectadas
            DataColumn cuenta = new DataColumn();
			cuenta.DataType = System.Type.GetType("System.String");
			cuenta.ColumnName = "CUENTA";
			cuenta.ReadOnly=true;
			comprobante.Columns.Add(cuenta);
			//1.Se crea la columna que almacenara el nit 
			DataColumn nit = new DataColumn();
			nit.DataType = System.Type.GetType("System.String");
			nit.ColumnName = "NIT";
			nit.ReadOnly=true;
			comprobante.Columns.Add(nit);
			//2.Se crea la columna que almacena el pref
			DataColumn pref = new DataColumn();
			pref.DataType = System.Type.GetType("System.String");
			pref.ColumnName = "PREF";
			pref.ReadOnly=true;
			comprobante.Columns.Add(pref);
			//3.Se crea la columna que almacena el docuemto de referncia
			DataColumn docref = new DataColumn();
			docref.DataType = System.Type.GetType("System.String");
			docref.ColumnName = "DOC_REF";
			docref.ReadOnly=true;
			comprobante.Columns.Add(docref);
            //4.Se crea la columna que almacena el detalle
            DataColumn detalle = new DataColumn();
			detalle.DataType = System.Type.GetType("System.String");
			detalle.ColumnName = "DETALLE";
			detalle.ReadOnly=true;
			comprobante.Columns.Add(detalle);
			//5.Se crea la columna que almacena la sede
			DataColumn sede = new DataColumn();
			sede.DataType = System.Type.GetType("System.String");
			sede.ColumnName = "SEDE";
			sede.ReadOnly=true;
			comprobante.Columns.Add(sede);
			//6.Se crea la columna que almacena el centro de costo
			DataColumn centroCosto = new DataColumn();
			centroCosto.DataType = System.Type.GetType("System.String");
			centroCosto.ColumnName = "CENTRO_COSTO";
			centroCosto.ReadOnly=true;
			comprobante.Columns.Add(centroCosto);
			//7.Se crea la columna que almacena el valor del debito
			DataColumn debitoC = new DataColumn();
			debitoC.DataType = System.Type.GetType("System.Double");
            debitoC.ColumnName = "DEBITO";
			//debitoC.ReadOnly=true;
			comprobante.Columns.Add(debitoC);
			//8.Se crea la columna que almacena el valor del credito
			DataColumn creditoC = new DataColumn();
			creditoC.DataType = System.Type.GetType("System.Double");
            creditoC.ColumnName = "CREDITO";
			//creditoC.ReadOnly=true;
			comprobante.Columns.Add(creditoC);
			//9.Se crea la columna de la base
			DataColumn baseC = new DataColumn();
			baseC.DataType = System.Type.GetType("System.Double");
			baseC.ColumnName = "BASE";
			baseC.ReadOnly=true;
			comprobante.Columns.Add(baseC);
            //10.Se crea la columna que almacena el valor del debito NIIF
            DataColumn debitoCNiif = new DataColumn();
            debitoCNiif.DataType = System.Type.GetType("System.Double");
            debitoCNiif.ColumnName = "DEBITO_NIIF";
            //debitoC.ReadOnly=true;
            comprobante.Columns.Add(debitoCNiif);
            //11.Se crea la columna que almacena el valor del credito NIIF
            DataColumn creditoCNiif = new DataColumn();
            creditoCNiif.DataType = System.Type.GetType("System.Double");
            creditoCNiif.ColumnName = "CREDITO_NIIF";
            //creditoC.ReadOnly=true;
            comprobante.Columns.Add(creditoCNiif);

            //12.Se crea la columna de Codígo del Activo
            DataColumn codiActi = new DataColumn();
            codiActi.DataType = System.Type.GetType("System.String");
            codiActi.ColumnName = "CODIGO_ACTIVO";
            codiActi.ReadOnly = true;
            comprobante.Columns.Add(codiActi);
            //13.Se crea la columna que almacena los codigos de las cuentas afectadas NIIF
            DataColumn cuentaNIIF = new DataColumn();
            cuentaNIIF.DataType = System.Type.GetType("System.String");
            cuentaNIIF.ColumnName = "CUENTA_NIIF";
            cuentaNIIF.ReadOnly = true;
            comprobante.Columns.Add(cuentaNIIF);

            //14.Se crea la columna que almacena el valor del debito NIIF Auxiliar
            DataColumn debitoCNiifAux = new DataColumn();
            debitoCNiifAux.DataType = System.Type.GetType("System.Double");
            debitoCNiifAux.ColumnName = "DEBITO_NIIFAUX";
            //debitoC.ReadOnly=true;
            comprobante.Columns.Add(debitoCNiifAux);
            //15.Se crea la columna que almacena el valor del credito NIIF Auxiliar
            DataColumn creditoCNiifAux = new DataColumn();
            creditoCNiifAux.DataType = System.Type.GetType("System.Double");
            creditoCNiifAux.ColumnName = "CREDITO_NIIFAUX";
            //creditoC.ReadOnly=true;
            comprobante.Columns.Add(creditoCNiifAux);

            dtComprobanteNiif = comprobante.Clone();
        }
		
		//FUNCION QUE ME CREA EL COMPROBANTE QUE RESPALDA LA DEPRECIACION DE LOS ACTIVOS FIJOS Y EL AJUSTE POR INFLACION DE LOS ACTIVOS FIJOS
		
		protected void LlenarComprobante()
		{
			fechaComprobante = ano.SelectedItem.ToString()+"-"+mesSeleccionado+"-"+DBFunctions.SingleData("SELECT pmes_dias FROM pmes WHERE pmes_mes="+mesSeleccionado+"");
			int i;
			total = 0;
            totalNiif = 0;
			activosFijos = new DataSet();
            string sqlActivos = "";

            sqlActivos =
                @"SELECT mafj_descripcion,pcco_centcost,mafj_fechinic,
                   CASE WHEN ((mafj_valoactniif + mafj_valomejoraniif) > mafj_depracumniif) THEN mafj_valoactniif ELSE 0 END as mafj_valoactniif,   
                   CASE WHEN ((mafj_valoactniif + mafj_valomejoraniif) > mafj_depracumniif) THEN mafj_valomejoraniif ELSE 0 END as mafj_valomejoraniif,
                   COALESCE(mafj_depracumniif,0) AS mafj_depracumniif, mafj_cuotasniif, mafj_cuenNIIFdepr, mafj_cuengastdeprNIIF,   
                   mafj_cuenNIIFacti, mafj_codiacti, coalesce(mafj_valoreciniif,0) as mafj_valoreciniif, tdep_tipodepr, mafj_horautil,
                   CASE WHEN ((mafj_valohist + mafj_valomejora) > mafj_depracum) THEN mafj_valohist ELSE 0 END as mafj_valohist , 
                   CASE WHEN ((mafj_valohist + mafj_valomejora) > mafj_depracum) THEN mafj_valomejora ELSE 0 END as mafj_valomejora,
                   mafj_depracum, mafj_cuotas, mafj_cuendepr, mafj_cuengastdepr, mafj_cuenactifijo
                   FROM mactivofijo   
                   WHERE  ((mafj_valoactniif + mafj_valomejoraniif) > mafj_depracumniif or (mafj_valohist + mafj_valomejora) > mafj_depracum) AND mafj_fechinic < '" + fechaComprobante + "' AND TVIG_VIGENCIA = 'V' ";
            //AND MAFJ_CODIACTI = '1972'

            //if (procesoTipo == "Efectuar NIIF") //Proceso para NIIF
            //    sqlActivos = @"SELECT mafj_descripcion,pcco_centcost,mafj_fechinic,mafj_valoactniif, " +
            //     " mafj_valomejoraniif,mafj_depracumniif,mafj_cuotasniif,mafj_cuenNIIFdepr,mafj_cuengastdeprNIIF, " +
            //     " mafj_cuenNIIFacti,mafj_codiacti, mafj_valoreciniif, tdep_tipodepr, mafj_horautil FROM mactivofijo " +
            //     " WHERE  (mafj_valoactniif + mafj_valomejoraniif) > mafj_depracumniif  AND mafj_fechinic < '" + fechaComprobante + "' ";

            //else  //Proceso para Legal
            //    sqlActivos = @"SELECT mafj_descripcion,pcco_centcost,mafj_fechinic,mafj_valohist," +
            //     " mafj_valomejora,mafj_depracum,mafj_cuotas,mafj_cuendepr,mafj_cuengastdepr, " +
            //     " mafj_cuenactifijo,mafj_codiacti, 0 as valoreci, tdep_tipodepr, mafj_horautil FROM mactivofijo " +
            //     " WHERE  (mafj_valohist + mafj_valomejora) > mafj_depracum  AND mafj_fechinic < '" + fechaComprobante + "' ";
            //     //" WHERE (mafj_valohist + mafj_valomejora - mafj_depracum ) > '" + valorMinimo + "' AND mafj_fechinic < '" + fechaComprobante + "' ";

            DBFunctions.Request(activosFijos, IncludeSchema.NO,sqlActivos);
            
            string ValorMinimoString = DBFunctions.SingleData("SELECT ccon_valominidrep FROM ccontabilidad");
            valorMinimo = Convert.ToDouble(ValorMinimoString);
            string almacen = DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
            string nitEmpresa = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
            //string prefijo = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + tComprobante.SelectedItem.ToString() + "'");
            string prefijo = DBFunctions.SingleData("select CCON_PREFNIIF from ccontabilidad");
            string numeroPref = this.ConsecutivoComprobante();

            //Aqui vamos a empezar a crear nuestro comprobante de ajuste
            for (i=0;i<activosFijos.Tables[0].Rows.Count;i++)
			{
				//Calculamos el valor de la amortizacion
                //double valorResidualNIIF = 0;
                //if (ViewState["proceso"].ToString() == "Efectuar NIIF")
                //{
                double valorResidualNIIF = System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[11]);
                //}


                double amortizacionNIIF = 0;
                double amortizacion = 0;
                
                if (activosFijos.Tables[0].Rows[i].ItemArray[12].ToString() == "L")  //Tipo de depreciacion: LINEA RECTA
                    amortizacionNIIF = (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[3]) + System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[4]) - valorResidualNIIF) / (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[6]));
                else if (activosFijos.Tables[0].Rows[i].ItemArray[12].ToString() == "H")  //Tipo de depreciacion: HORAS TRABAJADAS
                    amortizacionNIIF = ((System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[3]) + System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[4]) - valorResidualNIIF) / (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[13]))) * 25000; //valor de prueba de horas por mes

                if (activosFijos.Tables[0].Rows[i].ItemArray[12].ToString() == "L")  //Tipo de depreciacion: LINEA RECTA
                    amortizacion = (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[14]) + System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[15]) - valorResidualNIIF) / (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[17]));
                else if (activosFijos.Tables[0].Rows[i].ItemArray[12].ToString() == "H")  //Tipo de depreciacion: HORAS TRABAJADAS
                    amortizacion = ((System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[14]) + System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[15])) / (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[13]))) * 25000; //valor de prueba de horas por mes

                double saldoAmtNIIF = (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[3])) - (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[5]));
                double valor_ActivoNIIF = (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[3])) + (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[4]));
                double saldoAmt = (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[14])) - (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[16]));
                double valor_Activo = (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[14])) + (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[15]));

                if (valor_ActivoNIIF <= valorMinimo)
                {
                    amortizacionNIIF = valor_ActivoNIIF;
                }
                else if(saldoAmtNIIF < amortizacionNIIF)
                    amortizacionNIIF = saldoAmtNIIF;

                if (valor_Activo <= valorMinimo)
                {
                    amortizacion = valor_Activo;
                }
                else if (saldoAmt < amortizacion)
                    amortizacion = saldoAmt;


                total += amortizacion;
                totalNiif += amortizacionNIIF;
                //Armamos las filas del comprobante
                DataRow filaOrigen = comprobante.NewRow();
				DataRow filaDestino = comprobante.NewRow();

                DataRow filaOrigenNiif = dtComprobanteNiif.NewRow();
                DataRow filaDestinoNiif = dtComprobanteNiif.NewRow();

                //mcue_codipuc dtSource.Rows[i][0]
                //dcue_mnit dtSource.Rows[i][1]
                //dcue_numerefe dtSource.Rows[i][3]
                //dcue_detalle dtSource.Rows[i][4]
                //palm_almacen dtSource.Rows[i][5]
                //pcen_codigo dtSource.Rows[i][6]
                //debUtility dtSource.Rows[i][7]
                //credUtility dtSource.Rows[i][8]
                //dcue_valobase dtSource.Rows[i][9]
                //dcue_codirefe dtSource.Rows[i][10]
                
                filaOrigen["CUENTA_NIIF"] = filaOrigenNiif["CUENTA"] = activosFijos.Tables[0].Rows[i].ItemArray[7].ToString();
                filaDestino["CUENTA_NIIF"] = filaDestinoNiif["CUENTA"] = activosFijos.Tables[0].Rows[i].ItemArray[8].ToString();
                filaOrigen["CUENTA"] = activosFijos.Tables[0].Rows[i].ItemArray[18].ToString();
				filaDestino["CUENTA"] = activosFijos.Tables[0].Rows[i].ItemArray[19].ToString();
                filaOrigen["NIT"] = filaOrigenNiif["NIT"] = nitEmpresa;
				filaDestino["NIT"] = filaDestinoNiif["NIT"] = nitEmpresa;
				filaOrigen["PREF"] = filaOrigenNiif["PREF"] = prefijo;
				filaDestino["PREF"] = filaDestinoNiif["PREF"] = prefijo;
				filaOrigen["DOC_REF"] = filaOrigenNiif["DOC_REF"] = numeroPref;
				filaDestino["DOC_REF"] = filaDestinoNiif["DOC_REF"] = numeroPref;
                filaOrigen["CODIGO_ACTIVO"] = filaOrigenNiif["CODIGO_ACTIVO"] = activosFijos.Tables[0].Rows[i].ItemArray[10].ToString();
                filaDestino["CODIGO_ACTIVO"] = filaDestinoNiif["CODIGO_ACTIVO"] = activosFijos.Tables[0].Rows[i].ItemArray[10].ToString();
                filaOrigen["DETALLE"] = filaOrigenNiif["DETALLE"] = "DEPRECIACION DE " +activosFijos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
				filaDestino["DETALLE"] = filaDestinoNiif["DETALLE"] = "DEPRECIACION DE " +activosFijos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
				filaOrigen["SEDE"] = filaOrigenNiif["SEDE"] = almacen;
				filaDestino["SEDE"] = filaDestinoNiif["SEDE"] = almacen;
				filaOrigen["CENTRO_COSTO"] = filaOrigenNiif["CENTRO_COSTO"] = activosFijos.Tables[0].Rows[i].ItemArray[1].ToString();
				filaDestino["CENTRO_COSTO"] = filaDestinoNiif["CENTRO_COSTO"] = activosFijos.Tables[0].Rows[i].ItemArray[1].ToString();

                filaOrigen["DEBITO"] = filaOrigenNiif["DEBITO"] = 0;
                filaDestino["DEBITO"] = amortizacion;
                filaDestinoNiif["DEBITO"] = 0;
                filaDestino["CREDITO"] = filaDestinoNiif["CREDITO"] = 0;
                filaOrigen["CREDITO"] = amortizacion;
                filaOrigenNiif["CREDITO"] = 0;
                
                filaOrigen["BASE"] = filaOrigenNiif["BASE"] = 0;
                filaDestino["BASE"] = filaDestinoNiif["BASE"] = 0;

                filaOrigen["DEBITO_NIIF"] = filaOrigenNiif["DEBITO_NIIF"] = filaOrigen["DEBITO_NIIFAUX"] = 0;
                filaDestino["DEBITO_NIIF"] = 0;
                filaDestino["DEBITO_NIIFAUX"] = amortizacionNIIF;
                filaDestinoNiif["DEBITO_NIIF"] = amortizacionNIIF;
                filaDestino["CREDITO_NIIF"] = filaDestinoNiif["CREDITO_NIIF"] = filaDestino["CREDITO_NIIFAUX"] = 0;
                filaOrigen["CREDITO_NIIF"] = 0;
                filaOrigen["CREDITO_NIIFAUX"] = amortizacionNIIF;
                filaOrigenNiif["CREDITO_NIIF"] = amortizacionNIIF;
                
                comprobante.Rows.Add(filaOrigen);	
				comprobante.Rows.Add(filaDestino);

                dtComprobanteNiif.Rows.Add(filaOrigenNiif);
                dtComprobanteNiif.Rows.Add(filaDestinoNiif);

                /*  // por ahora derogada la inflacion
				if(((DBFunctions.SingleData("SELECT tcue_codigo FROM mcuenta WHERE mcue_codipuc='"+activosFijos.Tables[0].Rows[i].ItemArray[13].ToString()+"'"))=="M"))
				{
					double inflacionAcumulada = System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[14].ToString());
					double valor = ((inflacionAcumulada)+System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[3].ToString())+System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[4].ToString()))*(paag/100);
					total+=valor;
					DataRow filaOrigenInflActivo = comprobante.NewRow();
					DataRow filaDestinoCmonInfl = comprobante.NewRow();
					filaOrigenInflActivo["CUENTA"]= activosFijos.Tables[0].Rows[i].ItemArray[9].ToString();
					filaDestinoCmonInfl["CUENTA"]= activosFijos.Tables[0].Rows[i].ItemArray[10].ToString();
					filaOrigenInflActivo["NIT"] = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
					filaDestinoCmonInfl["NIT"] = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
					filaOrigenInflActivo["PREF"] = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+tComprobante.SelectedItem.ToString()+"'");
					filaDestinoCmonInfl["PREF"] = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+tComprobante.SelectedItem.ToString()+"'");
					filaOrigenInflActivo["DOC_REF"] = this.ConsecutivoComprobante();
					filaDestinoCmonInfl["DOC_REF"] = this.ConsecutivoComprobante();;
					filaOrigenInflActivo["DETALLE"] = "AJUSTE POR INFLACION DEL ACTIVO FIJO "+activosFijos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
					filaDestinoCmonInfl["DETALLE"] = "AJUSTE POR INFLACION DEL ACTIVO FIJO "+activosFijos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
					filaOrigenInflActivo["SEDE"] = DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
					filaDestinoCmonInfl["SEDE"] = DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
					filaOrigenInflActivo["CENTRO_COSTO"] = activosFijos.Tables[0].Rows[i].ItemArray[1].ToString();
					filaDestinoCmonInfl["CENTRO_COSTO"] = activosFijos.Tables[0].Rows[i].ItemArray[1].ToString();
					filaOrigenInflActivo["BASE"] = 0;
					filaDestinoCmonInfl["BASE"] = 0;
					filaOrigenInflActivo["DEBITO"]= valor;
					filaDestinoCmonInfl["DEBITO"]= 0;
					filaOrigenInflActivo["CREDITO"]= 0;
					filaDestinoCmonInfl["CREDITO"]= valor;
					comprobante.Rows.Add(filaOrigenInflActivo);
					comprobante.Rows.Add(filaDestinoCmonInfl);
					//Aqui vamos a calcular la inflacion de la depreciacion acumulada del activo fijo
					double valorInflacionDepreciacionAcumulada = (System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[15].ToString())+System.Convert.ToDouble(activosFijos.Tables[0].Rows[i].ItemArray[5].ToString()))*(paag/100);
					total+=valorInflacionDepreciacionAcumulada;
					DataRow filaOrigenInflDepr = comprobante.NewRow();
					DataRow filaDestinoCmonDepr = comprobante.NewRow();
					filaOrigenInflDepr["CUENTA"] = activosFijos.Tables[0].Rows[i].ItemArray[11].ToString();
					filaDestinoCmonDepr["CUENTA"] = activosFijos.Tables[0].Rows[i].ItemArray[12].ToString();
					filaOrigenInflDepr["NIT"] = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
					filaDestinoCmonDepr["NIT"] = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
					filaOrigenInflDepr["PREF"] = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+tComprobante.SelectedItem.ToString()+"'");
					filaDestinoCmonDepr["PREF"] = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+tComprobante.SelectedItem.ToString()+"'");
					filaOrigenInflDepr["DOC_REF"] = this.ConsecutivoComprobante();
					filaDestinoCmonDepr["DOC_REF"] = this.ConsecutivoComprobante();;
					filaOrigenInflDepr["DETALLE"] = "AJUSTE POR INFLACION DE LA DEPRECIACION DEL ACTIVO FIJO "+activosFijos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
					filaDestinoCmonDepr["DETALLE"] = "AJUSTE POR INFLACION DE LA DEPRECIACION DEL ACTIVO FIJO "+activosFijos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
					filaOrigenInflDepr["SEDE"] = DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
					filaDestinoCmonDepr["SEDE"] = DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
					filaOrigenInflDepr["CENTRO_COSTO"] = activosFijos.Tables[0].Rows[i].ItemArray[1].ToString();
					filaDestinoCmonDepr["CENTRO_COSTO"] = activosFijos.Tables[0].Rows[i].ItemArray[1].ToString();
					filaOrigenInflDepr["BASE"] = 0;
					filaDestinoCmonDepr["BASE"] = 0;
					filaOrigenInflDepr["DEBITO"] = valorInflacionDepreciacionAcumulada;
					filaDestinoCmonDepr["DEBITO"] = 0;
					filaOrigenInflDepr["CREDITO"] = 0;
					filaDestinoCmonDepr["CREDITO"] = valorInflacionDepreciacionAcumulada;
					comprobante.Rows.Add(filaOrigenInflDepr);
					comprobante.Rows.Add(filaDestinoCmonDepr);
				}
				*/
            }
            //if (Session["comprobanteLEGAL"] == null)
            //{

            int sumdebito = 0;
            int sumcredito = 0;
            int sumdebitoNIIF = 0;
            int sumcreditoNIIF = 0;

            foreach (DataRow dr in comprobante.Rows)
            {
                sumdebito += Convert.ToInt32(dr["DEBITO"]);
                sumcredito += Convert.ToInt32(dr["CREDITO"]);
                sumdebitoNIIF += Convert.ToInt32(dr["DEBITO_NIIF"]);
                sumcreditoNIIF += Convert.ToInt32(dr["CREDITO_NIIF"]);
            }

            LbtotalDebito.Text = sumdebito.ToString("C");
            LbtotalCredito.Text = sumcredito.ToString("C");
            LbtotalDebitoNIIF.Text = sumdebitoNIIF.ToString("C");
            LbtotalCreditoNIIF.Text = sumcreditoNIIF.ToString("C");

            
            comprobanteG.DataSource = comprobante;
            comprobanteG.DataBind();
            Session["dtActivosComprobante"] = comprobante;
            Utils.MostrarAlerta(Response, "El proceso inicialmente le muestra los calculos de la depreciacion, en la parte inferior encontrara los botones para contabilizar la depreciacion, Por favor revice Los valores antes de dar click en el boton de Guardar");
            //}
            //else 
            //{
            //    comprobanteG.DataSource = (DataTable)Session["comprobanteLEGAL"];
            //    comprobanteG.DataBind();

            //    comprobanteGNiif.DataSource = comprobante;
            //    comprobanteGNiif.DataBind();
            //}

        }

		//FIN DE LA FUNCION QUE CREA EL COMPROBANTE
		
		//FUNCION QUE CALCULA EL CONSECUTIVO DEL COMPROBANTE QUE SE VA A CREAR
		protected  string ConsecutivoComprobante()
		{
			string lastID = DBFunctions.SingleData("SELECT pdoc_ultidocu FROM PDOCUMENTO WHERE pdoc_codigo = (select CCON_PREFNIIF from ccontabilidad)");
            int lastIDint = Convert.ToInt32(lastID);
            string newID = "";
 			if(lastID == "")
 				newID = "1";
 			else
                newID = (lastIDint + 1).ToString();

            //if (Session["comprobanteLEGAL"] != null)
            //{
            //    newID = (lastIDint + 2).ToString();
            //}

			return newID;
		}		
		//FIN DE LA FUNCION QUE CALCULA EL CONSECUTIVO
		
		////////////////////////////////////////////////
		/// 
		
		protected  void  Cambio_Mes(Object  Sender, EventArgs e)
		{
			string mesS = DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+mes.SelectedItem.ToString()+"'");
			/*
            if(DBFunctions.RecordExist("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes="+mesS+""))
			{
				valorPaag.Text = "Valor del PAAG para el mes elegido: "+DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes="+mesS+"");
				efectuar.Enabled = true;
				guardar.Enabled = true;
			}
			else
			{
				valorPaag.Text = "Lo Sentimos debe definir un valor para PAAG para el mes que acaba de elegir.";
				efectuar.Enabled = false;
				guardar.Enabled = false;
			}
            */
            efectuar.Enabled = true;
            guardar.Enabled = true;
            guardar1.Enabled = true;
            guardar2.Enabled = true;
        }
		
		protected  void  Cambio_Ano(Object  Sender, EventArgs e)
		{
			string mesS = DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+mes.SelectedItem.ToString()+"'");
			/*
            if(DBFunctions.RecordExist("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes="+mesS+""))
			{
				valorPaag.Text = "Valor del PAAG para el mes elegido: "+DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes="+mesS+"");
				efectuar.Enabled = true;
				guardar.Enabled = true;
			}
			else
			{
				valorPaag.Text = "Lo Sentimos debe definir un valor para PAAG para el mes que acaba de elegir.";
				efectuar.Enabled = false;
				guardar.Enabled = false;
			}
            */
            efectuar.Enabled = true;
            guardar.Enabled = true;
		}

        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(((DataTable)Session["dtActivosComprobante"]).Copy());
                ds.Tables[0].Columns.Remove("CREDITO_NIIF");
                ds.Tables[0].Columns.Remove("DEBITO_NIIF");

                Utils.ImprimeExcel(ds, "DepreciacionActivos");
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //////////////////////////////////////////////////////
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
