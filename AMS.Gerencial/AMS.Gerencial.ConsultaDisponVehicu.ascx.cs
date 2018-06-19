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
	///		
	/// </summary>
	public partial class AMS_Gerencial_ConsultaDisponVehicu : System.Web.UI.UserControl
	{
		protected DataSet lineas;
		protected DataTable resultado;
		protected DataSet lineas2;
		protected DataTable resultado2;
		int totalN=0;
		int totalU=0;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			
		}
		protected  void  Generar_Click(Object  Sender, EventArgs e)
		{
			
			Panel2.Visible=true;
			///////////////////////////////////////////////////////
			this.PrepararTabla();
			this.PrepararTabla2();
			lineas = new DataSet();
			lineas2 = new DataSet();
            DataSet valNuevos = new DataSet();
            int cont = 0;
            double costoN = 0;
            double costoU = 0;

            // Nuevos
            DBFunctions.Request(lineas, IncludeSchema.NO,
                "select PM.PMAR_NOMBRE as Marca, pgc.pgru_GRUPO CONCAT ' - ' CONCAT pgc.pgru_nombre AS Familia, MCV.PCAT_CODIGO, PCAT_DESCRIPCION, MV.MCAT_VIN,  PCOL_DESCRIPCION, mv.Mveh_NUMED_O, mcv. MCAT_ANOMODE, TEST_NOMBESTA,  " +
                "days(current date) - days(MV.MVEH_FECHRECE) AS DIAS, COALESCE(PUBI_NOMBRE,'* SIN UBICAR *') AS UBICACION,    " +
                "ROUND(coalesce(pp.ppre_PRECIO,0) + DOUBLE(coalesce(pp.ppre_PRECIO,0) * DOUBLE(pca.piva_porciva*0.01)),0) AS PRECIOCONIVA , max(mu.mubi_codigo) as codUbicacion,  " +
                "CASE WHEN TC.TCOM_CODIGO = 'P' THEN 'Propio' else TC.TCOM_TIPOCOMPRA end as origen  " +
                "from DBXSCHEMA.MVEHICULO MV, dbxschema.pcatalogovehiculo pca, dbxschema.pgrupocatalogo pgc, DBXSCHEMA.PMARCA PM, DBXSCHEMA.PCOLOR PC, DBXSCHEMA.TESTADOVEHICULO te,DBXSCHEMA.TCOMPRAVEHICULO TC, DBXSCHEMA.MCATALOGOVEHICULO MCV " +
                "LEFT JOIN DBXSCHEMA.MUBICACIONVEHICULO mU ON  MCV.MCAT_VIN = mU.MCAT_VIN  " +
                "LEFT JOIN DBXSCHEMA.PUBICACION pU ON mu.pubi_CODIGO = pu.pubi_codigo  " +
                "LEFT JOIN dbxschema.ppreciovehiculo pp ON  MCV.pcat_codigo = pp.pcat_codigo  " +
                "where MCV.MCAT_VIN = MV.MCAT_VIN AND MV.TEST_TIPOESTA<=20 and MV.TCLA_CODIGO='N'  AND pca.pcat_codigo=McV.pcat_codigo  " +
                "AND PCA.PMAR_CODIGO = PM.PMAR_CODIGO AND MCV.PCOL_CODIGO = PC.PCOL_CODIGO and mv.TEST_TIPOESTA = te.TEST_TIPOESTA AND MV.TCOM_CODIGO = TC.TCOM_CODIGO  " +
                "and pca.pgru_grupo = pgc.pgru_grupo   " +
                "group BY MCV.PCAT_CODIGO,MV.MCAT_VIN, PM.PMAR_NOMBRE, PCAT_DESCRIPCION, PCOL_DESCRIPCION, mv.Mveh_NUMED_O, mcv. MCAT_ANOMODE, TEST_NOMBESTA,    " +
                "MV.MVEH_FECHRECE, PUBI_NOMBRE, PCA.PMAR_CODIGO, pp.ppre_costo, pp.ppre_PRECIO,pca.piva_porciva, TC.TCOM_CODIGO, TC.TCOM_TIPOCOMPRA,pgc.pgru_GRUPO, pgc.pgru_nombre   " +
                "ORDER BY pCA.pMAR_codigo, pgc.pgru_nombre, MCV.PCAT_CODIGO, MV.MVEH_FECHRECE;");

            string marcAnterior = "";
            string familiAnterior = "";
            for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
                string MARCA  = lineas.Tables[0].Rows[i].ItemArray[0].ToString();
                if (marcAnterior != MARCA)
                {
                    DataRow filaM;
                    filaM = resultado.NewRow();
                    filaM["CODIGO"] = "<FONT COLOR='blue'>"+MARCA.ToString()+"</Font>";
                    marcAnterior = MARCA;
                    resultado.Rows.Add(filaM);
                    familiAnterior = "";
                }
                string FAMILIA  = lineas.Tables[0].Rows[i].ItemArray[1].ToString();
                if (familiAnterior != FAMILIA)
                {
                    DataRow filaM;
                    filaM = resultado.NewRow();
                    filaM["CODIGO"] = "<FONT COLOR='blue'>"+MARCA.ToString()+"</Font>";
                    filaM["DESCRIPCION"] = "<FONT COLOR='green'>"+FAMILIA.ToString()+"</Font>";
                    familiAnterior = FAMILIA;
                    resultado.Rows.Add(filaM);
                }
                string descripcion = lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				string color  = lineas.Tables[0].Rows[i].ItemArray[5].ToString();
			    string motor  = lineas.Tables[0].Rows[i].ItemArray[6].ToString();
				int    modelo = Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[7].ToString());
			    string estado = lineas.Tables[0].Rows[i].ItemArray[8].ToString();
                int    dias   = Convert.ToInt32(lineas.Tables[0].Rows[i].ItemArray[9].ToString());
                string Ubicacion = lineas.Tables[0].Rows[i].ItemArray[10].ToString();
                string origen = lineas.Tables[0].Rows[i].ItemArray[13].ToString();
               	DataRow fila;
				string valor  = "";
				fila = resultado.NewRow();
                double valN = System.Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[11].ToString());
                    costoN += valN;
                    cont++;
                
                if(Request.QueryString["vvalor"]==null)
                    valor = " Precio: " + String.Format("{0:C}", valN);

                fila["MARCA"]   = MARCA.ToString();
				fila["CODIGO"]	= lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				fila["DESCRIPCION"]	= descripcion.ToString() + valor;
                fila["VIN"] = lineas.Tables[0].Rows[i].ItemArray[4].ToString() + " " + lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["MODELO"]	= modelo.ToString();
				fila["MOTOR"]	= motor.ToString();
				fila["COLOR"]	= color.ToString();
				fila["UBICACION"] = Ubicacion.ToString();
				fila["ESTADO"]	= estado.ToString();
				fila["DIAS"]    = dias.ToString();
                fila["ORIGEN"]  = origen.ToString();
				totalN = totalN+1;
			
				resultado.Rows.Add(fila);
			}
            //Usados
            DBFunctions.Request(lineas2, IncludeSchema.NO,
                  "select MCV.PCAT_CODIGO, MCV.MCAT_PLACA, PM.PMAR_NOMBRE, PCAT_DESCRIPCION, PCOL_DESCRIPCION, mv.Mveh_NUMED_O, mcv. MCAT_ANOMODE, TEST_NOMBESTA, " +
                "days(current date) - days(MV.MVEH_FECHRECE) AS DIAS, COALESCE(PUBI_NOMBRE,'* SIN UBICAR *') AS UBICACION, " +
                "ROUND(coalesce(pp.ppre_costo,0) + DOUBLE(coalesce(pp.ppre_costo,0) * DOUBLE(pca.piva_porciva*0.01)),0) AS PRECIOCONIVA , max(mu.mubi_codigo) as codUbicacion, " +
                "CASE WHEN TC.TCOM_CODIGO = 'P' THEN 'Propio' else TC.TCOM_TIPOCOMPRA end as origen2, mveh_valoinfl AS PRECIOVENTA, mcv.mcat_numeultikilo as kilometraje " +
                "from DBXSCHEMA.MVEHICULO MV, dbxschema.pcatalogovehiculo pca, DBXSCHEMA.PMARCA PM, DBXSCHEMA.PCOLOR PC, DBXSCHEMA.TESTADOVEHICULO te,DBXSCHEMA.TCOMPRAVEHICULO TC, DBXSCHEMA.MCATALOGOVEHICULO MCV " +
                " LEFT JOIN DBXSCHEMA.MUBICACIONVEHICULO mU ON  MCV.MCAT_VIN = mU.MCAT_VIN " +
                " LEFT JOIN DBXSCHEMA.PUBICACION pU ON mu.pubi_CODIGO = pu.pubi_codigo " +
                " LEFT JOIN dbxschema.ppreciovehiculo pp ON  MCV.pcat_codigo = pp.pcat_codigo " +
                "where MCV.MCAT_VIN = MV.MCAT_VIN AND MV.TEST_TIPOESTA<=20 and MV.TCLA_CODIGO='U'  AND pca.pcat_codigo=McV.pcat_codigo " +
                " AND PCA.PMAR_CODIGO = PM.PMAR_CODIGO AND MCV.PCOL_CODIGO = PC.PCOL_CODIGO and mv.TEST_TIPOESTA = te.TEST_TIPOESTA AND MV.TCOM_CODIGO = TC.TCOM_CODIGO " +
                "group BY MCV.PCAT_CODIGO,MV.MCAT_VIN, PM.PMAR_NOMBRE, PCAT_DESCRIPCION, PCOL_DESCRIPCION, mv.Mveh_NUMED_O, mcv. MCAT_ANOMODE, TEST_NOMBESTA, mcv.mcat_numeultikilo, " +
                        " MV.MVEH_FECHRECE, PUBI_NOMBRE, PCA.PMAR_CODIGO, pp.ppre_costo, pca.piva_porciva, TC.TCOM_CODIGO, TC.TCOM_TIPOCOMPRA, mveh_valoinfl, MCV.MCAT_PLACA " +
                "ORDER BY pCA.pMAR_codigo, MCV.PCAT_CODIGO, MV.MVEH_FECHRECE; ");
            for (int i = 0; i < lineas2.Tables[0].Rows.Count; i++)
			{
                string descripcion2 = lineas2.Tables[0].Rows[i].ItemArray[2].ToString()  + " " + lineas2.Tables[0].Rows[i].ItemArray[3].ToString();
                string color2 = lineas2.Tables[0].Rows[i].ItemArray[4].ToString();
                string motor2 = lineas2.Tables[0].Rows[i].ItemArray[5].ToString();
                int modelo2 = Convert.ToInt32(lineas2.Tables[0].Rows[i].ItemArray[6].ToString());
                string Ubicacion2 = lineas2.Tables[0].Rows[i].ItemArray[9].ToString();
                int dias2 = Convert.ToInt32(lineas2.Tables[0].Rows[i].ItemArray[8].ToString());
                string estado2 = lineas2.Tables[0].Rows[i].ItemArray[7].ToString();
                string origen2 = lineas2.Tables[0].Rows[i].ItemArray[12].ToString();
                double valoinfln = System.Convert.ToDouble(lineas2.Tables[0].Rows[i].ItemArray[13].ToString());
                double kilometraje = System.Convert.ToDouble(lineas2.Tables[0].Rows[i].ItemArray[14].ToString());
			  
                //Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila2;
				fila2 = resultado2.NewRow();
                costoU += valoinfln;
                string valoinfl = "";

                if(Request.QueryString["vvalor"]==null)
                    valoinfl = " Precio: " + String.Format("{0:C}", valoinfln);

				fila2["CODIGO2"]= lineas2.Tables[0].Rows[i].ItemArray[0].ToString();
				fila2["DESCRIPCION2"] = descripcion2.ToString() + valoinfl;
				fila2["VIN2"]	= lineas2.Tables[0].Rows[i].ItemArray[1].ToString();
				fila2["MODELO2"]= modelo2.ToString(); 
				fila2["MOTOR2"]	= motor2.ToString();
				fila2["COLOR2"]	= color2.ToString()+ " "+kilometraje.ToString("N")+ " Kms";
				fila2["UBICACION2"]	= Ubicacion2.ToString();
				fila2["ESTADO2"]= estado2.ToString();
				fila2["DIAS2"]  = dias2.ToString();
                fila2["ORIGEN2"] = origen2.ToString();
				totalU= totalU+1;
			
                resultado2.Rows.Add(fila2);
			}
		
			//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();
			Grid2.DataSource = resultado2;
			Grid2.DataBind();
            totalNu.Text = totalN.ToString();
            totalUsa.Text = totalU.ToString();
            /*
            if (Request.QueryString["vvalor"] == null)
            {
                totalCostoN.Text = String.Format("{0:C}", costoN.ToString());
                totalCostoU.Text = String.Format("{0:C}", costoU.ToString());
                totalVeh.Text = "" + (totalN + totalU);
                totalCost.Text = "" + String.Format("{0:C}", (costoN + costoU));

                Label4.Visible = true;
                totalCostoN.Visible = true;
                Label16.Visible = true;
                totalCostoU.Visible = true;
                Label19.Visible = true;
                totalCost.Visible = true;
            }
            else
            {
                Label4.Visible = false;
                totalCostoN.Visible = false;
                Label16.Visible = false;
                totalCostoU.Visible = false;
                Label19.Visible = false;
                totalCost.Visible = false;
            }
          */  
		}
	
		public void PrepararTabla()
		{
			resultado = new DataTable();
			
			//Adicionamos una columna que almacene el numero de la linea
            DataColumn MARCA = new DataColumn();
            MARCA.DataType = System.Type.GetType("System.String");
            MARCA.ColumnName = "MARCA";
            MARCA.ReadOnly = true;
            resultado.Columns.Add(MARCA);
            
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
			DataColumn vin = new DataColumn();
			vin.DataType = System.Type.GetType("System.String");
			vin.ColumnName = "VIN";
			vin.ReadOnly=true;
			resultado.Columns.Add(vin);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn modelo = new DataColumn();
			modelo.DataType = System.Type.GetType("System.String");
			modelo.ColumnName = "MODELO";
			modelo.ReadOnly=true;
			resultado.Columns.Add(modelo);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn motor= new DataColumn();
			motor.DataType = System.Type.GetType("System.String");
			motor.ColumnName = "MOTOR";
			motor.ReadOnly=true;
			resultado.Columns.Add(motor);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn color = new DataColumn();
			color.DataType = System.Type.GetType("System.String");
			color.ColumnName = "COLOR";
			color.ReadOnly=true;
			resultado.Columns.Add(color);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn ubicacion = new DataColumn();
			ubicacion.DataType = System.Type.GetType("System.String");
			ubicacion.ColumnName = "UBICACION";
			ubicacion.ReadOnly=true;
			resultado.Columns.Add(ubicacion);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn estado = new DataColumn();
			estado.DataType = System.Type.GetType("System.String");
			estado.ColumnName = "ESTADO";
			estado.ReadOnly=true;
			resultado.Columns.Add(estado);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn dias = new DataColumn();
			dias.DataType = System.Type.GetType("System.String");
			dias.ColumnName = "DIAS";
			dias.ReadOnly=true;
			resultado.Columns.Add(dias);

            //Adicionamos una columna que almacene el total del ORIGEN
            DataColumn origen = new DataColumn();
            origen.DataType = System.Type.GetType("System.String");
            origen.ColumnName = "ORIGEN";
            origen.ReadOnly = true;
            resultado.Columns.Add(origen);

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
			DataColumn descripcion2 = new DataColumn();
			descripcion2.DataType = System.Type.GetType("System.String");
			descripcion2.ColumnName = "DESCRIPCION2";
			descripcion2.ReadOnly=true;
			resultado2.Columns.Add(descripcion2);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn vin2 = new DataColumn();
			vin2.DataType = System.Type.GetType("System.String");
			vin2.ColumnName = "VIN2";
			vin2.ReadOnly=true;
			resultado2.Columns.Add(vin2);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn modelo2 = new DataColumn();
			modelo2.DataType = System.Type.GetType("System.String");
			modelo2.ColumnName = "MODELO2";
			modelo2.ReadOnly=true;
			resultado2.Columns.Add(modelo2);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn motor2= new DataColumn();
			motor2.DataType = System.Type.GetType("System.String");
			motor2.ColumnName = "MOTOR2";
			motor2.ReadOnly=true;
			resultado2.Columns.Add(motor2);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn color2 = new DataColumn();
			color2.DataType = System.Type.GetType("System.String");
			color2.ColumnName = "COLOR2";
			color2.ReadOnly=true;
			resultado2.Columns.Add(color2);
			
			//Adicionamos una columna que almacene el total de la linea
			DataColumn ubicacion2 = new DataColumn();
			ubicacion2.DataType = System.Type.GetType("System.String");
			ubicacion2.ColumnName = "UBICACION2";
			ubicacion2.ReadOnly=true;
			resultado2.Columns.Add(ubicacion2);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn estado2 = new DataColumn();
			estado2.DataType = System.Type.GetType("System.String");
			estado2.ColumnName = "ESTADO2";
			estado2.ReadOnly=true;
			resultado2.Columns.Add(estado2);

			//Adicionamos una columna que almacene el total de la linea
			DataColumn dias2 = new DataColumn();
			dias2.DataType = System.Type.GetType("System.String");
			dias2.ColumnName = "DIAS2";
			dias2.ReadOnly=true;
			resultado2.Columns.Add(dias2);

            //Adicionamos una columna que almacene el ORIGEN del usado
            DataColumn origen2 = new DataColumn();
            origen2.DataType = System.Type.GetType("System.String");
            origen2.ColumnName = "ORIGEN2";
            origen2.ReadOnly = true;
            resultado2.Columns.Add(origen2);

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


