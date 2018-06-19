using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.Tools;

namespace AMS.Reportes
{
    [Serializable]
    public class ColumnaDescriptor
    {
        public string Nombre { get; set; }
        public TipoCampo TipoCampo { get; set; }
        public TipoCampo TipoFiltro { get; set; }
        public string Agrupa { get; set; }
        public int TamanoPromedio { get; set; }
    }
}