namespace AMS.Tramite
{
    public class EstadoTramite
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

        public EstadoTramite()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public EstadoTramite(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public EstadoTramite(EstadoTramite tEstado)
        {
            this.codigo = tEstado.codigo;
            this.descripcion = tEstado.descripcion;
        }
    }
}