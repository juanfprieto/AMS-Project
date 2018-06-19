using System;
using System.Collections;

namespace AMS.Utilidades
{
	public class ManejoArrayList
	{
		public static void InsertarArrayListEnArrayList(ArrayList origen, ArrayList destino)
		{
			foreach(object objeto in origen)
				destino.Add(objeto);
		}
	}
}
