using System;
namespace AMS.Tramite
{
    public class Tramitador
    {

        private int codigo;
        private Nit nit;
        private TipoTramitador tipo;
        private DateTime fechaIngreso;
        private DateTime fechaRetiro;

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        public Nit Nit
        {
            get { return nit; }
            set { nit = value; }
        }
        public TipoTramitador Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public DateTime FechaIngreso
        {
            get { return fechaIngreso; }
            set { fechaIngreso = value; }
        }

        public DateTime FechaRetiro
        {
            get { return fechaRetiro; }
            set { fechaRetiro = value; }
        }

        public Tramitador()
        {
            this.codigo = -1;
            this.nit = null;
            this.tipo = null;
            this.fechaIngreso = new DateTime();
            this.fechaRetiro = new DateTime();
        }

        public Tramitador(
            int codigo,
            Nit nit,
            TipoTramitador tipo,
            DateTime fechaIngreso,
            DateTime fechaRetiro
        )
        {
            this.codigo = codigo;
            this.nit = nit;
            this.tipo = tipo;
            this.fechaIngreso = fechaIngreso;
            this.fechaRetiro = fechaRetiro;
        }

        public Tramitador(Tramitador tramitador)
        {
            this.codigo = tramitador.codigo;
            this.nit = tramitador.nit;
            this.tipo = tramitador.tipo;
            this.fechaIngreso = tramitador.fechaIngreso;
            this.fechaRetiro = tramitador.fechaRetiro;
        }
    }
}