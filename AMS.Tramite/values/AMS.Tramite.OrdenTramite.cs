using System;
using System.Collections.Generic;
namespace AMS.Tramite
{
    public class OrdenTramite
    {
        private Documento prefijo;
        private int numero;
        private EstadoOrden estado;
        private CatalogoVehiculo vehiculo;
        private Nit nit;
        private string contacto;
        private float costo;
        private float iva; //sale de CEMPRESA, talvez crear un objeto estatico cargado al inicio que designe los valores, crear un objeto cempresa es muy costoso a este nivel
        private DateTime creacion;
        private DateTime finalizacion;

        public Documento Prefijo
        {
            get { return prefijo; }
            set { prefijo = value; }
        }

        public int Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        public EstadoOrden Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public CatalogoVehiculo Vehiculo
        {
            get { return vehiculo; }
            set { vehiculo = value; }
        }

        public Nit Nit
        {
            get { return nit; }
            set { nit = value; }
        }

        public string Contacto
        {
            get { return contacto; }
            set { contacto = value; }
        }

        public float Costo
        {
            get { return costo; }
            set { costo = value; }
        }

        public float Iva
        {
            get { return iva; }
            set { iva = value; }
        }

        public DateTime Creacion
        {
            get { return creacion; }
            set { creacion = value; }
        }

        public DateTime Finalizacion
        {
            get { return finalizacion; }
            set { finalizacion = value; }
        }

        public OrdenTramite()
        {
            this.prefijo = null;
            this.numero = -1;
            this.estado = null;
            this.vehiculo = null;
            this.nit = null;
            this.contacto = null;
            this.costo = -1;
            this.iva = -1;
            this.creacion = new DateTime();
            this.finalizacion = new DateTime();
        }

        public OrdenTramite(
            Documento prefijo,
            int numero,
            EstadoOrden estado,
            CatalogoVehiculo vehiculo,
            Nit nit,
            string contacto,
            float costo,
            float iva,
            DateTime creacion,
            DateTime finalizacion
        )
        {
            this.prefijo = prefijo;
            this.numero = numero;
            this.estado = estado;
            this.vehiculo = vehiculo;
            this.nit = nit;
            this.contacto = contacto;
            this.costo = costo;
            this.iva = iva;
            this.creacion = creacion;
            this.finalizacion = finalizacion;
        }

        public OrdenTramite(OrdenTramite ordenTramite)
        {
            this.prefijo = ordenTramite.prefijo;
            this.numero = ordenTramite.numero;
            this.estado = ordenTramite.estado;
            this.vehiculo = ordenTramite.vehiculo;
            this.nit = ordenTramite.nit;
            this.contacto = ordenTramite.contacto;
            this.costo = ordenTramite.costo;
            this.iva = ordenTramite.iva;
            this.creacion = ordenTramite.creacion;
            this.finalizacion = ordenTramite.finalizacion;
        }
    }
}