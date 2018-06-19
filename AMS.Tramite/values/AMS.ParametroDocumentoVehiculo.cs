namespace AMS.Tramite
{
    public class ParametroDocumentoVehiculo
    {

        private string codigo;
        private string nombre;
        private char tieneVencimiento;
        private char tieneValor;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public char TieneVencimiento
        {
            get { return tieneVencimiento; }
            set { tieneVencimiento = value; }
        }

        public char TieneValor
        {
            get { return tieneValor; }
            set { tieneValor = value; }
        }

        public ParametroDocumentoVehiculo()
        {
            this.codigo = null;
            this.nombre = null;
            this.tieneVencimiento = ' ';
            this.tieneValor = ' ';
        }

        public ParametroDocumentoVehiculo(
            string codigo,
            string nombre,
            char tieneVencimiento,
            char tieneValor
        )
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.tieneVencimiento = tieneVencimiento;
            this.tieneValor = tieneValor;
        }

        public ParametroDocumentoVehiculo(ParametroDocumentoVehiculo parametro)
        {
            this.codigo = parametro.codigo;
            this.nombre = parametro.nombre;
            this.tieneVencimiento = parametro.tieneVencimiento;
            this.tieneValor = parametro.tieneValor;
        }
    }
}