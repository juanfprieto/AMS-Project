
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools; 

namespace AMS.Automotriz
{

	public partial class AdminKitsCombos : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlGrupos, "SELECT pgru_grupo, pgru_nombre CONCAT '  _  ' CONCAT pgru_grupo FROM pgrupocatalogo ORDER BY pgru_nombre, pgru_grupo");
                bind.PutDatasIntoDropDownList(ddlListasPrecios, "SELECT ppre_codigo, ppre_nombre FROM pprecioitem ORDER BY ppre_nombre");
                bind.PutDatasIntoDropDownList(ddlKitsEdit, "SELECT pkit_codigo, pkit_codigo CONCAT ' - ' CONCAT pkit_nombre CONCAT ' Grupo ' concat pgru_grupo FROM pkit ORDER BY pkit_nombre, PKIT_CODIGO");
                if (ddlKitsEdit.Items.Count > 1)
                    ddlKitsEdit.Items.Insert(0, "Seleccione:..");
            //    bind.PutDatasIntoDropDownList(ddlKitsDelete,"SELECT pkit_codigo, pkit_codigo CONCAT ' - ' CONCAT pkit_nombre FROM dbxschema.pkit WHERE pkit_codigo NOT IN(SELECT PKIT.pkit_codigo FROM dbxschema.pkit PKIT,dbxschema.morden MORD,dbxschema.dordenkit DORD WHERE DORD.pkit_codigo=PKIT.pkit_codigo AND DORD.pdoc_codigo=MORD.pdoc_codigo AND DORD.mord_numeorde=MORD.mord_numeorde) ORDER BY pkit_nombre, PKIT_CODIGO");
                bind.PutDatasIntoDropDownList(ddlKitsDelete, "SELECT pkit_codigo, pkit_codigo CONCAT ' - ' CONCAT pkit_nombre CONCAT ' Grupo ' concat pgru_grupo FROM dbxschema.pkit ORDER BY pkit_nombre, PKIT_CODIGO");
                if (ddlKitsDelete.Items.Count > 1)
                    ddlKitsDelete.Items.Insert(0, "Seleccione:..");
            }
		}
		
		protected void Ingresar_Kits(Object  Sender, EventArgs e)
		{
			//Revisamos si este kit ya fue creado o no
			if(DBFunctions.RecordExist("SELECT * FROM pkit WHERE pkit_codigo='"+tbCodigoKit.Text+"'"))
                Utils.MostrarAlerta(Response, "Codigo de Kit Repetido. Revise Por Favor");
			else
                Response.Redirect("" + indexPage + "?process=Automotriz.ControlKit&tipo=Nuevo&codigo=" + tbCodigoKit.Text + "&nombre=" + tbNombreKit.Text + "&grupo=" + ddlGrupos.SelectedValue + "&lista=" + ddlListasPrecios.SelectedValue + "&kilometraje=" + TextBoxKms.Text + "&meses=" + TextBoxMeses.Text + " ");
		}
		
		protected void Editar_Kits(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Automotriz.ControlKit&tipo=Editar&codigo="+this.ddlKitsEdit.SelectedValue+"&nombre=&grupo=&lista=");
		}
		
		protected void Borrar_Kits(Object  Sender, EventArgs e)
		{
			//Cargamos el Combo a eliminar
			if(ddlKitsDelete.Items.Count!=0)
			{
				Combo comboEliminar = new Combo(this.ddlKitsDelete.SelectedValue);
                if (comboEliminar.DeleteValues())
                {
                    ddlKitsDelete.Items.Remove(this.ddlKitsDelete.SelectedValue);
                    Response.Redirect("" + indexPage + "?process=Automotriz.AdminKitscombos");
                }
                else
                    lb.Text += "<br>" + comboEliminar.ProcessMsg;
			}
			else
                Utils.MostrarAlerta(Response, "No hay kits para eliminar");
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
