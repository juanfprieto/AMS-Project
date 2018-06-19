namespace AMS.Tramite
{
    public class ParametrosGama
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

        public ParametrosGama()
        {
            this.codigo = null;
            this.descripcion = null;
        }

        public ParametrosGama(string codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public ParametrosGama(ParametrosGama parametrosGama)
        {
            this.codigo = parametrosGama.codigo;
            this.descripcion = parametrosGama.descripcion;
        }
    }
}