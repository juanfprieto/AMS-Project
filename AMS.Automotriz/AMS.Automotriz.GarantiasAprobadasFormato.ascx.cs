// created on 11/05/2005 at 14:53

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
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class GarantiasAprobadasFormato : System.Web.UI.UserControl
	{
		protected DataTable dtInserts;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen, palm_descripcion FROM palmacen pa where (pa.pcen_centtal is not null  or pcen_centcoli is not null) and pa.TVIG_VIGENCIA = 'V' order by pa.PALM_DESCRIPCION;");
				bind.PutDatasIntoDropDownList(centCostos,"SELECT pcen_codigo, pcen_nombre FROM pcentrocosto");
				prefFact.Text    = Request.QueryString["pref"];
				fechFactura.Text = Request.QueryString["fech"];
				numFact.Text     = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefFact.Text+"'");
				nitCSAEX.Text    = Request.QueryString["nit"];
				if(Request.QueryString["gen"]=="A")
					numFact.ReadOnly = true;
				this.Preparar_Tabla_Inserts();
				this.dgInserts_Bind();
			}
			if(Session["dtInserts"]==null)
				this.Preparar_Tabla_Inserts();
			else
				dtInserts = (DataTable)Session["dtInserts"];
		}
		
		protected void Preparar_Tabla_Inserts()
		{
			dtInserts = new DataTable();
			dtInserts.Columns.Add(new DataColumn("PREFIJO",System.Type.GetType("System.String")));//0
			dtInserts.Columns.Add(new DataColumn("NUMERO",System.Type.GetType("System.String")));//1
			dtInserts.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.Double")));//2
			dtInserts.Columns.Add(new DataColumn("APROBADO",System.Type.GetType("System.Double")));//3
            dtInserts.Columns.Add(new DataColumn("APROBADOMO", System.Type.GetType("System.Double")));//4
            dtInserts.Columns.Add(new DataColumn("APROBADOTT", System.Type.GetType("System.Double")));//5
		}
		
		protected void dgInserts_Bind()
		{
			Session["dtInserts"] = dtInserts;
			dgInserts.DataSource = dtInserts;
			dgInserts.DataBind();
		}
		
		public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
    	{
    		dgInserts.EditItemIndex = -1;
    		this.dgInserts_Bind();
    	}
    	
    	public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
    	{
    		dtInserts.Rows[dgInserts.EditItemIndex][0] = ((TextBox)e.Item.FindControl("nitCSA")).Text;
    		dtInserts.Rows[dgInserts.EditItemIndex][1] = ((TextBox)e.Item.FindControl("nitCSAa")).Text;
    		dtInserts.Rows[dgInserts.EditItemIndex][2] = System.Convert.ToDouble(((TextBox)e.Item.FindControl("nitCSAb")).Text);
    		dtInserts.Rows[dgInserts.EditItemIndex][3] = System.Convert.ToDouble(((TextBox)e.Item.FindControl("aprobado")).Text);
            dtInserts.Rows[dgInserts.EditItemIndex][4] = System.Convert.ToDouble(((TextBox)e.Item.FindControl("aprobadomo")).Text);
            dtInserts.Rows[dgInserts.EditItemIndex][5] = System.Convert.ToDouble(((TextBox)e.Item.FindControl("aprobadott")).Text);
    		dgInserts.EditItemIndex = -1;
    		this.dgInserts_Bind();
    		this.Total();
    	}
    	
    	public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
  	 	{
			dgInserts.EditItemIndex = (int)e.Item.ItemIndex;
   	 		this.dgInserts_Bind();
    	}

		private void dgInserts_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
				((Button)e.Item.Cells[4].FindControl("btnAdd")).Attributes.Add("onclick","return RevisionTbFactura("+e.Item.Cells[0].FindControl("nitCSA").ClientID+");");
			}
		}
    	
    	protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow")
        	{
        		if(!this.Buscar_Factura(((TextBox)e.Item.Cells[0].Controls[1]).Text,((TextBox)e.Item.Cells[1].Controls[1]).Text))
        		{
        			DataRow fila = dtInserts.NewRow();
	        		fila["PREFIJO"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
 		       		fila["NUMERO"] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
   		     		fila["VALOR"] = System.Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
     		   		fila["APROBADO"] = System.Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
                    fila["APROBADOMO"] = System.Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
                    fila["APROBADOTT"] = System.Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
       		 		dtInserts.Rows.Add(fila);
        		}
        		else
                    Utils.MostrarAlerta(Response, "Factura Repetida. Por Favor Revise");
        	}

            if (((Button)e.CommandSource).CommandName == "AddDataAll")
            {
                bool pasar = true;
                DataSet da = new DataSet();
                double Valoraprobadorepuestos = 0, valoraprobadoterceros = 0;
                if (pasar)
                {
                    DBFunctions.Request(da, IncludeSchema.NO, "SELECT MF.pdoc_codigo AS Prefijo, cast(MF.mfac_numedocu as varchar(10)) AS Numero, MF.mfac_valofact AS Valor FROM mfacturacliente mf, mfacturaclientetaller mft WHERE mnit_nit = '"+nitCSAEX.Text+"' AND(MF.mfac_valofact + MF.mfac_valoiva + MF.mfac_valoflet + MF.mfac_valoivaflet - MF.mfac_valorete - MF.mfac_valoabon) > 0 and mf.pdoc_codigo = mft.pdoc_codigo and mf.mfac_numedocu = mft.mfac_numedocu and mft.TCAR_CARGO = 'G' ORDER BY MF.pdoc_codigo, MF.mfac_numedocu;");

                    foreach (DataRow Dr in da.Tables[0].Rows)
                    {
                        DataRow fila = dtInserts.NewRow();
                        fila[0] = Dr[0].ToString();
                        fila[1] = Dr[1].ToString();
                        fila[2] = Dr[2].ToString();
                        fila[3] = Valoraprobadorepuestos;
                        fila[4] = Dr[2].ToString();
                        fila[5] = valoraprobadoterceros;                        
                        dtInserts.Rows.Add(fila);
                    }
                }
            }
           

            if(((Button)e.CommandSource).CommandName == "DelDatasRow")
        	{
        		try
        		{
        			dtInserts.Rows.Remove(dtInserts.Rows[e.Item.ItemIndex]);
        		}
        		catch
        		{
        			dtInserts.Rows.Clear();
        		}
        	}
        	this.Total();
        	this.dgInserts_Bind();
        }
		
		protected bool Buscar_Factura(string prefijo, string numero)
		{
			bool encontrado = false;
			for(int i=0;i<dtInserts.Rows.Count;i++)
			{
				if((dtInserts.Rows[i][0].ToString() == prefijo)&&(dtInserts.Rows[i][1].ToString() == numero))
					encontrado = true;
			}
			return encontrado;
		}
		
		protected void Total()
		{
			double totalFacturas = 0, totalAprobado=0, valorIva=0, totalFactura=0, totalReps = 0, totalManoObra = 0, totalTrcrs = 0;
			double porcentajeIva = System.Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
			for(int i=0;i<dtInserts.Rows.Count;i++)
			{
				totalFacturas += System.Convert.ToDouble(dtInserts.Rows[i][2]);
                totalAprobado += System.Convert.ToDouble(dtInserts.Rows[i][3]) + System.Convert.ToDouble(dtInserts.Rows[i][4]) + System.Convert.ToDouble(dtInserts.Rows[i][5]);
                totalReps += System.Convert.ToDouble(dtInserts.Rows[i][3]);
                totalManoObra += System.Convert.ToDouble(dtInserts.Rows[i][4]);
                totalTrcrs += System.Convert.ToDouble(dtInserts.Rows[i][5]);
			}
			valorIva            = (totalAprobado*(porcentajeIva/100));
			totalFactura        = valorIva + totalAprobado;
			totalFacts.Text     = totalFacturas.ToString("C");
			totalApro.Text      = totalAprobado.ToString("C");
			totalIva.Text       = valorIva.ToString("C");
			totalFact.Text      = totalFactura.ToString("C");
            totalRepuestos.Text = totalReps.ToString("C");
            totalMObra.Text     = totalManoObra.ToString("C");
            totalTerceros.Text  = totalTrcrs.ToString("C");
		}
		
		protected void Cancelar_Factura(Object Sender, EventArgs E)
		{
			Response.Redirect("" + indexPage + "?process=Automotriz.GarantiasAprobadas");
		}
		
		protected ArrayList ArmarDetalles()
		{
			ArrayList sqlStrings=new ArrayList();
			for(int i=0;i<dtInserts.Rows.Count;i++)
			{
                sqlStrings.Add("INSERT INTO dfacturagarantias VALUES('@1',@2,'" + dtInserts.Rows[i][0].ToString() + "'," + dtInserts.Rows[i][1].ToString() + "," + dtInserts.Rows[i][3].ToString() + "," + dtInserts.Rows[i][4].ToString() + "," + dtInserts.Rows[i][5].ToString() + ")");
                double valor = Convert.ToDouble(DBFunctions.SingleData("SELECT MFAC_VALOFACT +  MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE FROM mfacturacliente WHERE pdoc_codigo='"+dtInserts.Rows[i][0].ToString()+"' AND mfac_numedocu="+dtInserts.Rows[i][1].ToString()+" "));
			//sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C', MFAC_VALOABON = MFAC_VALOFACT +  MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE WHERE pdoc_codigo='"+dtInserts.Rows[i][0].ToString()+"' AND mfac_numedocu="+dtInserts.Rows[i][1].ToString()+" ");
                sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C', MFAC_VALOABON = "+ valor +" WHERE pdoc_codigo='" + dtInserts.Rows[i][0].ToString() + "' AND mfac_numedocu=" + dtInserts.Rows[i][1].ToString() + " ");
                // SE GRABA EL DETALLE DEL PAGO   
                sqlStrings.Add("INSERT INTO ddetallefacturacliente values ('" + dtInserts.Rows[i][0].ToString() + "'," + dtInserts.Rows[i][1].ToString() + ", '@1',@2, " + valor + ", 'APROBACION GARANTIA POR " + dtInserts.Rows[i][3].ToString() + " ' )" );
            }
			return sqlStrings;
		}

		protected void Grabar_Factura(Object Sender, EventArgs E)
		{
			//Primero debemos revisar que se haya digitado una cantidad de dias valida
			if(DatasToControls.ValidarInt(diasPlazo.Text))
			{
				if(System.Convert.ToInt32(diasPlazo.Text)>0)
				{
					FacturaCliente miFacturaCliente=new FacturaCliente("FG",prefFact.Text,nitCSAEX.Text,almacen.SelectedValue,"F",Convert.ToUInt32(numFact.Text),Convert.ToUInt32(diasPlazo.Text),Convert.ToDateTime(fechFactura.Text),Convert.ToDateTime(fechFactura.Text).AddDays(Convert.ToInt32(diasPlazo.Text)),Convert.ToDateTime(null),Convert.ToDouble(totalApro.Text.Substring(1)),Convert.ToDouble(totalIva.Text.Substring(1)),0,0,0,0,centCostos.SelectedValue,obsrv.Text,DBFunctions.SingleData("SELECT pven_codigo FROM ccartera"),HttpContext.Current.User.Identity.Name.ToLower(),null);
					miFacturaCliente.SqlRels=ArmarDetalles();
					//Ahora creamos nuestro objeto de tipo FacturaGarantias
					if(miFacturaCliente.GrabarFacturaCliente(true))
						Response.Redirect("" + indexPage + "?process=Automotriz.GarantiasAprobadas&prefF="+miFacturaCliente.PrefijoFactura+"&numF="+miFacturaCliente.NumeroFactura+"");
						//lb.Text += miFacturaCliente.ProcessMsg;
					else
						lb.Text += miFacturaCliente.ProcessMsg;
				}
				else
                    Utils.MostrarAlerta(Response, "Valor de dias de plazo no puede ser negativo. Revise por favor");
			}
			else
                Utils.MostrarAlerta(Response, "Valor de dias de plazo no valido. Revise por favor");
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
			this.dgInserts.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgInserts_ItemDataBound);

		}
		#endregion		
	}
}
