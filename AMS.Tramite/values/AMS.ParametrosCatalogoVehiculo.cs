using System;
namespace AMS.Tramite
{
    public class ParametrosCatalogoVehiculo
    {
        private string codigo;
        private string descripcion;
        private string cilindraje;
        private Marca marca;
        private TipoServicioVehiculo tipoServicio;
        private ParametrosClaseVehiculo claseVehiculo;
        private string capacidad;
        private int numPuertas;
        private string carroceria;
        private string codTransito;
        private Iva iva;
        private ParametrosCombustibleMotor combustible;
        private ParametrosGama gama;
        private ParametrosGrupoCatalogo grupo;
        private string vinBasico;
        private string numPasajeros;
        private int limKilometrajeGarantia;
        private int mesesGarantia;
        private int consecutivoVIN;
        private ParametrosArancel arancel;

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

        public string Cilindraje
        {
          get { return cilindraje; }
          set { cilindraje = value; }
        }

        public Marca Marca
        {
          get { return marca; }
          set { marca = value; }
        }

        public TipoServicioVehiculo TipoServicio
        {
          get { return tipoServicio; }
          set { tipoServicio = value; }
        }

        public ParametrosClaseVehiculo ClaseVehiculo
        {
          get { return claseVehiculo; }
          set { claseVehiculo = value; }
        }

        public string Capacidad
        {
          get { return capacidad; }
          set { capacidad = value; }
        }

        public int NumPuertas
        {
          get { return numPuertas; }
          set { numPuertas = value; }
        }

        public string Carroceria
        {
          get { return carroceria; }
          set { carroceria = value; }
        }

        public string CodTransito
        {
          get { return codTransito; }
          set { codTransito = value; }
        }

        public Iva Iva
        {
          get { return iva; }
          set { iva = value; }
        }

        public ParametrosCombustibleMotor Combustible
        {
          get { return combustible; }
          set { combustible = value; }
        }

        public ParametrosGama Gama
        {
          get { return gama; }
          set { gama = value; }
        }

        public ParametrosGrupoCatalogo Grupo
        {
          get { return grupo; }
          set { grupo = value; }
        }

        public string VinBasico
        {
          get { return vinBasico; }
          set { vinBasico = value; }
        }

        public string NumPasajeros
        {
          get { return numPasajeros; }
          set { numPasajeros = value; }
        }

        public int LimKilometrajeGarantia
        {
          get { return limKilometrajeGarantia; }
          set { limKilometrajeGarantia = value; }
        }

        public int MesesGarantia
        {
          get { return mesesGarantia; }
          set { mesesGarantia = value; }
        }

        public int ConsecutivoVIN
        {
          get { return consecutivoVIN; }
          set { consecutivoVIN = value; }
        }

        public ParametrosArancel Arancel
        {
          get { return arancel; }
          set { arancel = value; }
        }

        public ParametrosCatalogoVehiculo()
        {
            this.codigo = null;
            this.descripcion = null;
            this.cilindraje = null;
            this.marca = null;
            this.tipoServicio = null;
            this.claseVehiculo = null;
            this.capacidad = null;
            this.numPuertas = -1;
            this.carroceria = null;
            this.codTransito = null;
            this.iva = null;
            this.combustible = null;
            this.gama = null;
            this.grupo = null;
            this.vinBasico = null;
            this.numPasajeros = null;
            this.limKilometrajeGarantia = -1;
            this.mesesGarantia = -1;
            this.consecutivoVIN = -1;
            this.arancel = null;
        }

        public ParametrosCatalogoVehiculo(
            string codigo,
            string descripcion,
            string cilindraje,
            Marca marca,
            TipoServicioVehiculo tipoServicio,
            ParametrosClaseVehiculo claseVehiculo,
            string capacidad,
            int numPuertas,
            string carroceria,
            string codTransito,
            Iva iva,
            ParametrosCombustibleMotor combustible,
            ParametrosGama gama,
            ParametrosGrupoCatalogo grupo,
            string vinBasico,
            string numPasajeros,
            int limKilometrajeGarantia,
            int mesesGarantia,
            int consecutivoVIN,
            ParametrosArancel arancel
        )
        {
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.cilindraje = cilindraje;
            this.marca = marca;
            this.tipoServicio = tipoServicio;
            this.claseVehiculo = claseVehiculo;
            this.capacidad = capacidad;
            this.numPuertas = numPuertas;
            this.carroceria = carroceria;
            this.codTransito = codTransito;
            this.iva = iva;
            this.combustible = combustible;
            this.gama = gama;
            this.grupo = grupo;
            this.vinBasico = vinBasico;
            this.numPasajeros = numPasajeros;
            this.limKilometrajeGarantia = limKilometrajeGarantia;
            this.mesesGarantia = mesesGarantia;
            this.consecutivoVIN = consecutivoVIN;
            this.arancel = arancel;
        }

        public ParametrosCatalogoVehiculo(ParametrosCatalogoVehiculo parametrosCatalogoVehiculo)
        {
            this.codigo = parametrosCatalogoVehiculo.codigo;
            this.descripcion = parametrosCatalogoVehiculo.descripcion;
            this.cilindraje = parametrosCatalogoVehiculo.cilindraje;
            this.marca = parametrosCatalogoVehiculo.marca;
            this.tipoServicio = parametrosCatalogoVehiculo.tipoServicio;
            this.claseVehiculo = parametrosCatalogoVehiculo.claseVehiculo;
            this.capacidad = parametrosCatalogoVehiculo.capacidad;
            this.numPuertas = parametrosCatalogoVehiculo.numPuertas;
            this.carroceria = parametrosCatalogoVehiculo.carroceria;
            this.codTransito = parametrosCatalogoVehiculo.codTransito;
            this.iva = parametrosCatalogoVehiculo.iva;
            this.combustible = parametrosCatalogoVehiculo.combustible;
            this.gama = parametrosCatalogoVehiculo.gama;
            this.grupo = parametrosCatalogoVehiculo.grupo;
            this.vinBasico = parametrosCatalogoVehiculo.vinBasico;
            this.numPasajeros = parametrosCatalogoVehiculo.numPasajeros;
            this.limKilometrajeGarantia = parametrosCatalogoVehiculo.limKilometrajeGarantia;
            this.mesesGarantia = parametrosCatalogoVehiculo.mesesGarantia;
            this.consecutivoVIN = parametrosCatalogoVehiculo.consecutivoVIN;
            this.arancel = parametrosCatalogoVehiculo.arancel;
        }
    }
}