using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using Ajax.JSON;
using Ajax;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class UnificarTemparioOperaciones : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

            if (!IsPostBack)
            {
                btnUnificar.Visible = false;
                
                if (Request.QueryString["OK"] != null)
                {
                    Utils.MostrarAlerta(Response, "La unificación de operaciones de tempario se ha completado correctamente!");
                }

            }
		}

        protected void btnVer_Click(object sender, System.EventArgs e)
        {
            if(txtOperacion.Text.Equals(""))
            {
                Utils.MostrarAlerta(Response, "Debe ingresar algún valor en el campo de descripción!");
            }
            else
            {
                DataSet operacionesTempario = new DataSet();
                DBFunctions.Request(operacionesTempario, IncludeSchema.NO,
                    @"SELECT PT.ptem_operacion, ptem_descripcion, COUNT(DO.PTEM_OPERACION) AS VECES  
                    FROM dbxschema.ptempario PT  
                    LEFT JOIN DORDENOPERACION DO ON PT.PTEM_OPERACION = DO.PTEM_OPERACION   
                    WHERE ptem_descripcion LIKE '%" + txtOperacion.Text + "%' GROUP BY PT.ptem_operacion, ptem_descripcion ORDER BY ptem_descripcion; ");

                //"SELECT ptem_operacion, ptem_descripcion FROM dbxschema.ptempario " +
                //"WHERE ptem_descripcion LIKE '%" + txtOperacion.Text + "%' ORDER BY ptem_descripcion;");

                if (operacionesTempario.Tables[0].Rows.Count > 0)
                {
                    operaciones.DataSource = operacionesTempario;
                    operaciones.DataBind();
                    operaciones.Visible = true;
                    btnUnificar.Visible = true;
                }
                else
                {
                    operaciones.Visible = false;
                    Utils.MostrarAlerta(Response, "No se encontraron coincidencias que contengan la descripción dada.");
                }
                
            }
        }

        protected void btnUnificar_Click(object sender, EventArgs e)
        {
            if (validarSeleccion())
            {
                RadioButton rb = new RadioButton();
                CheckBox ch = new CheckBox();
                string codCorrecto = "";
                ArrayList sqlStrings = new ArrayList();

                foreach (DataGridItem i in operaciones.Items)
                {
                    rb = (RadioButton)i.FindControl("rbsira");
                    if (rb.Checked == true)
                    {
                        DataBoundLiteralControl tx = (DataBoundLiteralControl)i.Cells[0].Controls[0];
                        codCorrecto = tx.Text.Trim();
                    }
                }

                foreach (DataGridItem i in operaciones.Items)
                {
                    // Se puede mejorar la velocidad creando una tabla temporal cargandole los codigo errados y reemplazando la sentencias = '" + codErroneo + "' por in (select ptem_operacion from tablatemporal);
                    string codErroneo = "";

                    ch = (CheckBox)i.FindControl("chkErrada");
                    if (ch.Checked == true)
                    {
                        DataBoundLiteralControl tx = (DataBoundLiteralControl)i.Cells[0].Controls[0];
                        codErroneo = tx.Text.Trim();
                        //string operacion = DBFunctions.SingleData("");
                        //string opAnulacion = DBFunctions.SingleData("");
                        
                        sqlStrings.Add(
                            "MERGE INTO dbxschema.dordenoperacion ar " +
                            "USING (SELECT PDOC_CODIGO, MORD_NUMEORDE, PTEM_OPERACION FROM dbxschema.dordenoperacion) ac " +
                            "ON (ar.PDOC_CODIGO = ac.PDOC_CODIGO AND ar.MORD_NUMEORDE = ac.MORD_NUMEORDE and ac.ptem_operacion = '" + codCorrecto + "') " +
                            "WHEN MATCHED and ar.ptem_operacion = '" + codErroneo + "' " + 
                            "THEN delete; "
                            );
                        sqlStrings.Add("UPDATE dbxschema.dordenoperacion SET ptem_operacion = '" + codCorrecto + "' WHERE PTEM_OPERACION='" + codErroneo + "';");

                        sqlStrings.Add(
                           "MERGE INTO dbxschema.dordenoperacionANULACION ar " +
                           "USING (SELECT PDOC_CODIGO, MORD_NUMEORDE, PTEM_OPERACION FROM dbxschema.dordenoperacionANULACION) ac " +
                           "ON (ar.PDOC_CODIGO = ac.PDOC_CODIGO AND ar.MORD_NUMEORDE = ac.MORD_NUMEORDE and ac.ptem_operacion = '" + codCorrecto + "' and ar.ptem_operacion = '" + codErroneo + "') " +
                           "WHEN MATCHED and ar.ptem_operacion = '" + codErroneo + "' " +
                           "THEN delete; "
                           );
                        sqlStrings.Add(
                            "UPDATE dbxschema.dordenoperacionanulacion SET ptem_operacion = '" + codCorrecto + "' WHERE ptem_operacion='" + codErroneo + "';");

                        sqlStrings.Add(
                            "MERGE INTO dbxschema.dobservacionesOT ar " +
                            "USING (SELECT PDOC_CODIGO, MORD_NUMEORDE, PTEM_OPERACION FROM dbxschema.dobservacionesOT) ac " +
                            "ON (ar.PDOC_CODIGO = ac.PDOC_CODIGO AND ar.MORD_NUMEORDE = ac.MORD_NUMEORDE and ac.ptem_operacion = '" + codCorrecto + "') " +
                            "WHEN MATCHED and ar.ptem_operacion = '" + codErroneo + "' " +
                            "THEN delete; "
                            );
                        sqlStrings.Add("UPDATE dbxschema.dobservacionesOT SET ptem_operacion = '" + codCorrecto + "' WHERE PTEM_OPERACION='" + codErroneo + "';");

                        sqlStrings.Add(
                          "MERGE INTO dbxschema.ptiempotaller ar " +
                          "USING (SELECT ptie_grupcata, ptie_tempario FROM dbxschema.ptiempotaller) ac " +
                          "ON (ar.ptie_grupcata = ac.ptie_grupcata and ac.ptie_tempario = '" + codCorrecto + "') " +
                          "WHEN MATCHED and ar.ptie_tempario = '" + codErroneo + "' " +
                          "THEN delete; "
                          );
                        sqlStrings.Add(
                            "UPDATE dbxschema.ptiempotaller SET ptie_tempario = '" + codCorrecto + "' WHERE ptie_tempario='" + codErroneo + "';");

                        sqlStrings.Add(
                          "MERGE INTO dbxschema.mkitoperacion ar " +
                          "USING (SELECT mkit_codikitoper, mkit_operacion FROM dbxschema.mkitoperacion) ac " +
                          "ON (ar.mkit_codikitoper = ac.mkit_codikitoper and ac.mkit_operacion = '" + codCorrecto + "') " +
                          "WHEN MATCHED and ar.mkit_operacion = '" + codErroneo + "' " +
                          "THEN delete; "
                          );
                        sqlStrings.Add(
                            "UPDATE dbxschema.mkitoperacion SET mkit_operacion = '" + codCorrecto + "' WHERE mkit_operacion='" + codErroneo + "';");
                       
                        sqlStrings.Add(
                            "DELETE FROM dbxschema.ptempario WHERE ptem_operacion = '" + codErroneo + "';");
                        sqlStrings.Add("COMMIT;");
                    }
                }
                if (DBFunctions.Transaction(sqlStrings))
                {
                    Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Automotriz.UnificarTemparioOperaciones&OK=1");
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Error durante la unificación! Revisar registros.");
                }
              }
            else
            {
                Utils.MostrarAlerta(Response, "Para realizar la unificación debe seleccionar una operación correcta y una o más operaciones erradas!");
            }

        }

         protected Boolean validarSeleccion()
         {
             RadioButton rb = new RadioButton();
             CheckBox ch = new CheckBox();
             Boolean rbOK = false;
             Boolean chOK = false;
             Boolean validar = false;

             foreach (DataGridItem i in operaciones.Items)
             {
                 rb = (RadioButton)i.FindControl("rbsira");
                 ch = (CheckBox)i.FindControl("chkErrada");

                 if(rb.Checked == true)
                     rbOK = true;

                 if (ch.Checked == true)
                     chOK = true;

                 if (rbOK == true && chOK == true)
                 {
                     validar = true;
                     break;
                 }
             }

             return validar;
         }


	}
}