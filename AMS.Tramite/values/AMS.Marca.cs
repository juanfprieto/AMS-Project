using System;
namespace AMS.Tramite
{
    public class Marca
    {
        private string codigo;
        private string nombre;
        private string distribuidor;

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

        public string Distribuidor
        {
          get { return distribuidor; }
          set { distribuidor = value; }
        }



        public Marca()
        {
            this.codigo = null;
            this.nombre = null;
            this.distribuidor = null;
        }

        public Marca(
            string codigo,
            string nombre,
            string distribuidor
        )
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.distribuidor = distribuidor;
        }

        public Marca(Marca marca)
        {
            this.codigo = marca.codigo;
            this.nombre = marca.nombre;
            this.distribuidor = marca.distribuidor;
        }
    }
}