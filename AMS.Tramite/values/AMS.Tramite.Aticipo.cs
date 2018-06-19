using System;
namespace AMS.Tramite
{
    public class Anticipo
    {

        private int codigo;
        private double valor;
        private string usuario; //deberia se una entidad, por simplicidad se guarda el login
        private DateTime fecha;

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public double Valor
        {
            get { return valor; }
            set { valor = value; }
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


        public Anticipo()
        {
            this.codigo = -1;
            this.valor = -1;
            this.usuario = null;
            this.fecha = new DateTime();
        }

        public Anticipo(
            int codigo,
            double valor,
            string usuario,
            DateTime fecha
           )
        {
            this.codigo = codigo;
            this.valor = valor;
            this.usuario = usuario;
            this.fecha = fecha;
        }

        public Anticipo(Anticipo anticipo)
        {
            this.codigo = anticipo.codigo;
            this.valor = anticipo.valor;
            this.usuario = anticipo.usuario;
            this.fecha = anticipo.fecha;
        }
    }
}