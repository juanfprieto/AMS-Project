using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;

namespace AMS.Tools
{
	public partial class SesionesAbiertas : System.Web.UI.UserControl 
	{
		protected DataTable tablaSesiones;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//Vamos a llenar la grilla con las sesiones que se encuentren abiertas
			if (!IsPostBack)
			{
				this.Llenar_Grilla_Sesiones();
			}
			if(Session["tablaSesiones"]==null)
				this.Preparar_Tabla_Sesiones();
			else if(Session["tablaSesiones"]!=null) 
				tablaSesiones = (DataTable)Session["tablaSesiones"];
		}
		
		protected void Preparar_Tabla_Sesiones()
		{
			tablaSesiones = new DataTable();
			tablaSesiones.Columns.Add(new DataColumn("NOMBUSUARIO",System.Type.GetType("System.String")));
			tablaSesiones.Columns.Add(new DataColumn("LOGIN",System.Type.GetType("System.String")));
			tablaSesiones.Columns.Add(new DataColumn("DIRECCIONIP",System.Type.GetType("System.String")));
		}
		
		protected void Llenar_Grilla_Sesiones()
		{
			this.Preparar_Tabla_Sesiones();
			DataSet sesionesAbiertasBD = new DataSet();
            DBFunctions.Request(sesionesAbiertasBD, IncludeSchema.NO, "SELECT * FROM susuario WHERE susu_flagingr='L' AND susu_tipcrea<>'D' AND susu_login<>'" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'  order by susu_nombre;");
			for(int i=0;i<sesionesAbiertasBD.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaSesiones.NewRow();
				fila["NOMBUSUARIO"] = sesionesAbiertasBD.Tables[0].Rows[i][1].ToString();
				fila["LOGIN"] = sesionesAbiertasBD.Tables[0].Rows[i][2].ToString();
				fila["DIRECCIONIP"] = sesionesAbiertasBD.Tables[0].Rows[i][5].ToString();
				tablaSesiones.Rows.Add(fila);
			}
			Session["tablaSesiones"] = tablaSesiones;
			sesionesAbiertas.DataSource = tablaSesiones;
			sesionesAbiertas.DataBind();
		}
		
		protected void dgCerrar_Sesion_Usuario(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "cerrarSesion")
			{
				DBFunctions.NonQuery("UPDATE susuario SET susu_flagingr='N', susu_ipaddr=null WHERE susu_nombre='"+tablaSesiones.Rows[e.Item.ItemIndex][0].ToString()+"' AND susu_login='"+tablaSesiones.Rows[e.Item.ItemIndex][1].ToString()+"'");
				tablaSesiones.Rows[e.Item.ItemIndex].Delete();
				Session["tablaSesiones"] = tablaSesiones;
				sesionesAbiertas.DataSource = tablaSesiones;
				sesionesAbiertas.DataBind();
			}
		}
		
		protected  void Actualizar_Grilla(Object  Sender, EventArgs e)
		{
			this.Llenar_Grilla_Sesiones();
		}
		
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
