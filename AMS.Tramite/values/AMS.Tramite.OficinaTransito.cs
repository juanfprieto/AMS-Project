namespace AMS.Tramite
{
    public class OficinaTransito
    {

        private int codigo;
        private string nombre;
        private string direccion;
        private Ciudad ciudadOficina;
        private TipoOficinaTransito tipo;

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

        public string Direccion
        {
            get
            {
                return direccion;
            }
            set
            {
                direccion = value;
            }
        }

        public Ciudad CiudadOficina
        {
            get
            {
                return ciudadOficina;
            }
            set
            {
                ciudadOficina = value;
            }
        }

        public TipoOficinaTransito Tipo
        {
            get
            {
                return tipo;
            }
            set
            {
                tipo = value;
            }
        }

        public OficinaTransito()
        {
            this.codigo = -1;
            this.nombre = null;
            this.direccion = null;
            this.ciudadOficina = null;
            this.tipo = null;
        }

        public OficinaTransito(int codigo, string nombre, string direccion, Ciudad ciudadOficina, TipoOficinaTransito tipo)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.direccion = direccion;
            this.ciudadOficina = ciudadOficina;
            this.tipo = tipo;
        }

        public OficinaTransito(OficinaTransito oficina)
        {
            this.codigo = oficina.codigo;
            this.nombre = oficina.nombre;
            this.direccion = oficina.direccion;
            this.ciudadOficina = oficina.ciudadOficina;
            this.tipo = oficina.tipo;
        }
    }
}