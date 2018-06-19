using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;

namespace AMS.Documentos
{
	public class Strings
	{
		public string Mid(string s,int offset,int numcharstoread)
		{
			/*int j=0;
 			for (int i=offset ; i < numcharstoread ; i++)
 			{
  				char ch=s[i];
  				s1[j]=ch;
  				j++;
			}
 			return s1;*/
			//if(s.Length<numcharstoread+offset) return(s);
			string valor=s.Substring(offset,numcharstoread);
			return valor;
		}
	}
}

