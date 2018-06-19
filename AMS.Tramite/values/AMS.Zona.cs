namespace AMS.Tramite
{
    public class Zona
    {

        private int codigo;
        private string nombre;

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

        public Zona()
        {
            this.codigo = -1;
            this.nombre = null;
        }

        public Zona(int codigo, string nombre)
        {
            this.codigo = codigo;
            this.nombre = nombre;
        }

        public Zona(Zona zona)
        {
            this.codigo = zona.codigo;
            this.nombre = zona.nombre;
        }
    }
}