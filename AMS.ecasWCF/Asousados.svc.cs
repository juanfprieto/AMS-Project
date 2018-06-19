using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Asousados.Operaciones;

namespace AMS.ecasWCF
{
    [AspNetCompatibilityRequirements(RequirementsMode
    = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Asousados : IAsousados
    {

        #region Métodos de Busqueda

        string IAsousados.BusquedaInteligente()
        { 
            return OperacionesBusqueda.BusquedaInteligente();
        }
        #endregion

    }
}
