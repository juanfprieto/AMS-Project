namespace AMS.Tramite
{
    public class Pais
    {

        private string codigo;
        private string nombre;
        private int discadoInternacional;

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

        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value;
            }
        }

        public int DiscadoInternacional
        {
            get
            {
                return discadoInternacional;
            }
            set
            {
                discadoInternacional = value;
            }
        }

        public Pais()
        {
            this.codigo = null;
            this.nombre = null;
            this.discadoInternacional = -1;
        }

        public Pais(string codigo, string nombre, int discadoInternacional)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.discadoInternacional = discadoInternacional;
        }

        public Pais(Pais pais)
        {
            this.codigo = pais.codigo;
            this.nombre = pais.nombre;
            this.discadoInternacional = pais.discadoInternacional;
        }
    }
}