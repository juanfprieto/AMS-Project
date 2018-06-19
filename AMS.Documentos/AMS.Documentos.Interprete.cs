using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Forms;

namespace AMS.Documentos
{
	public class Interprete
	{
        //private string moneda=" "+ConfigurationManager.AppSettings["MonedaNacional"];
		
		//Función que me ayuda a mirar si pongo o no la palabra mil en números de tipo millones
		protected bool Validar_Ceros(string entero)
		{
			bool todos=false;
			string ceros="";
			if(entero.Length>6 && entero.Length<10)
			{
				for(int i=entero.Length-1;i>=0;i--)
				{
					if(i==(entero.Length-1)-3 || i==(entero.Length-1)-4 || i==(entero.Length-1)-5)
					{
						if(entero[i]=='0')
							ceros+="0";
					}
				}
				if(ceros=="000")
					todos=true;
			}
			else
			{
				for(int i=entero.Length-1;i>=0;i--)
				{
					if(i==(entero.Length-1)-3 || i==(entero.Length-1)-4 || i==(entero.Length-1)-5)
					{
						if(entero[i]=='0')
							ceros+="0";
					}
				}
				if(ceros=="000")
					todos=true;
				else
					todos=false;
			}
			return todos;
		}
		
		public string Letras(string numero)
		{
			Strings str=new Strings();
			string palabras="", entero="", dec="", flag="", centavos="", moneda=""; 
			string letras;
			int num,x,y;
			flag="N";
			double cero=0;
			//Número Negativo
			if(str.Mid(numero,0,1) == "-")
			{ 
				numero = str.Mid(numero, 1, numero.ToString().Length - 1).ToString();
				palabras = "menos ";
			}
			cero=Convert.ToDouble(numero);
			if(cero==0) 
				letras="CERO PESOS M/CTE";
			else
			{
				//Si tiene ceros a la izquierda
				for(x=0;x<numero.Length;x++)
				{
					if( str.Mid(numero,0,1) == "0")
					{
                        palabras = "CERO";
						numero = str.Mid(numero, 1, numero.ToString().Length - 1).Trim();
						if(numero.Length==0)
							palabras = ""; 
					}
					else
						break;
				}
				//Dividir parte entera y decimal
				for(y=0;y<numero.Length;y++)
				{ 
					if(str.Mid(numero,y,1) == ".")
						flag="S";
					else
					{
						if(flag=="N")
							entero = entero + str.Mid(numero,y,1);
						else
							dec = dec + str.Mid(numero,y,1);
					}
				}
				if(dec.Length==1)
					dec+="0"; 
				//Aqui comienza la conversión de los decimales
				if(dec!="")
				{
					for(y=0;y<dec.Length;y++)
					{
						//Asigno palabras a las decenas 
						if(y==0)
						{
							switch(dec[y])
							{
								case '1':
									if(dec[y+1]=='0') 
										centavos+="diez";
									else
									{
										if(dec[y+1]=='1') 
											centavos+="once";
										else if(dec[y+1]=='2')
											centavos+="doce"; 
										else if(dec[y+1]=='3')
											centavos+="trece";
										else if(dec[y+1]=='4') 
											centavos+="catorce";
										else if(dec[y+1]=='5')
											centavos+="quince"; 
										else if(Convert.ToInt32(dec[y+1])>5)
											centavos+="dieci";
									}
									break; 
								case '2':
									if(dec[y+1]=='0')
										centavos+="veinte";
									else if( Convert.ToInt32(dec[y+1])!=0)
										centavos+="veinti";
									break;
								case '3':
									if(dec[y+1]=='0') 
										centavos+="treinta";
									else if(Convert.ToInt32(dec[y+1])!=0)
										centavos+="treinta y "; 
									break;
								case '4':
									if(dec[y+1]=='0')
										centavos+="cuarenta"; 
									else if(Convert.ToInt32(dec[y+1])!=0)
										centavos+="cuarenta y ";
									break;
								case '5': 
									if(dec[y+1]=='0')
										centavos+="cincuenta";
									else if(Convert.ToInt32(dec[y+1])!=0)
										centavos+="cincuenta y ";
									break;
								case '6':
									if(dec[y+1]=='0') 
										centavos+="sesenta";
									else if(Convert.ToInt32(dec[y+1])!=0)
										centavos+="sesenta y "; 
									break;
								case '7':
									if(dec[y+1]=='0')
										centavos+="setenta"; 
									else if(Convert.ToInt32(dec[y+1])!=0)
										centavos+="setenta y ";
									break;
								case '8': 
									if(dec[y+1]=='0')
										centavos+="ochenta";
									else if(Convert.ToInt32(dec[y+1])!=0)
										centavos+="ochenta y "; 
									break;
								case '9':
									if(dec[y+1]=='0')
										centavos+="noventa"; 
									else if(Convert.ToInt32(dec[y+1])!=0)
										centavos+="noventa y ";
									break;
								case '0': 
									if(dec[y+1]=='0')
										centavos+="cero";
									break;
							}
						} 
							//Asigno palabras a las unidades
						else if(y==1)
						{
							switch(dec[y])
							{
								case '1': 
									centavos+="un";
									break;
								case '2':
									centavos+="dos"; 
									break;
								case '3':
									centavos+="tres";
									break;
								case '4': 
									centavos+="cuatro";
									break;
								case '5':
									centavos+="cinco"; 
									break;
								case '6':
									centavos+="seis";
									break;
								case '7': 
									centavos+="siete";
									break;
								case '8':
									centavos+="ocho"; 
									break;
								case '9':
									centavos+="nueve";
									break;
							} 
						}
					}
				}
				//Aqui comienza la conversión real
				flag="N";
				if(DatasToControls.ValidarDouble(numero)) 
				{
					if(Convert.ToDouble(numero)<=999999999999.99)
					{
						for(y=entero.Length-1;y>=0;y--)
						{
							num = ( entero.Length) - (y+1);
							if((y+1)==3 || (y+1)==6 || (y+1)==9 || (y+1)==12)
							{
								//Aqui asigna palabras para las centenas
								switch( str.Mid(entero,num,1))
								{
									case "1":
										if(str.Mid(entero, num + 1, 1) == "0" && str.Mid(entero, num + 2, 1) == "0")
											palabras = palabras + "cien ";
										else
											palabras = palabras + "ciento "; 
										break;
									case "2":
										palabras = palabras + "doscientos ";
										break; 
									case "3":
										palabras = palabras + "trescientos ";
										break;
									case "4": 
										palabras = palabras + "cuatrocientos ";
										break;
									case "5":
										palabras = palabras + "quinientos "; 
										break;
									case "6":
										palabras = palabras + "seiscientos ";
										break; 
									case "7":
										palabras = palabras + "setecientos ";
										break;
									case "8": 
										palabras = palabras + "ochocientos ";
										break;
									case "9":
										palabras = palabras + "novecientos "; 
										break;
								}
							}
							else if((y+1)==2 || (y+1)==5 || (y+1)==8 || (y+1)==11)
							{ 
								//Asigna palabras para las decenas
								switch(str.Mid(entero,num,1))
								{
									case "0":
										flag="N";
										break;
									case "1": 
										if(str.Mid(entero, num + 1, 1) == "0")
										{
											flag = "S";
											palabras = palabras + "diez "; 
										}
										if(str.Mid(entero, num + 1, 1) == "1")
										{
											flag = "S"; 
											palabras = palabras + "once ";
										}
										if(str.Mid(entero, num + 1, 1) == "2") 
										{
											flag = "S";
											palabras = palabras + "doce ";
										} 
										if(str.Mid(entero, num + 1, 1) == "3")
										{
											flag = "S";
											palabras = palabras + "trece "; 
										}
										if(str.Mid(entero, num + 1, 1) == "4")
										{
											flag = "S"; 
											palabras = palabras + "catorce ";
										}
										if(str.Mid(entero, num + 1, 1) == "5") 
										{
											flag = "S";
											palabras = palabras + "quince ";
										} 
										if(Convert.ToInt32(str.Mid(entero, num + 1, 1)) > 5)
										{
											flag = "N";
											palabras = palabras + "dieci"; 
										}
										break;
									case "2":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "veinte ";
										} 
										else
										{    
											flag = "N";
											palabras = palabras + "veinti "; 
										}
										break;
									case "3":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "treinta ";
										} 
										else
										{
											flag = "N";
											palabras = palabras + "treinta y "; 
										}
										break;
									case "4":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "cuarenta ";
										} 
										else
										{
											flag = "N";
											palabras = palabras + "cuarenta y "; 
										}
										break;
									case "5":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "cincuenta ";
										} 
										else
										{
											flag = "N";
											palabras = palabras + "cincuenta y "; 
										}
										break;
									case "6":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "sesenta ";
										} 
										else
										{
											flag = "N";
											palabras = palabras + "sesenta y "; 
										}
										break;
									case "7":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "setenta ";
										} 
										else
										{
											flag = "N";
											palabras = palabras + "setenta y "; 
										}
										break;
									case "8":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "ochenta ";
										} 
										else
										{
											flag = "N";
											palabras = palabras + "ochenta y "; 
										}
										break;
									case "9":
										if(str.Mid(entero, num + 1, 1) == "0") 
										{
											flag = "S";
											palabras = palabras + "noventa ";
										} 
										else
										{
											flag = "N";
											palabras = palabras + "noventa y "; 
										}
										break;
								}
							}
							else if((y+1)==1 || (y+1)==4 || (y+1)==7 || (y+1)==10) 
							{
								//Asigna palabras a las unidades
								switch(str.Mid(entero, num, 1))
								{
									case "1": 
										if(flag == "N")
										{
											if((y+1) == 1)
												palabras = palabras + "uno "; 
											else
												palabras = palabras + "un ";
										}
										break; 
									case "2":
										if(flag == "N") palabras = palabras + "dos ";
										break;
									case "3":
										if(flag == "N") palabras = palabras + "tres ";
										break;
									case "4": 
										if(flag == "N") palabras = palabras + "cuatro ";
										break;
									case "5": 
										if(flag == "N") palabras = palabras + "cinco ";
										break;
									case "6":
										if(flag == "N") palabras = palabras + "seis ";
										break;
									case "7":
										if(flag == "N") palabras = palabras + "siete "; 
										break;
									case "8":
										if(flag == "N") palabras = palabras + "ocho ";
										break;
									case "9":
										if(flag == "N") palabras = palabras + "nueve ";
										break; 
								}
							}
							//Ponemos la palabra mil
							if((y+1) == 4)
							{
								if(!Validar_Ceros(entero)) 
									//if(str.Mid(entero, 3, 1) !="0" || (str.Mid(entero, 3, 1) == "0" && entero.Length <= 6) || (entero.Length>6 && str.Mid(entero,3,1)=="0") || !Validar_Ceros(entero)) 
									palabras = palabras + "mil ";
							}
							//Ponemos la palabra  millón
							if((y+1) == 7)
							{
								if(entero.Length == 7 && str.Mid(entero, 0, 1) == "1")
									palabras = palabras + "millón ";
								else
									palabras = palabras + "millones ";
							}
							if((y+1)==10)
								palabras = palabras + "mil "; 
						}
                        if (palabras.LastIndexOf("mill") == palabras.Length-9 || //tertmina con "millones"
                            palabras.LastIndexOf("mill") == palabras.Length-7) // termina con "millón"
                            moneda = " DE " + ConfigurationManager.AppSettings["MonedaNacional"];
                        else
                            moneda = " " + ConfigurationManager.AppSettings["MonedaNacional"];
                        
						//Uno la parte entera y la parte decimal
						if(dec != "")
							letras = (palabras + moneda + " con " + centavos + " centavos M/CTE").ToUpper(); 
						else
							letras = (palabras + moneda + " M/CTE").ToUpper();
					}
					else
						letras = ""; 
				}
				else
					letras="";
			}
			return letras;
		}


	}
}
