namespace AMS.Tramite
{
    public class ParametrosArancel
    {
        private string codigo;
        private string nombre;
        private TipoRegimenArancel regimen;
        private float porcentajeGavamen;
        private Iva iva;
        private string unidadComercial;
        private string vistoBueno;
        private string descMinima;

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

        public TipoRegimenArancel Regimen
        {
          get { return regimen; }
          set { regimen = value; }
        }

        public float PorcentajeGavamen
        {
          get { return porcentajeGavamen; }
          set { porcentajeGavamen = value; }
        }

        public Iva Iva
        {
          get { return iva; }
          set { iva = value; }
        }

        public string UnidadComercial
        {
          get { return unidadComercial; }
          set { unidadComercial = value; }
        }

        public string DescMinima
        {
          get { return descMinima; }
          set { descMinima = value; }
        }

        public string VistoBueno
        {
          get { return vistoBueno; }
          set { vistoBueno = value; }
        }

        public ParametrosArancel()
        {
            this.codigo = null;
            this.nombre = null;
            this.regimen = null;
            this.porcentajeGavamen = -1;
            this.iva = null;
            this.unidadComercial = null;
            this.vistoBueno = null;
            this.descMinima = null;
        }

        public ParametrosArancel(
            string codigo,
            string nombre,
            TipoRegimenArancel regimen,
            float porcentajeGavamen,
            Iva iva,
            string unidadComercial,
            string vistoBueno,
            string descMinima
        )
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.regimen = regimen;
            this.porcentajeGavamen = porcentajeGavamen;
            this.iva = iva;
            this.unidadComercial = unidadComercial;
            this.vistoBueno = vistoBueno;
            this.descMinima = descMinima;
        }

        public ParametrosArancel(ParametrosArancel parametrosArancel)
        {
            this.codigo = parametrosArancel.codigo;
            this.nombre = parametrosArancel.nombre;
            this.regimen = parametrosArancel.regimen;
            this.porcentajeGavamen = parametrosArancel.porcentajeGavamen;
            this.iva = parametrosArancel.iva;
            this.unidadComercial = parametrosArancel.unidadComercial;
            this.vistoBueno = parametrosArancel.vistoBueno;
            this.descMinima = parametrosArancel.descMinima;
        }
    }
}