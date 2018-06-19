using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using AMS.Tools;

namespace AMS.Reportes
{
    public class ConsultaFiltroUI
    {
        public PlaceHolder PlaceHolder { get; set; }
        public Control Control { get; set; }

        private FiltroDescriptor Descriptor { get; set; }

        public ConsultaFiltroUI(FiltroDescriptor descriptor)
        {
            PlaceHolder = new PlaceHolder();
            Descriptor = descriptor;
        }

        public DictionaryEntry GetValorFiltro()
        {
            DictionaryEntry resp = new DictionaryEntry();
            resp.Key = Descriptor.Id;

            switch (Descriptor.TipoCampo)
            {
                case TipoCampo.RelacionForanea:
                    resp.Value = ((DropDownList)Control).SelectedValue;
                    break;
                case TipoCampo.Cadena:
                    resp.Value = ((TextBox)Control).Text;
                    break;
                case TipoCampo.Fecha:
                    resp.Value = ((TextBox)Control).Text;
                    break;
                case TipoCampo.Numero:
                    resp.Value = ((TextBox)Control).Text;
                    break;
                case TipoCampo.Snippet:
                    resp.Value = ((IFiltroSnippet)((PlaceHolder)Control).Controls[0]).ObtenerValorFiltro(); // traer el valor
                    break;
                case TipoCampo.Timestamp:
                    break;
            }

            return resp;
        }
    }

}