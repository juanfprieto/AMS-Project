namespace AMS.Tramite
{
    public class TipoVigencia
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

        public TipoVigencia()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public TipoVigencia(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoVigencia(TipoVigencia tipoVigencia)
        {
            this.codigo = tipoVigencia.codigo;
            this.descripcion = tipoVigencia.descripcion;
        }
    }
}