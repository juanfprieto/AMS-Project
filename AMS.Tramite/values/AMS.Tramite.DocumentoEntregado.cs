using System;
namespace AMS.Tramite
{
    public class DocumentoEntregado
    {

        private DocumentoVehiculo documento;
        private string nombreRecibe;
        private string nombreEntrega;
        private TipoEntregable tipo;
        private string usuario;
        private DateTime fecha;

        public DocumentoVehiculo Documento
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

        public DocumentoEntregado()
        {
            this.documento = null;
            this.nombreRecibe = null;
            this.nombreEntrega = null;
            this.tipo = null;
            this.usuario = null;
            this.fecha = new DateTime();
        }

        public DocumentoEntregado(
            DocumentoVehiculo documento,
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

        public DocumentoEntregado(DocumentoEntregado documentoEntregado)
        {
            this.documento = documentoEntregado.documento;
            this.nombreRecibe = documentoEntregado.nombreRecibe;
            this.nombreEntrega = documentoEntregado.nombreEntrega;
            this.tipo = documentoEntregado.tipo;
            this.usuario = documentoEntregado.usuario;
            this.fecha = documentoEntregado.fecha;
        }
    }
}