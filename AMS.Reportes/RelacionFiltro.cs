using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace AMS.Reportes
{
    [Serializable]
    public class RelacionFiltro
    {
        public string Tabla { get; set; }
        public string CampoId { get; set; }
        public string CampoMuestra { get; set; }
        public ArrayList Data { get; set; }
    }
}