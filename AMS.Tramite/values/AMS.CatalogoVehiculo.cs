using System;
namespace AMS.Tramite
{
    public class CatalogoVehiculo
    {
        private ParametrosCatalogoVehiculo codigo;
        private string vin;
        private string placa;
        private string motor;
        private Nit nit;
        private string serie;
        private string chasis;
        private ParametrosColor color;
        private int anoModelo;
        private TipoServicioVehiculo servicio;
        private DateTime vencimientoSeguro;
        private string concecionarioVendedor;
        private DateTime fechaVenta;
        private float kilometrajeventa;
        private string numRadio;
        private float kilometraje;
        private float kilometrajePromedio;
        private char categoria;
        private string clave;
        private DateTime fechaUltimoKm;

        public ParametrosCatalogoVehiculo Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Vin
        {
            get { return vin; }
            set { vin = value; }
        }

        public string Placa
        {
            get { return placa; }
            set { placa = value; }
        }

        public string Motor
        {
            get { return motor; }
            set { motor = value; }
        }

        public Nit Nit
        {
            get { return nit; }
            set { nit = value; }
        }

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        public string Chasis
        {
            get { return chasis; }
            set { chasis = value; }
        }

        public ParametrosColor Color
        {
            get { return color; }
            set { color = value; }
        }

        public int AnoModelo
        {
            get { return anoModelo; }
            set { anoModelo = value; }
        }

        public TipoServicioVehiculo Servicio
        {
            get { return servicio; }
            set { servicio = value; }
        }

        public DateTime VencimientoSeguro
        {
            get { return vencimientoSeguro; }
            set { vencimientoSeguro = value; }
        }

        public string ConcecionarioVendedor
        {
            get { return concecionarioVendedor; }
            set { concecionarioVendedor = value; }
        }

        public DateTime FechaVenta
        {
            get { return fechaVenta; }
            set { fechaVenta = value; }
        }

        public float Kilometrajeventa
        {
            get { return kilometrajeventa; }
            set { kilometrajeventa = value; }
        }

        public string NumRadio
        {
            get { return numRadio; }
            set { numRadio = value; }
        }

        public float Kilometraje
        {
            get { return kilometraje; }
            set { kilometraje = value; }
        }

        public float KilometrajePromedio
        {
            get { return kilometrajePromedio; }
            set { kilometrajePromedio = value; }
        }

        public char Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        public DateTime FechaUltimoKm
        {
            get { return fechaUltimoKm; }
            set { fechaUltimoKm = value; }
        }

        public CatalogoVehiculo()
        {
            this.codigo = null;
            this.vin = null;
            this.placa = null;
            this.motor = null;
            this.nit = null;
            this.serie = null;
            this.chasis = null;
            this.color = null;
            this.anoModelo = -1;
            this.servicio = null;
            this.vencimientoSeguro = new DateTime();
            this.concecionarioVendedor = null;
            this.fechaVenta = new DateTime();
            this.kilometrajeventa = -1;
            this.numRadio = null;
            this.kilometraje = -1;
            this.kilometrajePromedio = -1;
            this.categoria = ' ';
            this.clave = null;
            this.fechaUltimoKm = new DateTime();
        }

        public CatalogoVehiculo(
            ParametrosCatalogoVehiculo codigo,
            string vin,
            string placa,
            string motor,
            Nit nit,
            string serie,
            string chasis,
            ParametrosColor color,
            int anoModelo,
            TipoServicioVehiculo servicio,
            DateTime vencimientoSeguro,
            string concecionarioVendedor,
            DateTime fechaVenta,
            float kilometrajeventa,
            string numRadio,
            float kilometraje,
            float kilometrajePromedio,
            char categoria,
            string clave,
            DateTime fechaUltimoKm
        )
        {
            this.codigo = codigo;
            this.vin = vin;
            this.placa = placa;
            this.motor = motor;
            this.nit = nit;
            this.serie = serie;
            this.chasis = chasis;
            this.color = color;
            this.anoModelo = anoModelo;
            this.servicio = servicio;
            this.vencimientoSeguro = vencimientoSeguro;
            this.concecionarioVendedor = concecionarioVendedor;
            this.fechaVenta = fechaVenta;
            this.kilometrajeventa = kilometrajeventa;
            this.numRadio = numRadio;
            this.kilometraje = kilometraje;
            this.kilometrajePromedio = kilometrajePromedio;
            this.categoria = categoria;
            this.clave = clave;
            this.fechaUltimoKm = fechaUltimoKm;
        }

        public CatalogoVehiculo(CatalogoVehiculo catalogoVehiculo)
        {
            this.codigo = catalogoVehiculo.codigo;
            this.vin = catalogoVehiculo.vin;
            this.placa = catalogoVehiculo.placa;
            this.motor = catalogoVehiculo.motor;
            this.nit = catalogoVehiculo.nit;
            this.serie = catalogoVehiculo.serie;
            this.chasis = catalogoVehiculo.chasis;
            this.color = catalogoVehiculo.color;
            this.anoModelo = catalogoVehiculo.anoModelo;
            this.servicio = catalogoVehiculo.servicio;
            this.vencimientoSeguro = catalogoVehiculo.vencimientoSeguro;
            this.concecionarioVendedor = catalogoVehiculo.concecionarioVendedor;
            this.fechaVenta = catalogoVehiculo.fechaVenta;
            this.kilometrajeventa = catalogoVehiculo.kilometrajeventa;
            this.numRadio = catalogoVehiculo.numRadio;
            this.kilometraje = catalogoVehiculo.kilometraje;
            this.kilometrajePromedio = catalogoVehiculo.kilometrajePromedio;
            this.categoria = catalogoVehiculo.categoria;
            this.clave = catalogoVehiculo.clave;
            this.fechaUltimoKm = catalogoVehiculo.fechaUltimoKm;
        }
    }
}