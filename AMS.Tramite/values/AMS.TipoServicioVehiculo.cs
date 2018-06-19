namespace AMS.Tramite
{
    public class TipoServicioVehiculo
    {

        private char codigo;
        private string descripcion;

        public char Codigo
        {
            get
            {
                return codigo;
            }
            set
            {
                codigo = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return descripcion;
            }
            set
            {
                descripcion = value;
            }
        }

        public TipoServicioVehiculo()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public TipoServicioVehiculo(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoServicioVehiculo(TipoServicioVehiculo tipoServicioVehiculo)
        {
            this.codigo = tipoServicioVehiculo.codigo;
            this.descripcion = tipoServicioVehiculo.descripcion;
        }
    }
}