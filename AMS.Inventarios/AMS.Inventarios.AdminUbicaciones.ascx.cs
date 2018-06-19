using System;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class AdminUbicaciones : System.Web.UI.UserControl
	{
		#region Atributos
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string itemubicacion;

        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
		{
            itemubicacion = Request.QueryString["codItem"];

            if (!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
                if (ddlAlmacen.Items.Count > 1)
                    ddlAlmacen.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(ddlAlmacenCfg, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
                if (ddlAlmacenCfg.Items.Count > 1)
                    ddlAlmacenCfg.Items.Insert(0, "Seleccione..");
                else if (ddlAlmacen.Items.Count == 1)
                        bind.PutDatasIntoDropDownList(ddlUbicacionCfg,"SELECT pubi_codigo, pubi_nombre FROM pubicacionitem WHERE pubi_codpad IS NULL AND palm_almacen='"+ddlAlmacenCfg.SelectedValue+"'");
			}
		}
		
		protected void CambioAlmacenConfiguracion(object Sender, EventArgs E)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlUbicacionCfg,"SELECT pubi_codigo, pubi_nombre FROM pubicacionitem WHERE pubi_codpad IS NULL AND palm_almacen='"+ddlAlmacenCfg.SelectedValue+"'");
		}
		
		protected void CrearUbicacionNivelUno(object Sender, EventArgs E)
		{
			//Creamos un registro de nivel uno en la tabla de pubicacionitem
			if(tbNombUbicacion.Text == "")
			{
                Utils.MostrarAlerta(Response, "No se ha ingresado un nombre para la nueva ubicación!");
				return;
			}
			if(DBFunctions.NonQuery("INSERT INTO pubicacionitem (pubi_codigo, pubi_nombre, pubi_codpad, pubi_ubicespacial, palm_almacen) VALUES(default,'"+this.tbNombUbicacion.Text+"',null,null,'"+this.ddlAlmacen.SelectedValue+"')") == 1)
				Response.Redirect(indexPage + "?process=Inventarios.AdminUbicaciones");
			else
				lb.Text += "<br>"+DBFunctions.exceptions;
		}
		
		protected void ConfigurarUbicacion(object Sender, EventArgs E)
		{
			//Revisamos que se tenga seleccionado alguna ubicacion de nivel uno 
			if(ddlUbicacionCfg.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna ubicación de nivel 1!");
				return;
			}

			Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+ddlUbicacionCfg.SelectedValue+ "&codItem="+itemubicacion+ "");
		}
		
		protected void EliminarUbicacion(object sender, System.EventArgs e)
		{
			if(ddlUbicacionCfg.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna bodega!");
				return;
			}

			Response.Redirect(indexPage + "?process=Inventarios.EliminarUbicaciones&codUbi="+ddlUbicacionCfg.SelectedValue+"&tipUbi=bodega");
		}
		#endregion
		
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
