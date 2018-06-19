using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;
using AMS.Utilidades;
using AMS.Tools;

namespace AMS.Inventarios
{
	//CREA PEDIDOS Y FACTURAS
	public class PedidoFactura
	{
		#region Campos

		protected DataTable	dtSource;
		protected ArrayList	types =	new	ArrayList();
		protected ArrayList	lbFields = new ArrayList();
		protected ArrayList	sqlStrings = new ArrayList();
		protected string tipopedido,coddocumento,tipoorden,nit, clasereg,almacen,observacion,cargoorden,centrocosto, kit, numLis ;
		//				Tipopedido cododocumento ej: T (taller)	prefijoorden NIT  claseregistro	ej:C almacen observaciones
		protected string numerolista,vendedor,coddocumentof, ppedCodigo, ppedNumero;
		//				  num lista	empaque	   vendedor	 cod doc fac
		protected uint numorden,numpedido,numdocumentof;//Numero orden
		protected int  diasplazo;//Dias plazo
		protected double valorfletes,ivafletes,totalsiniva,totalconiva, costP, costPA, costPH, costPHA, invI, invIA, cantI;
		protected DateTime fecha;
		public string processMsg, AnoA, MesA, tp, bko, dem, cntCst, carg ;
		public bool tieneDatos;//Indica si tiene transferencias o lista de empaque al eliminar el pedido
		private string tipo="OT", codI;
		
		#endregion

		#region Propiedades

		public ArrayList SqlStrings{get{return sqlStrings;}}
		public string Numerolista{get{return numerolista;}}
		public string Coddocumento{get{return coddocumento;}}
		public string Coddocumentof{get{return coddocumentof;}}
		public uint	Numpedido{get{return numpedido;}}
		public uint	Numdocumentof{get{return numdocumentof;}}
        public string ProcessMsg { get { return processMsg; } }
        public string CargoOrden { set { cargoorden = value; } get { return cargoorden; } }
        public bool GrabarTransferencias { set; get; }
		
		#endregion

        #region Constructores
        //Costructor para ambos:  #1
		public PedidoFactura(
			string tped, // 0
			string coddoc, // 1 	
			string nnit, // 2
			string alma, // 3
			uint   numped, // 4
			string tipord, // 5 
			uint   numord, // 6
			string clsreg, // 7 
			DateTime	fch, // 8 
			string obs, //  9
			/*FAC*/int diaspl, // 10
			string[] numLis, // 11
			string vend, // 12
			string coddocf, // 13	
			uint   numdocf, // 14
			string carOrd, // 15
			double valoFlete, // 16
			double ivaFlete, // 17
			double totsinIva, // 18
			double totsconIva // 19
			)
		{
            GrabarTransferencias = true;
			tipopedido=tped;		//tped_codigo (tpedido)
			tipoorden=tipord;		//pdoc_codigo(morden)	PreD
			numorden=numord;		//mord_numeorde(morden)	NumD
			coddocumento=coddoc;	//pped_codigo(PPEDIDO)
			numpedido=numped;		// Numero pedido
			nit=nnit;				//Nit
			clasereg=clsreg;		//Clase	de registro
			fecha=fch;				//Fecha
			almacen=alma;			//almacen
            centrocosto = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen	WHERE tvig_vigencia='V' and palm_almacen='" + almacen + "'");	//Centro Costo
			observacion=obs;		//observacion
			//fac:					
			diasplazo=diaspl;		//dias de plazo
			numerolista=numLis[0];		//numero lista empaque
			vendedor=vend;
			coddocumentof=coddocf;	//codigo documento factura
			numdocumentof=numdocf;	//numero documento factura
			cargoorden=carOrd;		
			valorfletes=valoFlete;
			ivafletes=ivaFlete;
			totalsiniva=totsinIva;
			totalconiva=totsconIva;
			sqlStrings = new ArrayList();
			CreaTabla();
						
		}
		
		//Costructor para pedido solo:	  #2
		public PedidoFactura(
			string tped, // 1
			string coddoc, // 2 
			string nnit, // 3
			string alma, // 4
			string vend, // 5
			uint numped, // 6
			string tipord, // 7
			uint numord, // 8 
			string clsreg, // 9	
			string carOrd, // 10 
			DateTime fch, // 11 
			string obs, // 12
            string  kitPed
			)
		{
            GrabarTransferencias = true;
			tipopedido=tped;	//tped_codigo (ppedido)
			tipoorden=tipord;	//pdoc_codigo(morden)	PreD
			numorden=numord;	//mord_numeorde(morden)	NumD
			coddocumento=coddoc;//pped_codigo(PPEDIDO)
			numpedido=numped;	// Numero pedido
			cargoorden=carOrd;
			nit=nnit;			//Nit
			clasereg=clsreg;	//Clase	de registro
			fecha=fch;			//Fecha
			almacen=alma;		//almacen
            centrocosto = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen	WHERE tvig_vigencia='V' and palm_almacen='" + almacen + "'");	//Centro Costo
			vendedor=vend;		//vendedor
			observacion=obs;
			sqlStrings = new ArrayList();
            kit = kitPed;
			CreaTabla();						
		}
		
		//Constructor para facturacion solo: #3
		public PedidoFactura(
			string nnit, // 1
			DateTime fch, // 2 
			string obs, //  3
			int diaspl, // 4
			string numLis, // 5 
			string vend, // 6
			string coddocf, // 7  
			uint numdocf, // 8
			double valoFlete, // 9
			double ivaFlete, // 10
			double totsinIva, // 11
			double totsconIva, // 12
            string kitPed,// 13
            string AnoA , //14
            string almacen //15
			)
		{
            GrabarTransferencias = true;
			nit=nnit;				//Nit
			fecha=fch;				//Fecha
			observacion=obs;
			diasplazo=diaspl;		//dias de plazo
			numerolista=numLis;		//numero lista empaque
			vendedor=vend;
			coddocumentof=coddocf;	//codigo documento factura
			numdocumentof=numdocf;	//numero documento factura
			valorfletes=valoFlete;
			ivafletes=ivaFlete;
			totalsiniva=totsinIva;
			totalconiva=totsconIva;
			sqlStrings = new ArrayList();
            kit = kitPed;
            CreaTabla();
		}
		
		//Constructor para eliminar	pedido:	#4
		public PedidoFactura(
			string coddoc,	
			uint numped, 
			string	clsreg
			)
		{
            GrabarTransferencias = true;
			coddocumento=coddoc;//pped_codigo(PPEDIDO)
			numpedido=numped;	// Numero pedido
			clasereg=clsreg;	//Clase	de registro
			sqlStrings = new ArrayList();
		}
		
		//Constructor para facturar	sin	que	exista una lista de	empaque: #5
		public PedidoFactura(
			string nitCliente ,
			string prefFactura,	
			uint numFactura,
			DateTime fch, 
			int diasPl, 
			string obs, 
			double valoFlete,
			double ivaFlete,
			double totsinIva,
			double totsconIva,
			string alm,
			string vend
			)
		{
            GrabarTransferencias = true;
			nit	= nitCliente;					//Nit del Cliente
			coddocumentof =	prefFactura;		//Prefijo Factura
			numdocumentof =	numFactura;			//Numero Factura
			fecha =	fch;						//Fecha
			diasplazo =	diasPl;					//Dias plazo
			observacion=obs;
			valorfletes=valoFlete;
			ivafletes=ivaFlete;
			totalsiniva=totsinIva;
			totalconiva=totsconIva;
			almacen	= alm;						//Almacen
            centrocosto = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen	WHERE tvig_vigencia='V' and palm_almacen='" + almacen + "'");	//Centro Costo
			vendedor = vend;					//Responsable
			sqlStrings = new ArrayList();
			CreaTablaEspecial();
		}
		//Constructor para lista de	empaque produccion: #5
		public PedidoFactura(
			string nitCliente ,
			string prefFactura,	
			uint numFactura,
			DateTime fch, 
			int diasPl, 
			string obs, 
			double valoFlete,
			double ivaFlete,
			double totsinIva,
			double totsconIva,
			string alm,
			string vend,
			string tip
			)
		{
            GrabarTransferencias = true;
			nit	= nitCliente;					//Nit del Cliente
			coddocumentof =	prefFactura;		//Prefijo Factura
			numdocumentof =	numFactura;			//Numero Factura
			fecha =	fch;						//Fecha
			diasplazo =	diasPl;					//Dias plazo
			observacion=obs;
			valorfletes=valoFlete;
			ivafletes=ivaFlete;
			totalsiniva=totsinIva;
			totalconiva=totsconIva;
			almacen	= alm;						//Almacen
            centrocosto = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen	WHERE tvig_vigencia='V' and palm_almacen='" + almacen + "'");	//Centro Costo
			vendedor = vend;					//Responsable
			sqlStrings = new ArrayList();
			tipo=tip;
			CreaTablaEspecial();
		}
		public PedidoFactura(){ }

        #endregion

        private	void CreaTabla()
		{
			//Tabla	pedidos
			lbFields.Add("mite_codigo");//0
			types.Add(typeof(string));
			lbFields.Add("mite_cantidad");//1
			types.Add(typeof(double));
			lbFields.Add("mite_precio");//2
			types.Add(typeof(double));
			lbFields.Add("mite_iva");//3
			types.Add(typeof(double));
			lbFields.Add("mite_desc");//4
			types.Add(typeof(double));
			lbFields.Add("mite_cantped");//5 facturacion cantidad pedida originalmente no se usa en	realizar pedido
			types.Add(typeof(double));
            lbFields.Add("pped_codigo");//6 Codigo del pedido
            types.Add(typeof(string));
            lbFields.Add("pped_numepedi");//7 Codigo del pedido
            types.Add(typeof(string));
            dtSource = new DataTable();
			for(int	i=0; i<lbFields.Count; i++)
				dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}
		
		private	void CreaTablaEspecial()
		{
			//Tabla	pedidos
			lbFields.Add("mite_codigo");//0
			types.Add(typeof(string));
			lbFields.Add("mite_cantidad");//1
			types.Add(typeof(double));
			lbFields.Add("mite_precio");//2
			types.Add(typeof(double));
			lbFields.Add("mite_iva");//3
			types.Add(typeof(double));
			lbFields.Add("mite_desc");//4
			types.Add(typeof(double));
			lbFields.Add("mite_cantped");//5 facturacion cantidad pedida originalmente no se usa en	realizar pedido
			types.Add(typeof(double));
			lbFields.Add("mite_pedrel");//6	Pedido relacionado
			types.Add(typeof(string));
			dtSource = new DataTable();
			for(int	i=0; i<lbFields.Count; i++)
				dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}
		
		public void	InsertaFila(string s1, double v1, double v2, double	v3,	double v4, double v5, string v6, string v7)
		{
			DataRow	dr;
			dr = dtSource.NewRow();
			dr[0] =	s1;
			dr[1] =	v1;
			dr[2] =	v2;
			dr[3] =	v3;		
			dr[4] =	v4;
			dr[5] =	v5;
            dr[6] = v6;
            dr[7] = v7;
            dtSource.Rows.Add(dr);
		}
		
		public void	InsertaFila(string s1, double v1, double v2, double	v3,	double v4, double v5, string s6)
		{
			DataRow	dr;
			dr = dtSource.NewRow();
			dr[0] =	s1;
			dr[1] =	v1;
			dr[2] =	v2;
			dr[3] =	v3;		
			dr[4] =	v4;
			dr[5] =	v5;
			dr[6] =	s6;
			dtSource.Rows.Add(dr);
		}
		
		//Cerrar Pedidos vencidos
		public bool	EliminarPeds(DateTime fechaHasta){
			
			//CERRAR PEDIDOS
			bool status=true;
            string sqlCierrePedidos = @"merge into DPEDIDOITEM d  
                 using (select D.pped_codigo, D.mped_numepedi, d.mite_codigo, d.mped_clasregi  
                  from DPEDIDOITEM d, PPEDIDO p, MPEDIDOITEM m  
                   WHERE P.PPED_CODIGO    = d.PPED_CODIGO  
                     AND M.mped_CLASEREGI = D.mped_clasregi  
                     and M.mnit_nit       = D.mnit_nit  
                     and M.PPED_CODIGO    = D.PPED_CODIGO  
                     and M.mPED_numepedi  = D.mPED_numepedi  
                	 and M.mped_pedido    < current date - coalesce(P.pped_meseback,6) months  
                     AND D.dped_cantasig  = 0 FETCH FIRST 25000 ROWS ONLY ) as o  
                  on  (d.mite_CODIGO      = o.mite_CODIGO  
                  and d.PPED_CODIGO       = o.PPED_CODIGO  
                  and d.mPED_numepedi     = o.mPED_numepedi  
                  and d.mped_CLASREGI     = o.mped_clasregi  
                  AND D.DPED_CANTASIG     = 0 )  
                  when matched then delete; ";

            string sqlBorraMsdoItem = @"UPDATE MSALDOITEM M SET MSAL_PEDITRANS = 0, MSAL_UNIDTRANS = 0, MSAL_PEDIPENDI = 0, MSAL_UNIDPENDI = 0  
                     WHERE MSAL_PEDITRANS <> 0 OR MSAL_UNIDTRANS <> 0 OR MSAL_PEDIPENDI <> 0 OR MSAL_UNIDPENDI <> 0 AND M.PANO_ANO = AND M.PANO_ANO = (SELECT PANO_ANO FROM CINVENTARIO) ";
			
            string sqlMsdoItemCliente = @"merge into MSALDOITEM MS  
                   using (select d.mite_codigo, CI.PANO_ANO, COUNT(*) AS PEDIDOS, SUM(d.Dped_cANTPEDI - D.DPED_CANTFACT) AS QPEDIDA, SUM(D.DPED_CANTASIG) AS ASIGNADA  
                  from DPEDIDOITEM d, MPEDIDOITEM m, CINVENTARIO CI  
                   WHERE M.mped_CLASEREGI = 'C'   
                     AND M.mped_CLASEREGI = D.mped_clasregi  
                     and M.mnit_nit       = D.mnit_nit  
                     and M.PPED_CODIGO    = D.PPED_CODIGO    
                     and M.mPED_numepedi  = D.mPED_numepedi GROUP BY D.MITE_CODIGO, CI.PANO_ANO   ) as o   
                  on  (MS.mite_CODIGO     = o.mite_CODIGO  and MS.PANO_ANO = O.PANO_ANO )  
                   when matched then UPDATE SET (MSAL_CANTASIG, MSAL_PEDIPENDI, MSAL_UNIDPENDI) = (ASIGNADA,PEDIDOS, QPEDIDA ); ";

            string sqlMsdoItemProveedor = @"merge into MSALDOITEM MS  
                 using (select d.mite_codigo, CI.PANO_ANO, COUNT(*) AS PEDIDOS, SUM(d.Dped_cANTPEDI - D.DPED_CANTFACT) AS QPEDIDA  
                  from DPEDIDOITEM d, MPEDIDOITEM m, CINVENTARIO CI  
                   WHERE M.mped_CLASEREGI = 'P'   
                     AND M.mped_CLASEREGI = D.mped_clasregi  
                     and M.mnit_nit       = D.mnit_nit  
                     and M.PPED_CODIGO    = D.PPED_CODIGO  
                     and M.mPED_numepedi  = D.mPED_numepedi GROUP BY D.MITE_CODIGO, CI.PANO_ANO   ) as o  
                  on  (MS.mite_CODIGO     = o.mite_CODIGO  and MS.PANO_ANO = O.PANO_ANO )  
                   when matched then UPDATE SET (MSAL_PEDITRANS, MSAL_UNIDTRANS) = (PEDIDOS, QPEDIDA ); ";

            string sqlMpedidoItem = @"delete from mpedidoitem where mped_claseregi||mnit_nit||pped_codigo||mped_numepedi not in 
                                (select mped_clasregi||mnit_nit||pped_codigo||mped_numepedi from dpedidoitem);";

            try
            {
                DBFunctions.NonQuery(sqlCierrePedidos);
                DBFunctions.NonQuery(sqlBorraMsdoItem);
                DBFunctions.NonQuery(sqlMsdoItemCliente);
                DBFunctions.NonQuery(sqlMsdoItemProveedor);
                DBFunctions.NonQuery(sqlMpedidoItem);
			}
			catch
			{
				status = false;
				processMsg +="Error	:"+	DBFunctions.exceptions;
			}
			return status;
		}
		public bool	EliminarPed()
		{
			bool status	= true;
			int	i;
			DataSet	 ds = new DataSet();
			string   codAlmacen  = DBFunctions.SingleData("SELECT palm_almacen	FROM mpedidoitem WHERE pped_codigo='"+this.coddocumento+"' AND mped_numepedi="+this.numpedido+"");
			DateTime fechaPedido = Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_pedido FROM mpedidoitem WHERE	pped_codigo='"+this.coddocumento+"'	AND	mped_numepedi="+this.numpedido+""));
			tieneDatos=false;
			if(clasereg	== "C")
			{ 
				//Revisar si tiene transferencias, si tiene no se puede eliminar
				if(DBFunctions.RecordExist("SELECT * FROM mpedidotransferencia WHERE pped_codigo='"+this.coddocumento+"' AND mped_numero="+this.numpedido+";"))
				{
					tieneDatos=true;
					return(true);
				}

				//Revisar si tiene lista empaque, si tiene no se pueden borrar los items que estan en ella
				if(DBFunctions.RecordExist("SELECT mlis_numero FROM dlistaempaque WHERE pped_codigo='"+this.coddocumento+"' AND mped_numepedi="+this.numpedido+";"))
					tieneDatos=true;

				//Ahora	vamos a	traer la informacion mas importante	sobre el detalle del pedido	(DPEDIDOITEM)
				if(!tieneDatos)
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT	mite_codigo, dped_cantpedi,	dped_cantasig FROM dpedidoitem WHERE pped_codigo='"+this.coddocumento+"' AND mped_numepedi="+this.numpedido+"");
				else
					DBFunctions.Request(ds,IncludeSchema.NO,
						@"SELECT mite_codigo, dped_cantpedi, dped_cantasig  
						FROM  dpedidoitem  
						WHERE pped_codigo='"+this.coddocumento+@"' AND mped_numepedi="+this.numpedido+@" AND  
						mite_codigo not in 
						(SELECT DL.MITE_CODIGO FROM DLISTAEMPAQUE DL  
						WHERE DL.pped_codigo='"+this.coddocumento+@"' AND DL.mped_numepedi="+this.numpedido+@");");
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					sqlStrings.Add("UPDATE msaldoitem SET msal_pedipendi = msal_pedipendi - 1, msal_unidpendi = msal_unidpendi -" + ds.Tables[0].Rows[i][1].ToString() + ", msal_cantasig=msal_cantasig-" + ds.Tables[0].Rows[i][2].ToString()+" WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"'	AND	pano_ano="+fechaPedido.Year.ToString()+"");
					sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantpendiente = msal_cantpendiente-" + ds.Tables[0].Rows[i][1].ToString() + ", msal_cantasig=msal_cantasig-" + ds.Tables[0].Rows[i][2].ToString()+"	WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND pano_ano="+fechaPedido.Year.ToString()+"	AND	palm_almacen='"+codAlmacen+"'");
					sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=0 WHERE pped_codigo='"+coddocumento+"'	AND	mped_numepedi="+this.numpedido+" AND mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"'");
				}
				//Ahora	eliminamos el pedido : OJO PUEDE QUE NO	SE NECESITE	ELIMINAR EL	PEDIDO 
				if(!tieneDatos)
				{
					//sqlStrings.Add("DELETE FROM	mpedidotransferencia WHERE pped_codigo='"+coddocumento+"' AND mped_numero="+this.numpedido+"");
					sqlStrings.Add("DELETE FROM	dpedidoitem	WHERE pped_codigo='"+coddocumento+"' AND mped_numepedi="+this.numpedido+"");
					sqlStrings.Add("DELETE FROM	mpedidoitem	WHERE pped_codigo='"+coddocumento+"' AND mped_numepedi="+this.numpedido+"");
                    sqlStrings.Add("DELETE FROM	mpedidoclienteautorizacion WHERE pDOC_codigo='" + coddocumento + "' AND mped_numepedi=" + this.numpedido + "");
                }
                else
				{
					sqlStrings.Add(@"DELETE FROM dpedidoitem	 
						WHERE pped_codigo='"+coddocumento+@"' AND mped_numepedi="+this.numpedido+@" AND  
						mite_codigo not in 
						(SELECT DL.MITE_CODIGO FROM DLISTAEMPAQUE DL  
						WHERE DL.pped_codigo='"+this.coddocumento+@"' AND DL.mped_numepedi="+this.numpedido+@");");
				}
			}
			else if(clasereg ==	"P")
			{
				//Cuando vamos a eliminar un pedido	a proveedor	primero	debemos	modificar la cantidad en transito y	eliminar los registros de las tablas de
				//mpedidoitem y	de dpedidoitem.	Traemos	el detalle del pedido y	modificacmos las unidades en transito de estos items
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT	mite_codigo, dped_cantpedi - dped_cantfact FROM	dpedidoitem	WHERE pped_codigo='"+this.coddocumento+"' AND mped_numepedi="+this.numpedido+"");
				//Para cada	uno	de estos items modificamos la cantidad en transito
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=msal_unidtrans-"+ds.Tables[0].Rows[i][1].ToString()+",	msal_peditrans=msal_peditrans-1	WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND pano_ano="+fechaPedido.Year.ToString()+"");
					sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito-"+ds.Tables[0].Rows[i][1].ToString()+"	WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND pano_ano="+fechaPedido.Year.ToString()+"	AND	palm_almacen='"+codAlmacen+"'");
				}
				//Ahora	debemos	eliminar los registros de mpedidoitem y	de dpedidoitem
				sqlStrings.Add("DELETE FROM	dpedidoitem	WHERE pped_codigo='"+this.coddocumento+"' AND mped_numepedi="+this.numpedido+"");
				sqlStrings.Add("DELETE FROM	mpedidoitem	WHERE pped_codigo='"+this.coddocumento+"' AND mped_numepedi="+this.numpedido+"");
			}
			//Realizamos la	transaccion
			if(DBFunctions.Transaction(sqlStrings))
				processMsg +="Bien :"+ DBFunctions.exceptions;
			else
			{
				status = false;
				processMsg +="Error	:"+	DBFunctions.exceptions;
			}
			return status;
		}
		
		//**********************************************************************************************************///////
		//**********************************************************************************************************///////
		//**********************************************************************************************************///////
		public bool	RealizarPedFac(bool	guardar)
		{
			//Primero creamos el pedido	
			bool status	= true;
			string FechaProc = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.ffffff");
			//MPEDIDOITEM
			while(DBFunctions.RecordExist("SELECT *	FROM mpedidoitem WHERE pped_codigo='"+coddocumento+"' AND mped_numepedi="+numpedido+""))
				numpedido += 1;
			sqlStrings.Add("insert into	MPEDIDOITEM	(MPED_CLASEREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MPED_PEDIDO,MPED_CREACION,MPED_OBSERVACION,PALM_ALMACEN,SUSU_CODIGO,PVEN_CODIGO)	values ('"+clasereg+"','"+nit+"','"+coddocumento+"',"+numpedido.ToString()+",'"+fecha.ToString("yyyy-MM-dd")+"','"+FechaProc+"','"+observacion+"','"+almacen+"','"+HttpContext.Current.User.Identity.Name+"','"+vendedor+"')");
			string ItemC = "";
			double IDesc=0, IIva=0, IValU=0, CantPed=0;
			int	n=0, nPedPleno=0, nPedParcial=0, numR=0;
			double numUniPed=0,	numUniAsig=0, totCnIva = 0,	totSnIva = 0;
			ArrayList cantsPed = new ArrayList();
			ArrayList cantsAsig	= new ArrayList();
			ArrayList arDesc = new ArrayList();
			ArrayList arIVA	 = new ArrayList();
			ArrayList arValU = new ArrayList();

            //string AnoA	= DBFunctions.SingleData("SELECT pano_ano from cinventario");//Año Vigente Inventario
            //string MesA	= DBFunctions.SingleData("SELECT pmes_mes from cinventario");//Mes Vigente Inventario
            //string tp   = DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido	where pped_codigo='"+coddocumento+"'").Trim(); //Tipo de Pedido
            //string bko  = DBFunctions.SingleData("SELECT tped_backorder from tpedido	where tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='"+coddocumento+"')");//BackOrder?
            //string dem  = DBFunctions.SingleData("SELECT tped_demanda from tpedido where	tped_codigo=(SELECT	tped_codigo	FROM ppedido WHERE pped_codigo='"+coddocumento+"')");//Demanda?
            //string cntCst = DBFunctions.SingleData("SELECT pcen_centinv	FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + almacen + "'");
            //string carg	= DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vendedor+"'");//Cargo???????

            datosFijos();

            //Si es	una	transferencia a	taller se debe crear el	registro que une las tablas	de MPEDIDOTRANSFERENCIA, MPEDIDOITEM y MORDEN, ademas debemos realizar los movimientos de kardex indicados
            if (tp == "T")
			{
				if(DBFunctions.SingleData("SELECT TDOC_TIPODOCU FROM PDOCUMENTO where pDOC_codigo='"+tipoorden+"'") == "OT")
					sqlStrings.Add("insert into	MPEDIDOTRANSFERENCIA (PDOC_CODIGO,MORD_NUMEORDE,TCAR_CARGO,PPED_CODIGO,MPED_NUMERO)	VALUES('"+tipoorden+"',"+numorden.ToString()+",'"+cargoorden+"','"+coddocumento+"',"+numpedido.ToString()+")");
				else
					sqlStrings.Add("insert into	MPEDIDOPRODUCCIONTRANSFERENCIA (PDOC_CODIGO,MORD_NUMEORDE,PPED_CODIGO,MPED_NUMERO)	VALUES('"+tipoorden+"',"+numorden.ToString()+",'"+coddocumento+"',"+numpedido.ToString()+")");
			}
			for(n=0;n<dtSource.Rows.Count;n++)
			{
				numR++;
				ItemC =	dtSource.Rows[n][0].ToString();	//Codigo del Item 
				CantPed	= Convert.ToDouble(dtSource.Rows[n][5]); //Cantidad	Pedida 
				numUniPed += CantPed; // Numero	de unidades	pedidas	en total
				IValU =	Convert.ToDouble(dtSource.Rows[n][2]);//Valor del item Unitario
				arValU.Add(IValU);//Arreglo	de Valores Unitarios
				IIva=Convert.ToDouble(dtSource.Rows[n][3]);//Valor del Iva para	este Item
				arIVA.Add(IIva);//Arreglo de Valores de	IVA
				IDesc =	Convert.ToDouble(dtSource.Rows[n][4]);//Porcentaje de Descuento	para este Item
				arDesc.Add(IDesc);//Arreglo	de Porcentajes de descuento
				double ca =	0;
				if(clasereg	== "C")
				{
					ca = Referencias.ConsultarAsignacion(ItemC,almacen,CantPed);
					if(ca==CantPed)
					{
						nPedPleno++;
						numUniAsig+=ca;
					}
					else if(ca<CantPed && ca>0)
					{
						nPedParcial++;
						numUniAsig+=ca;
					}
				}
				else if(clasereg ==	"P")
					ca = CantPed;
				totSnIva +=	ca*IValU;
				totCnIva +=	ca*(IValU+(IValU*(IIva/100)));
				cantsAsig.Add(ca);//Arreglo	de cantidades asignadas
				cantsPed.Add(CantPed);//Arreglo	de cantidades pedidas
				if(clasereg	== "C")//DPEDIDOITEM
					sqlStrings.Add("insert into	DPEDIDOITEM	(MPED_CLASREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MITE_CODIGO,DPED_CANTPEDI,DPED_CANTASIG,DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,PIVA_PORCIVA) values ('"+clasereg+"','"+nit+"','"+coddocumento+"',"+numpedido.ToString()+",'"+ItemC+"',"+CantPed.ToString()+",0,"+ca.ToString()+","+IValU.ToString()+","+IDesc.ToString()+","+IIva.ToString()+")");
					//																																																	   CREGI,		NIT,  CODIGO DOCUMENTO	  ,MPED_NUMEPEDI,		  MITE_CODIGO,	  DPED_CANTPEDI,DPED_CANTASIG, DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,		PIVA_PORCIVA
				else
					sqlStrings.Add("insert into	DPEDIDOITEM	(MPED_CLASREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MITE_CODIGO,DPED_CANTPEDI,DPED_CANTASIG,DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,PIVA_PORCIVA) values ('"+clasereg+"','"+nit+"','"+coddocumento+"',"+numpedido.ToString()+",'"+ItemC+"',"+CantPed.ToString()+",0,0,"+IValU.ToString()+","+IDesc.ToString()+","+IIva.ToString()+")");
				//																																																CREGI,			NIT, CODIGO	DOCUMENTO	 ,MPED_NUMEPEDI,		 MITE_CODIGO,  DPED_CANTPEDI,CANTASIG,PED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,		PIVA_PORCIVA
				if(!DBFunctions.RecordExist("SELECT	* FROM msaldoitem WHERE	mite_codigo	= '"+ItemC+"' AND PANO_ANO="+AnoA))
					sqlStrings.Add("insert into	msaldoitem (mite_codigo,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_costhist,msal_costhisthist,msal_cantinveinic,msal_ultivent,msal_ultiingr,msal_ulticost,msal_ultiprov,msal_abcd,msal_peditrans,msal_unidtrans,msal_pedipendi,msal_unidpendi)	values ('"+ItemC+"',"+AnoA+",0,0,0,0,0,0,0,null,null,0,null,null,0,0,0,0)");
				if(!DBFunctions.RecordExist("SELECT	* FROM msaldoitemalmacen WHERE mite_codigo = '"	+ ItemC	+ "' AND PALM_ALMACEN='"+almacen+"'	and	pano_ano="+AnoA))
					sqlStrings.Add("insert into	msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist)	values ('"+ItemC+"','"+almacen+"',"+AnoA+",0,0,0,0)");
                //if(!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEMALMACEN WHERE pano_ano="+AnoA+" and pmes_mes="+MesA+" and tmov_tipomovi=70	AND	mite_codigo	= '"+ItemC+"' AND PALM_ALMACEN='"+almacen+"'"))
                //    sqlStrings.Add("insert into	MACUMULADOITEMALMACEN (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,PALM_ALMACEN,macu_cantidad,macu_costo,macu_precio) values ('"+ItemC+"',70,"+AnoA+","+MesA+",'"+almacen+"',0,0,0)");
                //if(!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEM WHERE	pano_ano="+AnoA+" and pmes_mes="+MesA+"	and	tmov_tipomovi=70 AND mite_codigo = '"+ItemC+"'"))
                //    sqlStrings.Add("insert into	MACUMULADOITEM (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,macu_cantidad,macu_costo,macu_precio) values ('"+ItemC+"',70,"+AnoA+","+MesA+",0,0,0)");
				if(clasereg	== "C")
				{
					if(bko=="S"	&& (CantPed-ca)>0)//Genera backorder
					{
						sqlStrings.Add("update msaldoitemalmacen set msal_cantpendiente=msal_cantpendiente+"+(CantPed-ca).ToString()+" where mite_codigo='"+ItemC+"' and pano_ano="+AnoA+" and palm_almacen='"+almacen+"'");
						sqlStrings.Add("update msaldoitem set msal_unidpendi=msal_unidpendi+"+(CantPed-ca).ToString()+", msal_pedipendi=msal_pedipendi+1 where mite_codigo='"+ItemC+"' and pano_ano="+AnoA);
					}
					if(dem=="S")
					{	//Genera demanda
						sqlStrings.Add("update macumuladoitemalmacen set macu_cantidad=macu_cantidad+"+CantPed.ToString()+"	WHERE pano_ano="+AnoA+"	and	pmes_mes="+MesA+" and tmov_tipomovi=70 AND mite_codigo = '"+ItemC+"' AND PALM_ALMACEN='"+almacen+"'");
						sqlStrings.Add("update macumuladoitem set macu_cantidad=macu_cantidad+"+CantPed.ToString()+" WHERE pano_ano="+AnoA+" and pmes_mes="+MesA+" and tmov_tipomovi=70	AND	mite_codigo	= '"+ItemC+"'");
					}
				}
				else if(clasereg ==	"P")
				{
					sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=msal_unidtrans+"+ca.ToString()+", msal_peditrans=msal_peditrans+1 WHERE mite_codigo='"+ItemC+"' and pano_ano="+AnoA);
					sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito+"+ca.ToString()+" WHERE mite_codigo='"+ItemC+"' AND pano_ano="+AnoA+" AND palm_almacen='"+almacen+"'");
				}
			}
			if(clasereg	== "C")
			{
				////*********************************************************************************////
				//Se llena el registro correspondiente a fillrate ????????????????
				//MFILLRATE
				if(!DBFunctions.RecordExist("SELECT	* FROM MFILLRATE WHERE MNIT_NIT='"+nit+"' AND MPED_NUMEPEDI="+numpedido.ToString()))
					sqlStrings.Add("insert into	MFILLRATE (MNIT_NIT,MPED_NUMEPEDI,MFILL_RENGPEDIDOS,MFILL_RENGSATISFTOT,MFILL_RENGSATISFPAR,MFILL_UNIDPEDIDAS,MFILL_UNIDASIGNADAS) values ('"+nit+"',"+numpedido.ToString()+","+numR.ToString()+","+nPedPleno.ToString()+","+nPedParcial.ToString()+","+numUniPed.ToString()+","+numUniAsig.ToString()+")");
				//																																											MNIT_NIT,MPED_NUMEPEDI,RENGPEDIDOS,				RENGSATISFTOT,		   SATISFPAR				   MFILL_UNIDPEDIDAS,		MFILL_UNIDASIGNADAS
				//LISTAEMPAQUE
				if(nPedParcial>0||nPedPleno>0)
				{
					//Facturar
					int	tipomovi;
					if(tp == "T")
						tipomovi = 80;//Es taller
					else 
						tipomovi = 90;	//No es	taller
					DateTime dv	= fecha.AddDays(diasplazo);
					DataSet	da = new DataSet();
					//*****************************************************************************************************
					//Se crea un objeto	de facturacion y se	extrae los sql relacionados	con	la grabacion
					FacturaCliente facturaRepuestos	= new FacturaCliente("FRC",	coddocumentof, nit,almacen,	"F", numdocumentof,
						Convert.ToUInt32(diasplazo), fecha,	dv,	dv,	totSnIva, totCnIva - totSnIva,
						valorfletes,ivafletes,0,0,cntCst,
						observacion,vendedor,HttpContext.Current.User.Identity.Name,kit);
					facturaRepuestos.GrabarFacturaCliente(false);
					for(n=0;n<facturaRepuestos.SqlStrings.Count;n++)
						sqlStrings.Add(facturaRepuestos.SqlStrings[n].ToString());
					numdocumentof =	facturaRepuestos.NumeroFactura;
					//*****************************************************************************************************
					Movimiento Mov = new Movimiento(coddocumentof,numdocumentof,coddocumento,numpedido,tipomovi,nit,almacen,fecha,vendedor,carg,"","S", numLis);
					double cant,valU,pIVA,pDesc,cantDev,valP;
					 
					double costoF=0;	//Costo	factura
					//MFACTURAORDEN
					if(tipomovi	== 80)
					{
						if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+tipoorden+"' ")=="OT")
							sqlStrings.Add("INSERT INTO	MORDENTRANSFERENCIA(PDOC_FACTURA,MFAC_NUMERO,PDOC_CODIGO,MORD_NUMEORDE,TCAR_CARGO) VALUES('"+coddocumentof+"',"+numdocumentof.ToString()+",'"+tipoorden+"',"+numorden+",'"+cargoorden+"')");
						else
							sqlStrings.Add("INSERT INTO	MORDENproduccionTRANSFERENCIA(PDOC_FACTURA,MFAC_NUMERO,PDOC_CODIGO,MORD_NUMEORDE) VALUES('"+coddocumentof+"',"+numdocumentof.ToString()+",'"+tipoorden+"',"+numorden+")");
					}
					for(n=0;n<dtSource.Rows.Count;n++)
					{
						codI = dtSource.Rows[n][0].ToString();//codigoitem
						cant = Convert.ToDouble(cantsAsig[n]);
						valU = Convert.ToDouble(arValU[n]);//valor unidad
						cantI =	Convert.ToDouble(dtSource.Rows[n][5]);//cantidad pedida
						da.Clear();
						//try{costP =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_costprom FROM msaldoitem WHERE	pano_ano="+AnoA+" AND mite_codigo='"+codI+"'"));}
						//catch{costP=0;}
						//try{costPH = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM	msaldoitem WHERE pano_ano="+AnoA+" AND mite_codigo='"+codI+"'"));}
						//catch{costPH=0;}
						//try{costPA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM	msaldoitemalmacen WHERE	pano_ano="+AnoA+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
						//catch{costPA=0;}
						//try{costPHA	= Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist	FROM msaldoitemalmacen WHERE pano_ano="+AnoA+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
						//catch{costPHA=0;}
						//try{invI = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM	msaldoitem WHERE pano_ano="+AnoA+" AND mite_codigo='"+codI+"'"));}
						//catch{invI=0;}
						//try{invIA =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_cantactual	FROM msaldoitemalmacen WHERE pano_ano="+AnoA+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
						//catch{invIA=0;}

                        costosItem();
                        ppedCodigo = dtSource.Rows[n]["PPED_CODIGO"].ToString();
                        ppedNumero = dtSource.Rows[n]["MPED_NUMEPEDI"].ToString();
                        pIVA = Convert.ToDouble(arIVA[n]);//iva
						pDesc =	Convert.ToDouble(arDesc[n]);//descuento
						cantDev	= 0;
						valP = valU; 
						costoF+=valP*cant;
						Mov.InsertaFila(codI,cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA,cant, ppedCodigo, ppedNumero);
						sqlStrings.Add("UPDATE dpedidoitem SET pdoc_codigo='"+facturaRepuestos.PrefijoFactura+"', mfac_numedocu="+facturaRepuestos.NumeroFactura+" WHERE pped_codigo='"+coddocumento+"'	AND	mped_numepedi="+numpedido+"	AND	mite_codigo='"+codI+"'");
					}
					Mov.RealizarMov(false);
					for(n=0;n<Mov.SqlStrings.Count;n++)
						sqlStrings.Add(Mov.SqlStrings[n].ToString());
				}
				////*********************************************************************************////
			}
			//DOCUMENTO
			uint ultiPed = Convert.ToUInt32(DBFunctions.SingleData("SELECT pped_ultipedi FROM ppedido WHERE	pped_codigo	= '"+coddocumento+"'"));
			if(numpedido>ultiPed)
				sqlStrings.Add("UPDATE ppedido set pped_ultipedi="+numpedido+" WHERE pped_codigo = '"+coddocumento+"'");
			if(guardar)
			{
				if(DBFunctions.Transaction(sqlStrings))
					processMsg +="<br>Bien <br>"+ DBFunctions.exceptions;
				else
				{
					status = false;
					processMsg +="<br>ERROR<br>"+ DBFunctions.exceptions;
				}
			}
			return status;
		}

        private void datosFijos()
        {
            DataSet dsFijos = new DataSet();
            DBFunctions.Request(dsFijos, IncludeSchema.NO,
                @"SELECT pano_ano as anoa, pmes_mes as mesa,  pp.TPED_CODIGO as tp, tped_backorder as bko, tped_demanda as dem, pcen_centinv As cntcst, TVEND_CODIGO AS CARG
                    from cinventario, ppedido pp,  tpedido tp, palmacen pa, PVENDEDOR PV
                   where pp.pped_codigo=TRIM('" + coddocumento + @"')
                     and tp.tped_codigo = pp.tped_codigo
                     and pa.palm_almacen = '" + almacen + @"'
                     AND PV.PVEN_CODIGO = '"+vendedor+"'; ");
            AnoA = dsFijos.Tables[0].Rows[0][0].ToString();
            MesA = dsFijos.Tables[0].Rows[0][1].ToString();
            tp   = dsFijos.Tables[0].Rows[0][2].ToString();
            bko  = dsFijos.Tables[0].Rows[0][3].ToString();
            dem  = dsFijos.Tables[0].Rows[0][4].ToString();
            cntCst = dsFijos.Tables[0].Rows[0][5].ToString();
            carg = dsFijos.Tables[0].Rows[0][6].ToString();
            dsFijos.Clear();
        }

        private void costosItem ()
        {
            DataSet dsCostos = new DataSet();
            DBFunctions.Request(dsCostos, IncludeSchema.NO,
                  @"select coalesce(MS.msal_costprom,0) as costP, coalesce(MS.msal_costpromhist,0) as costPH, COALESCE(MSA.msal_costprom,0) AS costPA, 
                           COALESCE(MSA.msal_costpromhist,0) AS costPHA, COALESCE(MS.msal_cantactual,0) AS invI, COALESCE(MSA.msal_cantactual,0) AS invIA, mi.tori_codigo as origenItem      
                      from MITEMS MI, msaldoitem ms
                      leFT join msaldoitemalmacen msa on ms.pano_ano = msa.pano_ano and ms.mite_codigo= msa.mite_codigo and msa.palm_almacen = '" + almacen + @"'
                     WHERE MS.pano_ano = "+AnoA+@" AND MS.mite_codigo = '"+codI+@"' AND ms.mite_codigo = mi.mite_codigo; ");
            if (dsCostos.Tables.Count == 0 || dsCostos.Tables[0].Rows.Count == 0)
                costP = costPH = costPA = costPHA = invI = invIA = 0; // el único evento es cuando se factura un SERVICIO por Inventarios
            else
            {
                costP = Convert.ToDouble(dsCostos.Tables[0].Rows[0][0].ToString());
                costPH = Convert.ToDouble(dsCostos.Tables[0].Rows[0][1].ToString());
                costPA = Convert.ToDouble(dsCostos.Tables[0].Rows[0][2].ToString());
                costPHA = Convert.ToDouble(dsCostos.Tables[0].Rows[0][3].ToString());
                invI = Convert.ToDouble(dsCostos.Tables[0].Rows[0][4].ToString());
                invIA = Convert.ToDouble(dsCostos.Tables[0].Rows[0][5].ToString());
            }
            dsCostos.Clear();
        }

        //--------------------------------------------------------------------
        public bool	RealizarPedProdFac(bool	guardar)
		{
			//Primero creamos el pedido	
			bool status	= true;
			string FechaProc = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.ffffff");
			//MPEDIDOITEM
			while(DBFunctions.RecordExist("SELECT *	FROM mpedidoitem WHERE pped_codigo='"+coddocumento+"' AND mped_numepedi="+numpedido+""))
				numpedido += 1;
			sqlStrings.Add("insert into	MPEDIDOITEM	(MPED_CLASEREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MPED_PEDIDO,MPED_CREACION,MPED_OBSERVACION,PALM_ALMACEN,SUSU_CODIGO,PVEN_CODIGO)	values ('"+clasereg+"','"+nit+"','"+coddocumento+"',"+numpedido.ToString()+",'"+fecha.ToString("yyyy-MM-dd")+"','"+FechaProc+"','"+observacion+"','"+almacen+"','"+HttpContext.Current.User.Identity.Name+"','"+vendedor+"')");
			string ItemC = "";
			double IDesc=0,	IIva=0,	IValU=0, CantPed=0;
			int	n=0, nPedPleno=0, nPedParcial=0, numR=0;
			double numUniPed=0,	numUniAsig=0, totCnIva = 0,	totSnIva = 0;
			ArrayList cantsPed = new ArrayList();
			ArrayList cantsAsig	= new ArrayList();
			ArrayList arDesc = new ArrayList();
			ArrayList arIVA	= new ArrayList();
			ArrayList arValU = new ArrayList();

            // SE AGRUPAN EN EL METODO datosfijos(); hac Mar-7-2018
			//string AnoA	= DBFunctions.SingleData("SELECT pano_ano from cinventario");//Año Vigente Contable
			//string MesA	= DBFunctions.SingleData("SELECT pmes_mes from cinventario");//Mes Vigente Contable
			//string tp =	DBFunctions.SingleData("SELECT TPED_CODIGO FROM	ppedido	where pped_codigo='"+coddocumento+"'").Trim(); //Tipo de Pedido
			//string bko = DBFunctions.SingleData("SELECT	tped_backorder from	tpedido	where tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='"+coddocumento+"')");//BackOrder?
			//string dem = DBFunctions.SingleData("SELECT	tped_demanda from tpedido where	tped_codigo=(SELECT	tped_codigo	FROM ppedido WHERE pped_codigo='"+coddocumento+"')");//Demanda?
			//string cntCst =	DBFunctions.SingleData("SELECT pcen_centinv	FROM palmacen WHERE	palm_almacen='"+almacen+"'");
			//string carg	= DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vendedor+"'");//Cargo???????

            datosFijos();

			//Si es	una	transferencia a	taller se debe crear el	registro que une las tablas	de MPEDIDOTRANSFERENCIA, MPEDIDOITEM y MORDEN, ademas debemos realizar los movimientos de kardex indicados
            if (tp == "T")
				sqlStrings.Add("insert into	MPEDIDOPRODUCCIONTRANSFERENCIA (PDOC_CODIGO,MORD_NUMEORDE,PPED_CODIGO,MPED_NUMERO) VALUES('"+tipoorden+"',"+numorden.ToString()+",'"+coddocumento+"',"+numpedido.ToString()+")");
			for(n=0;n<dtSource.Rows.Count;n++)
			{
				numR++;
				ItemC =	dtSource.Rows[n][0].ToString();	//Codigo del Item 
				CantPed	= Convert.ToDouble(dtSource.Rows[n][5]);	//Cantidad Pedida 
				numUniPed += CantPed; // Numero	de unidades	pedidas	en total
				IValU =	Convert.ToDouble(dtSource.Rows[n][2]);//Valor del item Unitario
				arValU.Add(IValU);//Arreglo	de Valores Unitarios
				IIva=Convert.ToDouble(dtSource.Rows[n][3]);//Valor del Iva para	este Item
				arIVA.Add(IIva);//Arreglo de Valores de	IVA
				IDesc =	Convert.ToDouble(dtSource.Rows[n][4]);//Porcentaje de Descuento	para este Item
				arDesc.Add(IDesc);//Arreglo	de Porcentajes de descuento
				double ca =	0;
				if(clasereg	== "C")
				{
					ca = Referencias.ConsultarAsignacion(ItemC,almacen,CantPed);
					if(ca==CantPed)
					{
						nPedPleno++;
						numUniAsig+=ca;
					}
					else if(ca<CantPed && ca>0)
					{
						nPedParcial++;
						numUniAsig+=ca;
					}
				}
				else if(clasereg ==	"P")
					ca = CantPed;
				totSnIva +=	ca*IValU;
				totCnIva +=	ca*(IValU+(IValU*(IIva/100)));
				cantsAsig.Add(ca);//Arreglo	de cantidades asignadas
				cantsPed.Add(CantPed);//Arreglo	de cantidades pedidas
				if(clasereg	== "C")//DPEDIDOITEM
					sqlStrings.Add("insert into	DPEDIDOITEM	(MPED_CLASREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MITE_CODIGO,DPED_CANTPEDI,DPED_CANTASIG,DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,PIVA_PORCIVA) values ('"+clasereg+"','"+nit+"','"+coddocumento+"',"+numpedido.ToString()+",'"+ItemC+"',"+CantPed.ToString()+",0,"+ca.ToString()+","+IValU.ToString()+","+IDesc.ToString()+","+IIva.ToString()+")");
					//																																																	   CREGI,		NIT,  CODIGO DOCUMENTO	  ,MPED_NUMEPEDI,		  MITE_CODIGO,	  DPED_CANTPEDI,DPED_CANTASIG, DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,		PIVA_PORCIVA
				else
					sqlStrings.Add("insert into	DPEDIDOITEM	(MPED_CLASREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MITE_CODIGO,DPED_CANTPEDI,DPED_CANTASIG,DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,PIVA_PORCIVA) values ('"+clasereg+"','"+nit+"','"+coddocumento+"',"+numpedido.ToString()+",'"+ItemC+"',"+CantPed.ToString()+",0,0,"+IValU.ToString()+","+IDesc.ToString()+","+IIva.ToString()+")");
				//																																																CREGI,			NIT, CODIGO	DOCUMENTO	 ,MPED_NUMEPEDI,		 MITE_CODIGO,  DPED_CANTPEDI,CANTASIG,PED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,		PIVA_PORCIVA
				if(!DBFunctions.RecordExist("SELECT	* FROM msaldoitem WHERE	mite_codigo	= '"+ItemC+"' AND PANO_ANO="+AnoA))
					sqlStrings.Add("insert into	msaldoitem (mite_codigo,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_costhist,msal_costhisthist,msal_cantinveinic,msal_ultivent,msal_ultiingr,msal_ulticost,msal_ultiprov,msal_abcd,msal_peditrans,msal_unidtrans,msal_pedipendi,msal_unidpendi)	values ('"+ItemC+"',"+AnoA+",0,0,0,0,0,0,0,null,null,0,null,null,0,0,0,0)");
				if(!DBFunctions.RecordExist("SELECT	* FROM msaldoitemalmacen WHERE mite_codigo = '"	+ ItemC	+ "' AND PALM_ALMACEN='"+almacen+"'	and	pano_ano="+AnoA))
					sqlStrings.Add("insert into	msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist)	values ('"+ItemC+"','"+almacen+"',"+AnoA+",0,0,0,0)");
                //if(!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEMALMACEN WHERE pano_ano="+AnoA+" and pmes_mes="+MesA+" and tmov_tipomovi=70	AND	mite_codigo	= '"+ItemC+"' AND PALM_ALMACEN='"+almacen+"'"))
                //    sqlStrings.Add("insert into	MACUMULADOITEMALMACEN (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,PALM_ALMACEN,macu_cantidad,macu_costo,macu_precio) values ('"+ItemC+"',70,"+AnoA+","+MesA+",'"+almacen+"',0,0,0)");
                //if(!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEM WHERE	pano_ano="+AnoA+" and pmes_mes="+MesA+"	and	tmov_tipomovi=70 AND mite_codigo = '"+ItemC+"'"))
                //    sqlStrings.Add("insert into	MACUMULADOITEM (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,macu_cantidad,macu_costo,macu_precio) values ('"+ItemC+"',70,"+AnoA+","+MesA+",0,0,0)");
				if(clasereg	== "C")
				{
					if(bko=="S"	&& (CantPed-ca)>0)//Genera backorder
					{
						sqlStrings.Add("update msaldoitemalmacen set msal_cantpendiente=msal_cantpendiente+"+(CantPed-ca).ToString()+" where mite_codigo='"+ItemC+"' and pano_ano="+AnoA+" and palm_almacen='"+almacen+"'");
						sqlStrings.Add("update msaldoitem set msal_unidpendi=msal_unidpendi+"+(CantPed-ca).ToString()+", msal_pedipendi=msal_pedipendi+1 where mite_codigo='"+ItemC+"' and pano_ano="+AnoA);
					}
					if(dem=="S")
					{	//Genera demanda
						sqlStrings.Add("update macumuladoitemalmacen set macu_cantidad=macu_cantidad+"+CantPed.ToString()+"	WHERE pano_ano="+AnoA+"	and	pmes_mes="+MesA+" and tmov_tipomovi=70 AND mite_codigo = '"+ItemC+"' AND PALM_ALMACEN='"+almacen+"'");
						sqlStrings.Add("update macumuladoitem set macu_cantidad=macu_cantidad+"+CantPed.ToString()+" WHERE pano_ano="+AnoA+" and pmes_mes="+MesA+" and tmov_tipomovi=70	AND	mite_codigo	= '"+ItemC+"'");
					}
				}
				else if(clasereg ==	"P")
				{
					sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=msal_unidtrans+"+ca.ToString()+", msal_peditrans=msal_peditrans+1 WHERE mite_codigo='"+ItemC+"' and pano_ano="+AnoA);
					sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito+"+ca.ToString()+" WHERE mite_codigo='"+ItemC+"' AND pano_ano="+AnoA+" AND palm_almacen='"+almacen+"'");
				}
			}
			if(clasereg	== "C")
			{
				////*********************************************************************************////
				//Se llena el registro correspondiente a fillrate ????????????????
				//MFILLRATE
				if(!DBFunctions.RecordExist("SELECT	* FROM MFILLRATE WHERE MNIT_NIT='"+nit+"' AND MPED_NUMEPEDI="+numpedido.ToString()))
					sqlStrings.Add("insert into	MFILLRATE (MNIT_NIT,MPED_NUMEPEDI,MFILL_RENGPEDIDOS,MFILL_RENGSATISFTOT,MFILL_RENGSATISFPAR,MFILL_UNIDPEDIDAS,MFILL_UNIDASIGNADAS) values ('"+nit+"',"+numpedido.ToString()+","+numR.ToString()+","+nPedPleno.ToString()+","+nPedParcial.ToString()+","+numUniPed.ToString()+","+numUniAsig.ToString()+")");
				//																																											MNIT_NIT,MPED_NUMEPEDI,RENGPEDIDOS,				RENGSATISFTOT,		   SATISFPAR				   MFILL_UNIDPEDIDAS,		MFILL_UNIDASIGNADAS
				//LISTAEMPAQUE
				if(nPedParcial>0||nPedPleno>0)
				{
					//Facturar
					int	tipomovi;
					if(tp == "T")
						tipomovi = 80;//Es taller
					else 
						tipomovi = 90;	//No es	taller
					DateTime dv	= fecha.AddDays(diasplazo);
					DataSet	da = new DataSet();
					//*****************************************************************************************************
					//Se crea un objeto	de facturacion y se	extrae los sql relacionados	con	la grabacion
					FacturaCliente facturaRepuestos	= new FacturaCliente("FRC",	coddocumentof, nit,almacen,	"F", numdocumentof,
						Convert.ToUInt32(diasplazo), fecha,	dv,	dv,	totSnIva, totCnIva - totSnIva,
						valorfletes,ivafletes,0,0,cntCst,
						observacion,vendedor,HttpContext.Current.User.Identity.Name,kit);
					facturaRepuestos.GrabarFacturaCliente(false);
					for(n=0;n<facturaRepuestos.SqlStrings.Count;n++)
						sqlStrings.Add(facturaRepuestos.SqlStrings[n].ToString());
					numdocumentof =	facturaRepuestos.NumeroFactura;
					//*****************************************************************************************************
					Movimiento Mov = new Movimiento(coddocumentof,numdocumentof,coddocumento,numpedido,tipomovi,nit,almacen,fecha,vendedor,carg,"","S", numLis);
					double cant,valU,pIVA,pDesc,cantDev,valP;
					 
					double costoF=0;	//Costo	factura
					//MFACTURAORDEN
                    if (tipomovi == 80)
						sqlStrings.Add("INSERT INTO	MORDENPRODUCCIONTRANSFERENCIA(PDOC_FACTURA,MFAC_NUMERO,PDOC_CODIGO,MORD_NUMEORDE) VALUES('"+coddocumentof+"',"+numdocumentof.ToString()+",'"+tipoorden+"',"+numorden+")");
					for(n=0;n<dtSource.Rows.Count;n++)
					{
						codI = dtSource.Rows[n][0].ToString();//codigoitem
						cant = Convert.ToDouble(cantsAsig[n]);
						valU = Convert.ToDouble(arValU[n]);//valor unidad
						cantI =	Convert.ToDouble(dtSource.Rows[n][5]);//cantidad pedida
						da.Clear();
						//try{costP =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_costprom FROM msaldoitem WHERE	pano_ano="+AnoA+" AND mite_codigo='"+codI+"'"));}
						//catch{costP=0;}
						//try{costPH = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM	msaldoitem WHERE pano_ano="+AnoA+" AND mite_codigo='"+codI+"'"));}
						//catch{costPH=0;}
						//try{costPA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM	msaldoitemalmacen WHERE	pano_ano="+AnoA+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
						//catch{costPA=0;}
						//try{costPHA	= Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist	FROM msaldoitemalmacen WHERE pano_ano="+AnoA+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
						//catch{costPHA=0;}
						//try{ = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM	msaldoitem WHERE pano_ano="+AnoA+" AND mite_codigo='"+codI+"'"));}
						//catch{invI=0;}
						//try{invIA =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_cantactual	FROM msaldoitemalmacen WHERE pano_ano="+AnoA+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
						//catch{invIA=0;}

                        costosItem();
                        ppedCodigo = dtSource.Rows[n]["PPED_CODIGO"].ToString();
                        ppedNumero = dtSource.Rows[n]["MPED_NUMEPEDI"].ToString();
                        pIVA = Convert.ToDouble(arIVA[n]);//iva
						pDesc =	Convert.ToDouble(arDesc[n]);//descuento
						cantDev	= 0;
						valP = valU; 
						costoF+=valP*cant;
						Mov.InsertaFila(codI,cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA,cant, ppedCodigo, ppedNumero);
						sqlStrings.Add("UPDATE dpedidoitem SET pdoc_codigo='"+facturaRepuestos.PrefijoFactura+"', mfac_numedocu="+facturaRepuestos.NumeroFactura+" WHERE pped_codigo='"+coddocumento+"'	AND	mped_numepedi="+numpedido+"	AND	mite_codigo='"+codI+"'");
					}
					Mov.RealizarMov(false);
					for(n=0;n<Mov.SqlStrings.Count;n++)
						sqlStrings.Add(Mov.SqlStrings[n].ToString());
				}
				////*********************************************************************************////
			}
			//DOCUMENTO
			uint ultiPed = Convert.ToUInt32(DBFunctions.SingleData("SELECT pped_ultipedi FROM ppedido WHERE	pped_codigo	= '"+coddocumento+"'"));
			if(numpedido>ultiPed)
				sqlStrings.Add("UPDATE ppedido set pped_ultipedi="+numpedido+" WHERE pped_codigo = '"+coddocumento+"'");
			if(guardar)
			{
				if(DBFunctions.Transaction(sqlStrings))
					processMsg +="<br>Bien <br>"+ DBFunctions.exceptions;
				else
				{
					status = false;
					processMsg +="<br>ERROR<br>"+ DBFunctions.exceptions;
				}
			}
			return status;
		}
		//--------------------------------------------------------------------
		//**********************************************************************************************************///////
		//**********************************************************************************************************///////
		//**********************************************************************************************************///////
		
		public bool	RealizarFac(bool guardar)
		{
			bool status	= true;
            uint ndref = 0;
            //Facturar
            string ano_cont	= DBFunctions.SingleData("SELECT pano_ano from cinventario");
            AnoA = ano_cont;

            DataSet	ds = new DataSet();
            string[] numLs = numerolista.ToString().Split('-');
            for (int i=0; i < numLs.Length -1; i++)
            {
                DBFunctions.Request(ds, IncludeSchema.NO, "Select	PPED_CODIGO,MPED_NUMEPEDI,MNIT_NIT, MLIS_NUMERO from	DLISTAEMPAQUE where	MLIS_NUMERO=" + numLs[i] + "");
            }            
			string pdref = ds.Tables[0].Rows[0][0].ToString(); //Prefijo del pedido
			string tp =	DBFunctions.SingleData("SELECT TPED_CODIGO FROM	ppedido	where pped_codigo='"+pdref+"'");//Tipo del Pedido
			int	tipomovi;
			if(tp == "T")
				tipomovi = 80;//Es taller
			else 
				tipomovi = 90;	//No es	taller
            string tipoDocFac = "F";
            if (tp == "E")  // El tipo de pedido es CONSUMO INTERNO, el tipo de documento se graba con I caso contrario es F
            {
                tipoDocFac = "I";
                tipomovi = 60;	//Consumo Interno
            }
            
            for (int i = 0; i < numLs.Length - 1; i++)
            {
                numLis += ds.Tables[i].Rows[0][1].ToString() + ".";//Numero	del	pedido
            }
            ndref = Convert.ToUInt16(ds.Tables[0].Rows[0][1].ToString());
            string nnit	= ds.Tables[0].Rows[0][2].ToString();//Nit del cliente que realiza el pedido
			string palm	= DBFunctions.SingleData("SELECT PALM_ALMACEN FROM MLISTAEMPAQUE where MLIS_NUMERO="+ numerolista[0] + "");//Almacen
            almacen = palm;
            string kit  = DBFunctions.SingleData("SELECT Pkit_codigo  FROM MLISTAEMPAQUE where MLIS_NUMERO=" + numerolista[0] + "");//kit
            string cntCst = DBFunctions.SingleData("SELECT pcen_centinv	FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + palm + "'");
			string carg	= DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vendedor+"'");//Cargo???????
			DateTime dv	= fecha.AddDays(diasplazo);
			DataSet	da = new DataSet();
			ArrayList sqlUpdateDPedidoItem = new ArrayList();
			//*****************************************************************************************************
			//Se crea un objeto	de facturacion y se	extrae los sql relacionados	con	la grabacion
            FacturaCliente facturaRepuestos = new FacturaCliente("FRC", coddocumentof, nnit, palm, tipoDocFac, numdocumentof,
				Convert.ToUInt32(diasplazo), fecha,	dv,	dv,	totalsiniva, totalconiva - totalsiniva,
				valorfletes,ivafletes,0,0,cntCst,
				observacion,vendedor,HttpContext.Current.User.Identity.Name,kit);
		if(facturaRepuestos.GrabarFacturaCliente(false))
		{
            for(int	n=0;n<facturaRepuestos.SqlStrings.Count;n++)
				sqlStrings.Add(facturaRepuestos.SqlStrings[n].ToString());
			numdocumentof =	facturaRepuestos.NumeroFactura;
			//*****************************************************************************************************
			Movimiento Mov = new Movimiento(coddocumentof,numdocumentof,pdref,ndref,tipomovi,nnit,palm,fecha,vendedor,carg,"","N", numerolista);
			double cant,valU,pIVA,pDesc,cantDev,valP,cantP;
			 
			double costoF=0;	//Costo	factura
			//MFACTURAORDEN
			if(tipomovi == 80)
			{
				if(DBFunctions.RecordExist("SELECT * from MPEDIDOTRANSFERENCIA WHERE	PPED_CODIGO='"+pdref+"'	AND	MPED_NUMERO="+ndref.ToString()+";"))
				{	
					DBFunctions.Request(da,	IncludeSchema.NO,"Select pdoc_codigo, mord_numeorde, tcar_cargo	from MPEDIDOTRANSFERENCIA WHERE	PPED_CODIGO='"+pdref+"'	AND	MPED_NUMERO="+ndref.ToString());
					sqlStrings.Add("INSERT INTO	MORDENTRANSFERENCIA	(PDOC_FACTURA,MFAC_NUMERO,PDOC_CODIGO,MORD_NUMEORDE,TCAR_CARGO)	VALUES('"+coddocumentof+"',"+numdocumentof.ToString()+",'"+da.Tables[0].Rows[0][0].ToString()+"',"+da.Tables[0].Rows[0][1].ToString()+",'"+cargoorden+"')");
					da.Clear();
				}
				else
				{	
					DBFunctions.Request(da,	IncludeSchema.NO,"Select pdoc_codigo, mord_numeorde from MPEDIDOPRODUCCIONTRANSFERENCIA WHERE	PPED_CODIGO='"+pdref+"'	AND	MPED_NUMERO="+ndref.ToString());
					sqlStrings.Add("INSERT INTO	MORDENproduccionTRANSFERENCIA	(PDOC_FACTURA,MFAC_NUMERO,PDOC_CODIGO,MORD_NUMEORDE)	VALUES('"+coddocumentof+"',"+numdocumentof.ToString()+",'"+da.Tables[0].Rows[0][0].ToString()+"',"+da.Tables[0].Rows[0][1].ToString()+")");
					da.Clear();
				}
		
			}
			for(int	n=0;n<dtSource.Rows.Count;n++)
			{
				codI = dtSource.Rows[n][0].ToString();//codigoitem
				cant = Convert.ToDouble(dtSource.Rows[n][1]);//cantidfad facturada
				valU = Convert.ToDouble(dtSource.Rows[n][2]);//valor unidad
				cantI =	Convert.ToDouble(dtSource.Rows[n][5]);//cantidad pedida
				da.Clear();
				//try{costP =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_costprom FROM msaldoitem WHERE	pano_ano="+ano_cont+" AND mite_codigo='"+codI+"'"));}
				//catch{costP=0;}
				//try{costPH = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM	msaldoitem WHERE pano_ano="+ano_cont+" AND mite_codigo='"+codI+"'"));}
				//catch{costPH=0;}
				//try{costPA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM	msaldoitemalmacen WHERE	pano_ano="+ano_cont+" AND palm_almacen='"+palm+"' AND mite_codigo='"+codI+"'"));}
				//catch{costPA=0;}
				//try{costPHA	= Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist	FROM msaldoitemalmacen WHERE pano_ano="+ano_cont+" AND palm_almacen='"+palm+"' AND mite_codigo='"+codI+"'"));}
				//catch{costPHA=0;}
				//try{invI = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM	msaldoitem WHERE pano_ano="+ano_cont+" AND mite_codigo='"+codI+"'"));}
				//catch{invI=0;}
				//try{invIA =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_cantactual	FROM msaldoitemalmacen WHERE pano_ano="+ano_cont+" AND palm_almacen='"+palm+"' AND mite_codigo='"+codI+"'"));}
				//catch{invIA=0;}

                costosItem();

				pIVA = Convert.ToDouble(dtSource.Rows[n][3]);//iva
				pDesc =	Convert.ToDouble(dtSource.Rows[n][4]);//descuento
				cantDev	= 0;
				valP = valU; 
				costoF+=valP*cant;
                ppedCodigo = dtSource.Rows[n]["PPED_CODIGO"].ToString();//Codigo del Pedido
                ppedNumero = dtSource.Rows[n]["PPED_NUMEPEDI"].ToString();//Codigo del Pedido
                                                                                  //DPEDIDOITEM
                double cantAs =	Convert.ToDouble(dtSource.Rows[n][5]);//cantidad lista empaque original
				Mov.InsertaFila(codI,cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA,cantI, ppedCodigo, ppedNumero);//AQUI	VA EL ARREGLO
				try{cantP=Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi	from DPEDIDOITEM  where	mped_clasregi='C' AND MNIT_NIT='"+nnit+"' and pped_codigo='"+pdref+"' and mped_numepedi="+ndref.ToString()+" and mite_codigo='"+codI+"'"));}
				catch{cantP=0;};
                string[] numPed = numLis.ToString().Split('-');
                for (int i = 0; i < numLs.Length - 1; i++)
                 {
                   //Prefijo del  Pedido -	pdref ;	Numero de Factura -	ndref
                   sqlUpdateDPedidoItem.Add("UPDATE DPEDIDOITEM SET dped_cantasig=dped_cantasig-" + cantAs.ToString() + ",	 dped_cantfact=dped_cantfact+" + cant.ToString() + ", pdoc_codigo='" + coddocumentof + "', mfac_numedocu=" + numdocumentof + " where mped_clasregi='C' AND MNIT_NIT='" + nnit + "' and pped_codigo='" + pdref + "' and mped_numepedi=" + numLs[i].ToString() + " and mite_codigo='" + codI + "'");
                 }
                   
			}
			for(int	n=0;n<sqlUpdateDPedidoItem.Count;n++)
				sqlStrings.Add(sqlUpdateDPedidoItem[n].ToString());
                //Ahora	vamos a	eliminar la	lista de empaques por completo
                string[] numLsEm = numerolista.Split('-');
                for (int m=0; m < numLsEm.Length - 1; m++)
                {
                    sqlStrings.Add("DELETE FROM	dlistaempaque where	mlis_numero=" + numLsEm[m] + "");
                    sqlStrings.Add("DELETE FROM	mlistaempaque where	mlis_numero=" + numLsEm[m] + "");
                }
            // Borra todas las listas de empaque que no tengo items
            sqlStrings.Add("DELETE FROM MLISTAEMPAQUE WHERE MLIS_NUMERO IN (SELECT M.MLIS_NUMERO FROM mlistaempaque M LEFT JOIN DLISTAEMPAQUE D  ON M.MLIS_NUMERO = D.MLIS_NUMERO WHERE D.MLIS_NUMERO IS NULL);");
			Mov.RealizarMov(false);
			for(int	n=0;n<Mov.SqlStrings.Count;n++)
				sqlStrings.Add(Mov.SqlStrings[n].ToString());
			if(guardar)
			{
				if(DBFunctions.Transaction(sqlStrings))
					processMsg +="<br>Bien Parte 1:<br>"+ DBFunctions.exceptions;
				else
				{
					status = false;
					processMsg +="<br>ERROR	Parte 1:<br>"+ DBFunctions.exceptions;
				}
			}
			return status;
         }
         else
            {
                status = false;
                processMsg += "<br>ERROR: el CONSECUTIVO esta fuera de los rangos autorizados o la FECHA de resolución ya esta vencida. Por favor informe a Contabilidad <br>";
                return status;
            }
      	}
		
		public bool	RealizarFacDir()
		{
			bool status	= true;
			string ano_cont	= DBFunctions.SingleData("SELECT pano_ano from cinventario");
			//Se debe determinar que tipo de movimiento	se realizara si	es una salida de cliente o de taller
			//Para este	usamos el prefijo del documento	de facturacion : si	es una transferencia es	movimiento 80 sino es 90
			string tp =	DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+coddocumentof+"'");
			DataSet	da = new DataSet();
			ArrayList sqlUpdateDPedidoItem = new ArrayList();
			ArrayList Ordenes =	new	ArrayList();
			int	tipomovi;
			if(tp == "TT")
				tipomovi = 80;//Es taller
			else 
				tipomovi = 90;	//No es	taller
			string carg	= DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vendedor+"'");
			while(DBFunctions.RecordExist("SELECT *	FROM mfacturacliente WHERE pdoc_codigo='"+coddocumentof+"' AND mfac_numedocu="+numdocumentof+""))
				numdocumentof += 1;
			//Ahora	vamos a	crear el objeto	de movimiento con el segundo constructor
			Movimiento Mov = new Movimiento(coddocumentof,numdocumentof,tipomovi,fecha,"","",vendedor,DBFunctions.SingleData("SELECT tven_codigo FROM pvendedor	WHERE pven_codigo='"+vendedor+"'"),"","N");
			double cant,valU,pIVA,pDesc,cantDev,valP;
			string prefPed,prefOT,numOT,cargo;
			uint numPed;
			DateTime dv	= fecha.AddDays(diasplazo);; //Fecha de	vencimiento
			double costoF =	0;	//Costo	factura
			for(int	n=0;n<dtSource.Rows.Count;n++)
			{
				codI = dtSource.Rows[n][0].ToString();//codigoitem
				cant = Convert.ToDouble(dtSource.Rows[n][1]);//cantidad	facturada
				valU = Convert.ToDouble(dtSource.Rows[n][2]);//valor unidad
				cantI =	Convert.ToDouble(dtSource.Rows[n][5]);//cantidad pedida
				//try{costP =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_costprom FROM msaldoitem WHERE	pano_ano="+ano_cont+" AND mite_codigo='"+codI+"'"));}
				//catch{costP=0;}
				//if(tipo.Equals("OP"))valU=costP;
				//try{costPH = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM	msaldoitem WHERE pano_ano="+ano_cont+" AND mite_codigo='"+codI+"'"));}
				//catch{costPH=0;}
				//try{costPA = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM	msaldoitemalmacen WHERE	pano_ano="+ano_cont+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
				//catch{costPA=0;}
				//try{costPHA	= Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist	FROM msaldoitemalmacen WHERE pano_ano="+ano_cont+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
				//catch{costPHA=0;}
				//try{invI = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM	msaldoitem WHERE pano_ano="+ano_cont+" AND mite_codigo='"+codI+"'"));}
				//catch{invI=0;}
				//try{invIA =	Convert.ToDouble(DBFunctions.SingleData("SELECT	msal_cantactual	FROM msaldoitemalmacen WHERE pano_ano="+ano_cont+" AND palm_almacen='"+almacen+"' AND mite_codigo='"+codI+"'"));}
				//catch{invIA=0;}

                costosItem();

				pIVA = Convert.ToDouble(dtSource.Rows[n][3]);//iva
				pDesc =	Convert.ToDouble(dtSource.Rows[n][4]);//descuento
				if(tipo.Equals("OP"))pDesc=0;
				prefPed	= (dtSource.Rows[n][6].ToString().Split('-'))[0];
				numPed = Convert.ToUInt32((dtSource.Rows[n][6].ToString().Split('-'))[1]);
                if(tipo.Equals("OP")){
                    prefOT = DBFunctions.SingleData("SELECT	pdoc_codigo	FROM MPEDIDOPRODUCCIONTRANSFERENCIA WHERE	pped_codigo='" + prefPed + "' AND mped_numero=" + numPed.ToString() + "");
                    numOT = DBFunctions.SingleData("SELECT mord_numeorde FROM MPEDIDOPRODUCCIONTRANSFERENCIA WHERE pped_codigo='" + prefPed + "' AND mped_numero=" + numPed.ToString() + "");
                    cargo = "I";
                }
				else{
                    prefOT = DBFunctions.SingleData("SELECT	pdoc_codigo	FROM mpedidotransferencia WHERE	pped_codigo='"+prefPed+"' AND mped_numero="+numPed.ToString()+"");
				    numOT =	DBFunctions.SingleData("SELECT mord_numeorde FROM mpedidotransferencia WHERE pped_codigo='"+prefPed+"' AND mped_numero="+numPed.ToString()+"");
				    cargo =	DBFunctions.SingleData("SELECT tcar_cargo FROM mpedidotransferencia WHERE pped_codigo='"+prefPed+"'	AND	mped_numero="+numPed.ToString()+"");
                }
				cantDev	= 0;
				valP = valU;
				costoF+=valP*cant;
				Mov.InsertaFila(codI,cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA,prefPed,numPed);
				sqlUpdateDPedidoItem.Add("UPDATE DPEDIDOITEM SET dped_cantfact=dped_cantfact+"+cant.ToString()+", pdoc_codigo='"+coddocumentof+"', mfac_numedocu="+numdocumentof+" where mped_clasregi='C' AND MNIT_NIT='"+nit+"' and pped_codigo='"+prefPed+"'	and	mped_numepedi="+numPed.ToString()+"	and	mite_codigo='"+codI+"'");
				if(tp == "TT" && Ordenes.BinarySearch(prefOT+"-"+numOT)==-1)
				{
					if(DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+prefOT+"' ")=="OT")
						sqlUpdateDPedidoItem.Add("INSERT INTO mordentransferencia VALUES('"+prefOT+"',"+numOT+",'"+cargo+"','"+coddocumentof+"',"+numdocumentof+")");
					else
						sqlUpdateDPedidoItem.Add("INSERT INTO mordenPRODUCCIONtransferencia VALUES('"+prefOT+"',"+numOT+",'"+coddocumentof+"',"+numdocumentof+")");
                    Ordenes.Add(prefOT + "-" + numOT);				
				}
			}
			//*****************************************************************************************************
			//Se crea un objeto	de facturacion y se	extrae los sql relacionados	con	la grabacion
			FacturaCliente facturaRepuestos	= new FacturaCliente("FRC",	coddocumentof, nit,	almacen, "F", numdocumentof,
				Convert.ToUInt32(diasplazo), fecha,	dv,	dv,	totalsiniva, totalconiva - totalsiniva,
				valorfletes,ivafletes,0,costoF,centrocosto,
				observacion,vendedor,HttpContext.Current.User.Identity.Name,kit);
			facturaRepuestos.GrabarFacturaCliente(false);
			for(int	n=0;n<facturaRepuestos.SqlStrings.Count;n++)
				sqlStrings.Add(facturaRepuestos.SqlStrings[n].ToString());
			numdocumentof =	facturaRepuestos.NumeroFactura;
			//*****************************************************************************************************
			for(int	n=0;n<sqlUpdateDPedidoItem.Count;n++)
				sqlStrings.Add(sqlUpdateDPedidoItem[n].ToString());
			Mov.RealizarMov(false);
			for(int	n=0;n<Mov.SqlStrings.Count;n++)
				sqlStrings.Add(Mov.SqlStrings[n].ToString());
			if(DBFunctions.Transaction(sqlStrings))
				processMsg +="<br>Bien Parte 1:<br>"+ DBFunctions.exceptions;
			else
			{
				status = false;
				processMsg +="<br>ERROR	Parte 1:<br>"+ DBFunctions.exceptions;
			}
			return status;
		}

        public bool ActualizarPed(bool guardar)
        { 
            ArrayList sqlStrings = new ArrayList();
            ArrayList arValU = new ArrayList();
            ArrayList arDesc = new ArrayList();
            ArrayList arIVA = new ArrayList();
            ArrayList cantsAsig = new ArrayList();
            ArrayList cantsPed = new ArrayList();
            int numR=0;
            double numUniPed = 0, numUniAsig = 0;
            int nPedPleno = 0, nPedParcial = 0;
            bool validaM = false, status = false;

            // SE AGRUPAN EN EL METODO datosfijos(); hac Mar-7-2018
            //string AnoA	= DBFunctions.SingleData("SELECT pano_ano from cinventario");//Año Vigente Contable
            //string MesA	= DBFunctions.SingleData("SELECT pmes_mes from cinventario");//Mes Vigente Contable
            //string tp =	DBFunctions.SingleData("SELECT TPED_CODIGO FROM	ppedido	where pped_codigo='"+coddocumento+"'").Trim(); //Tipo de Pedido
            //string bko = DBFunctions.SingleData("SELECT	tped_backorder from	tpedido	where tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='"+coddocumento+"')");//BackOrder?
            //string dem = DBFunctions.SingleData("SELECT	tped_demanda from tpedido where	tped_codigo=(SELECT	tped_codigo	FROM ppedido WHERE pped_codigo='"+coddocumento+"')");//Demanda?

            datosFijos();
            string claseregIns=clasereg;
			if(claseregIns=="M")claseregIns="C";
            sqlStrings.Add("delete from	DPEDIDOITEM	where PPED_CODIGO='" + coddocumento + "' AND MPED_NUMEPEDI=" + numpedido.ToString() + ";");
            AgregarItems(sqlStrings, ref numR, ref numUniPed, ref numUniAsig, ref nPedPleno, ref nPedParcial, arValU, arDesc, arIVA, cantsAsig, cantsPed, validaM, tp, bko, dem, AnoA, MesA, claseregIns);
            if (guardar)
            {
                if (DBFunctions.Transaction(sqlStrings))
                {
                    processMsg += "<br>Bien <br>" + DBFunctions.exceptions;
                    status = true;
                }
                else
                {
                    processMsg += "<br>ERROR<br>" + DBFunctions.exceptions;
                    status = false;
                }
            }
            return(status);
        }


        public bool	RealizarPed(ref	bool status, bool guardar)
		{
			bool PedidoR = false; //Dice si	se pudo	realizar algun pedido parcial o	completo
			string FechaProc = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.ffffff");
			string claseregIns=clasereg;
			if(claseregIns=="M")
                claseregIns ="C";


			//MPEDIDOITEM
			while(DBFunctions.RecordExist("SELECT *	FROM mpedidoitem WHERE pped_codigo='"+coddocumento+"' AND mped_numepedi="+numpedido+""))
				numpedido += 1;
			
			sqlStrings.Add("insert into	MPEDIDOITEM	(MPED_CLASEREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MPED_PEDIDO,MPED_CREACION,MPED_OBSERVACION,PALM_ALMACEN,SUSU_CODIGO,PVEN_CODIGO)	values ('"+claseregIns+"','"+nit+"','"+coddocumento+"',"+numpedido.ToString()+",'"+fecha.ToString("yyyy-MM-dd")+"','"+FechaProc+"','"+observacion+"','"+almacen+"','"+HttpContext.Current.User.Identity.Name+"','"+vendedor+"')");
			
			string ItemC = "";
			int	n=0, nPedPleno=0, nPedParcial=0, numR=0;
			double numUniPed=0, numUniAsig=0;
			
			ArrayList cantsPed = new ArrayList();
			ArrayList cantsAsig	= new ArrayList();
			ArrayList arDesc = new ArrayList();
			ArrayList arIVA	= new ArrayList();
			ArrayList arValU = new ArrayList();

            // SE AGRUPAN EN EL METODO datosfijos(); hac Mar-7-2018
            //string AnoA	= DBFunctions.SingleData("SELECT pano_ano from cinventario");//Año Vigente Contable
            //string MesA	= DBFunctions.SingleData("SELECT pmes_mes from cinventario");//Mes Vigente Contable
            //string tp =	DBFunctions.SingleData("SELECT TPED_CODIGO FROM	ppedido	where pped_codigo='"+coddocumento+"'").Trim(); //Tipo de Pedido
            //string bko = DBFunctions.SingleData("SELECT	tped_backorder from	tpedido	where tped_codigo='"+tp+"'");//BackOrder?
            //string dem = DBFunctions.SingleData("SELECT	tped_demanda from tpedido where	tped_codigo='"+tp+"'");//Demanda?

            datosFijos();

			bool validaM=false;

			//Si es	una	transferencia a	taller se debe crear el	registro que une las tablas	de MPEDIDOTRANSFERENCIA, MPEDIDOITEM y MORDEN, ademas debemos realizar los movimientos de kardex indicados
            if (tp == "T")
			{
				if(DBFunctions.SingleData("SELECT TDOC_TIPODOCU FROM PDOCUMENTO where pDOC_codigo='"+tipoorden+"'") == "OT")
					sqlStrings.Add("insert into	MPEDIDOTRANSFERENCIA (PDOC_CODIGO,MORD_NUMEORDE,TCAR_CARGO,PPED_CODIGO,MPED_NUMERO)	VALUES('"+tipoorden+"',"+numorden.ToString()+",'"+cargoorden+"','"+coddocumento+"',"+numpedido.ToString()+")");
				else
					sqlStrings.Add("insert into	MPEDIDOPRODUCCIONTRANSFERENCIA (PDOC_CODIGO,MORD_NUMEORDE,PPED_CODIGO,MPED_NUMERO)	VALUES('"+tipoorden+"',"+numorden.ToString()+",'"+coddocumento+"',"+numpedido.ToString()+")");
			}
			
			//Autorizar pedido Mayor?
			if(clasereg	== "M")
			{
				//COMENTAR AUTORIZACION AUTOMATICA
				double saldoMora=Utilidades.Clientes.ConsultarSaldoMora(nit);
				double saldo=Utilidades.Clientes.ConsultarSaldo(nit);
				double cupo=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(MCLI_CUPOCRED,0) FROM mcliente WHERE mnit_nit='"+nit+"'"));
				validaM=((saldo<=cupo && saldoMora<=0 && cupo>0)||(DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA")== "901087944")); //EUROTECK si maneja todos los pedidos como mayor
				
				//Pedir autorizacion para todas
				//validaM=false;
				
				//validaM=(cupo>0);
			}

            AgregarItems(sqlStrings, ref numR, ref numUniPed, ref numUniAsig, ref nPedPleno, ref nPedParcial, arValU, arDesc, arIVA, cantsAsig, cantsPed, validaM, tp, bko, dem, AnoA, MesA, claseregIns);

			if(tp!="C")
			{
				if(clasereg	!= "P")
				{
					////*********************************************************************************////
					//Se llena el registro correspondiente a fillrate ????????????????
					//MFILLRATE
					if(!DBFunctions.RecordExist("SELECT	* FROM MFILLRATE WHERE MNIT_NIT='"+nit+"' AND MPED_NUMEPEDI="+numpedido.ToString()))
						sqlStrings.Add("insert into	MFILLRATE (MNIT_NIT,MPED_NUMEPEDI,MFILL_RENGPEDIDOS,MFILL_RENGSATISFTOT,MFILL_RENGSATISFPAR,MFILL_UNIDPEDIDAS,MFILL_UNIDASIGNADAS) values ('"+nit+"',"+numpedido.ToString()+","+numR.ToString()+","+nPedPleno.ToString()+","+nPedParcial.ToString()+","+numUniPed.ToString()+","+numUniAsig.ToString()+")");
					
					//																																											MNIT_NIT,MPED_NUMEPEDI,RENGPEDIDOS,				RENGSATISFTOT,		   SATISFPAR				   MFILL_UNIDPEDIDAS,		MFILL_UNIDASIGNADAS
					//LISTAEMPAQUE
					if(nPedParcial>0||nPedPleno>0)
					{
						PedidoR	= true;
						int	numLis=0;

						if(clasereg=="C" || validaM)
						{
							numLis = Convert.ToInt32(DBFunctions.SingleData("SELECT	CASE WHEN MAX(MLIS_NUMERO)IS NULL THEN 0 ELSE MAX(MLIS_NUMERO) END FROM	DBXSCHEMA.MLISTAEMPAQUE"))+1;
							numerolista	= numLis.ToString();

                            //MLISTAEMPAQUE
                            if (kit == "Seleccione..")
                                kit = null;
							sqlStrings.Add("insert into	MLISTAEMPAQUE (MLIS_NUMERO,MNIT_NIT,MLIS_FECHPROC,PALM_ALMACEN,SUSU_USUARIO, pkit_codigo) values	("+numLis+",'"+nit+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+almacen+"','"+HttpContext.Current.User.Identity.Name+"','"+kit+"') ") ;
							//																										MLIS_NUMERO, MNIT_NIT,		LIS_FECHPROC,							 PALM_ALMACEN,					  SUSU_USUARIO
						}

						string vp;
						int	c=0;
						
						for(n=0;n<dtSource.Rows.Count;n++)
						{
							double cantAsig=Convert.ToDouble(cantsAsig[n]);
							double cantPedi=Convert.ToDouble(cantsPed[n]);

							if(cantAsig>0)
							{
								ItemC=dtSource.Rows[n][0].ToString();
								
								vp=DBFunctions.SingleData("SELECT msal_costprom	FROM MSALDOITEM	WHERE MITE_CODIGO='"+ItemC+"' and pano_ano="+AnoA);
                                if (vp == "")
                                    vp = "0";
								if(clasereg	== "C" || validaM)
									sqlStrings.Add(
                                        "insert into	DLISTAEMPAQUE (MLIS_NUMERO,MITE_CODIGO,MPED_CLASEREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,DPED_CANTASIG,DLIS_PORCDESC,PIVA_PORCIVA,DLIS_VALOPUBL,DLIS_VALOUNIT) values ("+
                                        numLis + ",'" +             //MLIS_NUMERO
                                        ItemC + "','" +            //MITE_CODIGO
                                        claseregIns + "','" +      //CLASEREGI
                                        nit + "','" +              //NIT
                                        coddocumento + "'," +      //PEDICOD
                                        numpedido.ToString() + "," +   //NUMEPEDI
                                        cantAsig.ToString() + "," +    //DPED_CANTASIG
                                        arDesc[n].ToString() + "," +       //DLIS_PORCDESC
                                        arIVA[n].ToString() + "," +        //PIVA_PORCIVA
                                        arValU[n].ToString() + "," +       //VALOPUBL
                                        vp + ")");  // VALOUNIT

								//Genera Demanda?
								if(dem=="S")
								{
									if(!DBFunctions.RecordExist("SELECT	* FROM MDEMANDAITEMALMACEN WHERE mite_codigo='"+ItemC+"' AND pano_ano="+fecha.Year.ToString()+"	and	pmes_mes="+fecha.Month.ToString()+"	and	palm_almacen='"+almacen+"'"))
										sqlStrings.Add("insert into	MDEMANDAITEMALMACEN	(MITE_CODIGO,pano_ano,pmes_mes,palm_almacen,mdem_cantidad) values ('"+ItemC+"',"+fecha.Year.ToString()+","+fecha.Month.ToString()+",'"+almacen+"',"+cantPedi.ToString()+")");
									else
										sqlStrings.Add("update MDEMANDAITEMALMACEN set mdem_cantidad=mdem_cantidad+"+cantPedi.ToString()+" WHERE mite_codigo='"+ItemC+"' AND pano_ano="+fecha.Year.ToString()+"	and	pmes_mes="+fecha.Month.ToString()+"	and	palm_almacen='"+almacen+"'");
									if(!DBFunctions.RecordExist("SELECT	* FROM MDEMANDAITEM	WHERE mite_codigo='"+ItemC+"' AND pano_ano="+fecha.Year.ToString()+" and pmes_mes="+fecha.Month.ToString()+""))
										sqlStrings.Add("insert into	MDEMANDAITEM (MITE_CODIGO,pano_ano,pmes_mes,mdem_cantidad) values ('"+ItemC+"',"+fecha.Year.ToString()+","+fecha.Month.ToString()+","+cantPedi.ToString()+")");
									else
										sqlStrings.Add("update MDEMANDAITEM	set	mdem_cantidad=mdem_cantidad+"+cantPedi.ToString()+"	WHERE mite_codigo='"+ItemC+"' AND pano_ano="+fecha.Year.ToString()+" and pmes_mes="+fecha.Month.ToString()+"");
								}
								c++;
							}
						}
					}
					////*********************************************************************************////
				}
			}

            //Insertar Autorizacion
            if (clasereg == "M")
            {
                if (validaM)
                    sqlStrings.Add("INSERT INTO MPEDIDOCLIENTEAUTORIZACION VALUES(" +
                        "'" + coddocumento + "'," + numpedido.ToString() + ",'" + nit + "','S'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + HttpContext.Current.User.Identity.Name.ToLower() + "');");
                else
                    sqlStrings.Add("INSERT INTO MPEDIDOCLIENTEAUTORIZACION VALUES(" +
                        "'" + coddocumento + "'," + numpedido.ToString() + ",'" + nit + "',NULL," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'" + HttpContext.Current.User.Identity.Name.ToLower() + "');");
            }

            //DOCUMENTO
            uint ultiPed = Convert.ToUInt32(DBFunctions.SingleData("SELECT pped_ultipedi FROM ppedido WHERE	pped_codigo	= '"+coddocumento+"'"));
			
			if(numpedido>ultiPed)
				sqlStrings.Add("UPDATE ppedido set pped_ultipedi="+numpedido+" WHERE pped_codigo = '"+coddocumento+"'");
			
			if(guardar)
			{
				if(DBFunctions.Transaction(sqlStrings))
				{
					processMsg +="<br>Bien <br>"+ DBFunctions.exceptions;
				}
				else
				{
					status = false;
					processMsg +="<br>ERROR<br>"+ DBFunctions.exceptions;
				}
			}

			return (PedidoR && status);
		}
        private void AgregarItems(ArrayList sqlStrings, ref int numR, ref double numUniPed, ref double numUniAsig, ref int nPedPleno, ref int nPedParcial, ArrayList arValU, ArrayList arDesc, ArrayList arIVA, ArrayList cantsAsig, ArrayList cantsPed, bool validaM, string tp, string bko, string dem, string AnoA, string MesA, string claseregIns)
        {
            string ItemC = "";
            double CantPed, IValU, IDesc, IIva;
            for (int n = 0; n < dtSource.Rows.Count; n++)
            {
                numR++;
                ItemC = dtSource.Rows[n][0].ToString();	//Codigo del Item 
                CantPed = Convert.ToDouble(dtSource.Rows[n][5]); //Cantidad	Pedida 
                numUniPed += CantPed; // Numero	de unidades	pedidas	en total
                IValU = Convert.ToDouble(dtSource.Rows[n][2]);//Valor del item Unitario
                arValU.Add(IValU);//Arreglo	de Valores Unitarios
                IIva = Convert.ToDouble(dtSource.Rows[n][3]);//Valor del Iva para	este Item
                arIVA.Add(IIva);//Arreglo de Valores de	IVA
                IDesc = Convert.ToDouble(dtSource.Rows[n][4]);//Porcentaje de Descuento	para este Item
                arDesc.Add(IDesc);//Arreglo	de Porcentajes de descuento
                double ca = 0;

                if (clasereg != "P")
                {
                    if (clasereg == "M" && !validaM)
                        ca = 0;
                    else
                        ca = Referencias.ConsultarAsignacion(ItemC, almacen, CantPed);

                    if (ca == CantPed)
                    {
                        nPedPleno++;
                        numUniAsig += ca;
                    }
                    else if (ca < CantPed && ca > 0)
                    {
                        nPedParcial++;
                        numUniAsig += ca;
                    }
                }
                else if (clasereg == "P")
                        ca = CantPed;

                cantsAsig.Add(ca);//Arreglo	de cantidades asignadas
                cantsPed.Add(CantPed);//Arreglo	de cantidades pedidas

                if (clasereg != "P")//DPEDIDOITEM
                    sqlStrings.Add("insert into	DPEDIDOITEM	(MPED_CLASREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MITE_CODIGO,DPED_CANTPEDI,DPED_CANTASIG,DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,PIVA_PORCIVA) values ('" + claseregIns + "','" + nit + "','" + coddocumento + "'," + numpedido.ToString() + ",'" + ItemC + "'," + CantPed.ToString() + "," + ca.ToString() + ",0," + IValU.ToString() + "," + IDesc.ToString() + "," + IIva.ToString() + ")");
                //																																															CREGI,	NIT,				CODIGO DOCUMENTO	,MPED_NUMEPEDI,	  MITE_CODIGO,	  DPED_CANTPEDI,	  DPED_CANTASIG,  DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,		PIVA_PORCIVA
                else
                    sqlStrings.Add("insert into	DPEDIDOITEM	(MPED_CLASREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MITE_CODIGO,DPED_CANTPEDI,DPED_CANTASIG,DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,PIVA_PORCIVA) values ('" + claseregIns + "','" + nit + "','" + coddocumento + "'," + numpedido.ToString() + ",'" + ItemC + "'," + CantPed.ToString() + ",0,0," + IValU.ToString() + "," + IDesc.ToString() + "," + IIva.ToString() + ")");
                //																																																CREGI,			NIT, CODIGO	DOCUMENTO	 ,MPED_NUMEPEDI,		 MITE_CODIGO,  DPED_CANTPEDI,CANTASIG,PED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,		PIVA_PORCIVA

                if (tp != "C" && DBFunctions.SingleData("SELECT TORI_CODIGO FROM MITEMS WHERE MITE_CODIGO = '" + ItemC + "' ") != "Z") // es una Cotizacion o es item de servicio, No se graban los acumaulados, solo el kardex
                {
                    if (!DBFunctions.RecordExist("SELECT	* FROM msaldoitem WHERE	mite_codigo	= '" + ItemC + "' AND PANO_ANO=" + AnoA))
                        sqlStrings.Add("insert into	msaldoitem (mite_codigo,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_costhist,msal_costhisthist,msal_cantinveinic,msal_ultivent,msal_ultiingr,msal_ulticost,msal_ultiprov,msal_abcd,msal_peditrans,msal_unidtrans,msal_pedipendi,msal_unidpendi)	values ('" + ItemC + "'," + AnoA + ",0,0,0,0,0,0,0,null,null,0,null,null,0,0,0,0)");

                    if (!DBFunctions.RecordExist("SELECT	* FROM msaldoitemalmacen WHERE mite_codigo = '" + ItemC + "' AND PALM_ALMACEN='" + almacen + "'	and	pano_ano=" + AnoA))
                        sqlStrings.Add("insert into	msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist)	values ('" + ItemC + "','" + almacen + "'," + AnoA + ",0,0,0,0)");

                    //if (!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEMALMACEN WHERE pano_ano=" + AnoA + " and pmes_mes=" + MesA + " and tmov_tipomovi=70	AND	mite_codigo	= '" + ItemC + "' AND PALM_ALMACEN='" + almacen + "'"))
                    //    sqlStrings.Add("insert into	MACUMULADOITEMALMACEN (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,PALM_ALMACEN,macu_cantidad,macu_costo,macu_precio) values ('" + ItemC + "',70," + AnoA + "," + MesA + ",'" + almacen + "',0,0,0)");

                    //if (!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEM WHERE	pano_ano=" + AnoA + " and pmes_mes=" + MesA + "	and	tmov_tipomovi=70 AND mite_codigo = '" + ItemC + "'"))
                    //    sqlStrings.Add("insert into	MACUMULADOITEM (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,macu_cantidad,macu_costo,macu_precio) values ('" + ItemC + "',70," + AnoA + "," + MesA + ",0,0,0)");

                    if (clasereg != "P")
                    {
                        sqlStrings.Add("update msaldoitem set msal_cantasig=msal_cantasig+" + ca.ToString() + "	where mite_codigo='" + ItemC + "' and pano_ano=" + AnoA);
                        sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantasig=msal_cantasig+" + ca.ToString() + " WHERE mite_codigo='" + ItemC + "' AND pano_ano=" + AnoA + " AND palm_almacen='" + almacen + "'");

                        if (bko == "S" && (CantPed) > 0)//Genera backorder
                        {
                            sqlStrings.Add("update msaldoitemalmacen set msal_cantpendiente=msal_cantpendiente+" + (CantPed).ToString() + " where mite_codigo='" + ItemC + "' and pano_ano=" + AnoA + " and palm_almacen='" + almacen + "'");
                            sqlStrings.Add("update msaldoitem set msal_unidpendi=msal_unidpendi+" + (CantPed).ToString() + ", msal_pedipendi=msal_pedipendi+1 where mite_codigo='" + ItemC + "' and pano_ano=" + AnoA);
                        }

                        if (dem == "S")
                        {	//Genera demanda
                            sqlStrings.Add("update macumuladoitemalmacen set macu_cantidad=macu_cantidad+" + CantPed.ToString() + "	WHERE pano_ano=" + AnoA + "	and	pmes_mes=" + MesA + " and tmov_tipomovi=70 AND mite_codigo = '" + ItemC + "' AND PALM_ALMACEN='" + almacen + "'");
                            sqlStrings.Add("update macumuladoitem set macu_cantidad=macu_cantidad+" + CantPed.ToString() + " WHERE pano_ano=" + AnoA + " and pmes_mes=" + MesA + " and tmov_tipomovi=70	AND	mite_codigo	= '" + ItemC + "'");
                        }
                    }
                    else if (clasereg == "P")
                    {
                        sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=msal_unidtrans+" + ca.ToString() + ", msal_peditrans=msal_peditrans+1 WHERE mite_codigo='" + ItemC + "' and pano_ano=" + AnoA);
                        sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito+" + ca.ToString() + " WHERE mite_codigo='" + ItemC + "' AND pano_ano=" + AnoA + " AND palm_almacen='" + almacen + "'");
                    }
                }
                else
                    if (dem == "S")
                    {	//Genera demanda en las cotizaciones
                        sqlStrings.Add("update macumuladoitemalmacen set macu_cantidad=macu_cantidad+" + CantPed.ToString() + "	WHERE pano_ano=" + AnoA + "	and	pmes_mes=" + MesA + " and tmov_tipomovi=70 AND mite_codigo = '" + ItemC + "' AND PALM_ALMACEN='" + almacen + "'");
                        sqlStrings.Add("update macumuladoitem set macu_cantidad=macu_cantidad+" + CantPed.ToString() + " WHERE pano_ano=" + AnoA + " and pmes_mes=" + MesA + " and tmov_tipomovi=70	AND	mite_codigo	= '" + ItemC + "'");
                    }
            }
        }
	}
}
