namespace AMS.Tramite
{
    public class TipoEntregable
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

        public TipoEntregable()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public TipoEntregable(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoEntregable(TipoEntregable tipoEntregable)
        {
            this.codigo = tipoEntregable.codigo;
            this.descripcion = tipoEntregable.descripcion;
        }
    }
}