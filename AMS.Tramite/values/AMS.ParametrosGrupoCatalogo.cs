namespace AMS.Tramite
{
    public class ParametrosGrupoCatalogo
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

        public ParametrosGrupoCatalogo()
        {
            this.codigo = null;
            this.descripcion = null;
        }

        public ParametrosGrupoCatalogo(string codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public ParametrosGrupoCatalogo(ParametrosGrupoCatalogo parametrosGrupoCatalogo)
        {
            this.codigo = parametrosGrupoCatalogo.codigo;
            this.descripcion = parametrosGrupoCatalogo.descripcion;
        }
    }
}