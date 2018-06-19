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
using AMS.Forms;

namespace AMS.Tools
{
	public partial class Permisos : System.Web.UI.UserControl 
	{
		protected DataTable dtOpciones;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(perfiles,"SELECT ttipe_codigo, ttipe_descripcion FROM ttipoperfil WHERE ttipe_codigo<>'AS' order by ttipe_descripcion");
				this.Llenar_Tabla_Opciones();
			}
			else
			{
				if(Session["dtOpciones"]==null)
					this.Preparar_Tabla_Opciones();
				else
					dtOpciones = (DataTable)Session["dtOpciones"];
			}
		}
		
		protected void Preparar_Tabla_Opciones()
		{
			dtOpciones = new DataTable();
			dtOpciones.Columns.Add(new DataColumn("IDENTIFICADOR",System.Type.GetType("System.String")));
			dtOpciones.Columns.Add(new DataColumn("OPCION",System.Type.GetType("System.String")));
			dtOpciones.Columns.Add(new DataColumn("JERARQUIA",System.Type.GetType("System.String")));
		}
		
		protected void Llenar_Tabla_Opciones()
		{
			this.Preparar_Tabla_Opciones();
			DataSet consultaOpciones = new DataSet();
            DBFunctions.Request(consultaOpciones, IncludeSchema.NO, "SELECT smen_codigo, smen_opcion, smen_padre FROM smenu WHERE smen_url <> 'ND' AND smen_padre <> 'ND' order by smen_padre");
			for(int i=0;i<consultaOpciones.Tables[0].Rows.Count;i++)
			{
				DataRow fila = dtOpciones.NewRow();
				fila[0] = consultaOpciones.Tables[0].Rows[i][0].ToString();
				fila[1] = consultaOpciones.Tables[0].Rows[i][1].ToString();
				fila[2] = consultaOpciones.Tables[0].Rows[i][2].ToString()+"-"+DBFunctions.SingleData("SELECT smen_padre FROM smenu WHERE smen_opcion='"+consultaOpciones.Tables[0].Rows[i][2].ToString()+"'");//+"-"+DBFunctions.SingleData("SELECT smen_opcion FROM smenu WHERE smen_padre=(SELECT smen_padre FROM smenu WHERE smen_codigo="+consultaOpciones.Tables[0].Rows[i][2].ToString()+")");
				dtOpciones.Rows.Add(fila);
			}
			this.Bind_Opciones();
		}
		
		protected void Bind_Opciones()
		{
			Session["dtOpciones"] = dtOpciones;
			opcionesMenu.DataSource = dtOpciones;
			opcionesMenu.DataBind();
		}
		
		protected  void Generar_Permisos(Object  Sender, EventArgs e)
		{
			//Primero debemos eliminar de la base de datos los registros que tengan este tipo de perfil asociado
			//de la tabla SPERMISO
			ArrayList sqlStrings = new ArrayList();
			ArrayList opcionesEscogidas = new ArrayList();
			sqlStrings.Add("DELETE FROM spermiso WHERE ttipe_codigo='"+perfiles.SelectedValue+"'");
			for(int i=0;i<opcionesMenu.Items.Count;i++)
			{
				//Ahora revisamos si esta opcion esta activada
				if(((CheckBox)opcionesMenu.Items[i].Cells[3].Controls[1]).Checked)
				{
					//Ahora debemos de activar la opcion del menu que halla sido seleccionada
					sqlStrings.Add("INSERT INTO spermiso VALUES('"+perfiles.SelectedValue+"',"+dtOpciones.Rows[i][0].ToString()+")");
					//Ahora debemos de activar las dos opciones del menu padre de esta opcion
					string[] padres = dtOpciones.Rows[i][2].ToString().Split('-');
					//Primero debemos realizar la busqueda para la primer posicion del padres[0]
					if(!this.Buscar_Opcion_Registrada(opcionesEscogidas,DBFunctions.SingleData("SELECT smen_codigo FROM smenu WHERE smen_opcion='"+padres[0].Trim()+"'")))
					{
						sqlStrings.Add("INSERT INTO spermiso VALUES('"+perfiles.SelectedValue+"',"+DBFunctions.SingleData("SELECT smen_codigo FROM smenu WHERE smen_opcion='"+padres[0].Trim()+"'")+")");
						opcionesEscogidas.Add(DBFunctions.SingleData("SELECT smen_codigo FROM smenu WHERE smen_opcion='"+padres[0].Trim()+"'"));
					}
					//Busqueda en la posicion padres[1]
					if(padres[1]!="")
					{
						if(!this.Buscar_Opcion_Registrada(opcionesEscogidas,DBFunctions.SingleData("SELECT smen_codigo FROM smenu WHERE smen_opcion='"+padres[1].Trim()+"'")))
						{
							sqlStrings.Add("INSERT INTO spermiso VALUES('"+perfiles.SelectedValue+"',"+DBFunctions.SingleData("SELECT smen_codigo FROM smenu WHERE smen_opcion='"+padres[1].Trim()+"'")+")");
							opcionesEscogidas.Add(DBFunctions.SingleData("SELECT smen_codigo FROM smenu WHERE smen_opcion='"+padres[1].Trim()+"'"));
						}
					}
				}
			}
			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect("" + indexPage + "?process=Tools.Usuarios");
			else
				lb.Text += DBFunctions.exceptions + "<br>";
		}
		
		protected bool Buscar_Opcion_Registrada(ArrayList opcionesEscogidas, string codigoOpcion)
		{
			bool encontrado = false;
			for(int i=0;i<opcionesEscogidas.Count;i++)
			{
				if(opcionesEscogidas[i].ToString()==codigoOpcion)
					encontrado = true;
			}
			return encontrado;
		}
		
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
