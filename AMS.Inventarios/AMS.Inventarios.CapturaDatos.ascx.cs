using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.Tools;

namespace AMS.Inventarios
{
	/// <summary>
	///		Descripción breve de AMS_Inventarios_CapturaDatos.
	/// </summary>
	public partial class AMS_Inventarios_CapturaDatos : System.Web.UI.UserControl
	{
		#region Atributos		
		private DatasToControls bind = new DatasToControls();
		private string scriptValidacion;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Inventarios_CapturaDatos));	
			
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlInventarios,"SELECT INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),COUNT(DINV.dinv_mite_codigo) "+
					"FROM minventariofisico INF INNER JOIN dinventariofisico DINV ON INF.pdoc_codigo = DINV.pdoc_codigo AND INF.minf_numeroinv = DINV.minf_numeroinv "+
					"WHERE  INF.minf_fechacierre IS NULL AND INF.minf_fechainicio <= '"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND minf_fechacierre is null "+
					"GROUP BY INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)), CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)) "+
					"HAVING COUNT(DINV.dinv_mite_codigo) > 0 "+
					"ORDER BY CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10))");
				ddlInventarios.Items.Insert(0,new ListItem("Seleccione...",String.Empty));
				btnAceptar.Attributes.Add("onclick","return ValSelInvFis();");
				btnGenerar.Attributes.Add("onclick","return ContainerValidacionGeneracion();");
				btnGrabar.Attributes.Add ("onclick","return ValidarInfoGrilla();");
			}
			else
			{
				CargarInfoConteoRequest();
				CargarInfoTarjetasRequest();
			}
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			if(ddlInventarios.SelectedIndex > 0)
			{
				ddlInventarios.Enabled = false;
				pnlInfoProceso.Visible = true;

				//string prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
				//int numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());

                string prefijoInventario = "";
                int numeroInventario = 0;

                if (ddlInventarios.SelectedValue.Split('-').Length == 3)
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

                }
                else
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1]);
                }


				InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
				
				lbNumItems.Text = inst.ItemsInventario.Count.ToString();
				string conteo = inst.DtItemsInventarioFisico.Compute("MIN(dinv_conteoactual)","").ToString();
				
				if(conteo == "0")
					lbNumConteo.Text = "1";
				else
					lbNumConteo.Text = conteo;
				
				CargarConteosRealizados(inst.ConteosPendientesInstancia);
				
				if(ddlConteosRelacionados.Items.Count > 0)
				{
					CargarItemsRelacionadosConteo(Convert.ToInt32(ddlConteosRelacionados.SelectedValue),inst);
					CargarInfoReferencia((ddlReferencias.SelectedValue.Split('$'))[0],Convert.ToInt32((ddlReferencias.SelectedValue.Split('$'))[1]),inst);
					CargarTarjetasRelacionadosConteo(Convert.ToInt32(ddlConteosRelacionados.SelectedValue),inst);
				}
				else
					pnlInfoProceso.Visible = false;
			}
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.CapturaDatos");
		}
  
		protected void btnGenerar_Click(object sender, System.EventArgs e)
		{
			CargarItemsConteo();
		}

		private void dgItemsConteo_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
				scriptValidacion +=  "ElementosValidar[ElementosValidar.length] = '"+((TextBox)e.Item.Cells[5].FindControl("tbCantidad")).ClientID+"$"+((HtmlInputHidden)e.Item.Cells[5].FindControl("hdNumeroTarjeta")).ClientID+"';";
		}

		protected void btnGrabar_Click(object sender, System.EventArgs e)
		{
			//string prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
			//int numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());


            string prefijoInventario = "";
            int numeroInventario = 0;

            if (ddlInventarios.SelectedValue.Split('-').Length == 3)
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1]);
            }









			string salida = "";
		
			for(int i=0;i<dgItemsConteo.Items.Count;i++)
			{
				string codigoOriginalItem = ((HtmlInputHidden)dgItemsConteo.Items[i].Cells[5].FindControl("hdCodigoOriginal")).Value;
				string codigoModificadoItem = ((HtmlInputHidden)dgItemsConteo.Items[i].Cells[5].FindControl("hdCodigoModificado")).Value;
				int numeroTarjeta = Convert.ToInt32(((HtmlInputHidden)dgItemsConteo.Items[i].Cells[5].FindControl("hdNumeroTarjeta")).Value);
				int conteoRelacionado = Convert.ToInt32(((HtmlInputHidden)dgItemsConteo.Items[i].Cells[5].FindControl("hdConteoRelacionado")).Value);
				double cantidadIngresada = Convert.ToDouble(((TextBox)dgItemsConteo.Items[i].Cells[5].FindControl("tbCantidad")).Text);
				
				salida += InventarioFisico.ModificarEstadoConteoRenglon(prefijoInventario,numeroInventario,codigoOriginalItem,numeroTarjeta,conteoRelacionado,cantidadIngresada,codigoModificadoItem);
			}

			if(salida != String.Empty) 
				 Utils.MostrarAlerta(Response, ""+salida+"");
			else
			{
				InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
				CargarConteosRealizados(inst.ConteosPendientesInstancia);
				if(ddlConteosRelacionados.Items.Count > 0)
				{
					CargarItemsRelacionadosConteo(Convert.ToInt32(ddlConteosRelacionados.SelectedValue),inst);
					CargarInfoReferencia((ddlReferencias.SelectedValue.Split('$'))[0],Convert.ToInt32((ddlReferencias.SelectedValue.Split('$'))[1]),inst);
					CargarTarjetasRelacionadosConteo(Convert.ToInt32(ddlConteosRelacionados.SelectedValue),inst);
					dgItemsConteo.DataSource = null;
					dgItemsConteo.DataBind();
					btnGrabar.Visible = false;
				}
				else
					pnlInfoProceso.Visible = false;
			}
		}

		/*private void dgItemsConteo_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if(e.CommandName == "AgregarItem")
			{
				
				
				string salida = InventarioFisico.ModificarEstadoConteoRenglon(prefijoInventario,numeroInventario,codigoOriginalItem,numeroTarjeta,conteoRelacionado,cantidadIngresada,codigoModificadoItem);
				
				else
				{
					prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
					numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
					
					dgItemsConteo.DataSource = null;
					dgItemsConteo.DataBind();
				}
			}
		}*/

		#endregion

		#region Metodos Ajax

		[Ajax.AjaxMethod]
		public DataSet CargarItemsRelacionadosConteo(int conteoRelacionado, string prefijoInv, int numeroInv)
		{
			InventarioFisico inst = new InventarioFisico(prefijoInv,numeroInv);

			DataSet ds = new DataSet();

			DataTable dtCopy = inst.DtItemsInventarioFisico.Clone();
			DataRow[] select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1),"dinv_tarjeta ASC");
			
			for(int i=0;i<select.Length;i++)
			{
				DataRow dr = dtCopy.NewRow();

				for(int j=0;j<inst.DtItemsInventarioFisico.Columns.Count;j++)
					dr[j] = select[i][j];
				
				dtCopy.Rows.Add(dr);
			}

			ds.Tables.Add(dtCopy);
			
			return ds;
		}

		[Ajax.AjaxMethod]
		public string ConsultarInfoReferenciaInventarioFisico(string codigoReferencia, int numeroTarjeta, string prefijoInv, int numeroInv)
		{
			string salida = "";

			InventarioFisico inst = new InventarioFisico(prefijoInv,numeroInv);
			ItemsInventarioFisico instItem = inst.BuscarInstanciaItem(codigoReferencia,numeroTarjeta);
			
			if(instItem.NumeroTarjeta > -1)
				salida = instItem.ConteoActual+"&"+instItem.Conteo1+"&"+instItem.Conteo2+"&"+instItem.Conteo3;
			
			return salida;
		}

		#endregion

		#region Metodos

		private void CargarConteosRealizados(ArrayList conteos)
		{
			ddlConteosRelacionados.Items.Clear();

			for(int i=0;i<conteos.Count;i++)
			{
				if((int)conteos[i] <= 3)
					ddlConteosRelacionados.Items.Add(new ListItem(conteos[i].ToString(),conteos[i].ToString()));
			}	
		}

		private void CargarItemsRelacionadosConteo(int conteoRelacionado, InventarioFisico inst)
		{
			ddlReferencias.Items.Clear();

			DataRow[] select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+"","dinv_mite_codigo ASC");
			
			for(int i=0;i<select.Length;i++)
				ddlReferencias.Items.Add(new ListItem(select[i]["referencia_editada"].ToString()+"  -  "+select[i]["dinv_mite_nombre"].ToString(),select[i]["dinv_mite_codigo"].ToString()+"$"+select[i]["dinv_tarjeta"].ToString()));
		}

		private void CargarTarjetasRelacionadosConteo(int conteoRelacionado, InventarioFisico inst)
		{
			ArrayList arrTjsAdd = new ArrayList();

			ddlTarjetas.Items.Clear();

			DataRow[] select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+"","dinv_tarjeta ASC");
			
			for(int i=0;i<select.Length;i++)
			{
				if(arrTjsAdd.BinarySearch(Convert.ToInt32(select[i]["dinv_tarjeta"])) < 0)
				{
					ddlTarjetas.Items.Add(new ListItem(select[i]["dinv_tarjeta"].ToString(),select[i]["dinv_tarjeta"].ToString()));

					arrTjsAdd.Add(Convert.ToInt32(select[i]["dinv_tarjeta"]));
					arrTjsAdd.Sort();
				}
			}
		}

		private void CargarInfoReferencia(string codReferencia, int numeroTarjeta, InventarioFisico inst)
		{
			ItemsInventarioFisico instItem = inst.BuscarInstanciaItem(codReferencia,numeroTarjeta);
			if(instItem.NumeroTarjeta > -1)
			{
				lbUltimoConteo.Text = instItem.ConteoActual.ToString();
				lbConteo1.Text = instItem.Conteo1.ToString();
				lbConteo2.Text = instItem.Conteo2.ToString();
				lbConteo3.Text = instItem.Conteo3.ToString();
			}
		}

		private void CargarItemsConteo()
		{
			dgItemsConteo.DataSource = CargarItemsRelacionadosConteoFiltros(Convert.ToInt32(ddlConteosRelacionados.SelectedValue),(ddlInventarios.SelectedValue.Split('-'))[0],Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1]));
			scriptValidacion = "<script>var ElementosValidar  = new Array();";
			dgItemsConteo.DataBind();
			scriptValidacion += "</script>";
			Page.RegisterClientScriptBlock("validacion",scriptValidacion);
			btnGrabar.Visible = true;
		}

		private void CargarInfoConteoRequest()
		{
			if(ddlConteosRelacionados.Items.Count > 0)
			{
				//string prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
				//int numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
                string prefijoInventario = "";
                int numeroInventario = 0;

                if (ddlInventarios.SelectedValue.Split('-').Length == 3)
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

                }
                else
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1]);
                }
                
                
                
                
                InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
				CargarItemsRelacionadosConteo(Convert.ToInt32(ddlConteosRelacionados.SelectedValue),inst);
				ddlReferencias.SelectedValue = Request[ddlReferencias.UniqueID];
			}
		}

		private void CargarInfoTarjetasRequest()
		{
			if(ddlConteosRelacionados.Items.Count > 0)
			{
				//string prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
				//int numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
                string prefijoInventario = "";
                int numeroInventario = 0;

                if (ddlInventarios.SelectedValue.Split('-').Length == 3)
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

                }
                else
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1]);
                }
                
                
                
                
                
                
                
                InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
				CargarTarjetasRelacionadosConteo(Convert.ToInt32(ddlConteosRelacionados.SelectedValue),inst);
				ddlTarjetas.SelectedValue = Request[ddlTarjetas.UniqueID];
				if(rblReferencias.SelectedValue == "GT" || rblReferencias.SelectedValue == "GI")
					ddlTarjetas.Enabled = false;
				else if(rblReferencias.SelectedValue == "GJ")
					ddlTarjetas.Enabled = true;
			}
		}

		public DataSet CargarItemsRelacionadosConteoFiltros(int conteoRelacionado, string prefijoInv, int numeroInv)
		{
			InventarioFisico inst = new InventarioFisico(prefijoInv,numeroInv);
			
			DataSet ds = new DataSet();
			
			DataTable dtCopy = inst.DtItemsInventarioFisico.Clone();
			
			DataRow[] select = null;
			
			if(rblReferencias.SelectedValue == "GT" && rblTipoConteo.SelectedValue == "A")
				select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1),"dinv_tarjeta ASC");
			else if(rblReferencias.SelectedValue == "GT" && rblTipoConteo.SelectedValue == "SA")
			{
				if((conteoRelacionado-1) == 0) //Caso conteo 1 : No se revisa contra ninguno
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1),"dinv_tarjeta ASC");
				else if((conteoRelacionado-1) == 1) //Caso conteo 2 : Se revisa contra el saldo almacenado
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_conteo1 <> dinv_msal_cantactual","dinv_tarjeta ASC");
				else if((conteoRelacionado-1) == 2) //Caso conteo 3 : Se revisa contra el conteo anterior es decir el los que tienen diferencias entre el 1 y el 2
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_conteo1 <> dinv_conteo2","dinv_tarjeta ASC");
			}
			else if(rblReferencias.SelectedValue == "GI" && rblTipoConteo.SelectedValue == "A")
				select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_mite_codigo='"+(ddlReferencias.SelectedValue.Split('$'))[0]+"' AND dinv_tarjeta="+(ddlReferencias.SelectedValue.Split('$'))[1]+"","dinv_tarjeta ASC");
			else if(rblReferencias.SelectedValue == "GI" && rblTipoConteo.SelectedValue == "SA")
			{
				if((conteoRelacionado-1) == 0) //Caso conteo 1 : No se revisa contra ninguno
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_mite_codigo='"+ddlReferencias.SelectedValue.Split('$')[0]+"' AND dinv_tarjeta="+ddlReferencias.SelectedValue.Split('$')[1]+"","dinv_tarjeta ASC");
				else if((conteoRelacionado-1) == 1) //Caso conteo 2 : Se revisa contra el saldo almacenado
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_conteo1 <> dinv_msal_cantactual AND dinv_mite_codigo='"+ddlReferencias.SelectedValue.Split('$')[0]+"' AND dinv_tarjeta="+ddlReferencias.SelectedValue.Split('$')[1]+"","dinv_tarjeta ASC");
				else if((conteoRelacionado-1) == 2) //Caso conteo 3 : Se revisa contra el conteo anterior es decir el los que tienen diferencias entre el 1 y el 2
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_conteo1 <> dinv_conteo2 AND dinv_mite_codigo='"+ddlReferencias.SelectedValue.Split('$')[0]+"' AND dinv_tarjeta="+ddlReferencias.SelectedValue.Split('$')[1]+"","dinv_tarjeta ASC");
			}
			else if(rblReferencias.SelectedValue == "GJ" && rblTipoConteo.SelectedValue == "A")
				select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_tarjeta="+ddlTarjetas.SelectedValue+"","dinv_tarjeta ASC");
			else if(rblReferencias.SelectedValue == "GJ" && rblTipoConteo.SelectedValue == "SA")
			{
				if((conteoRelacionado-1) == 0) //Caso conteo 1 : No se revisa contra ninguno
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_tarjeta="+ddlTarjetas.SelectedValue+"","dinv_tarjeta ASC");
				else if((conteoRelacionado-1) == 1) //Caso conteo 2 : Se revisa contra el saldo almacenado
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_conteo1 <> dinv_msal_cantactual AND dinv_tarjeta="+ddlTarjetas.SelectedValue+"","dinv_tarjeta ASC");
				else if((conteoRelacionado-1) == 2) //Caso conteo 3 : Se revisa contra el conteo anterior es decir el los que tienen diferencias entre el 1 y el 2
					select = inst.DtItemsInventarioFisico.Select("dinv_conteoactual="+(conteoRelacionado-1)+" AND dinv_conteo1 <> dinv_conteo2 AND dinv_tarjeta="+ddlTarjetas.SelectedValue+"","dinv_tarjeta ASC");
			}

			for(int i=0;i<select.Length;i++)
			{
				DataRow dr = dtCopy.NewRow();

				for(int j=0;j<inst.DtItemsInventarioFisico.Columns.Count;j++)
					dr[j] = select[i][j];

				dtCopy.Rows.Add(dr);
			}

			ds.Tables.Add(dtCopy);
			
			return ds;
		}
		#endregion
		
		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgItemsConteo.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgItemsConteo_ItemDataBound);

		}
		#endregion
	}
}
