using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ListasEmpaque : System.Web.UI.UserControl
	{
		protected FormatosDocumentos formatoFactura;
        protected ArrayList lbFields = new ArrayList();
        protected ArrayList types = new ArrayList();
        protected DataTable dtSource;
        protected DataSet ds;
        protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
                
				if(Request.QueryString["subprocess"]=="Fact")
				{
					plRow1.Visible = true;
					if(Request.QueryString["factGen"] != null)
					{
                        Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo " + Request.QueryString["prefF"] + " y número " + Request.QueryString["numF"] + ".!");
						formatoFactura=new FormatosDocumentos();
						try
						{
							formatoFactura.Prefijo=Request.QueryString["prefF"];
							formatoFactura.Numero=Convert.ToInt32(Request.QueryString["numF"]);
							formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefF"]+"'");
							if(formatoFactura.Codigo!=string.Empty)
							{
								if(formatoFactura.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
							}
						}
						catch
						{
							lb.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
						}
					}
				}
				else if(Request.QueryString["subprocess"]=="Mod")
				{
					plRow2.Visible = true;
					DatasToControls bind = new DatasToControls();
					bind.PutDatasIntoDropDownList(this.ddlNLis,"SELECT mlis_numero FROM mlistaempaque ORDER BY 1");
					btnFacturar.Text = "Modificar";
				}
			}
		}
        protected void LoadDataColumns()
        {
            lbFields.Add("mlis_numero");//0 codigo Item
            types.Add(typeof(string));
            lbFields.Add("mite_codigo");//0 codigo Item
            types.Add(typeof(string));
        }
        protected void LoadDataTable()
        {
            dtSource = new DataTable();
            for (int i = 0; i < lbFields.Count; i++)
                dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
        }

        public void Facturar(object sender, System.EventArgs e)
		{
            string LsEmpaque = "";

            for (int i = 0; i < dgItems.Items.Count; i++)
            {
                if(((CheckBox)dgItems.Items[i].Cells[2].FindControl("cbRows")).Checked == true)
                {
                    LsEmpaque += dgItems.Items[i].Cells[0].Text + "-";
                }
            }
            string tp = DBFunctions.SingleData("SELECT t1.tped_codigo from PPEDIDO as t1, DLISTAEMPAQUE t2 WHERE t1.pped_codigo=t2.pped_codigo and t2.mlis_numero=" + LsEmpaque[0]);//Tipo de pedido
            if (tp == "T") Utils.MostrarAlerta(Response,"Debe seleccionar solo una lista de empaque para facturar las transferencias");
            string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
			if(Request.QueryString["subprocess"]=="Fact")
			{
				//if(ddlNLis.SelectedItem.Text.Length>0)
                if(LsEmpaque != "")
					//Response.Redirect(""+indexPage+"?process=Inventarios.Facturacion&path=Facturacion&nped="+this.ddlNLis.SelectedValue+"&orig=Inventarios.ListasEmpaque");
                    Response.Redirect("" + indexPage + "?process=Inventarios.Facturacion&path=Facturacion&nped=" + LsEmpaque + "&orig=Inventarios.ListasEmpaque");
				else
                    Utils.MostrarAlerta(Response, "Primero debe seleccionar una lista de empaque!");
			}
			else if(Request.QueryString["subprocess"]=="Mod")
			{
				if(ddlNLis.SelectedItem.Text.Length>0)
					Response.Redirect(""+indexPage+"?process=Inventarios.ModificadorListaEmpaque&numLista="+this.ddlNLis.SelectedValue+"");
				else
                    Utils.MostrarAlerta(Response, "Primero debe seleccionar una lista de empaque!");
			}
		}

        public void buscarCliente(object sender, System.EventArgs e)
        {
            tbNita.Text = DBFunctions.SingleData("select coalesce(NOMBRE,'Cliente NO tiene Listas de Empaque') from vmnit WHERE mnit_nit='" + this.tbNit.Text.Trim() + "' ").ToString();
            if (tbNita.Text == "")
                tbNita.Text = "Cliente NO tiene Listas de Empaque";
        }

        public void CargarLista(object sender, System.EventArgs e)
		{
            //Primero se verifica que este nit tenga registros dentro de la lista de empaque

            if (DBFunctions.RecordExist("SELECT * FROM mlistaempaque WHERE mnit_nit='"+this.tbNit.Text.Trim()+"' ORDER BY 1"))
			{
				plRow2.Visible = true;
				DatasToControls bind = new DatasToControls();
			//	bind.PutDatasIntoDropDownList(this.ddlNLis,"SELECT mlis_numero FROM mlistaempaque WHERE mnit_nit='"+this.tbNit.Text.Trim()+"'");
                bind.PutDatasIntoDropDownList(this.ddlNLis, @"SELECT distinct m.mlis_numero,  m.mlis_numero || '  Pedido '|| d.pped_codigo || '-'|| d.mped_numepedi || '  ' || 
                                     		  coalesce(mpt.pdoc_codigo ||'-'||mpt.mord_numeorde,'') ||' '||coalesce(tcar_cargo,'C') AS ASIGNACION
                                         FROM mlistaempaque m, dlistaempaque d 
                                         	  left join mpedidotransferencia mpt on d.pped_codigo = mpt.pped_codigo and d.mped_numepedi = mpt.mped_numero
                                         WHERE m.mlis_numero = d.mlis_numero and m.mnit_nit='" + this.tbNit.Text.Trim() + "' ORDER BY 1");
                btnFacturar.Text = "Facturar";
                string sqlDev = (@"SELECT distinct m.mlis_numero,m.mlis_numero || '  Pedido '|| d.pped_codigo || '-'|| d.mped_numepedi || '  ' || 
                                     		  coalesce(mpt.pdoc_codigo ||'-'||mpt.mord_numeorde,'') ||' '||coalesce(tcar_cargo,'C') AS mite_codigo
                                         FROM mlistaempaque m, dlistaempaque d 
                                         	  left join mpedidotransferencia mpt on d.pped_codigo = mpt.pped_codigo and d.mped_numepedi = mpt.mped_numero
                                         WHERE m.mlis_numero = d.mlis_numero and m.mnit_nit='" + this.tbNit.Text.Trim() + "' ORDER BY 1");
                ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO, sqlDev);
                LoadDataColumns();
                LoadDataTable();
                dtSource.Rows.Clear();
                for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
                {
                    DataRow fila = dtSource.NewRow();
                    fila[0] = ds.Tables[0].Rows[n][0].ToString();
                }
                dgItems.DataSource = ds.Tables[0];
                dgItems.DataBind();
                for (int i = 0; i < dgItems.Items.Count; i++)
                {
                    if (i >= 5 && i <= 7)
                        for (int j = 0; j < dgItems.Items.Count; j++)
                            dgItems.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    ((CheckBox)dgItems.Items[i].Cells[2].FindControl("cbRows")).Checked = true;
                }
                if (Request.QueryString["subprocess"]=="Fact"){
                    plRow3.Visible = true;
                    txtSaldoCartera.Text = Utilidades.Clientes.ConsultarSaldo(tbNit.Text.Trim()).ToString("#,##0");
                    txtSaldoMoraCartera.Text = Utilidades.Clientes.ConsultarSaldoMora(tbNit.Text.Trim()).ToString("#,##0");
                    try
                    {
                        txtCupo.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(MCLI_CUPOCRED,0) FROM mcliente WHERE mnit_nit='" + tbNit.Text.Trim() + "'")).ToString("#,##0");
                    }
                    catch {
                        txtCupo.Text = "0";
                    }
                }
			}
			else
                Utils.MostrarAlerta(Response, "El Nit Ingresado No Tiene Listas de Empaque Asociadas!");
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
