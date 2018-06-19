// created on 22/09/2004 at 9:38
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class OtrosDatos : System.Web.UI.UserControl	
	{
		#region Atributos
		protected DataTable tablaAccesorios;
		protected TextBox []cantidad;
		protected TextBox []detalle;
		private DatasToControls bind = new DatasToControls();
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//botones q hace la confirmacion de los datos y cancela el boton para q no se produzca. doble facturacion o etc
			//confirmar.Attributes.Add("Onclick","document.getElementById('"+confirmar.ClientID+"').disabled = true;"+this.Page.GetPostBackEventReference(confirmar)+";");
			//confirmar.Attributes.Add("onClick","confirm('Esta seguro que desea Continuar?')");
			if(!IsPostBack)
			{
				Session["tablaAccesorios"]= null;
				Llenar_Tabla_Accesorios();
				bind.PutDatasIntoRadioButtonList(nivelCombustible,"SELECT tnivcomb_codigo, tnivcomb_descripcion FROM tnivelcombustible");
				bind.PutDatasIntoDropDownList(ddlencuesta,"SELECT tres_sino,tres_nombre FROM trespuestasino");
				bind.PutDatasIntoDropDownList(ddlelevador,"SELECT tres_sino,tres_nombre FROM trespuestasino");
				bind.PutDatasIntoDropDownList(ddlpresupuesto,"SELECT tres_sino,tres_nombre FROM trespuestasino");
				if (ddlencuesta.Items.Count > 2)
					ddlencuesta.SelectedIndex = 2;
				if (ddlelevador.Items.Count > 2)
					ddlelevador.SelectedIndex = 2;
				if (ddlpresupuesto.Items.Count > 2)
					ddlpresupuesto.SelectedIndex = 2;
                ddlencuesta.Items.Insert(0, "--Escoja--");
                ddlelevador.Items.Insert(0, "--Escoja--");
                ddlpresupuesto.Items.Insert(0, "--Escoja--");
				
			}
			else
			{
				tablaAccesorios = (DataTable)Session["tablaAccesorios"];
				//accesorios.DataSource = tablaAccesorios;
			}
		}
		
		protected  void  Confirmar(Object  Sender, EventArgs e)
		{
            Control principal = (this.Parent).Parent; //Control principal ascx
			//Response.Write("<script language:javascript> if(!confirm('¿Seguro que ha leido las condiciones del contrato?'))this.form.submit(); </script>"); 

            if (ddlelevador.SelectedValue == "--Escoja--" || ddlpresupuesto.SelectedValue == "--Escoja--")
				Utils.MostrarAlerta(Response, "Debe seleccionar una opción en el cuadro de Información Adicional");
			else
			{
				for(int i=0;i<accesorios.Items.Count;i++)
				{
					if(((CheckBox)accesorios.Items[i].Cells[0].FindControl("chbacc")).Checked)
					{
						tablaAccesorios.Rows[i][2]=((TextBox)accesorios.Items[i].Cells[2].FindControl("tbcantacc")).Text;
						tablaAccesorios.Rows[i][3]=true;
					}
					Session["tablaAccesorios"]=tablaAccesorios;
				}
				Ocultar_Control();
			}
            
		}

		#endregion
		
		#region Metodos
		
		protected void Preparar_Tabla_Accesorios()
		{
			tablaAccesorios = new DataTable();
			tablaAccesorios.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			tablaAccesorios.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			tablaAccesorios.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.String")));
			tablaAccesorios.Columns.Add(new DataColumn("SELECCIONADO",typeof(bool)));
		}
		
		protected void Llenar_Tabla_Accesorios()
		{
			Preparar_Tabla_Accesorios();
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pacc_codigo,pacc_descripcion FROM dbxschema.paccesorio ORDER BY pacc_descripcion");
			try
			{
				if(ds.Tables.Count!=0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						DataRow fila=tablaAccesorios.NewRow();
						fila[0]=ds.Tables[0].Rows[i][0].ToString();
						fila[1]=ds.Tables[0].Rows[i][1].ToString();
                        fila[2] = "";
						fila[3]=false;
						tablaAccesorios.Rows.Add(fila);
					}
				}
			}
            catch
            {
                Utils.MostrarAlerta(Response, "Error Interno al cargar accesorios");
                return;
            }
			accesorios.DataSource = tablaAccesorios;
			accesorios.DataBind();
			Session["tablaAccesorios"] = tablaAccesorios;
		}
		
		protected void Ocultar_Control()
		{
			Control principal = (this.Parent).Parent; //Control principal ascx
            HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
            hdTabIndex.Value = "4";
			//((PlaceHolder)this.Parent).Visible = false;
			//((PlaceHolder)principal.FindControl("kitsCombos")).Visible = true;
            //((ImageButton)principal.FindControl("botonKits")).Enabled = true;
            //((ImageButton)principal.FindControl("otros")).ImageUrl="../img/AMS.BotonExpandir.png";
            //((ImageButton)principal.FindControl("botonKits")).ImageUrl="../img/AMS.BotonContraer.png";
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

