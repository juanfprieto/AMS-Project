namespace AMS.Tramite
{
    public class EstadoOrden
    {

        private char codigo;
        private string descripcion;

        public char Codigo
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

        public EstadoOrden()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public EstadoOrden(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public EstadoOrden(EstadoOrden estadoOrden)
        {
            this.codigo = estadoOrden.codigo;
            this.descripcion = estadoOrden.descripcion;
        }
    }
}