using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
	/// <summary>
	///		Clase controladora del control de usuario CambiarVisibilidad,
	///		proporciona metodos para cambiar la visibilidad de una opción o
	///		varias opciones del menú.
	/// </summary>
	public partial class CambiarVisibilidad : System.Web.UI.UserControl
	{
		protected Seleccionar Seleccion = new Seleccionar();

		/// <summary>
		/// Evento que se consume cada vez que se recarga la pagina, nos da
		/// la posibilidad de ejecutar instrucciones en el momento de carga
		/// de la pagina.
		/// </summary>
		/// <param name="sender">object Objeto que hizo la petición</param>
		/// <param name="e">EventArgs manejador de argumentos de eventos</param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Seleccion.ECargarG += new Seleccionar.DCargarG(Seleccion_ECargarG);
			Seleccion.AsignarEstados(true,true,true,true);
			Seleccion.AsignarTextoLabels("Visibles","No Visibles");
			
			if (!IsPostBack)
			{
				Cargar();
				BindGrid();
			}
		}

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

		/// <summary>
		/// Carga los ListBox del control de usuario Seleccionar con las consultas
		/// especificadas
		/// </summary>
		private void BindGrid()
		{
			DataSet ds = new DataSet();
			DataSet ds1 = new DataSet();

			string Sql = "Select smen_codigo as Codigo, smen_padre concat ' - ' concat smen_opcion as Nombre from DBXSCHEMA.SMENU where smen_codigo in (select smen_codigo from dbxschema.spermiso where ttipe_codigo = '" + Perfil.SelectedValue.ToString() + "' and ttipe_visible = 'S') ORDER BY Nombre;";

			DBFunctions.Request(ds,IncludeSchema.NO,Sql);
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				Seleccion.CargarOrigen(i,ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][0].ToString(),"Rojo");
			}

			       Sql = "Select smen_codigo as Codigo, smen_padre concat ' - ' concat smen_opcion as Nombre from DBXSCHEMA.SMENU where smen_codigo in (select smen_codigo from dbxschema.spermiso where ttipe_codigo = '" + Perfil.SelectedValue.ToString() + "' and ttipe_visible = 'N') ORDER BY Nombre;";

			DBFunctions.Request(ds1,IncludeSchema.NO,Sql);
			for(int i=0;i<ds1.Tables[0].Rows.Count;i++)
			{
				Seleccion.CargarDestino(i,ds1.Tables[0].Rows[i][1].ToString(),ds1.Tables[0].Rows[i][0].ToString(),"Rojo");
			}
			ds.Dispose();
			ds1.Dispose();
		}

		/// <summary>
		/// Enlaza el dropdownlist Perfil con la consulta especificada
		/// </summary>
		private void Cargar()
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(Perfil,"SELECT ttipe_codigo ,ttipe_descripcion FROM ttipoperfil order by ttipe_descripcion");
		}

		/// <summary>
		/// Función que ejecutara el delegado DCargarG del control de usuario
		/// Seleccionar. Actualiza la visibilidad de las opciones del menú
		/// contenidas en los ListBox
		/// </summary>
		/// <param name="Inicio">ListBox contiene los permisos con visibilidad asignados al perfil seleccionado</param>
		/// <param name="Final">ListBox contiene los permisos sin visibilidad asignados al perfil seleccionado</param>
		private void Seleccion_ECargarG(ListBox Inicio, ListBox Final)
		{
			string sql = "";
			string quedan = "";
			int x;
			int retorno = 0;
			int totinsert = 0;
			for(x=0;x<Final.Items.Count;x++)
			{
				quedan  = Final.Items[x].Value.ToString().Trim();
				try
				{
					sql = "Update spermiso set ttipe_visible = 'N' where ttipe_codigo = '"+ Perfil.SelectedValue.ToString() + "' and smen_codigo = " + quedan;
					retorno = DBFunctions.NonQuery(sql);
					totinsert++;
				}
				catch (Exception Ex)
				{
					Mensages.Text = "Error " + Ex.Message;
				}
			}
			for(x=0;x<Inicio.Items.Count;x++)
			{
				quedan  = Inicio.Items[x].Value.ToString().Trim();
				try
				{
					sql = "Update spermiso set ttipe_visible = 'S' where ttipe_codigo = '"+ Perfil.SelectedValue.ToString() + "' and smen_codigo = " + quedan;
					retorno = DBFunctions.NonQuery(sql);
					totinsert++;
				}
				catch (Exception Ex)
				{
					Mensages.Text = "Error " + Ex.Message;
				}
			}
			Mensages.Text = "Se Actualizaron " + totinsert.ToString() + " Registros";
		}

		/// <summary>
		/// Evento que se ejecuta cuando se cambia el perfil seleccionado
		/// </summary>
		/// <param name="sender">object Objeto que hizo la petición</param>
		/// <param name="e">EventArgs manejador de argumentos de eventos</param>
		protected void Entidad_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Seleccion.LimpiarTodo();
			BindGrid();
		}
	}
}
