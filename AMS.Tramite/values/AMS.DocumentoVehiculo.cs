using System;
namespace AMS.Tramite
{
    public class DocumentoVehiculo
    {

        private int codigo;
        private string vin;
        private ParametroDocumentoVehiculo parametrosDocumento;
        private string numero;
        private DateTime fechaIngreso;
        private DateTime fechaVencimiento;
        private float valor;
        private string observacion;
        private string nombreTramitante;
        private char estaEntregado;

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Vin
        {
            get { return vin; }
            set { vin = value; }
        }

        public ParametroDocumentoVehiculo ParametrosDocumento
        {
            get { return parametrosDocumento; }
            set { parametrosDocumento = value; }
        }

        public string Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        public DateTime FechaIngreso
        {
            get { return fechaIngreso; }
            set { fechaIngreso = value; }
        }

        public DateTime FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }

        public float Valor
        {
            get { return valor; }
            set { valor = value; }
        }
        public string Observacion
        {
            get { return observacion; }
            set { observacion = value; }
        }

        public string NombreTramitante
        {
            get { return nombreTramitante; }
            set { nombreTramitante = value; }
        }

        public char EstaEntregado
        {
            get { return estaEntregado; }
            set { estaEntregado = value; }
        }

        public DocumentoVehiculo()
        {
            this.codigo = -1;
            this.vin = null;
            this.parametrosDocumento = null;
            this.numero = null;
            this.fechaIngreso = new DateTime();
            this.fechaVencimiento = new DateTime();
            this.valor = -1;
            this.observacion = null;
            this.nombreTramitante = null;
            this.estaEntregado = ' ';
        }

        public DocumentoVehiculo(
            int codigo,
            string vin,
            ParametroDocumentoVehiculo parametrosDocumento,
            string numero,
            DateTime fechaIngreso,
            DateTime fechaVencimiento,
            float valor,
            string observacion,
            string nombreTramitante,
            char estaEntregado
        )
        {
            this.codigo = codigo;
            this.vin = vin;
            this.parametrosDocumento = parametrosDocumento;
            this.numero = numero;
            this.fechaIngreso = fechaIngreso;
            this.fechaVencimiento = fechaVencimiento;
            this.valor = valor;
            this.observacion = observacion;
            this.nombreTramitante = nombreTramitante;
            this.estaEntregado = estaEntregado;
        }

        public DocumentoVehiculo(DocumentoVehiculo documentoVehiculo)
        {
            this.codigo = documentoVehiculo.codigo;
            this.vin = documentoVehiculo.vin;
            this.parametrosDocumento = documentoVehiculo.parametrosDocumento;
            this.numero = documentoVehiculo.numero;
            this.fechaIngreso = documentoVehiculo.fechaIngreso;
            this.fechaVencimiento = documentoVehiculo.fechaVencimiento;
            this.valor = documentoVehiculo.valor;
            this.observacion = documentoVehiculo.observacion;
            this.nombreTramitante = documentoVehiculo.nombreTramitante;
            this.estaEntregado = documentoVehiculo.estaEntregado;
        }
    }
}