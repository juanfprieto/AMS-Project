using System;
using AMS.DB;

namespace AMS.Inventarios
{
	public class Ubicacion
	{
		/// <summary>
		///	Metodo que me permite determinar que nivel tiene la ubicación que vamos a configurar.
		/// </summary>
		/// <param name="codUbi">Código de la Ubicación</param>
		/// <returns>Nivel de la Ubicación</returns>
		public static int DeterminarNivel(string codUbi)
		{
			int nivel = 0;

			if(DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+"") == "")
				nivel = 1;
			else if(DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+")") == "")
				nivel = 2;
			else if(DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+")") != "")
				nivel = 3;

			return nivel;
		}
	}

	public class UbicacionItem
	{
		public static bool ExisteUbicacionItem(int pubi_codigo, string mite_codigo)
		{
			return DBFunctions.RecordExist("Select * from dbxschema.mubicacionitem where pubi_codigo = "+pubi_codigo.ToString()+" AND mite_codigo = '"+mite_codigo+"'");
		}

		public static bool CambiarUbicacionItem(int pubi_codigo, string mite_codigo)
		{
			bool valido = false;

			if (!ExisteUbicacionItem(pubi_codigo,mite_codigo) && pubi_codigo != 0)
				valido = (DBFunctions.NonQuery("INSERT INTO mubicacionitem (pubi_codigo, mite_codigo) VALUES("+pubi_codigo.ToString()+",'"+mite_codigo+"')") == 1);

			return valido;
		}
	}
}