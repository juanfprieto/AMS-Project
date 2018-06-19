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
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class AMS_Inventarios_CapturaDatosTarjeta : System.Web.UI.UserControl
	{
		#region Atributos
		protected System.Web.UI.WebControls.RadioButtonList rblTipoConteo;
		private DatasToControls bind = new DatasToControls();
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
			// Registra la clase en Ajax.
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Inventarios_CapturaDatosTarjeta));
		
			if(!IsPostBack)
			{
				// Carga el DropDownList con la lista de inventarios.
				bind.PutDatasIntoDropDownList(ddlInventarios,"SELECT INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),COUNT(DINV.dinv_mite_codigo) "+
					"FROM minventariofisico INF INNER JOIN dinventariofisico DINV ON INF.pdoc_codigo = DINV.pdoc_codigo AND INF.minf_numeroinv = DINV.minf_numeroinv "+
					"WHERE INF.minf_fechacierre IS NULL AND INF.minf_fechainicio <= '"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND minf_fechacierre is null "+
					"GROUP BY INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)), CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)) "+
					"HAVING COUNT(DINV.dinv_mite_codigo) > 0 "+
					"ORDER BY CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10))");
				
				ddlInventarios.Items.Insert(0,new ListItem("Seleccione...",String.Empty));

				// Carga el DropDownList con la lista de tarjetas de alta.
				bind.PutDatasIntoDropDownList(ddlLineaTarjAlta,"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				
				// Asocia los eventos a los controles pasando los parametros respectivos.
				tbCodRefAlta.Attributes.Add("ondblclick","MostrarRefs("+tbCodRefAlta.ClientID+","+ddlLineaTarjAlta.ClientID+");");
				tbCodRefAlta.Attributes.Add("onkeyup","ItemMask("+tbCodRefAlta.ClientID+","+ddlLineaTarjAlta.ClientID+");");
				ddlLineaTarjAlta.Attributes.Add("onchange","ChangeLine("+ddlLineaTarjAlta.ClientID+","+tbCodRefAlta.ClientID+");");
				btnAceptar.Attributes.Add("onclick","return ValSelInvFis();");
			}

			// Asocia los eventos a los controles.
			btnGuardarConteo.Attributes.Add("onclick","return ValidarValorConteo();");
			btnGrabarTarjAlta.Attributes.Add("onclick","return ValidarInformacionTarjetaAlta();");
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			if(ddlInventarios.SelectedIndex > 0)
			{
				// Muestra el panel de información de proceso.
				ddlInventarios.Enabled = false;
				pnlInfoProceso.Visible = true;

				// Carga el código del inventario físico.
                string prefijoInventario;
                int numeroInventario;


                if (ddlInventarios.SelectedValue.Split('-').Length == 3)
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

                }
                else
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
                }

                
				
				// Crea una instancia del inventario físico.
				InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
				
				// Carga los conteos realizados en el DropDownList.
				CargarConteosRealizados(inst.ConteosPendientesInstancia);

				if(ddlConteosRelacionados.Items.Count > 0)
				{
                    bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen,palm_descripcion FROM palmacen WHERE palm_almacen IN (SELECT dinv_palm_almacen FROM dinventariofisico WHERE pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario + ") and tvig_vigencia='V' ORDER BY palm_descripcion");
					
					CargarUbicaciones(ddlUbicacion,ddlAlmacen);
					CargarTarjetaConteo(prefijoInventario,numeroInventario,Convert.ToInt32(ddlConteosRelacionados.SelectedValue));

					pnlTarjetaPendiente.Visible = true;
				}
				else
					pnlTarjetaPendiente.Visible = false;

                bind.PutDatasIntoDropDownList(ddlAlmacenAlta, "SELECT palm_almacen,palm_descripcion FROM palmacen WHERE palm_almacen IN (SELECT dinv_palm_almacen FROM dinventariofisico WHERE pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario + ") and tvig_vigencia='V' ORDER BY palm_descripcion");
				
				CargarUbicaciones(ddlUbicacionAlta,ddlAlmacenAlta);
				CargarTarjetaConteoAlta(prefijoInventario,numeroInventario);
								pnlInfoProceso.Visible = true;
			}
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.CapturaDatosTarjeta");
		}

		protected void btnGuardarConteo_Click(object sender, System.EventArgs e)
		{
            if(tbNumeroTarjeta.Text == "")
            {
                Utils.MostrarAlerta(Response, "Digíte un número de Secuencia/Tarjeta válido !!!");
                return;
            }
            if (tbCantidadConteo.Text == "")
            {
                Utils.MostrarAlerta(Response, "Digíte un valor de conteo válido !!!, si no hay existencia digite 0 (cero)");
                return;
            }
            string prefijoInventario;
            int numeroInventario;


            if (ddlInventarios.SelectedValue.Split('-').Length == 3)
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
            }



			string codigoOriginalItem   = hdCodRef.Value;
			string codigoModificadoItem = lbCodigoReferencia.Text;
			
			int numeroTarjeta           = Convert.ToInt32(tbNumeroTarjeta.Text);
			int conteoRelacionado       = Convert.ToInt32(hdConteoActual.Value)-1;
			double cantidadIngresada    = Convert.ToDouble(tbCantidadConteo.Text);
			int codigoUbicacion         = Convert.ToInt32(Request[ddlUbicacion.UniqueID]);

            string nombUbi = ddlUbicacion.SelectedItem.Text.Split(']')[0].Substring(1);


			string salida = InventarioFisico.ModificarEstadoConteoRenglon(prefijoInventario,numeroInventario,codigoOriginalItem,numeroTarjeta,conteoRelacionado,cantidadIngresada,codigoModificadoItem,codigoUbicacion, nombUbi);
			
			if(salida != String.Empty)
                Utils.MostrarAlerta(Response, ""+salida+"");
			else
			{
				InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
				
				CargarConteosRealizados(inst.ConteosPendientesInstancia);
				
				try
				{
					ddlConteosRelacionados.SelectedValue = Request[ddlConteosRelacionados.UniqueID];
				}
				catch { }

				if(ddlConteosRelacionados.Items.Count > 0)
				{
                    bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen,palm_descripcion FROM palmacen WHERE palm_almacen IN (SELECT dinv_palm_almacen FROM dinventariofisico WHERE pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario + ") and tvig_vigencia='V' ORDER BY palm_descripcion");
					CargarUbicaciones(ddlUbicacion,ddlAlmacen);
					CargarTarjetaConteo(prefijoInventario,numeroInventario,Convert.ToInt32(ddlConteosRelacionados.SelectedValue));
				}
			}
		}

		protected void btnGrabarTarjAlta_Click(object sender, System.EventArgs e)
		{
			
            string prefijoInventario;
            int numeroInventario;


            if (ddlInventarios.SelectedValue.Split('-').Length == 3)
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
            }

            

			string lineaInventario   = ddlLineaTarjAlta.SelectedValue.Split('-')[1];
			int    numeroTarjeta = -1;
			int    codigoUbicacionInicial = Convert.ToInt32(ddlUbicacionAlta.SelectedValue);
			string almacen    = ddlAlmacenAlta.SelectedValue;
			int    conteoAlta = Convert.ToInt32(tbConteoAlta.Text);
			string codigoItemModificado = tbCodRefAlta.Text;
		
			if(!VerificarCodigoReferencia(lineaInventario,tbCodRefAlta.Text))
			{
                Utils.MostrarAlerta(Response, "- El código de ítem ingresado es invalido");
				return;
			}
			
			if(hdNumTarjAlta.Value != "-1")
			{
				try{numeroTarjeta = Convert.ToInt32(hdNumTarjAlta.Value);}

                catch { Utils.MostrarAlerta(Response, "- Número de tarjeta de alta invalido"); return; }
			}
			
			#region Código candidato a eliminación
			/*
			if(!VerificarUsoTarjetaAlta(prefijoInventario,numeroInventario,numeroTarjeta))
			{
				Page.RegisterClientScriptBlock("status","<script>alert('- Esta Tarjeta de alta ya fue utilizada o es un número invalido');</script>");
				return;
			}
			*/
			#endregion

			string codI = "";
			
			Referencias.Guardar(codigoItemModificado,ref codI,lineaInventario);

			ItemsInventarioFisico itemsInventarioFisico = new ItemsInventarioFisico(
				codI,
				Items.NombreReferencia(codI),
				2,
				numeroTarjeta,
				codigoUbicacionInicial,
				almacen,
                0,
		//		SaldoItem.ObtenerCantidadActual(codI,ConfiguracionInventario.Ano), si es ALTA el saldo en libros debe ser CERO
				SaldoItem.ObtenerCostoPromedio(codI,ConfiguracionInventario.Ano),
				conteoAlta,
                conteoAlta,
				-1,
				DateTime.Now,
				new DateTime(),
				string.Empty,
				string.Empty,
                conteoAlta,
                conteoAlta * SaldoItem.ObtenerCostoPromedio(codI, ConfiguracionInventario.Ano),
                conteoAlta,
				codigoItemModificado);

            string salida = InventarioFisico.IngresarValorTarjetaAlta(prefijoInventario,numeroInventario,itemsInventarioFisico);
			
			if(salida == String.Empty)
                Utils.MostrarAlerta(Response, "Se ha ingresado la información de la tarjeta de alta "+numeroTarjeta+"");
			else
                Utils.MostrarAlerta(Response, "" + salida + "");
			
			CargarTarjetaConteoAlta(prefijoInventario,numeroInventario);

			ddlLineaTarjAlta.SelectedIndex = 0;
			tbCodRefAlta.Text = tbConteoAlta.Text = String.Empty;
			ddlAlmacenAlta.SelectedIndex = 0;

			CargarUbicaciones(ddlUbicacionAlta,ddlAlmacenAlta);
		}
		#endregion

		#region Metodos
        
		private void CargarUbicaciones(DropDownList ddlRelacionado, DropDownList ddlAlmacenRelacionado)
		{
			ddlRelacionado.Items.Add(new ListItem("Sin Ubicación","-1"));
				
			DataTable dtUbicaciones = InventarioFisico.ConsultarUbicacionesUltimoNivelPorAlmacen(ddlAlmacenRelacionado.SelectedValue);

			ddlRelacionado.DataSource = dtUbicaciones;
			ddlRelacionado.DataValueField = dtUbicaciones.Columns[0].ColumnName;
			ddlRelacionado.DataTextField = dtUbicaciones.Columns[1].ColumnName;
			
			ddlRelacionado.DataBind();

			ddlRelacionado.Items.Add(new ListItem("Sin Ubicación","-1"));
		}

		private void CargarConteosRealizados(ArrayList conteos)
		{
			ddlConteosRelacionados.Items.Clear();

			for(int i=0;i<conteos.Count;i++)
			{
				if((int)conteos[i] <= 3)
					ddlConteosRelacionados.Items.Add(new ListItem(conteos[i].ToString(),conteos[i].ToString()));
			}
		}

		private void CargarTarjetaConteo(string prefijoInventario, int numeroInventario, int conteo)
        /*
             ---   Sentencia para mirar la estructura logica de las ubicaciones por niveles de un ALMACEN   ---
            SELECT  P3.PUBI_CODIGO codigo_n3, P3.PUBI_NOMBRE nombre_n3, P3.PUBI_CODPAD padre_n3,
                    P2.PUBI_CODIGO codigo_n2, P2.PUBI_NOMBRE nombre_n2, P2.PUBI_CODPAD padre_n2,
                    P1.PUBI_CODIGO codigo_n1, P1.PUBI_NOMBRE nombre_n1, P1.PUBI_CODPAD padre_n1
            FROM  PUBICACIONITEM P3
            LEFT JOIN PUBICACIONITEM P2 ON P3.PUBI_CODPAD = P2.PUBI_CODIGO
            LEFT JOIN PUBICACIONITEM P1 ON P2.PUBI_CODPAD = P1.PUBI_CODIGO
            WHERE P3.PALM_ALMACEN = 'CA11' --AND P2.PUBI_CODIGO IS NOT NULL  -- PONER EL ALMACEN OBJETO DE LA CONSULTA
            ORDER BY 1,4,7; 
        */
        {
			string[] informacionTarjeta = TraerInformacionTarjetaConteo(ConsultarNumeroTarjetaSecuencia(prefijoInventario,numeroInventario,conteo),prefijoInventario,numeroInventario).Split('&');
			
			if(informacionTarjeta.Length > 1)
			{
				lbTxtIngr2.Text         = (Convert.ToInt32(informacionTarjeta[6])+1).ToString();
				tbNumeroTarjeta.Text    = informacionTarjeta[0];
				hdCodRef.Value          = informacionTarjeta[0];
				lbCodigoReferencia.Text = informacionTarjeta[1];
				hdCodRef.Value          = informacionTarjeta[2];
				lbNombreReferencia.Text = informacionTarjeta[3];
			
				if(ddlAlmacen.SelectedValue != informacionTarjeta[4])
				{
					// ddlAlmacen.SelectedValue = informacionTarjeta[4]; VER QUE HACE AQUI
                    informacionTarjeta[4] = ddlAlmacen.SelectedValue ;
					CargarUbicaciones(ddlUbicacion,ddlAlmacen);
				}
                try
                {
                    ddlUbicacion.SelectedValue = informacionTarjeta[5];
                }
                catch
                {
                    // Response.Write("La tarjeta tbNumeroTarjeta.Text tiene la Ubicacion ERRADA, modifique esta ubicacion);
                    ddlUbicacion.SelectedValue = null;
                }

				lbConteoActual.Text     = (Convert.ToInt32(informacionTarjeta[6])+1).ToString();
				hdConteoActual.Value    = (Convert.ToInt32(informacionTarjeta[6])+1).ToString();
				lbCantidadConteo1.Text  = informacionTarjeta[7];
				lbCantidadConteo2.Text  = informacionTarjeta[8];
				lbCantidadConteo3.Text  = informacionTarjeta[9];
				tbCantidadConteo.Text   = "";
				pnlTarjetaPendiente.Visible = true;
			}
			else
			{
				pnlTarjetaPendiente.Visible = false;
                Utils.MostrarAlerta(Response, "No se encuentran tarjetas para este conteo.");
			}
		}

		private void CargarTarjetaConteoAlta(string prefijoInventario, int numeroInventario)
		{
			int numeroTarjetaAlta       = ConsultarNumeroTarjetaSecuenciaAlta(prefijoInventario,numeroInventario);

			if(numeroTarjetaAlta == -1)
			{
				tbNumTarjAlta.ReadOnly  = true;
				tbNumTarjAlta.Text      = "default";
				hdNumTarjAlta.Value     = "-1";
			}
			else
			{
				tbNumTarjAlta.ReadOnly  = false;
				tbNumTarjAlta.Text      = hdNumTarjAlta.Value = numeroTarjetaAlta.ToString();
			}
		}

		private string TraerInformacionTarjetaConteo(int numeroTarjeta,string prefijoInventario,int numeroInventario)
		{
			// 0 - Numero de tarjeta
			// 1 - Codigo de referencia modificada
			// 2 - Codigo de referencia original
			// 3 - Nombre de referencia
			// 4 - Codigo de Almacen Relacionado
			// 5 - Codigo de Ubicación
			// 6 - Conteo Actual
			// 7 - Cantidad Ingresada 1
			// 8 - Cantidad Ingresada 2
			// 9 - Cantidad Ingresada 3
			string informacionTarjeta = String.Empty;

			ItemsInventarioFisico inst = new ItemsInventarioFisico(prefijoInventario,numeroInventario,numeroTarjeta);
			
			if(inst.ConteoActual != -1)
				inst.NombreItemRelacionado=inst.NombreItemRelacionado.Replace("&"," ");
			 //   inst.CodigoItemModificado =inst.CodigoItemModificado.Replace("&"," ");
			    inst.CodigoItemRelacionado=inst.CodigoItemRelacionado.Replace("&"," ");
				informacionTarjeta = inst.NumeroTarjeta+"&"+inst.CodigoItemModificado+"&"+inst.CodigoItemRelacionado+"&"+inst.NombreItemRelacionado+"&"+inst.CodigoAlmacen+"&"+inst.CodigoUbicacionInicial+"&"+inst.ConteoActual+"&"+inst.Conteo1+"&"+inst.Conteo2+"&"+inst.Conteo3;
			
			return informacionTarjeta;
		}

		#endregion

		#region Metodos Ajax
		[Ajax.AjaxMethod]
		public string TraerInformacionTarjeta(string prefijoInventario, int numeroInventario, int numeroTarjeta)
		{
			string informacionTarjeta = String.Empty;

			ItemsInventarioFisico inst = new ItemsInventarioFisico(prefijoInventario,numeroInventario,numeroTarjeta);
			
			if(inst.ConteoActual != -1)
				informacionTarjeta = inst.NumeroTarjeta+"&"+inst.CodigoItemModificado+"&"+inst.CodigoItemRelacionado+"&"+inst.NombreItemRelacionado+"&"+inst.CodigoAlmacen+"&"+inst.CodigoUbicacionInicial+"&"+inst.ConteoActual+"&"+inst.Conteo1+"&"+inst.Conteo2+"&"+inst.Conteo3;
			
			return informacionTarjeta;
		}

		[Ajax.AjaxMethod]
		public int ConsultarNumeroTarjetaSecuencia(string prefijoInventario, int numeroInventario, int conteoRelacionado)
		{
			return InventarioFisico.ProximoNumeroTarjetaAConteo(prefijoInventario, numeroInventario, conteoRelacionado);
		}

		[Ajax.AjaxMethod]
		public int ConsultarNumeroTarjetaSecuenciaAlta(string prefijoInventario, int numeroInventario)
		{
			return InventarioFisico.ProximoNumeroTarjetaAltaNueva(prefijoInventario,numeroInventario);
		}

		#region Código candidato a eliminación
		/*
		[Ajax.AjaxMethod]
		public bool VerificarNumeroTarjetaAlta(string prefijoInventario, int numeroInventario, int numeroTarjeta)
		{
			return InventarioFisico.ConsultarExistenciaTarjetaAlta(prefijoInventario,numeroInventario,numeroTarjeta);
		}
		*/
		#endregion

		[Ajax.AjaxMethod]
		public bool VerificarCodigoReferencia(string lineaInventario, string codigoModificado)
		{
			return Items.ValidarExistenciaItem(lineaInventario,codigoModificado);
		}

		#region Código candidato a eliminación
		/*
		[Ajax.AjaxMethod]
		public bool VerificarUsoTarjetaAlta(string prefijoInventario, int numeroInventario, int numeroTarjeta)
		{
			return InventarioFisico.ConsultarExistenciaTarjetaAlta(prefijoInventario,numeroInventario,numeroTarjeta);
		}
		*/
		#endregion
		
		[Ajax.AjaxMethod]
		public DataTable ConsultaUbicacionesPorAlmacen(string codigoAlmacen)
		{
			return InventarioFisico.ConsultarUbicacionesUltimoNivelPorAlmacen(codigoAlmacen);
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
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
