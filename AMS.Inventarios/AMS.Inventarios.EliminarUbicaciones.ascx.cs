using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Ajax;
using AMS.Forms;
using AMS.DB;

namespace AMS.Inventarios
{
	public partial class AMS_Inventarios_EliminarUbicaciones : System.Web.UI.UserControl
	{
		#region Constantes
		private const string  sql_items_ubicaciones = @"select
	  mubi.mite_codigo,
	  mitems.mite_nombre,
	  mubi.pubi_codigo,
	  pubi.pubi_nombre,
	  pubi.pubi_codpad,
	  pubi.palm_almacen
from
	  dbxschema.mubicacionitem mubi 
	  inner join
	  		dbxschema.mitems mitems 
	  		on
			  mubi.mite_codigo = mitems.mite_codigo
	  inner join 
	  		dbxschema.pubicacionitem pubi
	  		on
	  		  pubi.pubi_codigo = mubi.pubi_codigo
where 
	   palm_almacen = ANY(
	 			  Select 
				  	  palm_almacen 
				  from
				  	  DBXSCHEMA.PUBICACIONITEM 
				  where 
				  	  pubi_codigo = {0})
order by
	  mubi.mite_codigo asc";

		private const string sql_ubicaciones_almacen = @"Select 
	   pubi_codigo,
	   pubi_nombre,
	   pubi_codpad,
	   palm_almacen 
from 
	   DBXSCHEMA.PUBICACIONITEM
where 
	   palm_almacen = ANY(
	 			  Select 
				  	  palm_almacen 
				  from
				  	  DBXSCHEMA.PUBICACIONITEM 
				  where 
				  	  pubi_codigo = {0})";
		#endregion

		#region Atributos

		protected DataTable tablaItems;
		protected string strValorDisplayUbicaciones;
		protected System.Web.UI.WebControls.Button btnEliminar;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Registra la clase en Ajax.
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Inventarios_EliminarUbicaciones));	

			if(!IsPostBack)
			{
				string codigo = Request.QueryString["codUbi"];
				string tipoUbicacion = Request.QueryString["tipUbi"];
				string ubicacionEspacial = string.Empty;

				hdnCodigo.Value = codigo;
				hdnTipoUbicacion.Value = tipoUbicacion;
	
				if ((tipoUbicacion == "estante") || (tipoUbicacion == "cajon"))
				{
					ubicacionEspacial = Request.QueryString["ubiEsp"];
					hdnUbicacionEspacial.Value = ubicacionEspacial;
				}

				PublicarUbicacion(codigo,tipoUbicacion,ubicacionEspacial);
			}
		}
		
		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.AdminUbicaciones");
		}
		#endregion
		
		#region Metodos
		protected void Preparar_Tabla_Items()
		{
			tablaItems = new DataTable();
			tablaItems.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			tablaItems.Columns.Add(new DataColumn("BODEGA",System.Type.GetType("System.String"))); //1
			tablaItems.Columns.Add(new DataColumn("ESTANTE",System.Type.GetType("System.String")));//2
			tablaItems.Columns.Add(new DataColumn("CAJON",System.Type.GetType("System.String")));//3
			tablaItems.Columns.Add(new DataColumn("ITEM",System.Type.GetType("System.String")));//4
		}

		protected void ingresar_datos_Operaciones(string codigo,string bodega,string estante,string cajon,string item)
		{
			DataRow fila = tablaItems.NewRow();

			fila["CODIGO"]=codigo;
			fila["BODEGA"]=bodega;
			fila["ESTANTE"]=estante;
			fila["CAJON"]=cajon;
			fila["ITEM"]=item;

			tablaItems.Rows.Add(fila);

			dgitems.DataSource= tablaItems;
			dgitems.DataBind();

			DatasToControls.Aplicar_Formato_Grilla(dgitems);
			DatasToControls.JustificacionGrilla(dgitems,tablaItems);
		}

		protected void publicar_Ubicaciones_Bodega(string pubi_codigo)
		{
			// Esta función genera los items que se encuentran almacenados en un determinado almacen.
			this.Preparar_Tabla_Items();

			DataSet ubicacionesAlmacen = new DataSet();

			// Carga todas las ubicacionesAlmacen del almacen.
			DBFunctions.Request(ubicacionesAlmacen,IncludeSchema.NO,String.Format(sql_ubicaciones_almacen,pubi_codigo));

			DataTable ubicacionesAlmacenTabla = ubicacionesAlmacen.Tables[0];

			DataRow[] bodegas = ubicacionesAlmacenTabla.Select("PUBI_CODIGO=" + pubi_codigo);

			if (bodegas.Length > 0)
			{
				DataRow bodega = bodegas[0];

				lbPregunta.Text = "¿Confirma que desea eliminar la ubicación [" + bodega[1].ToString() + "] seleccionada?";

				this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">" + bodega[0].ToString() + "</p>","<p style=\"COLOR: red\">" + bodega[1].ToString() + "</p>","","","");
						
				DataRow[] estantes = ubicacionesAlmacenTabla.Select("PUBI_CODPAD=" + bodega[0].ToString());
						
				for (int j = 0; j < estantes.Length; j++)
				{
					this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">" + estantes[j][0].ToString()+"</p>","","<p style=\"COLOR: red\">" + estantes[j][1].ToString()+"</p>","","");		
							
					DataRow[] cajones = ubicacionesAlmacenTabla.Select("PUBI_CODPAD=" + estantes[j][0].ToString());

					for (int y = 0; y < cajones.Length; y++)
					{
						this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">" + cajones[y][0].ToString()+"</p>","","",cajones[y][1].ToString(),"");		
					}
				}
			}
		}
		
		protected void publicar_Ubicaciones_Estante(string pubi_codigo, string pubi_ubicespacial)
		{
			// Esta función genera los items que se encuentran almacenados en un determinado almacen.
			this.Preparar_Tabla_Items();

			string codigoEstante = DBFunctions.SingleData("Select pubi_codigo FROM pubicacionitem WHERE pubi_codpad="+pubi_codigo+" AND pubi_ubicespacial='"+pubi_ubicespacial+"'");

			DataSet ubicacionesAlmacen = new DataSet();

			// Carga todas las ubicacionesAlmacen del almacen.
			DBFunctions.Request(ubicacionesAlmacen,IncludeSchema.NO,String.Format(sql_ubicaciones_almacen,codigoEstante));

			DataTable ubicacionesAlmacenTabla = ubicacionesAlmacen.Tables[0];

			DataRow[] estantes = ubicacionesAlmacenTabla.Select("PUBI_CODIGO=" + codigoEstante);

			if (estantes.Length > 0)
			{
				DataRow estante = estantes[0];

				lbPregunta.Text = "¿Confirma que desea eliminar la ubicación [" + estante[1].ToString() + "] seleccionada?";

				this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">" + estante[0].ToString() + "</p>","","<p style=\"COLOR: red\">" + estante[1].ToString()+"</p>","","");		
							
				DataRow[] cajones = ubicacionesAlmacenTabla.Select("PUBI_CODPAD=" + estante[0].ToString());

				for (int y = 0; y < cajones.Length; y++)
				{
					this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">" + cajones[y][0].ToString()+"</p>","","",cajones[y][1].ToString(),"");		
				}
			}
		}
		
		protected void publicar_Ubicaciones_Cajon(string pubi_codigo)
		{
			// Esta función genera los items que se encuentran almacenados en un determinado almacen.
			this.Preparar_Tabla_Items();

			DataSet ubicacionesAlmacen = new DataSet();

			// Carga todas las ubicaciones del almacen.
			DBFunctions.Request(ubicacionesAlmacen,IncludeSchema.NO,String.Format(sql_ubicaciones_almacen,pubi_codigo));

			DataTable ubicacionesAlmacenTabla = ubicacionesAlmacen.Tables[0];

			DataSet itemsCajon = new DataSet();

			// Carga todas los items del almacen.
			DBFunctions.Request(itemsCajon,IncludeSchema.NO,String.Format(sql_items_ubicaciones,pubi_codigo));

			DataTable itemsCajonTabla = itemsCajon.Tables[0];

			DataRow[] cajones = ubicacionesAlmacenTabla.Select("PUBI_CODIGO=" + pubi_codigo);

			if (cajones.Length > 0)
			{
				DataRow cajon = cajones[0];

				this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">" + cajon[0].ToString() + "</p>","","","<p style=\"COLOR: red\">" + cajon[1].ToString()+"</p>","");		
							
				lbPregunta.Text = "¿Confirma que desea eliminar la ubicación [" + cajon[1].ToString() + "] seleccionada?";

				DataRow[] items = itemsCajonTabla.Select("PUBI_CODIGO=" + cajon[0].ToString());

				for (int y = 0; y < items.Length; y++)
				{
					this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">" + items[y][0].ToString()+"</p>","","","",items[y][1].ToString());		
				}
			}
		}
		
		protected void PublicarUbicacion(string codigo, string tipoUbicacion, string ubicacionEspacial)
		{
			switch (tipoUbicacion)
			{
				case "bodega":
					publicar_Ubicaciones_Bodega(codigo);
					break;
				case "estante":
					publicar_Ubicaciones_Estante(codigo,ubicacionEspacial);
					break;
				case "cajon":
					publicar_Ubicaciones_Cajon(ubicacionEspacial);
					break;
			}
		}
		
		protected string eliminar_Ubicacion_Bodega(string pubi_codigo)
		{
			string url = string.Empty;

			ArrayList sqlStrings = new ArrayList();
			
			// Eliminamos los items almacenados en el cajón dependientes de la ubicación.
			sqlStrings.Add("DELETE FROM mubicacionitem WHERE pubi_codigo IN (SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad IN (SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad=" + pubi_codigo + "))");
			
			// Ahora eliminamos los cajones dependientes de la ubicación.
			sqlStrings.Add("DELETE FROM pubicacionitem WHERE pubi_codpad IN (SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad=" + pubi_codigo + ")");
			
			// Ahora eliminamos los estantes dependientes de la ubicación.
			sqlStrings.Add("DELETE FROM pubicacionitem WHERE pubi_codpad=" + pubi_codigo + "");
			
			// Ahora eliminamos la bodega.
			sqlStrings.Add("DELETE FROM pubicacionitem WHERE pubi_codigo= " + pubi_codigo + "");
			
			if(DBFunctions.Transaction(sqlStrings))
				url = indexPage + "?process=Inventarios.AdminUbicaciones";
			else
				lb.Text += "<br>ERROR "+DBFunctions.exceptions;
			
			return url;
		}
		
		protected string eliminar_Ubicacion_Estante(string pubi_codigo, string pubi_ubicespacial)
		{
			string url = string.Empty;

			ArrayList sqlStrings = new ArrayList();

			//Primero debemos eliminar los registros de mubicacionitem que tengan ubicaciones de nivel 3 de este nivel 2
			sqlStrings.Add("DELETE FROM mubicacionitem WHERE pubi_codigo IN (SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad=(SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad="+pubi_codigo+" AND pubi_ubicespacial='"+pubi_ubicespacial+"'))");

			//Ahora eliminamos las ubicaciones de nivel 3 relacionadas con esta ubicacion de nivel 2
			sqlStrings.Add("DELETE FROM pubicacionitem WHERE pubi_codpad=(SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad="+pubi_codigo+" AND pubi_ubicespacial='"+pubi_ubicespacial+"')");
			
			//Ahora eliminamos la ubicacion de nivel 2
			sqlStrings.Add("DELETE FROM pubicacionitem WHERE pubi_codpad="+pubi_codigo+" AND pubi_ubicespacial='"+pubi_ubicespacial+"'");
			
			if(DBFunctions.Transaction(sqlStrings))
				url = indexPage + "?process=Inventarios.CfgUbicaciones&codUbi=" + pubi_codigo;
			else
				lb.Text += "<br>ERROR "+DBFunctions.exceptions;

			return url;
		}
		
		protected string eliminar_Ubicacion_Cajon(string pubi_codigo, string pubi_ubicespacial)
		{
			string url = string.Empty;

			ArrayList sqlStrings = new ArrayList();
			
			//Eliminamos los registros de mubicacionitem que esten relacionados con esta ubicacion
			sqlStrings.Add("DELETE FROM mubicacionitem WHERE pubi_codigo="+pubi_ubicespacial+"");
			
			//Ahora eliminamos la ubicacion
			sqlStrings.Add("DELETE FROM pubicacionitem WHERE pubi_codigo="+pubi_ubicespacial+"");
						
			if(DBFunctions.Transaction(sqlStrings))
				url = indexPage + "?process=Inventarios.CfgUbicaciones&codUbi=" + pubi_codigo;
			else
				lb.Text += "<br>ERROR "+DBFunctions.exceptions;

			return url;
		}
		
		[Ajax.AjaxMethod()]
		public string EliminarUbicacion(string codigo, string tipoUbicacion, string ubicacionEspacial)
		{
			string url = string.Empty;

			switch (tipoUbicacion)
			{
				case "bodega":
					url = eliminar_Ubicacion_Bodega(codigo);
					break;
				case "estante":
					url = eliminar_Ubicacion_Estante(codigo,ubicacionEspacial);
					break;
				case "cajon":
					url = eliminar_Ubicacion_Cajon(codigo,ubicacionEspacial);
					break;
			}

			return url;
		}

		#endregion

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
