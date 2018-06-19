using System;
using AMS.DB;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Tools;

namespace AMS.Contabilidad
{
	/// <summary>
	///		Descripción breve de AMS_Contabilidad_CierreContable.
	/// </summary>
	public partial class CierreContable : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void BtnCierre_Click(object sender, System.EventArgs e)
		{
		
		string mesCierre  = DBFunctions.SingleData("Select pmes_mes from CCONTABILIDAD");
		string anoCierre  = DBFunctions.SingleData("Select pano_ano from CCONTABILIDAD");
        int    iMesCierre = Convert.ToInt32(mesCierre);
        int    iAnoCierre = Convert.ToInt32(anoCierre);
        string nitEmpresa = DBFunctions.SingleData("Select mnit_nit from CEMPRESA");
        string nitDian    = DBFunctions.SingleData("Select mnit_nitDIAN from CCONTABILIDAD");
        string nitMunicipio = DBFunctions.SingleData("Select mnit_nitMUNICIPIO from CCONTABILIDAD");
        string comprobanteCierre = DBFunctions.SingleData("Select PDOC_CODIDOCUCIERRE from CCONTABILIDAD c, pdocumento p where c.PDOC_CODIDOCUCIERRE = p.pdoc_codigo");
        string cierreMesTransporte = "delete from mauxiliarCENTROCOSTOcuenta m where m.pano_ano = " + iAnoCierre + " and m.pmes_mes = " + iMesCierre + " ;  " + 
                                    " insert into mauxiliarCENTROCOSTOcuenta " +
                                    " select pano_ano, pmes_mes, D.MCUE_CODIPUC, pcen_codigo, sum(dcue_valoDEBI), SUM(dcue_valoCRED) " +
                                    "  from dcuenta d, mcomprobante m, MCUENTA MC " +
                                    "  where d.pdoc_codigo = m.pdoc_codigo and d.mcom_numedocu = m.mcom_numedocu " +
                                    "   and m.pano_ano = " + iAnoCierre + " and m.pmes_mes = " + iMesCierre + " " +
                                    "   AND D.MCUE_CODIPUC = MC.MCUE_CODIPUC AND MC.TIMP_CODIGO = 'A' " +
                                    "  group by pano_ano, pmes_mes, D.MCUE_CODIPUC, PCEN_CODIGO " +
                                    "  order by pano_ano, pmes_mes, D.MCUE_CODIPUC, PCEN_CODIGO ; ";
        string cierreAnoTransporte = "delete from mauxiliarCENTROCOSTOcuenta m where m.pano_ano = " + iAnoCierre + "+1 and m.pmes_mes = 0 ;  " +
                                    " insert into mauxiliarCENTROCOSTOcuenta " +
                                    " select pano_ano+1, 0, MCUE_CODIPUC, pcen_codigo, sum(maux_valoDEBI), SUM(maux_valoCRED) " +
                                    "  from mauxiliarCENTROCOSTOcuenta m " +
                                    "  where pano_ano = " + iAnoCierre + " and mcue_codipuc < '4' " +
                                    "  group by pano_ano, m.MCUE_CODIPUC, PCEN_CODIGO " +
                                    "  order by pano_ano, m.MCUE_CODIPUC, PCEN_CODIGO ; ";

        if (nitEmpresa != "" && nitDian != "" && nitMunicipio != "" && comprobanteCierre != "")
		{
			if(mesCierre==TxBMes.Text && anoCierre==TxBAno.Text)
			{
				if(cruceCuenta(iMesCierre, iAnoCierre, nitDian, nitMunicipio, comprobanteCierre))
				{
                    if (nitEmpresa == "800174156") //Cootransbol...cambiar por tipo empresa transporte || Cierre de Transportes
                    {
                        if (DBFunctions.NonQuery(cierreMesTransporte) > 0)
                        {
                           Utils.MostrarAlerta(Response, "!!Cierre Contable Transportes Exitoso !!");
                        }
                        else
                        {
                            Utils.MostrarAlerta(Response, "!!Cierre Contable Transportes No Exitoso, Cierre Cancelado !!");
                            return;
                        }
                    }

                    if (mesCierre != "12")
                    {
                        int intMesCierre = Convert.ToInt32(mesCierre) + 1;
                        if (DBFunctions.NonQuery("UPDATE CCONTABILIDAD SET PMES_MES='" + intMesCierre + "' WHERE PMES_MES='" + mesCierre + "'") > 0)
                        {
                            Utils.MostrarAlerta(Response, "!!Cierre Contable Exitoso !!");
                        }
                    }
                    else
                    {
                        try
                        {
                            DBFunctions.NonQuery("CALL DBXSCHEMA.ACTUALIZACION_CIERRE_ANO(" + TxBAno.Text + ",'" + nitDian + "','" + nitMunicipio + "','" + comprobanteCierre + "')");
                            Utils.MostrarAlerta(Response, "!! Cierre Contable Anual Exitoso !!");
                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "!! Cierre Contable NO RELAIZADO, proceso FALLO !!!!!");
                            return;
                        }

                        if (nitEmpresa == "800174156") //Cootransbol...cambiar por tipo empresa transporte || Cierre de Transportes
                        {
                            if (DBFunctions.NonQuery(cierreAnoTransporte) > 0)
                            {
                                Utils.MostrarAlerta(Response, "!! Cierre Contable Anual de Transportes Exitoso !!");
                            }
                            else
                            {
                                Utils.MostrarAlerta(Response, "!! Cierre Contable Anual de Transportes No Exitoso, Cierre Cancelado !!");
                                return;
                            }
                        }
                    }
                }
                else Utils.MostrarAlerta(Response, "No se Puede hacer el cierre, hay diferencia entre el libro diario y el mayor.");
			}
            else Utils.MostrarAlerta(Response, "Mes o Año Selecionados diferentes a la Fecha de Cierre");
		}
        else Utils.MostrarAlerta(Response, "NIT de la EMPRESA o de la DIAN o del MUNICIPIO o el Comprobante de Cierre NO han sido correctamente PARAMETRIZADOS ... Cierre Cancelado !!!");
        }


        private bool cruceCuenta(int mes, int anio, string nitDian, string nitMunicipio, string comprobanteCierre)
		{	
			DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            string sql1 = "SELECT * FROM (select m.pmes_mes, d.mcue_codipuc, sum(d.dcue_valodebi), sum(d.dcue_valocred),ms.msal_valodebi, ms.msal_valocred, sum(d.dcue_valodebi)-ms.msal_valodebi AS DIFDEB,sum(d.dcue_valocred) - ms.msal_valocred as DIFCRED from dcuenta d, mcomprobante m, msaldocuenta ms" +
                        "where m.pano_ano = "+anio+" and m.pmes_mes = "+mes+"" +
                        "and m.pdoc_codigo = d.pdoc_codigo and m.mcom_numedocu = d.mcom_numedocu"+
                        "and d.mcue_codipuc = ms.mcue_codipuc and ms.pano_ano = m.pano_ano and ms.pmes_mes = m.pmes_mes" +
                        "group by m.pmes_mes, d.mcue_codipuc,  ms.msal_valodebi, ms.msal_valocred) AS A WHERE DIFDEB <> 0 OR DIFCRED <> 0";
            string sql2 = "SELECT * FROM (select M.MCUE_CODIPUC,    MS.PMES_MES, MSAL_VALODEBI - MSAL_VALOCRED AS BALANCE," +
                        "SUM (MAUX_VALODEBI - MAUX_VALOCRED) AS AUXILIAR,(MSAL_VALODEBI - MSAL_VALOCRED) - SUM(MAUX_VALODEBI - MAUX_VALOCRED) AS DIFER"+
                        "FROM MCUENTA M, MSALDOCUENTA MS left join MAUXILIARCUENTA MA on Ms.MCUE_CODIPUC = MA.MCUE_CODIPUC  AND MS.PANO_ANO = MA.PANO_ANO AND MS.PMES_MES = MA.PMES_MES"+
                        "WHERE M.MCUE_CODIPUC = MS.MCUE_CODIPUC AND M.TIMP_CODIGO = 'A' AND MS.PANO_ANO = "+anio+" AND MS.PMES_MES = "+mes+" GROUP BY M.MCUE_CODIPUC, MS.PMES_MES, MSAL_VALODEBI, MSAL_VALOCRED"+
                        "ORDER BY M.MCUE_CODIPUC) AS B WHERE DIFER <> 0;";
            DBFunctions.Request(ds1, IncludeSchema.NO, sql1);
            DBFunctions.Request(ds2, IncludeSchema.NO, sql2);
            if (ds1.Tables.Count == 0 && ds2.Tables.Count == 0)
            {
                if (nitDian.Length > 0 && nitMunicipio.Length > 0 && comprobanteCierre.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "Nit DIAN o Nit MUNICIPIO o Comprobante de Cierre NO DEFINIDO, No se pudo REALIZAR el cierre.");
                        return false;
                    }
            }
            else
            {
                Utils.MostrarAlerta(Response, "Hay DIFERCIAS en los Saldos y-o Auxiliares de Cuentas...!! No se pudo REALIZAR el cierre.");
                return false;
            }
		}

	}
}
