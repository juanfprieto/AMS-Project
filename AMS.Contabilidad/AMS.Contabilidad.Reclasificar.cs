
	
using AMS.DB;
using System;
using System.Collections;
using System.Data.Odbc;
using System.Data;
	
	
	
namespace AMS.Contabilidad  
{
		
	public class Reclasificar 
	{
		private string  newpar,oldpar,field,processMsg,tablaPrincipal;
		private DataSet tablasDependientes, tablasMaestras;
		private ArrayList operaciones;
				
		public string ProcessMsg{get{return processMsg;}}
		
		
		public Reclasificar(string newparam,string oldparam, string campokey, string tablaBorrado)	
		{
			
			newpar=newparam;
			oldpar=oldparam;	
			tablaPrincipal=tablaBorrado;
			field=campokey;
			operaciones = new ArrayList();
			//A continuacion llenamos un ArrayList llamado table con los nombres de las tablas que tienen de referencia a tablaBorrado
			tablasDependientes = new DataSet();
			DBFunctions.Request(tablasDependientes,IncludeSchema.NO,"SELECT tbname, fkcolnames from SYSIBM.SYSRELS where reftbname = '"+tablaBorrado.ToUpper()+"' AND tbname NOT LIKE 'M%' ORDER BY TBNAME");
			tablasMaestras = new DataSet();
			DBFunctions.Request(tablasMaestras,IncludeSchema.NO,"SELECT tbname, fkcolnames from SYSIBM.SYSRELS where reftbname = '"+tablaBorrado.ToUpper()+"' AND tbname LIKE 'M%' ORDER BY TBNAME");
		}
			
		
		public bool ProcUnParam()
		{
			int i;
			bool status = false;
			ArrayList sqlStrings = new ArrayList();
			
            if (tablaPrincipal=="MNIT")
            {
                string sqlMauxiliarcuenta =
                  "select m1.mcue_codipuc, m1.pano_ano, m1.pmes_mes, m1.mnit_nit, m2.maux_valodebi, m2.maux_valocred, m2.mnit_nit "+
                  " from mauxiliarcuenta m1, mauxiliarcuenta m2 "+
                  "where m1.mcue_codipuc = m2.mcue_codipuc "+
                  "  and m1.pano_ano = m2.pano_ano "+
                  "  and m1.pmes_mes = m2.pmes_mes "+
                  "  and m1.mnit_nit = '"+ newpar +"' and m2.mnit_nit = '"+ oldpar +"';";
                DataSet mAuxiliarcuenta = new DataSet();
                DBFunctions.Request(mAuxiliarcuenta, IncludeSchema.NO, sqlMauxiliarcuenta);
		        for (int j = 0; j < mAuxiliarcuenta.Tables[0].Rows.Count; j++)
                {
                    sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi = maux_valodebi + " + mAuxiliarcuenta.Tables[0].Rows[j][4].ToString() + ", maux_valoCRED = maux_valoCRED + " + mAuxiliarcuenta.Tables[0].Rows[j][5].ToString() + " WHERE MCUE_CODIPUC = '"+ mAuxiliarcuenta.Tables[0].Rows[j][0].ToString() +"' and pano_ano = " + mAuxiliarcuenta.Tables[0].Rows[j][1].ToString() + " and pmes_mes = " + mAuxiliarcuenta.Tables[0].Rows[j][2].ToString() + " and mnit_nit = '"+ newpar +"' ");
                    sqlStrings.Add("DELETE FROM mauxiliarcuenta WHERE mnit_nit = '" + this.oldpar + "' and pano_ano = " + mAuxiliarcuenta.Tables[0].Rows[j][1].ToString() + " and pmes_mes = " + mAuxiliarcuenta.Tables[0].Rows[j][2].ToString() + " and mcue_codipuc = '" + mAuxiliarcuenta.Tables[0].Rows[j][0].ToString() + "' ");
                }
                mAuxiliarcuenta.Dispose();

                DataSet  mpedidoItem = new DataSet();
                DateTime fechaPedido, fechaProceso;
                DBFunctions.Request(mpedidoItem, IncludeSchema.NO, "select * from mpedidoitem where mnit_nit = '"+oldpar+"';");
                if(mpedidoItem.Tables[0].Rows.Count > 0)
                {
                    sqlStrings.Add("ALTER TABLE DBXSCHEMA.MPEDIDOITEM DROP UNIQUE UK_MPEDNUM");  
                    for (int j = 0; j < mpedidoItem.Tables[0].Rows.Count; j++)
                    {
                        fechaPedido = Convert.ToDateTime(mpedidoItem.Tables[0].Rows[j][4].ToString());
                        fechaProceso = Convert.ToDateTime(mpedidoItem.Tables[0].Rows[j][5].ToString());
                        sqlStrings.Add("INSERT INTO MPEDIDOITEM VALUES( '" + mpedidoItem.Tables[0].Rows[j][0].ToString() + "','" + newpar + "','" + mpedidoItem.Tables[0].Rows[j][2].ToString() + "'," + mpedidoItem.Tables[0].Rows[j][3].ToString() + ",'" + fechaPedido.ToString("yyyy-MM-dd") + "','" + fechaProceso.ToString("yyyy-MM-dd HH:mm:ss") + "','" + mpedidoItem.Tables[0].Rows[j][6].ToString() + "','" + mpedidoItem.Tables[0].Rows[j][7].ToString() + "','" + mpedidoItem.Tables[0].Rows[j][8].ToString() + "','" + mpedidoItem.Tables[0].Rows[j][9].ToString() + "');");
                    }
                    mpedidoItem.Dispose();
                    sqlStrings.Add("UPDATE DPEDIDOITEM SET MNIT_NIT = '" + this.newpar + "' WHERE mnit_nit = '" + this.oldpar + "' ");
                    sqlStrings.Add("DELETE FROM Mpedidoitem WHERE mnit_nit = '" + this.oldpar + "' ");
                    sqlStrings.Add("ALTER TABLE DBXSCHEMA.MPEDIDOITEM ADD CONSTRAINT UK_MPEDNUM UNIQUE (PPED_CODIGO,MPED_NUMEPEDI);"); 
                }
            }
	        else
            {
			    // reemplazamos MSALDOCUENTA COMUNES
			    DataSet msaldocuenta = new DataSet();
			    DBFunctions.Request(msaldocuenta,IncludeSchema.NO,"SELECT M2.* FROM MSALDOCUENTA M1, MSALDOCUENTA M2 WHERE M1.MCUE_CODIPUC = '"+this.newpar+"' AND M2.MCUE_CODIPUC = '"+this.oldpar+"' and m1.pano_ano = m2.pano_ano and m1.pmes_mes = m2.pmes_mes;");
			    // (0)  año, (1) = mes, (2) = Cuenta, (3) = Debito, (4) = credito, (5) = Inflacion
			    for(int j=0; j<msaldocuenta.Tables[0].Rows.Count; j++)
		    	{
				    sqlStrings.Add("UPDATE msaldocuenta SET msaL_valodebi = msaL_valodebi + "+msaldocuenta.Tables[0].Rows[j][3].ToString()+",msaL_valoCRED = msaL_valoCRED + "+msaldocuenta.Tables[0].Rows[j][4].ToString()+",msaL_valoINFL = msaL_valoINFL + "+msaldocuenta.Tables[0].Rows[j][5].ToString()+" WHERE MCUE_CODIPUC = '"+this.newpar+"' and pano_ano = "+msaldocuenta.Tables[0].Rows[j][0].ToString()+" and pmes_mes = "+msaldocuenta.Tables[0].Rows[j][1].ToString()+" ");
				    sqlStrings.Add("DELETE FROM msaldocuenta WHERE mcue_codipuc = '"+this.oldpar+"' and pano_ano = "+msaldocuenta.Tables[0].Rows[j][0].ToString()+" and pmes_mes = "+msaldocuenta.Tables[0].Rows[j][1].ToString()+" ");
			    }	
			    msaldocuenta.Dispose();

			    // reemplazamos MAUXILIARCUENTA COMUNES
			    DataSet msaldocuentaA = new DataSet();
			    DBFunctions.Request(msaldocuentaA,IncludeSchema.NO,"SELECT M2.* FROM MauxiliarCUENTA M1, MauxiliarCUENTA M2 WHERE M1.MCUE_CODIPUC = '"+this.newpar+"' AND M2.MCUE_CODIPUC = '"+this.oldpar+"' and m1.mnit_nit = m2.mnit_nit and m1.pano_ano = m2.pano_ano and m1.pmes_mes = m2.pmes_mes;");
			    // (0)  año, (1) = mes, (2) = Cuenta, (3) = Nit, (4) = Debito, (5) = Credito
                if (msaldocuentaA.Tables.Count > 0)
                {
                    for (int j = 0; j < msaldocuentaA.Tables[0].Rows.Count; j++)
                    {
                        sqlStrings.Add("UPDATE mauxiliarcuenta SET maux_valodebi = maux_valodebi + " + msaldocuentaA.Tables[0].Rows[j][4].ToString() + ", maux_valoCRED = maux_valoCRED + " + msaldocuentaA.Tables[0].Rows[j][5].ToString() + " WHERE MCUE_CODIPUC = '" + this.newpar + "' and pano_ano = " + msaldocuentaA.Tables[0].Rows[j][0].ToString() + " and pmes_mes = " + msaldocuentaA.Tables[0].Rows[j][1].ToString() + " and mnit_nit = '" + msaldocuentaA.Tables[0].Rows[j][3].ToString() + "' ");
                        sqlStrings.Add("DELETE FROM mauxiliarcuenta WHERE mcue_codipuc = '" + this.oldpar + "' and pano_ano = " + msaldocuentaA.Tables[0].Rows[j][0].ToString() + " and pmes_mes = " + msaldocuentaA.Tables[0].Rows[j][1].ToString() + " and mnit_nit = '" + msaldocuentaA.Tables[0].Rows[j][3].ToString() + "' ");
                    }
                }
			    msaldocuentaA.Dispose();

                // reemplazamos MAUXILIARCUENTA Centro Costo COMUNES
                DataSet mauxcuentacCostoA = new DataSet();
                DBFunctions.Request(mauxcuentacCostoA, IncludeSchema.NO, "SELECT M2.* FROM MauxiliarCENTROCOSTOCUENTA M1, MauxiliarCENTROCOSTOCUENTA M2 WHERE M1.MCUE_CODIPUC = '" + this.newpar + "' AND M2.MCUE_CODIPUC = '" + this.oldpar + "' and m1.PCEN_CODIGO = m2.PCEN_CODIGO and m1.pano_ano = m2.pano_ano and m1.pmes_mes = m2.pmes_mes ;");
                // (0)  año, (1) = mes, (2) = Cuenta, (3) = C.Costo, (4) = Debito, (5) = Credito
                if (mauxcuentacCostoA.Tables.Count > 0 && mauxcuentacCostoA.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < mauxcuentacCostoA.Tables[0].Rows.Count; j++)
                    {
                        sqlStrings.Add("UPDATE mauxiliarCENTROCOSTOcuenta SET maux_valodebi = maux_valodebi + " + mauxcuentacCostoA.Tables[0].Rows[j][4].ToString() + ", maux_valoCRED = maux_valoCRED + " + mauxcuentacCostoA.Tables[0].Rows[j][5].ToString() + " WHERE MCUE_CODIPUC = '" + this.newpar + "' and pano_ano = " + mauxcuentacCostoA.Tables[0].Rows[j][0].ToString() + " and pmes_mes = " + mauxcuentacCostoA.Tables[0].Rows[j][1].ToString() + " and PCEN_CODIGO = '" + mauxcuentacCostoA.Tables[0].Rows[j][3].ToString() + "' ");
                        sqlStrings.Add("DELETE FROM mauxiliarCENTROCOSTOcuenta WHERE mcue_codipuc = '" + this.oldpar + "' and pano_ano = " + mauxcuentacCostoA.Tables[0].Rows[j][0].ToString() + " and pmes_mes = " + mauxcuentacCostoA.Tables[0].Rows[j][1].ToString() + " and PCEN_CODIGO = '" + mauxcuentacCostoA.Tables[0].Rows[j][3].ToString() + "' ");
                    }
                }
                mauxcuentacCostoA.Dispose();

			    // reemplazamos MCUENTAPRESUPUESTO COMUNES
			    DataSet msaldocuentaP = new DataSet();
			    DBFunctions.Request(msaldocuentaP,IncludeSchema.NO,"SELECT M2.* FROM MCUENTApresupuesto M1, MCUENTApresupuesto M2 WHERE M1.MCUE_CODIPUC = '"+this.newpar+"' AND M2.MCUE_CODIPUC = '"+this.oldpar+"' and m1.pcen_codigo = m2.pcen_codigo and m1.pano_ano = m2.pano_ano and m1.pmes_mes = m2.pmes_mes;");
			    // (0) = Cuenta, (1)  Año, (2) = Mes, (3) = C.Costo, (4) = Valor  
			    for(int j=0; j<msaldocuentaP.Tables[0].Rows.Count; j++)
			    {
				    sqlStrings.Add("UPDATE mcuentapresupuesto SET mpre_valor = mpre_valor + "+msaldocuentaP.Tables[0].Rows[j][4].ToString()+" WHERE MCUE_CODIPUC = '"+this.newpar+"' and pano_ano = "+msaldocuentaP.Tables[0].Rows[j][1].ToString()+" and pmes_mes = "+msaldocuentaP.Tables[0].Rows[j][2].ToString()+" and pcen_codigo = "+msaldocuentaP.Tables[0].Rows[j][3].ToString()+"");
				    sqlStrings.Add("DELETE FROM mcuentapresupuesto WHERE mcue_codipuc = '"+this.oldpar+"' and pano_ano = "+msaldocuentaP.Tables[0].Rows[j][0].ToString()+" and pmes_mes = "+msaldocuentaP.Tables[0].Rows[j][1].ToString()+" ");
			    }	
			    msaldocuentaP.Dispose();
			
            }
            sqlStrings.Add("commit");
            if (tablaPrincipal == "MNIT")
            {
                int vecesMprov = Convert.ToInt32(DBFunctions.SingleData("SELECT count(*) FROM MPROVEEDOR WHERE MNIT_NIT in ( '" + newpar + "','" + oldpar + "' ) "));
                if (vecesMprov == 2)  // elimino elproveedor errado si ya existe el correcto
                    sqlStrings.Add("DELETE FROM MPROVEEDOR WHERE mnit_nit = '" + this.oldpar + "' ");

                int vecesMclie = Convert.ToInt32(DBFunctions.SingleData("SELECT count(*) FROM MCLIENTE   WHERE MNIT_NIT  in ( '" + newpar + "','" + oldpar + "' ) "));
                if (vecesMclie == 2)  // elimino el cliente errado si ya exsite el correcto 
                    sqlStrings.Add("DELETE FROM MCLIENTE WHERE mnit_nit = '" + this.oldpar + "' ");
            }

			//Empezamos recorriendo el DataSet que tiene las tablas que hacen referencia a la tablaBorrado
			for(i=0; i<tablasDependientes.Tables[0].Rows.Count; i++)
			{
				//Ahora debemos revisar si el campo dentro de esta tabla hace parte de la llave primaria
				if(this.Determinar_Llave_Primaria(DBFunctions.SingleData("SELECT colnames FROM SYSIBM.SYSINDEXES WHERE tbname='"+tablasDependientes.Tables[0].Rows[i][0].ToString()+"'"),tablasDependientes.Tables[0].Rows[i][1].ToString().Trim()))
				{
					//si hace parte de la llave primaria revisamos si existe un registro con el valor que esta bien
					//si este valor existe debemos eliminar el registro, sino lo actualizamos simplemente
			//		if(DBFunctions.RecordExist("SELECT * FROM "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"'"))
			//		    sqlStrings.Add("DELETE FROM "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
			//		else
						sqlStrings.Add("UPDATE "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" SET "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"' WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
				}
				else
					    sqlStrings.Add("UPDATE "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" SET "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"' WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
			}
			//Ahora realizamos la actualizacion dentro de las tablas maestras y eleminamos el registro	tablasDependientes.Tables[0].Rows[i][0]	"CEMPRESA"	string

			for(i=0; i<tablasMaestras.Tables[0].Rows.Count; i++)
			{
		//		if (tablasMaestras.Tables[0].Rows[i][0].ToString()!="MAUXILIARCUENTA")
		//		{
		//			if(this.Determinar_Llave_Primaria(DBFunctions.SingleData("SELECT colnames FROM SYSIBM.SYSINDEXES WHERE tbname='"+tablasMaestras.Tables[0].Rows[i][0].ToString()+"'"),tablasMaestras.Tables[0].Rows[i][1].ToString().Trim()))
		//			{
		//				if(DBFunctions.RecordExist("SELECT * FROM "+tablasMaestras.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"'"))
		//					sqlStrings.Add("DELETE FROM "+tablasMaestras.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
		//				else
		//					sqlStrings.Add("UPDATE "+tablasMaestras.Tables[0].Rows[i][0].ToString()+" SET "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"' WHERE "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
		//			}
		//			else
		//			{
        //      sqlStrings.Add("UPDATE " + tablasMaestras.Tables[0].Rows[i][0].ToString() + " SET " + tablasMaestras.Tables[0].Rows[i][1].ToString() + "='" + this.newpar + "' WHERE " + tablasMaestras.Tables[0].Rows[i][1].ToString() + "='" + this.oldpar + "'  AND '" + this.newpar + "' not in (select '" + this.newpar + "' from "+ tablasMaestras.Tables[0].Rows[i][0].ToString() +") ");
                sqlStrings.Add("UPDATE " + tablasMaestras.Tables[0].Rows[i][0].ToString() + " SET " + tablasMaestras.Tables[0].Rows[i][1].ToString() + "='" + this.newpar + "' WHERE " + tablasMaestras.Tables[0].Rows[i][1].ToString() + "='" + this.oldpar + "' ");
        //			}
		//		}
				
			}
           
		    sqlStrings.Add("DELETE FROM "+this.tablaPrincipal+" WHERE "+this.field+"='"+this.oldpar+"'");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}
			
		private bool Determinar_Llave_Primaria(string colnames, string name)
		{
			bool encontrado = false;
			string []nombres = colnames.Split('+');
			for(int i=0;i<nombres.Length;i++)
			{
				if(nombres[i]==name)
					encontrado = true;
			}
			return encontrado;
		}
	}
}
