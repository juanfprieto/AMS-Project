namespace AMS.Tramite
{
    public class TipoDocumento
    {

        private string codigo;
        private string descripcion;

        public string Codigo
        {
            get {return codigo;}
            set {codigo = value;}
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

        public TipoDocumento()
        {
            this.codigo = null;
            this.descripcion = null;
        }

        public TipoDocumento(string codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoDocumento(TipoDocumento tipoDocumento)
        {
            this.codigo = tipoDocumento.codigo;
            this.descripcion = tipoDocumento.descripcion;
        }
    }
}