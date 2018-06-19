namespace AMS.Tramite
{
    public class TipoPintura
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

        public TipoPintura()
        {
            this.codigo = null;
            this.descripcion = null;
        }

        public TipoPintura(string codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoPintura(TipoPintura tipoPintura)
        {
            this.codigo = tipoPintura.codigo;
            this.descripcion = tipoPintura.descripcion;
        }
    }
}