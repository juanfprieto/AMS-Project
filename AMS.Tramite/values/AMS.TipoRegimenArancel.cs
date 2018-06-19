namespace AMS.Tramite
{
    public class TipoRegimenArancel
    {

        private char codigo;
        private string descripcion;

        public char Codigo
        {
            get { return codigo; }
            set { codigo = value; }
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

        public TipoRegimenArancel()
        {
            this.codigo = ' ';
            this.descripcion = null;
        }

        public TipoRegimenArancel(char codigo, string descripcion)
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
        }

        public TipoRegimenArancel(TipoRegimenArancel tipoRegimenArancel)
        {
            this.codigo = tipoRegimenArancel.codigo;
            this.descripcion = tipoRegimenArancel.descripcion;
        }
    }
}