using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;

namespace AMS.Web
{
	public partial class PrintBox : System.Web.UI.Page
	{
			
		public PrintBox()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string codigoFormato = Request.QueryString["codForm"];
			string prefijoDocumento = Request.QueryString["prefDocu"];
			string numeroDocumento = Request.QueryString["numeDocu"];
			string paramLines = Construccion_Valor_Header(codigoFormato, prefijoDocumento,numeroDocumento);
			paramLines += "*";
			paramLines += Construccion_Valor_Body(codigoFormato, prefijoDocumento,numeroDocumento);
			paramLines += "*";
			paramLines += Construccion_Valor_Footer(codigoFormato, prefijoDocumento,numeroDocumento);
			plcObjCOM.Controls.Add(new LiteralControl("<SCRIPT LANGUAGE='javascript' FOR='container' EVENT='OnFin()'>"));
			plcObjCOM.Controls.Add(new LiteralControl("window.close();"));
			plcObjCOM.Controls.Add(new LiteralControl("</SCRIPT>"));
			//plcObjCOM.Controls.Add(new LiteralControl("<object id=\"container\" classid=\"http://"+Server.MachineName+"/libreria/AMS.Print.dll#AMS.Print.UIPrinter\" height=\"150\" width=\"230\">"));
			plcObjCOM.Controls.Add(new LiteralControl("<object id=\"container\" classid=\"http://"+Server.MachineName+"/libreria/AMS.Print.dll#AMS.Print.UIPrinter\" height=\"700\" width=\"1000\">"));
			plcObjCOM.Controls.Add(new LiteralControl("<param name =\"ParamLines\" value=\""+paramLines+"\">"));
			plcObjCOM.Controls.Add(new LiteralControl("</object>"));
		}
		
		private string Construccion_Valor_Header(string codigoFormato, string prefijoDocumento, string numeroDocumento)
		{
			//Ahora traemos los elementos de este formato correspondiente a este documento
			int i,j;
			DataSet infoFormato = new DataSet();
			DBFunctions.Request(infoFormato,IncludeSchema.NO,"SELECT stfc_codigo, sfpf_posidx, sfpf_posidy, sfpf_posfdx, sfpf_posfdy FROM sformatopreimpresofactura WHERE sfpf_codigo="+codigoFormato+"");
			//  												          0			      1			2			3				4
			//Ahora traemos los elementos que se configuraron para este formato preimpreso
			DataSet elemFormato = new DataSet();
			DBFunctions.Request(elemFormato,IncludeSchema.NO,"SELECT staf_nombretabla, stfc_codigo, tpar_codigo, sdff_campo, sdff_posicionx, sdff_posiciony, sdff_mascara, sdff_etiqueta, sdff_ordencolumna, sdff_resulsuma FROM sdetalleformatopreimpfact WHERE sfpf_codigo="+codigoFormato+"");
			//															  0				  1			   2			  3			4               5                   6            7              8                 9                                        
			//Ahora construimos los valores y los almacenamos en un string que enviamos al objeto COM
			//el formato de este string sera separador dentro de linea (-) y separador de lineas *
			string resultado = "";
			//Primero construimos los valores asociados al header del documento
			DataRow[] selectionHeader = elemFormato.Tables[0].Select("TPAR_CODIGO='H'");
			for(i=0;i<selectionHeader.Length;i++)
			{
				string resultadoSelect = "";
				//Debemos traer la forma de acceder a esta tabla y que tablas relacionadas tiene
				string condicional = DBFunctions.SingleData("SELECT staf_condicional FROM stablaasociadafactura WHERE staf_nombretabla='"+selectionHeader[i][0].ToString()+"' AND stfc_codigo='"+selectionHeader[i][1].ToString()+"'");
				string[] tablas = DBFunctions.SingleData("SELECT staf_tabquery FROM stablaasociadafactura WHERE staf_nombretabla='"+selectionHeader[i][0].ToString()+"' AND stfc_codigo='"+selectionHeader[i][1].ToString()+"'").Trim().Split('-');
				//ahora vamos a construir el select que nos trae los datos ya relacionados de la fatcura en el header
				string select = "";
				string tipoDato = DBFunctions.SingleData("SELECT typename FROM sysibm.syscolumns WHERE name='"+selectionHeader[i][3].ToString()+"' AND tbname='"+selectionHeader[i][0].ToString()+"'");
				string llaveFK = Determinar_Llave(selectionHeader[i][0].ToString(),selectionHeader[i][3].ToString());
				bool elementoCompuesto = Determinar_Elemento_Compuesto(selectionHeader[i][0].ToString(),selectionHeader[i][3].ToString(),infoFormato.Tables[0].Rows[0][0].ToString());
				if(llaveFK == "")
				{
					if(!elementoCompuesto)
						select = "SELECT "+selectionHeader[i][0].ToString()+"."+selectionHeader[i][3].ToString()+" FROM ";
					else
						select = "SELECT "+selectionHeader[i][3].ToString().Replace("@","'")+" FROM ";
				}
				else
				{
					//es porque hace parte de la llave foranea
					string[] sepFK = llaveFK.Split('-');
					if(sepFK[0]!="PDOCUMENTO")
						select = "SELECT "+sepFK[0]+"."+DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbname='"+sepFK[0]+"' AND colno=1")+" FROM ";
					else
						select = "SELECT "+selectionHeader[i][0].ToString()+"."+selectionHeader[i][3].ToString()+" FROM ";
				}
				//ahora asociamos las tablas
				if(llaveFK!="")
				{
					string[] sepFK = llaveFK.Split('-');
					if(sepFK[0]!="PDOCUMENTO")
						select += sepFK[0]+",";
				}
				for(j=0;j<tablas.Length;j++)
					select += tablas[j]+",";				
				select = select.Substring(0,select.Length-1);
				//Ahora necesitamos reemplazar los valores del prefijodocumento y del numerodocumento en la sentencia select
				for(j=0;j<2;j++)
				{
					string sql1 = condicional.Substring(0,condicional.IndexOf('@'));
					string sql2 = condicional.Substring(condicional.IndexOf('@')+1);
					if(j==0)
						condicional = sql1+prefijoDocumento+sql2;
					else if(j==1)
						condicional = sql1+numeroDocumento+sql2;
				}
				select += " WHERE "+condicional.Replace("*","'");
				//Ahora agregamos la asociacion con la otra tabla en caso de que sea parte de la llave foranea
				if(llaveFK!="")
				{
					string[] sepFK = llaveFK.Split('-');
					if(sepFK[0]!="PDOCUMENTO")
					{
						select += " AND ";
						DataSet fkOwn = new DataSet();
						DataSet fkExt = new DataSet();
						DBFunctions.Request(fkOwn,IncludeSchema.NO,"SELECT tbname,colname FROM sysibm.syskeycoluse WHERE tbname='"+selectionHeader[i][0].ToString()+"' AND constname='"+sepFK[1]+"' ORDER BY colseq");
						DBFunctions.Request(fkExt,IncludeSchema.NO,"SELECT tbname,colname FROM sysibm.syskeycoluse WHERE tbname='"+sepFK[0]+"' AND constname='"+sepFK[2]+"' ORDER BY colseq");
						for(j=0;j<fkOwn.Tables[0].Rows.Count;j++)
							select += fkOwn.Tables[0].Rows[j][0].ToString()+"."+fkOwn.Tables[0].Rows[j][1].ToString()+"="+fkExt.Tables[0].Rows[j][0].ToString()+"."+fkExt.Tables[0].Rows[j][1].ToString()+" AND";
						select = select.Substring(0,select.Length-3);
					}
				}
				//revisamos si se necesita aplicar algun formato al resultado del select
				if(selectionHeader[i][6].ToString()!="")
				{
					if(tipoDato == "DATE")
						try{resultadoSelect = System.Convert.ToDateTime(DBFunctions.SingleData(select)).ToString(selectionHeader[i][6].ToString());}catch{}
					else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
						resultadoSelect = System.Convert.ToDouble(DBFunctions.SingleData(select)).ToString(selectionHeader[i][6].ToString());
				}
				else
					resultadoSelect = DBFunctions.SingleData(select);
				resultado += resultadoSelect+"+"+selectionHeader[i][4].ToString()+"+"+selectionHeader[i][5].ToString()+"*";
			}
			return resultado.Substring(0,resultado.Length-1);
		}
		
		private string Construccion_Valor_Body(string codigoFormato, string prefijoDocumento, string numeroDocumento)
		{
			int i,j,l;
			string resultado = "";
			DataSet infoFormato = new DataSet();
			DBFunctions.Request(infoFormato,IncludeSchema.NO,"SELECT stfc_codigo, sfpf_posidx, sfpf_posidy, sfpf_posfdx, sfpf_posfdy FROM sformatopreimpresofactura WHERE sfpf_codigo="+codigoFormato+"");
			//  												          0			      1			2			3				4
			DataSet elemFormato = new DataSet();
			DBFunctions.Request(elemFormato,IncludeSchema.NO,"SELECT staf_nombretabla, stfc_codigo, tpar_codigo, sdff_campo, sdff_posicionx, sdff_posiciony, sdff_mascara, sdff_etiqueta, sdff_ordencolumna, sdff_resulsuma, sdff_porcanch, sdff_justificacion FROM sdetalleformatopreimpfact WHERE sfpf_codigo="+codigoFormato+" AND tpar_codigo='B'");
			//															  0				  1			   2			  3			4               5                   6            7              8                 9					10				11
			//Ahora traemos las diferentes tipos de tablas que hacen parte del body del documento
			DataSet tabAsociadas = new DataSet();
			DBFunctions.Request(tabAsociadas,IncludeSchema.NO,"SELECT staf_nombretabla, staf_condicional, staf_tabquery FROM stablaasociadafactura WHERE stfc_codigo='"+infoFormato.Tables[0].Rows[0][0].ToString()+"' AND tpar_codigo='B'");
			double posIX = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][1]);
			double posIY = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][2]);
			double posFX = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][3]);
			double posFY = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][4]);
			double areaX = posFX- posIX;
			double posMovY = posIY;
			double posMovX = posIX;
			double pivotY = 0.4;
			double cantiCaracCm = 4;
			for(i=0;i<tabAsociadas.Tables[0].Rows.Count;i++)
			{
				DataRow[] selectionElementos = elemFormato.Tables[0].Select("STAF_NOMBRETABLA='"+tabAsociadas.Tables[0].Rows[i][0].ToString()+"'","SDFF_ORDENCOLUMNA ASC");
				string[] tablas = tabAsociadas.Tables[0].Rows[i][2].ToString().Split('-');
				string select = "SELECT ";
				string from = " FROM ";
				string where = " WHERE "+tabAsociadas.Tables[0].Rows[i][1].ToString().Replace("*","'");
				double[] anchoColumna = new double[selectionElementos.Length];
				for(j=0;j<2;j++)
				{
					string sql1 = where.Substring(0,where.IndexOf('@'));
					string sql2 = where.Substring(where.IndexOf('@')+1);
					if(j==0)
						where = sql1+prefijoDocumento+sql2;
					else if(j==1)
						where = sql1+numeroDocumento+sql2;
				}
				for(j=0;j<selectionElementos.Length;j++)
				{
					anchoColumna[j] = this.CalculoAnchoColumna(areaX,System.Convert.ToDouble(selectionElementos[j][10].ToString()));
					string llaveFK = this.Determinar_Llave(tabAsociadas.Tables[0].Rows[i][0].ToString(),selectionElementos[j][3].ToString());
					bool elementoCompuesto = this.Determinar_Elemento_Compuesto(tabAsociadas.Tables[0].Rows[i][0].ToString(),selectionElementos[j][3].ToString(),infoFormato.Tables[0].Rows[0][0].ToString());
					if(llaveFK == "")
					{
						if(!elementoCompuesto)
							select += tabAsociadas.Tables[0].Rows[i][0].ToString()+"."+selectionElementos[j][3].ToString()+" ,";
						else
							select += selectionElementos[j][3].ToString().Replace("@","'")+" ,";
					}
					else
					{
						string[] sepFK = llaveFK.Split('-');
						select += sepFK[0]+"."+DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbname='"+sepFK[0]+"' AND colno=1")+" ,";
						from += sepFK[0]+" ,";
						where += " AND ";
						DataSet fkOwn = new DataSet();
						DataSet fkExt = new DataSet();
						DBFunctions.Request(fkOwn,IncludeSchema.NO,"SELECT tbname,colname FROM sysibm.syskeycoluse WHERE tbname='"+tabAsociadas.Tables[0].Rows[i][0].ToString()+"' AND constname='"+sepFK[1]+"' ORDER BY colseq");
						DBFunctions.Request(fkExt,IncludeSchema.NO,"SELECT tbname,colname FROM sysibm.syskeycoluse WHERE tbname='"+sepFK[0]+"' AND constname='"+sepFK[2]+"' ORDER BY colseq");
						for(l=0;l<fkOwn.Tables[0].Rows.Count;l++)
							where += fkOwn.Tables[0].Rows[l][0].ToString()+"."+fkOwn.Tables[0].Rows[l][1].ToString()+"="+fkExt.Tables[0].Rows[l][0].ToString()+"."+fkExt.Tables[0].Rows[l][1].ToString()+" AND";
						where = where.Substring(0,where.Length-3);
					}
				}
				for(j=0;j<tablas.Length;j++)
					from += tablas[j]+",";				
				from = from.Substring(0,from.Length-1);
				select = select.Substring(0,select.Length-1);
				string query = select+from+where;
				DataSet valores = new DataSet();
				DBFunctions.Request(valores,IncludeSchema.NO,query);
				for(int k=0;k<valores.Tables[0].Rows.Count;k++)
				{
					for(int m=0;m<valores.Tables[0].Columns.Count;m++)
					{
						int cantidadCaracteres = (int)Math.Floor((anchoColumna[m]*cantiCaracCm));
						string valorEspecifico = "";
						string tipoDato = DBFunctions.SingleData("SELECT typename FROM sysibm.syscolumns WHERE name='"+selectionElementos[m][3].ToString()+"' AND tbname='"+selectionElementos[m][0].ToString()+"'");
						double cantidadCorrimiento = ((double)((double)(valores.Tables[0].Rows[k][m].ToString().Length-4))/10)/2;
						//Ahora revisamos los de las mascaras ha aplicar a estos valores del body
						if(selectionElementos[m][6].ToString()!="")
						{
							if(tipoDato == "DATE")
								valorEspecifico = System.Convert.ToDateTime(valores.Tables[0].Rows[k][m].ToString().Trim()).ToString(selectionElementos[m][6].ToString());
							else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
								valorEspecifico = System.Convert.ToDouble(valores.Tables[0].Rows[k][m].ToString().Trim()).ToString(selectionElementos[m][6].ToString());
							if((selectionElementos[m][6].ToString()=="C")&&(tipoDato==""))
								valorEspecifico = System.Convert.ToDouble(valores.Tables[0].Rows[k][m].ToString().Trim()).ToString(selectionElementos[m][6].ToString());
						}
						else
							valorEspecifico = valores.Tables[0].Rows[k][m].ToString();
						if(valorEspecifico.Length>cantidadCaracteres)
							resultado += valorEspecifico.Substring(0,cantidadCaracteres)+"+"+posMovX+"+"+(posMovY+(pivotY*k)).ToString()+"*";
						else
						{
							if(selectionElementos[m][11].ToString()=="I")
								resultado += valorEspecifico+"+"+posMovX+"+"+(posMovY+(pivotY*k)).ToString()+"*";
							else if(selectionElementos[m][11].ToString()=="D")
							{
								int diferenciaCaracteres = cantidadCaracteres - valorEspecifico.Length;
								double varPosX = diferenciaCaracteres/cantiCaracCm;
								resultado += valorEspecifico+"+"+(posMovX+varPosX+cantidadCorrimiento).ToString()+"+"+(posMovY+(pivotY*k)).ToString()+"*";
							}
						}
						posMovX += anchoColumna[m];
					}
					posMovX = posIX;
				}
				posMovY += (pivotY*(valores.Tables[0].Rows.Count+3));
				posMovX = posIX;
			}
			return resultado.Substring(0,resultado.Length-1);
		}
		
		private string Construccion_Valor_Footer(string codigoFormato, string prefijoDocumento, string numeroDocumento)
		{
			int i,j;
			string resultado = "";
			DataSet infoFormato = new DataSet();
			DBFunctions.Request(infoFormato,IncludeSchema.NO,"SELECT stfc_codigo, sfpf_posidx, sfpf_posidy, sfpf_posfdx, sfpf_posfdy FROM sformatopreimpresofactura WHERE sfpf_codigo="+codigoFormato+"");
			//  												          0			      1			2			3				4
			DataSet elemFormato = new DataSet();
			DBFunctions.Request(elemFormato,IncludeSchema.NO,"SELECT staf_nombretabla, stfc_codigo, tpar_codigo, sdff_campo, sdff_posicionx, sdff_posiciony, sdff_mascara, sdff_etiqueta, sdff_ordencolumna, sdff_resulsuma, sdff_porcanch, sdff_justificacion FROM sdetalleformatopreimpfact WHERE sfpf_codigo="+codigoFormato+" AND tpar_codigo='F'");
			//															  0				  1			   2			  3			4               5                   6            7              8                 9					10				11
			double posIX = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][1]);
			double posIY = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][2]);
			double posFX = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][3]);
			double posFY = System.Convert.ToDouble(infoFormato.Tables[0].Rows[0][4]);
			double areaX = posFX- posIX;
			double cantiCaracCm = 4;
			int cantidadCaracteres = (int)Math.Floor((areaX*cantiCaracCm));
			for(i=0;i<elemFormato.Tables[0].Rows.Count;i++)
			{
				string select = "";
				string from = " FROM ";
				string where = " WHERE ";
				string condicional = DBFunctions.SingleData("SELECT staf_condicional FROM stablaasociadafactura WHERE staf_nombretabla='"+elemFormato.Tables[0].Rows[i][0].ToString()+"' AND stfc_codigo='"+elemFormato.Tables[0].Rows[i][1].ToString()+"'");
				string[] tablas = DBFunctions.SingleData("SELECT staf_tabquery FROM stablaasociadafactura WHERE staf_nombretabla='"+elemFormato.Tables[0].Rows[i][0].ToString()+"' AND stfc_codigo='"+elemFormato.Tables[0].Rows[i][1].ToString()+"'").Trim().Split('-');
				string tipoDato = DBFunctions.SingleData("SELECT typename FROM sysibm.syscolumns WHERE name='"+elemFormato.Tables[0].Rows[i][3].ToString()+"' AND tbname='"+elemFormato.Tables[0].Rows[i][0].ToString()+"'");
				bool elementoCompuesto = this.Determinar_Elemento_Compuesto(elemFormato.Tables[0].Rows[i][0].ToString(),elemFormato.Tables[0].Rows[i][3].ToString(),infoFormato.Tables[0].Rows[0][0].ToString());
				//vamos a armar la parte del select
				if(elemFormato.Tables[0].Rows[i][9].ToString()=="N")
				{
					if(!elementoCompuesto)
						select = "SELECT "+elemFormato.Tables[0].Rows[i][0].ToString()+"."+elemFormato.Tables[0].Rows[i][3].ToString()+" ";
					else
						select = "SELECT "+elemFormato.Tables[0].Rows[i][3].ToString().Replace("@","'")+" ";
				}
				else
				{
					if(!elementoCompuesto)
						select = "SELECT SUM("+elemFormato.Tables[0].Rows[i][0].ToString()+"."+elemFormato.Tables[0].Rows[i][3].ToString()+") ";
					else
						select = "SELECT SUM("+elemFormato.Tables[0].Rows[i][3].ToString().Replace("@","'")+") ";
				}
				//ahora construimos el from
				for(j=0;j<tablas.Length;j++)
					from += tablas[j]+",";				
				from = from.Substring(0,from.Length-1);
				//Ahora debemos construir el condicional del where
				for(j=0;j<2;j++)
				{
					string sql1 = condicional.Substring(0,condicional.IndexOf('@'));
					string sql2 = condicional.Substring(condicional.IndexOf('@')+1);
					if(j==0)
						condicional = sql1+prefijoDocumento+sql2;
					else if(j==1)
						condicional = sql1+numeroDocumento+sql2;
				}
				where += condicional.Replace("*","'");
				string resultadoSelect = DBFunctions.SingleData(select+from+where);
				double cantidadCorrimiento = ((double)((double)(resultadoSelect.Length-4))/10)/2;
				if(elemFormato.Tables[0].Rows[i][6].ToString()!="")
				{
					if(tipoDato == "DATE")
						try{resultadoSelect = System.Convert.ToDateTime(resultadoSelect).ToString(elemFormato.Tables[0].Rows[i][6].ToString());}catch{}
					else if(tipoDato == "BIGINT" || tipoDato == "DECIMAL" || tipoDato == "DOUBLE" || tipoDato == "INTEGER" || tipoDato == "REAL" || tipoDato == "SMALLINT")
						try{resultadoSelect = System.Convert.ToDouble(resultadoSelect.Trim()).ToString(elemFormato.Tables[0].Rows[i][6].ToString());}catch{}
					if((elemFormato.Tables[0].Rows[i][6].ToString()=="C") && (tipoDato == ""))
						try{resultadoSelect = System.Convert.ToDouble(resultadoSelect.Trim()).ToString(elemFormato.Tables[0].Rows[i][6].ToString());}catch{}
				}
				resultado += elemFormato.Tables[0].Rows[i][7].ToString()+"+"+elemFormato.Tables[0].Rows[i][4].ToString()+"+"+elemFormato.Tables[0].Rows[i][5].ToString()+"*";
				double cantidadCmMov = (System.Convert.ToDouble(elemFormato.Tables[0].Rows[i][7].ToString().Length))/((double)cantiCaracCm);
				if(elemFormato.Tables[0].Rows[i][11].ToString()=="I")
					resultado += resultadoSelect +"+"+(System.Convert.ToDouble(elemFormato.Tables[0].Rows[i][4].ToString())+cantidadCmMov+2).ToString()+"+"+elemFormato.Tables[0].Rows[i][5].ToString()+"*";
				else if(elemFormato.Tables[0].Rows[i][11].ToString()=="D")
				{
					int diferenciaCaracteres = cantidadCaracteres - resultadoSelect.Length;
					double varPosX = diferenciaCaracteres/cantiCaracCm;
					resultado += resultadoSelect+"+"+(posIX+varPosX+cantidadCorrimiento).ToString()+"+"+elemFormato.Tables[0].Rows[i][5].ToString()+"*";
				}
			}
			return resultado.Substring(0,resultado.Length-1);
		}
		
		
		private double CalculoAnchoColumna(double anchoTotal, double porcentajeUtilizacion)
		{
			return anchoTotal*(porcentajeUtilizacion/100);
		}
		
		private string Determinar_Llave(string nombreTabla, string nombreCampo)
		{
			string llave = "";
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT reftbname, relname, refkeyname FROM sysibm.sysrels WHERE tbname = '"+nombreTabla+"'");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				if(DBFunctions.RecordExist("SELECT * FROM SYSIBM.SYSKEYCOLUSE WHERE constname='"+ds.Tables[0].Rows[i][1].ToString()+"' AND tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"'"))
				{
					llave = ds.Tables[0].Rows[i][0].ToString()+"-"+ds.Tables[0].Rows[i][1].ToString()+"-"+ds.Tables[0].Rows[i][2].ToString();
					i = ds.Tables[0].Rows.Count;
				}
			}
			return llave;
		}

		private void InitializeComponent()
		{

		}
		
		
		private bool Determinar_Elemento_Compuesto(string tabla, string campo, string tipoFactura)
		{
			bool compuesto = false;
			if(DBFunctions.RecordExist("SELECT * FROM scolumnaespecial WHERE staf_nombretabla='"+tabla+"' AND stfc_codigo='"+tipoFactura+"' AND scle_columna='"+campo+"'"))
				compuesto = true;
			return compuesto;
		}
	}
}
