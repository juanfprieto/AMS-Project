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
using AMS.Documentos;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ControlSustitucion : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataTable dtSustitucion;
		protected string prefijo, numero;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			prefijo = Request.QueryString["prefDoc"];
			numero = Request.QueryString["numDoc"];
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlLinea,"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				lbPrefijo.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
				lbNumero.Text = numero;
				lbFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
				tbCodOrigen.Attributes.Add("onkeyup","ItemMask("+tbCodOrigen.ClientID+","+ddlLinea.ClientID+");");
				tbCodOrigen.Attributes.Add("ondblclick","MostrarRefs1("+tbCodOrigen.ClientID+","+ddlLinea.ClientID+");");
				tbCodSust.Attributes.Add("onkeyup","ItemMask("+tbCodSust.ClientID+","+ddlLinea.ClientID+");");
				tbCodSust.Attributes.Add("ondblclick","MostrarRefs2("+tbCodSust.ClientID+","+ddlLinea.ClientID+","+ddlTipoSust.ClientID+");");
				ddlLinea.Attributes.Add("onchange","ChangeLine2("+ddlLinea.ClientID+","+tbCodOrigen.ClientID+","+tbCodSust.ClientID+");");
			}
			if(Session["dtSustitucion"] == null)
				PrepararDtSustitucion();
			else
				dtSustitucion = (DataTable)Session["dtSustitucion"];
		}
		
		public void EliminarRegistro(Object sender, DataGridCommandEventArgs e)
		{
			try
            {
        		dtSustitucion.Rows.Remove(dtSustitucion.Rows[e.Item.ItemIndex]);
	            dgSustitucion.EditItemIndex = -1;
			}catch(Exception ex){lb.Text += "<br>"+ex.ToString();};
			BindDgSustitucion();
		}
		
		protected void AgregarSustitucion(object Sender, EventArgs E)
		{
			//Revisamos que los TextBox donde se ingresan los codigos de las referencias no esten vacios
			if(tbCodOrigen.Text == "")
			{
                Utils.MostrarAlerta(Response, "El codigo del item de origen es vacio.\\nRevise Por Favor");
				return;
			}
			if(tbCodSust.Text == "")
			{
                Utils.MostrarAlerta(Response, "El codigo del item de sustitución es vacio.\\nRevise Por Favor");
				return;
			}
			string codOriI = "", codSusI = "";
			//Ahora revisamos si es valido los codigos ingresados son validos
			if(!Referencias.Guardar(tbCodOrigen.Text.Trim(),ref codOriI,(ddlLinea.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo del item de origen " + tbCodOrigen.Text.Trim() + " es no valido.\\nRevise Por Favor");
				return;
			}
			if(!Referencias.Guardar(tbCodSust.Text.Trim(),ref codSusI,(ddlLinea.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo del item de sustitución " + tbCodSust.Text.Trim() + " es no valido.\\nRevise Por Favor");
				return;
			}
			//Ahora Revisamos si los codigos de item existen dentro de la base de datos
			//El codigo de origen siempre debe existir, mientras que el de sustitucion solo debera existir cuando la sustitucion sea de unificacion
			if(!DBFunctions.RecordExist("SELECT * FROM mitems WHERE mite_codigo='"+codOriI+"'"))
			{
                Utils.MostrarAlerta(Response, "El codigo del item de origen " + tbCodOrigen.Text.Trim() + " no se encuentra registrado.\\nRevise Por Favor");
				return;
			}
			if(ddlTipoSust.SelectedValue == "U")
			{
				if(!DBFunctions.RecordExist("SELECT * FROM mitems WHERE mite_codigo='"+codSusI+"'"))
				{
                    Utils.MostrarAlerta(Response, "El codigo del item de sustitución " + tbCodSust.Text.Trim() + " no se encuentra registrado.\\nRevise Por Favor");
					return;
				}
			}
			else
			{
				if(DBFunctions.RecordExist("SELECT * FROM mitems WHERE mite_codigo='"+codSusI+"'"))
				{
                    Utils.MostrarAlerta(Response, "El codigo del item de sustitución " + tbCodSust.Text.Trim() + " se encuentra registrado.\\nRevise Por Favor");
					return;
				}
			}
			//Ahora revisamos que no se haya agregado ya una fila con este codigo de origen
			if((dtSustitucion.Select("CODORIGEN='"+tbCodOrigen.Text.Trim()+"'")).Length > 0)
			{
                Utils.MostrarAlerta(Response, "El codigo del item de origen " + tbCodOrigen.Text.Trim() + " ya se ha agregado a esta sustitución.\\nRevise Por Favor");
				return;
			}
			//Ahora si creamos la fila dentro de la tabla dtSustitucion
			DataRow fila = dtSustitucion.NewRow();
			fila[0] = ddlTipoSust.SelectedValue;
			fila[1] = tbCodOrigen.Text.Trim();
			fila[2] = codOriI;
			fila[3] = tbCodSust.Text.Trim();
			fila[4] = codSusI;
			fila[5] = ddlLinea.SelectedValue;
			dtSustitucion.Rows.Add(fila);
			BindDgSustitucion();
			tbCodOrigen.Text = tbCodSust.Text = "";
		}
		
		protected void CrearSustitucion(object Sender, EventArgs E)
		{
			if(dtSustitucion.Rows.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado ningun item para realizar sustituciones.\\nRevise Por Favor");
				return;
			}
			Sustitucion miSustitucion = new Sustitucion(prefijo,Convert.ToUInt32(numero),dtSustitucion);
			miSustitucion.Fecha = DateTime.Now.ToString("yyyy-MM-dd");
			miSustitucion.Usuario = HttpContext.Current.User.Identity.Name;
			if(miSustitucion.CrearSustitucion())
           	{
                //lb.Text += "<br>Bien "+miSustitucion.ProcessMsg;
                Utils.MostrarAlerta(Response, "La sustitucion ha sido realizada Satisfactoriamente !!! ");
                Response.Redirect(indexPage + "?process=Inventarios.AdminSustitucion");
			}
		    else
				lb.Text += "<br>Error "+miSustitucion.ProcessMsg;
		}
		
		#endregion
		
		protected void BindDgSustitucion()
		{
			Session["dtSustitucion"] = dtSustitucion;
			dgSustitucion.DataSource = dtSustitucion;
			dgSustitucion.DataBind();
		}
		
		protected void PrepararDtSustitucion()
		{
			dtSustitucion = new DataTable();
			dtSustitucion.Columns.Add(new DataColumn("CLASE",System.Type.GetType("System.String")));//0
			dtSustitucion.Columns.Add(new DataColumn("CODORIGEN",System.Type.GetType("System.String")));//1
			dtSustitucion.Columns.Add(new DataColumn("CODORIGENORIGINAL",System.Type.GetType("System.String")));//2
			dtSustitucion.Columns.Add(new DataColumn("CODSUSTI",System.Type.GetType("System.String")));//3
			dtSustitucion.Columns.Add(new DataColumn("CODSUSTIORIGINAL",System.Type.GetType("System.String")));//4
			dtSustitucion.Columns.Add(new DataColumn("LINEA",System.Type.GetType("System.String")));//5
		}
		
		#region Metodos
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
