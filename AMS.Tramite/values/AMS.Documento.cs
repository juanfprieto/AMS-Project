using System;
namespace AMS.Tramite
{
    public class Documento
    {
        private string codigo;
        private string nombre;
        private string descripcion;
        private TipoDocumento tipo;
        private string resolucion;
        private DateTime fechaResolucion;
        private DateTime vencimientoResolucion;
        private string habilitacion;
        private DateTime fechaHabilitacion;
        private int numInicial;
        private int numFinal;
        private int ultimoDocu;
        private string formato1;
        private string observaciones;
        private TipoVigencia vigencia;
        private string formato2;
        private bool esConsultaGerencial;

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

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public TipoDocumento Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public string Resolucion
        {
            get { return resolucion; }
            set { resolucion = value; }
        }

        public DateTime FechaResolucion
        {
            get { return fechaResolucion; }
            set { fechaResolucion = value; }
        }

        public DateTime VencimientoResolucion
        {
            get { return vencimientoResolucion; }
            set { vencimientoResolucion = value; }
        }

        public string Habilitacion
        {
            get { return habilitacion; }
            set { habilitacion = value; }
        }

        public DateTime FechaHabilitacion
        {
            get { return fechaHabilitacion; }
            set { fechaHabilitacion = value; }
        }

        public int NumInicial
        {
            get { return numInicial; }
            set { numInicial = value; }
        }

        public int NumFinal
        {
            get { return numFinal; }
            set { numFinal = value; }
        }

        public int UltimoDocu
        {
            get { return ultimoDocu; }
            set { ultimoDocu = value; }
        }

        public string Formato1
        {
            get { return formato1; }
            set { formato1 = value; }
        }

        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }

        public TipoVigencia Vigencia
        {
            get { return vigencia; }
            set { vigencia = value; }
        }

        public string Formato2
        {
            get { return formato2; }
            set { formato2 = value; }
        }

        public bool EsConsultaGerencial
        {
            get { return esConsultaGerencial; }
            set { esConsultaGerencial = value; }
        }

        public Documento()
        {
            this.codigo = null;
            this.nombre = null;
            this.descripcion = null;
            this.tipo = null;
            this.resolucion = null;
            this.fechaResolucion = new DateTime();
            this.vencimientoResolucion = new DateTime();
            this.habilitacion = null;
            this.fechaHabilitacion = new DateTime();
            this.numInicial = -1;
            this.numFinal = -1;
            this.ultimoDocu = -1;
            this.formato1 = null;
            this.observaciones = null;
            this.vigencia = null;
            this.formato2 = null;
            this.esConsultaGerencial = false;
        }

        public Documento(
            string codigo,
            string nombre,
            string descripcion,
            TipoDocumento tipo,
            string resolucion,
            DateTime fechaResolucion,
            DateTime vencimientoResolucion,
            string habilitacion,
            DateTime fechaHabilitacion,
            int numInicial,
            int numFinal,
            int ultimoDocu,
            string formato1,
            string observaciones,
            TipoVigencia vigencia,
            string formato2,
            bool esConsultaGerencial
        )
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.resolucion = resolucion;
            this.fechaResolucion = fechaResolucion;
            this.vencimientoResolucion = vencimientoResolucion;
            this.habilitacion = habilitacion;
            this.fechaHabilitacion = fechaHabilitacion;
            this.numInicial = numInicial;
            this.numFinal = numFinal;
            this.ultimoDocu = ultimoDocu;
            this.formato1 = formato1;
            this.observaciones = observaciones;
            this.vigencia = vigencia;
            this.formato2 = formato2;
            this.esConsultaGerencial = esConsultaGerencial;
        }

        public Documento(Documento documento)
        {
            this.codigo = documento.codigo;
            this.nombre = documento.nombre;
            this.descripcion = documento.descripcion;
            this.tipo = documento.tipo;
            this.resolucion = documento.resolucion;
            this.fechaResolucion = documento.fechaResolucion;
            this.vencimientoResolucion = documento.vencimientoResolucion;
            this.habilitacion = documento.habilitacion;
            this.fechaHabilitacion = documento.fechaHabilitacion;
            this.numInicial = documento.numInicial;
            this.numFinal = documento.numFinal;
            this.ultimoDocu = documento.ultimoDocu;
            this.formato1 = documento.formato1;
            this.observaciones = documento.observaciones;
            this.vigencia = documento.vigencia;
            this.formato2 = documento.formato2;
            this.esConsultaGerencial = documento.esConsultaGerencial;
        }
    }
}