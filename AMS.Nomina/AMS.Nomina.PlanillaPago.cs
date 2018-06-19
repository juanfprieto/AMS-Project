using System;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class PlanillaPago
	{
				
		public PlanillaPago()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}

	    public string  Completar_Campos(string cadena,int longitud,string relleno,bool justificacion)
		{
			int cont;
			int cadenalong=cadena.Length;
			int i=0;
			//si la cadena es menor habra que completar con el relleno q nos manden.
			if (cadenalong<longitud)
			{
				//si la justificacion es a la izquierda completo hacia la derecha.
				cont=longitud-cadenalong;
				if (justificacion==true)
				{
					for (i=0;i<cont;i++)
					{
						cadena+=relleno;
					}
				}
					
				//si la justificacion es a la derecha completo hacia la izquierda.
				if (justificacion==false)
				{
					string cadena2=cadena;
					string rellenoini="";
					for (i=0;i<cont;i++)
					{
						rellenoini+=relleno;
					}
					cadena="";
					cadena=rellenoini+cadena2;
				}
					
			}
			//si la cadena es mayor habra que truncar.
			if (cadenalong>longitud)
			{
				string cadena3="";
				for (i=0;i<longitud;i++)
				{
					cadena3+=cadena[i].ToString();
				}
				//Response.Write("<script language:javascript>alert('"+cadena3+" mas larga q los espacios');</script>");			
				cadena=cadena3;
			}
			
			return cadena;
		}
		

	}
}
