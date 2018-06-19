namespace AMS.Tramite
{
    public class Tramite
    {

        private int codigo;
        private string nombre;
        private string descripcion;
        private DestinoTramite destino;
        private float valorAproximado;

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

        public DestinoTramite Destino
        {
            get
            {
                return destino;
            }
            set
            {
                destino = value;
            }
        }

        public float ValorAproximado
        {
            get
            {
                return valorAproximado;
            }
            set
            {
                valorAproximado = value;
            }
        }

        public Tramite()
        {
            this.codigo = -1;
            this.nombre = null;
            this.descripcion = null;
            this.destino = null;
            this.valorAproximado = -1;
        }

        public Tramite(int codigo, string descripcion, string nombre, DestinoTramite destino, float valorAprox)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.destino = destino;
            this.valorAproximado = valorAprox;
        }

        public Tramite(Tramite tramite)
        {
            this.codigo = tramite.codigo;
            this.nombre = tramite.nombre;
            this.descripcion = tramite.descripcion;
            this.destino = tramite.destino;
            this.valorAproximado = tramite.valorAproximado;
        }
    }
}