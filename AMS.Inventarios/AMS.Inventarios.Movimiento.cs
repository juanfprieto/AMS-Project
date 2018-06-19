using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using System.Configuration;

namespace AMS.Inventarios
{
	//ACTUALIZA EL KARDEX DE ACUERDO AL TIPO DE MOVIMIENTO Y REALIZA ACTUALIZACIONES PARTICULARES DE CADA MOVIMIENTO DEL KARDEX
	public class Movimiento
	{
		#region Campos

		protected DataTable dtSource;
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
		protected ArrayList sqlL;
		protected ArrayList sqlAux;
		protected UInt64 numerodoc,numdocuref;
		protected int tipomovi,claseMovimiento;
		protected string codigodoc,prefdocuref,mnit,codalmacen,codvendedor,codcargo,codcentrocosto,processMsg,indDir, numlsEmpa;
        protected string observaciones = "";
        protected DateTime fechadocu;//fecha documento
		protected bool cerrar=false;

		#endregion

		#region Propiedades

		public string ProcessMsg{get{return processMsg;}}
        public string Observaciones { set { observaciones = value; } get { return observaciones; } }
		public ArrayList SqlStrings{get{return sqlL;}}

		#endregion

		#region Constructores

		public Movimiento()
		{

		}

		//Constructor #1.
		public Movimiento(
			string pdoccod,
			uint numedocu,
			string prefdocrf,
			uint numdocrf,
			int tmov,
			string nit,
			string palm,
			DateTime fechdoc,
			string vencod,
			string cargo,
			string cencos,
			string indDirecto,
            string numeListEm)
		{
			int i;
			codigodoc   =pdoccod;//Prefijo Documento Ajuste
			numerodoc   =numedocu;//Numero Documento Ajuste
			prefdocuref =prefdocrf;//Prefijo Referencia ??????????
			numdocuref  =numdocrf;//Numero Referencia ????????????
			tipomovi    =tmov;//Tipo de Movimiento
			mnit        =nit;//Nit
			codalmacen  =palm;//Almacen Afectado
			fechadocu   =fechdoc;//Fecha de documento
			codvendedor =vencod;//Persona Responsable
			codcargo    =cargo;//Codigo Cargo???????????
			codcentrocosto=cencos;//Centro de costos
			claseMovimiento = 0;
			indDir      = indDirecto;
            numlsEmpa = numeListEm;

			//Segun dITEM
			lbFields.Add("codigo");//0
			types.Add(typeof(string));

			lbFields.Add("cantidad");//1
			types.Add(typeof(double));

			lbFields.Add("valounit");//2
			types.Add(typeof(double));

			lbFields.Add("costprom");//3
			types.Add(typeof(double));

			lbFields.Add("costpromalma");//4
			types.Add(typeof(double));

			lbFields.Add("porciva");//5
			types.Add(typeof(double));

			lbFields.Add("porcdesc");//6
			types.Add(typeof(double));

			lbFields.Add("cantdevo");//7
			types.Add(typeof(double));

			lbFields.Add("costpromhis");//8
			types.Add(typeof(double));

			lbFields.Add("costpromhisalma");//9
			types.Add(typeof(double));

			lbFields.Add("valopubl");//10
			types.Add(typeof(double));

			lbFields.Add("inveini");//11
			types.Add(typeof(double));

			lbFields.Add("inveinialma");//12
			types.Add(typeof(double));

			lbFields.Add("cantidadAuxiliar");//13
			types.Add(typeof(double));

            lbFields.Add("codalmacen");//14
            types.Add(typeof(string));

            lbFields.Add("numdocrf");//15
            types.Add(typeof(int));

            lbFields.Add("numlsEmpa");//16
            types.Add(typeof(string));

            dtSource = new DataTable();

			for(i=0; i<lbFields.Count; i++)
				dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}

		//Constructor #2 <Marca>
		public Movimiento(
			string pdoccod,
			UInt64 numedocu,
			int tmov,
			DateTime fechdoc,
			string nit,
			string palm,
			string vencod,
			string cargo,
			string cencos,
			string indDirecto)
		{
			int i;
			codigodoc   =pdoccod;//Prefijo Documento Ajuste
			numerodoc   =numedocu;//Numero Documento Ajuste
			tipomovi    =tmov;//Tipo de Movimiento
			fechadocu   =fechdoc;//Fecha de documento
			codvendedor =vencod;//Persona Responsable
			codcargo    =cargo;//Codigo Cargo???????????
			codcentrocosto=cencos;//Centro de costos
			mnit        =nit;//Nit
			codalmacen  =palm;//Almacen Afectado
			claseMovimiento = 1;
			indDir      = indDirecto;
			//Segun dITEM
			lbFields.Add("codigo");//0
			types.Add(typeof(string));

			lbFields.Add("cantidad");//1
			types.Add(typeof(double));

			lbFields.Add("valounit");//2
			types.Add(typeof(double));

			lbFields.Add("costprom");//3
			types.Add(typeof(double));

			lbFields.Add("costpromalma");//4
			types.Add(typeof(double));

			lbFields.Add("porciva");//5
			types.Add(typeof(double));

			lbFields.Add("porcdesc");//6
			types.Add(typeof(double));

			lbFields.Add("cantdevo");//7
			types.Add(typeof(double));

			lbFields.Add("costpromhis");//8
			types.Add(typeof(double));

			lbFields.Add("costpromhisalma");//9
			types.Add(typeof(double));

			lbFields.Add("valopubl");//10
			types.Add(typeof(double));

			lbFields.Add("inveini");//11
			types.Add(typeof(double));

			lbFields.Add("inveinialma");//12
			types.Add(typeof(double));

			lbFields.Add("prefdocrf");//13
			types.Add(typeof(string));

			lbFields.Add("numdocrf");//14
			types.Add(typeof(uint));

			lbFields.Add("cantidadAuxiliar");//15
			types.Add(typeof(double));

			dtSource = new DataTable();

			for(i=0; i<lbFields.Count; i++)
				dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}

		//Constructor #3
		public Movimiento(
			string pdoccod,
			uint numedocu,
			int tmov,
			string nit,
			DateTime fechdoc,
			string vencod,
			string cargo,
			string cencos,
			string indDirecto)
		{
			int i;
			codigodoc   =pdoccod;//Prefijo Documento Ajuste
			numerodoc   =numedocu;//Numero Documento Ajuste
			tipomovi    =tmov;//Tipo de Movimiento
			fechadocu   =fechdoc;//Fecha de documento
			codvendedor =vencod;//Persona Responsable
			mnit        =nit;//Nit
			codcargo    =cargo;//Codigo Cargo???????????
			codcentrocosto=cencos;//Centro de costos
			claseMovimiento = 2;
			indDir      = indDirecto;

			//Segun dITEM
			lbFields.Add("codigo");//0
			types.Add(typeof(string));

			lbFields.Add("cantidad");//1
			types.Add(typeof(double));

			lbFields.Add("valounit");//2
			types.Add(typeof(double));

			lbFields.Add("costprom");//3
			types.Add(typeof(double));

			lbFields.Add("costpromalma");//4
			types.Add(typeof(double));

			lbFields.Add("porciva");//5
			types.Add(typeof(double));

			lbFields.Add("porcdesc");//6
			types.Add(typeof(double));

			lbFields.Add("cantdevo");//7
			types.Add(typeof(double));

			lbFields.Add("costpromhis");//8
			types.Add(typeof(double));

			lbFields.Add("costpromhisalma");//9
			types.Add(typeof(double));

			lbFields.Add("valopubl");//10
			types.Add(typeof(double));

			lbFields.Add("inveini");//11
			types.Add(typeof(double));

			lbFields.Add("inveinialma");//12
			types.Add(typeof(double));

			lbFields.Add("codalmacen");//13
			types.Add(typeof(string));

			lbFields.Add("cantidadAuxiliar");//14
			types.Add(typeof(double));

			dtSource = new DataTable();

			for(i=0; i<lbFields.Count; i++)
				dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}

		//Constructor #5
		public Movimiento(
			string pdoccod,
			uint numedocu,
			string prefdocrf,
			uint numdocrf,
			int tmov,
			string nit,
			DateTime fechdoc,
			string vencod,
			string cargo,
			string cencos,
			string indDirecto)
		{
			int i;
			codigodoc   =pdoccod;//Prefijo Documento Ajuste
			numerodoc   =numedocu;//Numero Documento Ajuste
			prefdocuref =prefdocrf;//Prefijo Referencia ??????????
			numdocuref  =numdocrf;//Numero Referencia ????????????
			tipomovi    =tmov;//Tipo de Movimiento
			mnit        =nit;//Nit
			fechadocu   =fechdoc;//Fecha de documento
			codvendedor =vencod;//Persona Responsable
			codcargo    =cargo;//Codigo Cargo???????????
			codcentrocosto=cencos;//Centro de costos
			claseMovimiento = 0;
			indDir      = indDirecto;

			//Segun dITEM
			lbFields.Add("codigo");//0
			types.Add(typeof(string));

			lbFields.Add("cantidad");//1
			types.Add(typeof(double));

			lbFields.Add("valounit");//2
			types.Add(typeof(double));

			lbFields.Add("costprom");//3
			types.Add(typeof(double));

			lbFields.Add("costpromalma");//4
			types.Add(typeof(double));

			lbFields.Add("porciva");//5
			types.Add(typeof(double));

			lbFields.Add("porcdesc");//6
			types.Add(typeof(double));

			lbFields.Add("cantdevo");//7
			types.Add(typeof(double));

			lbFields.Add("costpromhis");//8
			types.Add(typeof(double));

			lbFields.Add("costpromhisalma");//9
			types.Add(typeof(double));

			lbFields.Add("valopubl");//10
			types.Add(typeof(double));

			lbFields.Add("inveini");//11
			types.Add(typeof(double));

			lbFields.Add("inveinialma");//12
			types.Add(typeof(double));

			lbFields.Add("cantidadAuxiliar");//13
			types.Add(typeof(double));

			lbFields.Add("codalmacen");//14
			types.Add(typeof(string));

            lbFields.Add("numdocrf");//15
            types.Add(typeof(int));


			dtSource = new DataTable();

			for(i=0; i<lbFields.Count; i++)
				dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}
		#endregion

		#region Metodos

		public void InsertaFila(
			string s1,
			double v1,
			double v2,
			double v3,
			double v4,
			double v5,
			double v6,
			double v7,
			double v8,
			double v9,
			double v10,
			double v11,
			double v12,
			double v13,
            string v14,
            string v15)
		{
			DataRow dr;
			dr = dtSource.NewRow();

			dr[0] = s1;
			dr[1] = v1;
			dr[2] = v2;
			dr[3] = v3;
			dr[4] = v4;
			dr[5] = v5;
			dr[6] = v6;
			dr[7] = v7;
			dr[8] = v8;
			dr[9] = v9;
			dr[10] = v10;
			dr[11] = v11;
			dr[12] = v12;
            dr[13] = v13;
            dr[14] = v14;
            dr[15] = v15;

			dtSource.Rows.Add(dr);
		}


        // proceso de cierre de inventario fisico
		public void InsertaFila(
			string s1,
			double v1,
			double v2,
			double v3,
			double v4,
			double v5,
			double v6,
			double v7,
			double v8,
			double v9,
			double v10,
			double v11,
			double v12,
			double v13,
			string v14,
            int    tarjeta)
		{
			DataRow dr;
			dr = dtSource.NewRow();

			dr[0] = s1;
			dr[1] = v1;
			dr[2] = v2;
			dr[3] = v3;
			dr[4] = v4;
			dr[5] = v5;
			dr[6] = v6;
			dr[7] = v7;
			dr[8] = v8;
			dr[9] = v9;
			dr[10] = v10;
			dr[11] = v11;
			dr[12] = v12;
			dr[13] = v13;
			dr[14] = v14;
            dr[15] = tarjeta;

			dtSource.Rows.Add(dr);
		}

		public void InsertaFila(
			string codigo,
			double cantidad,
			double valounit,
			double costprom,
			double costpromalma,
			double porciva,
			double porcdesc,
			double cantdevo,
			double costpromhis,
			double costpromhisalma,
			double valopubl,
			double inveini,
			double inveinialma,
			string prefdocrf,
			UInt64 numdocrf)
		{
			DataRow dr;
			dr = dtSource.NewRow();

			dr["codigo"] = codigo;
			dr["cantidad"] = cantidad;
			dr["valounit"] = valounit;
			dr["costprom"] = costprom;
			dr["costpromalma"] = costpromalma;
			dr["porciva"] = porciva;
			dr["porcdesc"] = porcdesc; //Porc Descuento
			dr["cantdevo"] = cantdevo;
			dr["costpromhis"] = costpromhis;
			dr["costpromhisalma"] = costpromhisalma;
			dr["valopubl"] = valopubl;
			dr["inveini"] = inveini;
			dr["inveinialma"] = inveinialma;
			dr["prefdocrf"] = prefdocrf;
			dr["numdocrf"] = numdocrf;

			dtSource.Rows.Add(dr);
		}

		public void InsertaFila(
			string codigo,
			double cantidad,
			double valorUnitario,
			double costoPromedio,
			double costoPromedioAlmacen,
			double porcentajeIva,
			double porcentajeDescuento,
			double cantidadDevolucion,
			double costoPromedioHistorico,
			double costoPromedioHistoricoAlmacen,
			double valorPublico,
			double inventarioInicial,
			double inventarioInicialAlmacen,
			string codigoAlmacen)
		{
			DataRow dr;
			dr = dtSource.NewRow();

			dr["codigo"] = codigo;
			dr["cantidad"] = cantidad;
			dr["valounit"] = valorUnitario;
			dr["costprom"] = costoPromedio;
			dr["costpromalma"] = costoPromedioAlmacen;
			dr["porciva"] = porcentajeIva;
			dr["porcdesc"] = porcentajeDescuento;
			dr["cantdevo"] = cantidadDevolucion;
			dr["costpromhis"] = costoPromedioHistorico;
			dr["costpromhisalma"] = costoPromedioHistoricoAlmacen;
			dr["valopubl"] = valorPublico;
			dr["inveini"] = inventarioInicial;
			dr["inveinialma"] = inventarioInicialAlmacen;
			dr["codalmacen"] = codigoAlmacen;

			dtSource.Rows.Add(dr);
		}
		
		public bool CerrarInventario(bool grabar,ArrayList sqlA){
			sqlAux=sqlA;
			cerrar=true;
			if(DBFunctions.RecordExist("SELECT dinv_tarjeta from DINVENTARIOFISICO WHERE DINV_CONTDEFINITIVO IS NULL AND PDOC_CODIGO='"+codigodoc+"' AND MINF_NUMEROINV="+numerodoc+";"))
			{
				processMsg="ERROR: El inventario no se puede cerrar porque existen tarjetas sin contar.";
				return(false);
			}
			return(RealizarMov(grabar));
		}

		public bool RealizarMov(bool grabar)
		{
			int n;
			bool status = true;
			sqlL = new ArrayList();
            string[] numPedi = new String[] { };


            string ano_cinv = ConfiguracionInventario.Ano;
			string mes_cinv = ConfiguracionInventario.Mes;
			string fechSis = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.ffffff");
			string FechaDoc = fechadocu.ToString("yyyy-MM-dd");
            DataSet ds = new DataSet();
            string[] numLs = numlsEmpa.ToString().Split('-');
            for (int i = 0; i < numLs.Length - 1; i++)
            {
                DBFunctions.Request(ds, IncludeSchema.NO, "Select	PPED_CODIGO,MPED_NUMEPEDI,MNIT_NIT, MLIS_NUMERO from	DLISTAEMPAQUE where	MLIS_NUMERO=" + numLs[i] + "");
            }

            for (n=0;n<dtSource.Rows.Count;n++)
			{
               
			   switch(claseMovimiento)
			   {
					case 1:
					{
						prefdocuref = dtSource.Rows[n]["prefdocrf"].ToString();
						numdocuref = Convert.ToUInt32(dtSource.Rows[n]["numdocrf"]);

						if(mnit=="")
							mnit = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidoitem WHERE pped_codigo='"+prefdocuref+"' AND mped_numepedi="+numdocuref.ToString()+"");

						if(codalmacen=="")
							codalmacen = DBFunctions.SingleData("SELECT palm_almacen FROM mpedidoitem WHERE pped_codigo='"+prefdocuref+"' AND mped_numepedi="+numdocuref.ToString()+"");
					}
					break;

					case 2:
					{
						prefdocuref = codigodoc;
						numdocuref = numerodoc;
						codalmacen = dtSource.Rows[n]["codalmacen"].ToString();
					}
					break;
			  }
                if (DBFunctions.SingleData("SELECT TORI_CODIGO FROM MITEMS WHERE MITE_CODIGO = '" + dtSource.Rows[n]["codigo"].ToString() + "' ") == "Z")
                { // es un servicio, No se graban los acumaulados, solo el kardex
                    
                    sqlL.Add("insert into DITEMS(DITE_SECUENCIA,PDOC_CODIGO, DITE_NUMEDOCU, MITE_CODIGO , DITE_PREFDOCUREFE, DITE_NUMEDOCUREFE, TMOV_TIPOMOVI, MNIT_NIT, PALM_ALMACEN, PANO_ANO, PMES_MES, DITE_FECHDOCU, DITE_CANTIDAD, DITE_VALOUNIT, DITE_COSTPROM, DITE_COSTPROMALMA, DITE_COSTPROMHIS, DITE_COSTPROMHISALMA, PIVA_PORCIVA, DITE_PORCDESC, PVEN_CODIGO, DITE_CANTDEVO, TCAR_CARGO, DITE_VALOPUBL, DITE_INVEINIC, DITE_INVEINICALMA, PCEN_CODIGO, DITE_PROCESO)" +
                       " values (DEFAULT, '" + codigodoc + "'," + numerodoc + ", '" + dtSource.Rows[n]["codigo"].ToString() + "','" + dtSource.Rows[n][14].ToString() + "', " + dtSource.Rows[n][15].ToString() + "," + tipomovi.ToString() + ",'" + mnit + "','" + codalmacen + "'," + ano_cinv + "," + mes_cinv + ",'" + FechaDoc + "'," + dtSource.Rows[n]["cantidad"] + "," + dtSource.Rows[n]["valounit"] + ",0,0,0,0," + dtSource.Rows[n]["porciva"] + "," + dtSource.Rows[n]["porcdesc"] + ",'" + codvendedor + "'," + dtSource.Rows[n]["cantdevo"] + "," + "'" + codcargo + "'," + dtSource.Rows[n]["valopubl"] + ",0,0,'" + codcentrocosto + "','" + fechSis + "'); ");
                }
                else  // Graba los registros de movimiento de items fisico, el origen Z son SERVICIOS facturados por el sistema de Inventarios
                {

                    #region Calculo de costos y cantidades

                    double costoPromedioNuevo = 0;
                    double costoPromedioAlmacenNuevo = 0;
                    double costoPromedioHistoricoNuevo = 0;
                    double costoPromedioHistoricoAlmacenNuevo = 0;
                    double cantidadDespuesTransaccion = 0;
                    double cantidadDespuesTransaccionAlmacen = 0;

                    //10 PreRecepcion de Items
                    //20 Entradas de Producción
                    //30 Entradas de Proveedor
                    //50 Ajustes
                    //81 Devolucion de Transferencias
                    //91 Devolucion de Cliente
                    #region Tipo de Movimiento 30, 50, 20, 10.
                    if (tipomovi == 30 || tipomovi == 50 || tipomovi == 20 || tipomovi == 10) // Calculo Costo Promedio
                    {
                        //Si la cantidad es mayor a cero se debe recalcular costos promedios
                        if (Convert.ToDouble(dtSource.Rows[n]["cantidad"]) > 0)
                        {
                            costoPromedioNuevo = CalcularCostoPromedio(
                                Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                Convert.ToDouble(dtSource.Rows[n]["inveini"]),
                                Convert.ToDouble(dtSource.Rows[n]["costprom"]),
                                Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                            costoPromedioAlmacenNuevo = CalcularCostoPromedio(
                                Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                Convert.ToDouble(dtSource.Rows[n]["inveinialma"]),
                                Convert.ToDouble(dtSource.Rows[n]["costpromalma"]),
                                Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                            costoPromedioHistoricoNuevo = CalcularCostoPromedio(
                                Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                Convert.ToDouble(dtSource.Rows[n]["inveini"]),
                                Convert.ToDouble(dtSource.Rows[n]["costpromhis"]),
                                Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                            costoPromedioHistoricoAlmacenNuevo = CalcularCostoPromedio(
                                Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                Convert.ToDouble(dtSource.Rows[n]["inveinialma"]),
                                Convert.ToDouble(dtSource.Rows[n]["costpromalma"]),
                                Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                        }
                        else
                        {
                            costoPromedioNuevo = Convert.ToDouble(dtSource.Rows[n]["costprom"]);
                            costoPromedioHistoricoAlmacenNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromalma"]);
                        }
                        cantidadDespuesTransaccion = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveini"]), Convert.ToDouble(dtSource.Rows[n]["cantidad"]));
                        cantidadDespuesTransaccionAlmacen = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveinialma"]), Convert.ToDouble(dtSource.Rows[n]["cantidad"]));
                    }
                    #endregion
                    #region Tipo de Movimiento 31, 81, 91.
                    else if (tipomovi == 31 || tipomovi == 61 || tipomovi == 81 || tipomovi == 91)
                    //31 Devolucion a Proveedor  61 Devol Consumo Interno 81 Devol Transferencia  91 Devol Cliente  
                    {
                        //Si la cantidad es mayor a cero se debe recalcular costos promedios
                        if (Convert.ToDouble(dtSource.Rows[n]["cantidad"]) > 0)
                        {
                            if (tipomovi == 31)
                            {
                                costoPromedioNuevo = CalcularCostoPromedioDevolucion(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                    Convert.ToDouble(dtSource.Rows[n]["inveini"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costprom"]),
                                    Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                                costoPromedioAlmacenNuevo = CalcularCostoPromedioDevolucion(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                    Convert.ToDouble(dtSource.Rows[n]["inveinialma"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromalma"]),
                                    Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                                costoPromedioHistoricoNuevo = CalcularCostoPromedioDevolucion(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                    Convert.ToDouble(dtSource.Rows[n]["inveini"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromhis"]),
                                    Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                                costoPromedioHistoricoAlmacenNuevo = CalcularCostoPromedioDevolucion(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["valounit"]),
                                    Convert.ToDouble(dtSource.Rows[n]["inveinialma"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromalma"]),
                                    Convert.ToDouble(dtSource.Rows[n]["porcdesc"]));
                            }
                            else
                            {
                                costoPromedioNuevo = CalcularCostoPromedio(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costprom"]), // ORIGINAL VENTA
                                    Convert.ToDouble(dtSource.Rows[n]["inveini"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costprom"]),
                                    0.00);
                                costoPromedioAlmacenNuevo = CalcularCostoPromedio(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromalma"]), // ORIGINAL VENTA
                                    Convert.ToDouble(dtSource.Rows[n]["inveinialma"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromalma"]),
                                    0.00);
                                costoPromedioHistoricoNuevo = CalcularCostoPromedio(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromhis"]), // ORIGINAL VENTA
                                    Convert.ToDouble(dtSource.Rows[n]["inveini"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromhis"]),
                                    0.00);
                                costoPromedioHistoricoAlmacenNuevo = CalcularCostoPromedio(
                                    Convert.ToDouble(dtSource.Rows[n]["cantidad"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromalma"]), // ORIGINAL VENTA
                                    Convert.ToDouble(dtSource.Rows[n]["inveinialma"]),
                                    Convert.ToDouble(dtSource.Rows[n]["costpromalma"]),
                                    0.00);
                                // POR AHORA DEJO EL COSTO PROMEDIO AL ORIGINAL
                                costoPromedioNuevo = Convert.ToDouble(dtSource.Rows[n]["costprom"]);
                                costoPromedioAlmacenNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromalma"]);
                                costoPromedioHistoricoNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromhis"]);
                                costoPromedioHistoricoAlmacenNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromhisalma"]);

                            }
                        }
                        else
                        {
                            costoPromedioNuevo = Convert.ToDouble(dtSource.Rows[n]["costprom"]);
                            costoPromedioHistoricoAlmacenNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromalma"]);
                        }
                        cantidadDespuesTransaccion = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveini"]), (Convert.ToDouble(dtSource.Rows[n]["cantidad"])) * -1);
                        cantidadDespuesTransaccionAlmacen = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveinialma"]), (Convert.ToDouble(dtSource.Rows[n]["cantidad"])) * -1);

                    }
                    #endregion
                    #region Tipo de Movimiento 40, 80, 90.
                    else if (tipomovi == 90 || tipomovi == 80 || tipomovi == 60 || tipomovi == 40)
                    //90 Factura Clte   80 Transferencia a Taller  60 Consumo Interno   40 Remision Almacen
                    {
                        costoPromedioNuevo = Convert.ToDouble(dtSource.Rows[n]["costprom"]);
                        costoPromedioAlmacenNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromalma"]);
                        costoPromedioHistoricoNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromhis"]);
                        costoPromedioHistoricoAlmacenNuevo = Convert.ToDouble(dtSource.Rows[n]["costpromhisalma"]);

                        if (tipomovi == 40) // 40 Remisiones entre Almacenes
                        {
                            cantidadDespuesTransaccion = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveini"]), Convert.ToDouble(dtSource.Rows[n]["cantidad"]));
                            cantidadDespuesTransaccionAlmacen = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveinialma"]), Convert.ToDouble(dtSource.Rows[n]["cantidad"]));
                        }
                        else
                        {
                            cantidadDespuesTransaccion = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveini"]), Convert.ToDouble(dtSource.Rows[n]["cantidad"]) * (-1));
                            cantidadDespuesTransaccionAlmacen = CalcularSaldoDespuesTransaccion(Convert.ToDouble(dtSource.Rows[n]["inveinialma"]), Convert.ToDouble(dtSource.Rows[n]["cantidad"]) * (-1));
                        }
                    }
                    #endregion

                    #endregion

                    //DITEMS:
                    //Se debe recalcular el costo promedio, costo promedio almacen, costo promedio historico, costo promedio histortico almacen, inventario despues de transaccion, inventario despues de transaccion almacen.
                    if (cerrar)
                    {
                        if (codalmacen == "")
                            codalmacen = dtSource.Rows[n]["codalmacen"].ToString();
                    }

                    if (tipomovi == 50 && dtSource.Rows[n]["numdocrf"].ToString() != "")
                        numdocuref = Convert.ToUInt32(dtSource.Rows[n]["numdocrf"]);

                    sqlL.Add("insert into DITEMS(DITE_SECUENCIA,PDOC_CODIGO, DITE_NUMEDOCU, MITE_CODIGO , DITE_PREFDOCUREFE, DITE_NUMEDOCUREFE, TMOV_TIPOMOVI, MNIT_NIT, PALM_ALMACEN, PANO_ANO, PMES_MES, DITE_FECHDOCU, DITE_CANTIDAD, DITE_VALOUNIT, DITE_COSTPROM, DITE_COSTPROMALMA, DITE_COSTPROMHIS, DITE_COSTPROMHISALMA, PIVA_PORCIVA, DITE_PORCDESC, PVEN_CODIGO, DITE_CANTDEVO, TCAR_CARGO, DITE_VALOPUBL, DITE_INVEINIC, DITE_INVEINICALMA, PCEN_CODIGO, DITE_PROCESO)" +
                        " values (DEFAULT, '" + codigodoc + "'," + numerodoc + ", '" + dtSource.Rows[n]["codigo"].ToString() + "','" + dtSource.Rows[n][14].ToString() + "', " + dtSource.Rows[n][15].ToString() + "," + tipomovi.ToString() + ",'" + mnit + "','" + codalmacen + "'," + ano_cinv + "," + mes_cinv + ",'" + FechaDoc + "'," + dtSource.Rows[n]["cantidad"] + "," + dtSource.Rows[n]["valounit"] + "," + costoPromedioNuevo + "," + costoPromedioAlmacenNuevo + "," + costoPromedioHistoricoNuevo + "," + costoPromedioHistoricoAlmacenNuevo + "," + dtSource.Rows[n]["porciva"] + "," + dtSource.Rows[n]["porcdesc"] + ",'" + codvendedor + "'," + dtSource.Rows[n]["cantdevo"] + "," + "'" + codcargo + "'," + dtSource.Rows[n]["valopubl"] + "," + dtSource.Rows[n]["inveini"] + "," + dtSource.Rows[n]["inveinialma"] + ",'" + codcentrocosto + "','" + fechSis + "'); ");

                    if (tipomovi == 40 || tipomovi == 50 || tipomovi == 10)
                    {
                        if (n == 0)  // remisiones (40) ajustes (50) prerecepciones (10) graba padre
                        {
                            sqlL.Add("insert into MDOCUMENTOUSUARIO values ('" + codigodoc + "'," + numerodoc + ",'" + HttpContext.Current.User.Identity.Name + "','" + observaciones + "')");
                        }
                    }

                    if (indDir == "N")
                    {
                        //Crear filas si no existen (MSALDOITEM, MSALDOITEMALMACEN, MACUMLADOITEM, MACUMLADOITEMALMACEN)
                        if (!DBFunctions.RecordExist("SELECT * FROM msaldoitem WHERE mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PANO_ANO=" + ano_cinv) && !EncontrarValorArrayList("insert into msaldoitem (mite_codigo,msal_cantactual,pano_ano,msal_cantasig,msal_costprom,msal_costpromhist,msal_costhist,msal_costhisthist,msal_cantinveinic,msal_ultivent,msal_ultiingr,msal_ulticost,msal_ultiprov,msal_abcd,msal_peditrans,msal_unidtrans,msal_pedipendi,msal_unidpendi) values ('" + dtSource.Rows[n]["codigo"].ToString() + "',0," + ano_cinv + ",0,0,0,0,0,0,null,null," + Convert.ToDouble(dtSource.Rows[n]["valounit"]).ToString() + ",null,null,0,0,0,0)", sqlL))
                            sqlL.Add("insert into msaldoitem (mite_codigo,msal_cantactual,pano_ano,msal_cantasig,msal_costprom,msal_costpromhist,msal_costhist,msal_costhisthist,msal_cantinveinic,msal_ultivent,msal_ultiingr,msal_ulticost,msal_ultiprov,msal_abcd,msal_peditrans,msal_unidtrans,msal_pedipendi,msal_unidpendi) values ('" + dtSource.Rows[n]["codigo"].ToString() + "',0," + ano_cinv + ",0,0,0,0,0,0,null,null," + Convert.ToDouble(dtSource.Rows[n]["valounit"]).ToString() + ",null,null,0,0,0,0)");

                        if (!DBFunctions.RecordExist("SELECT * FROM msaldoitemalmacen WHERE mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' AND PANO_ANO=" + ano_cinv) && !EncontrarValorArrayList("SELECT * FROM msaldoitem WHERE mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PANO_ANO=" + ano_cinv, sqlL) && !EncontrarValorArrayList("insert into msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_cantpendiente,msal_canttransito,msal_cantinveinic) values ('" + dtSource.Rows[n]["codigo"].ToString() + "','" + codalmacen + "'," + ano_cinv + ",0,0,0,0,0,0,0)", sqlL))
                            sqlL.Add("insert into msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_cantpendiente,msal_canttransito,msal_cantinveinic) values ('" + dtSource.Rows[n]["codigo"].ToString() + "','" + codalmacen + "'," + ano_cinv + ",0,0,0,0,0,0,0)");

                        if (!DBFunctions.RecordExist("SELECT * FROM MACUMULADOITEM WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'") && !EncontrarValorArrayList("insert into MACUMULADOITEM (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,macu_cantidad,macu_costo,macu_precio) values ('" + dtSource.Rows[n]["codigo"].ToString() + "'," + tipomovi + "," + fechadocu.Year.ToString() + "," + fechadocu.Month.ToString() + ",0,0,0)", sqlL))
                            sqlL.Add("insert into MACUMULADOITEM (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,macu_cantidad,macu_costo,macu_precio) values ('" + dtSource.Rows[n]["codigo"].ToString() + "'," + tipomovi + "," + fechadocu.Year.ToString() + "," + fechadocu.Month.ToString() + ",0,0,0)");

                        if (!DBFunctions.RecordExist("SELECT * FROM MACUMULADOITEMALMACEN WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "'") && !EncontrarValorArrayList("insert into MACUMULADOITEMALMACEN (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,PALM_ALMACEN,macu_cantidad,macu_costo,macu_precio) values ('" + dtSource.Rows[n]["codigo"].ToString() + "'," + tipomovi + "," + fechadocu.Year.ToString() + "," + fechadocu.Month.ToString() + ",'" + codalmacen + "',0,0,0)", sqlL))
                            sqlL.Add("insert into MACUMULADOITEMALMACEN (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,PALM_ALMACEN,macu_cantidad,macu_costo,macu_precio) values ('" + dtSource.Rows[n]["codigo"].ToString() + "'," + tipomovi + "," + fechadocu.Year.ToString() + "," + fechadocu.Month.ToString() + ",'" + codalmacen + "',0,0,0)");
                    }

                    //COMUN A TODOS LOS MOVIMIENTOS:
                    //
                    //ACCIONES PARTICULARES DE CADA MOVIMIENTO

                    #region Tipo de Movimiento 10, 11, 30, 50, 20.
                    if (tipomovi == 50 || tipomovi == 30 || tipomovi == 10 || tipomovi == 20) // || tipomovi==11) //Ajustes kardex(50) Entradas proveedor-Recep.Directa(30) prerecepcion(10) legalización (11)
                    {
                        double precio = 0;
                        double cantEntrada = Convert.ToDouble(dtSource.Rows[n]["cantidad"]);

                        //Msaldoitem 
                        if (tipomovi == 50)
                            sqlL.Add("UPDATE msaldoitem SET msal_cantactual=msal_cantactual + (" + cantEntrada + ") where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PANO_ANO=" + ano_cinv + " ; ");
                        else
                            sqlL.Add("UPDATE msaldoitem SET msal_cantactual=msal_cantactual + (" + cantEntrada + "), msal_ulticost=" + Convert.ToDouble(dtSource.Rows[n]["valounit"]).ToString() + ", msal_ultiingr='" + Convert.ToDateTime(FechaDoc).ToString("yyyy-MM-dd") + "', msal_ultiprov='" + mnit + "' where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PANO_ANO=" + ano_cinv + " ;");

                        if (Convert.ToDouble(dtSource.Rows[n]["cantidad"]) > 0)
                            sqlL.Add("UPDATE msaldoitem SET msal_costprom=" + costoPromedioNuevo + ",msal_costpromhist=" + costoPromedioHistoricoNuevo + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PANO_ANO=" + ano_cinv + " ; ");

                        //Msaldoitemalmacen
                        sqlL.Add("UPDATE msaldoitemalmacen SET msal_cantactual=msal_cantactual + (" + cantEntrada + ") where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' AND PANO_ANO=" + ano_cinv + " ; ");

                        if (Convert.ToDouble(dtSource.Rows[n]["cantidad"]) > 0)
                            sqlL.Add("UPDATE msaldoitemalmacen SET msal_costprom=" + costoPromedioAlmacenNuevo + ",msal_costpromhist=" + costoPromedioHistoricoAlmacenNuevo + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' AND PANO_ANO=" + ano_cinv + " ; ");

                        precio = Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * Convert.ToDouble(dtSource.Rows[n]["valounit"].ToString());
                        precio = precio - (precio * Convert.ToDouble(dtSource.Rows[n]["porcdesc"]) / 100);

                        //Macumuladoitemalmacen
                        sqlL.Add("UPDATE macumuladoitemalmacen SET macu_cantidad=macu_cantidad+(" + cantEntrada + "), macu_costo=macu_costo+(" + costoPromedioAlmacenNuevo * Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) + "),macu_precio=macu_precio+" + precio + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' ;");
                        //Macumuladoitem
                        sqlL.Add("UPDATE macumuladoitem SET macu_cantidad=macu_cantidad+(" + cantEntrada + "), macu_costo=macu_costo+(" + costoPromedioNuevo * Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) + "),macu_precio=macu_precio+" + precio + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'; ");
                    }
                    #endregion
                    #region Tipo de Movimiento 31.
                    if (tipomovi == 31) //Devoluciones a proveedor
                    {
                        double costo = 0;
                        double precio = 0;

                        sqlL.Add("UPDATE macumuladoitemalmacen  SET macu_cantidad  =macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "'");
                        sqlL.Add("UPDATE macumuladoitem         SET macu_cantidad  =macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'");
                        sqlL.Add("UPDATE msaldoitem             SET msal_cantactual=msal_cantactual-" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + ", msal_costprom = " + costoPromedioNuevo + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' and pano_ano=" + ano_cinv);
                        sqlL.Add("UPDATE msaldoitemalmacen      SET msal_cantactual=msal_cantactual-" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + ", msal_costprom = " + costoPromedioAlmacenNuevo + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' and pano_ano=" + ano_cinv);

                        costo = (Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * costoPromedioNuevo);
                        sqlL.Add("UPDATE macumuladoitem SET macu_costo=macu_costo+" + costo.ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'");

                        precio = Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * Convert.ToDouble(dtSource.Rows[n]["valounit"].ToString());
                        precio = precio - (precio * Convert.ToDouble(dtSource.Rows[n]["porcdesc"]) / 100);
                        sqlL.Add("UPDATE macumuladoitem SET macu_precio=macu_precio+" + precio.ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'");

                        costo = (Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * costoPromedioAlmacenNuevo);
                        sqlL.Add("UPDATE macumuladoitemalmacen SET macu_costo=macu_costo+" + costo + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + this.codalmacen + "'");
                        sqlL.Add("UPDATE macumuladoitemalmacen SET macu_precio=macu_precio+" + precio + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + this.codalmacen + "'");
                    }
                    #endregion
                    #region Tipo de Movimiento 80, 90.
                    if (tipomovi == 90 || tipomovi == 80 || tipomovi == 60) //Pedido Cliente(90) / Pedido taller(80) / Consumo Interno (60)
                    {
                        double cantP = 0;
                        double costo = 0;
                        double precio = 0;
                        double cantAsig = 0;
                        double cantFact = 0;


                        costo = (Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * costoPromedioNuevo);
                        precio = Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * Convert.ToDouble(dtSource.Rows[n]["valounit"].ToString());
                        precio = precio - (precio * Convert.ToDouble(dtSource.Rows[n]["porcdesc"]) / 100);
                        //             sqlL.Add("UPDATE macumuladoitem SET macu_cantidad=macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'");
                        //             sqlL.Add("UPDATE macumuladoitem SET macu_costo=macu_costo+" + costo.ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'");
                        sqlL.Add("UPDATE macumuladoitem SET macu_cantidad=macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + ", macu_costo=macu_costo+" + costo.ToString() + ", macu_precio=macu_precio+" + precio.ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'");

                        costo = (Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * costoPromedioAlmacenNuevo);
                        //              sqlL.Add("UPDATE macumuladoitemalmacen SET macu_cantidad=macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + this.codalmacen + "'");
                        //              sqlL.Add("UPDATE macumuladoitemalmacen SET macu_costo=macu_costo+"+costo+"    WHERE pano_ano="+fechadocu.Year.ToString()+" and pmes_mes="+fechadocu.Month.ToString()+" and tmov_tipomovi="+tipomovi.ToString()+" AND mite_codigo = '"+dtSource.Rows[n]["codigo"].ToString()+"' AND PALM_ALMACEN='"+this.codalmacen+"'");
                        sqlL.Add("UPDATE macumuladoitemalmacen SET macu_cantidad=macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + ", macu_costo=macu_costo+" + costo + ", macu_precio=macu_precio+" + precio + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + this.codalmacen + "'");

                        if (indDir == "N")
                        {
                            //Se trae la cantidad que originalmente se solicito en el pedido
                            try
                            {
                                cantP = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi from DPEDIDOITEM where mped_clasregi='C' AND MNIT_NIT='" + this.mnit + "' and pped_codigo='" + this.prefdocuref + "' and mped_numepedi=" + this.numdocuref.ToString() + " and mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "'"));
                            }
                            catch { };

                            //Se trae el valor asignado para este movimiento, este valor es antes de realizar modificacion
                            try
                            {
                                cantAsig = Convert.ToDouble(dtSource.Rows[n]["cantidadAuxiliar"]);
                            }
                            catch { }

                            //Se trae el valor facturado
                            try
                            {
                                cantFact = Convert.ToDouble(dtSource.Rows[n]["cantidad"]);
                            }
                            catch { }

                            //Determinamos las unidades pendientes de la siguiente forma, a la cantidad facturada se le resta la cantidad asignada
                            double cantidadPendiente = cantFact - cantAsig;

                            //Ahora se trae la cantidad asiganada para ese pedido
                            if (Convert.ToDouble(dtSource.Rows[n]["cantidad"]) == cantP)
                                sqlL.Add("UPDATE msaldoitem SET msal_pedipendi=msal_pedipendi-1, msal_unidpendi = msal_unidpendi - " + cantFact + ", msal_cantactual=msal_cantactual-" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + ",msal_cantasig=msal_cantasig-" + cantAsig + ", msal_ultivent='" + Convert.ToDateTime(FechaDoc).ToString("yyyy-MM-dd") + "' where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' and pano_ano=" + ano_cinv);
                            else
                                sqlL.Add("UPDATE msaldoitem SET msal_unidpendi=msal_unidpendi - " + cantFact + ", msal_cantactual=msal_cantactual-" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + ",msal_cantasig=msal_cantasig-" + cantAsig + ", msal_ultivent='" + Convert.ToDateTime(FechaDoc).ToString("yyyy-MM-dd") + "' where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' and pano_ano=" + ano_cinv);

                            sqlL.Add("UPDATE msaldoitemalmacen SET msal_cantactual=msal_cantactual-" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + ",msal_cantasig=msal_cantasig-" + cantAsig + ", msal_CANTpendiente = msal_CANTpendiente  - " + cantFact + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' and pano_ano=" + ano_cinv);

                            #region Código candidato a eliminación
                            /*try{cantP=Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi from DPEDIDOITEM where mped_clasregi='C' AND MNIT_NIT='"+this.mnit+"' and pped_codigo='"+this.prefdocuref+"' and mped_numepedi="+this.numdocuref.ToString()+" and mite_codigo='"+dtSource.Rows[n]["codigo"].ToString()+"'"));}
                            catch{cantP=0;};
                            if(Convert.ToDouble(dtSource.Rows[n]["cantidad"]) == cantP)
                            sqlL.Add("UPDATE msaldoitem SET msal_pedipendi=msal_pedipendi-1, msal_unidpendi = msal_unidpendi - "+Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString()+", msal_cantactual=msal_cantactual-"+Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString()+",msal_cantasig=msal_cantasig-"+cantP.ToString()+" where mite_codigo='"+dtSource.Rows[n]["codigo"].ToString()+"' and pano_ano="+ano_cinv);
                            else
                            sqlL.Add("UPDATE msaldoitem SET msal_unidpendi=msal_unidpendi-"+Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString()+", msal_cantactual=msal_cantactual-"+Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString()+",msal_cantasig=msal_cantasig-"+cantP.ToString()+" where mite_codigo='"+dtSource.Rows[n]["codigo"].ToString()+"' and pano_ano="+ano_cinv);
                            sqlL.Add("UPDATE msaldoitemalmacen SET msal_cantactual=msal_cantactual-"+Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString()+",msal_cantasig=msal_cantasig-"+cantP.ToString()+" where mite_codigo='"+dtSource.Rows[n]["codigo"].ToString()+"' AND PALM_ALMACEN='"+this.codalmacen+"' and pano_ano="+ano_cinv);*/
                            #endregion
                        }
                        else if (indDir == "S")
                        {
                            sqlL.Add("UPDATE msaldoitem SET msal_cantactual=msal_cantactual-" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' and pano_ano=" + ano_cinv);
                            sqlL.Add("UPDATE msaldoitemalmacen SET msal_cantactual=msal_cantactual-" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + this.codalmacen + "' and pano_ano=" + ano_cinv);
                        }
                    }
                    #endregion
                    #region Tipo de Movimiento 81, 91, 61.
                    if (tipomovi == 91 || tipomovi == 81 || tipomovi == 61)//Devoluciones de cliente o de taller o de Consumo Interno
                    {
                        double costo = 0;
                        double precio = 0;

                        //				sqlL.Add("UPDATE macumuladoitemalmacen  SET macu_cantidad=macu_cantidad+"+dtSource.Rows[n]["cantidad"].ToString()+" WHERE pano_ano="+fechadocu.Year.ToString()+" and pmes_mes="+fechadocu.Month.ToString()+" and tmov_tipomovi="+tipomovi.ToString()+" AND mite_codigo = '"+dtSource.Rows[n]["codigo"].ToString()+"' AND PALM_ALMACEN='"+codalmacen+"'");
                        //				sqlL.Add("UPDATE macumuladoitem         SET macu_cantidad=macu_cantidad+"+dtSource.Rows[n]["cantidad"].ToString()+" WHERE pano_ano="+fechadocu.Year.ToString()+" and pmes_mes="+fechadocu.Month.ToString()+" and tmov_tipomovi="+tipomovi.ToString()+" AND mite_codigo = '"+dtSource.Rows[n]["codigo"].ToString()+"'");
                        sqlL.Add("UPDATE msaldoitem             SET msal_cantactual=msal_cantactual+" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' and pano_ano=" + ano_cinv);
                        sqlL.Add("UPDATE msaldoitemalmacen      SET msal_cantactual=msal_cantactual+" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' and pano_ano=" + ano_cinv);

                        costo = (Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * costoPromedioNuevo);
                        //				sqlL.Add("UPDATE macumuladoitem SET macu_costo=macu_costo+"+costo.ToString()+" WHERE pano_ano="+fechadocu.Year.ToString()+" and pmes_mes="+fechadocu.Month.ToString()+" and tmov_tipomovi="+tipomovi.ToString()+" AND mite_codigo = '"+dtSource.Rows[n]["codigo"].ToString()+"'");
                        precio = Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * Convert.ToDouble(dtSource.Rows[n]["valounit"].ToString());
                        precio = precio - (precio * Convert.ToDouble(dtSource.Rows[n]["porcdesc"]) / 100);
                        sqlL.Add("UPDATE macumuladoitem SET macu_cantidad=macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + ", macu_costo=macu_costo+" + costo.ToString() + ", macu_precio=macu_precio+" + precio.ToString() + " WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "'");

                        costo = (Convert.ToDouble(dtSource.Rows[n]["cantidad"].ToString()) * costoPromedioAlmacenNuevo);
                        sqlL.Add("UPDATE macumuladoitemalmacen SET macu_cantidad=macu_cantidad+" + dtSource.Rows[n]["cantidad"].ToString() + ", macu_costo=macu_costo+" + costo + ", macu_precio=macu_precio+" + precio + "   WHERE pano_ano=" + fechadocu.Year.ToString() + " and pmes_mes=" + fechadocu.Month.ToString() + " and tmov_tipomovi=" + tipomovi.ToString() + " AND mite_codigo = '" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + this.codalmacen + "'");
                        // 				sqlL.Add("UPDATE macumuladoitemalmacen SET macu_precio=macu_precio+"+precio+" WHERE pano_ano="+fechadocu.Year.ToString()+" and pmes_mes="+fechadocu.Month.ToString()+" and tmov_tipomovi="+tipomovi.ToString()+" AND mite_codigo = '"+dtSource.Rows[n]["codigo"].ToString()+"' AND PALM_ALMACEN='"+this.codalmacen+"'");
                    }
                    #endregion
                    #region Tipo de Movimiento 40.
                    if (tipomovi == 40) // 40 Remision entre almacenes
                    {
                        sqlL.Add("UPDATE msaldoitemalmacen SET msal_cantactual=msal_cantactual+(" + Convert.ToDouble(dtSource.Rows[n]["cantidad"]).ToString() + "), msal_costprom=" + costoPromedioAlmacenNuevo + ", msal_costpromhist=" + costoPromedioHistoricoAlmacenNuevo + " where mite_codigo='" + dtSource.Rows[n]["codigo"].ToString() + "' AND PALM_ALMACEN='" + codalmacen + "' and pano_ano=" + ano_cinv);
                    }
                    #endregion
                }
            }
			if(sqlAux!=null && sqlAux.Count>0)
				for(int c=0;c<sqlAux.Count;c++)
					sqlL.Add(sqlAux[c]);

			if(grabar)
			{
				if(DBFunctions.Transaction(sqlL))
					processMsg += "<br>Bien "+DBFunctions.exceptions;
				else
				{
					status = false;
					processMsg += "<br>ERROR : "+DBFunctions.exceptions;
				}
			}

			return status;
		}

		private bool EncontrarValorArrayList(string valor, ArrayList sqlStrings)
		{
			bool encontrado = false;

			for(int i=0;i<sqlStrings.Count;i++)
			{
				string valor1 = ((string)sqlStrings[i]).Trim().ToUpper();
				string valor2 = valor.Trim().ToUpper();

				if(valor1 == valor2)
					encontrado = true;
			}

			return encontrado;
		}

		#region Código candidato a eliminación
		/*private double CalcularCostoPromedio(string codigoItem, string codAlmacen, double cantidadIngreso, double valorIngreso)
		{
		string ano_cinv=DBFunctions.SingleData("SELECT pano_ano from cinventario");
		double cantidadActual = 0, costoPromedioActual = 0;
		try{cantidadActual = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitemalmacen WHERE mite_codigo='"+codigoItem+"' AND palm_almacen='"+codAlmacen+"' AND pano_ano="+ano_cinv+""));}
		catch{}
		try{costoPromedioActual = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitemalmacen WHERE mite_codigo='"+codigoItem+"' AND palm_almacen='"+codAlmacen+"' AND pano_ano="+ano_cinv+""));}
		catch{}
		return ((cantidadActual*costoPromedioActual)+(cantidadIngreso*valorIngreso))/(cantidadIngreso+cantidadActual);
		}*/
		#endregion

		#endregion

		#region Estaticas

		public static double CalcularCostoPromedio(double cantidadIngreso, double valorIngreso, double cantidadInicial, double costoPromedioInicial, double porcentajeDescuento)
		{
            if (cantidadInicial < 0)  // chambonada, el inventario de entrada no puede ser negativo, buscar donde esta quedando negativo
                cantidadInicial = 0;
            if ((cantidadInicial + cantidadIngreso) == 0)
                return (costoPromedioInicial);
            else
            {
                int numDecimales = 2;
                bool decimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
                if (decimales)
                    numDecimales = Convert.ToInt32(ConfigurationManager.AppSettings["tamanoDecimal"]);
                return Math.Round(((cantidadInicial * costoPromedioInicial) + (cantidadIngreso * (valorIngreso * (1 - (porcentajeDescuento / 100))))) / (cantidadInicial + cantidadIngreso), numDecimales);
            }
		}

        public static double CalcularCostoPromedioDevolucion(double cantidadDevolucion, double valorDevolucion, double cantidadInicial, double costoPromedioInicial, double porcentajeDescuento)
        {
            if (cantidadInicial < 0)  // chambonada, el inventario de entrada no puede ser negativo, buscar donde esta quedando negativo
                cantidadInicial = 0;
            if ((cantidadInicial - cantidadDevolucion) == 0)
                return (costoPromedioInicial);
            else
            {
                int numDecimales = 2;
                bool decimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
                if (decimales)
                    numDecimales = Convert.ToInt32(ConfigurationManager.AppSettings["tamanoDecimal"]);

                return Math.Round(((cantidadInicial * costoPromedioInicial) - (cantidadDevolucion * (valorDevolucion * (1 - (porcentajeDescuento / 100))))) / (cantidadInicial - cantidadDevolucion), numDecimales);
            }
        }

		public static double CalcularSaldoDespuesTransaccion(double cantidadActual, double cantidadNueva)
		{
			return cantidadActual+cantidadNueva;
		}

		#endregion
	}
}
