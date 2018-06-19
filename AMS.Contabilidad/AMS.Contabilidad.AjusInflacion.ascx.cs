using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;


namespace AMS.Contabilidad
{
	
	
	
public partial class AjusInflacion : System.Web.UI.UserControl 
{
	protected DataTable comprobante = new DataTable();
	protected DataTable saldos = new DataTable();
	protected DataTable hd = new DataTable();
	protected double paag,total;
	protected DataSet noMonetarias = new DataSet(); //Variable para guardar las cuentas No Monetarias
	protected DataSet mPaagCuenta = new DataSet(); //Variable para almacenar los pares
	protected int mes, ano, enlace=7;

	
	
	
	protected void Page_Load(object sender, System.EventArgs e)
	{
		if (!IsPostBack)
		{	
			DatasToControls bind = new DatasToControls();
            //bind.PutDatasIntoDropDownList(typeDoc, "SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu LIKE '%AI' and tvig_vigencia = 'V' ");
            Utils.llenarPrefijos(Response , ref typeDoc , "%", "%", "%AI");
			bind.PutDatasIntoDropDownList(year, "SELECT pano_ano FROM pano");
			bind.PutDatasIntoDropDownList(month, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 13");
			valorPaag.Text+=" "+DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano=1998 AND pmes_mes=1");
		}	
			
	}
	
	protected  void  efectuar_ajuste(Object  Sender, EventArgs e)
	{
		lb.Text="";
		if(!Comprobacion())
			lb.Text += "<BR>Existen problemas para realizar el ajuste por inflacion";
		else
		{
			ano = System.Convert.ToInt32(year.SelectedItem.ToString());
			mes = System.Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+month.SelectedItem.ToString()+"'"));
			paag = System.Convert.ToDouble((DBFunctions.SingleData("SELECT PA.ppaa_inflacion FROM ppaag PA WHERE PA.pano_ano="+year.SelectedItem.ToString()+" AND PA.pmes_mes=(SELECT PM.pmes_mes FROM pmes PM WHERE PM.pmes_nombre='"+month.SelectedItem.ToString()+"')")));
			this.Llenar_Tabla("saldos");
			this.Llenar_Tabla("header");
			this.Llenar_Tabla("comprobante");
			Session.Clear();
			Session["hd"]=hd;
			Session["comprobante"]=comprobante;
			Session["total"]=total;
		}
	}
	
	
	///Funcion para guardar comprobante del ajuste de inflacion
	/// 
	protected  void  guardar_comprobante(Object  Sender, EventArgs e)
	{
		total = (double)Session["total"];
		hd = new DataTable();
		hd = ((DataTable)Session["hd"]).Copy();
		comprobante = new DataTable();
		comprobante = ((DataTable)Session["comprobante"]).Copy();
		Comprobante comp = new Comprobante(comprobante);
		comp.Type=DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+hd.Rows[0].ItemArray[0].ToString()+"'");
		comp.Number=hd.Rows[0].ItemArray[1].ToString();
		comp.Year=hd.Rows[0].ItemArray[2].ToString();
		comp.Month=hd.Rows[0].ItemArray[3].ToString();
		comp.Date=hd.Rows[0].ItemArray[4].ToString();
		comp.Detail=hd.Rows[0].ItemArray[5].ToString();
		comp.User="";
		comp.Value=total.ToString();
		comp.Consecutivo=false;
        bool completo = false;

        ArrayList sqlStrings = new ArrayList();
        if (comp.CommitValues(ref sqlStrings)){
            completo = true;
        }
        else
            lb.Text += comp.ProcessMsg;

        if(completo)
            if (DBFunctions.Transaction(sqlStrings))
                lb.Text += "<BR>guardado";
            else
                lb.Text += "Error al grabar.";
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////funcion que verifica la existencia de los pares de cuentas para realizar el ajuste por inflacion//
	protected bool Comprobacion()
	{
		bool estado = true;
		int i;		
		DBFunctions.Request(noMonetarias,IncludeSchema.NO,"SELECT mcue_codipuc, mcue_nombre, tnat_codigo,tcue_codigo  FROM mcuenta WHERE tcue_codigo='N' OR tcue_codigo='I' ORDER BY mcue_codipuc");
		DBFunctions.Request(mPaagCuenta,IncludeSchema.NO,"SELECT mcue_codigo, mcue_codipucrefe FROM mpaagcuenta ORDER BY mcue_codigo");
		//Comprobacion de cuentas no monetarias contra sus referencias en mpaagcuenta e inverso
		//lb.Text+="<BR>Las cuentas No monetarias que no se encuetran en MPAAGCUENTA son:";
		for(i=0;i<noMonetarias.Tables[0].Rows.Count;i++)
		{
			if(!DBFunctions.RecordExist("SELECT mcue_codigo FROM mpaagcuenta WHERE mcue_codigo='"+noMonetarias.Tables[0].Rows[i].ItemArray[0].ToString()+"'"))
			{
				estado = false;
				lb.Text+="<BR>El Registro: "+noMonetarias.Tables[0].Rows[i].ItemArray[0].ToString()+" No existe en MPAAGCUENTA";
			}
				
		}
		//Comprobacion que las cuentas pares de mpaagcuenta existen en mcuenta
		for(i=0;i<mPaagCuenta.Tables[0].Rows.Count;i++)
		{
			if(!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='"+mPaagCuenta.Tables[0].Rows[i].ItemArray[1].ToString().Trim()+"'"))
			{
				estado=false;
				lb.Text+="<BR> El Registro: "+mPaagCuenta.Tables[0].Rows[i].ItemArray[1].ToString()+" No existe en MCUENTA";
			}
		}
			
		return estado;
		
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// Funcion para llenar un datatable con la informacion necesaria////////////////////////////////////////
	protected void Llenar_Tabla(string tabla)
	{
		int i;
		if(tabla=="saldos")
		{
			//saldos = new DataTable();
			//Se crea la columna donde vamos ha alamacenar los codigos de las cuentas
			DataColumn codigo = new DataColumn();
			codigo.DataType = System.Type.GetType("System.String");
			codigo.ColumnName = "CODIGO";
			codigo.ReadOnly=true;
			saldos.Columns.Add(codigo);
			//Se crea la columna para almacenar el nombre de la cuenta
			DataColumn nombre = new DataColumn();
			nombre.DataType = System.Type.GetType("System.String");
			nombre.ColumnName = "NOMBRE";
			nombre.ReadOnly=true;
			saldos.Columns.Add(nombre);
			//Se crea la columna para almacenar el porcentaje de la inflacion
			DataColumn inflacion = new DataColumn();
			inflacion.DataType = System.Type.GetType("System.Double");
			inflacion.ColumnName = "INFLACION";
			inflacion.ReadOnly=true;
			saldos.Columns.Add(inflacion);
			//Se crea la columna para almacenar el valor calculado del saldo de la cuenta
			DataColumn saldo = new DataColumn();
			saldo.DataType = System.Type.GetType("System.Double");
			saldo.ColumnName = "SALDO";
			saldo.ReadOnly=true;
			saldos.Columns.Add(saldo);
			//Se crea la columna para almacenar el valor que se va  ajustar a la cuenta
			DataColumn valor = new DataColumn();
			valor.DataType = System.Type.GetType("System.Double");
			valor.ColumnName = "VALORCALCULADO";
			valor.ReadOnly=true;
			saldos.Columns.Add(valor);
			//Se crea la columna para almacenar la naturaleza de la cuenta 
			DataColumn naturaleza = new DataColumn();
			naturaleza.DataType = System.Type.GetType("System.String");
			naturaleza.ColumnName = "NATURALEZA";
			naturaleza.ReadOnly=true;
			saldos.Columns.Add(naturaleza);
			//Se crea la columna para almacenar el codigo de la cuenta de ajuste respectiva
			DataColumn ajuste = new DataColumn();
			ajuste.DataType = System.Type.GetType("System.String");
			ajuste.ColumnName = "AJUSTE";
			ajuste.ReadOnly=true;
			saldos.Columns.Add(ajuste);
			for(i=0;i<noMonetarias.Tables[0].Rows.Count;i++)
			{
				if(DBFunctions.RecordExist("SELECT * FROM msaldocuenta where pano_ano="+ano.ToString()+" AND pmes_mes<="+mes.ToString()+" AND mcue_codipuc='"+noMonetarias.Tables[0].Rows[i].ItemArray[0]+"'"))
				{
					//Aqui se calcula el saldo de la cuenta ha ajustar
					string nature = noMonetarias.Tables[0].Rows[i].ItemArray[2].ToString().Trim();
					string referenciaAjuste="";
					double debito=0,credito=0,saldoC=0;
					//Calculo del saldo para la cuenta que estamos analizando
					DataSet cuenta = new DataSet();
					DBFunctions.Request(cuenta,IncludeSchema.NO,"SELECT msal_valodebi, msal_valocred FROM msaldocuenta WHERE pano_ano="+ano.ToString()+" AND pmes_mes<="+mes.ToString()+" AND mcue_codipuc='"+noMonetarias.Tables[0].Rows[i].ItemArray[0]+"'");
					for(int k=0;k<cuenta.Tables[0].Rows.Count;k++)
					{
						debito+=System.Convert.ToDouble(cuenta.Tables[0].Rows[k].ItemArray[0].ToString());
						credito+=System.Convert.ToDouble(cuenta.Tables[0].Rows[k].ItemArray[1].ToString());						
					}
					if(nature=="D")
						saldoC = debito - credito;
					else if(nature=="C")
						saldoC = credito - debito;
					//Fin de calculo del saldo para la cuenta que estamos analizando
					//Aqui vamos a revisar cuando una cuenta es no monetaria o de inflacion como debemos guardarla en el DataTable
					if((noMonetarias.Tables[0].Rows[i].ItemArray[3].ToString())=="N")
						referenciaAjuste = DBFunctions.SingleData("SELECT mcue_codipucrefe FROM mpaagcuenta WHERE mcue_codigo='"+noMonetarias.Tables[0].Rows[i].ItemArray[0].ToString()+"'");
					else if((noMonetarias.Tables[0].Rows[i].ItemArray[3].ToString())=="I")
						referenciaAjuste = noMonetarias.Tables[0].Rows[i].ItemArray[0].ToString();
					//Fin revision no monetaria o de inflacion
					if(saldoC!=0)
					{
						DataRow fila;
						fila = saldos.NewRow();
						fila["CODIGO"] = noMonetarias.Tables[0].Rows[i].ItemArray[0];
						fila["NOMBRE"] = noMonetarias.Tables[0].Rows[i].ItemArray[1];
						fila["INFLACION"] = paag;
						fila["SALDO"] = saldoC;
						fila["VALORCALCULADO"]= (saldoC*(paag/100));
						fila["NATURALEZA"] = nature;
						fila["AJUSTE"] = referenciaAjuste;
						saldos.Rows.Add(fila);
					}					
				}
			}		
			Grid.DataSource = saldos;
			Grid.DataBind();
			
			
		}		
		else if(tabla=="header")
		{
			//hd = new DataTable();
			//Se crea la columna que contiene el tipo de comprobante a almacenar
			DataColumn tipo = new DataColumn();
			tipo.DataType = System.Type.GetType("System.String");
			tipo.ColumnName = "TIPO";
			tipo.ReadOnly=true;
			hd.Columns.Add(tipo);
			//Se crea la columna que contiene el numero del comprobante que vamos a almacenar
			DataColumn numero = new DataColumn();
			numero.DataType = System.Type.GetType("System.String");
			numero.ColumnName = "NUMERO";
			numero.ReadOnly=true;
			hd.Columns.Add(numero);
			//Se crea la columna que contiene el año 
			DataColumn anoC = new DataColumn();
			anoC.DataType = System.Type.GetType("System.String");
			anoC.ColumnName = "ANO";
			anoC.ReadOnly=true;
			hd.Columns.Add(anoC);
			//Se crea la columna que contiene el mes
			DataColumn mesC = new DataColumn();
			mesC.DataType = System.Type.GetType("System.String");
			mesC.ColumnName = "MES";
			mesC.ReadOnly=true;
			hd.Columns.Add(mesC);
			//Se crea la columna que va almacenar la fecha de realizacion del comprobante
			DataColumn fecha = new DataColumn();
			fecha.DataType = System.Type.GetType("System.String");
			fecha.ColumnName = "FECHA";
			fecha.ReadOnly=true;
			hd.Columns.Add(fecha);
			//Se crea la columna que va almcenar la razon del comprbante
			DataColumn razon = new DataColumn();
			razon.DataType = System.Type.GetType("System.String");
			razon.ColumnName = "RAZON";
			razon.ReadOnly=true;
			hd.Columns.Add(razon);
			/////////////////////////////////////////////////////////////////////////
			/// Creamos la fila que va en el datagrid
			DataRow fila;
			fila = hd.NewRow();
			fila["TIPO"]=typeDoc.SelectedItem.ToString();
			fila["NUMERO"]=this.ConsecutivoComprobante();
			fila["ANO"]=ano.ToString();
			fila["MES"]=mes.ToString();
			string date = ano.ToString()+"-"+mes.ToString()+"-"+DBFunctions.SingleData("SELECT pmes_dias FROM pmes WHERE pmes_mes="+mes.ToString()+"");
			fila["FECHA"] = date;
			fila["RAZON"] = "AJUSTES POR INFLACION DEL MES "+(month.SelectedItem.ToString().ToUpper()); 
			hd.Rows.Add(fila);
			header.DataSource = hd;
			header.DataBind();			
			
		}
		else if(tabla=="comprobante")
		{
			//Se crea la columna que almacena los codigos de las cuentas afectadas
			DataColumn cuenta = new DataColumn();
			cuenta.DataType = System.Type.GetType("System.String");
			cuenta.ColumnName = "CUENTA";
			cuenta.ReadOnly=true;
			comprobante.Columns.Add(cuenta);
			//Se crea la columna que almacenara el nit 
			DataColumn nit = new DataColumn();
			nit.DataType = System.Type.GetType("System.String");
			nit.ColumnName = "NIT";
			nit.ReadOnly=true;
			comprobante.Columns.Add(nit);
			//Se crea la columna que almacena el pref
			DataColumn pref = new DataColumn();
			pref.DataType = System.Type.GetType("System.String");
			pref.ColumnName = "PREF";
			pref.ReadOnly=true;
			comprobante.Columns.Add(pref);
			//Se crea la columna que almacena el docuemto de referncia
			DataColumn docref = new DataColumn();
			docref.DataType = System.Type.GetType("System.String");
			docref.ColumnName = "DOC_REF";
			docref.ReadOnly=true;
			comprobante.Columns.Add(docref);
			//Se crea la columna que almacena el detalle
			DataColumn detalle = new DataColumn();
			detalle.DataType = System.Type.GetType("System.String");
			detalle.ColumnName = "DETALLE";
			detalle.ReadOnly=true;
			comprobante.Columns.Add(detalle);
			//Se crea la columna que almacena la sede
			DataColumn sede = new DataColumn();
			sede.DataType = System.Type.GetType("System.String");
			sede.ColumnName = "SEDE";
			sede.ReadOnly=true;
			comprobante.Columns.Add(sede);
			//Se crea la columna que almacena el centro de costo
			DataColumn centroCosto = new DataColumn();
			centroCosto.DataType = System.Type.GetType("System.String");
			centroCosto.ColumnName = "CENTRO_COSTO";
			centroCosto.ReadOnly=true;
			comprobante.Columns.Add(centroCosto);
			//Se crea la columna que almacena el valor del debito
			DataColumn debitoC = new DataColumn();
			debitoC.DataType = System.Type.GetType("System.Double");
			debitoC.ColumnName = "DEBITO";
			debitoC.ReadOnly=true;
			comprobante.Columns.Add(debitoC);
			//Se crea la columna que almacena el valor del debito
			DataColumn creditoC = new DataColumn();
			creditoC.DataType = System.Type.GetType("System.Double");
			creditoC.ColumnName = "CREDITO";
			creditoC.ReadOnly=true;
			comprobante.Columns.Add(creditoC);
			//Se crea la columna de la base
			DataColumn baseC = new DataColumn();
			baseC.DataType = System.Type.GetType("System.Double");
			baseC.ColumnName = "BASE";
			baseC.ReadOnly=true;
			comprobante.Columns.Add(baseC);
			//Vamos ha llenar el comprobante a continuacion
			for(i=0;i<saldos.Rows.Count;i++)
				{
					bool existe = false;
					double saldoFinalAjuste=0;
					for(int k=0;k<comprobante.Rows.Count;k++)
						if((comprobante.Rows[k].ItemArray[0].ToString())==(saldos.Rows[i].ItemArray[6].ToString()))
							existe=true;						 
					if(!existe)
					{
						for(int l=0;l<saldos.Rows.Count;l++)
							if((saldos.Rows[l].ItemArray[6].ToString())==(saldos.Rows[i].ItemArray[6].ToString()))						
								saldoFinalAjuste+=System.Convert.ToDouble(saldos.Rows[l].ItemArray[4].ToString());
						DataRow filaOrigen,filaDestino;
						filaOrigen = comprobante.NewRow();
						filaDestino=comprobante.NewRow();
						filaOrigen["CUENTA"]=saldos.Rows[i].ItemArray[6].ToString();
						filaDestino["CUENTA"]=DBFunctions.SingleData("SELECT mcue_codipucrefe FROM mpaagcuenta WHERE mcue_codigo='"+saldos.Rows[i].ItemArray[6]+"'");
						filaOrigen["NIT"]=DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
						filaDestino["NIT"]=DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");;
						filaOrigen["PREF"]=DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+hd.Rows[0].ItemArray[0].ToString()+"'");
						filaDestino["PREF"]=DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+hd.Rows[0].ItemArray[0].ToString()+"'");
						filaOrigen["DOC_REF"]=hd.Rows[0].ItemArray[1].ToString();
						filaDestino["DOC_REF"]=hd.Rows[0].ItemArray[1].ToString();
						filaOrigen["DETALLE"]=hd.Rows[0].ItemArray[5].ToString();
						filaDestino["DETALLE"]=hd.Rows[0].ItemArray[5].ToString();
						filaOrigen["SEDE"]=DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
						filaDestino["SEDE"]=DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
						filaOrigen["CENTRO_COSTO"]=DBFunctions.SingleData("SELECT pcen_codigo FROM ccontabilidad");
						filaDestino["CENTRO_COSTO"]=DBFunctions.SingleData("SELECT pcen_codigo FROM ccontabilidad");
						filaOrigen["BASE"]=0;
						filaDestino["BASE"]=0;
						if((saldos.Rows[i].ItemArray[5].ToString())=="D")
						{
							filaOrigen["DEBITO"]=saldoFinalAjuste;
							filaDestino["CREDITO"]=saldoFinalAjuste;
							filaOrigen["CREDITO"]=0;
							filaDestino["DEBITO"]=0;
						}
						else if((saldos.Rows[i].ItemArray[5].ToString())=="C")
						{
							filaOrigen["CREDITO"]=saldoFinalAjuste;
							filaDestino["DEBITO"]=saldoFinalAjuste;
							filaOrigen["DEBITO"]=0;
							filaDestino["CREDITO"]=0;
						}	
						comprobante.Rows.Add(filaOrigen);	
						comprobante.Rows.Add(filaDestino);
					}
				}		
			comprobanteG.DataSource=comprobante;
			comprobanteG.DataBind();
			total=0;
			for(i=0;i<comprobante.Rows.Count;i++)
				total+=System.Convert.ToDouble(comprobante.Rows[i].ItemArray[7].ToString());				
		}
		
		
	}
		
	
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// funcion para calcular el consecutivo del comprobante
	public string ConsecutivoComprobante()
	{
		string lastID = DBFunctions.SingleData("SELECT pdoc_ultidocu FROM PDOCUMENTO WHERE pdoc_nombre='"+typeDoc.SelectedItem.ToString()+"'");
		string newID = "";
 		if(lastID == "")
 			newID = "1";
 		else
 			newID = (Convert.ToInt32(lastID)+1).ToString(); 	
		return newID;
	}
	///Fin funcion para calcular el consecutivo////
	//////////////////////////////////////////////
	///
	
	protected  void  Cambio_Fecha(Object  Sender, EventArgs e)
	{
		string mes = DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+month.SelectedItem.ToString()+"'");
		if(DBFunctions.RecordExist("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+year.SelectedItem.ToString()+" AND pmes_mes="+mes+""))
		{
			valorPaag.Text = "Valor del PAAG para el mes elegido: "+DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag WHERE pano_ano="+year.SelectedItem.ToString()+" AND pmes_mes="+mes+"");
			Efectuar.Enabled = true;
			guardar.Enabled = true;
		}
		else
		{
			valorPaag.Text = "Lo Sentimos debe definir un valor para PAAG para el mes que acaba de elegir.";
			Efectuar.Enabled = false;
			guardar.Enabled = false;
		}
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

	}
	#endregion
		
		
}
	

	
	
	
}
