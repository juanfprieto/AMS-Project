
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
    using AMS.Documentos;
	using AMS.Inventarios;
using AMS.Tools;

namespace AMS.Inventarios
{
	/// <summary>
	///		Descripción breve de AMS_Inventarios_ImportarPedidos.
	/// </summary>
	public partial class AMS_Inventarios_ImportarPedidos : System.Web.UI.UserControl
	{
		#region Constantes

		private const string SALTOLINEA = "<BR>";

		private const int PREFIJOPEDIDO = 0;
		private const int NUMEROPEDIDO = 1;
		private const int PREFIJOORDENTALLER = 2;
		private const int NUMEROORDENTALLER = 3;
		private const int NIT = 4;
		private const int ALMACEN = 5;
		private const int VENDEDOR = 6;
		private const int CLASE = 7;
		private const int CARGO = 8;
		private const int FECHA = 9;
		private const int OBSERVACIONES = 10;

		private const int CODIGOITEM = 0;
		private const int PRECIO = 1;
		private const int PORCENTAJEIVA = 2;
		private const int PORCENTAJEDESCUENTO = 3;
		private const int CANTIDADPEDIDO = 4;
		private const int LINEAITEM = 5;

		#endregion

		#region Campos


		#endregion

		#region Propiedades

		public string NombreRangoMaestro{get{return txtNombreRangoMaestro.Text;}}
		public string NombreRangoDetalle{get{return txtNombreRangoDetalle.Text;}}

		#endregion

		#region Metodos

		private bool ValidarArchivoExcel(string nombreArchivo,ref string archivo)
		{
			bool valido = false;

			if(nombreArchivo == string.Empty)
			{
                Utils.MostrarAlerta(Response, "No ha seleccionado un archivo.");
				valido = false;
			}
			else
			{
				string[] carpetas = nombreArchivo.Split('\\');
				archivo = carpetas[carpetas.Length - 1];
				string[] archivoExtension = archivo.Split('.');
				string extension = archivoExtension[archivoExtension.Length - 1];

				if(extension.ToUpper() != "XLS")
				{
                    Utils.MostrarAlerta(Response, "No es un archivo de Excel.");
					valido = false;
				}
				else
				{
					valido = true;
				}
			}

			return valido;
		}

		private string CargarArchivoExcel(string nombreArchivo,HtmlInputFile htmlInputFile)
		{
			string rutaImportarExcel = ConfigurationManager.AppSettings["PathToImportsExcel"];
			string rutaArchivoExcel = "";
			string archivo = "";

			if (ValidarArchivoExcel(nombreArchivo,ref archivo))
			{
				rutaArchivoExcel = rutaImportarExcel + archivo;
				htmlInputFile.PostedFile.SaveAs(rutaArchivoExcel);
			}

			return rutaArchivoExcel;
		}

		public bool Importar(DataSet maestroPedidoDataSet, DataSet detallePedidoDataSet)
		{
			bool satisfactorio = false;

			string resultados = string.Empty;

			foreach(DataTable maestroPedidoDataTable in maestroPedidoDataSet.Tables)
			{
				foreach(DataRow maestroPedidoDataRow in maestroPedidoDataTable.Rows)
				{
					bool valido = false;

					int numeroOrdenTaller = 0;
			
					string tipoPedido = DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido where pped_codigo='"+maestroPedidoDataRow[PREFIJOPEDIDO]+"'");
			
					try
					{
						if(tipoPedido=="T")
						{
							numeroOrdenTaller = 0;

							if((maestroPedidoDataRow[PREFIJOORDENTALLER].ToString() == "") || (maestroPedidoDataRow[NUMEROORDENTALLER].ToString() == ""))
							{
								resultados += string.Format("El pedido {0} - {1} es una trasferencia de taller, pero no tiene una orden de trabajo asociada.{2}",maestroPedidoDataRow[PREFIJOPEDIDO],maestroPedidoDataRow[NUMEROPEDIDO],SALTOLINEA);
								valido = false;
							}	 
							if(DBFunctions.SingleData("SELECT test_estado FROM MORDEN WHERE pdoc_codigo='"+maestroPedidoDataRow[PREFIJOORDENTALLER]+"' AND MORD_NUMEORDE="+maestroPedidoDataRow[NUMEROORDENTALLER]) != "A")
							{
								resultados += string.Format("El prefijo, numero o estado de la orden de taller {0} - {1} de la orden de pedido {2} - {3} no es valido.{4}",maestroPedidoDataRow[PREFIJOORDENTALLER],maestroPedidoDataRow[NUMEROORDENTALLER],maestroPedidoDataRow[PREFIJOPEDIDO],maestroPedidoDataRow[NUMEROPEDIDO],SALTOLINEA);
								valido = false;
							}

							try {numeroOrdenTaller = Convert.ToInt32(maestroPedidoDataRow[NUMEROORDENTALLER]);} 
							catch { };
						}
					}
					catch (Exception excepcion){resultados = excepcion.ToString();}

					//Constructor Tipo 2 Solo Pedido
					PedidoFactura pedidoFactura = new PedidoFactura(
						tipoPedido, // 1 Tipo de Pedido
						maestroPedidoDataRow[PREFIJOPEDIDO].ToString(), // 2 Prefijo Documento
						maestroPedidoDataRow[NIT].ToString(), // 3 Nit
						maestroPedidoDataRow[ALMACEN].ToString(), // Almacen
						maestroPedidoDataRow[VENDEDOR].ToString(), // 5 Vendedor
						Convert.ToUInt32(maestroPedidoDataRow[NUMEROPEDIDO]), // 6 Numero Pedido
						maestroPedidoDataRow[PREFIJOORDENTALLER].ToString(), // 7 Prefijo OT
						Convert.ToUInt32(maestroPedidoDataRow[NUMEROORDENTALLER]), // 8 Numero OT
						maestroPedidoDataRow[CLASE].ToString(), // 9 Tipo de Pedido
						maestroPedidoDataRow[CARGO].ToString(), // 10 Cargo
						Convert.ToDateTime(maestroPedidoDataRow[FECHA]),  // 11 Fecha
						maestroPedidoDataRow[OBSERVACIONES].ToString(),
                        null // 12 Observaciones
						);

					DataRow[] detallePedidoDataRows = detallePedidoDataSet.Tables[0].Select(PREFIJOPEDIDO+"="+maestroPedidoDataRow[PREFIJOPEDIDO]+" AND "+NUMEROPEDIDO+"="+maestroPedidoDataRow[NUMEROPEDIDO]);

					foreach(DataRow detallePedidoDataRow in detallePedidoDataRows) //Se agregan las filas que detallan el pedido
					{
						string codigoItemInvertido= "";

						Referencias.Guardar((string)detallePedidoDataRow[CODIGOITEM],ref codigoItemInvertido,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+detallePedidoDataRow[LINEAITEM]+"'"));
						pedidoFactura.InsertaFila(
							codigoItemInvertido, // Codigo de Item
							0, // Cantidad Facturada
							Convert.ToDouble(detallePedidoDataRow[PRECIO]), // Precio
							Convert.ToDouble(detallePedidoDataRow[PORCENTAJEIVA]), // Porcentaje IVA
							Convert.ToDouble(detallePedidoDataRow[PORCENTAJEDESCUENTO]), // Porcentaje Descuento
							Convert.ToDouble(detallePedidoDataRow[CANTIDADPEDIDO]), // Cantidad Pedida
                            "",// Codigo pedido
                            ""// Numero pedido
                             );
					}

					bool status = true;

					bool realizoPedido = pedidoFactura.RealizarPed(ref status,true);
					
					if(status)
					{
						Session.Clear();
					}
					else
						lb.Text += pedidoFactura.ProcessMsg;
				}
			}
			
			//txtNumPed.Text = DBFunctions.SingleData("SELECT pped_ultipedi+1 FROM ppedido WHERE pped_codigo='"+ddlCodigo.SelectedValue+"'");

			return satisfactorio;
		}

		#endregion

		#region Eventos

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(Request.QueryString["ext"]!=null)
                    Utils.MostrarAlerta(Response, "Proceso Exitoso.\\nSe ha logrado ingresar la totalidad de los registros");
			}
		}

		public void btnSubir_Click(object sender, System.EventArgs e)
		{
			DataSet maestroPedidoDataSet = new DataSet();
			DataSet detallePedidoDataSet = new DataSet();

			string nombreArchivoMaestro = CargarArchivoExcel(inpArchivoMaestro.PostedFile.FileName.ToString(),inpArchivoMaestro);

			if (nombreArchivoMaestro != string.Empty)
			{
				ExcelFunctions excel = new ExcelFunctions(nombreArchivoMaestro);
				excel.Request(maestroPedidoDataSet,IncludeSchema.NO,"SELECT * FROM " + NombreRangoMaestro);
			}

			string nombreArchivoDetalle = CargarArchivoExcel(inpArchivoDetalle.PostedFile.FileName.ToString(),inpArchivoDetalle);

			if (nombreArchivoDetalle != string.Empty)
			{
				ExcelFunctions excel = new ExcelFunctions(nombreArchivoDetalle);
				excel.Request(detallePedidoDataSet,IncludeSchema.NO,"SELECT * FROM " + NombreRangoDetalle);
			}

			//Si el commit no devolvio errores, lo redirecciono a la misma página
			if(Importar(maestroPedidoDataSet,detallePedidoDataSet))
			{
				Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Inventarios.ImportarPedidos&ext=1");
			}
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

		}
		#endregion
	}
}
