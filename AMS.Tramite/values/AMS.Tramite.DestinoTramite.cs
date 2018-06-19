namespace AMS.Tramite
{
    public class DestinoTramite
    {

        private int codigo;
        private string descripcion;

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

        public DestinoTramite()
        {
            this.codigo = -1;
            this.descripcion = null;
        }

        public DestinoTramite(int codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public DestinoTramite(DestinoTramite destino)
        {
            this.codigo = destino.codigo;
            this.descripcion = destino.descripcion;
        }
    }
}