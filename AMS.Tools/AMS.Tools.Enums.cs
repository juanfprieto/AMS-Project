using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AMS.DB;
using System.Collections;
using AMS.Forms;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using System.Configuration;
using AMS.CriptoServiceProvider;

namespace AMS.Tools
{
    public class Enums { }

    public enum TipoCampo
    {
        Moneda,
        Fecha,
        Cadena,
        Numero,
        Entero,
        Timestamp,
        RelacionForanea,
        Archivo,
        Snippet
    }
}