// created on 31/08/2004 at 15:47

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
using AMS.Tools;


namespace AMS.Contabilidad
{
	public partial class CausaDiferidos : System.Web.UI.UserControl
	{
		protected string mesSeleccionado,fechaComprobante;
		protected DataTable comprobante;
		protected double total;
		protected DataSet diferidos;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ano, "SELECT pano_ano FROM pano ORDER BY 1 DESC");
				bind.PutDatasIntoDropDownList(mes, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 13 ORDER BY 1");
				//bind.PutDatasIntoDropDownList(tComprobante, "SELECT pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='CC'");
                Utils.llenarPrefijos(Response, ref tComprobante, "%", "%", "CC");
			}
		}
		

		protected void efectuar_causacion(Object Sender, EventArgs e)
		{
			lb.Text="";
			if(!this.ExisteComprobante())
			{
				this.PrepararTabla();
				this.LlenarComprobante();
				ArrayList header = new ArrayList();
				string prefijo = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_codigo='"+tComprobante.SelectedValue.ToString()+"'").ToString();//tipo
                header.Add(prefijo);
                header.Add(this.ConsecutivoComprobante());
				header.Add(ano.SelectedItem.ToString());
				header.Add(mesSeleccionado);
				header.Add(fechaComprobante);
				header.Add("CAUSACION DIFERIDO DEL MES "+mes.SelectedItem.ToString().ToUpper());
				header.Add(total.ToString());				
				Session.Clear();
				Session["header"]= header;
				Session["comprobante"] = comprobante;		
				Session["diferidos"] = diferidos.Tables[0];
				guardar.Visible = true;
			}
			else
			{
				lb.Text="Este proceso ya fue realizado para el mes seleccionado";
			}
				
		}
		
		
		protected  void  guardar_comprobante(Object  Sender, EventArgs e)
		{
			//Guardamos el comprobante
			ArrayList header = new ArrayList();
			ArrayList sqlStrings = new ArrayList();
			header = (ArrayList)Session["header"];
			comprobante = new DataTable();
			comprobante = ((DataTable)Session["comprobante"]).Copy();
			Comprobante comp = new Comprobante(comprobante);
			comp.Type = header[0].ToString();
			comp.Number = header[1].ToString();
			comp.Year = header[2].ToString();
			comp.Month = header[3].ToString();
			comp.Date = header[4].ToString();
			comp.Detail = header[5].ToString();
			comp.User= HttpContext.Current.User.Identity.Name.ToString();
			comp.Value=header[6].ToString();
			comp.Consecutivo=false;
            bool completo = false;
            if (comp.CommitValues(ref sqlStrings)){
                lb.Text += "<BR>guardado";
                completo = true;
            }
			else
				lb.Text+=comp.ProcessMsg;
			//Hacemos los cambios correspondientes en la Tabla mdiferido
			DataTable diferidos = new DataTable();
			diferidos = ((DataTable)Session["diferidos"]).Copy();
			for(int i=0;i<diferidos.Rows.Count;i++)
			{
				double amortizacion = (System.Convert.ToDouble(diferidos.Rows[i].ItemArray[4]))/(System.Convert.ToDouble(diferidos.Rows[i].ItemArray[3]));
				double saldoAmt = (System.Convert.ToDouble(diferidos.Rows[i].ItemArray[4]))- (System.Convert.ToDouble(diferidos.Rows[i].ItemArray[5]));
				if(saldoAmt<amortizacion)
					amortizacion=saldoAmt;
				sqlStrings.Add("UPDATE mdiferido SET mdif_valoamtz = mdif_valoamtz +"+amortizacion.ToString()+" WHERE mdif_nombdife='"+diferidos.Rows[i].ItemArray[0].ToString()+"'");
			}
            if (completo)
			    DBFunctions.Transaction(sqlStrings);

            lb.Text += "Error al grabar.";
		}		
		
		//////////////////////FUNCION QUE BUSCA SI EL COMPROBANTE YA FUE REALIZADO PARA EL MES SELECCIONADO////
		///////////////////////////////////////////////////////////////////////////////////////////////////////
		
		protected bool ExisteComprobante()
		{
			bool existe = false;
			mesSeleccionado = DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='"+mes.SelectedItem.ToString()+"'");
			if(DBFunctions.RecordExist("SELECT * FROM mcomprobante WHERE pano_ano="+ano.SelectedItem.ToString()+" AND pmes_mes="+mesSeleccionado+" AND mcom_razon LIKE '%CAUSACION DIFERIDO%'"))
				existe = true;
			return existe;
		}
		///////////////////////FIN FUNCION DE BUSQUEDA DE COMPROBANTE EXISTENTE////////////////////////////////
		
		protected void PrepararTabla()
		{
			comprobante = new DataTable();
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

            //Se crea la columna que almacena el valor del debito niif
            DataColumn debitoNiifC = new DataColumn();
            debitoNiifC.DataType = System.Type.GetType("System.Double");
            debitoNiifC.ColumnName = "DEBITONIIF";
            debitoNiifC.ReadOnly = true;
            comprobante.Columns.Add(debitoNiifC);

            //Se crea la columna que almacena el valor del debito niif
            DataColumn creditoNiifC = new DataColumn();
            creditoNiifC.DataType = System.Type.GetType("System.Double");
            creditoNiifC.ColumnName = "CREDITONIIF";
            creditoNiifC.ReadOnly = true;
            comprobante.Columns.Add(creditoNiifC);
        }

        //FUNCION QUE LLENA EL COMPROBANTE QUE RESPALDA EL AJUSTE DE DIFERIDOS

        protected void LlenarComprobante()
        {
            fechaComprobante = ano.SelectedItem.ToString() + "-" + mesSeleccionado + "-" + DBFunctions.SingleData("SELECT pmes_dias FROM pmes WHERE pmes_mes=" + mesSeleccionado + "");
            diferidos = new DataSet();
            string prefijo = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_codigo='" + tComprobante.SelectedValue.ToString() + "'").ToString();
            int numero = Convert.ToUInt16(this.ConsecutivoComprobante().ToString());
            int i;
            total = 0;
            DBFunctions.Request(diferidos, IncludeSchema.NO, @"SELECT mdif_nombdife, pcco_centcost, mdif_fechinic, mdif_cuotas, mdif_valohist, mdif_valoamtz, mdif_cuenreal, mdif_cuennomi, mc.tnat_codigo, ce.mnit_nit, cc.palm_almacen , coalesce(mcdf.MCUE_codiPUC,'') AS NIIF_CUENTA, coalesce(mcdf.tnat_codigo,'N') AS NIIF_NAT
                       FROM mdiferido md, cempresa ce, ccontabilidad cc, mcuenta mc
                            left join mcuenta mcdf on mc.mcue_codipucniif = mcdf.mcue_codipuc
                       WHERE mdif_cuenreal = mc.mcue_codipuc and mdif_valohist <> mdif_valoamtz AND mdif_fechinic < '" + fechaComprobante + "'");
            //Aqui vamos a empezar a crear nuestro comprobante de ajuste
            if (diferidos.Tables.Count > 0)
            {
                for (i = 0; i < diferidos.Tables[0].Rows.Count; i++)
                {
                    //Calculamos el valor de la amortizacion
                    double amortizacion = (System.Convert.ToDouble(diferidos.Tables[0].Rows[i].ItemArray[4])) / (System.Convert.ToDouble(diferidos.Tables[0].Rows[i].ItemArray[3]));
                    double saldoAmt = (System.Convert.ToDouble(diferidos.Tables[0].Rows[i].ItemArray[4])) - (System.Convert.ToDouble(diferidos.Tables[0].Rows[i].ItemArray[5]));
                    if (saldoAmt < amortizacion)
                        amortizacion = saldoAmt;
                    total += amortizacion;
                    //Armamos las filas del comprobante
                    DataRow filaOrigen = comprobante.NewRow();
                    DataRow filaDestino = comprobante.NewRow();
                    filaOrigen["CUENTA"] = diferidos.Tables[0].Rows[i].ItemArray[6].ToString();
                    filaDestino["CUENTA"] = diferidos.Tables[0].Rows[i].ItemArray[7].ToString();
                    filaOrigen["NIT"] = diferidos.Tables[0].Rows[i].ItemArray[9].ToString();  //DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
                    filaDestino["NIT"] = diferidos.Tables[0].Rows[i].ItemArray[9].ToString(); //DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
                    filaOrigen["PREF"] = prefijo; // DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + tComprobante.SelectedValue.ToString() + "'").ToString();
                    filaDestino["PREF"] = prefijo; // DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + tComprobante.SelectedValue.ToString() + "'").ToString();
                    filaOrigen["DOC_REF"] = numero; // this.ConsecutivoComprobante();
                    filaDestino["DOC_REF"] = numero; // this.ConsecutivoComprobante(); 
                    filaOrigen["DETALLE"] = "CAUSACION DE " + diferidos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
                    filaDestino["DETALLE"] = "CAUSACION DE " + diferidos.Tables[0].Rows[i].ItemArray[0].ToString().ToUpper();
                    filaOrigen["SEDE"] = diferidos.Tables[0].Rows[i].ItemArray[10].ToString();  //DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
                    filaDestino["SEDE"] = diferidos.Tables[0].Rows[i].ItemArray[10].ToString();  //DBFunctions.SingleData("SELECT palm_almacen FROM ccontabilidad");
                    filaOrigen["CENTRO_COSTO"] = diferidos.Tables[0].Rows[i].ItemArray[1].ToString();
                    filaDestino["CENTRO_COSTO"] = diferidos.Tables[0].Rows[i].ItemArray[1].ToString();
                    filaOrigen["DEBITO"] = 0;
                    filaDestino["DEBITO"] = 0;
                    filaOrigen["CREDITO"] = 0;
                    filaDestino["CREDITO"] = 0;
                    filaOrigen["BASE"] = 0;
                    filaDestino["BASE"] = 0;
                    string naturaleza = diferidos.Tables[0].Rows[i].ItemArray[8].ToString(); //(8) = TNAT_CODIGO  DBFunctions.SingleData("SELECT tnat_codigo FROM mcuenta WHERE mcue_codipuc='" + diferidos.Tables[0].Rows[i].ItemArray[6].ToString() + "'");
                    if (naturaleza == "D")
                    {
                        filaOrigen["DEBITO"] = amortizacion;
                        filaDestino["DEBITO"] = 0;
                        filaOrigen["CREDITO"] = 0;
                        filaDestino["CREDITO"] = amortizacion;
                    }
                    else if (naturaleza == "C")
                    {
                        filaOrigen["DEBITO"] = 0;
                        filaDestino["DEBITO"] = amortizacion;
                        filaOrigen["CREDITO"] = amortizacion;
                        filaDestino["CREDITO"] = 0;
                    }

                    filaOrigen["DEBITONIIF"] = 0; // EN NIIF no hay diferidos nic(38)
                    filaDestino["DEBITONIIF"] = 0;
                    filaOrigen["CREDITONIIF"] = 0;
                    filaDestino["CREDITONIIF"] = 0;

                    if (diferidos.Tables[0].Rows[i].ItemArray[11].ToString() != "" && diferidos.Tables[0].Rows[i].ItemArray[12].ToString() == "R")
                    {
                        filaDestino["DEBITONIIF"] = filaDestino["DEBITO"]; // IMPUTA niif siempre y cuando la cuenta equivalente de la causación sea REAL
                        filaDestino["CREDITONIIF"] = filaDestino["CREDITO"];
                    }

                    comprobante.Rows.Add(filaOrigen);
                    comprobante.Rows.Add(filaDestino);

                    // IMPUTA niif siempre y cuando la cuenta equivalente de la causación sea REAL
                    if (diferidos.Tables[0].Rows[i].ItemArray[11].ToString() != "" && diferidos.Tables[0].Rows[i].ItemArray[12].ToString() == "R")
                    {
                        filaOrigen["CUENTA"] = diferidos.Tables[0].Rows[i].ItemArray[11].ToString();
                        if (naturaleza == "D")
                        {
                            filaOrigen["DEBITONIIF"] = amortizacion;
                            filaOrigen["CREDITONIIF"] = 0;
                        }
                        else if (naturaleza == "C")
                        {
                                filaOrigen["DEBITONIIF"] = 0;
                                filaOrigen["CREDITONIIF"] = amortizacion;
                        }
                        comprobante.Rows.Add(filaOrigen);
                    }

                    comprobanteG.DataSource = comprobante;
                    comprobanteG.DataBind();
                    //fin del comprobante
                    //Ahora vamos a modificar la tabla mdiferido		 
                }
            }
        }
		/// FIN DE FUNCION QUE LLENA EL COMPROBANTE
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// funcion para calcular el consecutivo del comprobante
		protected  string ConsecutivoComprobante()
		{
			string lastID = DBFunctions.SingleData("SELECT pdoc_ultidocu FROM PDOCUMENTO WHERE pdoc_codigo='"+tComprobante.SelectedValue.ToString()+"'");
			string newID = "";
 			if(lastID == "")
 				newID = "1";
 			else
 				newID = (Convert.ToInt32(lastID)+1).ToString(); 	
			return newID;
		}
		///Fin funcion para calcular el consecutivo////
		/// 
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
