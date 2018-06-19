using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using AMS.Forms;
using AMS.DB;




namespace AMS.Contabilidad
{


    public class Comprobante
    {

        protected int i = 0, j = 0;
        protected bool insertions = true, consecutivo = true;
        protected string processDate, user, val, processMsg = "", tipoProceso = "";
        protected string type, number, year, month, date, detail;
        protected DataTable dtSource;


        public DataTable Source { set { dtSource = value; } get { return dtSource; } }
        public bool Consecutivo { set { consecutivo = value; } }
        public string Type { set { type = value; } get { return type; } }
        public string Number { set { number = value; } get { return number; } }
        public string Year { set { year = value; } get { return year; } }
        public string Month { set { month = value; } get { return month; } }
        public string Date { set { date = value; } get { return date; } }
        public string Detail { set { detail = value; } get { return detail; } }
        public string ProcessDate { set { processDate = value; } }
        public string User { set { user = value; } get { return user; } }
        public string Value { set { val = value; } get { return val; } }
        public string ProcessMsg { get { return processMsg; } }
        public string TipoProceso { set { tipoProceso = value; } get { return tipoProceso; } }
        public string Basura = "";


        public Comprobante()
        {

        }

        public Comprobante(DataTable sr)
        {
            dtSource = sr;

        }


        /*
        public void RollBackValues()
        {
            ArrayList sqlStrings= new ArrayList();
            DataSet dsRBV =  new DataSet();
            string account, tipoImpu, clase, accountUtility,nit; 
            double valoDebi=0, valoCred=0, debUtility=0, credUtility=0;
		
            DBFunctions.Request(dsRBV,IncludeSchema.NO,"select dcuenta.mcue_codipuc, dcuenta.mnit_nit, dcuenta.dcue_codirefe, dcuenta.dcue_numerefe, dcuenta.dcue_detalle, dcuenta.palm_almacen, dcuenta.pcen_codigo, dcuenta.dcue_valodebi, dcuenta.dcue_valocred, dcuenta.dcue_valobase, mcuenta.mcue_tipoimpu, mcuenta.mcue_clase from dcuenta, mcuenta where dcuenta.mcom_numedocu=" + number + " AND dcuenta.mcue_codipuc=mcuenta.mcue_codipuc");

            for (i=0; i<dsRBV.Tables[0].Rows.Count -1; i++)
            {
                account  = dsRBV.Tables[0].Rows[i].ItemArray[0].ToString();
                valoDebi = Convert.ToDouble(dsRBV.Tables[0].Rows[i].ItemArray[7]);
                valoCred = Convert.ToDouble(dsRBV.Tables[0].Rows[i].ItemArray[8]);
                tipoImpu = dsRBV.Tables[0].Rows[i].ItemArray[10].ToString();
                clase    = dsRBV.Tables[0].Rows[i].ItemArray[11].ToString();
                nit      = dsRBV.Tables[0].Rows[i].ItemArray[1].ToString();
			
                if(clase == "N")
                {
                    debUtility += valoDebi;
                    credUtility += valoCred;
                }
			
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valodebi=msal_valodebi-" + valoDebi.ToString() + ", msal_valocred=msal_valocred-" + valoCred.ToString() + " WHERE mcue_codipuc='" + account + "' AND pano_ano=" + year + " AND  pmes_mes=" + month + "");
                if(tipoImpu == "A")
                    sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi-" + valoDebi.ToString() + ", maux_valocred=maux_valocred-" + valoCred.ToString() + " WHERE mcue_codipuc='" + account + "' AND pano_ano=" + year + " AND  pmes_mes=" + month + " AND mnit_nit='"+nit+"';");
            }

            accountUtility = DBFunctions.SingleData("SELECT mcue_codipuc FROM ccontabilidad");
            sqlStrings.Add("UPDATE msaldocuenta SET msal_valocred = msal_valocred-(" + (credUtility-debUtility).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "'");
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipucNOMI FROM ccontabilidad");
            sqlStrings.Add("UPDATE msaldocuenta SET msal_valoDEBI = msal_valoDEBI-(" + (credUtility - debUtility).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "'");
            sqlStrings.Add("DELETE FROM dcuenta WHERE mcom_numedocu=" + number + "");
            sqlStrings.Add("DELETE FROM mcomprobante WHERE mcom_numedocu=" + number + "");
            DBFunctions.Transaction(sqlStrings);			
        }
        */

        //public bool CommitValues()
        public bool CommitValues(ref ArrayList sqlStrings)
        {
            //ArrayList sqlStrings = new ArrayList();
            string tipoImpu, accountUtility, tipoNomi;
            double debUtility = 0, credUtility = 0, debUtilityNiif = 0, credUtilityNiif = 0;
             
            DataSet dsSaldosCuenta = new DataSet();
            DataSet dsCuentas = new DataSet();
            DataSet dsAuxiliarCuentas = new DataSet();
            
            //ArrayList sqlStrings = new ArrayList();
            ArrayList creacion = new ArrayList();

            Hashtable h_msaldocuenta = new Hashtable();
            Hashtable h_mauxiliarcuenta = new Hashtable();
             
            //Verifico el Consecutivo Automatico
            if (consecutivo == true)
            {
                //Verifico que el Comprobante no Exista
                while (DBFunctions.RecordExist("SELECT * FROM mcomprobante WHERE pdoc_codigo = '" + type + "' AND mcom_numedocu = " + number + " "))
                {
                    DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu = pdoc_ultidocu + 1 WHERE pdoc_codigo ='" + type + "'");
                    number = (Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo ='" + type + "'")) + 1).ToString();
                }
                //Actualizo el consecutivo
                DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu = " + number + " WHERE pdoc_codigo ='" + type + "'");
            }
            string horaProc = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sqlStrings.Add("INSERT INTO mcomprobante VALUES('" + type + "', " + number + ", " + (Convert.ToDateTime(date).Year) + ", " + month + ", '" + date + "', '" + detail + "', '" + horaProc + "', '" + user + "', " + val + ")");

            DBFunctions.Request(dsSaldosCuenta, IncludeSchema.NO, "SELECT * FROM msaldocuenta WHERE pano_ano=" + year + " AND pmes_mes=" + month);
            for (i = 0; i < dtSource.Rows.Count; i++)
            {
                //if ((!DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "")))
                DataRow[] drSalCuenta = dsSaldosCuenta.Tables[0].Select("mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'");

                if (!(Convert.ToDouble(dtSource.Rows[i][7]) == 0 && Convert.ToDouble(dtSource.Rows[i][8]) == 0 &&
                         Convert.ToDouble(dtSource.Rows[i][10]) == 0 && Convert.ToDouble(dtSource.Rows[i][11]) == 0))
                {
                    if (drSalCuenta.Length == 0)
                    {
                        if (h_msaldocuenta.ContainsKey(dtSource.Rows[i][0].ToString()) == false)
                        {
                            //creacion.Add("INSERT INTO msaldocuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', 0,0,0,0,0)");
                            sqlStrings.Add("INSERT INTO msaldocuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', 0,0,0,0,0)");
                            h_msaldocuenta.Add(dtSource.Rows[i][0].ToString(), dtSource.Rows[i][0].ToString());
                        }
                    }
                }
                //if (!DBFunctions.RecordExist("SELECT * FROM mauxiliarcuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'"))
                //{
                //    if (h_mauxiliarcuenta.ContainsKey(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString()) == false)
                //    {
                //        //creacion.Add("INSERT into mauxiliarcuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][1].ToString() + "', 0, 0, 0, 0 )");
                //        sqlStrings.Add("INSERT into mauxiliarcuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][1].ToString() + "', 0, 0, 0, 0 )");
                //        h_mauxiliarcuenta.Add(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString(),dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString());
                //    }
                //}
            }

            //Se comenta para dejar todas las instrucciones SQL en un solo llamado... Evitar grabar solo la mitad de datos en DB.
            //if(DBFunctions.Transaction(creacion))
            //{
            //    status = true;
            //    processMsg += DBFunctions.exceptions + "<br>";
            //}
            //else
            //    processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";

            DBFunctions.Request(dsCuentas, IncludeSchema.NO, "SELECT timp_codigo, tcla_codigo, mcue_codipuc FROM mcuenta");
            DBFunctions.Request(dsAuxiliarCuentas, IncludeSchema.NO, "SELECT * FROM mauxiliarcuenta WHERE pano_ano=" + year + " AND pmes_mes=" + month);

            for (i = 0; i < dtSource.Rows.Count; i++)
            {
                DataRow[] drCuenta = dsCuentas.Tables[0].Select("mcue_codipuc = '" + dtSource.Rows[i][0].ToString() + "'");
                tipoImpu = drCuenta[0][0].ToString();
                tipoNomi = drCuenta[0][1].ToString();
                //tipoImpu = DBFunctions.SingleData("SELECT timp_codigo FROM mcuenta where mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'");
                //tipoNomi = DBFunctions.SingleData("SELECT tcla_codigo FROM mcuenta where mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'");

                //Condicion para evitar la creacion de registros con valores en ceros unicamente.
                if (!(Convert.ToDouble(dtSource.Rows[i][7]) == 0 && Convert.ToDouble(dtSource.Rows[i][8]) == 0 &&
                         Convert.ToDouble(dtSource.Rows[i][10]) == 0 && Convert.ToDouble(dtSource.Rows[i][11]) == 0))
                {
                    if (tipoNomi == "N")
                    {
                        debUtility += Convert.ToDouble(dtSource.Rows[i][7]);
                        credUtility += Convert.ToDouble(dtSource.Rows[i][8]);

                        debUtilityNiif += Convert.ToDouble(dtSource.Rows[i][10]);
                        credUtilityNiif += Convert.ToDouble(dtSource.Rows[i][11]);
                    }
                    string prefijo = "";
                    try { prefijo = dtSource.Rows[i][2].ToString(); }
                    catch(Exception er) { prefijo = dtSource.Rows[i][12].ToString(); } //Condicion para enviar el codigo del activo NIIF para depreciacion de activos NIIF
                    
                    sqlStrings.Add("INSERT INTO dcuenta VALUES('" + type + "', " + number + ", '" + dtSource.Rows[i][0].ToString() + "', '" + prefijo + "', " + dtSource.Rows[i][3].ToString() + ", " + (i + 1).ToString() + ", '" + dtSource.Rows[i][1].ToString() + "', '" + dtSource.Rows[i][5].ToString() + "', '" + dtSource.Rows[i][6].ToString() + "', '" + dtSource.Rows[i][4].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", " + dtSource.Rows[i][9].ToString() + "," + dtSource.Rows[i][10].ToString() + "," + dtSource.Rows[i][11].ToString() + ")");
                    sqlStrings.Add("UPDATE msaldocuenta SET msal_valodebi=msal_valodebi+" + dtSource.Rows[i][7].ToString() + ", msal_valocred=msal_valocred+" + dtSource.Rows[i][8].ToString() +
                                   ", msal_niifdebi=msal_niifdebi+" + dtSource.Rows[i][10].ToString() + ", msal_niifcred=msal_niifcred+" + dtSource.Rows[i][11].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");

                    if (tipoImpu == "A")
                    {
                        //if (DBFunctions.RecordExist("SELECT * FROM mauxiliarcuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'"))
                        DataRow[] drAuxCuenta = dsAuxiliarCuentas.Tables[0].Select("mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                        if (drAuxCuenta.Length > 0)
                            sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi+" + dtSource.Rows[i][7].ToString() + ", maux_valocred=maux_valocred+" + dtSource.Rows[i][8].ToString() +
                                           ", maux_niifdebi=maux_niifdebi+" + dtSource.Rows[i][10].ToString() + ", maux_niifcred=maux_niifcred+" + dtSource.Rows[i][11].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                        else
                        {

                            if (h_mauxiliarcuenta.ContainsKey(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString()) == false)
                            {
                                sqlStrings.Add("INSERT into mauxiliarcuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][1].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + "," + dtSource.Rows[i][10].ToString() + "," + dtSource.Rows[i][11].ToString() + ")");
                                h_mauxiliarcuenta.Add(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString(), dtSource.Rows[i][0].ToString());
                            }
                            else
                                sqlStrings.Add("UPDATE mauxiliarcuenta SET  maux_valodebi=maux_valodebi+" + dtSource.Rows[i][7].ToString() + ", maux_valocred=maux_valocred+" + dtSource.Rows[i][8].ToString() +
                                               ", maux_niifdebi =maux_niifdebi+" + dtSource.Rows[i][10].ToString() + ", maux_niifcred=maux_niifcred+" + dtSource.Rows[i][11].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                        }
                    }


                    //Proceso NIIF
                    //if (tipoProceso == "Efectuar NIIF")
                    //{
                    //    sqlStrings.Add("INSERT INTO dcuenta VALUES('" + type + "', " + number + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][10].ToString() + "', " + dtSource.Rows[i][3].ToString() + ", " + (i + 1).ToString() + ", '" + dtSource.Rows[i][1].ToString() + "', '" + dtSource.Rows[i][5].ToString() + "', '" + dtSource.Rows[i][6].ToString() + "', '" + dtSource.Rows[i][4].ToString() + "', 0, 0, " + dtSource.Rows[i][9].ToString() + ", "+ dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ")");
                    //    sqlStrings.Add("UPDATE msaldocuenta SET msal_niifdebi=msal_niifdebi+" + dtSource.Rows[i][7].ToString() + ", msal_niifcred=msal_niifcred+" + dtSource.Rows[i][8].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");

                    //    if (tipoImpu == "A")
                    //    {
                    //        if (DBFunctions.RecordExist("SELECT * FROM mauxiliarcuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'"))
                    //            sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_niifdebi=maux_niifdebi+" + dtSource.Rows[i][7].ToString() + ", maux_niifcred=maux_niifcred+" + dtSource.Rows[i][8].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                    //        else
                    //        {

                    //            if (h_mauxiliarcuenta.ContainsKey(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString()) == false)
                    //            {
                    //                sqlStrings.Add("INSERT into mauxiliarcuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][1].ToString() + "', 0, 0," + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ")");
                    //                h_mauxiliarcuenta.Add(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString(), dtSource.Rows[i][0].ToString());
                    //            }
                    //            else
                    //                sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_niifdebi=maux_niifdebi+" + dtSource.Rows[i][7].ToString() + ", maux_niifcred=maux_niifcred+" + dtSource.Rows[i][8].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                    //        }
                    //    }
                    //} //Proceso LEGAL
                    //else
                    //{
                    //    sqlStrings.Add("INSERT INTO dcuenta VALUES('" + type + "', " + number + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][10].ToString() + "', " + dtSource.Rows[i][3].ToString() + ", " + (i + 1).ToString() + ", '" + dtSource.Rows[i][1].ToString() + "', '" + dtSource.Rows[i][5].ToString() + "', '" + dtSource.Rows[i][6].ToString() + "', '" + dtSource.Rows[i][4].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", " + dtSource.Rows[i][9].ToString() + ", 0, 0)");
                    //    sqlStrings.Add("UPDATE msaldocuenta SET msal_valodebi=msal_valodebi+" + dtSource.Rows[i][7].ToString() + ", msal_valocred=msal_valocred+" + dtSource.Rows[i][8].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");

                    //    if (tipoImpu == "A")
                    //    {
                    //        if (DBFunctions.RecordExist("SELECT * FROM mauxiliarcuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'"))
                    //            sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi+" + dtSource.Rows[i][7].ToString() + ", maux_valocred=maux_valocred+" + dtSource.Rows[i][8].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                    //        else
                    //        {

                    //            if (h_mauxiliarcuenta.ContainsKey(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString()) == false)
                    //            {
                    //                sqlStrings.Add("INSERT into mauxiliarcuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][1].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", 0, 0)");
                    //                h_mauxiliarcuenta.Add(dtSource.Rows[i][0].ToString() + " " + dtSource.Rows[i][1].ToString(), dtSource.Rows[i][0].ToString());
                    //            }
                    //            else
                    //                sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi+" + dtSource.Rows[i][7].ToString() + ", maux_valocred=maux_valocred+" + dtSource.Rows[i][8].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                    //        }

                    //    }
                    //}

                }
            }

            // Al final de cada comprobante, debe imputar la utilidad tanto en la real como en la nominal

            //utilidad  Cuenta Real
            accountUtility = DBFunctions.SingleData("SELECT c.mcue_codipuc FROM ccontabilidad c, mcuenta m where c.mcue_codipuc = m.mcue_codipuc and m.timp_codigo <> 'N' AND M.TCLA_CODIGO = 'R';");
            if (accountUtility == "")
            {
                processMsg += "Error: No HA parametrizado la cuenta REAL de UTILIDAD en configuración de Contabilidad";
                return false;
            }

            if (this.tipoProceso == "1" || DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano = " + year + " AND pmes_mes = " + month + ""))
            {
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valocred=msal_valoCRED + (" + (credUtility - debUtility).ToString() + "), msal_niifcred=msal_niifCRED+(" + (credUtilityNiif - debUtilityNiif).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            }
            else
            {
                sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "', 0, (" + (credUtility - debUtility).ToString() + "), 0, 0, (" + (credUtilityNiif - debUtilityNiif).ToString() + "))");
            }

            //if (DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano = " + year + " AND pmes_mes = " + month + ""))
            //{
            //    if (tipoProceso == "Efectuar NIIF")
            //    {
            //        sqlStrings.Add("UPDATE msaldocuenta SET msal_niifcred=msal_niifCRED+(" + (credUtility - debUtility).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            //    }
            //    else
            //    {
            //        sqlStrings.Add("UPDATE msaldocuenta SET msal_valocred=msal_valoCRED+(" + (credUtility - debUtility).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            //    }
            //}
            //else
            //{
            //    if (tipoProceso == "Efectuar NIIF")
            //    {
            //        sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "', 0, 0, 0, 0, (" + (credUtility - debUtility).ToString() + "))");
            //    }
            //    else
            //    {
            //        sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "', 0, (" + (credUtility - debUtility).ToString() + "), 0, 0, 0)");
            //    }
            //}

            //utilidad  Cuenta Nominal
            accountUtility = DBFunctions.SingleData("SELECT c.mcue_codipucNOMI FROM ccontabilidad c, mcuenta m where c.mcue_codipucNOMI = m.mcue_codipuc and m.timp_codigo <> 'N' AND M.TCLA_CODIGO = 'N';");
            if (accountUtility == "")
            {
                processMsg += "Error: No HA parametrizado la cuenta NOMINAL de UTILIDAD en configuración de Contabilidad";
                return false;
            }

            if (this.tipoProceso == "1" || DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano = " + year + " AND pmes_mes = " + month + ""))
            {
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valoDEBI=msal_valoDEBI+(" + (credUtility - debUtility).ToString() + "), msal_niifDEBI=msal_niifDEBI+(" + (credUtilityNiif - debUtilityNiif).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            }
            else
            {
                sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "', (" + (credUtility - debUtility).ToString() + "), 0, 0, (" + (credUtilityNiif - debUtilityNiif).ToString() + "), 0)");
            }

            //sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "', (" + (credUtility - debUtility).ToString() + "), 0, 0, (" + (credUtilityNiif - debUtilityNiif).ToString() + "),0)");

            //if (DBFunctions.Transaction(sqlStrings))
            //{
            //    status = true;
            //    processMsg += DBFunctions.exceptions + "<br>";
            //}
            //else
            //{
            //    processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";

            //}

            //return status;
            return true;

        }

        public void CommitValuesCommandMode()
        {
            ArrayList sqlStrings = new ArrayList();
            string tipoImpu, accountUtility;
            double debUtility = 0, credUtility = 0, debUtilityNiif = 0, credUtilityNiif = 0;

            processMsg += "INSERT INTO mcomprobante VALUES('" + type + "', " + number + ", " + year + ", " + month + ", '" + date + "', '" + detail + "', CURRENT DATE, '" + user + "', " + val + ")" + "<br><br>";

            for (i = 0; i < dtSource.Rows.Count; i++)
            {
                processMsg += "INSERT INTO dcuenta VALUES('" + type + "', " + number + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][2].ToString() + "', " + dtSource.Rows[i][3].ToString() + ", " + (i + 1).ToString() + ", '" + dtSource.Rows[i][1].ToString() + "', '" + dtSource.Rows[i][5].ToString() + "', '" + dtSource.Rows[i][6].ToString() + "', '" + dtSource.Rows[i][4].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", " + dtSource.Rows[i][9].ToString() + ",, " + dtSource.Rows[i][10].ToString() + ", " + dtSource.Rows[i][11].ToString() + ")" + "<br><br>";

                //tipoImpu = DBFunctions.SingleData("SELECT timp_codigo FROM mcuenta where mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'");
                tipoImpu = DBFunctions.SingleData("SELECT tcla_codigo FROM mcuenta where mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'");
                processMsg += "checking: " + "SELECT timp_codigo FROM mcuenta where mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'" + "<br><br>";

                if (tipoImpu == "N")
                {
                    debUtility += Convert.ToDouble(dtSource.Rows[i][7]);
                    credUtility += Convert.ToDouble(dtSource.Rows[i][8]);
                    debUtilityNiif += Convert.ToDouble(dtSource.Rows[i][10]);
                    credUtilityNiif += Convert.ToDouble(dtSource.Rows[i][11]);
                }

                //saldos
                if (DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + ""))
                    sqlStrings.Add("UPDATE msaldocuenta SET msal_valodebi=msal_valodebi+" + dtSource.Rows[i][7].ToString() + ", msal_valocred=msal_valocred+" + dtSource.Rows[i][8].ToString() + ", msal_NIIFdebi=msal_NIIFdebi+" + dtSource.Rows[i][10].ToString() + ", msal_NIIFcred=msal_NIIFcred+" + dtSource.Rows[i][11].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
                else
                    sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", 0, " + dtSource.Rows[i][10].ToString() + ", " + dtSource.Rows[i][11].ToString() + ", )");

                //auxiliares
                if (tipoImpu == "A")
                {
                    if (DBFunctions.RecordExist("SELECT * FROM mauxiliarcuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'"))
                    {
                        sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi+" + dtSource.Rows[i][7].ToString() + ", maux_valocred=maux_valocred+" + dtSource.Rows[i][8].ToString() + ", maux_niifdebi=maux_niifdebi+" + dtSource.Rows[i][10].ToString() + ", maux_niifcred=maux_niifcred+" + dtSource.Rows[i][11].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");

                    }
                    else
                        sqlStrings.Add("INSERT into mauxiliarcuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][1].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", , " + dtSource.Rows[i][10].ToString() + ", " + dtSource.Rows[i][11].ToString() + ")");
                }
            }
            //        ESTA RUTiNA ESTA REPETIDA, se debe unificar en un solo metodo con la grabacion de comprobantes
            //utilidad
            //  Utilidad de la CUENTA REAL
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipuc FROM ccontabilidad");
            if (DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " "))
            {
                processMsg += "UPDATE msaldocuenta SET msal_valocred=msal_valocred+(" + (credUtility - debUtility).ToString() + "), msal_niifcred=msal_niifcred+(" + (credUtilityNiif - debUtilityNiif).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "'" + "<br><br>";
            }
            else
            {
                processMsg += "INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "', 0, (" + (credUtility - debUtility).ToString() + "), 0, 0, (" + (credUtilityNiif - debUtilityNiif).ToString() + "))";
            }

            //  Utilidad de la CUENTA NOMINAL
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipucNOMI FROM ccontabilidad");
            if (DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " "))
            {
                processMsg += "UPDATE msaldocuenta SET msal_valoDEBI=msal_valoDEBI+(" + (credUtility - debUtility).ToString() + "), msal_niifDEBI=msal_niifDEBI+(" + (credUtilityNiif - debUtilityNiif).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "'" + "<br><br>";
            }
            else
            {
                processMsg += "INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "',(" + (credUtility - debUtility).ToString() + "), 0, 0,(" + (credUtilityNiif - debUtilityNiif).ToString() + "), 0)";
            }
        }

        public bool UpdateRecord(string number)
        {
            string tipoImpu, accountUtility, tipoNomi;
            double UtilidadAnterior = 0, UtilidadAnteriorNiif = 0;
            bool status = false;
            //JFSC 11022008 Poner en comentario por no ser usado
            //bool repetido=false;
            ArrayList sqlStrings = new ArrayList();
            // BORRADO DE ANTERIOR COMPROBANTE
            DataSet arregloSaldos = new DataSet();

            DBFunctions.Request(arregloSaldos, IncludeSchema.NO, "SELECT mcue_codipuc, dcue_valodebi, dcue_valocred, mnit_nit, dcue_niifdebi, dcue_niifcred FROM dcuenta where pdoc_codigo='" + type + "' AND mcom_numedocu= " + number + "");
            for (int i = 0; i < arregloSaldos.Tables[0].Rows.Count; i++)
            {
                //JFSC 01/02/2008 Adición de actualización de nombre de la cuenta puc en msaldocuenta
                sqlStrings.Add("UPDATE msaldocuenta SET  msal_valodebi=msal_valodebi-" + arregloSaldos.Tables[0].Rows[i]["dcue_valodebi"].ToString() + ", msal_valocred=msal_valocred-" + arregloSaldos.Tables[0].Rows[i]["dcue_valocred"].ToString() + ",  msal_NIIFdebi=msal_NIIFdebi-" + arregloSaldos.Tables[0].Rows[i]["dcue_niifdebi"].ToString() + ", msal_NIIFcred=msal_NIIFcred-" + arregloSaldos.Tables[0].Rows[i]["dcue_niifcred"].ToString() + " WHERE pano_ano=" + year + " AND pmes_mes=" + month + " AND mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i]["mcue_codipuc"].ToString() + "'");
                tipoImpu = DBFunctions.SingleData("SELECT timp_codigo FROM mcuenta where mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i]["mcue_codipuc"].ToString() + "'");
                tipoNomi = DBFunctions.SingleData("SELECT tcla_codigo FROM mcuenta where mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i]["mcue_codipuc"].ToString() + "'");


                if (tipoNomi == "N")
                {
                    UtilidadAnterior +=
                        (Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_valocred"])
                        - Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_valodebi"]));
                    UtilidadAnteriorNiif +=
                        (Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_niifcred"])
                        - Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_niifdebi"]));
                }
                if (tipoImpu == "A")
                {
                    //nit = DBFunctions.SingleData("SELECT mnit_nit FROM mauxiliarcuenta WHERE pano_ano="+year+" AND pmes_mes="+month+" AND mcue_codipuc='"+arregloSaldos.Tables[0].Rows[i].ItemArray[0].ToString()+"'");
                    //sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi+" + dtSource.Rows[i][7].ToString() + ", maux_valocred=maux_valocred+" + dtSource.Rows[i][8].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                    sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi-" + arregloSaldos.Tables[0].Rows[i]["dcue_valodebi"].ToString() + ", maux_valocred=maux_valocred-" + arregloSaldos.Tables[0].Rows[i]["dcue_valocred"].ToString() + ",  maux_niifdebi=maux_niifdebi-" + arregloSaldos.Tables[0].Rows[i]["dcue_niifdebi"].ToString() + ", maux_niifcred=maux_niifcred-" + arregloSaldos.Tables[0].Rows[i]["dcue_niifcred"].ToString() + " WHERE mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i]["mcue_codipuc"].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + arregloSaldos.Tables[0].Rows[i]["mnit_nit"] + "';");
                }
            }

            //Guardar historial DCUENTA
            DataSet dsElimina = new DataSet();
            string strHistorialCambio = "", strHistorialCambioL = "", strHistorialCambioR = "";
            DBFunctions.Request(dsElimina, IncludeSchema.NO, "Select * FROM dcuenta WHERE pdoc_codigo='" + type + "' AND mcom_numedocu=" + number + ";");
            strHistorialCambioL = "DEFAULT,'DCUENTA','U','";
            strHistorialCambioR = "'," +
                "'" + HttpContext.Current.User.Identity.Name.ToLower() + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";

            accountUtility = DBFunctions.SingleData("SELECT mcue_codipuc FROM ccontabilidad");
            sqlStrings.Add("UPDATE msaldocuenta SET msal_valocred = msal_valocred-( " + UtilidadAnterior + " ), msal_niifcred = msal_niifcred-( " + UtilidadAnteriorNiif + " ) WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipucNOMI FROM ccontabilidad");
            sqlStrings.Add("UPDATE msaldocuenta SET msal_valoDEBI = msal_valoDEBI-( " + UtilidadAnterior + " ), msal_niifDEBI = msal_niifDEBI-( " + UtilidadAnteriorNiif + " ) WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            sqlStrings.Add("Delete FROM dcuenta WHERE pdoc_codigo='" + type + "' AND mcom_numedocu=" + number);
            sqlStrings.Add("Delete FROM mcomprobante where pdoc_codigo='" + type + "' AND mcom_numedocu=" + number);
            // FIN BORRADO DE ANTERIOR COMPROBANTE
            // CREACION DE NUEVO COMPROBANTE
            tipoImpu = accountUtility = tipoNomi = String.Empty;
            double UtilidadActual = 0, UtilidadActualNiif = 0;
            status = false;
            ArrayList msaldoCuenta = new ArrayList();
            ArrayList mauxiliarCuenta = new ArrayList();
            ArrayList msaldoCuentaNiif = new ArrayList();
            ArrayList mauxiliarCuentaNiif = new ArrayList();
            //Verifico el Consecutivo Automatico

            sqlStrings.Add("INSERT INTO mcomprobante VALUES('" + type + "', " + number + ", " + year + ", " + month + ", '" + date + "', '" + detail + "', CURRENT DATE, '" + user + "', " + val + ")");

            ///Correccion realizada para crear las cuentas dentro de mcuenta antes de que se le empicen a enviar valores
            for (i = 0; i < dtSource.Rows.Count; i++)
            {
                /*for(int j=i-1;j>=0;j--)
                    if(dtSource.Rows[i][0].ToString()==dtSource.Rows[j][0].ToString())		
                        repetido=true;*/
                if ((!DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "")) && (!BuscarValorArrayList(msaldoCuenta, dtSource.Rows[i][0].ToString())))
                {
                    sqlStrings.Add("INSERT INTO msaldocuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', 0,0,0,0,0)");
                    msaldoCuenta.Add(dtSource.Rows[i][0].ToString());
                    msaldoCuentaNiif.Add(dtSource.Rows[i][0].ToString());
                }
            }

            for (i = 0; i < dtSource.Rows.Count; i++)
            {
                sqlStrings.Add("INSERT INTO dcuenta VALUES('" + type + "', " + number + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][2].ToString() + "', " + dtSource.Rows[i][3].ToString() + ", " + (i + 1).ToString() + ", '" + dtSource.Rows[i][1].ToString() + "', '" + dtSource.Rows[i][5].ToString() + "', '" + dtSource.Rows[i][6].ToString() + "', '" + dtSource.Rows[i][4].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", " + dtSource.Rows[i][9].ToString() + ", " + dtSource.Rows[i][10].ToString() + ", " + dtSource.Rows[i][11].ToString() + ")");

                tipoImpu = DBFunctions.SingleData("SELECT timp_codigo FROM mcuenta where mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'");
                tipoNomi = DBFunctions.SingleData("SELECT tcla_codigo FROM mcuenta where mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "'");

                if (tipoNomi == "N")
                {
                    UtilidadActual +=
                        (Convert.ToDouble(dtSource.Rows[i][8])     // credito
                        - Convert.ToDouble(dtSource.Rows[i][7]));   // debito
                    UtilidadActualNiif +=
                        (Convert.ToDouble(dtSource.Rows[i][11])     // credito Niif
                        - Convert.ToDouble(dtSource.Rows[i][10]));  // debito Niif
                }

                //saldos
                if (DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "") || BuscarValorArrayList(msaldoCuenta, dtSource.Rows[i][0].ToString()))
                    sqlStrings.Add("UPDATE msaldocuenta SET msal_valodebi=msal_valodebi+" + dtSource.Rows[i][7].ToString() + ", msal_valocred=msal_valocred+" + dtSource.Rows[i][8].ToString() + ", msal_NIIFdebi=msal_NIIFdebi+" + dtSource.Rows[i][10].ToString() + ", msal_NIIFcred=msal_NIIFcred+" + dtSource.Rows[i][11].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
                else
                    sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", 0, " + dtSource.Rows[i][10].ToString() + ", " + dtSource.Rows[i][11].ToString() + ")");

                //auxiliares
                if (tipoImpu == "A")
                {
                    if (DBFunctions.RecordExist("SELECT * FROM mauxiliarcuenta WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'") || BuscarValorArrayList(mauxiliarCuenta, dtSource.Rows[i][0].ToString() + "@" + dtSource.Rows[i][1].ToString()))
                        sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi+" + dtSource.Rows[i][7].ToString() + ", maux_valocred=maux_valocred+" + dtSource.Rows[i][8].ToString() + ", maux_NIIFdebi=maux_NIIFdebi+" + dtSource.Rows[i][10].ToString() + ", maux_NIIFcred=maux_NIIFcred+" + dtSource.Rows[i][11].ToString() + " WHERE mcue_codipuc='" + dtSource.Rows[i][0].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + dtSource.Rows[i][1].ToString() + "'");
                    else
                    {
                        sqlStrings.Add("INSERT into mauxiliarcuenta VALUES(" + year + ", " + month + ", '" + dtSource.Rows[i][0].ToString() + "', '" + dtSource.Rows[i][1].ToString() + "', " + dtSource.Rows[i][7].ToString() + ", " + dtSource.Rows[i][8].ToString() + ", " + dtSource.Rows[i][10].ToString() + ", " + dtSource.Rows[i][11].ToString() + ")");
                        mauxiliarCuenta.Add(dtSource.Rows[i][0].ToString() + "@" + dtSource.Rows[i][1].ToString());
                        mauxiliarCuentaNiif.Add(dtSource.Rows[i][0].ToString() + "@" + dtSource.Rows[i][1].ToString());  //ver
                    }
                }
            }
            // utilidad
            //               ESTA RUTINA ESTA REPETIDA, se debe unificar con la de captura de comprobante
            //  Utilidad CUENTA REAL
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipuc FROM ccontabilidad");
            if (DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano = " + year + " AND pmes_mes = " + month + "") || BuscarValorArrayList(msaldoCuenta, accountUtility))
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valocred = msal_valocred+( " + UtilidadActual + " ), msal_NIIFcred = msal_NIIFcred+( " + UtilidadActualNiif + " ) WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            else
            {
                sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "', 0, ( " + UtilidadActual + " ), 0, 0, ( " + UtilidadActualNiif + " ))");
                msaldoCuenta.Add(accountUtility);
                msaldoCuentaNiif.Add(accountUtility);  // ver
            }

            //  Utilidad CUENTA NOMINAL
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipucNOMI FROM ccontabilidad");
            if (DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano = " + year + " AND pmes_mes = " + month + "") || BuscarValorArrayList(msaldoCuenta, accountUtility))
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valoDEBI = msal_valoDEBI+( " + UtilidadActual + " ), msal_niifDEBI = msal_niifDEBI+( " + UtilidadActualNiif + " ) WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            else
            {
                sqlStrings.Add("INSERT into msaldocuenta VALUES(" + year + ", " + month + ", '" + accountUtility + "',  ( " + UtilidadActual + " ), 0, 0,  ( " + UtilidadActualNiif + " ), 0)");
                msaldoCuenta.Add(accountUtility);
            }


            // Guardar en MHISTORIAL_CAMBIOS los DCUENTA originales
            for (int n = 0; n < dsElimina.Tables[0].Rows.Count; n++)
            {
                strHistorialCambio = "";
                for (int c = 0; c < dsElimina.Tables[0].Columns.Count; c++)
                    strHistorialCambio += dsElimina.Tables[0].Rows[n][c].ToString() + ",";
                sqlStrings.Add("INSERT INTO MHISTORIAL_CAMBIOS VALUES(" + strHistorialCambioL +
                    strHistorialCambio.Substring(0, strHistorialCambio.Length - 1) + strHistorialCambioR);
            }

            // FIN CREACION DE NUEVO COMPROBANTE
            if (DBFunctions.Transaction(sqlStrings))
            {
                status = true;
                processMsg += DBFunctions.exceptions + "<br>";
            }
            else
                processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";

            /*
            if (status)   //  impresión del documento soporte
            {
                try
                {
                    formatoRecibo.Prefijo = type;
                    formatoRecibo.Numero = Convert.ToInt32(number);
                    formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + type + "'");
                    if (formatoRecibo.Codigo != string.Empty)
                    {
                        if (formatoRecibo.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                    }
                }
                catch
                {
                }
            }
            */

            return status;
        }

        /////////////////////////////////////

        public bool DeleteRecord(string number)
        {
            ArrayList sqlStrings = new ArrayList();
            DataSet arregloSaldos = new DataSet();
            DataSet dsElimina = new DataSet();
            string tipoImpu, accountUtility, claseCuenta;
            double debUtility = 0, credUtility = 0, debUtilityNiif = 0, credUtilityNiif = 0;
            string strHistorialCambioL = "", strHistorialCambioR = "", strHistorialCambio = "";

            DBFunctions.Request(arregloSaldos, IncludeSchema.NO, "SELECT mcue_codipuc, dcue_valodebi, dcue_valocred, mnit_nit, dcue_niifdebi, dcue_niifcred FROM dcuenta where pdoc_codigo='" + type + "' AND mcom_numedocu= " + number + "");
            for (int i = 0; i < arregloSaldos.Tables[0].Rows.Count; i++)
            {
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valodebi = msal_valodebi-(" + arregloSaldos.Tables[0].Rows[i]["dcue_valodebi"].ToString() + "), msal_valocred = msal_valocred-(" + arregloSaldos.Tables[0].Rows[i]["dcue_valocred"].ToString() + "), msal_NIIFdebi = msal_NIIFdebi-(" + arregloSaldos.Tables[0].Rows[i]["dcue_niifdebi"].ToString() + "), msal_NIIFcred = msal_NIIFcred-(" + arregloSaldos.Tables[0].Rows[i]["dcue_niifcred"].ToString() + ") WHERE pano_ano=" + year + " AND pmes_mes=" + month + " AND mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i]["mcue_codipuc"].ToString() + "'");
                tipoImpu = DBFunctions.SingleData("SELECT timp_codigo FROM mcuenta where mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i].ItemArray[0].ToString() + "'");
                claseCuenta = DBFunctions.SingleData("SELECT tcla_codigo FROM mcuenta where mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i]["mcue_codipuc"].ToString() + "'");
                if (claseCuenta == "N")  // nominal
                {
                    debUtility += Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_valodebi"]);
                    credUtility += Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_valocred"]);
                    debUtilityNiif += Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_niifdebi"]);
                    credUtilityNiif += Convert.ToDouble(arregloSaldos.Tables[0].Rows[i]["dcue_niifcred"]);
                }
                if (tipoImpu == "A")  // auxiliar de terceros
                {
                    sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi = maux_valodebi-(" + arregloSaldos.Tables[0].Rows[i]["dcue_valodebi"].ToString() + "), maux_valocred = maux_valocred-(" + arregloSaldos.Tables[0].Rows[i]["dcue_valocred"].ToString() + "), maux_niifdebi = maux_niifdebi-(" + arregloSaldos.Tables[0].Rows[i]["dcue_niifdebi"].ToString() + "), maux_niifcred = maux_niifcred-(" + arregloSaldos.Tables[0].Rows[i]["dcue_niifcred"].ToString() + ") WHERE mcue_codipuc='" + arregloSaldos.Tables[0].Rows[i]["mcue_codipuc"].ToString() + "' AND pano_ano=" + year + " AND pmes_mes=" + month + " AND mnit_nit='" + arregloSaldos.Tables[0].Rows[i]["mnit_nit"].ToString() + "';");
                }
            }

            //Guardar en MHISTORIALCAMBIOS DCUENTAS y MCOMPROBANTE
            DBFunctions.Request(dsElimina, IncludeSchema.NO, "Select * FROM dcuenta WHERE pdoc_codigo='" + type + "' AND mcom_numedocu=" + number + ";" +
                "Select * FROM mcomprobante where pdoc_codigo='" + type + "' AND mcom_numedocu=" + number + ";");
            strHistorialCambioL = "DEFAULT,'DCUENTA','D','";
            strHistorialCambioR = "'," +
                "'" + HttpContext.Current.User.Identity.Name.ToLower() + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            if (dsElimina.Tables[0].Rows.Count > 0)
            {
                for (int n = 0; n < dsElimina.Tables[0].Rows.Count; n++)
                {
                    strHistorialCambio = "";
                    for (int c = 0; c < dsElimina.Tables[0].Columns.Count; c++)
                        strHistorialCambio += dsElimina.Tables[0].Rows[n][c].ToString() + ",";
                    sqlStrings.Add("INSERT INTO MHISTORIAL_CAMBIOS VALUES(" + strHistorialCambioL +
                        strHistorialCambio.Substring(0, strHistorialCambio.Length - 1) + strHistorialCambioR);
                }
            }
            strHistorialCambioL = "DEFAULT,'MCOMPROBANTE','D','";
            strHistorialCambioR = "'," +
                "'" + HttpContext.Current.User.Identity.Name.ToLower() + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            strHistorialCambio = "";
            if (dsElimina.Tables[1].Rows.Count > 0)
            {
                for (int c = 0; c < dsElimina.Tables[1].Columns.Count; c++)
                    strHistorialCambio += dsElimina.Tables[1].Rows[0][c].ToString() + ",";
                sqlStrings.Add("INSERT INTO MHISTORIAL_CAMBIOS VALUES(" + strHistorialCambioL +
                    strHistorialCambio.Substring(0, strHistorialCambio.Length - 1) + strHistorialCambioR);
            }

            //  ACTUALIZA Utilidad restando la del comprobante INICIAL
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipuc FROM ccontabilidad");
            sqlStrings.Add("UPDATE msaldocuenta SET msal_valocred = msal_valocred-(" + (credUtility - debUtility).ToString() + "), msal_niifcred = msal_niifcred-(" + (credUtilityNiif - debUtilityNiif).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");
            accountUtility = DBFunctions.SingleData("SELECT mcue_codipucNOMI FROM ccontabilidad");
            sqlStrings.Add("UPDATE msaldocuenta SET msal_valoDEBI = msal_valoDEBI-(" + (credUtility - debUtility).ToString() + "), msal_niifDEBI = msal_niifDEBI-(" + (credUtilityNiif - debUtilityNiif).ToString() + ") WHERE mcue_codipuc='" + accountUtility + "' AND pano_ano=" + year + " AND pmes_mes=" + month + "");

            sqlStrings.Add("Delete FROM dcuenta WHERE pdoc_codigo='" + type + "' AND mcom_numedocu=" + number);
            sqlStrings.Add("Delete FROM mcomprobante where pdoc_codigo='" + type + "' AND mcom_numedocu=" + number);
            if (DBFunctions.Transaction(sqlStrings))
                return true;
            else
                return false;
        }

        ///////////////////////////////////////

        private bool BuscarValorArrayList(ArrayList lista, string valor)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].ToString() == valor)
                    return true;
            }
            return false;
        }
    }

}