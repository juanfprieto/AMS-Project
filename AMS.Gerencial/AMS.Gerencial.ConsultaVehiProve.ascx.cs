namespace AMS.Gerencial
{
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
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
	using System.Text;
	using AMS.Tools;
	using System;

	/// <summary>
	///		Descripción breve de AMS_Gerencial_ConsultaVehiProve.
	/// </summary>
	public partial class AMS_Gerencial_ConsultaVehiProve : System.Web.UI.UserControl
	{
		protected DataSet lineas;
		protected DataTable resultado;
		protected DataSet lineas2;
		protected DataTable resultado2;
		int totalN=0;
		double valorTotal=0;
		int totalasignados=0;
		int totafacturados=0;
		int cantVende=0;
		double totVen=0;
		double valorPedi=0;
		int cantidadVehiculos=0;
		int cantped=0;
		int cantAsignados=0;
		int cantFacturados=0;
		int catPedidosG=0;
		int metas=0;
		int dia1=1;
		int dia2=1;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Request.QueryString["error"] != null)
                Utils.MostrarAlerta(Response, "Se debe parametrizar tablas C  ");
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ano1, "SELECT pano_ano    FROM DBXSCHEMA.pano order by 1 desc");
                bind.PutDatasIntoDropDownList(ano2, "SELECT pano_ano    FROM DBXSCHEMA.pano order by 1 desc");
                bind.PutDatasIntoDropDownList(mes1, "SELECT pmes_nombre FROM DBXSCHEMA.pmes order by pmes_mes");
                bind.PutDatasIntoDropDownList(mes2, "SELECT pmes_nombre FROM DBXSCHEMA.pmes order by pmes_mes");
                //bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM PVENDEDOR");
				DiaFin.Text=DateTime.Now.Day.ToString();
				
			}
		
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
		protected  void  Generar_Click(Object  Sender, EventArgs e)
		{
			Panel2.Visible=true;
			Label8.Visible=true;
			///////////////////////////////////////////////////////
			this.PrepararTabla();
			this.PrepararTabla2();
			lineas = new DataSet();
			lineas2 = new DataSet();
			int Año1=Convert.ToInt32(ano1.SelectedValue.ToString());
			int Año2=Convert.ToInt32(ano2.SelectedValue.ToString());
			int Mes1=Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.PMES WHERE PMES_NOMBRE='"+mes1.SelectedValue.ToString()+"'"));
			int Mes2=Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.PMES WHERE PMES_NOMBRE='"+mes2.SelectedValue.ToString()+"'"));
			string dia1S=DiaInicio.Text;
			string dia2S=DiaFin.Text;
			if(dia1S.ToString().Equals(null) || dia1S.ToString().Equals("0"))
			{
				dia1=1;
			}
			else
			{
			dia1=Convert.ToInt32(DiaInicio.Text.ToString());
			}
			if(dia2S.ToString().Equals(null) || dia2S.ToString().Equals("0"))
			{
				dia2=Convert.ToInt32(DateTime.Now.Day);
			}
			else
			{
				dia2=Convert.ToInt32(DiaFin.Text.ToString());
			}

			string fecha = Año1 + "-" + Mes1 + "-" + dia1; 
			string fecha2= Año2 + "-" + Mes2 + "-" + dia2;
            DBFunctions.Request(lineas, IncludeSchema.NO, @"SELECT DISTINCT  MPVP.MPED_PEDIDO, DPVP.DPED_VALOUNIT,DPVP.PCAT_CODIGO,DPVP.DPED_CANTPEDI,DPVP.DPED_CANTINGR, PC.PCOL_CODIGO, PC.PCOL_DESCRIPCION 
                                                            FROM DBXSCHEMA.DPEDIDOVEHICULOPROVEEDOR DPVP,DBXSCHEMA.MPEDIDOVEHICULOPROVEEDOR MPVP, DBXSCHEMA.PCOLOR PC,DBXSCHEMA.PDOCUMENTO PD
                                                            WHERE MPVP.PDOC_CODIGO=PD.PDOC_CODIGO 
                                                            AND MPVP.MPED_NUMEPEDI=DPVP.MPED_NUMEPEDI 
                                                            AND MPVP.PDOC_CODIGO=DPVP.PDOC_CODIGO 
                                                            AND DPVP.PCOL_CODIGO=PC.PCOL_CODIGO  
                                                            and MPVP.MPED_PEDIDO BETWEEN '" + fecha + "' AND '" + fecha2 + "' ");
			
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				
				string codigovehi=lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				string NombreVehi=DBFunctions.SingleData("select PCAT_DESCRIPCION from DBXSCHEMA.PCATALOGOVEHICULO WHERE PCAT_CODIGO='"+codigovehi+"' ");
				
				
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				int cantidad=Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[3].ToString());
				
				fila= resultado.NewRow();
				double ValorU=Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[1].ToString());
				string ValorUF=String.Format("{0:C}",ValorU);
				fila["CODIGO"]	= lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["DESCRIPCION"]	=  NombreVehi.ToString();
				fila["VALOR"]	= ValorUF.ToString();
				fila["COLOR"]	= lineas.Tables[0].Rows[i].ItemArray[6].ToString();
				fila["CANTIDAD"]	= lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				totalN= totalN+1;
				valorTotal=valorTotal + (Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[1].ToString())* cantidad);
				cantidadVehiculos =	cantidadVehiculos + Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[3].ToString());
				resultado.Rows.Add(fila);
						

			}
			//DBFunctions.Request(lineas2, IncludeSchema.NO, "select distinct PVEN_CODIGO from DBXSCHEMA.MPEDIDOVEHICULO where MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' AND PVEN_CODIGO = '" + vendedor + "' order by PVEN_CODIGO");
            DBFunctions.Request(lineas2, IncludeSchema.NO, "select distinct PVEN_CODIGO from DBXSCHEMA.MPEDIDOVEHICULO where MPED_PEDIDO BETWEEN '" + fecha + "' AND '" + fecha2 + "' order by PVEN_CODIGO");
            /////////////////////////////////////////////////////     0                          
			
            int año = 0;
            int mes = 0;
            try
            {
                año = Convert.ToInt32(DBFunctions.SingleData("select PANO_ANO from DBXSCHEMA.CINVENTARIO"));
                mes = Convert.ToInt32(DBFunctions.SingleData("select PMES_MES from DBXSCHEMA.CINVENTARIO"));
            }
            catch
            {

                // Response.Redirect(indexPage + "?process=DBManager.Gerencial.ConsultaVehiProve&error=1");
                Response.Redirect(indexPage + "?process=Gerencial.ConsultaVehiProve&error=1");


            }
            
            
            
            
            
            for(int i=0;i<lineas2.Tables[0].Rows.Count;i++)
			{
				
				string nomVen=DBFunctions.SingleData("Select PVEN_NOMBRE from DBXSCHEMA.PVENDEDOR  WHERE PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' ");	
				string cantpedS=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' ");
			
				if(cantpedS.Equals(null)||cantpedS.Equals(""))
				{
					cantped=0;
				}
				else
				{
					cantped=Convert.ToInt32(DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' "));
				}
				string valorPediS=DBFunctions.SingleData("select sum(MPED_VALOUNIT) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' ");
				if(valorPediS.Equals(null) || valorPediS.Equals("") )
				{
					valorPedi=0;
				}
				else
				{
					valorPedi=Convert.ToDouble(DBFunctions.SingleData("select sum(MPED_VALOUNIT) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' "));
				}
				string cantAsignadosS=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'and TEST_TIPOESTA=20 and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' ");	
				if(cantAsignadosS.Equals(null)||cantAsignadosS.Equals("") )
				{
					cantAsignados=0;
				}
				else
				{
					cantAsignados=Convert.ToInt32(DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'and TEST_TIPOESTA=20 and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' "));	
				}
				string cantFacturadosS=DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'and TEST_TIPOESTA=30 and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' ");	
				if(cantFacturadosS.Equals(null) || cantFacturadosS.Equals(""))
				{
					cantFacturados=0;
				}
				else
				{
					cantFacturados=Convert.ToInt32(DBFunctions.SingleData("select count(*) from DBXSCHEMA.MPEDIDOVEHICULO where  PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"'and TEST_TIPOESTA=30 and MPED_PEDIDO BETWEEN '"+fecha+"' AND '"+fecha2+"' "));	
				}
				string metaS=DBFunctions.SingleData("select (MMAR_CANTUSADOS)+(MMAR_CANTNUEVOS)FROM DBXSCHEMA.MMARKETINGPRESUPUESTO WHERE PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND PANO_ANO="+año+" AND TMES_MES="+mes+" ");
				if(metaS.Equals(null) || metaS.Equals(""))
				{
					metas=0;
				}
				else
				{
					metas=Convert.ToInt32(DBFunctions.SingleData("select (MMAR_CANTUSADOS)+(MMAR_CANTNUEVOS)FROM DBXSCHEMA.MMARKETINGPRESUPUESTO WHERE PVEN_CODIGO='"+lineas2.Tables[0].Rows[i].ItemArray[0].ToString()+"' AND PANO_ANO="+año+" AND TMES_MES="+mes+" "));
				}
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila2;
				
				fila2= resultado2.NewRow();
				
				string PedidosF=String.Format("{0:C}",valorPedi);
				fila2["CODIGO2"]	= lineas2.Tables[0].Rows[i].ItemArray[0].ToString();
				fila2["NOMBRE"]	= nomVen.ToString();
				fila2["TOTALPEDIDOS"]	= PedidosF.ToString();
				fila2["PEDIDOS"]= cantped.ToString();
				fila2["ASIGNADOS"] = cantAsignados.ToString();
				fila2["FACTURADOS"] = cantFacturados.ToString();
				fila2["META"] = metas.ToString();
				cantVende=cantVende+1;
				totVen=totVen+valorPedi;
				catPedidosG=catPedidosG+cantped;
				
				totalasignados=totalasignados+cantAsignados;
				totafacturados=totafacturados+cantFacturados;

							
				resultado2.Rows.Add(fila2);
						

			}
			

			//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();
			Grid2.DataSource = resultado2;
			Grid2.DataBind();
			CantPedido.Text=totalN.ToString();
			string ValTF=String.Format("{0:C}",valorTotal);
			ValoPedidos.Text=ValTF.ToString();
			numven.Text=cantVende.ToString();
			string CantVenf=String.Format("{0:C}",totVen);
			totalven.Text=CantVenf.ToString();
			cantVehi.Text=cantidadVehiculos.ToString();
			totasig.Text=totalasignados.ToString();
			totfac.Text=totafacturados.ToString();
			totVehiP.Text=catPedidosG.ToString();
			
		
			
		}

		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn codigo = new DataColumn();
			codigo.DataType = System.Type.GetType("System.String");
			codigo.ColumnName = "CODIGO";
			codigo.ReadOnly=true;
			resultado.Columns.Add(codigo);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn descripcion = new DataColumn();
			descripcion.DataType = System.Type.GetType("System.String");
			descripcion.ColumnName = "DESCRIPCION";
			descripcion.ReadOnly=true;
			resultado.Columns.Add(descripcion);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn valor = new DataColumn();
			valor.DataType = System.Type.GetType("System.String");
			valor.ColumnName = "VALOR";
			valor.ReadOnly=true;
			resultado.Columns.Add(valor);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn color = new DataColumn();
			color.DataType = System.Type.GetType("System.String");
			color.ColumnName = "COLOR";
			color.ReadOnly=true;
			resultado.Columns.Add(color);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn cantidad= new DataColumn();
			cantidad.DataType = System.Type.GetType("System.String");
			cantidad.ColumnName = "CANTIDAD";
			cantidad.ReadOnly=true;
			resultado.Columns.Add(cantidad);

			

		}
		public void PrepararTabla2()
		{
			resultado2 = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn codigo2 = new DataColumn();
			codigo2.DataType = System.Type.GetType("System.String");
			codigo2.ColumnName = "CODIGO2";
			codigo2.ReadOnly=true;
			resultado2.Columns.Add(codigo2);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn nombre = new DataColumn();
			nombre.DataType = System.Type.GetType("System.String");
			nombre.ColumnName = "NOMBRE";
			nombre.ReadOnly=true;
			resultado2.Columns.Add(nombre);
			
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn totalventas= new DataColumn();
			totalventas.DataType = System.Type.GetType("System.String");
			totalventas.ColumnName = "TOTALPEDIDOS";
			totalventas.ReadOnly=true;
			resultado2.Columns.Add(totalventas);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn pedidos= new DataColumn();
			pedidos.DataType = System.Type.GetType("System.String");
			pedidos.ColumnName = "PEDIDOS";
			pedidos.ReadOnly=true;
			resultado2.Columns.Add(pedidos);
			//////
			DataColumn asignados= new DataColumn();
			asignados.DataType = System.Type.GetType("System.String");
			asignados.ColumnName = "ASIGNADOS";
			asignados.ReadOnly=true;
			resultado2.Columns.Add(asignados);
			//////
			DataColumn facturados= new DataColumn();
			facturados.DataType = System.Type.GetType("System.String");
			facturados.ColumnName = "FACTURADOS";
			facturados.ReadOnly=true;
			resultado2.Columns.Add(facturados);
			//////
			DataColumn meta= new DataColumn();
			meta.DataType = System.Type.GetType("System.String");
			meta.ColumnName = "META";
			meta.ReadOnly=true;
			resultado2.Columns.Add(meta);
			
			

		}		
		
		
	}
	
	
	
	
	
}