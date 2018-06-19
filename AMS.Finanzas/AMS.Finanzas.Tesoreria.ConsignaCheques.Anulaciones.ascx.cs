using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class Anulaciones : System.Web.UI.UserControl
	{
		protected DataTable tablaDatos;
		protected Button anular;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind=new DatasToControls();
                bind.PutDatasIntoDropDownList(tipoDocAnular, "SELECT pdoc_codigo, pdoc_codigo || '  -  ' ||pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu IN ('CS','EC','ND','NR','DE','TC','RF','DF') and tvig_vigencia = 'V' ");
				if(tipoDocAnular.Items.Count>0)
                    tipoDocAnular.Items.Insert(0,"Seleccione:..");
                else
				bind.PutDatasIntoDropDownList(numeroDocumento,"SELECT mtes_numero FROM mtesoreria WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"' AND test_estadodoc<>'N' ORDER BY mtes_numero");
                //bind.PutDatasIntoDropDownList(ddlPrefDev, "SELECT pdoc_codigo,pdoc_codigo ||'-'|| pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='NC' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref ddlPrefDev , "%", "%", "NC");
                //bind.PutDatasIntoDropDownList(ddlPrefDevProv, "SELECT pdoc_codigo,pdoc_codigo ||'-'|| pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='NP' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref ddlPrefDevProv , "%", "%", "NP");
			}
			else
			{
				if(Session["tablaDatos"]!=null)
					tablaDatos=(DataTable)Session["tablaDatos"];
			}
		}
		
		protected void tipoDocAnular_Changed(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroDocumento,"SELECT mtes_numero FROM mtesoreria WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"' AND test_estadodoc<>'N' ORDER BY mtes_numero");
			lbInfo.Text=lbEfectivo.Text="";
		}

		protected void Cargar_Tabla_Consignacion()
		{
			BoundColumn prefRC,numRC,tipDoc,numDoc,ban,val,prefCons,numCons,cue,nit;
			prefRC=new BoundColumn();prefRC.DataField="PREFIJO RECIBO CAJA";prefRC.HeaderText="Prefijo Recibo Caja";
			numRC=new BoundColumn();numRC.DataField="NUMERO RECIBO CAJA";numRC.HeaderText="Número Recibo Caja";
            tipDoc=new BoundColumn();tipDoc.DataField="TIPO DE DOCUMENTO";tipDoc.HeaderText="Tipo de Documento";
			numDoc=new BoundColumn();numDoc.DataField="NUMERO DOCUMENTO";numDoc.HeaderText="Número de Documento";
			ban=new BoundColumn();ban.DataField="BANCO";ban.HeaderText="Banco";
			val=new BoundColumn();val.DataField="VALOR";val.DataFormatString="{0:C}";val.HeaderText="Valor";
			prefCons=new BoundColumn();prefCons.DataField="PREFIJO CONSIGNACION";prefCons.HeaderText="Prefijo Consignación";
			numCons=new BoundColumn();numCons.DataField="NUMERO CONSIGNACION";numCons.HeaderText="Número de Consignación";
			cue=new BoundColumn();cue.DataField="CUENTA";cue.HeaderText="Cuenta";
			nit=new BoundColumn();nit.DataField="NIT";nit.HeaderText="Nit";
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("PREFIJO RECIBO CAJA",typeof(string));
			tablaDatos.Columns.Add("NUMERO RECIBO CAJA",typeof(int));
			tablaDatos.Columns.Add("TIPO DE DOCUMENTO",typeof(string));
			tablaDatos.Columns.Add("NUMERO DOCUMENTO",typeof(string));
			tablaDatos.Columns.Add("BANCO",typeof(string));
			tablaDatos.Columns.Add("VALOR",typeof(double));
			tablaDatos.Columns.Add("PREFIJO CONSIGNACION",typeof(string));
			tablaDatos.Columns.Add("NUMERO CONSIGNACION",typeof(int));
			tablaDatos.Columns.Add("CUENTA",typeof(string));
			tablaDatos.Columns.Add("NIT",typeof(string));
			gridDatos.Columns.Add(prefRC);gridDatos.Columns.Add(numRC);gridDatos.Columns.Add(tipDoc);
			gridDatos.Columns.Add(numDoc);gridDatos.Columns.Add(ban);gridDatos.Columns.Add(val);
			gridDatos.Columns.Add(prefCons);gridDatos.Columns.Add(numCons);gridDatos.Columns.Add(cue);
			gridDatos.Columns.Add(nit);
		}

		protected void Cargar_Tabla_Devoluciones()
		{
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("CUENTA",typeof(string));//0
			tablaDatos.Columns.Add("FECHA",typeof(DateTime));//1
			tablaDatos.Columns.Add("PREFIJO FACTURA",typeof(string));//2
			tablaDatos.Columns.Add("NUMERO FACTURA",typeof(int));//3
			tablaDatos.Columns.Add("PREFIJO CONSIGNACION",typeof(string));//4
			tablaDatos.Columns.Add("NUMERO CONSIGNACION",typeof(int));//5
			tablaDatos.Columns.Add("PREFIJO RECIBO CAJA",typeof(string));//6
			tablaDatos.Columns.Add("NUMERO RECIBO CAJA",typeof(int));//7
			tablaDatos.Columns.Add("NUMERO DOCUMENTO",typeof(string));//8
			tablaDatos.Columns.Add("VALOR",typeof(double));//9
			tablaDatos.Columns.Add("PREFIJO NOTA",typeof(string));//10
			tablaDatos.Columns.Add("BANCO",typeof(string));//11
		}
		
		protected void Cargar_Tabla_Remisiones()
		{
			BoundColumn prefRc,numRC,nitFin,valor,numChe,prefFac,numFac;
			prefRc=new BoundColumn();prefRc.HeaderText="Prefijo Recibo Caja";prefRc.DataField="PREFIJO RECIBO CAJA";
			numRC=new BoundColumn();numRC.HeaderText="Número Recibo Caja";numRC.DataField="NUMERO RECIBO CAJA";
			nitFin=new BoundColumn();nitFin.HeaderText="Nit Financiera";nitFin.DataField="NIT FINANCIERA";
			valor=new BoundColumn();valor.HeaderText="Valor";valor.DataField="VALOR";valor.DataFormatString="{0:C}";
			numChe=new BoundColumn();numChe.HeaderText="Número Cheque";numChe.DataField="NUMERO CHEQUE";
			prefFac=new BoundColumn();prefFac.HeaderText="Prefijo Factura";prefFac.DataField="PREFIJO FACTURA";
			numFac=new BoundColumn();numFac.HeaderText="Número Factura";numFac.DataField="NUMERO FACTURA";
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("PREFIJO RECIBO CAJA",typeof(string));//0
			tablaDatos.Columns.Add("NUMERO RECIBO CAJA",typeof(int));//1
			tablaDatos.Columns.Add("NIT FINANCIERA",typeof(string));//2
			tablaDatos.Columns.Add("VALOR",typeof(double));//3
			tablaDatos.Columns.Add("NUMERO CHEQUE",typeof(string));//4
			tablaDatos.Columns.Add("PREFIJO FACTURA",typeof(string));//5
			tablaDatos.Columns.Add("NUMERO FACTURA",typeof(int));//6
			gridDatos.Columns.Add(prefRc);
			gridDatos.Columns.Add(numRC);gridDatos.Columns.Add(nitFin);gridDatos.Columns.Add(valor);
			gridDatos.Columns.Add(numChe);gridDatos.Columns.Add(prefFac);gridDatos.Columns.Add(numFac);
		}
		
		protected void Cargar_Tabla_DevRemisiones()
		{
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("PREFIJO REMISION",typeof(string));//0
			tablaDatos.Columns.Add("NUMERO REMISION",typeof(int));//1
			tablaDatos.Columns.Add("PREFIJO DEVOLUCION FINANCIERA",typeof(string));//2
			tablaDatos.Columns.Add("NUMERO DEVOLUCION FINANCIERA",typeof(int));//3
			tablaDatos.Columns.Add("PREFIJO RECIBO CAJA",typeof(string));//4
			tablaDatos.Columns.Add("NUMERO RECIBO CAJA",typeof(int));//5
			tablaDatos.Columns.Add("NUMERO CHEQUE",typeof(string));//6
			tablaDatos.Columns.Add("VALOR",typeof(double));//7
			tablaDatos.Columns.Add("PREFIJO FACTURA CLIENTE",typeof(string));//8
			tablaDatos.Columns.Add("NUMERO FACTURA CLIENTE",typeof(int));//9
			tablaDatos.Columns.Add("PREFIJO NOTA DEVOLUCION",typeof(string));//10
		}
		
		protected void Cargar_Tabla_Traslados()
		{
			BoundColumn fec,cueOri,cueDes;
			fec=new BoundColumn();fec.DataField="FECHA";fec.DataFormatString="{0:yyyy'-'MM'-'dd}";fec.HeaderText="Fecha";
			cueOri=new BoundColumn();cueOri.DataField="CUENTAORI";cueOri.HeaderText="Cuenta Origen";
			cueDes=new BoundColumn();cueDes.DataField="CUENTADES";cueDes.HeaderText="Cuenta Destino";
			this.tablaDatos=new DataTable();
			tablaDatos.Columns.Add("FECHA",typeof(DateTime));
			tablaDatos.Columns.Add("CUENTAORI",typeof(string));
			tablaDatos.Columns.Add("CUENTADES",typeof(string));
			gridDatos.Columns.Add(fec);gridDatos.Columns.Add(cueOri);gridDatos.Columns.Add(cueDes);
		}
		
		protected void Cargar_Tabla_TrasladosCh()
		{
			BoundColumn fec,cueOri,cueDes,codCaj,numCaj,tipDoc,numDoc;
			fec=new BoundColumn();fec.DataField="FECHA";fec.DataFormatString="{0:yyyy'-'MM'-'dd}";fec.HeaderText="Fecha";
			cueOri=new BoundColumn();cueOri.DataField="CUEORI";cueOri.HeaderText="Cuenta Origen";
			cueDes=new BoundColumn();cueDes.DataField="CUEDES";cueDes.HeaderText="Cuenta Destino";
			codCaj=new BoundColumn();codCaj.DataField="CODIGOCAJA";codCaj.HeaderText="Prefijo Recibo de Caja";
			numCaj=new BoundColumn();numCaj.DataField="NUMEROCAJA";numCaj.HeaderText="Número Recibo de Caja";
			tipDoc=new BoundColumn();tipDoc.DataField="TIPODOC";tipDoc.HeaderText="Tipo de Documento";
			numDoc=new BoundColumn();numDoc.DataField="NUMERODOC";numDoc.HeaderText="Número del Documento";
			this.tablaDatos=new DataTable();
			tablaDatos.Columns.Add("FECHA",typeof(DateTime));
			tablaDatos.Columns.Add("CUEORI",typeof(string));
			tablaDatos.Columns.Add("CUEDES",typeof(string));
			tablaDatos.Columns.Add("CODIGOCAJA",typeof(string));
			tablaDatos.Columns.Add("NUMEROCAJA",typeof(string));
			tablaDatos.Columns.Add("TIPODOC",typeof(string));
			tablaDatos.Columns.Add("NUMERODOC",typeof(string));
			gridDatos.Columns.Add(fec);gridDatos.Columns.Add(cueOri);gridDatos.Columns.Add(cueDes);
			gridDatos.Columns.Add(codCaj);gridDatos.Columns.Add(numCaj);gridDatos.Columns.Add(tipDoc);
			gridDatos.Columns.Add(numDoc);
		}
			
		protected void Cargar_Tabla_Notas()
		{
			BoundColumn cue,val;
			cue=new BoundColumn();cue.DataField="CUENTA";cue.HeaderText="Cuenta";
			val=new BoundColumn();val.DataField="VALOR";val.DataFormatString="{0:C}";val.HeaderText="Valor";
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("CUENTA",typeof(string));
			tablaDatos.Columns.Add("VALOR",typeof(double));
			gridDatos.Columns.Add(cue);gridDatos.Columns.Add(val);
		}
		
		protected void Cargar_Tabla_Entregas()
		{
			BoundColumn prefEgr,numEgr,nit,numDoc,ban,val;
			prefEgr=new BoundColumn();prefEgr.DataField="PREFIJO COMPROBANTE DE EGRESO";prefEgr.HeaderText="Prefijo del Comprobante de Egreso";
			numEgr=new BoundColumn();numEgr.DataField="NUMERO COMPROBANTE DE EGRESO";numEgr.HeaderText="Número del Comprobante de Egreso";
			nit=new BoundColumn();nit.DataField="NIT DEL BENEFICIARIO";nit.HeaderText="Nit del Beneficiario";
			numDoc=new BoundColumn();numDoc.DataField="NUMERO DEL DOCUMENTO";numDoc.HeaderText="Número del Documento";
			ban=new BoundColumn();ban.DataField="BANCO";ban.HeaderText="Banco";
			val=new BoundColumn();val.DataField="VALOR";val.DataFormatString="{0:C}";val.HeaderText="Valor";
			tablaDatos=new DataTable();
			tablaDatos.Columns.Add("PREFIJO COMPROBANTE DE EGRESO",typeof(string));
			tablaDatos.Columns.Add("NUMERO COMPROBANTE DE EGRESO",typeof(int));
			tablaDatos.Columns.Add("NIT DEL BENEFICIARIO",typeof(string));
			tablaDatos.Columns.Add("NUMERO DEL DOCUMENTO",typeof(string));
			tablaDatos.Columns.Add("BANCO",typeof(string));
			tablaDatos.Columns.Add("VALOR",typeof(double));
            gridDatos.Columns.Add(prefEgr);gridDatos.Columns.Add(numEgr);gridDatos.Columns.Add(nit);
			gridDatos.Columns.Add(numDoc);gridDatos.Columns.Add(ban);gridDatos.Columns.Add(val);
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
            //fecha del documento para su posterior validación
            string fecha = Convert.ToDateTime(DBFunctions.SingleData("SELECT MTES_FECHA FROM DBXSCHEMA.MTESORERIA WHERE PDOC_CODIGO = '" + this.tipoDocAnular.SelectedValue + "' AND MTES_NUMERO = " + this.numeroDocumento.SelectedValue + ";")).GetDateTimeFormats()[5];

            if (!Tools.General.validarCierreFinanzas(fecha, "T"))
            {
                Utils.MostrarAlerta(Response, "La fecha del documento no corresponde a la vigencia del sistema de Tesorería. Por favor revise.");
                return;
            }
            int i;
            bool sinregistros=false;
			DataSet ds;
			if(numeroDocumento.Items.Count==0)
                Utils.MostrarAlerta(Response, "No existen documentos");
			else
			{
				Session.Clear();
				gridDatos.DataBind();
				Control padre=(this.Parent).Parent;
				//Dependiendo del tipo de operación, son los datos que debo cargar
				//CONSIGNACIONES
				if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="CS")
				{
					if(DBFunctions.RecordExist("SELECT dtes_valor FROM dtesoreriaefectivo WHERE mtes_codigo='"+this.tipoDocAnular.SelectedValue+"' AND mtes_numero="+this.numeroDocumento.SelectedValue+""))
						lbEfectivo.Text="Total Consignación Efectivo : " + Convert.ToDouble(DBFunctions.SingleData("SELECT dtes_valor FROM dtesoreriaefectivo WHERE mtes_codigo='"+this.tipoDocAnular.SelectedValue+"' AND mtes_numero="+this.numeroDocumento.SelectedValue+"")).ToString("C");
					else
						lbEfectivo.Text="Total Consignación Efectivo : " + Convert.ToDouble("0").ToString("C");
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MCAJ_CODIGO AS \"PREFIJO RECIBO CAJA\", MCAJ_NUMERO AS \"NUMERO RECIBO CAJA\", TTIP_CODIGO AS \"TIPO DE DOCUMENTO\",DTES_NUMERODOC \"NUMERO DOCUMENTO\", PBAN_BANCO AS BANCO, DTES_VALOR AS VALOR,MTES_CODIGO AS \"PREFIJO CONSIGNACION\", MTES_NUMERO AS \"NUMERO CONSIGNACION\", PCUE_CODIGO AS CUENTA,MNIT_NIT AS NIT FROM DTESORERIADOCUMENTOS WHERE MTES_CODIGO='"+tipoDocAnular.SelectedValue+"' AND MTES_NUMERO="+numeroDocumento.SelectedValue+"");
					if(ds.Tables[0].Rows.Count!=0)
					{
						if(SePuedeAnularConsignacion(tipoDocAnular.SelectedValue,numeroDocumento.SelectedValue))
						{
							lbInfo.Text="Datos Consignación :";
							Cargar_Tabla_Consignacion();
							for(i=0;i<ds.Tables[0].Rows.Count;i++)
								tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
							Session["tablaDatos"]=tablaDatos;
							gridDatos.DataSource=tablaDatos;
							gridDatos.DataBind();
							hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
							hdnTip.Value="CS";
						}
						else
						{
                            Utils.MostrarAlerta(Response, "Imposible anular la consignación, se ha producido alguno de los siguientes errores : \\n1. El estado de alguno de los documentos relacionados es Devuelto \\n2. Estos documentos ya se hicieron efectivos. \\nRevise por favor");
							return;
						}
					}
					else
					{
						lbInfo.Text="Datos Consignación :";
						tablaDatos=new DataTable();
						tablaDatos=ds.Tables[0];
						Session["tablaDatos"]=tablaDatos;
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
						hdnTip.Value="CS";
					}
				}
					//DEVOLUCIONES
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="DE")
				{
					//Si es una devolucion
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pcue_codigo AS CUENTA,M.mtes_fecha AS FECHA,D.mfac_codigo AS \"PREFIJO FACTURA\",D.mfac_numero AS \"NUMERO FACTURA\",D.mtes_codcons AS \"PREFIJO CONSIGNACION\",D.mtes_numcons AS \"NUMERO CONSIGNACION\",E.mcaj_codigo AS \"PREFIJO RECIBO CAJA\",E.mcaj_numero AS \"NUMERO RECIBO CAJA\",E.dtes_numerodoc AS \"NUMERO DOCUMENTO\",E.dtes_valor AS VALOR,P.pban_codigo AS BANCO FROM mtesoreria M,dtesoreriadevoluciones D,dtesoreriadocumentos E,mcaja J,mcajapago P WHERE M.pdoc_codigo=D.mtes_codigo AND M.mtes_numero=D.mtes_numero AND D.mtes_codcons=E.mtes_codigo AND D.mtes_numcons=E.mtes_numero AND D.dtes_consecutivo=E.dtes_consecutivo AND E.mcaj_codigo=J.pdoc_codigo AND E.mcaj_numero=J.mcaj_numero AND J.pdoc_codigo=P.pdoc_codigo AND J.mcaj_numero=P.mcaj_numero AND M.pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"' AND M.mtes_numero="+this.numeroDocumento.SelectedValue+" AND P.test_estado='D'");
					if(ds.Tables[0].Rows.Count!=0)
					{
						lbInfo.Text="Datos Devolución :";
						lbEfectivo.Text="";
						Cargar_Tabla_Devoluciones();
						if(SePuedeAnularDevolucion(tipoDocAnular.SelectedValue,numeroDocumento.SelectedValue))
						{
							for(i=0;i<ds.Tables[0].Rows.Count;i++)
								tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
							Session["tablaDatos"]=tablaDatos;
							dgDev.DataSource=tablaDatos;
							dgDev.DataBind();
							DatasToControls.Aplicar_Formato_Grilla(dgDev);
							dgDev.Visible=true;
							hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldoencanje*-1 FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
							hdnTip.Value="DE";
						}
						else
						{
                            Utils.MostrarAlerta(Response, "Imposible anular la devolución. Alguno de los cheques devueltos fue reconsignado");
							return;
						}
					}
                    else
                    {
                        lbInfo.Text = "NO HAY Documentos para Anular";
                        sinregistros = true;
                    }
				}
					//REMISIONES A FINANCIERAS
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="RF")
				{
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MCAJ_CODIGO AS \"PREFIJO RECIBO CAJA\", MCAJ_NUMERO AS \"NUMERO RECIBO CAJA\", MCAJ_CONS AS \"ID PAGO\", MNIT_NITFINANCIERA AS \"NIT FINANCIERA\", DTES_VALOR AS \"VALOR\", DTES_NUMDOC AS \"NUMERO CHEQUE\", MFAC_CODIGO AS \"PREFIJO FACTURA\", MFAC_NUMERO AS \"NUMERO FACTURA\" FROM DBXSCHEMA.DTESORERIAREMISION WHERE MTES_CODIGO='"+tipoDocAnular.SelectedValue+"' AND MTES_NUMERO="+numeroDocumento.SelectedValue+"");
					if(ds.Tables[0].Rows.Count!=0)
					{
						lbInfo.Text="Datos Remisión :";
						lbEfectivo.Text="";
						Cargar_Tabla_Remisiones(); 
						for(i=0;i<ds.Tables[0].Rows.Count;i++)
							tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
						Session["tablaDatos"]=tablaDatos;
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						DatasToControls.JustificacionGrilla(gridDatos,tablaDatos);
						hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
						hdnTip.Value="RF";
						lbTitulo.Text="Prefijo Nota Devolución  : ";
						lbTitulo.Visible=true;
						ddlPrefDev.Visible=true;
					}
                    else
                    {
                        lbInfo.Text = "NO HAY Documentos para Anular";
                        sinregistros = true;
					}
				}
					//DEVOLUCIONES DE CHEQUES DE FINANCIERAS
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="DF")
				{
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DTD.mtes_codigoremision AS \"PREFIJO REMISION\",DTD.mtes_numeroremision AS \"NUMERO REMISION\",DTD.dtes_numerocheque AS \"NUMERO CHEQUE\",DTD.dtes_valorcheque AS \"VALOR\",DTD.mfac_codigo AS \"PREFIJO FACTURA CLIENTE\",DTD.mfac_numero AS \"NUMERO FACTURA CLIENTE\",DTR.mcaj_codigo AS \"PREFIJO RECIBO CAJA\",DTR.mcaj_numero AS \"NUMERO RECIBO CAJA\",DTD.MFAC_CODIORDEPAGO AS \"PREFIJO DEVOLUCION FINANCIERA\",DTD.mfac_numeordepago AS \"NUMERO DEVOLUCION FINANCIERA\" FROM dbxschema.dtesoreriadevolucionremision DTD,dbxschema.dtesoreriaremision DTR WHERE DTD.mtes_codigoremision=DTR.mtes_codigo AND DTD.mtes_numeroremision=DTR.mtes_numero AND DTD.dtes_numerocheque=DTR.dtes_numdoc AND DTD.mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND DTD.mtes_numero="+numeroDocumento.SelectedValue+"");
					if(ds.Tables[0].Rows.Count!=0)
					{
						if(SePuedeAnularDevolucionRemision(ds.Tables[0]))
						{
							lbInfo.Text="Datos Devolución Remisión : ";
							lbEfectivo.Text="";
							Cargar_Tabla_DevRemisiones();
							for(i=0;i<ds.Tables[0].Rows.Count;i++)
								tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
							Session["tablaDatos"]=tablaDatos;
							dgDevRem.DataSource=tablaDatos;
							dgDevRem.DataBind();
							DatasToControls.Aplicar_Formato_Grilla(dgDevRem);
							dgDevRem.Visible=true;
							lbTitulo.Text="Prefijo Nota Devolución : ";
							lbTitulo.Visible=true;
							ddlPrefDevProv.Visible=true;
							hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo*-1 FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
							hdnTip.Value="DF";
						}
						else
						{
                            Utils.MostrarAlerta(Response, "Imposible anular la devolución. Alguno de los cheques devueltos fue reconsignado");
							return;
						}
					}
                    else
                    {
                        lbInfo.Text = "NO HAY Documentos para Anular";
                        sinregistros = true;
					}
				}

					//TRASLADOS ENTRE CUENTAS
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="TC")
				{
					ds=new DataSet();
					//Si es un traslado con carta
					if(DBFunctions.RecordExist("SELECT * FROM dtesoreriatraslados WHERE mtes_codigo='"+this.tipoDocAnular.SelectedValue+"' AND mtes_numero="+this.numeroDocumento.SelectedValue+""))
					{
						DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.mtes_fecha AS FECHA,D.pcue_codigoori AS CUENTAORI,D.pcue_codigodes AS CUENTADES FROM mtesoreria M,dtesoreriatraslados D WHERE M.pdoc_codigo=D.mtes_codigo AND M.mtes_numero=D.mtes_numero AND M.pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"' AND M.mtes_numero="+this.numeroDocumento.SelectedValue+"");
						lbInfo.Text="Información del Traslado";
						lbEfectivo.Text="Total Traslado :" + Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+this.tipoDocAnular.SelectedValue+"' AND mtes_numero="+this.numeroDocumento.SelectedValue+" AND mtes_saldo>0")).ToString("C");
						Cargar_Tabla_Traslados();
						for(i=0;i<ds.Tables[0].Rows.Count;i++)
							tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
						Session["tablaDatos"]=tablaDatos;
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
						hdnTip.Value="TC";
					}
						//Si es un traslado con cheque
					else
					{
						DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.mtes_fecha AS FECHA,H.pcue_codigoori AS CUEORI,H.pcue_codigodes AS CUEDES,H.mcaj_codigo AS CODIGOCAJA,H.mcaj_numero AS NUMEROCAJA,T.ttip_nombre AS TIPODOC,H.dtes_numerodoc AS NUMERODOC FROM mtesoreria M,dtesoreriatrasladoscheque H,ttipopago T WHERE M.pdoc_codigo=H.mtes_codigo AND M.mtes_numero=H.mtes_numero AND T.ttip_codigo=H.ttip_tipopago AND M.pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"' AND M.mtes_numero="+this.numeroDocumento.SelectedValue+"");
						lbInfo.Text="Información del Traslado";
						lbEfectivo.Text="Total Traslado :" + Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+this.tipoDocAnular.SelectedValue+"' AND mtes_numero="+this.numeroDocumento.SelectedValue+" AND mtes_saldo>0")).ToString("C");
						Cargar_Tabla_TrasladosCh();
						for(i=0;i<ds.Tables[0].Rows.Count;i++)
							tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
						Session["tablaDatos"]=tablaDatos;
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
						hdnTip.Value="TC";
					}
				}
					//NOTAS BANCARIAS DEBITO
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="ND")
				{
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mcue_codipuc AS CUENTA,dtes_valor AS VALOR FROM dbxschema.dtesorerianota WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
					Cargar_Tabla_Notas();
					for(i=0;i<ds.Tables[0].Rows.Count;i++)
						tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
					Session["tablaDatos"]=tablaDatos;
					gridDatos.DataSource=tablaDatos;
					gridDatos.DataBind();
					hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
					hdnTip.Value="ND";
				}
					//NOTAS BANCARIAS CREDITO
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="NR")
				{
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mcue_codipuc AS CUENTA,dtes_valor AS VALOR FROM dbxschema.dtesorerianota WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
					Cargar_Tabla_Notas();
					for(i=0;i<ds.Tables[0].Rows.Count;i++)
						tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
					Session["tablaDatos"]=tablaDatos;
					gridDatos.DataSource=tablaDatos;
					gridDatos.DataBind();
					DatasToControls.JustificacionGrilla(gridDatos,tablaDatos);
					hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
					hdnTip.Value="NR";
				}
					//SI ES UNA ENTREGA DE CHEQUES
				else if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.tipoDocAnular.SelectedValue+"'")=="EC")
				{
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DTES.mcaj_codigo AS \"PREFIJO COMPROBANTE DE EGRESO\",DTES.mcaj_numero AS \"NUMERO COMPROBANTE DE EGRESO\",DTES.dtes_nitben AS \"NIT DEL BENEFICIARIO\",DTES.dtes_numerodoc AS \"NUMERO DEL DOCUMENTO\",MCAJ.PBAN_CODIGO AS BANCO,MCAJ.MCPAG_VALOR AS VALOR FROM DBXSCHEMA.DTESORERIAENTREGAS DTES,DBXSCHEMA.MCAJAPAGO MCAJ WHERE MCAJ.PDOC_CODIGO=DTES.MCAJ_CODIGO AND MCAJ.MCAJ_NUMERO=DTES.MCAJ_NUMERO AND MCAJ.MCPAG_NUMERODOC=CAST(DTES.DTES_NUMERODOC AS CHAR(15)) AND DTES.MTES_CODIGO='"+tipoDocAnular.SelectedValue+"' AND DTES.MTES_NUMERO="+numeroDocumento.SelectedValue+"");
					if(ds.Tables[0].Rows.Count!=0)
					{
						lbInfo.Text="Datos de la Entrega de Cheques";
						Cargar_Tabla_Entregas();
						for(i=0;i<ds.Tables[0].Rows.Count;i++)
							tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
						Session["tablaDatos"]=tablaDatos;
						gridDatos.DataSource=tablaDatos;
						gridDatos.DataBind();
						hdnTotal.Value=DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+tipoDocAnular.SelectedValue+"' AND mtes_numero="+numeroDocumento.SelectedValue+"");
						hdnTip.Value="EC";
					}
                    else
                    {
                        lbInfo.Text = "NO HAY Documentos para Anular";
                        sinregistros = true;
                    }
				}
				aceptar.Enabled=false;
				tipoDocAnular.Enabled=false;
				numeroDocumento.Enabled=false;
				((Label)padre.FindControl("lbDetalle")).Text="Detalle Anulación :";
				((TextBox)padre.FindControl("detalleTransaccion")).Visible=true;
				((TextBox)padre.FindControl("valorConsignado")).Visible=false;
				((TextBox)padre.FindControl("totalEfectivo")).Visible=false;
				((Panel)padre.FindControl("panelValores")).Visible=true;
                if ( sinregistros == false)
				((Button)padre.FindControl("guardar")).Enabled=true;
			}
		}

		protected void btCan_Click(object sender, System.EventArgs e)
		{
			Control padre=(this.Parent).Parent;
			Session.Clear();
			gridDatos.DataBind();
			lbEfectivo.Text=lbInfo.Text=hdnTip.Value=hdnTotal.Value="";
			tipoDocAnular.Enabled=numeroDocumento.Enabled=aceptar.Enabled=true;
			dgDev.Visible=false;
			lbTitulo.Visible=false;
			ddlPrefDev.Visible=false;
			((Button)padre.FindControl("guardar")).Enabled=false;
		}

		public bool SePuedeAnularConsignacion(string prefijo,string numero)
		{
			bool sePuede=true;
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM dtesoreriadocumentos WHERE mtes_codigo='"+prefijo+"' AND mtes_numero="+numero+"");
			if(ds.Tables[0].Rows.Count!=0)
			{
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					if(EstadoDocumento(ds.Tables[0].Rows[i][4].ToString(),ds.Tables[0].Rows[i][5].ToString(),ds.Tables[0].Rows[i][6].ToString(),ds.Tables[0].Rows[i][7].ToString(),ds.Tables[0].Rows[i][8].ToString())!="G")
					{
						sePuede=false;
						break;
					}
				}
			}
			return sePuede;
		}

		public string EstadoDocumento(string prefCaj,string numCaj,string tipPag,string numDoc,string ban)
		{
			string estado="";
			estado=DBFunctions.SingleData("SELECT test_estado FROM mcajapago WHERE pdoc_codigo='"+prefCaj+"' AND mcaj_numero="+numCaj+" AND pban_codigo='"+ban+"' AND ttip_codigo='"+tipPag+"' AND mcpag_numerodoc='"+numDoc+"'");
			return estado;
		}

		private bool SePuedeAnularDevolucion(string prefijo,string numero)
		{
			bool sePuede=true;
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DTDV.mtes_codigo AS prefijo_devolucion, DTDV.mtes_numero AS numero_devolucion, DTDV.mfac_codigo AS prefijo_factura, DTDV.mfac_numero AS numero_factura, MTES.pcue_codigo AS cuenta, DTD.mtes_codigo AS codigo_consignacion, DTD.mtes_numero AS numero_consignacion, DTD.mcaj_codigo AS codigo_recibo, DTD.mcaj_numero AS numero_recibo, MCP.mcpag_numerodoc AS numero_cheque, MCP.mcpag_valor AS valor_cheque,MCP.pban_codigo AS banco FROM dbxschema.dtesoreriadevoluciones DTDV, dbxschema.dtesoreriadocumentos DTD, dbxschema.mcajapago MCP, dbxschema.mtesoreria MTES WHERE DTDV.mtes_codigo=MTES.pdoc_codigo AND DTDV.mtes_numero=MTES.mtes_numero AND DTD.mtes_codigo=DTDV.mtes_codcons AND DTD.mtes_numero=DTDV.mtes_numcons AND MCP.pdoc_codigo=DTD.mcaj_codigo AND MCP.mcaj_numero=DTD.mcaj_numero AND MCP.mcpag_numerodoc=DTD.dtes_numerodoc AND MCP.test_estado='D' AND DTDV.mtes_codigo='"+prefijo+"' AND DTDV.mtes_numero="+numero+"");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				if(BuscarChequeReconsignado(ds.Tables[0].Rows[i][7].ToString(),ds.Tables[0].Rows[i][8].ToString(),ds.Tables[0].Rows[i][9].ToString(),ds.Tables[0].Rows[i][11].ToString()))
				{
					sePuede=false;
					break;
				}
			}
			return sePuede;
		}

		private bool BuscarChequeReconsignado(string prefijo,string numero,string cheque,string banco)
		{
			bool existe=false;
			if(DBFunctions.RecordExist("SELECT MCP.mcpag_numerodoc FROM dbxschema.mcajapago MCP,dbxSchema.pdocumento PDOC WHERE MCP.pdoc_codigo=PDOC.pdoc_codigo AND MCP.mcpag_numerodoc='"+cheque+"' AND MCP.pban_codigo='"+banco+"' AND (MCP.pdoc_codigo,MCP.mcaj_numero) NOT IN(SELECT pdoc_codigo,mcaj_numero FROM dbxschema.mcajapago where pdoc_codigo='"+prefijo+"' AND mcaj_numero="+numero+") AND PDOC.tdoc_tipodocu<>'RP'"))
				existe=true;
			return existe;
		}

		private bool SePuedeAnularDevolucionRemision(DataTable dt)
		{
			bool sePuede=true;
			for(int i=0;i<dt.Rows.Count;i++)
			{
				if(BuscarChequeReconsignado(dt.Rows[i][4].ToString(),dt.Rows[i][5].ToString(),dt.Rows[i][6].ToString()))
				{
					sePuede=false;
					break;
				}
			}
			return sePuede;
		}

		private bool BuscarChequeReconsignado(string prefijo,string numero,string cheque)
		{
			bool existe=false;
			if(DBFunctions.RecordExist("SELECT MCP.mcpag_numerodoc FROM dbxschema.mcajapago MCP,dbxschema.pdocumento PDOC WHERE MCP.pdoc_codigo=PDOC.pdoc_codigo AND MCP.mcpag_numerodoc='"+cheque+"' AND (MCP.pdoc_codigo,MCP.mcaj_numero) NOT IN(SELECT pdoc_codigo,mcaj_numero FROM dbxschema.mcajapago where pdoc_codigo='"+prefijo+"' AND mcaj_numero="+numero+") AND PDOC.tdoc_tipodocu<>'RP'"))
				existe=true;
			return existe;
		}

		private bool ExistePrefijoConsignacion(ArrayList al,string prefijo)
		{
			bool existe=false;
			for(int i=0;i<al.Count;i++)
			{
				if(al[i].ToString()==prefijo)
				{
					existe=true;
					break;
				}
			}
			return existe;
		}
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
			this.dgDev.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgDev_ItemDataBound);
			this.dgDevRem.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgDevRem_ItemDataBound);

		}
		#endregion

		private void dgDev_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(e.Item.ItemType==ListItemType.AlternatingItem || e.Item.ItemType==ListItemType.Item)
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[10].FindControl("ddlPrefNot")), "SELECT pdoc_codigo,pdoc_codigo ||'-'|| pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='NC' and tvig_vigencia = 'V' ");
		}

		private void ConstruirDataTableDevoluciones(string prefijo,string numero)
		{
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT M.pcue_codigo AS CUENTA,M.mtes_fecha AS FECHA,D.mfac_codigo AS \"PREFIJO FACTURA\",D.mfac_numero AS \"NUMERO FACTURA\",D.mtes_codcons AS \"PREFIJO CONSIGNACION\",D.mtes_numcons AS \"NUMERO CONSIGNACION\" FROM dbxschema.mtesoreria M,dbxschema.dtesoreriadevoluciones D WHERE M.pdoc_codigo=D.mtes_codigo AND M.mtes_numero=D.mtes_numero AND M.pdoc_codigo='"+prefijo+"' AND M.mtes_numero="+numero+"");
		}

		private ArrayList SacarPrefijosConsignacion(DataTable dt)
		{
			ArrayList prefijos=new ArrayList();
			for(int i=0;i<dt.Rows.Count;i++)
			{
				if(!ExistePrefijoConsignacion(prefijos,dt.Rows[i][4].ToString()))
					prefijos.Add(dt.Rows[i][4].ToString());
			}
			return prefijos;
		}

		private void dgDevRem_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(e.Item.ItemType==ListItemType.AlternatingItem || e.Item.ItemType==ListItemType.Item)
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[10].FindControl("ddlPrefNotRem")), "SELECT pdoc_codigo,pdoc_codigo ||'-'|| pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='NC' and tvig_vigencia = 'V' ");
		}
	}
}