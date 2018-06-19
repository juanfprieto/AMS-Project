namespace AMS.Tramite
{
    public class Ciudad
    {

        private int codigo;
        private string nombre;
        private Pais paisCiudad;
        private int discadoNacional;
        private int codigoNacional;
        private Zona zonaCiudad;

        public int Codigo
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

        public Pais PaisCiudad
        {
            get
            {
                return paisCiudad;
            }
            set
            {
                paisCiudad = value;
            }
        }

        public int DiscadoNacional
        {
            get
            {
                return discadoNacional;
            }
            set
            {
                discadoNacional = value;
            }
        }

        public int CodigoNacional
        {
            get
            {
                return codigoNacional;
            }
            set
            {
                codigoNacional = value;
            }
        }

        public Zona ZonaCiudad
        {
            get
            {
                return zonaCiudad;
            }
            set
            {
                zonaCiudad = value;
            }
        }

        public Ciudad()
        {
            this.codigo = -1;
            this.nombre = null;
            this.paisCiudad = null;
            this.discadoNacional = -1;
            this.codigoNacional = -1;
            this.zonaCiudad = null;
        }

        public Ciudad(int codigo, string nombre, Pais paisc, int discadoNacional, int codigoNacional, Zona zonac)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.paisCiudad = paisc;
            this.discadoNacional = discadoNacional;
            this.codigoNacional = codigoNacional;
            this.zonaCiudad = zonac;
        }

        public Ciudad(Ciudad ciudad)
        {
            this.codigo = ciudad.codigo;
            this.nombre = ciudad.nombre;
            this.paisCiudad = ciudad.paisCiudad;
            this.discadoNacional = ciudad.discadoNacional;
            this.codigoNacional = ciudad.codigoNacional;
            this.zonaCiudad = ciudad.zonaCiudad;
        }
    }
}