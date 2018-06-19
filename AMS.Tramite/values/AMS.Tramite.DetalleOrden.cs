using System;
using System.Collections.Generic;
namespace AMS.Tramite
{
    public class DetalleOrden
    {
        private int secuencia;
        private Tramite tramite;
        private Tramitador tramitador;
        private OficinaTransito oficina;
        private EstadoTramite estado;
        private float valorTramite;
        private float costoGestion;
        private DateTime ordenamiento;
        private DateTime cumplimiento;
        private List<Anticipo> anticipos;
        private List<DocumentoRecibido> documentosRecibidos;
        private List<DocumentoEntregado> documentosEntregados;

        public int Secuencia
        {
            get { return secuencia; }
            set { secuencia = value; }
        }

        public Tramite Tramite
        {
            get { return tramite; }
            set { tramite = value; }
        }

        public Tramitador Tramitador
        {
            get { return tramitador; }
            set { tramitador = value; }
        }

        public OficinaTransito Oficina
        {
            get { return oficina; }
            set { oficina = value; }
        }

        public EstadoTramite Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public float ValorTramite
        {
            get { return valorTramite; }
            set { valorTramite = value; }
        }

        public float CostoGestion
        {
            get { return costoGestion; }
            set { costoGestion = value; }
        }

        public DateTime Ordenamiento
        {
            get { return ordenamiento; }
            set { ordenamiento = value; }
        }

        public DateTime Cumplimiento
        {
            get { return cumplimiento; }
            set { cumplimiento = value; }
        }

        public ICollection<Anticipo> Anticipos
        {
            get { return anticipos; }
            set { anticipos = (List<Anticipo>)value; }
        }

        public ICollection<DocumentoRecibido> DocumentosRecibidos
        {
            get { return documentosRecibidos; }
            set { documentosRecibidos = (List<DocumentoRecibido>)value; }
        }

        public ICollection<DocumentoEntregado> DocumentosEntregados
        {
            get { return documentosEntregados; }
            set { documentosEntregados = (List<DocumentoEntregado>)value; }
        }

        public DetalleOrden()
        {
            this.secuencia = -1;
            this.tramite = null;
            this.tramitador = null;
            this.oficina = null;
            this.estado = null;
            this.valorTramite = -1;
            this.costoGestion = -1;
            this.ordenamiento = new DateTime();
            this.cumplimiento = new DateTime();
            this.anticipos = null;
            this.documentosRecibidos = null;
            this.documentosEntregados = null;
        }

        public DetalleOrden(
            int secuencia,
            Tramite tramite,
            Tramitador tramitador,
            OficinaTransito oficina,
            EstadoTramite estado,
            float valorTramite,
            float costoGestion,
            DateTime ordenamiento,
            DateTime cumplimiento,
            ICollection<Anticipo> anticipos,
            ICollection<DocumentoRecibido> documentosRecibidos,
            ICollection<DocumentoEntregado> documentosEntregados
        )
        {
            this.secuencia = secuencia;
            this.tramite = tramite;
            this.tramitador = tramitador;
            this.oficina = oficina;
            this.estado = estado;
            this.valorTramite = valorTramite;
            this.costoGestion = costoGestion;
            this.ordenamiento = ordenamiento;
            this.cumplimiento = cumplimiento;
            this.anticipos = new List<Anticipo>(anticipos);
            this.documentosRecibidos = new List<DocumentoRecibido>(documentosRecibidos);
            this.documentosEntregados = new List<DocumentoEntregado>(documentosEntregados);
        }

        public DetalleOrden(DetalleOrden detalleOrden)
        {
            this.secuencia = detalleOrden.secuencia;
            this.tramite = detalleOrden.tramite;
            this.tramite = detalleOrden.tramite;
            this.tramitador = detalleOrden.tramitador;
            this.oficina = detalleOrden.oficina;
            this.estado = detalleOrden.estado;
            this.valorTramite = detalleOrden.valorTramite;
            this.costoGestion = detalleOrden.costoGestion;
            this.ordenamiento = detalleOrden.ordenamiento;
            this.cumplimiento = detalleOrden.cumplimiento;
            this.anticipos = new List<Anticipo>(detalleOrden.anticipos);
            this.documentosRecibidos = new List<DocumentoRecibido>(detalleOrden.documentosRecibidos);
            this.documentosEntregados = new List<DocumentoEntregado>(detalleOrden.documentosEntregados);
        }
    }
}