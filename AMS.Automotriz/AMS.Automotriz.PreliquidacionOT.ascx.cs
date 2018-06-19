using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;
using AMS.Inventarios;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class PreliquidacionOT : System.Web.UI.UserControl
	{
		#region Atributos
        protected string prefijoOT, numOT, liqGarantiaFabrica, cargo;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//boton q hace la confirmacion de los datos y cancela el boton para q no se produzca. doble facturacion o etc
            Button1.Attributes.Add("Onclick", "document.getElementById('" + Button1.ClientID + "').disabled = true;" + this.Page.GetPostBackEventReference(Button1) + ";");

			prefijoOT = Request.QueryString["pref"];
			numOT = Request.QueryString["num"];
            try { liqGarantiaFabrica = DBFunctions.SingleData("SELECT COALESCE(CTAL_IVALIQUGARA,'S') FROM ctaller"); }
            catch { liqGarantiaFabrica = "S"; };
        
            if (!IsPostBack)
			{
              
                lbPrefOT.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='" + prefijoOT + "'");
                lbNumOT.Text = numOT;
                lbNitCliente.Text = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                lbNomCliente.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT COALESCE(mnit_apellido2,'') CONCAT ' ' CONCAT mnit_nombres CONCAT ' ' CONCAT COALESCE(mnit_nombre2,'') FROM mnit WHERE mnit_nit='" + lbNitCliente.Text + "'");

                String contacto = DBFunctions.SingleData("SELECT MORD_CONTACTO FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                lbNomContacto.Text = (contacto != null) ? contacto : "";
                    
                lbCrgPrnOT.Text = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo=(SELECT tcar_carGO FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + ")");
				CargarCargosRelacionados(ddlCrgsRels);
				plInfoCrg.Visible = false;
                if (!DBFunctions.RecordExist("SELECT * FROM MORDENDESCUENTO WHERE PDOC_CODIGO = '" + prefijoOT + "' AND MORD_NUMEORDE = " + numOT + " ;"))
                {
                    txtValMob.Text = DBFunctions.SingleData("SELECT Coalesce (SUM (dord_valooper),0) AS OPERACIONES FROM dordenoperacion WHERE pdoc_codigo = '" + prefijoOT + "' AND mord_numeorde =" + numOT + "");
                    txtValRep.Text = DBFunctions.SingleData(@"SELECT Coalesce (SUM (DIT.dite_valounit),0) AS REPUESTOS FROM ditems DIT, mordentransferencia MOT, mitems MIT, plineaitem PLIN 
                                                            WHERE DIT.mite_codigo = MIT.mite_codigo AND MIT.plin_codigo = PLIN.plin_codigo AND MOT.pdoc_factura = DIT.pdoc_codigo AND MOT.mfac_numero = DIT.dite_numedocu
                                                            AND MOT.pdoc_codigo = '" + prefijoOT + "' AND MOT.mord_numeorde = " + numOT + " --AND MOT.tcar_cargo = 'C'");
                    txtDes.Text = "0.00";
                    txtValDesc.Text = "0.00";
                    txtDesRep.Text = "0.00";
                    txtValDescRep.Text = "0.00";
                }
                else
                {
                    DataSet dsValDesc = new DataSet();
                    DBFunctions.Request(dsValDesc, IncludeSchema.NO, @"SELECT PDOC_CODIGO AS PREFIJO,MORD_NUMEORDE AS NUMERO,MORD_PORCDESCMOB AS PORDESC_MOB,MORD_VALODESCMOB AS VALO_DESC,MORD_PORCDESCREP AS PORCDESCREP, MORD_VALODESCREP AS VALODESREP
                                                                        FROM MORDENDESCUENTO WHERE PDOC_CODIGO = '" + prefijoOT + "'  AND MORD_NUMEORDE = " + numOT + "");
                    txtValMob.Text = DBFunctions.SingleData("SELECT Coalesce (SUM (dord_valooper),0) AS OPERACIONES FROM dordenoperacion WHERE pdoc_codigo = '" + prefijoOT + "' AND mord_numeorde =" + numOT + "");
                    //txtValRep.Text = DBFunctions.SingleData(@"SELECT Coalesce (SUM (DIT.dite_valounit),0) AS REPUESTOS FROM ditems DIT, mordentransferencia MOT, mitems MIT, plineaitem PLIN 
                    //                                      WHERE DIT.mite_codigo = MIT.mite_codigo AND MIT.plin_codigo = PLIN.plin_codigo AND MOT.pdoc_factura = DIT.pdoc_codigo AND MOT.mfac_numero = DIT.dite_numedocu
                    //                                    AND MOT.pdoc_codigo = '" + prefijoOT + "' AND MOT.mord_numeorde = " + numOT + " --AND MOT.tcar_cargo = 'C'");
                    txtValRep.Text = DBFunctions.SingleData(@"SELECT SUM(VALOR_UNITARIO * CANTIDAD)
                                                                  FROM (
                                                                    SELECT MO.PDOC_CODIGO AS PRE_ORDEN, MO.MORD_NUMEORDE AS NUM_ORDEN, 
                                                                        MT.PDOC_FACTURA AS PREF_TRANSFERENCIA, MT.MFAC_NUMERO AS 
                                                                        NUM_TRANSFERENCIA , DBXSCHEMA.EDITARREFERENCIAS(MI.MITE_CODIGO, 
                                                                        PLI.PLIN_TIPO) AS REFERENCIA , MI.MITE_NOMBRE AS DESCRIPCION, 
                                                                        DI.DITE_CANTIDAD - sum(COALESCE( DID.DITE_CANTIDAD,0)) AS CANTIDAD, 
                                                                        DI.DITE_VALOUNIT AS VALOR_UNITARIO, DI.PIVA_PORCIVA AS 
                                                                        PORCENTAJE_IVAditms , DI.DITE_PORCDESC AS DESCUENTO, DI.PIVA_PORCIVA 
                                                                        AS PORCENTAJE_IVAmitms, mt.tcar_cargo, MI.TORI_CODIGO AS ORIGEN
                                                                      FROM DBXSCHEMA.MORDEN MO, DBXSCHEMA.MORDENTRANSFERENCIA MT, 
                                                                        DBXSCHEMA.MITEMS MI, DBXSCHEMA.PLINEAITEM PLI, 
                                                                        DBXSCHEMA.MFACTURACLIENTE MF, DBXSCHEMA.DITEMS DI
                                                                        LEFT JOIN DBXSCHEMA.DITEMS DID
                                                                        ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE
                                                                        AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE
                                                                        AND DI.MITE_CODIGO = DID.MITE_CODIGO
                                                                        AND DID.TMOV_TIPOMOVI = 81
                                                                      WHERE MT.PDOC_CODIGO = MO.PDOC_CODIGO
                                                                        AND MT.MORD_NUMEORDE = MO.MORD_NUMEORDE
                                                                        AND DI.MITE_CODIGO = MI.MITE_CODIGO
                                                                        AND MI.PLIN_CODIGO = PLI.PLIN_CODIGO
                                                                        AND DI.PDOC_CODIGO = MT.PDOC_FACTURA
                                                                        AND DI.DITE_NUMEDOCU = MT.MFAC_NUMERO
                                                                        AND MT.PDOC_FACTURA = MF.PDOC_CODIGO
                                                                        AND MT.MFAC_NUMERO = MF.MFAC_NUMEDOCU
                                                                      group by MO.PDOC_CODIGO, MO.MORD_NUMEORDE,MI.MITE_CODIGO, PLI.PLIN_TIPO,
                                                                          DI.DITE_PORCDESC,MI.TORI_CODIGO,mt.tcar_cargo,DI.PIVA_PORCIVA, 
                                                                          DI.DITE_PORCDESC, DI.DITE_VALOUNIT,DI.DITE_CANTIDAD, MI.MITE_NOMBRE,
                                                                          MT.MFAC_NUMERO,MT.PDOC_FACTURA ) AS A
                                                                  WHERE A.CANTIDAD > 0 AND PRE_ORDEN = '" + prefijoOT + @"' AND NUM_ORDEN = " + numOT + ";");


                    txtDes.Text = dsValDesc.Tables[0].Rows[0][2].ToString();
                    txtValDesc.Text = dsValDesc.Tables[0].Rows[0][3].ToString();
                    txtDesRep.Text = dsValDesc.Tables[0].Rows[0][4].ToString(); 
                    txtValDescRep.Text = dsValDesc.Tables[0].Rows[0][5].ToString();
                }
            }

            cargo = ddlCrgsRels.SelectedValue;
		}
		
        protected void MostrarInfoOTCrg(Object Sender, EventArgs e)
		{
			int i;
			//Realizamos la consulta de las operaciones relacionadas a esta orden de trabajo que tengan el cargo seleccionado en el dropdownlist
			DataSet dsInfoCrg = new DataSet();
            DBFunctions.Request(dsInfoCrg, IncludeSchema.NO, 
                @"SELECT DOR.ptem_operacion AS CODIGO, PTE.ptem_descripcion AS DESCRIPCION, PVE.pven_nombre AS MECANICO, DOR.dord_valooper AS VALOROPERACION 
                    FROM dordenoperacion DOR, ptempario PTE, pvendedor PVE 
                    WHERE DOR.ptem_operacion = PTE.ptem_operacion AND DOR.pven_codigo = PVE.pven_codigo 
                     AND DOR.tcar_cargo = '" + ddlCrgsRels.SelectedValue + @"' AND DOR.pdoc_codigo = '" + prefijoOT + @"' AND DOR.mord_numeorde = " + numOT + @"; 
                  
                  SELECT * FROM (SELECT DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo),MIT.mite_nombre,DIT.dite_valounit, dIT.piva_porciva,  
                   DIT.dite_cantidad - COALESCE(DID.DITE_CANTIDAD,0) AS CANTIDAD,DIT.pdoc_codigo CONCAT '-' CONCAT CAST(DIT.dite_numedocu AS CHARACTER(15)), DIT.mite_codigo,DIT.dite_porcdesc  
                   FROM  mitems MIT, plineaitem PLIN, mordentransferencia MOT, ditems DIT  
                   LEFT JOIN DITEMS DID ON DIT.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DIT.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DIT.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81  
                   WHERE DIT.mite_codigo=MIT.mite_codigo AND MIT.plin_codigo = PLIN.plin_codigo AND DIT.pdoc_codigo = MOT.pdoc_factura  
                   AND DIT.dite_numedocu = MOT.mfac_numero AND DIT.dite_cantidad > 0 AND MOT.pdoc_codigo = '" + prefijoOT + @"' AND MOT.mord_numeorde = " + numOT + @" AND MOT.tcar_cargo = '" + ddlCrgsRels.SelectedValue + @"'
                   AND DIT.TMOV_TIPOMOVI = 80) WHERE CANTIDAD > 0 ;");
			//																					0										1				2				3					4										5													6				7			
			dgInfoOper.DataSource = dsInfoCrg.Tables[0];
			dgInfoOper.DataBind();
            DatasToControls.JustificacionGrilla(dgInfoOper, dsInfoCrg.Tables[0]);
			////////////////////////////////////////////
			DataTable dtRepuestos = new DataTable();
			dtRepuestos = PrepararDtRepuestos();
            for (i = 0; i < dsInfoCrg.Tables[1].Rows.Count; i++)
			{
				//Cambio realizado el 25 de noviembre de 2006
				//Se revisa si aun existen repuestos para esta transferencia si la cantidad es mayor a cero se agrega al DataTable si no, pues no
                double cantidad = Items.ConsultaRelacionItemsTransferencias(dsInfoCrg.Tables[1].Rows[i][6].ToString(), (dsInfoCrg.Tables[1].Rows[i][5].ToString().Split('-'))[0],
                                                                          Convert.ToInt64((dsInfoCrg.Tables[1].Rows[i][5].ToString().Split('-'))[1].Trim()), 80, 81);
                string cargoSeleccionado = ddlCrgsRels.SelectedValue.ToString();
                if (cantidad > 0)
				{
                    double valor = 0;
					DataRow fila = dtRepuestos.NewRow();
					fila[0] = dsInfoCrg.Tables[1].Rows[i][0].ToString();
					fila[1] = dsInfoCrg.Tables[1].Rows[i][1].ToString();
					fila[2] = Convert.ToDouble(dsInfoCrg.Tables[1].Rows[i][2]);
                    if (cargoSeleccionado == "G" && liqGarantiaFabrica == "N")
                         fila[3] = 0;
                    else fila[3] = Convert.ToDouble(dsInfoCrg.Tables[1].Rows[i][3]);
				  //fila[4] = Convert.ToDouble(dsInfoCrg.Tables[1].Rows[i][4]);
					fila[4] = cantidad;
                    valor = Convert.ToDouble(dsInfoCrg.Tables[1].Rows[i][2]) * (1 - (Convert.ToDouble(dsInfoCrg.Tables[1].Rows[i][7]) / 100));
                    fila[5] = (valor + (valor * (Convert.ToDouble(fila[3]) / 100))) * cantidad;
					fila[6] = dsInfoCrg.Tables[1].Rows[i][5].ToString();
					dtRepuestos.Rows.Add(fila);
				}
			}
			dgInfoItems.DataSource = dtRepuestos;
			dgInfoItems.DataBind();
            DatasToControls.JustificacionGrilla(dgInfoItems, dtRepuestos);
			plInfoCrg.Visible = true;
		}
		
        protected void PreliquidarOT(Object Sender, EventArgs e)
		{
            ArrayList sqlRefs = new ArrayList();
            try
            {
                if (!DBFunctions.RecordExist("SELECT * FROM MORDENDESCUENTO WHERE PDOC_CODIGO = '" + prefijoOT + "' AND MORD_NUMEORDE = " + numOT + " ;"))
                {
                    sqlRefs.Add(@"INSERT INTO MORDENDESCUENTO(PDOC_CODIGO, MORD_NUMEORDE, TCAR_CARGO, MORD_PORCDESCMOB, MORD_VALODESCMOB, MORD_PORCDESCREP, MORD_VALODESCREP)
                         VALUES('" + prefijoOT + "', " + numOT + ",  '" + ddlCrgsRels.SelectedValue + "',  " + txtDes.Text + ",  " + txtValDesc.Text + ", " + txtDesRep.Text + ",  " + txtValDescRep.Text + ")");
                }
                else
                {
                    sqlRefs.Add(@"UPDATE MORDENDESCUENTO
                               SET MORD_PORCDESCMOB = " + txtDes.Text + ", MORD_VALODESCMOB = " + txtValDesc.Text + ",  MORD_PORCDESCREP = " + txtDesRep.Text + ",  MORD_VALODESCREP = " + txtValDescRep.Text + " WHERE PDOC_CODIGO = '" + prefijoOT + "' AND MORD_NUMEORDE = " + numOT + "");
                }
                if (DBFunctions.Transaction(sqlRefs))
                {
                    Utils.MostrarAlerta(Response, "Descuento Modificado Satisfactoriamente!");
                }
            }
            catch { };

            int contCumplidas = 0, contOperaciones = 0;
			DataSet cantidadOperacionesOrden = new DataSet();
			DataSet cantidadOperacionesCumplidas = new DataSet();
            DBFunctions.Request(cantidadOperacionesOrden, IncludeSchema.NO, "SELECT MOR.mord_numeorde,COUNT(DOR.mord_numeorde) FROM DBXSCHEMA.morden MOR, DBXSCHEMA.dordenoperacion DOR WHERE MOR.pdoc_codigo=DOR.pdoc_codigo AND MOR.mord_numeorde = DOR.mord_numeorde AND MOR.pdoc_codigo='" + prefijoOT + "' AND MOR.mord_numeorde=" + numOT + " AND MOR.test_estado = 'A' GROUP BY MOR.mord_numeorde");
            DBFunctions.Request(cantidadOperacionesCumplidas, IncludeSchema.NO, "SELECT MOR.mord_numeorde,COUNT(DOR.mord_numeorde) FROM DBXSCHEMA.morden MOR, DBXSCHEMA.dordenoperacion DOR WHERE MOR.pdoc_codigo=DOR.pdoc_codigo AND MOR.mord_numeorde = DOR.mord_numeorde AND MOR.pdoc_codigo='" + prefijoOT + "' AND MOR.mord_numeorde=" + numOT + " AND DOR.test_estado = 'C' GROUP BY MOR.mord_numeorde");
            
            if (cantidadOperacionesOrden.Tables[0].Rows.Count == 0)
                contOperaciones = 0;
            else
                contOperaciones = Convert.ToInt32(cantidadOperacionesOrden.Tables[0].Rows[0][1]);
			       
            
            if (cantidadOperacionesCumplidas.Tables[0].Rows.Count == 0)
                contCumplidas = 0;
			else
                contCumplidas = Convert.ToInt32(cantidadOperacionesCumplidas.Tables[0].Rows[0][1]);

            //string tieneTabs = ddlCrgsRels.Items.Count > 1 ? "&tabs=N" : "&tabs=S";
            string respuesta = DBFunctions.SingleData(@"SELECT COUNT(*) FROM MORDEN WHERE PDOC_CODIGO = '" + prefijoOT + "' AND MORD_NUMEORDE =  '" + numOT + "' AND PDOC_CODIGO||MORD_NUMEORDE IN (SELECT PDOC_PREFORDETRAB||MORD_NUMEORDE  FROM MFACTURACLIENTETALLER WHERE PDOC_PREFORDETRAB = '" + prefijoOT + "' AND MORD_NUMEORDE = '" + numOT + "')");
            string tieneTabs = Convert.ToInt32(respuesta) > 0 ? "&tabs=0" : "&tabs=1";

            if (contOperaciones == contCumplidas)
            {
                if (DBFunctions.NonQuery("UPDATE morden SET mord_estaliqu='P' WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "") == 1)
				{
                    Response.Redirect(indexPage + "?process=Automotriz.VistaImpresion&prefOT=" + prefijoOT + "&numOT=" + numOT + "&cargo=" + cargo + "&dest=2&tipVis=prel&ImpOrd=S" + tieneTabs);
					//Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden");
				}
				else
				{
                    Utils.MostrarAlerta(Response, "Error Al Generar Cambio de Estado.\\nRevise Por Favor!");
                    lb.Text += "<br>" + DBFunctions.exceptions;
				}
			}
			else
			{
                Response.Redirect(indexPage + "?process=Automotriz.VistaImpresion&prefOT=" + prefijoOT + "&numOT=" + numOT + "&cargo=" + cargo + "&dest=2&tipVis=prel&msg=1&ImpOrd=S" + tieneTabs);
			}
		}
		
        protected void Volver(Object Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden");
		}
		#endregion
		
		#region Metodos
		protected DataTable PrepararDtRepuestos()
		{
			DataTable dtRepuestos = new DataTable();
            dtRepuestos.Columns.Add(new DataColumn("CODIGO", System.Type.GetType("System.String")));//0
            dtRepuestos.Columns.Add(new DataColumn("DESCRIPCION", System.Type.GetType("System.String")));//1
            dtRepuestos.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.Double")));//2
            dtRepuestos.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.Double")));//3
            dtRepuestos.Columns.Add(new DataColumn("CANTIDAD", System.Type.GetType("System.Double")));//4
            dtRepuestos.Columns.Add(new DataColumn("TOTAL", System.Type.GetType("System.Double")));//5
            dtRepuestos.Columns.Add(new DataColumn("TRANSFERENCIA", System.Type.GetType("System.String")));//6
			return dtRepuestos;
		}
		
		protected void CargarCargosRelacionados(DropDownList ddl)
		{
			int i;
			ddl.Items.Clear();
			//Traemos los cargos relacionados con las operaciones y con los repuestos

            string sql1 = String.Format(
             @"SELECT DISTINCT DOR.tcar_cargo AS CODIGO, TCA.tcar_nombre AS NOMBRE  
             FROM dordenoperacion DOR, tcargoorden TCA   
             WHERE DOR.pdoc_codigo = '{0}'   
             AND   DOR.mord_numeorde = {1} 
             AND   DOR.tcar_cargo = TCA.tcar_cargo  
             AND   DOR.tcar_cargo <> 'X'  
             AND   DOR.tcar_cargo NOT IN (SELECT TCAR_CARGO  
                                          FROM Mfacturaclientetaller mft  
                                          WHERE DOR.PDOC_CODIGO = MFT.PDOC_PREFORDETRAB  
                                          AND   DOR.MORD_numeorde = mft.mord_numeorde) ORDER BY 1;"
             , prefijoOT
             , numOT);

            string sql2 = String.Format(
           /*
             "SELECT DISTINCT MOT.tcar_cargo AS CODIGO, \n" +
             "       TCA.tcar_nombre AS NOMBRE \n" +
             "FROM mordentransferencia MOT \n" +
             "				inner join ditems d1  \n" +
             "						left join ditems d2 on d1.pdoc_codigo=d2.dite_prefdocurefe and d1.dite_numedocu=d2.dite_numedocurefe \n" +
             "				on d1.pdoc_codigo=mot.pdoc_factura and d1.dite_numedocu=mot.mfac_numero \n" +
             "	   		left join tcargoorden TCA on MOT.tcar_cargo = TCA.tcar_cargo \n" +
             "WHERE MOT.pdoc_codigo = '{0}'  \n" +
             "AND   MOT.mord_numeorde = {1} \n" +
             "AND		d1.tmov_tipomovi=80 \n" +
             "AND   d2.pdoc_codigo is null"
             , prefijoOT
             , numOT);
           */

            @"select DISTINCT CODIGO,NOMBRE from (  
             SELECT  MOT.tcar_cargo AS CODIGO, TCA.tcar_nombre AS NOMBRE, SUM(d1.dite_cantidad - coalesce(d2.dite_cantidad,0)  ) as cantidad  
             FROM mordentransferencia MOT  
            	LEFT join ditems d1   on  d1.pdoc_codigo = mot.pdoc_factura and d1.dite_numedocu = mot.mfac_numero AND d1.tmov_tipomovi  = 80  
               	left join ditems d2 on  d1.pdoc_codigo = d2.dite_prefdocurefe  
            			   	 	  	              and d1.dite_numedocu = d2.dite_numedocurefe  
            								              and d1.mite_codigo = d2.mite_codigo  
                 left join tcargoorden TCA on MOT.tcar_cargo = TCA.tcar_cargo  
             WHERE MOT.pdoc_codigo   =  '{0}' 
              AND  MOT.mord_numeorde =   {1}   
              AND  MOT.tcar_cargo NOT IN (SELECT TCAR_CARGO  
                                           FROM Mfacturaclientetaller mft  
                                           WHERE MOT.PDOC_CODIGO = MFT.PDOC_PREFORDETRAB  
                                           AND   MOT.MORD_numeorde = mft.mord_numeorde)  
               GROUP BY MOT.tcar_cargo, TCA.tcar_nombre) as a where cantidad <> 0  "
             , prefijoOT 
             , numOT);

			DataSet dsCargos = new DataSet();
            DBFunctions.Request(dsCargos, IncludeSchema.NO, sql1 + sql2);
            for (i = 0; i < dsCargos.Tables[0].Rows.Count; i++)
                ddl.Items.Add(new ListItem(dsCargos.Tables[0].Rows[i][1].ToString(), dsCargos.Tables[0].Rows[i][0].ToString()));
            for (i = 0; i < dsCargos.Tables[1].Rows.Count; i++)
			{
                if ((dsCargos.Tables[0].Select("CODIGO='" + dsCargos.Tables[1].Rows[i][0].ToString() + "'")).Length == 0)
                    ddl.Items.Add(new ListItem(dsCargos.Tables[1].Rows[i][1].ToString(), dsCargos.Tables[1].Rows[i][0].ToString()));
			}
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
		}
		#endregion
	}
}
