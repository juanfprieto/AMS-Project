namespace AMS.Tramite
{
    public class TipoOficinaTransito
    {

        private string codigo;
        private string descripcion;

        public string Codigo
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

        public TipoOficinaTransito()
        {
            this.codigo = null;
            this.descripcion = null;
        }

        public TipoOficinaTransito(string codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoOficinaTransito(TipoOficinaTransito tOficina)
        {
            this.codigo = tOficina.codigo;
            this.descripcion = tOficina.descripcion;
        }
    }
}