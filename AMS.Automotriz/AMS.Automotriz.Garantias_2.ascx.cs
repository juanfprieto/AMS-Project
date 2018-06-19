
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using Ajax;

namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_Garantias_2.
	/// </summary>
	public partial class AMS_Automotriz_Garantias_2 : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{				 
				//se cargan los datos para los drpdownlist que traen informacion de la base de datos
				DatasToControls bind = new DatasToControls();
				FechaActual.Text=DateTime.Now.ToString("yyyy-MM-dd");
				if(ddlOperaciones.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(ddlOperaciones,"Select ptem_operacion, pTem_descripcion from DBXSCHEMA.PTEMPARIO ORDER BY PTEM_descripcion");
					ListItem first=new ListItem("--Seleccione una Operación Por Favor--","0");
					ddlOperaciones.Items.Insert(0,first);
				}
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

		protected void Agregar_O_Click(object sender, System.EventArgs e)
		{
			//Agregar a la grilla
		}
	}
}
