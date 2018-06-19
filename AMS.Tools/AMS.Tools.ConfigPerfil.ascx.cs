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
	///		Descripción breve de AMS_Tools_ConfigPerfil.
	/// </summary>
	public partial class ConfigPerfil : System.Web.UI.UserControl
	{
		protected Seleccionar Seleccion = new Seleccionar();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Seleccion.ECargarG += new Seleccionar.DCargarG(Seleccion_ECargarG);
			Seleccion.AsignarEstados(true,true,true,true);
			Seleccion.AsignarTextoLabels("Sin Asignar","Asignado");
			
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

		private void BindGrid()
		{
			DataSet ds = new DataSet();
			DataSet ds1 = new DataSet();

			string Sql = "Select smen_codigo as Codigo, smen_padre concat ' - ' concat smen_opcion as Nombre from DBXSCHEMA.SMENU where smen_codigo not in (select smen_codigo from dbxschema.spermiso where ttipe_codigo = '" + Perfil.SelectedValue.ToString() + "') ORDER BY Nombre;";

			DBFunctions.Request(ds,IncludeSchema.NO,Sql);
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				Seleccion.CargarOrigen(i,ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][0].ToString(),"Rojo");
			}

			Sql = "Select smen_codigo as Codigo, smen_padre concat ' - ' concat smen_opcion as Nombre from DBXSCHEMA.SMENU where smen_codigo in (select smen_codigo from dbxschema.spermiso where ttipe_codigo = '" + Perfil.SelectedValue.ToString() + "') ORDER BY Nombre;";

			DBFunctions.Request(ds1,IncludeSchema.NO,Sql);
			for(int i=0;i<ds1.Tables[0].Rows.Count;i++)
			{
				Seleccion.CargarDestino(i,ds1.Tables[0].Rows[i][1].ToString(),ds1.Tables[0].Rows[i][0].ToString(),"Rojo");
			}
			ds.Dispose();
			ds1.Dispose();
		}

		private void Cargar()
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(Perfil,"SELECT ttipe_codigo ,ttipe_descripcion FROM ttipoperfil");
		}

		protected void Entidad_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Seleccion.LimpiarTodo();
			BindGrid();
		}

		private void Seleccion_ECargarG(ListBox Inicio, ListBox Final)
		{
			string sql = "";
			string estaban = "";
			string quedan = "";
			int i,x;
			int retorno = 0;
			int totinsert = 0;
			int totdelete = 0;
			bool eliminar = false;
			bool adicionar = false;
			bool seguir = true;

			DataSet ds = new DataSet();

			sql = "Select smen_codigo from DBXSCHEMA.SMENU where smen_codigo in (select smen_codigo from dbxschema.spermiso where ttipe_codigo = '" + Perfil.SelectedValue.ToString() + "') ORDER BY smen_codigo;";

			DBFunctions.Request(ds,IncludeSchema.NO,sql);
			
			for(x=0;x<Final.Items.Count;x++)
			{
				adicionar = true;
				quedan  = Final.Items[x].Value.ToString().Trim();


				if (ds.Tables[0].Rows.Count > 0)
				{
					for(i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						estaban = ds.Tables[0].Rows[i][0].ToString().Trim();
						if ( quedan == estaban)
						{	
							adicionar = false;
						}
					}
					if (adicionar)
					{
						try
						{
							sql = "Insert into spermiso values ('"+ Perfil.SelectedValue.ToString() + "'," + quedan + ",1,'S')";
							retorno = DBFunctions.NonQuery(sql);
							totinsert++;
						}
						catch (Exception Ex)
						{
							lb.Text = "Error " + Ex.Message;
							seguir = false;
						}
					}
				}
				else
				{
					try
					{
						sql = "Insert into spermiso values ('"+ Perfil.SelectedValue.ToString() + "'," + quedan + ",1,'S')";
						retorno = DBFunctions.NonQuery(sql);
						totinsert++;
					}
					catch (Exception Ex)
					{
						lb.Text = "Error " + Ex.Message;
						seguir = false;
					}
				}
			}		
			if (seguir)
			{
				if (ds.Tables[0].Rows.Count > 0)
				{
					for(x=0;x<ds.Tables[0].Rows.Count;x++)
					{
						eliminar = true;

						for(i=0;i<Final.Items.Count;i++)
						{
							estaban = ds.Tables[0].Rows[x][0].ToString().Trim();
							quedan  = Final.Items[i].Value.ToString().Trim();

							if (estaban == quedan)
							{	
								eliminar = false;
							}
						}
						if (eliminar)
						{
							try
							{
								sql = "Delete from spermiso where ttipe_codigo = '"+ Perfil.SelectedValue.ToString() + "' and smen_codigo = " + estaban;
								retorno = DBFunctions.NonQuery(sql);
								totdelete++;
							}
							catch (Exception Ex)
							{
								lb.Text = "Error " + Ex.Message;
							}
						}
					}
				}
			}
			if (seguir)
			{
				lb.Text = "Se Insertaron " + totinsert.ToString() + " Registros y se Borraron " + totdelete.ToString() + " Registros";
			}
		}
	}
}
