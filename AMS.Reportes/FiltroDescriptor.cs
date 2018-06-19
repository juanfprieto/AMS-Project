using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.Tools;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
//using AjaxControlToolkit;

namespace AMS.Reportes
{
    [Serializable]
    public class FiltroDescriptor
    {
        //DATA
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Label { get; set; }
        public string TablaLupa { get; set; }
        public string TipoComparacion { get; set; }
        public string DatoComparar { get; set; }
        public string Filtro { get; set; }
        public RelacionFiltro Relacion { get; set; }
        public TipoCampo TipoCampo { get; set; }

        protected string images = ConfigurationManager.AppSettings["PathToImages"];
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];

        public ConsultaFiltroUI GetFiltroUI(Page page, int contadorFiltros)
        {
            Control control = null;
            ConsultaFiltroUI holder = new ConsultaFiltroUI(this);
            Label lbl = new Label();
            Table tabla = new Table();
            TableRow trow = new TableRow();
            TableCell tcell = new TableCell();
            lbl.Text = this.Label;
            lbl.ID = "lbl_" + contadorFiltros;

            tcell.Controls.Add(lbl);
            trow.Cells.Add(tcell);
            tcell = new TableCell();

            switch (this.TipoCampo)
            {
                case TipoCampo.RelacionForanea:
                    DropDownList ddlRelacion = new DropDownList();
                    ddlRelacion.ID = "ddl_" + contadorFiltros;
                    Utils.FillDll(ddlRelacion, this.Relacion.Data, false);
                    control = ddlRelacion;
                    tcell.Controls.Add(ddlRelacion);
                    break;
                case TipoCampo.Cadena:
                    TextBox txtCadena = new TextBox();
                    txtCadena.ID = "txtCadena_" + contadorFiltros;
                    tcell.Controls.Add(txtCadena);
                    System.Web.UI.WebControls.Image lupa = PutImageLupa(TablaLupa, txtCadena);
                    tcell.Controls.Add(lupa);
                    control = txtCadena;
                    break;
                case TipoCampo.Fecha:
                    TextBox txtFecha = new TextBox();
                    txtFecha.CssClass = "calendario";
                    txtFecha.ID = "txtFilterControlF_" + this.Id;
                    control = txtFecha;
                    tcell.Controls.Add(txtFecha); System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                    img.ID = "_ctl1_img_" + this.Id;
                    img.ImageUrl = images + "AMS.Icon.Calendar.png";
                    //CalendarExtender calendar = new CalendarExtender();
                    //TextBox txtFecha = new TextBox();
                    //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                    //img.ID = "imgFilterControl_" + this.Id;
                    //img.ImageUrl = images + "AMS.Calendar.png";
                    //calendar.TargetControlID = txtFecha.ID;
                    //calendar.Format = "yyyy-MM-dd";
                    //calendar.PopupButtonID = img.ID;
                    //control = txtFecha;
                    //tcell.Controls.Add(txtFecha);
                    tcell.Controls.Add(img);
                    //tcell.Controls.Add(calendar);
                    break;
                case TipoCampo.Numero:
                    TextBox txtNumero = new TextBox();
                    txtNumero.ID = "txtFilterControl_" + this.Id;
                    control = txtNumero;
                    tcell.Controls.Add(txtNumero);
                    break;
                case TipoCampo.Timestamp:
                    //campo = TipoCampo.Timestamp;
                    break;
                case TipoCampo.Snippet:
                    PlaceHolder phSnippet = new PlaceHolder();
                    phSnippet.Controls.Add(page.LoadControl(pathToControls + this.Nombre));
                    control = phSnippet;
                    tcell.Controls.Add(phSnippet);
                    break;
            }

            trow.Cells.Add(tcell);
            tabla.Rows.Add(trow);
            tabla.ID = "tablaFiltro";

            holder.PlaceHolder.Controls.Add(tabla);
            holder.Control = control;
            return holder;
        }

        protected System.Web.UI.WebControls.Image PutImageLupa(string tableName, TextBox tb)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            //img.ID = "img_" + i.ToString();
            if (tableName != "")
            {
                img.ImageUrl = images + "AMS.Search.png";
                img.Attributes.Add("onClick", "ModalDialog(" + tb.ClientID + ",\"SELECT * FROM " + tableName + ";\",new Array());");
                tb.Attributes.Add("onDblClick", "ModalDialog(" + tb.ClientID + ",\"SELECT * FROM " + tableName + ";\",new Array());");
                img.ToolTip = "Busqueda";
            }

            return img;
        }



    }
}