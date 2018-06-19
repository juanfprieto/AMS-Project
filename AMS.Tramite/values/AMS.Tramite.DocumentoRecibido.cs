using System;
namespace AMS.Tramite
{
    public class DocumentoRecibido
    {

        private ParametroDocumentoVehiculo documento;
        private string nombreRecibe;
        private string nombreEntrega;
        private TipoEntregable tipo;
        private string usuario;
        private DateTime fecha;

        public ParametroDocumentoVehiculo Documento
        {
            get { return documento; }
            set { documento = value; }
        }

        public string NombreRecibe
        {
            get { return nombreRecibe; }
            set { nombreRecibe = value; }
        }

        public string NombreEntrega
        {
            get { return nombreEntrega; }
            set { nombreEntrega = value; }
        }

        public TipoEntregable Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public DocumentoRecibido()
        {
            this.documento = null;
            this.nombreRecibe = null;
            this.nombreEntrega = null;
            this.tipo = null;
            this.usuario = null;
            this.fecha = new DateTime();
        }

        public DocumentoRecibido(
            ParametroDocumentoVehiculo documento,
            string nombreRecibe,
            string nombreEntrega,
            TipoEntregable tipo,
            string usuario,
            DateTime fecha
        )
        {
            this.documento = documento;
            this.nombreRecibe = nombreRecibe;
            this.nombreEntrega = nombreEntrega;
            this.tipo = tipo;
            this.usuario = usuario;
            this.fecha = fecha;
        }

        public DocumentoRecibido(DocumentoRecibido documentoRecibido)
        {
            this.documento = documentoRecibido.documento;
            this.nombreRecibe = documentoRecibido.nombreRecibe;
            this.nombreEntrega = documentoRecibido.nombreEntrega;
            this.tipo = documentoRecibido.tipo;
            this.usuario = documentoRecibido.usuario;
            this.fecha = documentoRecibido.fecha;
        }
    }
}