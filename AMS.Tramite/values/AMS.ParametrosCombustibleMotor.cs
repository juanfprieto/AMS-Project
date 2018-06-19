namespace AMS.Tramite
{
    public class ParametrosCombustibleMotor
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

        public ParametrosCombustibleMotor()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public ParametrosCombustibleMotor(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public ParametrosCombustibleMotor(ParametrosCombustibleMotor parametrosCombustibleMotor)
        {
            this.codigo = parametrosCombustibleMotor.codigo;
            this.descripcion = parametrosCombustibleMotor.descripcion;
        }
    }
}