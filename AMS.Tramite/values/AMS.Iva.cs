using System;
namespace AMS.Tramite
{
    public class Iva
    {

        private float porcentaje;
        private string decreto;
        private DateTime expedicion;

        public float Porcentaje
        {
          get { return porcentaje; }
          set { porcentaje = value; }
        }

        public string Decreto
        {
          get { return decreto; }
          set { decreto = value; }
        }

        public DateTime Expedicion
        {
          get { return expedicion; }
          set { expedicion = value; }
        }

        public Iva()
        {
            this.porcentaje = -1;
            this.decreto = null;
            this.expedicion = new DateTime();
        }

        public Iva(
            float porcentaje,
            string decreto,
            DateTime expedicion
        )
        {
            this.porcentaje = porcentaje;
            this.decreto = decreto;
            this.expedicion = expedicion;
        }

        public Iva(Iva iva)
        {
            this.porcentaje = iva.porcentaje;
            this.decreto = iva.decreto;
            this.expedicion = iva.expedicion;
        }
    }
}