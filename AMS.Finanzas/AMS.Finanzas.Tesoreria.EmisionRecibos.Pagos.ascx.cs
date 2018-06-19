using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
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
	public partial class Pagos : System.Web.UI.UserControl
	{
		#region Atributos
		
		protected DataTable tablaPagos,tablaRtns,numerosCheques;
        protected Label lb, lblFalta;
		protected Button aceptar,cargarPagos;
		protected Control datosCliente,encabezado,varios,noCausados,documentos,padre;
		protected int pos;
		protected ArrayList alConsig;
        protected HtmlGenericControl divflag_eliminar = new HtmlGenericControl();

		#endregion
		
		#region Métodos

		protected void Cargar_Tabla_Pagos()
		{
			tablaPagos=new DataTable();
			tablaPagos.Columns.Add(new DataColumn("TIPO", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("CODIGOBANCO", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("NUMERODOC", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("TIPOMONEDA", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("VALOR", typeof(double)));
			tablaPagos.Columns.Add(new DataColumn("VALORTC", typeof(double)));
			tablaPagos.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
			tablaPagos.Columns.Add(new DataColumn("FECHA", typeof(string)));
			tablaPagos.Columns.Add(new DataColumn("ESTADO", typeof(string)));
            tablaPagos.Columns.Add(new DataColumn("RAZON", typeof(string)));
      	}
		
		protected void Mostrar_gridPagos()
		{
			Session["tablaPagos"]=tablaPagos;
			gridPagos.DataSource=tablaPagos;
			gridPagos.DataBind();
		}
		
		protected void Cargar_Tabla_Rtns()
		{
			tablaRtns=new DataTable();
			tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
			tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
			tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
		}
		
		protected void Mostrar_gridRtns()
		{
			Session["tablaRtns"]=tablaRtns;
			gridRtns.DataSource=tablaRtns;
			gridRtns.DataBind();
		}
		
		protected void Cargar_Numeros_Cheques()
		{
			numerosCheques=new DataTable();
			numerosCheques.Columns.Add("CHEQUERA",typeof(string));
			numerosCheques.Columns.Add("ULTIMO",typeof(string));
			Session["numerosCheques"]=numerosCheques;
		}
		
		protected string Armar_String_Numeros()
		{
			string numeros="";
			for(int i=0;i<numerosCheques.Rows.Count;i++)
			{
				if(i!=numerosCheques.Rows.Count-1)
					numeros+=numerosCheques.Rows[i][0].ToString()+"a"+numerosCheques.Rows[i][1].ToString()+"b";
				else
					numeros+=numerosCheques.Rows[i][0].ToString()+"a"+numerosCheques.Rows[i][1].ToString();
			}
			return numeros;
		}

		protected bool Buscar_Pago(string pago)
		{
			bool encontrado=false;
			if(tablaPagos!=null)
			{
				if(tablaPagos.Rows.Count!=0)
				{
					for(int i=0;i<tablaPagos.Rows.Count;i++)
					{
						if(tablaPagos.Rows[i][0].ToString()==pago)
							encontrado=true;
					}
				}
			}
			return encontrado;
		}
		
		protected bool Buscar_Documento(string banco,string numero)
		{
			bool encontrado=false;
			DataRow[] filas=tablaPagos.Select("CODIGOBANCO='"+banco+"' AND NUMERODOC='"+numero+"'");
			if(filas.Length!=0)
				encontrado=true;
			return encontrado;
		}

		protected bool Buscar_Documento(string banco,string numero,string tipo)
		{
			bool encontrado=false;
			if(DBFunctions.RecordExist("SELECT pdoc_codigo FROM dbxschema.mcajapago WHERE pban_codigo='"+banco+"' AND mcpag_numerodoc='"+numero+"' AND ttip_codigo='"+tipo+"'"))
				encontrado=true;
			return encontrado;
		}

		protected bool Buscar_Banco_Cuenta(string banco)
		{
			bool encontrado=false;
			DataRow[] filas=tablaPagos.Select("CODIGOBANCO='"+banco+"'");
			if(filas.Length!=0)
				encontrado=true;
			return encontrado;
		}
		
        protected void Cargar_Controles(string pago,DataGridCommandEventArgs e)
		{
			((TextBox)e.Item.Cells[1].Controls[1]).Text="";
			((TextBox)e.Item.Cells[1].Controls[5]).Text="";
			((TextBox)e.Item.Cells[2].Controls[1]).Text="";
			if(pago=="E" || pago=="DC" || pago=="DL" || pago=="X")
			{
				((TextBox)e.Item.Cells[1].Controls[1]).Enabled=false;
				((TextBox)e.Item.Cells[2].Controls[1]).Enabled=false;
				((RadioButton)e.Item.Cells[3].Controls[1]).Enabled=true;
				((RadioButton)e.Item.Cells[3].Controls[1]).Checked=true;
				((RadioButton)e.Item.Cells[3].Controls[3]).Enabled=true;
				((TextBox)e.Item.Cells[4].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[5].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[6].Controls[1]).Enabled=false;
				((TextBox)e.Item.Cells[6].Controls[1]).Text="0";
				((TextBox)e.Item.Cells[7].Controls[1]).Text=System.DateTime.Now.Date.ToString("yyyy-MM-dd");
				((TextBox)e.Item.Cells[7].Controls[1]).ReadOnly=true;
                ((TextBox)e.Item.Cells[8].Controls[1]).ReadOnly = false;
				((Button)e.Item.Cells[9].Controls[1]).Enabled=true;
			}
			else if(pago=="D" || pago=="T")
			{
				((TextBox)e.Item.Cells[1].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[2].Controls[1]).Enabled=true;
				((RadioButton)e.Item.Cells[3].Controls[1]).Enabled=true;
				((RadioButton)e.Item.Cells[3].Controls[1]).Checked=true;
				((RadioButton)e.Item.Cells[3].Controls[3]).Enabled=true;
				((TextBox)e.Item.Cells[4].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[5].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[6].Controls[1]).Enabled=true;
				if(Request.QueryString["tipo"]=="RC")
					((TextBox)e.Item.Cells[6].Controls[1]).Text=(Convert.ToDouble(((TextBox)documentos.FindControl("totalCli")).Text.Substring(1))).ToString();
				else if(Request.QueryString["tipo"]=="CE")
					((TextBox)e.Item.Cells[6].Controls[1]).Text=(Convert.ToDouble(((TextBox)documentos.FindControl("totalPro")).Text.Substring(1))).ToString();
				((TextBox)e.Item.Cells[7].Controls[1]).Text=System.DateTime.Now.Date.ToString("yyyy-MM-dd");
				((TextBox)e.Item.Cells[7].Controls[1]).ReadOnly=true;
                ((TextBox)e.Item.Cells[8].Controls[1]).ReadOnly = true;
				((Button)e.Item.Cells[9].Controls[1]).Enabled=true;
			}
			else if(pago=="CD" || pago=="B")
			{
				((TextBox)e.Item.Cells[1].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[2].Controls[1]).Enabled=true;
				((RadioButton)e.Item.Cells[3].Controls[1]).Enabled=true;
				((RadioButton)e.Item.Cells[3].Controls[1]).Checked=true;
				((RadioButton)e.Item.Cells[3].Controls[3]).Enabled=true;
				((TextBox)e.Item.Cells[4].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[5].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[6].Controls[1]).Enabled=false;
				((TextBox)e.Item.Cells[6].Controls[1]).Text="0";
				((TextBox)e.Item.Cells[7].Controls[1]).Text=System.DateTime.Now.Date.ToString("yyyy-MM-dd");
				((TextBox)e.Item.Cells[7].Controls[1]).ReadOnly=true;
                ((TextBox)e.Item.Cells[8].Controls[1]).ReadOnly = true;
				((Button)e.Item.Cells[9].Controls[1]).Enabled=true;
			}
            else if (pago == "C" || pago == "CC")
			{
				((TextBox)e.Item.Cells[1].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[2].Controls[1]).Enabled=true;
				((RadioButton)e.Item.Cells[3].Controls[1]).Enabled=true;
				((RadioButton)e.Item.Cells[3].Controls[1]).Checked=true;
				((RadioButton)e.Item.Cells[3].Controls[3]).Enabled=true;
				((TextBox)e.Item.Cells[4].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[5].Controls[1]).Enabled=true;
				((TextBox)e.Item.Cells[6].Controls[1]).Enabled=false;
				((TextBox)e.Item.Cells[6].Controls[1]).Text="0";
				((TextBox)e.Item.Cells[7].Controls[1]).Text=System.DateTime.Now.Date.ToString("yyyy-MM-dd");
				((TextBox)e.Item.Cells[7].Controls[1]).ReadOnly=false;
				((TextBox)e.Item.Cells[7].Controls[1]).Enabled=true;
                ((TextBox)e.Item.Cells[8].Controls[1]).ReadOnly = true;
				((Button)e.Item.Cells[9].Controls[1]).Enabled=true;
			}
		}
		
		protected bool Validar_Datos_Efectivo(DataGridCommandEventArgs e)
		{
			bool error=false;
			//Si falta algun campo por llenar que muestre un aviso
			if((!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text)) || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text)))
			{
                Utils.MostrarAlerta(Response, "Falta un campo por llenar");
				error=true;
			}
				//Si escogio tipo de moneda nacional e introdujo algun valor para la tasa de cambio diferente de 1
				//muestra un mensaje de advertencia
			else if((((RadioButton)e.Item.Cells[3].Controls[1]).Checked==true)&&(((TextBox)e.Item.Cells[5].Controls[1]).Text!="1"))
			{
                Utils.MostrarAlerta(Response, "El tipo de moneda es nacional por tanto no se admite valor de cambio");
				((TextBox)e.Item.Cells[5].Controls[1]).Text="1";
				error=true;
			}
				//Si escogio tipo de moneda extranjera e introdujo 0 en el valor de la tasa de cambio
				//muestra un mensaje de advertencia
			else if((((RadioButton)e.Item.Cells[3].Controls[3]).Checked==true)&&(((TextBox)e.Item.Cells[5].Controls[1]).Text=="0"))
			{
                Utils.MostrarAlerta(Response, "Debe introducir un valor distinto de 0 para la tasa de cambio");
				error=true;
			}

            if (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "DC")
            {
                string cuentaPUC = DBFunctions.SingleData("SELECT mcue_codipuc FROM dbxschema.mcuenta where timp_codigo IN ('A','P') and tCUE_codigo <> 'N' and mcue_codipuc = '" + ((TextBox)e.Item.Cells[1].FindControl("codbantxt3")).Text + "';");
                string nitPUC = DBFunctions.SingleData("select mnit_nit from mnit where mnit_nit = '" + ((TextBox)e.Item.Cells[2].FindControl("codbantxt4")).Text + "';");
                if (cuentaPUC == "")
                {
                    Utils.MostrarAlerta(Response, "La cuenta PUC ingresada no esta definida en el sistema o es NO IMPUTABLE o es solo NIIF");
                    error = true;
                }
                else if (nitPUC == "")
                {
                    Utils.MostrarAlerta(Response, "El NIT ingresado no esta definido en el sistema");
                    error = true;
                }
            }

			return error;
		}
		
		protected bool Validar_Datos_Documentos(DataGridCommandEventArgs e)
		{
			bool error=false;
			//Si falta algun campo por llenar que muestre un aviso
			if((((TextBox)e.Item.Cells[1].Controls[1]).Text=="") || (((TextBox)e.Item.Cells[2].Controls[1]).Text=="") || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text)) || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text)) || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text)) || (!DatasToControls.ValidarDateTime(((TextBox)e.Item.Cells[7].Controls[1]).Text)))
			{
                Utils.MostrarAlerta(Response, "Faltan Datos");
				error=true;
			}
			//Si escogio tipo de moneda nacional e introdujo algun valor para la tasa de cambio diferente de 1
			//muestra un mensaje de advertencia
			else if((((RadioButton)e.Item.Cells[3].Controls[1]).Checked==true)&&(((TextBox)e.Item.Cells[5].Controls[1]).Text!="1"))
			{
                Utils.MostrarAlerta(Response, "El tipo de moneda es nacional por tanto no se admite valor de cambio");
				((TextBox)e.Item.Cells[5].Controls[1]).Text="1";
				error=true;
			}
			//Si escogio tipo de moneda extranjera e introdujo 0 en el valor de la tasa de cambio
			//muestra un mensaje de advertencia
			else if((((RadioButton)e.Item.Cells[3].Controls[3]).Checked==true)&&(((TextBox)e.Item.Cells[5].Controls[1]).Text=="0"))
			{
                Utils.MostrarAlerta(Response, "Debe introducir un valor distinto de 0 para la tasa de cambio");
				error=true;
			}
            //Si es un pago en cheque o un giro en cheque
            if (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "C")
            {
                //Si es un provisional
                if (Request.QueryString["tipo"] == "RP")
                {
                    //Si la fecha digitada es menor o igual a la actual
                    if (Convert.ToDateTime(((TextBox)e.Item.Cells[7].Controls[1]).Text) <= Convert.ToDateTime(((TextBox)encabezado.FindControl("fecha")).Text))
                    {
                        Utils.MostrarAlerta(Response, "La fecha del cheque no puede ser menor o igual a la del recibo");
                        error = true;
                    }
                }
                //Si es un egreso y la chequera maneja consecutivo automatico
                else if (Request.QueryString["tipoPagoa"] == "CE" && (((TextBox)e.Item.Cells[2].FindControl("codbantxt4")).ReadOnly == true || ((((TextBox)e.Item.Cells[2].FindControl("codbantxt4")).Attributes["readonly"] != null) && ((TextBox)e.Item.Cells[2].FindControl("codbantxt4")).Attributes["readonly"].Equals("readonly"))))
                {
                    //Si el número calculado excede la totalidad de la chequera
                    if (Convert.ToInt32(DBFunctions.SingleData("SELECT pche_numefin FROM dbxschema.pchequera WHERE pche_id=" + ((TextBox)e.Item.Cells[1].Controls[5]).Text.Trim() + " AND tres_manejacons='S'")) < Convert.ToInt32(((TextBox)e.Item.Cells[2].Controls[1]).Text))
                    {
                        Utils.MostrarAlerta(Response, "El cheque está fuera del rango de la chequera");
                        error = true;
                    }
                    //Si el cheque ya existe en el datatable
                    else if (this.Buscar_Documento((((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim() + "-" + ((TextBox)e.Item.Cells[1].Controls[5]).Text.Trim()), ((TextBox)e.Item.Cells[2].Controls[1]).Text))
                    {
                        Utils.MostrarAlerta(Response, "Número de cheque repetido");
                        error = true;
                    }
                    //Si la fecha digitada es menor a la actual
                    else if (Convert.ToDateTime(((TextBox)e.Item.Cells[7].Controls[1]).Text) < Convert.ToDateTime(((TextBox)encabezado.FindControl("fecha")).Text))
                    {
                        Utils.MostrarAlerta(Response, "La fecha del cheque no puede ser menor a la del recibo");
                        error = true;
                    }
                }
                //Si es un recibo de caja
                else
                {
                    //Si la fecha digitada es menor a la actual EN MAS DE 178 DIAS hector
                    if (Convert.ToDateTime(((TextBox)e.Item.Cells[7].Controls[1]).Text) < Convert.ToDateTime(((TextBox)encabezado.FindControl("fecha")).Text))
                    {
                        Utils.MostrarAlerta(Response, "La fecha del cheque no puede ser menor a la del recibo");
                        error = true;
                    }
                }
            }
            //Si es un pago en Transferencia Bancaria
            else if (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "B")
            {
                //Si existe en mtesoreria ya un pago por banca electronica para este recibo
                if (DBFunctions.RecordExist("Select PDOC_CODIGO||MTES_NUMERO FROM MTESORERIA WHERE PDOC_CODIGO = '"+ Request.QueryString["pre"] + "' AND MTES_NUMERO = "+ Convert.ToInt32(Request.QueryString["num"]) + "  ; "))
                {
                    Utils.MostrarAlerta(Response, "Ya existe registro de Banca Electrónica para este recibo de caja");
                    error = true;
                }
            }
				//Si ya existen pagos, busco si la relación pago-banco-numero ya existe
                if (tablaPagos.Rows.Count!=0)
				{
                    string banco = ((TextBox)e.Item.Cells[1].FindControl("codbantxt")).Text;
                    if (((DropDownList)e.Item.Cells[0].FindControl("tipoPagoa")).SelectedValue == "B")
                        banco += "-"+banco;
                    DataRow[] docs=tablaPagos.Select("TIPO='"+((DropDownList)e.Item.Cells[0].FindControl("tipoPagoa")).SelectedValue+"' AND CODIGOBANCO = '"+ banco +"'  AND NUMERODOC='"+((TextBox)e.Item.Cells[2].FindControl("codbantxt4")).Text+"'");
					if(docs.Length!=0)
					{
                        Utils.MostrarAlerta(Response, "La relación Tipo de Pago, Banco y Número Documento ya existe");
						error=true;
					}
				}
	//		}
			//Finalmente revisamos que no exista previamente un pago con las mismas características
			if(Buscar_Documento(((TextBox)e.Item.FindControl("codbantxt")).Text,((TextBox)e.Item.FindControl("codbantxt4")).Text,((DropDownList)e.Item.FindControl("tipoPagoa")).SelectedValue))
			{
                Utils.MostrarAlerta(Response, "La relación banco-número de documento-tipo de pago ya existe en la base de datos. Imposible registrar pago");
				error=true;
			}
			//Si es una tranferencia bancaria
			if(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="B")
			{
                string msgTransf = "";
				if(Buscar_Banco_Cuenta((((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim()+"-"+((TextBox)e.Item.Cells[1].Controls[5]).Text.Trim())))
				{
                    msgTransf = "Ya hay transferencias de esta cuenta \n";
		  	//		error=true;         
				}
				if(Buscar_Pago("B"))
				{
                    msgTransf += "Ya hay un pago de tipo transferencia bancaria, asegurese que sea correcto \n";
		 	//		error=true;
				}
                if (msgTransf.Length > 0)
                    Utils.MostrarAlerta(Response, msgTransf);
            }
			return error;
		}
		
		protected void Llenar_Tabla_Pagos_Efectivo(string tipo, string cuentaPUC, string numIdentificacion, string moneda,double valor,double tasa,double vBase,string fecha, string razonObservacion, string estado)
		{
			DataRow fila;
			if(Session["tablaPagos"]==null)
				this.Cargar_Tabla_Pagos();
			fila=tablaPagos.NewRow();
			fila[0]=tipo;//Tipo de Pago
            fila[1]= cuentaPUC;//Numero de cuenta (PUC)
            fila[2]= numIdentificacion;//Numero de identificacion (cedula)
			fila[3]=moneda;//Moneda Nacional o Extranjera
			fila[4]=valor;//Valor
			fila[5]=tasa;//Tasa de Cambio
			fila[6]=vBase;//Valor Base
			fila[7]=fecha;//Fecha
			fila[8] = estado;//Estado del Pago
            fila[9] = razonObservacion;//Razon del descuento.
			tablaPagos.Rows.Add(fila);
			hdnval.Value=(Convert.ToDouble(hdnval.Value)+(valor*tasa)).ToString();
			Sumar_Pagos(valor,tasa,tipo);
		}

        protected void Llenar_Tabla_Pagos_Documentos(string tipo, string banco, string numero, string moneda, double valor, double tasa, double vBase, string fecha, string estado)
		{
			DataRow fila;
			if(Session["tablaPagos"]==null)
				this.Cargar_Tabla_Pagos();
			fila=tablaPagos.NewRow();
			fila[0]=tipo;//Tipo de Pago
			fila[1]=banco;//Banco
			fila[2]=numero;//Numero del Documento
			fila[3]=moneda;//Moneda Nacional o Extranjera
			fila[4]=valor;//Valor
			fila[5]=tasa;//Tasa de Cambio
			fila[6]=vBase;//Valor Base
			fila[7]=fecha;//Fecha
			fila[8]=estado;//Estado del Pago
           
			tablaPagos.Rows.Add(fila);
			hdnval.Value=(Convert.ToDouble(hdnval.Value)+(valor*tasa)).ToString();
			//if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue!="P" && ((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue!="R")
				Sumar_Pagos(valor,tasa,tipo);
		}
		
		public void  Cargar_Grilla_Pagos()
		{
			string tipo,banco,numero,moneda,fecha,estado;
			double valor,tasa,vBase;
			DataSet ds=new DataSet();
			if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="R")
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor/mcpag_valortasacambio,mcpag_valortasacambio,mcpag_valorbase,mcpag_fecha,test_estado FROM mcajapago P,mcaja M WHERE M.pdoc_codigo=P.pdoc_codigo AND (M.mnit_nit IN ('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"') OR M.mnit_nitben IN('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"')) AND M.mcaj_numero=P.mcaj_numero AND ttip_codigo='C' AND test_estado='D'");
				if(ds.Tables[0].Rows.Count!=0)
				{
					if(Session["tablaPagos"]==null)
						this.Cargar_Tabla_Pagos();
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						tipo=ds.Tables[0].Rows[i][0].ToString();
						banco=ds.Tables[0].Rows[i][1].ToString();
						numero=ds.Tables[0].Rows[i][2].ToString();
						moneda=ds.Tables[0].Rows[i][3].ToString();
						valor=Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
						tasa=Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());
						vBase=Convert.ToDouble(ds.Tables[0].Rows[i][6].ToString());
						fecha=Convert.ToDateTime(ds.Tables[0].Rows[i][7].ToString()).ToString("yyyy-MM-dd");
						estado=ds.Tables[0].Rows[i][8].ToString();
						this.Llenar_Tabla_Pagos_Documentos(tipo,banco,numero,moneda,valor,tasa,vBase,fecha,estado);
						gridPagos.DataSource=tablaPagos;
						gridPagos.DataBind();
						Session["tablaPagos"]=tablaPagos;
					}
				}
			}
			else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="P")
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor/mcpag_valortasacambio,mcpag_valortasacambio,mcpag_valorbase,mcpag_fecha,test_estado FROM dbxschema.mcajapago P,dbxschema.mcaja M,dbxschema.pdocumento D WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND M.pdoc_codigo=D.pdoc_codigo AND (M.mnit_nit IN ('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"') OR M.mnit_nitben IN('"+((TextBox)encabezado.FindControl("datCli")).Text+"','"+((TextBox)encabezado.FindControl("datBen")).Text+"')) AND P.ttip_codigo='C' AND P.test_estado='C' AND D.tdoc_tipodocu='RP'");
				if(ds.Tables[0].Rows.Count!=0)
				{
					if(Session["tablaPagos"]==null)
						this.Cargar_Tabla_Pagos();
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						tipo=ds.Tables[0].Rows[i][0].ToString();
						banco=ds.Tables[0].Rows[i][1].ToString();
						numero=ds.Tables[0].Rows[i][2].ToString();
						moneda=ds.Tables[0].Rows[i][3].ToString();
						valor=Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
						tasa=Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());
						vBase=Convert.ToDouble(ds.Tables[0].Rows[i][6].ToString());
						fecha=Convert.ToDateTime(ds.Tables[0].Rows[i][7].ToString()).ToString("yyyy-MM-dd");
						estado=ds.Tables[0].Rows[i][8].ToString();
						this.Llenar_Tabla_Pagos_Documentos(tipo,banco,numero,moneda,valor,tasa,vBase,fecha,estado);
						gridPagos.DataSource=tablaPagos;
						gridPagos.DataBind();
						Session["tablaPagos"]=tablaPagos;
					}
				}
			}
		}
		
		protected bool Buscar_Numeros_Cheques(string chequera,int numero, ref int posicion)
		{
			bool  encontrado=false;
			for(int i=0;i<numerosCheques.Rows.Count;i++)
			{
				if(numerosCheques.Rows[i][0].ToString()==chequera && Convert.ToInt32(numerosCheques.Rows[i][1])==numero)
				{
					encontrado=true;
					pos=i;
				}
			}
			return encontrado;
		}

		protected bool Buscar_Retencion(string rtn)
		{
			bool encontrado=false;
			if(tablaRtns!=null && tablaRtns.Rows.Count!=0)
			{
				for(int i=0;i<tablaRtns.Rows.Count;i++)
				{
					if(tablaRtns.Rows[i][0].ToString()==rtn)
						encontrado=true;
				}
			}
			return encontrado;
		}

		protected void Sumar_Pagos(double valor,double tasa,string tipo)
		{
			double bruto=0,neto=0;
			if(valorBruto.Text==string.Empty && valorNeto.Text==string.Empty)
			{
				//Si no es desc comercial ni de ley, sumo a bruto y neto
				if(tipo!="DL" && tipo!="DC")
				{
					bruto=bruto+(valor*tasa);
					neto=neto+(valor*tasa);
				}
					//Sino solo sumo a bruto
				else
					bruto=bruto+(valor*tasa);
			}
			else
			{
				bruto=Convert.ToDouble(valorBruto.Text.Substring(1));
				neto=Convert.ToDouble(valorNeto.Text.Substring(1));
				if(tipo!="DL" && tipo!="DC")
				{
					bruto=bruto+(valor*tasa);
					neto=neto+(valor*tasa);
				}
				else
					bruto=bruto+(valor*tasa);
			}
			valorBruto.Text=bruto.ToString("C");
			valorNeto.Text=neto.ToString("C");
            Habilitar_Guardar();
		}

		protected void Restar_Pagos(double valor,double tasa,string tipo)
		{
			double bruto=0,neto=0;
			bruto=Convert.ToDouble(valorBruto.Text.Substring(1));
			neto=Convert.ToDouble(valorNeto.Text.Substring(1));
			if(tipo!="DL" && tipo!="DC")
			{
				bruto=bruto-(valor*tasa);
				neto=neto-(valor*tasa);
			}
			else
				bruto=bruto-(valor*tasa);
			valorBruto.Text=bruto.ToString("C");
			valorNeto.Text=neto.ToString("C");
		}

		protected void Sumar_Retenciones(double valor)
		{
			double bruto=0;
			try{bruto=Convert.ToDouble(valorBruto.Text.Substring(1));}
			catch{}
			bruto=bruto+valor;
			valorBruto.Text=bruto.ToString("C");
			Habilitar_Guardar();
		}

		protected void Restar_Retenciones(double valor)
		{
			double bruto=0;
			try{bruto=Convert.ToDouble(valorBruto.Text.Substring(1));}
			catch{}
			bruto=bruto-valor;
			valorBruto.Text=bruto.ToString("C");
		}

		protected void Habilitar_Guardar()
		{
			Control padre=(this.Parent).Parent;
            double valoresA = Calcular_Valores();
            if (valorBruto.Text == "" || valorNeto.Text == "")
                Utils.MostrarAlerta(Response, "Falta la relación de los medios de pago. Imposible registrar pago");
		    else
            {
                double diferencia = Convert.ToDouble(valorBruto.Text.Substring(1)) - valoresA;
			    if(Convert.ToDouble(valorBruto.Text.Substring(1))>0 && Convert.ToDouble(valorBruto.Text.Substring(1))>=Convert.ToDouble(valorNeto.Text.Substring(1)) && diferencia>=0)
			    {
				    ((Button)padre.FindControl("guardar")).Enabled=true;
                    if(diferencia>0)
                        lblFalta.Text = "Sobran " + diferencia.ToString("C");
                    else
                        lblFalta.Text = "";
			    }
			    else
			    {
				    ((Button)padre.FindControl("guardar")).Enabled=false;
                    lblFalta.Text = "Faltan " + (-diferencia).ToString("C");
			    }
            }
		}

		protected double Calcular_Valores()
		{
			double valor=0,valorInfo=0,valorConceptos=0,valorDocs=0;
			if(lbInfo.Text.IndexOf("$")!=-1)
			{
				try{valorInfo=Convert.ToDouble(lbInfo.Text.Substring(lbInfo.Text.IndexOf("$")+1));}
				catch{}
			}
			if(lbConceptos.Text.IndexOf("$")!=-1)
			{
				try{valorConceptos=Convert.ToDouble(lbConceptos.Text.Substring(lbConceptos.Text.IndexOf("$")+1));}
				catch{}
			}
			if(lbDocs.Text.IndexOf("$")!=-1)
			{
				try{valorDocs=Convert.ToDouble(lbDocs.Text.Substring(lbDocs.Text.IndexOf("$")+1));}
				catch{}
			}
			valor=valorInfo+valorConceptos+valorDocs;
			return valor;
		}

		protected void ConstruirArrayListConsignacion()
		{
			string[] cuentas=hdncue.Value.Split('@');
			alConsig.Add(cuentas[cuentas.Length-1]);
			Session["alConsig"]=alConsig;
		}

        #endregion

        #region MetodosAjax

        //[Ajax.AjaxMethod]
        //public bool validarEdicionNum(string codBanco)
        //{
        //    if (Request.QueryString["tipo"] == "CE")
        //    {
        //        bool llevaConsecutivo = DBFunctions.SingleData(
        //            String.Format("select tres_manejacons from pchequera where pche_id='{0}'", codBanco)) == "S";

        //        if (llevaConsecutivo)
        //            ((TextBox)gridPagos.FindControl("codbantxt4")).Enabled = false;
        //    }
        //    return false;

        //    //string nombre="";
        //    //nombre = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + valor + "'");
        //    //if (nombre == "")
        //    //    nombre += "Error";
        //    //return nombre;
        //}

        #endregion

		#region Eventos

		protected void Page_Load(object Sender,EventArgs e)
		{
            //string empresa = GlobalData.EMPRESA;
            padre = (this.Parent).Parent;
            encabezado = ((PlaceHolder)padre.FindControl("phEncabezado")).Controls[0];
            
            
			//datosCliente=((PlaceHolder)padre.FindControl("phDatosCliente")).Controls[0];
			documentos=((PlaceHolder)padre.FindControl("phDocumentos")).Controls[0];
			varios=((PlaceHolder)padre.FindControl("phVarios")).Controls[0];
			noCausados=((PlaceHolder)padre.FindControl("phNoCausados")).Controls[0];
			DatasToControls bind=new DatasToControls();
            
			if(!IsPostBack)
			{
                //bind.PutDatasIntoDropDownList(ddlPrefijoFactura, "SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='FC' and tvig_vigencia = 'V' ");
                //bind.PutDatasIntoDropDownList(ddlPrefNot, "SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='NC' and tvig_vigencia = 'V' ");
                //bind.PutDatasIntoDropDownList(ddlPrefNotPro, "SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='NP' and tvig_vigencia = 'V' ");
     			bind.PutDatasIntoDropDownList(ddlCuentas,"SELECT PCUE.pcue_codigo AS CODIGO,PCUE.pcue_codigo ||'-'|| PCUE.pban_banco ||'-'|| PBAN.pban_nombre ||'-'|| PCUE.pcue_numero FROM dbxschema.pcuentacorriente PCUE,dbxschema.pbanco PBAN WHERE PCUE.pban_banco=PBAN.pban_codigo and pcue.tvig_vigencia = 'V'");
				DataSet ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pche_id,CASE WHEN pche_ultche IS NULL THEN pche_numeini-1 ELSE pche_ultche END FROM dbxschema.pchequera WHERE tvig_vigencia IN('V') ORDER BY pche_id");
				if(Request.QueryString["tipo"]=="CE")
				{
					DataRow fila;
					if(numerosCheques==null)
						this.Cargar_Numeros_Cheques();
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						fila=numerosCheques.NewRow();
						fila[0]=ds.Tables[0].Rows[i][0].ToString();
						if(ds.Tables[0].Rows[i][1].GetType().ToString()=="System.DBNull")
							fila[1]="0";
						else
							fila[1]=ds.Tables[0].Rows[i][1].ToString();
						numerosCheques.Rows.Add(fila);
						Session["numerosCheques"]=numerosCheques;
					}
				}
				this.Cargar_Tabla_Pagos();
				this.Mostrar_gridPagos();
				this.Cargar_Tabla_Rtns();
				this.Mostrar_gridRtns();

                string prefnotaprov = DBFunctions.SingleData("SELECT pd.pdoc_codigo FROM pdocumento pd, ccartera cc WHERE tdoc_tipodocu='NP' and tvig_vigencia = 'V' and pd.pdoc_codigo = cc.pdoc_codirdepago;");
                string prefnotaclie = DBFunctions.SingleData("SELECT pd.pdoc_codigo FROM pdocumento pd, ccartera cc WHERE tdoc_tipodocu='NC' and tvig_vigencia = 'V' and pd.pdoc_codigo = cc.pdoc_codigo;");
                if (prefnotaclie == "" || prefnotaprov == "")
                {
                    Utils.MostrarAlerta(Response, "NO ha parametrizado el prefijo para NOTAS de Cliente y-o Proveedor por sabrantes en caja, Configure su Sistema de Cartera.");
                    return;
                }
            }
			else
			{
				if(Session["tablaPagos"]!=null)
					tablaPagos=(DataTable)Session["tablaPagos"];
				if(Session["tablaRtns"]!=null)
					tablaRtns=(DataTable)Session["tablaRtns"];
				if(Session["numerosCheques"]!=null)
					numerosCheques=(DataTable)Session["numerosCheques"];
				if(Session["alConsig"]!=null)
					alConsig=(ArrayList)Session["alConsig"];
			}
		}

		protected void gridPagos_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			string consigAuto=DBFunctions.SingleData("SELECT CTES_CONSAUTOTDTC FROM DBXSCHEMA.CTESORERIA");
			if(Request.QueryString["tipo"]=="RP")
			{
				if(e.Item.ItemType==ListItemType.Footer)
				{
					//function CambioTipoPago(obCmbTipPg,obBanco,obNumDocu,obTipMon,obVal,obValTasCmb,obValBas,obFech,lbch)
                    string sqlTipoPago = "SELECT ttip_codigo,ttip_nombre FROM ttipopago WHERE ttip_codigo='C'";
                    if (divflag_eliminar.InnerHtml == "eliminar")
                    {
                        sqlTipoPago = "SELECT ttip_codigo,ttip_nombre FROM ttipopago WHERE ttip_codigo='DC'";
                    }
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]), sqlTipoPago);
                    ((DropDownList)e.Item.Cells[0].Controls[1]).Attributes.Add("onchange","CambioTipoPago("+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+
						""+((TextBox)e.Item.Cells[2].Controls[1]).ClientID+","+
						""+((TextBox)e.Item.Cells[4].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[5].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[6].Controls[1]).ClientID+","+
						""+((TextBox)e.Item.Cells[7].Controls[1]).ClientID+","+((Button)e.Item.Cells[9].Controls[1]).ClientID+","+((Label)e.Item.Cells[1].FindControl("lbch")).ClientID+",'"+consigAuto+"'," + ((TextBox)e.Item.Cells[1].FindControl("codbantxt3")).ClientID + "," + ((TextBox)e.Item.Cells[8].Controls[1]).ClientID + ",'" + Request.QueryString["tipo"] + "');");
					((TextBox)e.Item.Cells[7].Controls[1]).Text = DateTime.Now.ToString("yyyy-MM-dd");
					if(((DropDownList)e.Item.Cells[0].Controls[1]).Items[0].Text != "-- Escoja --")
						((DropDownList)e.Item.Cells[0].Controls[1]).Items.Insert(0,new ListItem("-- Escoja --","ESC"));
					//((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pban_codigo,pban_nombre FROM pbanco',new Array());");
					//Cargar_Consulta(controltipoPago,tipoRecibo,campoNumero,numeros);
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","Cargar_Consulta(this,"+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+",'"+Request.QueryString["tipo"]+"',null,null)");
				}
			}
			else if(Request.QueryString["tipo"]=="CE")
			{
				if(e.Item.ItemType==ListItemType.Footer)
				{
                    string sqlTipoPago = "SELECT ttip_codigo,ttip_nombre FROM ttipopago";
                    if (divflag_eliminar.InnerHtml == "eliminar")
                    {
                        sqlTipoPago = "SELECT ttip_codigo,ttip_nombre FROM ttipopago WHERE ttip_codigo='DC'";
                    }
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]), sqlTipoPago);
                    ((DropDownList)e.Item.Cells[0].Controls[1]).Attributes.Add("onchange","CambioTipoPago("+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+
						""+((TextBox)e.Item.Cells[2].Controls[1]).ClientID+","+
						""+((TextBox)e.Item.Cells[4].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[5].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[6].Controls[1]).ClientID+","+
                        "" + ((TextBox)e.Item.Cells[7].Controls[1]).ClientID + "," + ((Button)e.Item.Cells[9].Controls[1]).ClientID + "," + ((Label)e.Item.Cells[1].FindControl("lbch")).ClientID + ",'" + consigAuto + "'," + ((TextBox)e.Item.Cells[1].FindControl("codbantxt3")).ClientID + "," + ((TextBox)e.Item.Cells[8].Controls[1]).ClientID + ",'" + Request.QueryString["tipo"] + "');");
					((TextBox)e.Item.Cells[7].Controls[1]).Text = DateTime.Now.ToString("yyyy-MM-dd");
					if(((DropDownList)e.Item.Cells[0].Controls[1]).Items[0].Text != "-- Escoja --")
						((DropDownList)e.Item.Cells[0].Controls[1]).Items.Insert(0,new ListItem("-- Escoja --","ESC"));
					//((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","ModalDialogPagos(this,'SELECT PBAN.pban_codigo AS Banco,PBAN.pban_nombre AS Nombre,PCHE.pcue_codigo AS Cuenta,PCHE.pche_id AS Chequera,CASE WHEN PCHE.pche_ultche IS NULL THEN PCHE.pche_numeini ELSE PCHE.pche_ultche END AS \"ULTIMO CHEQUE\" FROM dbxschema.pbanco PBAN,dbxschema.pchequera PCHE,dbxschema.pcuentacorriente PCUE WHERE PBAN.pban_codigo=PCUE.pban_banco AND PCUE.pcue_codigo=PCHE.pcue_codigo AND PCHE.tvig_vigencia IN(\\'V\\')',"+((TextBox)e.Item.Cells[2].Controls[1]).ClientID+",'"+this.Armar_String_Numeros()+"');");
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","Cargar_Consulta(this,"+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+",'"+Request.QueryString["tipo"]+"',"+((TextBox)e.Item.Cells[2].Controls[1]).ClientID+",'"+this.Armar_String_Numeros()+"')");
                    ((TextBox)e.Item.Cells[1].FindControl("codbantxt3")).Attributes.Add("ondblclick", "Cargar_CuentasPUC(this," + ((DropDownList)e.Item.Cells[0].Controls[1]).ClientID + ")");
                    string controlesModificar = "" + ((TextBox)e.Item.Cells[4].FindControl("valortxt")).ClientID + "," + ((TextBox)e.Item.Cells[7].FindControl("fechatxt")).ClientID;
                    ((TextBox)e.Item.Cells[2].FindControl("codbantxt4")).Attributes.Add("ondblclick", "Cargar_NITS(this," + ((DropDownList)e.Item.Cells[0].Controls[1]).ClientID + "," + ((TextBox)e.Item.Cells[2].FindControl("codbantxt")).ClientID + ",'" + controlesModificar +"')");
				}
			}
			else
			{
				if(e.Item.ItemType==ListItemType.Footer)
				{
                    string sqlTipoPago = "SELECT ttip_codigo,ttip_nombre FROM ttipopago where ttip_codigo <> 'CC'";
                    if (divflag_eliminar.InnerHtml == "eliminar")
                    {
                        sqlTipoPago = "SELECT ttip_codigo,ttip_nombre FROM ttipopago WHERE ttip_codigo='DC'";
                    }
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]), sqlTipoPago);
                    ((DropDownList)e.Item.Cells[0].Controls[1]).Attributes.Add("onchange","CambioTipoPago("+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+
						""+((TextBox)e.Item.Cells[2].Controls[1]).ClientID+","+
						""+((TextBox)e.Item.Cells[4].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[5].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[6].Controls[1]).ClientID+","+
                        "" + ((TextBox)e.Item.Cells[7].Controls[1]).ClientID + "," + ((Button)e.Item.Cells[9].Controls[1]).ClientID + "," + ((Label)e.Item.Cells[1].FindControl("lbch")).ClientID + ",'" + consigAuto + "'," + ((TextBox)e.Item.Cells[1].FindControl("codbantxt3")).ClientID + "," + ((TextBox)e.Item.Cells[8].Controls[1]).ClientID + ",'" + Request.QueryString["tipo"] + "');");
					((TextBox)e.Item.Cells[7].Controls[1]).Text = DateTime.Now.ToString("yyyy-MM-dd");
					if(((DropDownList)e.Item.Cells[0].Controls[1]).Items[0].Text != "-- Escoja --")
						((DropDownList)e.Item.Cells[0].Controls[1]).Items.Insert(0,new ListItem("-- Escoja --","ESC"));
					//((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","ModalDialog(this,'SELECT pban_codigo,pban_nombre FROM pbanco',new Array());");
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onClick","Cargar_Consulta(this,"+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+",'"+Request.QueryString["tipo"]+"',null,null)");
                    ((TextBox)e.Item.Cells[1].FindControl("codbantxt3")).Attributes.Add("ondblclick", "Cargar_CuentasPUC(this," + ((DropDownList)e.Item.Cells[0].Controls[1]).ClientID + ")");
                    ((TextBox)e.Item.Cells[2].FindControl("codbantxt4")).Attributes.Add("ondblclick", "Cargar_NITS(this," + ((DropDownList)e.Item.Cells[0].Controls[1]).ClientID + ", '', '')");
				}
			}
		}

		protected void gridPagos_Item(object Sender,DataGridCommandEventArgs e)
		{
            string tipo, banco, numero, moneda = "", fecha, estado = "", razonObservacion="", cuentaPUC = "", numDocumento = "";
			double valor,tasa,vBase;
            string cuentaCorriente = "";
            try
            { //utilizado para los procesos normales de recibos de caja
                cuentaCorriente = ((TextBox)e.Item.Cells[1].Controls[5]).Text;
            }
            catch
            { //utilizado para la legalizacion de provisionales
                cuentaCorriente = hdncue.Value;
            }
            if (gridPagos.Items.Count == 0)
            {
                tablaPagos.Clear();
            }

			if(((Button)e.CommandSource).CommandName=="AgregarPagos")
			{
				//Si es un pago efectivo, descuento comercial, descuento de ley, credito directo
				if((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="E") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="DC") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="DL") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="X"))
				{
					if(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="E" && Buscar_Pago("E"))
					{
                        Utils.MostrarAlerta(Response, "Solo puede haber un pago en efectivo.");
						this.Cargar_Controles(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue,e);
					}
					else
					{
						//Si no faltan datos
						if(!Validar_Datos_Efectivo(e))
						{
							tipo=((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
                            cuentaPUC = ((TextBox)e.Item.Cells[1].FindControl("codbantxt3")).Text;
                            numDocumento = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
                            if(((RadioButton)e.Item.Cells[3].Controls[1]).Checked)
								moneda=((RadioButton)e.Item.Cells[3].Controls[1]).Text;
							else if(((RadioButton)e.Item.Cells[3].Controls[3]).Checked)
								moneda=((RadioButton)e.Item.Cells[3].Controls[3]).Text;
							valor=Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
							tasa=Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
							vBase=Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text.Trim());
							fecha=Convert.ToDateTime(((TextBox)e.Item.Cells[7].Controls[1]).Text).ToString("yyyy-MM-dd");
                            razonObservacion = ((TextBox)e.Item.Cells[8].Controls[1]).Text;
                            //Si el pago es efectivo
							if(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="E")
								estado="C";
							else if(((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="DC")) || ((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="DL")) || ((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="X")))
								estado="E";
							this.Llenar_Tabla_Pagos_Efectivo(tipo, cuentaPUC, numDocumento, moneda,valor,tasa,vBase,fecha,razonObservacion, estado);
							gridPagos.DataSource=tablaPagos;
							gridPagos.DataBind();
							Session["tablaPagos"]=tablaPagos;
						}
						else
						{
							if(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="E")
								this.Cargar_Controles(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue,e);
							else if(((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="DC")) || ((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="DL")) || ((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="X")))
								this.Cargar_Controles(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue,e);
						}
					}
				}
					//Si es un pago tarjeta debito, tarjeta credito, transferencia bancaria o cheque
                else if ((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "D") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "T") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "B") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "C") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "CC") )
				{
					if(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="C" && !Regex.IsMatch(((TextBox)e.Item.Cells[2].Controls[1]).Text, "^[0-9]+$"))
					{
                        Utils.MostrarAlerta(Response, "Numero de cheque no valido");
						this.Cargar_Controles(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue,e);
					}
					if(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="C" && Buscar_Pago("C") && Request.QueryString["tipo"]=="CE")
					{
                        Utils.MostrarAlerta(Response, "Proceso no permitido");
						this.Cargar_Controles(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue,e);
					}
					else
					{
						//Si no faltan datos
						if(!Validar_Datos_Documentos(e))
						{
							tipo=((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
                            if (Request.QueryString["tipo"] == "CE" && (tipo == "C" || tipo == "CC" || tipo == "D" || tipo == "T")) // CHQ T-DEBITO, T-CREDITO
								banco=((TextBox)e.Item.Cells[1].Controls[1]).Text +"-"+ ((TextBox)e.Item.Cells[1].Controls[5]).Text;
							else if(tipo=="B")
								banco=((TextBox)e.Item.Cells[1].Controls[1]).Text +"-"+ ((TextBox)e.Item.Cells[1].Controls[5]).Text;
							else
								banco=((TextBox)e.Item.Cells[1].Controls[1]).Text;
							numero=((TextBox)e.Item.Cells[2].Controls[1]).Text;
							if(((RadioButton)e.Item.Cells[3].Controls[1]).Checked)
								moneda=((RadioButton)e.Item.Cells[3].Controls[1]).Text;
							else if(((RadioButton)e.Item.Cells[3].Controls[3]).Checked)
								    moneda=((RadioButton)e.Item.Cells[3].Controls[3]).Text;
							valor=Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
							tasa =Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
							vBase=Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
							fecha=Convert.ToDateTime(((TextBox)e.Item.Cells[7].Controls[1]).Text).ToString("yyyy-MM-dd");
							//Si el pago es con tarjeta debito, credito o cheque
                            if ((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "D") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "T") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "C") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "CC"))
								estado="C";
							else if((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="CD") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="B"))
								estado="E";
                            string valorCheque = "";
                            if(Request.QueryString["tipo"]=="CE" && ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="C")
							{
								if(DBFunctions.SingleData("SELECT tres_manejacons FROM pchequera WHERE pche_id="+(banco.Split('-'))[1]+"")=="S")
								{
									if(this.Buscar_Numeros_Cheques(((banco.Split('-'))[1]),(Convert.ToInt32(numero)-1),ref pos))
									{
										DataRow fila=numerosCheques.NewRow();
										fila[0]=((banco.Split('-'))[1]);
										fila[1]=numero;
										numerosCheques.Rows.RemoveAt(pos);
										Session["numerosCheques"]=numerosCheques;
										numerosCheques.Rows.InsertAt(fila,pos);
										Session["numerosCheques"]=numerosCheques;
									}
								}
							}
                            else if (Request.QueryString["tipo"] == "CE" && ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "CC")
                            {
                                valorCheque = DBFunctions.SingleData("select COALESCE(mcpag_valor,0) from mcajapago where pban_codigo = '" + ((TextBox)e.Item.Cells[1].Controls[1]).Text + "' and mcpag_numerodoc = '" + ((TextBox)e.Item.Cells[2].Controls[1]).Text + "'  and test_estado = 'C';");
                                if (valorCheque == null || valorCheque == "")
                                {
                                    valorCheque = "0";
                                    Utils.MostrarAlerta(Response, "El cheque de cliente no esta registrado en caja.");
                                }
                                else
                                {
                                    this.Llenar_Tabla_Pagos_Documentos(tipo, banco, numero, moneda, Convert.ToDouble(valorCheque), tasa, vBase, fecha, estado);
                                }
                            }
                            
                            if(valorCheque == null || valorCheque == "")
                                this.Llenar_Tabla_Pagos_Documentos(tipo, banco, numero, moneda, valor, tasa, vBase, fecha, estado);
							gridPagos.DataSource=tablaPagos;
							gridPagos.DataBind();
							Session["tablaPagos"]=tablaPagos;
						}
						else
						{
							if((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="D") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="T") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="C"))
								this.Cargar_Controles(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue,e);
							else if((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="CD") || (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue=="B"))
								this.Cargar_Controles(((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue,e);
						}
					}
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverPagos")
			{
				hdnval.Value=(Convert.ToDouble(hdnval.Value)-(Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][4])*Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][5]))).ToString();
				Restar_Pagos(Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][4]),Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][5]),tablaPagos.Rows[e.Item.DataSetIndex][0].ToString());
				tablaPagos.Rows[e.Item.DataSetIndex].Delete();
				gridPagos.DataSource=tablaPagos;
				gridPagos.DataBind();
				Session["tablaPagos"]=tablaPagos;
				Habilitar_Guardar();
			}
			else if(((Button)e.CommandSource).CommandName=="AgregarCheques")
			{
				tablaPagos.Rows[e.Item.DataSetIndex][8]='L';
				Session["tablaPagos"]=tablaPagos;
				Sumar_Pagos(Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][4]),Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][5]),tablaPagos.Rows[e.Item.DataSetIndex][0].ToString());
				((Button)e.Item.Cells[10].Controls[1]).CommandName="RemoverCheques";
				((Button)e.Item.Cells[10].Controls[1]).Text="Remover";
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverCheques")
			{
				tablaPagos.Rows[e.Item.DataSetIndex][8]='C';
				Session["tablaPagos"]=tablaPagos;
				Restar_Pagos(Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][4]),Convert.ToDouble(tablaPagos.Rows[e.Item.DataSetIndex][5]),tablaPagos.Rows[e.Item.DataSetIndex][0].ToString());
				Habilitar_Guardar();
				((Button)e.Item.Cells[10].Controls[1]).CommandName="AgregarCheques";
				((Button)e.Item.Cells[10].Controls[1]).Text="Agregar";
			}
		}

		protected void gridRtns_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="AgregarRetencion")
			{
				if((((TextBox)e.Item.Cells[0].Controls[1]).Text==""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
				else if(this.Buscar_Retencion(((TextBox)e.Item.Cells[0].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
				else if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else if(!((RadioButton)e.Item.Cells[1].FindControl("rbap1")).Checked && !((RadioButton)e.Item.Cells[1].FindControl("rbap2")).Checked)
                    Utils.MostrarAlerta(Response, "Debe seleccionar un porcentaje de retención para aplicar");
				else
				{
					if(Session["tablaRtns"]==null)
						this.Cargar_Tabla_Rtns();
					fila=tablaRtns.NewRow();
					fila["CODRET"]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					if(((TextBox)e.Item.Cells[1].FindControl("codretb")).Text!="" && ((RadioButton)e.Item.Cells[1].FindControl("rbap1")).Checked)
						fila["PORCRET"]=Convert.ToDouble(((TextBox)e.Item.Cells[1].FindControl("codretb")).Text);
					else if(((TextBox)e.Item.Cells[1].FindControl("codretc")).Text!="" && ((RadioButton)e.Item.Cells[1].FindControl("rbap2")).Checked)
						fila["PORCRET"]=Convert.ToDouble(((TextBox)e.Item.Cells[1].FindControl("codretc")).Text);
					fila["VALOR"]=Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
					Sumar_Retenciones(Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text));
					tablaRtns.Rows.Add(fila);
					gridRtns.DataSource=tablaRtns;
					gridRtns.DataBind();
					Session["tablaRtns"]=tablaRtns;
                    Habilitar_Guardar();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverRetencion")
			{
				if(Session["tablaRtns"]==null)
					this.Cargar_Tabla_Rtns();
				Restar_Retenciones(Convert.ToDouble(tablaRtns.Rows[e.Item.DataSetIndex][2]));
				tablaRtns.Rows[e.Item.DataSetIndex].Delete();
				gridRtns.DataSource=tablaRtns;
				gridRtns.DataBind();
				Session["tablaRtns"]=tablaRtns;
				Habilitar_Guardar();
			}
		}

		#endregion
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{

		}
		#endregion

	}
}
