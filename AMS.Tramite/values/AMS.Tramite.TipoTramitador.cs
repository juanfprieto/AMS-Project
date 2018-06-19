namespace AMS.Tramite
{
    public class TipoTramitador
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

        public TipoTramitador()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public TipoTramitador(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoTramitador(TipoTramitador tTramitador)
        {
            this.codigo = tTramitador.codigo;
            this.descripcion = tTramitador.descripcion;
        }
    }
}