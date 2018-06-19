namespace AMS.Tramite
{
    public class ParametrosColor
    {
        private string codigo;
        private string descripcion;
        private TipoPintura tipo;
        private string casaMatriz;
        private string colorRGB;
        private string vigente;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public TipoPintura Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public string CasaMatriz
        {
            get { return casaMatriz; }
            set { casaMatriz = value; }
        }

        public string ColorRGB
        {
            get { return colorRGB; }
            set { colorRGB = value; }
        }

        public string Vigente
        {
            get { return vigente; }
            set { vigente = value; }
        }

        public ParametrosColor()
        {
            this.codigo = null;
            this.descripcion = null;
            this.tipo = null;
            this.casaMatriz = null;
            this.colorRGB = null;
            this.vigente = null;
        }

        public ParametrosColor(
            string codigo,
            string descripcion,
            TipoPintura tipo,
            string casaMatriz,
            string colorRGB,
            string vigente
        )
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.casaMatriz = casaMatriz;
            this.colorRGB = colorRGB;
            this.vigente = vigente;
        }

        public ParametrosColor(ParametrosColor parametrosColor)
        {
            this.codigo = parametrosColor.codigo;
            this.descripcion = parametrosColor.descripcion;
            this.tipo = parametrosColor.tipo;
            this.casaMatriz = parametrosColor.casaMatriz;
            this.colorRGB = parametrosColor.colorRGB;
            this.vigente = parametrosColor.vigente;
        }
    }
}