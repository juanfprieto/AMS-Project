using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AMS.Forms;
using AMS.Tools;
using AMS.DB;
using System.Configuration;
using System.Drawing;

namespace AMS.SAC_Asesoria
{
	public partial class AgendaActualizaciones : System.Web.UI.UserControl
	{
        protected DataTable dtabAgregar;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlEmpresas, "select MCLG_CLIENTE from MCLIENTEGESTION order by mclg_cliente");
                ddlEmpresas.Items.Insert(0, "Seleccione un Cliente...");

                DataSet dsMarcadorActualizaciones = new DataSet();
                DBFunctions.Request(dsMarcadorActualizaciones, IncludeSchema.NO,
                    "select ma.mcli_cliente as Cliente, COUNT(ma.mcli_cliente) as Pendientes from MCLIENTEGESTION mc, MCLIENTEACTUALIZACION ma " +
                    "where mc.mclg_cliente=ma.mcli_cliente and mcli_check='N' group by ma.mcli_cliente order by Pendientes desc;");
                
                //Punto equilibrio actualizaciones
                int cantidad = dsMarcadorActualizaciones.Tables[0].Rows.Count;
                int ultima = Convert.ToInt16(dsMarcadorActualizaciones.Tables[0].Rows[0][1].ToString());
                int primera = Convert.ToInt16(dsMarcadorActualizaciones.Tables[0].Rows[cantidad-1][1].ToString());
                int promedio = ((ultima - primera)/2) + primera;
                ViewState["promedio"] = promedio;

                dgMarcadorAct.DataSource = dsMarcadorActualizaciones;
                dgMarcadorAct.DataBind();

                dtabAgregar = new DataTable();
                dtabAgregar.Columns.Add("Objeto");
                dtabAgregar.Columns.Add("Especificacion");
                dgAgregar.DataSource = dtabAgregar;
                dgAgregar.DataBind();
                Session["dtabAgregar"] = dtabAgregar;
                if(Request.QueryString["mens"] == "1")
                {
                    Utils.MostrarAlerta(Response, "Actualización pendiente registrada correctamente para todas las empresas!");
                }
                else if (Request.QueryString["mens"] != null)
                {
                    Utils.MostrarAlerta(Response, "Actualización registrada correctamente para: " + Request.QueryString["mens"]);
                }
            }
            else
            {
                dtabAgregar = (DataTable)Session["dtabAgregar"];
            }
		}

        protected void AgregarModificacion(object Sender, EventArgs e)
        {
            plcOpciones.Visible = false;
            plcAgregar.Visible = true;
            plcMarcadorActualizaciones.Visible = false;
        }

        protected void RevisarModificacion(object Sender, EventArgs e)
        {
            plcOpciones.Visible = false;
            plcRevisar.Visible = true;
            plcMarcadorActualizaciones.Visible = false;
        }

        protected void DgMarcadorActDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (ViewState["promedio"] != null)
                {
                    int actual = Convert.ToInt16((((DataBoundLiteralControl)e.Item.Cells[1].Controls[0])).Text.Trim());
                    int promedio = Convert.ToInt16(ViewState["promedio"]);
                    if (actual <= promedio)
                    {
                        ViewState.Remove("promedio");
                        e.Item.Cells[0].BorderStyle = BorderStyle.Solid;
                        e.Item.Cells[1].BorderStyle = BorderStyle.Solid;
                        e.Item.Cells[0].BorderColor = Color.DodgerBlue;
                        e.Item.Cells[1].BorderColor = Color.DodgerBlue;
                    }
                }
            }
        }

        protected void DgInsertsDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].FindControl("ddlObjetos")), "select * from  DBXSCHEMA.MOBJETOS order by mobj_nombre;");
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].FindControl("ddlEdit_Objetos")), "select * from  DBXSCHEMA.MOBJETOS order by mobj_nombre;");
                ((DropDownList)e.Item.Cells[0].FindControl("ddlEdit_Objetos")).SelectedValue = dtabAgregar.Rows[dgAgregar.EditItemIndex][0].ToString();
            }
        }

        protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "ClearRows")
                dtabAgregar.Rows.Clear();
            else if (((Button)e.CommandSource).CommandName == "AddDatasRow")
            {
                string objeto = ((DropDownList)e.Item.Cells[0].FindControl("ddlObjetos")).SelectedValue;
                string especificacion = ((TextBox)e.Item.Cells[1].FindControl("txtEspecificacion")).Text;
                DataRow dtRow = dtabAgregar.NewRow();
                dtRow[0] = objeto;
                dtRow[1] = especificacion;
                dtabAgregar.Rows.Add(dtRow);
                Session["dtabAgregar"] = dtabAgregar;
                dgAgregar.EnableViewState = true;
                dgAgregar.DataSource = dtabAgregar;
                dgAgregar.DataBind();
            }

        }

        public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
        {
            string objeto = ((DropDownList)e.Item.Cells[0].FindControl("ddlEdit_Objetos")).SelectedValue;
            string especificacion = ((TextBox)e.Item.Cells[1].FindControl("txtEdit_Especificacion")).Text;
            dtabAgregar.Rows[dgAgregar.EditItemIndex][0] = objeto;
            dtabAgregar.Rows[dgAgregar.EditItemIndex][1] = especificacion;
            Session["dtabAgregar"] = dtabAgregar;
            dgAgregar.EditItemIndex = -1;
            dgAgregar.EnableViewState = true;
            dgAgregar.DataSource = dtabAgregar;
            dgAgregar.DataBind();
        }

        protected void DgInserts_Delete(Object sender, DataGridCommandEventArgs e)
        {
            dtabAgregar.Rows.Remove(dtabAgregar.Rows[e.Item.ItemIndex]);
            dgAgregar.EditItemIndex = -1;
            dgAgregar.EnableViewState = true;
            dgAgregar.DataSource = dtabAgregar;
            dgAgregar.DataBind();
        }

        public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgAgregar.EditItemIndex = -1;
            dgAgregar.EnableViewState = true;
            dgAgregar.DataSource = dtabAgregar;
            dgAgregar.DataBind();
        }

        public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
        {
            dgAgregar.EditItemIndex = (int)e.Item.ItemIndex;
            dgAgregar.EnableViewState = true;
            dgAgregar.DataSource = dtabAgregar;
            dgAgregar.DataBind();
        }

        protected void EnviarModificacion(object Sender, EventArgs e)
        {
            if (txtMotivo.Text == "")
            {
                Utils.MostrarAlerta(Response, "Falta ingresar el motivo de la actualización!");
                return;
            }
            else if (dtabAgregar.Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se han ingresado los objetos y especificaciones de actualización!");
                return;
            }
            else 
            {
                ArrayList sqls = new ArrayList();
                sqls.Add("INSERT INTO MACTUALIZACION(MACT_MOTIVO,MACT_FECHA,MACT_USUARIO)VALUES( '" + txtMotivo.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + HttpContext.Current.User.Identity.Name.ToLower() + "')");
               
                for (int k = 0; k < dtabAgregar.Rows.Count; k++)
                {
                    sqls.Add("INSERT INTO DACTUALIZACION( MACT_CODIGO, MOBJ_CODIGO, DACT_ESPECIF)VALUES( (select MACT_CODIGO from MACTUALIZACION order by MACT_CODIGO DESC FETCH FIRST ROW ONLY), '" + dtabAgregar.Rows[k][0] + "', '" + dtabAgregar.Rows[k][1] + "');");
                }

                DataSet dsClientes = new DataSet();
                DBFunctions.Request(dsClientes, IncludeSchema.NO,"select MCLG_CLIENTE from MCLIENTEGESTION;");

                for(int j=0; j < dsClientes.Tables[0].Rows.Count; j++)
                {
                    sqls.Add("INSERT INTO MCLIENTEACTUALIZACION( MCLI_CLIENTE, MACT_CODIGO)VALUES( '" + dsClientes.Tables[0].Rows[j][0] + "', (select MACT_CODIGO from MACTUALIZACION order by MACT_CODIGO DESC FETCH FIRST ROW ONLY));");
                }

                if (DBFunctions.Transaction(sqls))
                {
                    Response.Redirect(indexPage + "?process=SAC_Asesoria.AgendaActualizaciones&mens=1");
                }
            }
        }

        protected void Cambio_Empresa(Object Sender, EventArgs e)
        {

            DatasToControls bind = new DatasToControls();
            ImageButton btnAccion = null;
            try
            {
                btnAccion = (ImageButton)Sender;
            }
            catch (Exception err)
            { }

            if (btnAccion != null)
            {
                ddlPendientesCerrar.Visible = false;
                btnActualizado.Visible = false;
                ddlEmpresas.Visible = false;
                tituloAccion.InnerText = "Listado Historial de Actualizaciones";
                plcOpciones.Visible = false;
                plcRevisar.Visible = true;
                plcMarcadorActualizaciones.Visible = false;
            }
            else 
            {
                ddlPendientesCerrar.Visible = true;
                btnActualizado.Visible = true;

                bind.PutDatasIntoDropDownList(ddlPendientesCerrar,
                        "select distinct ma.mact_codigo, ma.mact_motivo from MACTUALIZACION ma left join DACTUALIZACION da on ma.mact_codigo=da.mact_codigo " +
                        "left join MCLIENTEACTUALIZACION mc on mc.mact_codigo=da.mact_codigo where mc.mcli_cliente='" + ddlEmpresas.SelectedValue + "' and mc.mcli_check='N';");
                ddlPendientesCerrar.Items.Insert(0, "Seleccione una actualización...");
            }
            
            DataSet dsActPendientes = new DataSet();
            if (ddlEmpresas.SelectedValue != "0" || btnAccion != null)
            {
                divPendientes.InnerHtml = "";

                if( btnAccion != null)
                {
                    DBFunctions.Request(dsActPendientes, IncludeSchema.NO,
                        "select ma.mact_codigo, ma.mact_motivo,ma.mact_fecha,ma.mact_usuario,da.mobj_codigo,da.dact_especif from MACTUALIZACION ma left join DACTUALIZACION da on ma.mact_codigo=da.mact_codigo   order by mact_codigo;");
                }
                else
                {
                    DBFunctions.Request(dsActPendientes, IncludeSchema.NO,
                        "select ma.mact_codigo, ma.mact_motivo,ma.mact_fecha,ma.mact_usuario,da.mobj_codigo,da.dact_especif from MACTUALIZACION ma left join DACTUALIZACION da on ma.mact_codigo=da.mact_codigo " +
                        "left join MCLIENTEACTUALIZACION mc on mc.mact_codigo=da.mact_codigo where mc.mcli_cliente='" + ddlEmpresas.SelectedValue + "' and mc.mcli_check='N'  order by mact_codigo;");
                }

                if (dsActPendientes.Tables[0].Rows.Count > 0)
                {
                    int codigoActualizacionAux = Convert.ToInt16(dsActPendientes.Tables[0].Rows[0][0].ToString());
                    for (int r = 0; r < dsActPendientes.Tables[0].Rows.Count; r++)
                    {
                        int codigoActualizacion = Convert.ToInt16(dsActPendientes.Tables[0].Rows[r][0].ToString());
                        divPendientes.InnerHtml += "<fieldset style='background-color:#FFFFFF;'><legend><font size='4'>" + dsActPendientes.Tables[0].Rows[r][1] + "</font></legend>";
                        divPendientes.InnerHtml += "<table border='1'><tr><td bgcolor='#55BBFF'><b>" + ((DateTime)dsActPendientes.Tables[0].Rows[r][2]).ToString("yyyy-MM-dd") + "</b></td><td bgcolor='#55BBFF'><b>" + dsActPendientes.Tables[0].Rows[r][3] + "</b></td></tr>";

                        while (codigoActualizacion == codigoActualizacionAux)
                        {
                            divPendientes.InnerHtml += "<tr><td>" + dsActPendientes.Tables[0].Rows[r][4] + "</td><td>" + dsActPendientes.Tables[0].Rows[r][5] + "</td></tr>";
                            r++;

                            if (r < dsActPendientes.Tables[0].Rows.Count)
                            {
                                codigoActualizacionAux = Convert.ToInt16(dsActPendientes.Tables[0].Rows[r][0].ToString());
                            }
                            else
                            {
                                codigoActualizacionAux++;
                            }
                        }
                        r--;

                        divPendientes.InnerHtml += "</table><br>";
                        divPendientes.InnerHtml += "</fieldset><br><br>";
                    }

                }
            }
        }

        protected void ActualizacionHecha(Object Sender, EventArgs e)
        {
            if (ddlPendientesCerrar.SelectedValue != "0")
            {
                ArrayList sqls = new ArrayList();
                sqls.Add("update MCLIENTEACTUALIZACION set mcli_check='S', mcli_usuario='" + HttpContext.Current.User.Identity.Name.ToLower() + "' where mact_codigo=" + ddlPendientesCerrar.SelectedValue + " and mcli_cliente='" + ddlEmpresas.SelectedValue + "';");
                if (DBFunctions.Transaction(sqls))
                {
                    Response.Redirect(indexPage + "?process=SAC_Asesoria.AgendaActualizaciones&mens=" + ddlEmpresas.SelectedValue);
                }
            }
        }

	}
}