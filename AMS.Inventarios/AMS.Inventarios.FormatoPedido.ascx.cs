using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class FormatoPedido : System.Web.UI.UserControl
	{
		protected DataTable dtFormato;
		protected DatasToControls bind;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			bind = new DatasToControls();
			if(!IsPostBack)
			{
				Session.Clear();
                bind.PutDatasIntoDropDownList(ddlProveedor, "SELECT MPRO.mnit_nit, MNIT.mnit_NIT CONCAT ' - ' CONCAT MNIT.NOMBRE AS PROVEEDOR FROM Vmnit MNIT, mproveedor MPRO WHERE MPRO.mnit_nit = MNIT.mnit_nit ORDER BY 2;");
				this.BindDdlInformacion(ddlObjeto.SelectedValue);
			}
			if(Session["dtFormato"]==null)
				this.PrepararDtFormato();
			else
				dtFormato = (DataTable)Session["dtFormato"];
		}
		
		protected void CambioObjeto(object sender, System.EventArgs e)
		{
			this.BindDdlInformacion(ddlObjeto.SelectedValue);
			if(ddlObjeto.SelectedValue == "O")
				this.tbInformacion.Visible = true;
			else
				this.tbInformacion.Visible = false;
		}
		
		protected void ConfirmarNit(object sender, System.EventArgs e)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mfpi_opcdpedite, mfpi_secuencia FROM mformpeditemprov WHERE mnit_nit='"+this.ddlProveedor.SelectedValue+"' ORDER BY mfpi_secuencia ASC");
			if(ds.Tables[0].Rows.Count>0)
			{
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataRow fila = dtFormato.NewRow();
					fila[0] = ds.Tables[0].Rows[i][0].ToString();
					dtFormato.Rows.Add(fila);
				}
				BindDgFormato();
			}
			btnConfirmar.Enabled = ddlProveedor.Enabled = false;
			plProceso.Visible = true;
		}
		
		protected void AgregarElemento(object sender, System.EventArgs e)
		{
			//Primero revisamos si es un valor estatico entonces lo agregamos normalmente
			DataRow fila = dtFormato.NewRow();
			if(this.ddlInformacion.SelectedValue == "TBING")
			{
				if(this.tbInformacion.Text == "")
                    Utils.MostrarAlerta(Response, "Debe ingresar un valor en el cuadro de texto");
				else
				{
					fila[0] = this.tbInformacion.Text;
					dtFormato.Rows.Add(fila);
				}
			}
			else
			{
				if((dtFormato.Select("VALORAGREGADO= '"+this.ddlInformacion.SelectedValue+"'")).Length==0)
				{
					fila[0] = this.ddlInformacion.SelectedValue;
					dtFormato.Rows.Add(fila);
				}
				else
                    Utils.MostrarAlerta(Response, "Ya existe este elemento");
			}
			BindDgFormato();
		}
		
		protected void AceptarElementos(object sender, System.EventArgs e)
		{
			DBFunctions.NonQuery("DELETE FROM mformpeditemprov WHERE mnit_nit='"+this.ddlProveedor.SelectedValue+"'");
			ArrayList sqlStrings = new ArrayList();
			for(int i=0;i<this.dtFormato.Rows.Count;i++)
				sqlStrings.Add("INSERT INTO mformpeditemprov VALUES('"+this.ddlProveedor.SelectedValue+"','"+dtFormato.Rows[i][0].ToString()+"',"+(i+1).ToString()+")");
			if(DBFunctions.Transaction(sqlStrings))
				//lb.Text += "<br>Bien "+DBFunctions.exceptions;
				Response.Redirect("" + indexPage + "?process=Inventarios.FormatoPedidoProveedor");
			else
				lb.Text += "<br>Error "+DBFunctions.exceptions;
		}
		
		
		
		protected void CancelarProceso(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.FormatoPedidoProveedor");
		}
		
		protected void DgFormatoDelete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				dtFormato.Rows[e.Item.DataSetIndex].Delete();
				this.BindDgFormato();
			}
            catch (Exception ex)
            {
                Utils.MostrarAlerta(Response, "Error Eliminado Elemento");lb.Text += ex.ToString();}
		}
		
		protected void DgFormatoCommand(object sender, DataGridCommandEventArgs e)
		{
			int indexRow = e.Item.DataSetIndex;
			if(e.CommandName == "up")
			{
				if(indexRow>0)
				{
					DataRow filaTemp = dtFormato.NewRow();
					filaTemp[0] = dtFormato.Rows[indexRow][0];
					dtFormato.Rows[indexRow].Delete();
					dtFormato.Rows.InsertAt(filaTemp,indexRow-1);
					lb.Text += "<br>index : "+indexRow+ " new index "+(indexRow-1);
				}
				else
                    Utils.MostrarAlerta(Response, "No se puede subir el elemento");
			}
			else if(e.CommandName == "down")
			{
				if(indexRow == (dtFormato.Rows.Count-1))
                    Utils.MostrarAlerta(Response, "No se puede bajar el elemento");
				else
				{
					DataRow filaTemp = dtFormato.NewRow();
					filaTemp[0] = dtFormato.Rows[indexRow][0];
					dtFormato.Rows[indexRow].Delete();
					dtFormato.Rows.InsertAt(filaTemp,indexRow+1);
					lb.Text += "<br>index : "+indexRow+ " new index "+(indexRow+1);
				}
			}
			DataTable dtInput = dtFormato.Copy();
			this.BindDgFormato(dtInput);
		}
		
		protected void BindDdlInformacion(string opc)
		{
			ddlInformacion.Items.Clear();
			if(opc=="P")//Pedido
			{
				ddlInformacion.Items.Add(new ListItem("Número de Pedido","pped_codigo CONCAT *-* CONCAT CAST(mped_numepedi AS CHARACTER(5))@MPEDIDOITEM"));
				ddlInformacion.Items.Add(new ListItem("Fecha de Pedido","mped_pedido@MPEDIDOITEM"));
			}
			else if(opc=="R")//Proveedor
			{
				ddlInformacion.Items.Add(new ListItem("Nit Proveedor","mnit_nit@MPEDIDOITEM"));
				ddlInformacion.Items.Add(new ListItem("Codigo en Proveedor","mpro_numeclie@MPROVEEDOR"));
			}
			else if(opc=="I")//Items
			{
				ddlInformacion.Items.Add(new ListItem("Codigo del Item","mite_codigo@DPEDIDOITEM"));
				ddlInformacion.Items.Add(new ListItem("Nombre del Item","mite_nombre@MITEMS"));
				ddlInformacion.Items.Add(new ListItem("Linea de Bodega","plin_codigo@MITEMS"));
				ddlInformacion.Items.Add(new ListItem("Familia","pfam_codigo@MITEMS"));
				ddlInformacion.Items.Add(new ListItem("Marca","pmar_codigo@MITEMS"));
				ddlInformacion.Items.Add(new ListItem("Cantidad Pedida","dped_cantpedi@DPEDIDOITEM"));
				ddlInformacion.Items.Add(new ListItem("Valor Unitario","dped_valounit@DPEDIDOITEM"));
				ddlInformacion.Items.Add(new ListItem("Porcentaje de Iva","dped_porcdesc@DPEDIDOITEM"));
			}
			else if(opc=="O")//Otro Valor
			{
				ddlInformacion.Items.Add(new ListItem("Valor Ingresado","TBING"));
				ddlInformacion.Items.Add(new ListItem("Valor Secuencial","SEC"));
			}
		}
		
		protected void BindDgFormato()
		{
			dgFormato.DataSource = dtFormato;
			dgFormato.DataBind();
			Session["dtFormato"] = dtFormato;
		}
		
		protected void BindDgFormato(DataTable input)
		{
			dgFormato.DataSource = input;
			dgFormato.DataBind();
			Session["dtFormato"] = dtFormato;
		}
		
		protected void PrepararDtFormato()
		{
			dtFormato = new DataTable();
			dtFormato.Columns.Add(new DataColumn("VALORAGREGADO",System.Type.GetType("System.String")));//0
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
