using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;

namespace AMS.Finanzas.Cartera
{
	public class FacturaAdministrativa
	{
		protected string prefijoFactura,fecha,fechaVencimiento,nit,almacen,centroCosto,vendedor,mensajes,tipoFactura,tipoGasto,observacion,usuario,prefijoProveedor,cuenta;
		protected int numeroFactura,diasPlazo;
        protected long numeroProveedor;
		protected double valorFactura,valorIva,valorFletes,valorIvaFletes,costoFactura,valorRete;
		protected DataTable tablaDetallesFactura,tablaRetenciones,tablaIva;
		protected ArrayList inserts;
		
		public string PrefijoFactura{set{prefijoFactura=value;}get{return prefijoFactura;}}
		public string Fecha{set{fecha=value;}get{return fecha;}}
		public string FechaVencimiento{set{fechaVencimiento=value;} get{return fechaVencimiento;}}
		public string Nit{set{nit=value;}get{return nit;}}
		public string Almacen{set{almacen=value;}get{return almacen;}}
		public string CentroCosto{set{centroCosto=value;}get{return centroCosto;}}
		public string Vendedor{set{vendedor=value;}get{return vendedor;}}
		public string Mensajes{set{mensajes=value;}get{return mensajes;}}
		public string TipoGasto{set{tipoGasto=value;}get{return tipoGasto;}}
		public string Observacion{set{observacion=value;}get{return observacion;}}
		public string Usuario{set{usuario=value;}get{return usuario;}}
		public string PrefijoProveedor{set{prefijoProveedor=value;}get{return prefijoProveedor;}}
		public string Cuenta{set{cuenta=value;} get{return cuenta;}}
		public int NumeroFactura{set{numeroFactura=value;}get{return numeroFactura;}}
		public long NumeroProveedor{set{numeroProveedor=value;}get{return numeroProveedor;}}
		public int DiasPlazo{set{diasPlazo=value;} get{return diasPlazo;}}
		public double ValorFactura{set{valorFactura=value;}get{return valorFactura;}}
		public double ValorIva{set{valorIva=value;}get{return valorIva;}}
		public double ValorFletes{set{valorFletes=value;}get{return valorFletes;}}
		public double ValorIvaFletes{set{valorIvaFletes=value;}get{return valorIvaFletes;}}
		public double CostoFactura{set{costoFactura=value;}get{return costoFactura;}}
		public double ValorRete{set{valorRete=value;}get{return valorRete;}}
		public ArrayList Inserts{set{inserts=value;}get{return inserts;}}
		
		public FacturaAdministrativa()
		{ 
			tablaDetallesFactura=null;
			tablaRetenciones=null;
			tablaIva=null;
		}
		
		public FacturaAdministrativa(DataTable tdf,DataTable tr,DataTable ti,string tf)
		{
			tablaDetallesFactura=tdf;
			tablaRetenciones=tr;
			tablaIva=ti;
			tipoFactura=tf;
		}
		
		protected void Sacar_Sqls(ref ArrayList sqlStrings)
		{
			for(int i=0;i<inserts.Count;i++)
				sqlStrings.Add(inserts[i]);
		}

		public bool Guardar_Factura()
		{
			bool estado=false;
			FacturaCliente miFactura=new FacturaCliente();
			FacturaProveedor miFacturaP=new FacturaProveedor();
			ArrayList sqlStrings=new ArrayList();
            ArrayList sqlOpcional = new ArrayList();
            int i;
			if(tipoFactura=="C")
			{
				miFactura=new FacturaCliente("FC",this.prefijoFactura,this.nit,this.almacen,"A",Convert.ToUInt32(this.numeroFactura),Convert.ToUInt32(diasPlazo),Convert.ToDateTime(this.fecha),Convert.ToDateTime(fechaVencimiento),Convert.ToDateTime(null),this.valorFactura,this.valorIva,this.valorFletes,this.valorIvaFletes,this.valorRete,this.costoFactura,this.centroCosto,this.observacion,this.vendedor,this.usuario,null);
				//							  0				1			2			3		4						5				  6					7							8											9					10				11				12					13				14				15					16				17				18			19		
				//Activos Fijos
				if(tipoGasto=="1")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
					{
						sqlStrings.Add("INSERT INTO dgastoactivocliente VALUES('@1',@2,1,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"',"+Convert.ToDouble(this.tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+")");
						sqlStrings.Add("UPDATE mactivofijo SET tvig_vigencia='C' WHERE mafj_codiacti='"+tablaDetallesFactura.Rows[i][1].ToString()+"'");
					}
				}
					//Gastos Diferidos
				else if(tipoGasto=="2")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
						sqlStrings.Add("INSERT INTO dgastodiferidocliente VALUES('@1',@2,2,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"',"+Convert.ToDouble(this.tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+")");
				}
					//Gastos Operativos
				else if(tipoGasto=="3")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
					{
						if(this.tablaDetallesFactura.Rows[i][2].ToString()=="")
							sqlStrings.Add("INSERT INTO dgastooperativocliente VALUES('@1',@2,3,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"',null,"+Convert.ToDouble(this.tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+","+i+")");
						else
							sqlStrings.Add("INSERT INTO dgastooperativocliente VALUES('@1',@2,3,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"','"+this.tablaDetallesFactura.Rows[i][2].ToString()+"',"+Convert.ToDouble(this.tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+","+i+")");
					}
				}
				//Gastos Varios
				else if(tipoGasto=="4")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
					{
						//Si es debito y tiene valor base
						if(((Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString()))!=0)&&((Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))!=0))
							sqlStrings.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt32(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+System.Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString()).ToString()+",'D',"+System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()).ToString()+")");
							//Si es debito y no tiene valor base
						else if(((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString()))!=0)&&((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))==0))
							sqlStrings.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt32(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString())).ToString()+",'D',0)");
							//Si es credito y tiene valor base
						else if(((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString()))!=0)&&((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))!=0))
							sqlStrings.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt32(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString())).ToString()+",'C',"+System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()).ToString()+")");
							//Si es credito y no tiene valor base
						else if(((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString()))!=0)&&((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))==0))
							sqlStrings.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt64(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString())).ToString()+",'C',0)");
					}
				}
                //Activos Fijos
                else if (tipoGasto == "5")
                {
                    for (i = 0; i < tablaDetallesFactura.Rows.Count; i++)
                    {
                        sqlStrings.Add("INSERT INTO DACTIVOFIJOMEJORA VALUES('" + tablaDetallesFactura.Rows[i][1].ToString() + "'," + tablaDetallesFactura.Rows[i][3].ToString().Substring(1).Replace(",", "").Trim() + ",'"+observacion+"', '"+this.prefijoFactura+"', "+this.NumeroFactura+ ", '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
                        sqlStrings.Add("UPDATE MACTIVOFIJO SET MAFJ_VALOMEJORA = "+tablaDetallesFactura.Rows[i][3].ToString().Substring(1).Replace(",", "").Trim()+ " WHERE MAFJ_CODIACTI = '" + tablaDetallesFactura.Rows[i][1].ToString() + "'");
                    }
                }
                if (tablaRetenciones!=null)
				{
					if(tablaRetenciones.Rows.Count!=0)
					{
						for(i=0;i<tablaRetenciones.Rows.Count;i++)
                            sqlStrings.Add("INSERT INTO mfacturaclienteretencion VALUES('@1',@2,'" + tablaRetenciones.Rows[i][0].ToString() + "'," + Convert.ToDouble(tablaRetenciones.Rows[i][3]).ToString() + "," + Convert.ToDouble(tablaRetenciones.Rows[i][2]).ToString() + ")");
					}
				}
				if(tablaIva!=null)
				{
					if(tablaIva.Rows.Count!=0)
					{
						for(i=0;i<tablaIva.Rows.Count;i++)
							sqlStrings.Add("INSERT INTO dfacturaclienteiva VALUES('@1',@2,"+tablaIva.Rows[i][0].ToString()+",'"+tablaIva.Rows[i][3].ToString()+"','"+tablaIva.Rows[i][2].ToString()+"',"+tablaIva.Rows[i][1].ToString()+")");
					}
				}
				sqlStrings.Add("INSERT INTO mfacturaadministrativacliente VALUES('@1',@2,'"+cuenta+"')");
				miFactura.SqlRels=sqlStrings;
				if(miFactura.GrabarFacturaCliente(true))
				{
					this.mensajes=miFactura.ProcessMsg;
					estado=true;
				}
				else
					this.mensajes=miFactura.ProcessMsg;
			}
			else if(tipoFactura=="P")
			{
				miFacturaP=new FacturaProveedor("FP",prefijoFactura,prefijoProveedor,nit,almacen,"A",Convert.ToUInt64(numeroFactura),Convert.ToUInt64(numeroProveedor),
				//								 0			1				2		  3		4	  5					6								7
				                                "V",Convert.ToDateTime(fecha),Convert.ToDateTime(fechaVencimiento),Convert.ToDateTime(null),Convert.ToDateTime(fecha),valorFactura,valorIva,
				//								8				9						10						11						12						13			14
				                               valorFletes,valorIvaFletes,valorRete,observacion,usuario);
				//									15			16			 17			18		   19
				this.Sacar_Sqls(ref sqlStrings);
                // cuando el saldo de la factura sea 0, se pone la vigencia en C
                if(valorFactura + valorIva + valorFletes + valorIvaFletes - valorRete == 0)
                    sqlStrings.Add("update MFACTURAPROVEEDOR SET TVIG_VIGENCIA = 'C' WHERE PDOC_CODIORDEPAGO = '@1' AND MFAC_NUMEORDEPAGO = @2 ");
                //Activos Fijos
                if (tipoGasto=="1")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
						sqlStrings.Add("INSERT INTO dgastoactivoproveedor VALUES('@1',@2,1,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"',"+Convert.ToDouble(tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+")");
				}
				//Gastos Diferidos
				else if(tipoGasto=="2")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
						sqlStrings.Add("INSERT INTO dgastodiferidoproveedor VALUES('@1',@2,2,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"',"+Convert.ToDouble(tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+")");
				}
				//Gastos Operativos
				else if(tipoGasto=="3")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
					{
						if(this.tablaDetallesFactura.Rows[i][2].ToString()=="")
							sqlStrings.Add("INSERT INTO dgastooperativoproveedor VALUES('@1',@2,3,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"',null,"+Convert.ToDouble(tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+","+i+")");
						else
							sqlStrings.Add("INSERT INTO dgastooperativoproveedor VALUES('@1',@2,3,"+this.tablaDetallesFactura.Rows[i][0].ToString()+",'"+this.tablaDetallesFactura.Rows[i][1].ToString()+"','"+this.tablaDetallesFactura.Rows[i][2].ToString()+"',"+Convert.ToDouble(tablaDetallesFactura.Rows[i][3].ToString().Substring(1))+","+i+")");
					}
				}
				//Gastos Varios
				else if(tipoGasto=="4")
				{
					for(i=0;i<tablaDetallesFactura.Rows.Count;i++)
					{
						//Si es debito y tiene valor base
						if(((Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString()))!=0)&&((Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))!=0))
							sqlStrings.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToUInt64(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+System.Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString()).ToString()+",'D',"+System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()).ToString()+")");
							//Si es debito y no tiene valor base
						else if(((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString()))!=0)&&((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))==0))
							sqlStrings.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToUInt64(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaDetallesFactura.Rows[i][7].ToString())).ToString()+",'D',0)");
							//Si es credito y tiene valor base
						else if(((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString()))!=0)&&((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))!=0))
							sqlStrings.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToUInt64(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString())).ToString()+",'C',"+System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()).ToString()+")");
							//Si es credito y no tiene valor base
						else if(((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString()))!=0)&&((System.Convert.ToDouble(tablaDetallesFactura.Rows[i][9].ToString()))==0))
							sqlStrings.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2,"+i.ToString()+",'"+tablaDetallesFactura.Rows[i][6].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToUInt64(tablaDetallesFactura.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaDetallesFactura.Rows[i][1].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][0].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][2].ToString().Trim()+"','"+tablaDetallesFactura.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaDetallesFactura.Rows[i][8].ToString())).ToString()+",'C',0)");
					}
				}
                //Activos Fijos
                else if (tipoGasto == "5")
                {
                    for (i = 0; i < tablaDetallesFactura.Rows.Count; i++)
                    {
                        sqlStrings.Add("INSERT INTO DACTIVOFIJOMEJORA VALUES('" + tablaDetallesFactura.Rows[i][1].ToString() + "'," + tablaDetallesFactura.Rows[i][3].ToString().Substring(1).Replace(",", "").Trim() + ",'" + observacion + "', '" + this.prefijoFactura + "', " + this.NumeroFactura + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                        sqlStrings.Add("UPDATE MACTIVOFIJO SET MAFJ_VALOMEJORA = " + tablaDetallesFactura.Rows[i][3].ToString().Substring(1).Replace(",", "").Trim() + " WHERE MAFJ_CODIACTI = '" + tablaDetallesFactura.Rows[i][1].ToString() + "'");
                    }
                }
                if (tablaRetenciones!=null)
				{
					if(tablaRetenciones.Rows.Count!=0)
					{
						for(i=0;i<tablaRetenciones.Rows.Count;i++)
                            sqlStrings.Add("INSERT INTO mfacturaproveedorretencion VALUES('@1',@2,'" + tablaRetenciones.Rows[i][0].ToString() + "'," + Convert.ToDouble(tablaRetenciones.Rows[i][3]).ToString() + "," + Convert.ToDouble(tablaRetenciones.Rows[i][2]).ToString() + ")");
					}
				}
				if(tablaIva!=null)
				{
					if(tablaIva.Rows.Count!=0)
					{
						for(i=0;i<tablaIva.Rows.Count;i++)
							sqlStrings.Add("INSERT INTO dfacturaproveedoriva VALUES('@1',@2,"+tablaIva.Rows[i][0].ToString()+",'"+tablaIva.Rows[i][3].ToString()+"','"+tablaIva.Rows[i][2].ToString()+"',"+tablaIva.Rows[i][1].ToString()+")");
					}
				}
				sqlStrings.Add("INSERT INTO mfacturaadministrativaproveedor VALUES('@1',@2,'"+cuenta+"')");
                
				miFacturaP.SqlRels=sqlStrings;
				if(miFacturaP.GrabarFacturaProveedor(true))	
				{
                    //Este proceso aplica solamente para EUROTECK y cancela la factua en finanzas sin generar comprobantes de egreso cuando usan la cuenta 1330
                    if (DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA") == "901087944" && cuenta.ToString().Substring(0, 4) == "1330")
                    {
                        sqlOpcional.Add("update MFACTURAPROVEEDOR set MFAC_VALOABON = MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE, TVIG_VIGENCIA = 'C' where PDOC_CODIORDEPAGO = '"+ prefijoFactura + "' and MFAC_NUMEORDEPAGO = "+ Convert.ToUInt64(numeroFactura) + "");
                        if (DBFunctions.Transaction(sqlStrings))
                        {
                            string processMsg = DBFunctions.exceptions;
                        }
                    }                    
                    estado =true;
					this.mensajes=miFacturaP.ProcessMsg;
				}
				else
					this.mensajes="Error : "+miFacturaP.ProcessMsg;
			}
			return estado;
		}
	}
}
