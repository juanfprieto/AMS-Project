namespace AMS.Tramite
{
    public class Nit
    {

        private long numeroNit;
        private char digito;
        private string nombre1;
        private string nombre2;
        private string apellido1;
        private string apellido2;
        private char tipoNit;
        private Ciudad ciudadExpedicion;
        private char nacionalidad;    //debería ser una clase Nacionalidad (TNACIONALIDAD), talvez integrada a pais, se deja en char por simplicidad del módulo
        private string direccion;
        private Ciudad ciudad;
        private string telefono;
        private string celular;
        private string email;
        private string web;
        private char vigencia;  //deberia ser clase (TVIGENCIA)
        private char sociedad;  //deberia ser clase (TSOCIEDAD)
        private char regimenIva;  //deberia ser clase (TREGIMENIVA)
        private char actividad;


        public long NumeroNit

        {
          get { return numeroNit; }
          set { numeroNit = value; }
        }

        public char Digito
        {
          get { return digito; }
          set { digito = value; }
        }  

        public string Nombre1
        {
          get { return nombre1; }
          set { nombre1 = value; }
        }

        public string Nombre2
        {
          get { return nombre2; }
          set { nombre2 = value; }
        }

        public string Apellido1
        {
          get { return apellido1; }
          set { apellido1 = value; }
        }

        public string Apellido2
        {
          get { return apellido2; }
          set { apellido2 = value; }
        }

        public char TipoNit
        {
          get { return tipoNit; }
          set { tipoNit = value; }
        }

        public Ciudad CiudadExpedicion
        {
          get { return ciudadExpedicion; }
          set { ciudadExpedicion = value; }
        }
                
        public char Nacionalidad
        {
          get { return nacionalidad; }
          set { nacionalidad = value; }
        }

        public string Direccion
        {
          get { return direccion; }
          set { direccion = value; }
        }

        public Ciudad Ciudad
        {
          get { return ciudad; }
          set { ciudad = value; }
        }

        public string Telefono
        {
          get { return telefono; }
          set { telefono = value; }
        }

        public string Celular
        {
          get { return celular; }
          set { celular = value; }
        }

        public string Email
        {
          get { return email; }
          set { email = value; }
        }        

        public string Web
        {
          get { return web; }
          set { web = value; }
        }

        public char Vigencia
        {
          get { return vigencia; }
          set { vigencia = value; }
        }

        public char Sociedad
        {
          get { return sociedad; }
          set { sociedad = value; }
        }

        public char RegimenIva
        {
          get { return regimenIva; }
          set { regimenIva = value; }
        }
                
        public char Actividad
        {
          get { return actividad; }
          set { actividad = value; }
        }



        public Nit()
        {
            this.numeroNit = -1;
            this.digito = ' ';
            this.nombre1 = null;
            this.nombre2 = null;
            this.apellido1 = null;
            this.apellido2 = null;
            this.tipoNit = ' ';
            this.ciudadExpedicion = null;
            this.nacionalidad = ' ';
            this.direccion = null;
            this.ciudad = null;
            this.telefono = null;
            this.celular = null;
            this.email = null;
            this.web = null;
            this.vigencia = ' ';
            this.sociedad = ' ';
            this.regimenIva = ' ';
            this.actividad = ' ';
        }

        public Nit(
            long numeroNit,
            char digito,
            string nombre1,
            string nombre2,
            string apellido1,
            string apellido2,
            char tipoNit,
            Ciudad ciudadExpedicion,
            char nacionalidad,
            string direccion,
            Ciudad ciudad,
            string telefono,
            string celular,
            string email,
            string web,
            char vigencia,
            char sociedad,
            char regimenIva,
            char actividad
            )
        {
            this.numeroNit = numeroNit;
            this.digito = digito;
            this.nombre1 = nombre1;
            this.nombre2 = nombre2;
            this.apellido1 = apellido1;
            this.apellido2 = apellido2;
            this.tipoNit = tipoNit;
            this.ciudadExpedicion = ciudadExpedicion;
            this.nacionalidad = nacionalidad;
            this.direccion = direccion;
            this.ciudad = ciudad;
            this.telefono = telefono;
            this.celular = celular;
            this.email = email;
            this.web = web;
            this.vigencia = vigencia;
            this.sociedad = sociedad;
            this.regimenIva = regimenIva;
            this.actividad = actividad;
        }

        public Nit(Nit nit)
        {
            this.numeroNit = nit.numeroNit;
            this.digito = nit.digito;
            this.nombre1 = nit.nombre1;
            this.nombre2 = nit.nombre2;
            this.apellido1 = nit.apellido1;
            this.apellido2 = nit.apellido2;
            this.tipoNit = nit.tipoNit;
            this.ciudadExpedicion = nit.ciudadExpedicion;
            this.nacionalidad = nit.nacionalidad;
            this.direccion = nit.direccion;
            this.ciudad = nit.ciudad;
            this.telefono = nit.telefono;
            this.celular = nit.celular;
            this.email = nit.email;
            this.web = nit.web;
            this.vigencia = nit.vigencia;
            this.sociedad = nit.sociedad;
            this.regimenIva = nit.regimenIva;
            this.actividad = nit.actividad;
        }
    }
}