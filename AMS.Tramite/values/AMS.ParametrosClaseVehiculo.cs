namespace AMS.Tramite
{
    public class ParametrosClaseVehiculo
    {

        private string codigo;
        private string descripcion;
        private char tipo;
        private int codFUN;

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

        public int CodFUN
        {
          get { return codFUN; }
          set { codFUN = value; }
        } 

        public char Tipo
        {
          get { return tipo; }
          set { tipo = value; }
        }

        public ParametrosClaseVehiculo()
        {
            this.codigo = null;
            this.descripcion = null;
            this.tipo = ' ';
            this.codFUN = -1;
        }

        public ParametrosClaseVehiculo(string codigo, string descripcion, char tipo, int codFUN)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.codFUN = codFUN;
        }

        public ParametrosClaseVehiculo(ParametrosClaseVehiculo parametroClaseVehiculo)
        {
            this.codigo = parametroClaseVehiculo.codigo;
            this.descripcion = parametroClaseVehiculo.descripcion;
            this.tipo = parametroClaseVehiculo.tipo;
            this.codFUN = parametroClaseVehiculo.codFUN;
        }
    }
}