using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Inventarios;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Inventarios
{
	/// <summary>
	///		Descripci�n breve de CrearInventarioFisico.
	/// </summary>
	public partial class CrearInventarioFisico : System.Web.UI.UserControl
	{
		#region Atributos
		protected int num;
		protected System.Web.UI.WebControls.DropDownList DropDownList5;
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			//Aqui se carga el prefijo de el inventario fisico, el tipo de inv y el tipo de ubicacion
			if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				bind.PutDatasIntoDropDownList(ddltipo,"Select tift_codigo,tift_descripcion from TINVENTARIOFISICOTIPO");
				bind.PutDatasIntoDropDownList(ddltipoubic,"Select ttifu_codigo,ttifu_descripcion from TINVENTARIOFISICOUBICACION");
                //bind.PutDatasIntoDropDownList(ddlPrefijo, "select pdoc_codigo,pdoc_nombre from pdocumento where tdoc_tipodocu='IF' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref ddlPrefijo , "%", "%", "IF");
				NumeroInv.Text = DBFunctions.SingleData("select pdoc_ultidocu + 1 from pdocumento where pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");
				ddltipoubic.SelectedIndex=0;
			}
		}

		protected void botoncrear_Click(object sender, System.EventArgs e)
		{
			// Esta funcion guarda en minventario la creacion de un nuevo inventario fisico
			if(!DBFunctions.RecordExist("select minf_numeroinv from minventariofisico where pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND minf_numeroinv="+NumeroInv.Text))
			{
				string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                if (costo.Text == "") 
                { 
                    Utils.MostrarAlerta(Response, "No se ha ingresado el costo del contador por d�a..");
                    return;
                }
				InventarioFisico invInstancia = new InventarioFisico(ddlPrefijo.SelectedValue,Convert.ToInt32(NumeroInv.Text),cldFechInventario.SelectedDate,ddltipoubic.SelectedValue,ddltipo.SelectedValue,Convert.ToDouble(costo.Text),HttpContext.Current.User.Identity.Name);
				if(invInstancia.AlmacenarRegistroInicioInventarioFisico())
					Response.Redirect("" + indexPage + "?process=Inventarios.RolesInventario&tiqs="+invInstancia.PrefijoInventario+"&tiq="+invInstancia.NumeroInventario+"&tipo="+invInstancia.CodigoTipoInventarioTipo+"");
				else
                    Utils.MostrarAlerta(Response, "Se ha presentado un error en la creaci�n del inventario fisico!");
			}
			else
                Utils.MostrarAlerta(Response, "Este Inventario ya ha sido Creado");
		}

		protected void ddlPrefijo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			NumeroInv.Text = DBFunctions.SingleData("select pdoc_ultidocu + 1 from pdocumento where pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");
		}

		#endregion
		
		#region C�digo generado por el Dise�ador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		M�todo necesario para admitir el Dise�ador. No se puede modificar
		///		el contenido del m�todo con el editor de c�digo.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
