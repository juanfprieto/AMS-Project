using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;
using AMS.Documentos;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Contabilidad;


namespace AMS.Inventarios
{
	/// <summary>
	/// Clase que maneja los roles de los inventarios fisicos
	/// </summary>
	public class RolesInventarioFisico
	{
		#region Atributos
		string codigoRol, nitPersona;
		int codigoGrupoRelacionado,codigoSecuencia;
       	#endregion

		#region Propiedades
		public string CodigoRol {get{return codigoRol;}set{codigoRol=value;}}
		public string NitPersona {get{return nitPersona;}set{nitPersona=value;}}
		public int CodigoGrupoRelacionado {get{return codigoGrupoRelacionado;}set{codigoGrupoRelacionado=value;}}
		public int CodigoSecuencia {get{return codigoSecuencia;}set{codigoSecuencia=value;}}
		#endregion

		#region Constructor
		public RolesInventarioFisico(string codigoRol, string nitPersona)
		{
			this.codigoRol = codigoRol;
			this.nitPersona = nitPersona;
			this.codigoGrupoRelacionado = -1;
			this.codigoSecuencia = -1;
		}

		public RolesInventarioFisico(string codigoRol, string nitPersona, int codigoGrupoRelacionado)
		{
			this.codigoRol = codigoRol;
			this.nitPersona = nitPersona;
			this.codigoGrupoRelacionado = codigoGrupoRelacionado;
			this.codigoSecuencia = -1;
		}

		public RolesInventarioFisico(int codigoSecuencia, string nitPersona, string codigoRol, int codigoGrupoRelacionado)
		{
			this.codigoSecuencia = codigoSecuencia;
			this.nitPersona = nitPersona;
			this.codigoRol = codigoRol;
			this.codigoGrupoRelacionado = codigoGrupoRelacionado;
		}
		#endregion
	}

	/// <summary>
	/// Clase que permite la creacion de colecciones de Roles de Inventarios
	/// </summary>
	public class CollectionRolesInventario : CollectionBase
	{
		#region Sobrecarga Metodos Heredados
		public virtual void Add(RolesInventarioFisico NewRolesInventarioFisico)
		{
			this.List.Add(NewRolesInventarioFisico);
		}

		public virtual RolesInventarioFisico this[int Index]
		{
			get{return (RolesInventarioFisico)this.List[Index];}
		}
		#endregion
	}

	/// <summary>
	/// Clase que permite el manejo de las opciones relacionadas con los items de un inventario fisico
	/// </summary>
	public class ItemsInventarioFisico
	{
		#region Constantes
		private const string sqlDInventarioFisico = @"SELECT 
	DINV.dinv_mite_codigo,
	DINV.dinv_mite_nombre,
	DINV.dinv_conteoactual,
	DINV.dinv_tarjeta,
	DINV.dinv_pubi_codigo,
	DINV.dinv_palm_almacen,
	DINV.dinv_msal_cantactual,
	DINV.dinv_costprom,
	DINV.dinv_conteo1,
	DINV.dinv_conteo2,
	DINV.dinv_conteo3,
	DINV.dinv_fechconteo,
	DINV.dinv_fechconteofin,
	DINV.dinv_ubicacion,
	DINV.dinv_stand,
	COALESCE(DINV.dinv_diferencia,0),
	DBXSCHEMA.EDITARREFERENCIAS(DINV.dinv_mite_codigo,PLIN.plin_tipo) as referencia_editada,
	DINV.dinv_costdiferencia,
	DINV.dinv_contdefinitivo
FROM 
	dinventariofisico DINV 
	INNER JOIN 
	mitems MIT 
	ON 
		MIT.mite_codigo = DINV.dinv_mite_codigo 
	INNER JOIN 
	plineaitem PLIN 
	ON 
		MIT.plin_codigo = PLIN.plin_codigo 
WHERE 
	DINV.dinv_tarjeta = {0} AND 
	DINV.pdoc_codigo = '{1}' AND 
	DINV.minf_numeroinv = {2}";
		#endregion

		#region Atributos
		private int conteoActual, numeroTarjeta, codigoUbicacionInicial, conteo1, conteo2, conteo3, diferenciaConteo, conteoDefinitivo;
		private string codigoItemRelacionado, nombreItemRelacionado, codigoAlmacen, descripcionUbicacion, nombreStand, codigoItemModificado;
		private double saldoActual,costoPromedio,costoDiferencia;
		private DateTime fechaInicioConteo, fechaFinConteo;
		#endregion

		#region Propiedades
		public int ConteoActual{get{return conteoActual;}set{conteoActual=value;}}
		public int NumeroTarjeta{get{return numeroTarjeta;}set{numeroTarjeta=value;}}
		public int CodigoUbicacionInicial{get{return codigoUbicacionInicial;}set{codigoUbicacionInicial=value;}}
		public int Conteo1{get{return conteo1;}set{conteo1=value;}}
		public int Conteo2{get{return conteo2;}set{conteo2=value;}}
		public int Conteo3{get{return conteo3;}set{conteo3=value;}}
		public int ConteoDefinitivo{get{return ObtenerConteoDefinitivo();}}
		public string CodigoItemRelacionado{get{return codigoItemRelacionado;}set{codigoItemRelacionado=value;}}
		public string NombreItemRelacionado{get{return nombreItemRelacionado;}set{nombreItemRelacionado=value;}}
		public string CodigoAlmacen{get{return codigoAlmacen;}set{codigoAlmacen=value;}}
		public string DescripcionUbicacion{get{return descripcionUbicacion;}set{descripcionUbicacion=value;}}
		public string NombreStand{get{return nombreStand;}set{nombreStand=value;}}
		public string CodigoItemModificado{get{return codigoItemModificado;}set{codigoItemModificado=value;}}
		public double SaldoActual{get{return saldoActual;}set{saldoActual=value;}}
		public double CostoPromedio{get{return costoPromedio;}set{costoPromedio=value;}}
		public object DiferenciaConteo{get{return CalcularDiferenciaConteo();}}
		public object CostoDiferencia{get{return CalcularCostoDiferencia();}}
		public DateTime FechaInicioConteo{get{return fechaInicioConteo;}set{fechaInicioConteo=value;}}
		public DateTime FechaFinConteo{get{return fechaFinConteo;}set{fechaFinConteo=value;}}
		#endregion

		#region Métodos
	
		private void Inicializar()
		{
			this.conteoActual = -1;
			this.numeroTarjeta = -1;
			this.codigoUbicacionInicial = -1;
			this.conteo1 = -1;
			this.conteo2 = -1;
			this.conteo3 = -1;
			this.diferenciaConteo = -1;
			this.conteoDefinitivo = -1;
			this.codigoItemRelacionado = String.Empty;
			this.nombreItemRelacionado = String.Empty;
			this.codigoAlmacen = String.Empty;
			this.descripcionUbicacion = String.Empty;
			this.nombreStand = String.Empty;
			this.saldoActual = 0;
			this.costoPromedio = 0;
			this.costoDiferencia = -1;
			this.fechaInicioConteo = Convert.ToDateTime(null);
			this.fechaFinConteo = Convert.ToDateTime(null);
		}

		private object CalcularDiferenciaConteo()
		{
			object diferencia = null;

			switch(this.ConteoActual)
			{
				case 1:
					if(this.Conteo1 == Convert.ToInt32(this.SaldoActual))
						diferencia = (int)0;
					break;
				case 2:
					if(this.Conteo2 == this.Conteo1)
						diferencia = this.Conteo1 - Convert.ToInt32(this.SaldoActual);
                    else
                        if (this.Conteo2 == Convert.ToInt32(this.SaldoActual))
                            diferencia = this.Conteo2 - Convert.ToInt32(this.SaldoActual);
                    break;
				case 3:
					diferencia = this.Conteo3 - Convert.ToInt32(this.SaldoActual);
					break;
			}

			return diferencia;
		}

		private object CalcularCostoDiferencia()
		{
			object costoDiferencia = null;

			object diferenciaConteo = this.CalcularDiferenciaConteo();

			if(diferenciaConteo != null)
				costoDiferencia = Convert.ToInt32(diferenciaConteo) * this.CostoPromedio;

			return costoDiferencia;
		}

		private int ObtenerConteoDefinitivo()
		{
			int conteoDefinitivo = -1;

			switch(this.ConteoActual)
			{
				case 1:
					if(this.Conteo1 == Convert.ToInt32(this.SaldoActual))
						conteoDefinitivo = this.Conteo1;
					break;
				case 2:
					if(this.Conteo2 == this.Conteo1)
						conteoDefinitivo = this.Conteo2;
					break;
				case 3:
					conteoDefinitivo = this.Conteo3;
					break;
			}

			return conteoDefinitivo;			
		}

		#endregion

		#region Constructor

		public ItemsInventarioFisico()
		{
			this.Inicializar();
		}
		
		public ItemsInventarioFisico(
			int codigoUbicacionInicial, 
			string codigoItemRelacionado, 
			string nombreItemRelacionado, 
			string codigoAlmacen, 
			double saldoActual, 
			double costoPromedio, 
			string descripcionUbicacion, 
			string nombreStand)
		{
			this.conteoActual = 0;
			this.numeroTarjeta = -1;
			this.conteo1 = -1;
			this.conteo2 = -1;
			this.conteo3 = -1;
			this.diferenciaConteo = -1;
			this.conteoDefinitivo = -1;
			this.codigoAlmacen = codigoAlmacen;

			if(DBFunctions.RecordExist("SELECT mite_codigo FROM mitems WHERE mite_codigo='"+codigoItemRelacionado+"'"))
			{
				this.codigoUbicacionInicial = codigoUbicacionInicial;
				this.codigoItemRelacionado = codigoItemRelacionado;
			}
			else
			{
				this.codigoUbicacionInicial = -1;
				this.codigoItemRelacionado = String.Empty;
				this.conteoActual = -1;
			}
			
			this.nombreItemRelacionado = nombreItemRelacionado;
			this.descripcionUbicacion = descripcionUbicacion;
			this.nombreStand = nombreStand;
			this.saldoActual = saldoActual;
			this.costoPromedio = costoPromedio;
			this.costoDiferencia = -1;
			this.fechaInicioConteo = Convert.ToDateTime(null);
			this.fechaFinConteo = Convert.ToDateTime(null);
		}

		public ItemsInventarioFisico(
			string codigoItemRelacionado, 
			string nombreItemRelacionado, 
			int conteoActual, 
			int numeroTarjeta, 
			int codigoUbicacionInicial,
			string codigoAlmacen, 
			double saldoActual, 
			double costoPromedio, 
			int conteo1, 
			int conteo2, 
			int conteo3, 
			DateTime fechaInicioConteo, 
			DateTime fechaFinConteo,
			string descripcionUbicacion, 
			string nombreStand, 
			int diferenciaConteo,
			double costoDiferencia,
			int conteoDefinitivo,
			string codigoItemModificado)
		{
			this.codigoItemRelacionado = codigoItemRelacionado;
			this.nombreItemRelacionado = nombreItemRelacionado;
			this.conteoActual = conteoActual;
			this.numeroTarjeta = numeroTarjeta;
			this.codigoUbicacionInicial = codigoUbicacionInicial;
			this.codigoAlmacen = codigoAlmacen;
			this.saldoActual = saldoActual;
			this.costoPromedio = costoPromedio;
			this.conteo1 = conteo1;
			this.conteo2 = conteo2;
			this.conteo3 = conteo3;
			this.fechaInicioConteo = fechaInicioConteo;
			this.fechaFinConteo = fechaFinConteo;
			this.descripcionUbicacion = descripcionUbicacion;
			this.nombreStand = nombreStand;
			this.diferenciaConteo = diferenciaConteo;
			this.costoDiferencia = costoDiferencia;
			this.conteoDefinitivo = conteoDefinitivo;
			this.codigoItemModificado = codigoItemModificado;
		}

		public ItemsInventarioFisico(
			string prefijoInventario, 
			int numeroInventario,
			int numeroTarjeta)
		{
			DataSet dsConsulta = new DataSet();

			DBFunctions.Request(dsConsulta,IncludeSchema.NO,String.Format(sqlDInventarioFisico,numeroTarjeta,prefijoInventario,numeroInventario));
			
			if (dsConsulta.Tables.Count > 0)
			{
				if(dsConsulta.Tables[0].Rows.Count > 0)
				{
					this.codigoItemRelacionado = InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][0],typeof(string)).ToString();
					this.nombreItemRelacionado = InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][1],typeof(string)).ToString().Replace('&',' ').Replace("'"," ").Replace('-', ' ');
					this.conteoActual   = Convert.ToInt32(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][2],typeof(int)));
					this.numeroTarjeta  = Convert.ToInt32(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][3],typeof(int)));
					this.codigoUbicacionInicial = Convert.ToInt32(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][4],typeof(int)));
					this.codigoAlmacen  = InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][5],typeof(string)).ToString();
					this.saldoActual    = Convert.ToDouble(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][6],typeof(double)));
					this.costoPromedio  = Convert.ToDouble(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][7],typeof(double)));
					this.conteo1        = Convert.ToInt32(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][8],typeof(int)));
					this.conteo2        = Convert.ToInt32(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][9],typeof(int)));
					this.conteo3        = Convert.ToInt32(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][10],typeof(int)));
					this.FechaInicioConteo = Convert.ToDateTime(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][11],typeof(DateTime)));
					this.FechaFinConteo = Convert.ToDateTime(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][12],typeof(DateTime)));
					this.descripcionUbicacion = InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][13],typeof(string)).ToString().Replace('-', ' ');
					this.nombreStand    = InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][14],typeof(string)).ToString().Replace('-', ' ');
					this.diferenciaConteo = Convert.ToInt32(InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][15],typeof(int)));
					this.codigoItemModificado = InventarioFisico.ValidarDBNull(dsConsulta.Tables[0].Rows[0][16],typeof(string)).ToString();
				}
				else
					Inicializar();
			}
		}
		#endregion
	}

	public class CollectionItemsInventarioFisico : CollectionBase
	{
		#region Sobrecarga Metodos Heredados

		public virtual void Add(ItemsInventarioFisico NewItemsInventarioFisico)
		{
			this.List.Add(NewItemsInventarioFisico);
		}

		public virtual ItemsInventarioFisico this[int Index]
		{
			get{return (ItemsInventarioFisico)this.List[Index];}
		}

		#endregion
	}
	
	/// <summary>
	/// Clase que se encarga del manejo del inventario fisico como instancia.
	/// Se relaciona toda la información del inventario como roles, 
	/// </summary>
	public class InventarioFisico
	{
		#region Constantes

		const string cadenaNulo = "null";

		private const string sqlItesInventarioFisico = @"SELECT 
	DINV.dinv_mite_codigo, 
	DINV.dinv_mite_nombre, 
	DINV.dinv_conteoactual, 
	DINV.dinv_tarjeta, 
	DINV.dinv_pubi_codigo, 
	DINV.dinv_palm_almacen, 
	COALESCE(DINV.dinv_msal_cantactual,0) dinv_msal_cantactual, 
	DINV.dinv_costprom, 
	DINV.dinv_conteo1, 
	DINV.dinv_conteo2, 
	DINV.dinv_conteo3, 
	DINV.dinv_fechconteo, 
	DINV.dinv_fechconteofin, 
	DINV.dinv_ubicacion, 
	DINV.dinv_stand, 
	COALESCE(DINV.dinv_diferencia,0) as dinv_diferencia, 
	DINV.dinv_costdiferencia, 
	DINV.dinv_contdefinitivo, 
	DBXSCHEMA.EDITARREFERENCIAS(DINV.dinv_mite_codigo,PLIN.plin_tipo) as referencia_editada 
FROM 
	dinventariofisico DINV 
	INNER JOIN 
	mitems MIT 
	ON 
		MIT.mite_codigo = DINV.dinv_mite_codigo 
	INNER JOIN 
	plineaitem PLIN 
	ON 
		MIT.plin_codigo = PLIN.plin_codigo 
WHERE 
	DINV.pdoc_codigo = '{0}' AND 
	DINV.minf_numeroinv = {1} {2}
ORDER BY
	DINV.dinv_tarjeta";
		#endregion

		#region Atributos
		private string prefijoInventario, codigoTipoInventarioUbicacion, codigoTipoInventarioTipo, usuarioRelacionado, indicativoAjuste, processMsg;
		private int numeroInventario;
		private double costoContador;
		private DateTime fechaInicio, fechaTerminacion, fechaAjuste;
		private CollectionRolesInventario rolesEspecificos;
		private CollectionItemsInventarioFisico itemsInventario;
		private DataTable dtItemsInventarioFisico;
		#endregion

		#region Propiedades
		public string PrefijoInventario {get{return prefijoInventario;}set{prefijoInventario=value;}}
		public string CodigoTipoInventarioUbicacion {get{return codigoTipoInventarioUbicacion;}set{codigoTipoInventarioUbicacion=value;}}
		public string CodigoTipoInventarioTipo{get{return codigoTipoInventarioTipo;}set{codigoTipoInventarioTipo=value;}}
		public string UsuarioRelacionado{get{return usuarioRelacionado;}set{usuarioRelacionado=value;}}
		public string ProcessMsg{get{return processMsg;}set{processMsg=value;}}
		public bool IndicativoAjuste{get{bool retorno = true;if(indicativoAjuste == "N")retorno = false;return retorno;}set{if(value == true)indicativoAjuste="S";else if(value == true)indicativoAjuste="N";}}
		public int NumeroInventario{get{return numeroInventario;}set{numeroInventario=value;}}
		public ArrayList ConteosPendientesInstancia{get{return InventarioFisico.ConteosPendientes(this.PrefijoInventario,this.numeroInventario);}}
		public double CostoContador{get{return costoContador;}set{costoContador=value;}}
		public DateTime FechaInicio{get{return fechaInicio;}set{fechaInicio=value;}}
		public DateTime FechaTerminacion{get{return fechaTerminacion;}set{fechaTerminacion=value;}}
		public DateTime FechaAjuste{get{return fechaAjuste;}set{fechaAjuste=value;}}
		public CollectionRolesInventario RolesEspecificos{get{return rolesEspecificos;}set{rolesEspecificos=value;}}
		public CollectionItemsInventarioFisico ItemsInventario{get{return itemsInventario;}set{itemsInventario=value;}}
		public DataTable DtItemsInventarioFisico{get{return dtItemsInventarioFisico;}}
		public int NumeroTotalTarjetasAConteoInstancia{get{return InventarioFisico.NumeroTotalTarjetasAConteo(this.PrefijoInventario,this.NumeroInventario);}}
		public int NumeroTotalTarjetasAConteo1Instancia{get{return InventarioFisico.NumeroTotalTarjetasAConteo1(this.PrefijoInventario,this.NumeroInventario);}}
		public int NumeroTotalTarjetasAConteo2Instancia{get{return InventarioFisico.NumeroTotalTarjetasAConteo2(this.PrefijoInventario,this.NumeroInventario);}}
		public int NumeroTotalTarjetasAConteo3Instancia{get{return InventarioFisico.NumeroTotalTarjetasAConteo3(this.PrefijoInventario,this.NumeroInventario);}}
		public int NumeroTarjetasEnConteo1{get{return InventarioFisico.NumeroTarjetasEnConteoActual(this.PrefijoInventario,this.NumeroInventario,1);}}
		public int NumeroTarjetasEnConteo2{get{return InventarioFisico.NumeroTarjetasEnConteoActual(this.PrefijoInventario,this.NumeroInventario,2);}}
		public int NumeroTarjetasEnConteo3{get{return InventarioFisico.NumeroTarjetasEnConteoActual(this.PrefijoInventario,this.NumeroInventario,3);}}
		public int NumeroTotalTarjetasEnConteoDefinitivoInstancia{get{return InventarioFisico.NumeroTotalTarjetasEnConteoDefinitivo(this.PrefijoInventario,this.NumeroInventario);}}
		public int NumeroTotalTarjetasInstancia{get{return InventarioFisico.NumeroTotalTarjetas(this.PrefijoInventario,this.NumeroInventario);}}
		public DataTable TarjetasAConteo1{get{return ConsultaItemsInventarioFisico("AND dinv_conteoactual = 0");}}
		public DataTable TarjetasAConteo2{get{return ConsultaItemsInventarioFisico("AND dinv_conteoactual = 1 AND dinv_conteo1 <> dinv_msal_cantactual");}}
		public DataTable TarjetasAConteo3{get{return ConsultaItemsInventarioFisico("AND dinv_conteoactual = 2 AND dinv_conteo1 <> dinv_conteo2");}}
		public DataTable TarjetasAConteoDefinitivo{get{return ConsultaItemsInventarioFisico("AND dinv_contdefinitivo IS NOT NULL");}}
		public int numeroTarjetaActDesde,numeroTarjetaActHasta;
		#endregion
		
		#region Constructor

		public InventarioFisico(
			string prefijoInventario, 
			int numeroInventario, 
			DateTime fechaInicio, 
			string codigoTipoInventarioUbicacion, 
			string codigoTipoInventarioTipo, 
			double costoContador, 
			string usuarioRelacionado)
		{
			this.prefijoInventario = prefijoInventario;
			this.numeroInventario = numeroInventario;
			this.fechaInicio = fechaInicio;
			this.fechaTerminacion = fechaInicio;
			this.codigoTipoInventarioUbicacion = codigoTipoInventarioUbicacion;
			this.codigoTipoInventarioTipo = codigoTipoInventarioTipo;
			this.costoContador = costoContador;
			this.indicativoAjuste = "N";
			this.fechaAjuste = fechaInicio;
			this.usuarioRelacionado = usuarioRelacionado;
			rolesEspecificos = new CollectionRolesInventario();
			itemsInventario = new CollectionItemsInventarioFisico();
		}

		public InventarioFisico(
			string prefijoInventario, 
			int numeroInventario)
        {
            this.prefijoInventario = prefijoInventario;
            this.numeroInventario = numeroInventario;
			int i=0;

			rolesEspecificos = new CollectionRolesInventario();
			itemsInventario = new CollectionItemsInventarioFisico();

			DataSet ds = new DataSet();

			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,minf_numeroinv,minf_fechainicio,minf_fechacierre,tifu_codigo,tift_codigo,minf_costocontador,minf_realizoajuste,minf_fechaajuste,susu_usuario FROM minventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+";"+
				"SELECT mrinf_secuencia, prinf_codigo, mnit_nit, mrol_grupoasoc FROM mrolinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinf="+numeroInventario+";"+
				"SELECT DINV.dinv_mite_codigo, DINV.dinv_mite_nombre, DINV.dinv_conteoactual, DINV.dinv_tarjeta, DINV.dinv_pubi_codigo, DINV.dinv_palm_almacen, COALESCE(DINV.dinv_msal_cantactual,0) dinv_msal_cantactual, DINV.dinv_costprom, DINV.dinv_conteo1, DINV.dinv_conteo2, DINV.dinv_conteo3, DINV.dinv_fechconteo, DINV.dinv_fechconteofin, DINV.dinv_ubicacion, DINV.dinv_stand, COALESCE(DINV.dinv_diferencia,0) as dinv_diferencia, DINV.dinv_costdiferencia, DINV.dinv_contdefinitivo, DBXSCHEMA.EDITARREFERENCIAS(DINV.dinv_mite_codigo,PLIN.plin_tipo) as referencia_editada FROM dinventariofisico DINV INNER JOIN mitems MIT ON MIT.mite_codigo = DINV.dinv_mite_codigo INNER JOIN plineaitem PLIN ON MIT.plin_codigo = PLIN.plin_codigo WHERE DINV.pdoc_codigo='"+prefijoInventario+"' AND DINV.minf_numeroinv="+numeroInventario+";");
			
			if(ds.Tables[0].Rows.Count > 0)
			{
				this.fechaInicio = Convert.ToDateTime(ds.Tables[0].Rows[0][2]);

				if(ds.Tables[0].Rows[0][3].ToString() != String.Empty)
					this.fechaTerminacion = Convert.ToDateTime(ds.Tables[0].Rows[0][3]);
				
				this.codigoTipoInventarioUbicacion = ds.Tables[0].Rows[0][4].ToString();
				this.codigoTipoInventarioTipo = ds.Tables[0].Rows[0][5].ToString();
				this.costoContador = Convert.ToDouble(ds.Tables[0].Rows[0][6]);
				this.indicativoAjuste = ds.Tables[0].Rows[0][7].ToString();
				
				if(ds.Tables[0].Rows[0][8].ToString() != String.Empty)
					this.fechaAjuste = Convert.ToDateTime(ds.Tables[0].Rows[0][8]);
				
				this.usuarioRelacionado = ds.Tables[0].Rows[0][9].ToString();
				
				for(i=0;i<ds.Tables[1].Rows.Count;i++)
				{
					if(ds.Tables[1].Rows[i][3] != DBNull.Value)
						this.rolesEspecificos.Add(new RolesInventarioFisico(Convert.ToInt32(ds.Tables[1].Rows[i][0]),ds.Tables[1].Rows[i][2].ToString(),ds.Tables[1].Rows[i][1].ToString(),Convert.ToInt32(ds.Tables[1].Rows[i][3])));
					else
						this.rolesEspecificos.Add(new RolesInventarioFisico(Convert.ToInt32(ds.Tables[1].Rows[i][0]),ds.Tables[1].Rows[i][2].ToString(),ds.Tables[1].Rows[i][1].ToString(),-1));
				}
				
				dtItemsInventarioFisico = ds.Tables[2];
				
				for(i=0;i<ds.Tables[2].Rows.Count;i++)
				{
					DateTime fechConteo = Convert.ToDateTime(null);
					DateTime fechConteoFin = Convert.ToDateTime(null);
					
					int diferenciaConteo = -1;
					
					if(ds.Tables[2].Rows[i][11] != DBNull.Value)
						fechConteo = Convert.ToDateTime(ds.Tables[2].Rows[i][11]);
					
					if(ds.Tables[2].Rows[i][12] != DBNull.Value)
						fechConteoFin = Convert.ToDateTime(ds.Tables[2].Rows[i][12]);
					
					if(ds.Tables[2].Rows[i][15] != DBNull.Value)
						diferenciaConteo = Convert.ToInt32(ds.Tables[2].Rows[i][15]);

					try
					{
						this.itemsInventario.Add(new ItemsInventarioFisico(
							ds.Tables[2].Rows[i][0].ToString(),
							ds.Tables[2].Rows[i][1].ToString(),
							Convert.ToInt32(ds.Tables[2].Rows[i][2]),
							Convert.ToInt32(ds.Tables[2].Rows[i][3]),
							Convert.ToInt32(ds.Tables[2].Rows[i][4]),
							ds.Tables[2].Rows[i][5].ToString(),
							Convert.ToDouble(ds.Tables[2].Rows[i][6]),
							Convert.ToDouble(ds.Tables[2].Rows[i][7]),
							Convert.ToInt32(ds.Tables[2].Rows[i][8]),
							Convert.ToInt32(ds.Tables[2].Rows[i][9]),
							Convert.ToInt32(ds.Tables[2].Rows[i][10]),
							fechConteo,
							fechConteoFin,
							ds.Tables[2].Rows[i][13].ToString(),
							ds.Tables[2].Rows[i][14].ToString(),
							diferenciaConteo,
							Convert.ToDouble(ds.Tables[2].Rows[i][16].ToString()),
							Convert.ToInt32(ds.Tables[2].Rows[i][17].ToString()),
							ds.Tables[2].Rows[i][18].ToString()));
					}
					catch(Exception ex)
					{
						string sr = ex.ToString();
					}
				}
			}
		}
		#endregion

		#region Metodos

		private CollectionItemsInventarioFisico ConsultarItemsInventarioFisico(string sql)
		{
			CollectionItemsInventarioFisico collectionItemsInventarioFisico = new CollectionItemsInventarioFisico();

			DataSet ds = new DataSet();

			DBFunctions.Request(ds,IncludeSchema.NO,string.Format(sqlItesInventarioFisico,this.PrefijoInventario,this.NumeroInventario,sql));

			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				try
				{
					collectionItemsInventarioFisico.Add(new ItemsInventarioFisico(
						ValidarDBNull(ds.Tables[0].Rows[i][0],typeof(string)).ToString(),
						ValidarDBNull(ds.Tables[0].Rows[i][1],typeof(string)).ToString(),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][2],typeof(int))),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][3],typeof(int))),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][4],typeof(int))),
						ValidarDBNull(ds.Tables[0].Rows[i][5],typeof(string)).ToString(),
						Convert.ToDouble(ValidarDBNull(ds.Tables[0].Rows[i][6],typeof(double))),
						Convert.ToDouble(ValidarDBNull(ds.Tables[0].Rows[i][7],typeof(double))),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][8],typeof(int))),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][9],typeof(int))),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][10],typeof(int))),
						Convert.ToDateTime(ValidarDBNull(ds.Tables[0].Rows[i][11],typeof(DateTime))),
						Convert.ToDateTime(ValidarDBNull(ds.Tables[0].Rows[i][12],typeof(DateTime))),
						ValidarDBNull(ds.Tables[0].Rows[i][13],typeof(string)).ToString(),
						ValidarDBNull(ds.Tables[0].Rows[i][14],typeof(string)).ToString(),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][15],typeof(int))),
						Convert.ToDouble(ValidarDBNull(ds.Tables[0].Rows[i][16],typeof(double))),
						Convert.ToInt32(ValidarDBNull(ds.Tables[0].Rows[i][17],typeof(int))),
						ValidarDBNull(ds.Tables[0].Rows[i][18],typeof(string)).ToString()));
				}
				catch(Exception ex)
				{
					string sr = ex.ToString();
				}
			}

			return collectionItemsInventarioFisico;
		}

		private DataTable ConsultaItemsInventarioFisico(string sql)
		{
			DataSet ds = new DataSet();

			DBFunctions.Request(ds,IncludeSchema.NO,string.Format(sqlItesInventarioFisico,this.PrefijoInventario,this.NumeroInventario,sql));

			if(ds.Tables.Count > 0)
				return ds.Tables[0];
			else
				return null;
		}

		public bool AlmacenarRegistroInicioInventarioFisico()
		{
			bool status = false;

			// Obtener el primer número de inventario físico disponible.
			while(DBFunctions.RecordExist("SELECT minf_numeroinv FROM minventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario))
				numeroInventario += 1;

			ArrayList sqlStrings = new ArrayList();

			sqlStrings.Add("INSERT INTO minventariofisico(pdoc_codigo,minf_numeroinv,minf_fechainicio,minf_fechacierre,tifu_codigo,tift_codigo,minf_costocontador,minf_realizoajuste,minf_fechaajuste,susu_usuario) VALUES ('"+prefijoInventario+"',"+numeroInventario+",'"+fechaInicio.ToString("yyyy-MM-dd")+"',NULL,'"+codigoTipoInventarioUbicacion+"','"+codigoTipoInventarioTipo+"',"+costoContador+",'"+indicativoAjuste+"',NULL,'"+usuarioRelacionado+"')");
			
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroInventario+" WHERE pdoc_codigo='"+prefijoInventario+"'");
			
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg = DBFunctions.exceptions;
			}

			return status;
		}

		public bool CrearRolesInventarioFisico()
		{
			bool status = false;

			ArrayList sqlStrings = new ArrayList();
			
			//En primer lugar se eliminan los roles anteriores y luego se agregan los que se han configurado
			sqlStrings.Add("DELETE FROM mrolinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinf="+numeroInventario);

			//Ahora recorremos los roles que se han configurado
			for(int i=0;i<rolesEspecificos.Count;i++)
			{
				if(rolesEspecificos[i].CodigoGrupoRelacionado != -1)
					sqlStrings.Add("INSERT INTO mrolinventariofisico(pdoc_codigo,minf_numeroinf,prinf_codigo,mnit_nit,mrol_grupoasoc) VALUES ('"+prefijoInventario+"',"+numeroInventario+","+rolesEspecificos[i].CodigoRol+",'"+rolesEspecificos[i].NitPersona+"',"+rolesEspecificos[i].CodigoGrupoRelacionado+")");
				else
					sqlStrings.Add("INSERT INTO mrolinventariofisico(pdoc_codigo,minf_numeroinf,prinf_codigo,mnit_nit,mrol_grupoasoc) VALUES ('"+prefijoInventario+"',"+numeroInventario+","+rolesEspecificos[i].CodigoRol+",'"+rolesEspecificos[i].NitPersona+"',null)");
			}

			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg = DBFunctions.exceptions;
			}

			return status;
		}

		public bool CrearItemsInventario()
		{
			string processMsg = string.Empty;
 
			ArrayList sqlStrings = new ArrayList();
			
			//En primer lugar se eliminan los registros anteriores y luego se agregan los nuevos que se han configurado
			sqlStrings.Add("DELETE FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario);
			
			if(!DBFunctions.Transaction(sqlStrings))
				processMsg = DBFunctions.exceptions;
			
			//Ahora recorremos los ítems de inventario que se han configurado
			for(int i=0;i<itemsInventario.Count;i++)
			{
				itemsInventario[i].NumeroTarjeta = i + 1;

				processMsg = processMsg + InsertarItemsInventarioFisico(PrefijoInventario,NumeroInventario,itemsInventario[i]);

				#region Código candidato a eliminación
				/*
				if(itemsInventario[i].CodigoItemRelacionado != String.Empty)
				{
					// Ingresar tarjeta de conteo.
					sqlStrings.Add("INSERT INTO dinventariofisico(pdoc_codigo,minf_numeroinv,dinv_conteoactual,dinv_tarjeta,dinv_pubi_codigo,dinv_mite_codigo,dinv_mite_nombre,dinv_palm_almacen,dinv_msal_cantactual,dinv_costprom,dinv_conteo1,dinv_conteo2,dinv_conteo3,dinv_fechconteo,dinv_fechconteofin,dinv_ubicacion,dinv_stand,dinv_diferencia,dinv_costdiferencia,dinv_contdefinitivo) VALUES"+
						"('"+prefijoInventario+"',"+numeroInventario+", "+itemsInventario[i].ConteoActual+","+numeroTarjeta.ToString()+","+itemsInventario[i].CodigoUbicacionInicial+",'"+itemsInventario[i].CodigoItemRelacionado+"','"+itemsInventario[i].NombreItemRelacionado+"','"+itemsInventario[i].CodigoAlmacen+"',"+itemsInventario[i].SaldoActual+","+itemsInventario[i].CostoPromedio+","+ValidarValorCadena(itemsInventario[i].Conteo1)+","+ValidarValorCadena(itemsInventario[i].Conteo2)+","+ValidarValorCadena(itemsInventario[i].Conteo3)+","+ValidarValorCadena(itemsInventario[i].FechaInicioConteo)+","+ValidarValorCadena(itemsInventario[i].FechaFinConteo)+",'"+itemsInventario[i].DescripcionUbicacion+"','"+itemsInventario[i].NombreStand+"',"+ValidarValorCadena(itemsInventario[i].DiferenciaConteo)+","+ValidarValorCadena(itemsInventario[i].CostoDiferencia)+","+ValidarValorCadena(itemsInventario[i].ConteoDefinitivo)+")");
				}
				else
				{
					// Ingresar tarjeta de conteo de alta.
					sqlStrings.Add("INSERT INTO dinventariofisico(pdoc_codigo,minf_numeroinv,dinv_conteoactual,dinv_tarjeta,dinv_pubi_codigo,dinv_mite_codigo,dinv_mite_nombre,dinv_palm_almacen,dinv_msal_cantactual,dinv_costprom,dinv_conteo1,dinv_conteo2,dinv_conteo3,dinv_fechconteo,dinv_fechconteofin,dinv_ubicacion,dinv_stand,dinv_diferencia) VALUES"+
						"('"+prefijoInventario+"',"+numeroInventario+", "+itemsInventario[i].ConteoActual+","+numeroTarjeta.ToString()+",null,null,null,'"+itemsInventario[i].CodigoAlmacen+"',"+itemsInventario[i].SaldoActual+","+ValidarValorCadena(itemsInventario[i].CostoPromedio)+","+ValidarValorCadena(itemsInventario[i].Conteo1)+","+ValidarValorCadena(itemsInventario[i].Conteo2)+","+ValidarValorCadena(itemsInventario[i].Conteo3)+",null,null,null,null,null)");
				}
				*/
				#endregion
			}
			this.processMsg=processMsg;
			return (processMsg == string.Empty);
		}


		public bool ActualizarItemsInventario()
		{
			string processMsg = string.Empty;
 
			ArrayList sqlStrings = new ArrayList();
			
			DataSet dsInventario=new DataSet();
			//Consultar dinventariofisico actual
			DBFunctions.Request(dsInventario,IncludeSchema.NO,
				"SELECT * FROM dinventariofisico "+
				"WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario);
			DataTable dtInventario=dsInventario.Tables[0];
			DataRow[] drItem;

			//Consultar ultima tarjeta del inventario
			int numTarjetaN;
			bool cambia=false;
			try
			{
				numTarjetaN=Convert.ToInt16(DBFunctions.SingleData(
					"SELECT MAX(DINV_TARJETA) FROM dinventariofisico "+
					"WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario))+1;
				numeroTarjetaActDesde=numTarjetaN;
			}
			catch
			{
				numTarjetaN=1;
				numeroTarjetaActDesde=numTarjetaN;
			}

			//Poner todas las cantidades en ceros (algunas referencias pueden no existir), luego actualizamos cada una
			processMsg = processMsg + ReiniciarItemsInventarioFisico(PrefijoInventario,NumeroInventario);
			
			//Ahora recorremos los ítems de inventario que se han configurado
			for(int i=0;i<itemsInventario.Count;i++)
			{
				//Existe?
				drItem=dtInventario.Select("IsNull(DINV_PUBI_CODIGO,-1)="+itemsInventario[i].CodigoUbicacionInicial+" AND DINV_MITE_CODIGO='"+itemsInventario[i].CodigoItemRelacionado+"' AND IsNull(DINV_PALM_ALMACEN,'')='"+itemsInventario[i].CodigoAlmacen+"'");
				if(drItem.Length>0)
				{
					//Es necesario actualizar?
					//if(itemsInventario[i].SaldoActual != Convert.ToDouble(drItem[0]["dinv_msal_cantactual"]) || itemsInventario[i].CostoPromedio != Convert.ToDouble(drItem[0]["dinv_costprom"]))
					//Actualizar los que existian
					processMsg = processMsg + ModificarItemsInventarioFisico(PrefijoInventario,NumeroInventario,Convert.ToInt16(drItem[0]["dinv_tarjeta"]),itemsInventario[i]);
				}
				else
				{
					//Insertar item nuevo (no existe con el almacen y ubicacion dadas) si la cantidad>0
					if(itemsInventario[i].SaldoActual>0)
					{
						itemsInventario[i].NumeroTarjeta = numTarjetaN;
						processMsg = processMsg + InsertarItemsInventarioFisico(PrefijoInventario,NumeroInventario,itemsInventario[i]);
						numeroTarjetaActHasta=numTarjetaN;
						numTarjetaN++;
						cambia=true;
					}
				}
			}
			if(!cambia)
				numeroTarjetaActDesde=numeroTarjetaActHasta=0;
			this.processMsg=processMsg;
			return (processMsg == string.Empty);
		}

		public bool ActualizarItemsInventarioFisicoInstancia()
		{
			bool status = true;

			string processMsg = string.Empty;
			
			//Ahora recorremos los ítems de inventario que se han configurado.
			for(int i=0;i<itemsInventario.Count;i++)
			{
				#region Código candidato a eliminación
				/*
				if(DBFunctions.RecordExist("SELECT dinv_tarjeta FROM dbxschema.dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario.ToString()+" dinv_tarjeta="+itemsInventario[i].NumeroTarjeta.ToString()))
				{
					// Actualizar tarjetas de conteo.
					sqlStrings.Add("UPDATE dbxschema.dinventariofisico SET dinv_conteoactual"+ValidarValorCadena(itemsInventario[i].ConteoActual)+",dinv_pubi_codigo="+ValidarValorCadena(itemsInventario[i].CodigoUbicacionInicial)+",dinv_mite_codigo="+ValidarValorCadena(itemsInventario[i].CodigoItemRelacionado)+",dinv_mite_nombre="+ValidarValorCadena(itemsInventario[i].NombreItemRelacionado)+",dinv_palm_almacen="+ValidarValorCadena(itemsInventario[i].CodigoAlmacen)+",dinv_msal_cantactual="+ValidarValorCadena(itemsInventario[i].SaldoActual)+",dinv_costprom="+ValidarValorCadena(itemsInventario[i].CostoPromedio)+",dinv_conteo1="+ValidarValorCadena(itemsInventario[i].Conteo1)+",dinv_conteo2="+ValidarValorCadena(itemsInventario[i].Conteo2)+",dinv_conteo3="+ValidarValorCadena(itemsInventario[i].Conteo3)+",dinv_fechconteo="+ValidarValorCadena(itemsInventario[i].FechaInicioConteo)+",dinv_fechconteofin="+ValidarValorCadena(itemsInventario[i].FechaFinConteo)+",dinv_ubicacion="+ValidarValorCadena(itemsInventario[i].DescripcionUbicacion)+",dinv_stand="+ValidarValorCadena(itemsInventario[i].NombreStand)+",dinv_diferencia="+ValidarValorCadena(itemsInventario[i].DiferenciaConteo)+",dinv_costdiferencia="+ValidarValorCadena(itemsInventario[i].CostoDiferencia)+",dinv_contdefinitivo="+ValidarValorCadena(itemsInventario[i].ConteoDefinitivo)+" WHERE  pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario.ToString()+" AND dinv_tarjeta="+itemsInventario[i].NumeroTarjeta.ToString());
				}
				*/
				#endregion

				processMsg = processMsg + ActualizarItemsInventarioFisico(PrefijoInventario,NumeroInventario,ItemsInventario[i]);
			}
			
			if(processMsg != string.Empty)
			{
				status = true;
				this.processMsg = processMsg;
			}
			
			return status;
		}

		public ItemsInventarioFisico BuscarInstanciaItem(string codigoReferencia, int numeroTarjeta)
		{
			ItemsInventarioFisico salida = new ItemsInventarioFisico();
			
			for(int i=0;i<itemsInventario.Count;i++)
			{
				if(itemsInventario[i].CodigoItemRelacionado == codigoReferencia && itemsInventario[i].NumeroTarjeta == numeroTarjeta)
				{
					salida = itemsInventario[i];
					break;
				}
			}
			
			return salida;
		}
		#endregion

		#region Metodos Estático

		public static object ValidarDBNull(object valor,Type tipo)
		{
			object valorValidado = null;

			if(valor != DBNull.Value) 
				valorValidado = valor;
			else
			{
				if(tipo.Equals(typeof(string)))							 
					valorValidado = string.Empty;
				else if(tipo.Equals(typeof(int)))
					valorValidado = -1;
				else if(tipo.Equals(typeof(double)))
					valorValidado = -1;
				else if(tipo.Equals(typeof(DateTime)))
					valorValidado = new DateTime();
			}

			return valorValidado;
		}

		public static string ValidarValorCadena(object valor)
		{
			string valorValidado = cadenaNulo;

			if(valor is string)
			{
				if((string)valor == string.Empty)
					valorValidado = cadenaNulo;
				else
					valorValidado = "'"+valor.ToString()+"'";
			}
			else if(valor is int)
			{
				if ((int)valor == -1)
					valorValidado = cadenaNulo;
				else
					valorValidado = valor.ToString();
			}
			else if(valor is double) 
			{
				if((double)valor == -1)
					valorValidado = cadenaNulo;
				else
					valorValidado = valor.ToString();
			}
			else if(valor is DateTime)
			{
				if((DateTime)valor == DateTime.MinValue)
					valorValidado = cadenaNulo;
				else
					valorValidado = "'"+Convert.ToDateTime(valor).ToString("yyyy-MM-dd")+"'";
			}

			return valorValidado;
		}

		#region Código candidato a eliminación
		/*
		public static string ActualizarItemsInventarioFisico(string prefijoInventario, int numeroInventario,ItemsInventarioFisico itemsInventario)
		{
			string processMsg = string.Empty;
 
			ArrayList sqlStrings = new ArrayList();
			
			if(DBFunctions.RecordExist("SELECT dinv_tarjeta FROM dbxschema.dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario.ToString()+" AND dinv_tarjeta="+itemsInventario.NumeroTarjeta.ToString()))
				sqlStrings.Add("UPDATE dbxschema.dinventariofisico SET dinv_conteoactual="+ValidarValorCadena(itemsInventario.ConteoActual)+",dinv_pubi_codigo="+ValidarValorCadena(itemsInventario.CodigoUbicacionInicial)+",dinv_mite_codigo="+ValidarValorCadena(itemsInventario.CodigoItemRelacionado)+",dinv_mite_nombre="+ValidarValorCadena(itemsInventario.NombreItemRelacionado)+",dinv_palm_almacen="+ValidarValorCadena(itemsInventario.CodigoAlmacen)+",dinv_msal_cantactual="+ValidarValorCadena(itemsInventario.SaldoActual)+",dinv_costprom="+ValidarValorCadena(itemsInventario.CostoPromedio)+",dinv_conteo1="+ValidarValorCadena(itemsInventario.Conteo1)+",dinv_conteo2="+ValidarValorCadena(itemsInventario.Conteo2)+",dinv_conteo3="+ValidarValorCadena(itemsInventario.Conteo3)+",dinv_fechconteo="+ValidarValorCadena(itemsInventario.FechaInicioConteo)+",dinv_fechconteofin="+ValidarValorCadena(itemsInventario.FechaFinConteo)+",dinv_ubicacion="+ValidarValorCadena(itemsInventario.DescripcionUbicacion)+",dinv_stand="+ValidarValorCadena(itemsInventario.NombreStand)+",dinv_diferencia="+ValidarValorCadena(itemsInventario.DiferenciaConteo)+",dinv_costdiferencia="+ValidarValorCadena(itemsInventario.CostoDiferencia)+",dinv_contdefinitivo="+ValidarValorCadena(itemsInventario.ConteoDefinitivo)+" WHERE  pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario.ToString()+" AND dinv_tarjeta="+itemsInventario.NumeroTarjeta.ToString());
			
			if(!DBFunctions.Transaction(sqlStrings))
				processMsg = DBFunctions.exceptions;
			
			return processMsg;
		}
		*/
		#endregion

		public static bool ExistaTarjeta(string prefijoInventario, int numeroInventario, int numeroTarjeta)
		{
			return DBFunctions.RecordExist("SELECT dinv_tarjeta FROM dbxschema.dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario.ToString()+" AND dinv_tarjeta="+numeroTarjeta.ToString());
		}

		public static string InsertarItemsInventarioFisico(string prefijoInventario, int numeroInventario,ItemsInventarioFisico itemsInventarioFisico)
		{
			string processMsg = string.Empty;
        
			ArrayList sqlStrings = new ArrayList();
			
			sqlStrings.Add("INSERT INTO dinventariofisico(pdoc_codigo,minf_numeroinv,dinv_conteoactual,dinv_tarjeta,dinv_pubi_codigo,dinv_mite_codigo,dinv_mite_nombre,dinv_palm_almacen,dinv_msal_cantactual,dinv_costprom,dinv_conteo1,dinv_conteo2,dinv_conteo3,dinv_fechconteo,dinv_fechconteofin,dinv_ubicacion,dinv_stand,dinv_diferencia,dinv_costdiferencia,dinv_contdefinitivo) VALUES"+
				"('"+prefijoInventario+"',"+
                numeroInventario+","+
                ValidarValorCadena(itemsInventarioFisico.ConteoActual)+","+
                itemsInventarioFisico.NumeroTarjeta.ToString()+","+
                ValidarValorCadena(itemsInventarioFisico.CodigoUbicacionInicial)+","+
                ValidarValorCadena(itemsInventarioFisico.CodigoItemRelacionado)+","+
                ValidarValorCadena(itemsInventarioFisico.NombreItemRelacionado.Replace("&"," "))+","+
                ValidarValorCadena(itemsInventarioFisico.CodigoAlmacen)+","+
                ValidarValorCadena(itemsInventarioFisico.SaldoActual) + "," +
         //       "0,"+
                ValidarValorCadena(itemsInventarioFisico.CostoPromedio)+","+
                ValidarValorCadena(itemsInventarioFisico.Conteo1)+","+
                ValidarValorCadena(itemsInventarioFisico.Conteo2)+","+
                ValidarValorCadena(itemsInventarioFisico.Conteo3)+","+
                ValidarValorCadena(itemsInventarioFisico.FechaInicioConteo)+","+
                ValidarValorCadena(itemsInventarioFisico.FechaFinConteo)+","+
                ValidarValorCadena(itemsInventarioFisico.DescripcionUbicacion)+","+
                ValidarValorCadena(itemsInventarioFisico.NombreStand)+","+
                ValidarValorCadena(itemsInventarioFisico.DiferenciaConteo)+","+
                ValidarValorCadena(itemsInventarioFisico.CostoDiferencia)+","+
                ValidarValorCadena(itemsInventarioFisico.ConteoDefinitivo)+")");
			
			if(!DBFunctions.Transaction(sqlStrings))
				processMsg = DBFunctions.exceptions;
			
			return processMsg;
		}
		
		public static string ReiniciarItemsInventarioFisico(string prefijoInventario, int numeroInventario)
		{
			string processMsg = string.Empty;
 
			ArrayList sqlStrings = new ArrayList();
			
			sqlStrings.Add("UPDATE dinventariofisico "+
				"SET DINV_MSAL_CANTACTUAL=0 "+
				"WHERE PDOC_CODIGO='"+prefijoInventario+"' AND "+
				"MINF_NUMEROINV="+numeroInventario+";");
			
			if(!DBFunctions.Transaction(sqlStrings))
				processMsg = DBFunctions.exceptions;
			
			return processMsg;
		}

		public static string ModificarItemsInventarioFisico(string prefijoInventario, int numeroInventario, int numTarjeta, ItemsInventarioFisico itemsInventarioFisico)
		{
			string processMsg = string.Empty;
 
			ArrayList sqlStrings = new ArrayList();
			
			sqlStrings.Add("UPDATE dinventariofisico "+
				"SET DINV_MSAL_CANTACTUAL="+ValidarValorCadena(itemsInventarioFisico.SaldoActual)+","+
				"DINV_COSTPROM="+ValidarValorCadena(itemsInventarioFisico.CostoPromedio)+" "+
				"WHERE PDOC_CODIGO='"+prefijoInventario+"' AND "+
				"MINF_NUMEROINV="+numeroInventario+" AND DINV_TARJETA="+numTarjeta+" AND DINV_PALM_ALMACEN='"+itemsInventarioFisico.CodigoAlmacen+"';");
			
			if(!DBFunctions.Transaction(sqlStrings))
				processMsg = DBFunctions.exceptions;
			
			return processMsg;
		}

		public static string ActualizarItemsInventarioFisico(string prefijoInventario, int numeroInventario,ItemsInventarioFisico itemsInventarioFisico)
		{
			string processMsg = string.Empty;
 
			ArrayList sqlStrings = new ArrayList();
			
			if(ExistaTarjeta(prefijoInventario,numeroInventario,itemsInventarioFisico.NumeroTarjeta))
			{
                int numeroTarjeta = new int ();
                numeroTarjeta = itemsInventarioFisico.NumeroTarjeta;
				// Altera la tabla de mubicacionitem, con los valores de las tarjetas.
			 	if(!UbicacionItem.ExisteUbicacionItem(itemsInventarioFisico.CodigoUbicacionInicial,itemsInventarioFisico.CodigoItemRelacionado))
			 		UbicacionItem.CambiarUbicacionItem(itemsInventarioFisico.CodigoUbicacionInicial,itemsInventarioFisico.CodigoItemRelacionado);
                if(itemsInventarioFisico.DiferenciaConteo!=null)
				    sqlStrings.Add("UPDATE dbxschema.dinventariofisico SET dinv_conteoactual=" + ValidarValorCadena(itemsInventarioFisico.ConteoActual) + ",dinv_pubi_codigo=" + ValidarValorCadena(itemsInventarioFisico.CodigoUbicacionInicial) + ",dinv_mite_codigo= "+ ValidarValorCadena(itemsInventarioFisico.CodigoItemRelacionado) + ",dinv_mite_nombre=" + ValidarValorCadena(itemsInventarioFisico.NombreItemRelacionado) + ",dinv_palm_almacen=" + ValidarValorCadena(itemsInventarioFisico.CodigoAlmacen) + ",dinv_msal_cantactual=" + ValidarValorCadena(itemsInventarioFisico.SaldoActual) + ",dinv_costprom=" + ValidarValorCadena(itemsInventarioFisico.CostoPromedio) + ",dinv_conteo1=" + ValidarValorCadena(itemsInventarioFisico.Conteo1) + ",dinv_conteo2=" + ValidarValorCadena(itemsInventarioFisico.Conteo2) + ",dinv_conteo3=" + ValidarValorCadena(itemsInventarioFisico.Conteo3) + ",dinv_fechconteo=" + ValidarValorCadena(itemsInventarioFisico.FechaInicioConteo) + ",dinv_fechconteofin=" + ValidarValorCadena(itemsInventarioFisico.FechaFinConteo) 
                        + ",dinv_ubicacion=" + ValidarValorCadena(itemsInventarioFisico.DescripcionUbicacion) + ",dinv_stand=" + ValidarValorCadena(itemsInventarioFisico.NombreStand) + ",dinv_diferencia=" + itemsInventarioFisico.DiferenciaConteo.ToString() + ",         dinv_costdiferencia=" + ValidarValorCadena(itemsInventarioFisico.CostoDiferencia) + ",dinv_contdefinitivo=" + ValidarValorCadena(itemsInventarioFisico.ConteoDefinitivo) + " WHERE  pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario.ToString() + " AND dinv_tarjeta=" + numeroTarjeta.ToString());
			    else
                    sqlStrings.Add("UPDATE dbxschema.dinventariofisico SET dinv_conteoactual=" + ValidarValorCadena(itemsInventarioFisico.ConteoActual) + ",dinv_pubi_codigo=" + ValidarValorCadena(itemsInventarioFisico.CodigoUbicacionInicial) + ",dinv_mite_codigo=" + ValidarValorCadena(itemsInventarioFisico.CodigoItemRelacionado) + ",dinv_mite_nombre=" + ValidarValorCadena(itemsInventarioFisico.NombreItemRelacionado) + ",dinv_palm_almacen=" + ValidarValorCadena(itemsInventarioFisico.CodigoAlmacen) + ",dinv_msal_cantactual=" + ValidarValorCadena(itemsInventarioFisico.SaldoActual) + ",dinv_costprom=" + ValidarValorCadena(itemsInventarioFisico.CostoPromedio) + ",dinv_conteo1=" + ValidarValorCadena(itemsInventarioFisico.Conteo1) + ",dinv_conteo2=" + ValidarValorCadena(itemsInventarioFisico.Conteo2) + ",dinv_conteo3=" + ValidarValorCadena(itemsInventarioFisico.Conteo3) + ",dinv_fechconteo=" + ValidarValorCadena(itemsInventarioFisico.FechaInicioConteo) + ",dinv_fechconteofin=" + ValidarValorCadena(itemsInventarioFisico.FechaFinConteo) + ",dinv_ubicacion=" + ValidarValorCadena(itemsInventarioFisico.DescripcionUbicacion) + ",dinv_stand=" + ValidarValorCadena(itemsInventarioFisico.NombreStand) + ",dinv_diferencia=" + ValidarValorCadena(itemsInventarioFisico.DiferenciaConteo) + ",dinv_costdiferencia=" + ValidarValorCadena(itemsInventarioFisico.CostoDiferencia) + ",dinv_contdefinitivo=" + ValidarValorCadena(itemsInventarioFisico.ConteoDefinitivo) + " WHERE  pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario.ToString() + " AND dinv_tarjeta=" + numeroTarjeta.ToString());
			
            }
			
			if(!DBFunctions.Transaction(sqlStrings))
				processMsg = DBFunctions.exceptions;
			
			return processMsg;
		}

		public static string ModificarEstadoConteoRenglon(string prefijoInventario, int numeroInventario, string codigoReferencia, int numeroTarjeta, int conteoRelacionado, double valorConteo, string codigoReferenciaModificado)
		{
			if(conteoRelacionado > 2 || Convert.ToInt32(DBFunctions.SingleData("SELECT dinv_conteoactual FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta)) > 2)
				return "- Se ha sobrepasado la cantidad de conteos para el ítem "+codigoReferenciaModificado+" con tarjeta "+numeroTarjeta+"\\n";
			
			if(conteoRelacionado != Convert.ToInt32(DBFunctions.SingleData("SELECT dinv_conteoactual FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta)))
				return "- Ya se ha ingresado la información del conteo "+conteoRelacionado+" para el item "+codigoReferenciaModificado+" con la tarjeta "+numeroTarjeta+"\\n";
			
			ItemsInventarioFisico itemsInventarioFisico = new ItemsInventarioFisico(prefijoInventario,numeroInventario,numeroTarjeta);

			switch(conteoRelacionado)
			{
				case 0:
					itemsInventarioFisico.ConteoActual = 1;
					itemsInventarioFisico.Conteo1 = Convert.ToInt32(valorConteo);
					itemsInventarioFisico.FechaInicioConteo = DateTime.Now;
					break;
				case 1:
					itemsInventarioFisico.ConteoActual = 2;
					itemsInventarioFisico.Conteo2 = Convert.ToInt32(valorConteo);
					break;
				case 2:
					itemsInventarioFisico.ConteoActual = 3;
					itemsInventarioFisico.Conteo3 = Convert.ToInt32(valorConteo);
					itemsInventarioFisico.FechaFinConteo = DateTime.Now;
					break;
			}

			return ActualizarItemsInventarioFisico(prefijoInventario,numeroInventario,itemsInventarioFisico);
		}

		public static string ModificarEstadoConteoRenglon(string prefijoInventario, int numeroInventario, string codigoReferencia, int numeroTarjeta, int conteoRelacionado, double valorConteo, string codigoReferenciaModificado, int codigoUbicacion, string descripcionUbi)
		{
            #region Código candidato a eliminación
            /*
			if(conteoRelacionado > 2 || Convert.ToInt32(DBFunctions.SingleData("SELECT dinv_conteoactual FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta+" AND dinv_mite_codigo='"+codigoReferencia+"'")) > 2)
				return "- Se ha sobrepasado la cantidad de conteos para el item "+codigoReferenciaModificado+ "con tarjeta "+numeroTarjeta+"\\n";
			if(conteoRelacionado != Convert.ToInt32(DBFunctions.SingleData("SELECT dinv_conteoactual FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta+" AND dinv_mite_codigo='"+codigoReferencia+"'")))
				return "- Ya se ha ingresado la información del conteo "+conteoRelacionado+" para el item "+codigoReferenciaModificado+" con la tarjeta "+numeroTarjeta+"\\n";
			int filasModificadas = -1;
			if(conteoRelacionado == 0)
				filasModificadas = DBFunctions.NonQuery("UPDATE dinventariofisico SET dinv_conteoactual=1,dinv_conteo1="+valorConteo+",dinv_diferencia=dinv_msal_cantactual-"+valorConteo+",dinv_fechconteo='"+DateTime.Now.ToString("yyyy-MM-dd")+"', dinv_pubi_codigo = "+codigoUbicacion+" WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta+" AND dinv_mite_codigo='"+codigoReferencia+"'");
			else if(conteoRelacionado == 1)
				filasModificadas = DBFunctions.NonQuery("UPDATE dinventariofisico SET dinv_conteoactual=2,dinv_conteo2="+valorConteo+",dinv_diferencia=dinv_conteo1-"+valorConteo+", dinv_pubi_codigo = "+codigoUbicacion+"  WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta+" AND dinv_mite_codigo='"+codigoReferencia+"'");
			else if(conteoRelacionado == 2)
				filasModificadas = DBFunctions.NonQuery("UPDATE dinventariofisico SET dinv_conteoactual=3,dinv_conteo3="+valorConteo+",dinv_diferencia=dinv_conteo2-"+valorConteo+",dinv_fechconteofin='"+DateTime.Now.ToString("yyyy-MM-dd")+"', dinv_pubi_codigo = "+codigoUbicacion+" WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta+" AND dinv_mite_codigo='"+codigoReferencia+"'");
			if(filasModificadas <= 0)
				return DBFunctions.exceptions;
			else
				return String.Empty;
			*/
            #endregion
            ItemsInventarioFisico itemsInventarioFisico = new ItemsInventarioFisico(prefijoInventario, numeroInventario, numeroTarjeta);

            if (conteoRelacionado > 2 || itemsInventarioFisico.ConteoActual > 2)
				return "- Se ha sobrepasado la cantidad de conteos para el ítem "+codigoReferenciaModificado+" con tarjeta "+numeroTarjeta+"\\n";
			
			if(conteoRelacionado != itemsInventarioFisico.ConteoActual)
				return "- Ya se ha ingresado la información del conteo "+conteoRelacionado+" para el item "+codigoReferenciaModificado+" con la tarjeta "+numeroTarjeta+"\\n";
			
			//ItemsInventarioFisico itemsInventarioFisico = new ItemsInventarioFisico(prefijoInventario,numeroInventario,numeroTarjeta);

			itemsInventarioFisico.CodigoUbicacionInicial = codigoUbicacion;
            itemsInventarioFisico.DescripcionUbicacion = descripcionUbi;
            //itemsInventarioFisico.
			switch(conteoRelacionado)
			{
				case 0:
					itemsInventarioFisico.ConteoActual = 1;
					itemsInventarioFisico.Conteo1 = Convert.ToInt32(valorConteo);
					itemsInventarioFisico.FechaInicioConteo = DateTime.Now;
					break;
				case 1:
					itemsInventarioFisico.ConteoActual = 2;
					itemsInventarioFisico.Conteo2 = Convert.ToInt32(valorConteo);
					break;
				case 2:
					itemsInventarioFisico.ConteoActual = 3;
					itemsInventarioFisico.Conteo3 = Convert.ToInt32(valorConteo);
					itemsInventarioFisico.FechaFinConteo = DateTime.Now;
					break;
			}

			return ActualizarItemsInventarioFisico(prefijoInventario,numeroInventario,itemsInventarioFisico);
		}

		public static string EditarValorConteoTarjeta(string prefijoInventario, int numeroInventario, string codigoReferencia, int numeroTarjeta, int conteoRelacionado, double valorConteo, string codigoAlmacen, int codigoUbicacion)
		{
			#region Código candidato a eliminación
			/*
			int filasModificadas = -1;
			if(conteoRelacionado == 0)
				filasModificadas = DBFunctions.NonQuery("UPDATE dinventariofisico SET dinv_conteo1="+valorConteo+",dinv_diferencia=dinv_msal_cantactual-"+valorConteo+",dinv_palm_almacen='"+codigoAlmacen+"',dinv_pubi_codigo="+codigoUbicacion+" WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta);
			else if(conteoRelacionado == 1)
				filasModificadas = DBFunctions.NonQuery("UPDATE dinventariofisico SET dinv_conteo2="+valorConteo+",dinv_diferencia=dinv_conteo2-"+valorConteo+",dinv_palm_almacen='"+codigoAlmacen+"',dinv_pubi_codigo="+codigoUbicacion+" WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta);
			else if(conteoRelacionado == 2)
				filasModificadas = DBFunctions.NonQuery("UPDATE dinventariofisico SET dinv_conteo3="+valorConteo+",dinv_diferencia=dinv_conteo3-"+valorConteo+",dinv_palm_almacen='"+codigoAlmacen+"',dinv_pubi_codigo="+codigoUbicacion+" WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario+" AND dinv_tarjeta="+numeroTarjeta);
			if(filasModificadas <= 0)
				return DBFunctions.exceptions;
			else
				return String.Empty;
			*/
			#endregion

			ItemsInventarioFisico itemsInventarioFisico = new ItemsInventarioFisico(prefijoInventario,numeroInventario,numeroTarjeta);

			itemsInventarioFisico.CodigoAlmacen = codigoAlmacen;
			itemsInventarioFisico.CodigoUbicacionInicial = codigoUbicacion;

			switch(conteoRelacionado)
			{
				case 0:
					itemsInventarioFisico.Conteo1 = Convert.ToInt32(valorConteo);
					break;
				case 1:
					itemsInventarioFisico.Conteo2 = Convert.ToInt32(valorConteo);
					break;
				case 2:
					itemsInventarioFisico.Conteo3 = Convert.ToInt32(valorConteo);
					break;
			}

			return ActualizarItemsInventarioFisico(prefijoInventario,numeroInventario,itemsInventarioFisico);
		}

		public static string IngresarValorTarjetaAlta(
			//			string prefijoInventario, 
			//			int numeroInventario, 
			//			ref int numeroTarjeta, 
			//			int codigoUbicacion, 
			//			string codigoItem, 
			//			string almacen, 
			//			int cantidadConteo, 
			//			DateTime fechaInicioConteo)
			string prefijoInventario, 
			int numeroInventario,
			ItemsInventarioFisico itemsInventarioFisico)
		{
			return InsertarItemsInventarioFisico(prefijoInventario,numeroInventario,itemsInventarioFisico);
		}

		#region Código candidato a eliminación
		/*
		public static bool ConsultarExistenciaTarjetaAlta(string prefijoInventario, int numeroInventario, int numeroTarjeta)
		{
			return DBFunctions.RecordExist("SELECT dinv_tarjeta FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario+" AND dinv_mite_codigo IS NULL AND dinv_tarjeta = "+numeroTarjeta);
		}
		*/
		#endregion

		public static DataTable ConsultarUbicacionesUltimoNivelPorAlmacen(string codigoAlmacen)
		{
			DataTable dtUbicaciones = new DataTable();

			dtUbicaciones.Columns.Add(new DataColumn("CODIGO_UBICACION",typeof(string)));
			dtUbicaciones.Columns.Add(new DataColumn("NOMBRE_UBICACION",typeof(string)));

			DataSet dsUbicaciones = new DataSet();

            DBFunctions.Request(dsUbicaciones, IncludeSchema.NO, "SELECT pubi_codigo,pubi_codpad,pubi_nombre FROM pubicacionitem WHERE palm_almacen='" + codigoAlmacen + "' ORDER BY pubi_nombre");
			
			//Se seleccionan las ubicaciones de primer nivel
			DataRow[] primerNivel = dsUbicaciones.Tables[0].Select("pubi_codpad is null","pubi_codigo ASC");
			
			for(int i=0;i<primerNivel.Length;i++)
			{
				//Ahora se seleccionan las ubicaciones de segundo nivel
				DataRow[] segundoNivel = dsUbicaciones.Tables[0].Select("pubi_codpad = "+primerNivel[i]["pubi_codigo"],"pubi_NOMBRE ASC");
				
				for(int j=0;j<segundoNivel.Length;j++)
				{
					DataRow[] tercerNivel = dsUbicaciones.Tables[0].Select("pubi_codpad = "+segundoNivel[j]["pubi_codigo"],"pubi_NOMBRE ASC");
					
					for(int k=0;k<tercerNivel.Length;k++)
					{
						DataRow dr = dtUbicaciones.NewRow();

						dr[0] = tercerNivel[k]["pubi_codigo"].ToString();
						dr[1] = "["+tercerNivel[k]["pubi_nombre"].ToString()+"] - ["+segundoNivel[j]["pubi_nombre"].ToString()+"] - ["+primerNivel[i]["pubi_nombre"].ToString()+"]";

						dtUbicaciones.Rows.Add(dr);
					}
				}
			}

			return dtUbicaciones;
		}

		public static string ConsultarNombreUbicacion(int codigoUbicacion)
		{
			return DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codpad IS NULL AND pubi_codigo IN (SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo IN (SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo = "+codigoUbicacion+"))")+" - "+DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo IN (SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo = "+codigoUbicacion+")")+" - "+DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo = "+codigoUbicacion);
		}

		public static string CerrarInventarioFisico(string prefijoInventario, int numeroInventario)
		{
		
             
            string mensaje="";
         	   
                 //   uso constructor 5
                 string tarjetasAjuste = "SELECT D.pdoc_codigo concat cast(MINF_NUMEROINV as char (8)), dinv_tarjeta, dinv_mite_codigo, dinv_diferencia, "+
                                         " dinv_costprom, dinv_palm_almacen, CE.MNIT_NIT, PA.PCEN_CENTINV, CC.PVEN_CODIGO, PLIN_TIPO "+ 
                                         "  FROM dinventariofisico D, PALMACEN PA, CEMPRESA CE, CCARTERA CC, PLINEAITEM PL, MITEMS MI "+
                                         " WHERE D.pdoc_codigo = '"+prefijoInventario+"' AND MINF_NUMEROINV = "+numeroInventario+" AND dinv_diferencia <> 0 "+
                                         "   AND DINV_PALM_ALMACEN = PA.PALM_ALMACEN AND DINV_MITE_CODIGO = MI.MITE_CODIGO AND MI.PLIN_CODIGO = PL.PLIN_CODIGO "+
                                         " ORDER BY dinv_tarjeta; "; 
                 DataSet dsTarjetas = new DataSet ();
                 DBFunctions.Request(dsTarjetas, IncludeSchema.NO,tarjetasAjuste);
                 if(dsTarjetas.Tables[0].Rows.Count==0)
                 {
                     mensaje = "NO HAY tarjetas pora ajustar en este inventario !!!" ;
                     return mensaje;
                 }
                
                 //Movimiento de Kardex   Constructor #1
                 Movimiento Mov = new Movimiento(prefijoInventario,        // PREFIJO
                                      (uint)numeroInventario,              // NUMERO
                                      dsTarjetas.Tables[0].Rows[0][0].ToString(), // DOCMTO REFER
                                      (uint)Convert.ToInt32(dsTarjetas.Tables[0].Rows[0][1].ToString()), // NUMERO REFER
                                      50,                                  // MOVTO ajuestes
                                      dsTarjetas.Tables[0].Rows[0][6].ToString(), // NIT
                                      dsTarjetas.Tables[0].Rows[0][5].ToString(), // ALMACEN
                                      DateTime.Now ,                       // FECHA
                                      dsTarjetas.Tables[0].Rows[0][8].ToString(), // VENDEDOR
                                      "I",                                 // CARGO
                                      dsTarjetas.Tables[0].Rows[0][7].ToString(), // CENTRO COSTO
                                      "N","");                                // INDICATIVO
            
                 int n;
                 string ano_cont = DBFunctions.SingleData("SELECT pano_ano from cinventario");
                 ArrayList sqlStrings = new ArrayList();
                 //     ajusta cada tarjeta del Inventario Fisico que haya tenido diferencia en el conteo
                 for(n=0;n<dsTarjetas.Tables[0].Rows.Count;n++)
                 {
                     string codI  = "";
             //	    Referencias.Guardar(dsTarjetas.Tables[0].Rows[n][2].ToString(),ref codI,dsTarjetas.Tables[0]..Rows[n][9].ToString());
                     codI         = dsTarjetas.Tables[0].Rows[n][2].ToString();
                     double cant  = Convert.ToDouble(dsTarjetas.Tables[0].Rows[n][3].ToString());
                     double valU  = Convert.ToDouble(dsTarjetas.Tables[0].Rows[n][4].ToString());
                     string almacen = dsTarjetas.Tables[0].Rows[n][5].ToString();
                     int tarjeta  = Convert.ToInt16(dsTarjetas.Tables[0].Rows[n][1].ToString());
                     double costP = valU,costPH = valU,costPA = valU,costPHA = valU, pIva = 0, pDes = 0, invI = 0, invIA = 0;
                     DateTime fecha=DateTime.Now;
                     Mov.InsertaFila(codI,cant,valU,costP,costPA,pIva,pDes,0,costPH,costPHA,valU,invI,invIA,0,almacen,tarjeta);
                  }
            //    cierra el maestro de Inventario Fisico   
                  if (Mov.CerrarInventario(true, null))
                  {
                         DBFunctions.NonQuery("UPDATE minventariofisico SET minf_fechacierre='" + DateTime.Now.ToString("yyyy-MM-dd") + "', minf_realizoAJUSTE = 'S',minf_fechaAJUSTE='" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario + " ");
                         string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                         ProceHecEco contaOnline = new ProceHecEco();
	                     contaOnline.contabilizarOnline(prefijoInventario, numeroInventario, DateTime.Now, "");
             
                         mensaje = "";
                  }
                  else
                  {
                         mensaje += "<br>Error al ejecutar ajustes al Inventario : " + DBFunctions.exceptions + Mov.ProcessMsg;
                  }
                  
                 
		return mensaje;
        }

        public static string SegundoConteoAutomaticoInventarioFisico(string prefijoInventario, int numeroInventario)
        {
            string mensaje = "";

            try
            {
               DBFunctions.NonQuery(@"UPDATE dinventariofisico 
                                    SET DINV_CONTEOACTUAL = 2, DINV_CONTEO2 = DINV_CONTEO1, DINV_CONTEO3 = NULL, DINV_CONTDEFINITIVO = DINV_CONTEO1, 
                                    DINV_DIFERENCIA = DINV_CONTEO1 - DINV_MSAL_CANTACTUAL, DINV_COSTDIFERENCIA = (DINV_CONTEO1 - DINV_MSAL_CANTACTUAL) * DINV_COSTPROM 
                                    where pdoc_codigo = '" + prefijoInventario + "' AND MINF_NUMEROINV = " + numeroInventario + " ");
                string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                ProceHecEco contaOnline = new ProceHecEco();
                mensaje += "" ;
            }
            catch
            {
                mensaje += "<br>Error al ejecutar Segundo Conteo Automático al Inventario, proceso NO realizado !!! ";
            }


            return mensaje;
        }

		public static int NumeroTarjetasEnConteoActual(string prefijoInventario, int numeroInventario, int conteoActual)
		{
			int numeroTarjetasEnConteoActual = -1; 

			try
			{
				numeroTarjetasEnConteoActual = Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.dinventariofisico where pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = "+conteoActual.ToString()));
			}
			catch { }

			return numeroTarjetasEnConteoActual;
		}
	
		public static int NumeroTotalTarjetasAConteo1(string prefijoInventario, int numeroInventario)
		{
			return InventarioFisico.NumeroTarjetasEnConteoActual(prefijoInventario,numeroInventario,0);
		}
	
		public static int NumeroTotalTarjetasAConteo2(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasAConteo2 = -1; 

			try
			{
				numeroTotalTarjetasAConteo2 = Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.dinventariofisico where pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 1 AND dinv_conteo1 <> dinv_msal_cantactual"));
			}
			catch { }

			return numeroTotalTarjetasAConteo2;
		}
	
		public static int NumeroTotalTarjetasAConteo3(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasAConteo3 = -1; 

			try
			{
				numeroTotalTarjetasAConteo3 = Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.dinventariofisico where pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 2 AND dinv_conteo1 <> dinv_conteo2"));
			}
			catch { }

			return numeroTotalTarjetasAConteo3;
		}
	
		public static int NumeroTotalTarjetasEnConteoDefinitivo(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasEnConteoDefinitivo = -1; 

			try
			{
				numeroTotalTarjetasEnConteoDefinitivo = Convert.ToInt32(DBFunctions.SingleData("Select totalconteo1 + totalconteo2 + totalconteo3 totaldefinitivo From " +
					"(Select count(*) totalconteo1 from dbxschema.dinventariofisico where pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 1 AND dinv_conteo1 = dinv_msal_cantactual) TOT1, " +
					"(Select count(*) totalconteo2 from dbxschema.dinventariofisico where pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 2 AND dinv_conteo1 = dinv_conteo2) TOT2, " +
					"(Select count(*) totalconteo3 from dbxschema.dinventariofisico where pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 3) TOT3"));
			}
			catch { }

			return numeroTotalTarjetasEnConteoDefinitivo;
		}
	
		public static int NumeroTotalTarjetasAConteo(string prefijoInventario, int numeroInventario)
		{
			return NumeroTotalTarjetasAConteo1(prefijoInventario,numeroInventario) + NumeroTotalTarjetasAConteo2(prefijoInventario,numeroInventario) + NumeroTotalTarjetasAConteo3(prefijoInventario,numeroInventario);
		}
	
		public static int NumeroTotalTarjetas(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasAConteo = -1; 

			try
			{
				numeroTotalTarjetasAConteo = Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.dinventariofisico where pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString())) - NumeroTotalTarjetasAlta(prefijoInventario,numeroInventario);
			}
			catch { }

			return numeroTotalTarjetasAConteo;
		}
	
		public static int NumeroTotalTarjetasAlta(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasAConteo = -1; 

			try
			{
				numeroTotalTarjetasAConteo = Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.dinventariofisico where dinv_pubi_codigo is null AND pdoc_codigo = '"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()));
			}
			catch { }

			return numeroTotalTarjetasAConteo;
		}

		public static int ProximoNumeroTarjetaAConteo1(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasAConteo = -1; 

			try
			{
				numeroTotalTarjetasAConteo = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(MIN(dinv_tarjeta),-1) FROM dbxschema.dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 0"));
			}
			catch { }

			return numeroTotalTarjetasAConteo;
		}

		public static int ProximoNumeroTarjetaAConteo2(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasAConteo = -1; 

			try
			{
				numeroTotalTarjetasAConteo = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(MIN(dinv_tarjeta),-1) FROM dbxschema.dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 1 AND dinv_conteo1 <> dinv_msal_cantactual"));
			}
			catch { }

			return numeroTotalTarjetasAConteo;
		}

		public static int ProximoNumeroTarjetaAConteo3(string prefijoInventario, int numeroInventario)
		{
			int numeroTotalTarjetasAConteo = -1; 

			try
			{
				numeroTotalTarjetasAConteo = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(MIN(dinv_tarjeta),-1) FROM dbxschema.dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario.ToString()+" AND dinv_conteoactual = 2 AND dinv_conteo1 <> dinv_conteo2"));
			}
			catch { }

			return numeroTotalTarjetasAConteo;
		}

		public static int ProximoNumeroTarjetaAConteo(string prefijoInventario, int numeroInventario, int conteo)
		{
			int numeroTarjeta = -1;

			switch(conteo)
			{
				case 1:
					numeroTarjeta = ProximoNumeroTarjetaAConteo1(prefijoInventario,numeroInventario);
					break;
				case 2:
					numeroTarjeta = ProximoNumeroTarjetaAConteo2(prefijoInventario,numeroInventario);
					break;
				case 3:
					numeroTarjeta = ProximoNumeroTarjetaAConteo3(prefijoInventario,numeroInventario);
					break;
			}

			return numeroTarjeta;
		}

		#region Código candidato a eliminación
		/*
		public static int ProximoNumeroTarjetaAltaAConteo(string prefijoInventario, int numeroInventario)
		{
			int numeroTarjeta = -1;

			try
			{
				numeroTarjeta = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(MIN(dinv_tarjeta),-1) FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv = "+numeroInventario+" AND dinv_mite_codigo is null"));
			}
			catch { }

			return numeroTarjeta;
		}
		*/
		#endregion
		
		public static ArrayList ConteosPendientes(string prefijoInventario, int numeroInventario)
		{
			ArrayList conteosPendientes = new ArrayList();

			if(NumeroTotalTarjetasAConteo1(prefijoInventario,numeroInventario) > 0)
				conteosPendientes.Add(1);
			
			if(NumeroTotalTarjetasAConteo2(prefijoInventario,numeroInventario) > 0)
				conteosPendientes.Add(2);
			
			if(NumeroTotalTarjetasAConteo3(prefijoInventario,numeroInventario) > 0)
				conteosPendientes.Add(3);

			return conteosPendientes;
		}

		public static int ProximoNumeroTarjetaAltaNueva(string prefijoInventario, int numeroInventario)
		{
			int proximoNumeroTarjetaAltaNueva = -1; 

			try
			{
				proximoNumeroTarjetaAltaNueva = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(MAX(dinv_tarjeta),-1) + 1 FROM dinventariofisico WHERE pdoc_codigo='"+prefijoInventario+"' AND minf_numeroinv="+numeroInventario.ToString()));
			}
			catch { }

			return proximoNumeroTarjetaAltaNueva;
		}

		#endregion
	}
}
