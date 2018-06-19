using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class Validador : System.Web.UI.UserControl 
	{
		protected DataTable dtGastos;

		protected void ArmarGrilla()
		{
			dtGastos = new DataTable();
			dtGastos.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			dtGastos.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			dtGastos.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.Double")));
		}

		protected void ingresarDatos(string codigo,string nombre,double valor)
		{
			DataRow fila = dtGastos.NewRow();
			fila["CODIGO"]=codigo;
			fila["NOMBRE"]=nombre;
			fila["COSTO"]=valor;
			dtGastos.Rows.Add(fila);
			dgGastos.DataSource=dtGastos;
			dgGastos.DataBind();
			
			DatasToControls.Aplicar_Formato_Grilla(dgGastos);
			//DatasToControls.JustificacionGrilla(DATAGRIDPERESCOGIDO,DataTable2);
            
		}

		protected void validar(object sender, EventArgs e )
		{
			pan.Visible=false;
			int i ;
			lb.Text="";
			DataSet precios = new DataSet();
			double gastosVehiculo=0;
			double utilidad=0,costo=0,utilidadSugerida=0,porcUtilidad=0;
			//proceso: costo vehiculo-beneficio-gastos directos= costo.
            DBFunctions.Request(precios, IncludeSchema.NO, "select COALESCE(ppre_costo,0), COALESCE(ppre_porcapoyo,0), COALESCE(ppre_utilsugerida,0), COALESCE(ppre_precio,0), COALESCE(ppre_dtomax,0) from DBXSCHEMA.PPRECIOVEHICULO where pcat_codigo='" + ddlcatalogo.SelectedValue.ToString() + "'");
			if(precios.Tables[0].Rows.Count==0)
			{	
                Utils.MostrarAlerta(Response, "No se encontraron precios para el Vehículo seleccionado");
			}
			else
			{
				if (dgGastos.Items.Count==0)
				{
                    Utils.MostrarAlerta(Response, "No ha ingresado ningun Gasto,por favor revise..");
					gastosVehiculo=0;
				}
				else
				{
					for (i=0;i<dgGastos.Items.Count;i++)
					{
						if (   ( (CheckBox)dgGastos.Items[i].Cells[3].FindControl("chkBox")).Checked   )
						{
							gastosVehiculo+=Convert.ToDouble(dgGastos.Items[i].Cells[2].Text.Substring(1));

						}
					}
				}
			
				if (valVenta.Text==String.Empty)
				{
                    Utils.MostrarAlerta(Response, "Ingrese el Valor Venta del Vehículo");
				}
				else
				{
					pan.Visible=true;
					costo= double.Parse(precios.Tables[0].Rows[0][0].ToString())
						 - (double.Parse(precios.Tables[0].Rows[0][0].ToString())
						   *double.Parse(precios.Tables[0].Rows[0][1].ToString())/100)
						   +gastosVehiculo;
					utilidad=double.Parse(valVenta.Text)-costo;
					porcUtilidad=(utilidad*100)/costo;
					//utilidad sugerida
					utilidadSugerida=(double.Parse(precios.Tables[0].Rows[0][3].ToString())
						            *(1-(double.Parse(precios.Tables[0].Rows[0][4].ToString())/100))) 
						            +gastosVehiculo-costo;
					lbCosto.Text=costo.ToString("C");
					lbUtilidad.Text=utilidad.ToString("C");
					lbPorcentaje.Text=Math.Round(porcUtilidad,2).ToString()+"%";
					lbUtilSugerida.Text=utilidadSugerida.ToString("C");
					//semaforo
					if(utilidad>=utilidadSugerida)
					{
						lb.Text+="<font color=green>Validación APROBADA</font>";
					}
					else
					{
						lb.Text="<font color=red> Validación NO Aprobada </font> <br>";
					  	double valorMin=costo+utilidadSugerida;
						lb.Text+="Valor Minimo "+valorMin.ToString("C")+"";
					}
				}
			}
		}

		protected void Page_Load(object sender , EventArgs e)
		{
			if (!IsPostBack)
			{
				DataSet costos = new DataSet();
				int i;
				DatasToControls pens= new DatasToControls();
				pens.PutDatasIntoDropDownList(ddlcatalogo,CatalogoVehiculos.CATALOGOVEHICULOSLISTA);

            //    DBFunctions.Request(costos, IncludeSchema.NO, "Select pgas_codigo,pgas_nombre,pgas_costprom from DBXSCHEMA.PGASTODIRECTO where pgas_indicostvehi='S' ORDER BY pgas_nombre");
                DBFunctions.Request(costos, IncludeSchema.NO, "Select pITE_codigo, pITE_nombre, ROUND(PITE_COSTO*1.16,0) from DBXSCHEMA.PITEMVENTAVEHICULO ORDER BY 2");
                this.ArmarGrilla();
			
				for(i=0;i<costos.Tables[0].Rows.Count;i++)
				{
					this.ingresarDatos(""+costos.Tables[0].Rows[i][0].ToString()+"",""+costos.Tables[0].Rows[i][1].ToString()+"",double.Parse(costos.Tables[0].Rows[i][2].ToString()));
				}
			}
		}

		//protected HtmlInputFile fDocument;
		
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
