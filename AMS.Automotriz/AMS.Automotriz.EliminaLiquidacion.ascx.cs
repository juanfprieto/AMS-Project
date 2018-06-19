using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;
using eWorld.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Xml;



namespace AMS.Automotriz
{
	public partial class EliminaLiquidacion : System.Web.UI.UserControl
	{

        protected DatasToControls bind;
        protected DataTable dtInserts;
        protected DataSet ds;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string fechaSel;

		protected void Page_Load(object sender, EventArgs e)
		{
            ddltipoComi.Enabled = true;
            txtFecha.Enabled = true;
            if (Request["fecha"] != null) txtFecha.Text = Request["fecha"];
            if (Request["prhecho"] == "true" && !IsPostBack)
            {
                Utils.MostrarAlerta(Response, "Se han eliminado los registros satisfactoriamente");
            }
            dtInserts = new DataTable();
		}

        //Carga las liquidaciones que se van a eliminar..
        protected void mostrarLiquidacion(object sender, EventArgs e) 
        {

            if (txtFecha.Text == "" || txtFecha.Text == "dd/mm/aaaa")
            {
                Utils.MostrarAlerta(Response, "Selecione una fecha!");
                return;
            }
            if (ddltipoComi.SelectedValue == "0")
            {
                Utils.MostrarAlerta(Response, "Seleccione el tipo de Comisión!");
                return;

            }
            fldOrdenes.Visible = true;
            ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.YES, "SELECT * FROM MORDENCOMISION where MORD_FECHLIQU = '" + txtFecha.Text + "' AND MORD_TIPOCOMI = '" + ddltipoComi.SelectedValue.ToString() + "';");
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltipoComi.Enabled = false;
                txtFecha.Enabled = false;
                dtInserts = ds.Tables[0];
                dtInserts.Columns[0].ColumnName = "Orden Trabajo";
                dtInserts.Columns[1].ColumnName = "Número de órden";
                dtInserts.Columns[2].ColumnName = "Fecha liquidación";
                dtInserts.Columns[3].ColumnName = "Tipo Comisión";

                ViewState["dtInserts"] = dtInserts;
                dgOrden.DataSource = ViewState["dtInserts"];
                dgOrden.DataBind();
            }
            else
            {
                Utils.MostrarAlerta(Response, "No se encontró ningún registro..!");
            }
        }


        protected void ChangeDate(Object Sender, EventArgs E)
        {
            txtFecha.Text = fecha.SelectedDate.Date.ToString("yyyy-MM-dd");
            fechaSel = txtFecha.Text;
        }

        //Cancelación de la eliminación..
        protected void cancelarAccion(object Sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=Automotriz.EliminaLiquidacion&fecha=" + txtFecha.Text + "&prhecho=false");
        }

        //Proceso de eliminación de liquidaciónes..
        protected void EliminaLiquida(object sender, EventArgs e)
        {
                DBFunctions.NonQuery("delete from MORDENCOMISION where mord_fechliqu = '" + txtFecha.Text + "' AND MORD_TIPOCOMI = '" + ddltipoComi.SelectedValue + "'");

                //if (DBFunctions.SingleData("select pdoc_codigo from mordencomision where mord_tipocomi = '" + ddltipoComi.SelectedValue + "'") == "")
                if (DBFunctions.SingleData("select pdoc_codigo from mordencomision where mord_tipocomi = '" + ddltipoComi.SelectedValue + "' AND MORD_FECHLIQU = '" + txtFecha.Text + "'" ) == "")
                {
                    Response.Redirect(indexPage + "?process=Automotriz.EliminaLiquidacion&prhecho=true");
                }
                
                else{
                    Utils.MostrarAlerta(Response, "Ocurrió un error durante la ejecución..!");
                    return;
                }
        }

	}
}