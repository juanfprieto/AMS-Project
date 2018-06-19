using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class ModificarFactura : System.Web.UI.UserControl
	{
        public RevisorFactura revisorFac;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlPrefijoFactura, "select pdoc_codigo, pdoc_codigo concat ' - ' concat pdoc_nombre from pdocumento where pdoc_codigo = 'APV'  or pdoc_codigo = 'FV';");
                bind.PutDatasIntoDropDownList(ddlNumeroFactura, "select mfac_numedocu from mfacturacliente where pdoc_codigo='" + ddlPrefijoFactura.SelectedValue + "' order by mfac_numedocu desc;");

                if (Request.QueryString["prefF"] != null && Request.QueryString["numF"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha ajustado correctamente la Factura: " + Request.QueryString["prefF"] + "-" + Request.QueryString["numF"]);
                }
            }
		}

        protected void DgInsertsDataBound(object sender, DataGridItemEventArgs e)
        {
        }
        protected void DgInsertsDataBoundRep(object sender, DataGridItemEventArgs e)
        {
        }
        protected void DgInsertsDataBoundBod(object sender, DataGridItemEventArgs e)
        {
        }

        public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
        {
            dgObservaciones.EditItemIndex = (int)e.Item.ItemIndex;
            BindDatas("Op");
            //((TextBox)dgObservaciones.Items[dgObservaciones.EditItemIndex].Cells[7].Controls[1]).ReadOnly = false; //Ejemplo para alterar campo en modo edicion...
        }
        public void DgInserts_EditRep(Object sender, DataGridCommandEventArgs e)
        {
            dgRepuestos.EditItemIndex = (int)e.Item.ItemIndex;
            BindDatas("Re");
        }
        public void DgInserts_EditBod(Object sender, DataGridCommandEventArgs e)
        {
            dgBodegaRep.EditItemIndex = (int)e.Item.ItemIndex;
            BindDatas("Bo");
        }

        public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgObservaciones.EditItemIndex = -1;
            BindDatas("Op");
        }
        public void DgInserts_CancelRep(Object sender, DataGridCommandEventArgs e)
        {
            dgRepuestos.EditItemIndex = -1;
            BindDatas("Re");
        }
        public void DgInserts_CancelBod(Object sender, DataGridCommandEventArgs e)
        {
            dgBodegaRep.EditItemIndex = -1;
            BindDatas("Bo");
        }

        public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
        {
            //if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text))
            //{
            //    Utils.MostrarAlerta(Response, "Cantidad Invalida!");
            //    dgObservaciones.EditItemIndex = -1;
            //    BindDatas();
            //    return;
            //}
            revisorFac = (RevisorFactura)Session["revisorFac"];
            double nuevoValorReal = 0;
            try
            {
                nuevoValorReal = Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
            }
            catch (Exception ee)
            {
                nuevoValorReal = 0;
            }
            revisorFac.Operaciones.Tables[0].Rows[dgObservaciones.EditItemIndex][4] = nuevoValorReal;
            revisorFac.Operaciones.Tables[0].Rows[dgObservaciones.EditItemIndex][3] = Math.Round(nuevoValorReal, 2);
            revisorFac.CalcularTotales("Op");
            AjustarTotalesOperaciones();
            Session["revisorFac"] = revisorFac;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][2] = cant;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][3] = cantA;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][4] = pr;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][6] = desc;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][7] = tot;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][8] = cantD;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][9] = totA;
            //dtInserts.Rows[dgObservaciones.EditItemIndex][10] = pr;

            dgObservaciones.EditItemIndex = -1;
            BindDatas("Op");
        }
        public void DgInserts_UpdateRep(Object sender, DataGridCommandEventArgs e)
        {
            revisorFac = (RevisorFactura)Session["revisorFac"];
            double nuevoValorReal = 0;
            try
            {
                nuevoValorReal = Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
            }
            catch (Exception ee)
            {
                nuevoValorReal = 0;
            }
            revisorFac.Repuestos.Tables[0].Rows[dgRepuestos.EditItemIndex][4] = nuevoValorReal;
            revisorFac.Repuestos.Tables[0].Rows[dgRepuestos.EditItemIndex][3] = Math.Round(nuevoValorReal, 2);
            revisorFac.CalcularTotales("Re");
            AjustarTotalesRepuestosBodegaRe();
            Session["revisorFac"] = revisorFac;

            dgRepuestos.EditItemIndex = -1;
            BindDatas("Re");
        }
        public void DgInserts_UpdateBod(Object sender, DataGridCommandEventArgs e)
        {
            revisorFac = (RevisorFactura)Session["revisorFac"];
            double nuevoValorReal = 0;
            try
            {
                nuevoValorReal = Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
            }
            catch (Exception ee)
            {
                nuevoValorReal = 0;
            }
            revisorFac.BodegaRepuestos.Tables[0].Rows[dgBodegaRep.EditItemIndex][4] = nuevoValorReal;
            revisorFac.BodegaRepuestos.Tables[0].Rows[dgBodegaRep.EditItemIndex][3] = Math.Round(nuevoValorReal, 2);
            revisorFac.CalcularTotales("Bo");
            AjustarTotalesRepuestosBodegaRe();
            Session["revisorFac"] = revisorFac;

            dgBodegaRep.EditItemIndex = -1;
            BindDatas("Bo");
        }

        protected void BindDatas(string tipo)
        {
            switch (tipo)
            {
                case "Op": //Operaciones.
                    dgObservaciones.EnableViewState = true;
                    revisorFac = (RevisorFactura)Session["revisorFac"];
                    dgObservaciones.DataSource = revisorFac.Operaciones;
                    dgObservaciones.DataBind();
                    break;
                case "Re": //Repuestos.
                    dgRepuestos.EnableViewState = true;
                    revisorFac = (RevisorFactura)Session["revisorFac"];
                    dgRepuestos.DataSource = revisorFac.Repuestos;
                    dgRepuestos.DataBind();
                    break;
                case "Bo": //Bodega Repuestos.
                    dgBodegaRep.EnableViewState = true;
                    revisorFac = (RevisorFactura)Session["revisorFac"];
                    dgBodegaRep.DataSource = revisorFac.BodegaRepuestos;
                    dgBodegaRep.DataBind();
                    break;
                default:
                    break;
            }

            //dvInserts = new DataView(dtInserts);
            
            //Session["dtInsertsCP"] = dtInserts;
            //tipoPedido = DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "'");
            
            //for (i = 0; i < dgObservaciones.Columns.Count; i++)
            //    if (i >= 3 && i <= 9)
            //        for (j = 0; j < dgObservaciones.Items.Count; j++)
            //            dgObservaciones.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
            //Debemos revisar si es tipo cliente y asi colocar los respectivos colores del semaforo
            //if (Tipo != "P")
            //{
            //    for (i = 0; i < dtInserts.Rows.Count; i++)
            //    {
            //        if (dtInserts.Rows[i][12].ToString() == "0")
            //            dgObservaciones.Items[i].Cells[4].BackColor = Color.Red;
            //        else if (dtInserts.Rows[i][12].ToString() == "1")
            //            dgObservaciones.Items[i].Cells[4].BackColor = Color.Yellow;
            //        else if (dtInserts.Rows[i][12].ToString() == "2")
            //            dgObservaciones.Items[i].Cells[4].BackColor = Color.Green;
            //        else
            //            lbInfo.Text += "<br>" + dtInserts.Rows[i][12].ToString();
            //    }
            //}
            //txtNumItem.Text = dtInserts.Rows.Count.ToString();
            //double t = 0, ta = 0;
            //int n;
            //if (dtInserts.Rows.Count > 0)
            //{
            //    for (n = 0; n < dtInserts.Rows.Count; n++)
            //    {
            //        t += Convert.ToDouble(dtInserts.Rows[n][7]);
            //        ta += Convert.ToDouble(dtInserts.Rows[n][9]);
            //    }
            //}
            //txtTotal.Text = t.ToString("C");
            //txtTotAsig.Text = ta.ToString("C");
            //if (dtInserts.Rows.Count == 0)
            //{
            //    dgObservaciones.EditItemIndex = -1;
            //    ddlAlmacen.Enabled = ddlPrecios.Enabled = ddlTipoOrden.Enabled = ddlCodigo.Enabled = ddlNumOrden.Enabled = txtNIT.Enabled = txtNITa.Enabled = tbDate.Enabled = txtNumPed.Enabled = ddlCargo.Enabled = true;
            //}
            //else
            //    ddlAlmacen.Enabled = ddlPrecios.Enabled = ddlNumOrden.Enabled = ddlTipoOrden.Enabled = ddlCodigo.Enabled = txtNumPed.Enabled = txtNIT.Enabled = txtNITa.Enabled = tbDate.Enabled = ddlCargo.Enabled = false;
        }

        protected void AjustarTotalesOperaciones()
        {
            txtTotalRound.Text = revisorFac.SumaOperacionesAprox.ToString("N2");
            txtTotalReal.Text = revisorFac.SumaOperacionesReal.ToString("N4");
            txtTotalRealRound.Text = Math.Round(revisorFac.SumaOperacionesReal, 2).ToString("N2");
            if (txtTotalRound.Text == txtTotalRealRound.Text)
            {
                txtTotalRound.BackColor = Color.LightGreen;
                txtTotalRealRound.BackColor = Color.LightGreen;
            }
            else
            {
                txtTotalRound.BackColor = Color.Transparent;
                txtTotalRealRound.BackColor = Color.Transparent;
            }
            AjustarTotalesFinales();
        }

        protected void AjustarTotalesRepuestosBodegaRe()
        {
            txtTotalRoundRepBodRe.Text = (revisorFac.SumaRepuestosAprox + revisorFac.SumaBodegaRepuestosAprox).ToString("N2");
            txtTotalRealRepBodRe.Text = (revisorFac.SumaRepuestosReal + revisorFac.SumaBodegaRepuestosReal).ToString("N4");
            txtTotalRealRoundRepBodRe.Text = Math.Round(revisorFac.SumaRepuestosReal + revisorFac.SumaBodegaRepuestosReal, 2).ToString("N2");
            if (txtTotalRoundRepBodRe.Text == txtTotalRealRoundRepBodRe.Text)
            {
                txtTotalRoundRepBodRe.BackColor = Color.LightGreen;
                txtTotalRealRoundRepBodRe.BackColor = Color.LightGreen;
            }
            else
            {
                txtTotalRoundRepBodRe.BackColor = Color.Transparent;
                txtTotalRealRoundRepBodRe.BackColor = Color.Transparent;
            }
            AjustarTotalesFinales();
        }

        protected void AjustarTotalesFinales()
        {
            try
            {
                txtSubTotal.Text = (Convert.ToDecimal(txtTotalRound.Text) + Convert.ToDecimal(txtTotalRoundRepBodRe.Text)).ToString("N2");
                txtIVA.Text = (revisorFac.SumaOperacionesIVA + revisorFac.SumaRepuestosIVA + revisorFac.SumaBodegaRepuestosIVA).ToString("N2");
                txtTotal.Text = (Convert.ToDecimal(txtSubTotal.Text) + Convert.ToDecimal(txtIVA.Text)).ToString("N2");
                if (txtTotalRound.BackColor == Color.LightGreen && txtTotalRoundRepBodRe.BackColor == Color.LightGreen)
                {
                    txtSubTotal.BackColor = Color.LightBlue;
                }
                else
                {
                    txtSubTotal.BackColor = Color.Transparent;
                }
            }
            catch (Exception ee) 
            { }
        }

        protected void CargarNumeroFacturas(Object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlNumeroFactura, "select mfac_numedocu from mfacturacliente where pdoc_codigo='" + ddlPrefijoFactura.SelectedValue + "' order by mfac_numedocu desc;");
        }

        protected void AjustarFactura(Object Sender, EventArgs e)
        {
            revisorFac = (RevisorFactura)Session["revisorFac"];
            if (txtSubTotal.BackColor != Color.LightBlue)
            {
                Utils.MostrarAlerta(Response,"El subtotal aun no se encuentra ajustado. Por favor verificar.");
                return;
            }

            if ( revisorFac.AjustarFactura() )
            {
                Session.Clear();
                Response.Redirect("" + indexPage + "?process=Automotriz.ModificarFactura&prefF=" + ddlPrefijoFactura.SelectedValue + "&numF=" + ddlNumeroFactura.SelectedValue );
            }
            else 
            {
                Utils.MostrarAlerta(Response,"Ha ocurrido un error en la actualización de esta Factura. Contactar administrador de Sistemas.");
            }
        }

        protected void RevisarFactura(Object Sender, EventArgs e)
        {
            plcBuscarFactura.Visible = false;
            plcTablasDatosFactura.Visible = true;

            string prefijoFac = ddlPrefijoFactura.SelectedValue;
            string numeroFac = ddlNumeroFactura.SelectedValue;
            revisorFac = new RevisorFactura(prefijoFac,numeroFac);
            Session["revisorFac"] = revisorFac;
            lblFactura.Text = "REVISANDO FACTURA: " + prefijoFac + "-" + numeroFac;
            lblsubTotalDB.Text = revisorFac.SubTotal_DB.ToString("N2");
            lblIVADB.Text = revisorFac.IVA_DB.ToString("N2");

            dgObservaciones.DataSource = revisorFac.Operaciones;
            dgObservaciones.DataBind();
            dgRepuestos.DataSource = revisorFac.Repuestos;
            dgRepuestos.DataBind();
            dgBodegaRep.DataSource = revisorFac.BodegaRepuestos;
            dgBodegaRep.DataBind();

            AjustarTotalesOperaciones();
            AjustarTotalesRepuestosBodegaRe();
            AjustarTotalesFinales();
        }
	}
}
