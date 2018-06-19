using System;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;


namespace AMS.Inventarios
{
	/// <summary>
	///		Descripción breve de AMS_Inventarios_ClasificacionABC.
	/// </summary>
	public partial class AMS_Inventarios_ClasificacionABC : System.Web.UI.UserControl
	{
        ArrayList sqlStrings = new ArrayList();
        private DatasToControls bind = new DatasToControls();
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
            // Introducir aquí el código de usuario para inicializar la página
            if(!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(ddlLinea, "select pl.plin_codigo, pl.plin_codigo || ' ' ||plin_nombre || ' tiene ' || count(*) || ' itmes ' from mitems m, plineaitem pl where m.plin_codigo = pl.plin_codigo group by pl.plin_codigo, plin_nombre order by plin_nombre;");//linea
                if (ddlLinea.Items.Count > 0)
                    ddlLinea.Items.Insert(0, "Seleccione..");

            }
        }

		protected void btnClasificar_Click(object sender, System.EventArgs e)
		{
            if(tbCantidadMeses.Text == "" || tbCantidadMeses.Text.Length == 0)
            {
                Utils.MostrarAlerta(Response, "Falta indicar el número de meses, por favor revise.");
                return;
            }
			//lbText.Text = DBFunctions.SingleData("SELECT SUM(macu_cantidad) FROM VINVENTARIO_MACUMULADOITEM WHERE fecha < '"+DateTime.Now.ToString("yyyy-MM-dd")+"'");
			// Se Construyen las dos fechas que representan el intervalo
			DateTime fechaActual  = DateTime.Now.AddDays(0);
			DateTime fechaEstudio = DateTime.Now.AddMonths(Convert.ToInt32(tbCantidadMeses.Text)*-1);
            DateTime demandaM6 = DateTime.Now.AddMonths(- 1);
            DateTime demandaM6D = demandaM6.AddDays(1);
            DateTime demandaM5 = DateTime.Now.AddMonths(- 2);
            DateTime demandaM5D = demandaM5.AddDays(1);
            DateTime demandaM4 = DateTime.Now.AddMonths(- 3);
            DateTime demandaM4D = demandaM4.AddDays(1);
            DateTime demandaM3 = DateTime.Now.AddMonths(- 4);
            DateTime demandaM3D = demandaM3.AddDays(1);
            DateTime demandaM2 = DateTime.Now.AddMonths(- 5);
            DateTime demandaM2D = demandaM2.AddDays(1);
            DateTime demandaM1 = DateTime.Now.AddMonths(- 6);
            DateTime demandaM1D = demandaM1.AddDays(1);
            int ano1 = fechaActual.Year;
            int mes1 = fechaActual.Month;

            fechaEstudio = fechaEstudio.AddDays((fechaEstudio.Day-1)*-1);
			//Generamos la sumatoria a la fecha de acuerdo a la variable seleccionada teniendo en cuenta las entradas y devoluciones
			// salidas(80-90) - entradas(81-91)
			
			string consultaDetalleItems = "";
            string consultaDetalleInsumos = "";
            string linea = ddlLinea.SelectedValue;

            if (ddlVariable.SelectedValue == "A")  // Opción Valores de venta y Unidades vendidas
            {
                // SE DESCARTAN LOS INSUMOS porque por sus cantidades tan grandes de gramos afectan el analasis del inventario
                consultaDetalleItems =
                @"SELECT ITEM, SUM(DM1)+SUM(DM2)+SUM(DM3)+SUM(DM4)+SUM(DM5)+SUM(DM6) AS TOTAL, SUM(DM1) AS DM1, SUM(DM2) AS DM2, SUM(DM3) DM3, SUM(DM4) DM4, SUM(DM5) DM5, SUM(DM6) DM6 FROM (
                select d1.mite_codigo AS ITEM, 0 as TOTAL, sum((d1.dite_cantidad - coalesce(d2.dite_cantidad, 0)) * d1.dite_valounit) as dm1, 0 as dm2, 0 as dm3, 0 as dm4, 0 as dm5, 0 as dm6
                  from Mitems M, ditems d1
                   left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)
                  where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN '" + demandaM6D.ToString("yyyy-MM-dd") + @"' AND '" + fechaActual.ToString("yyyy-MM-dd") + @"' and d1.tmov_tipomovi in (80,90) 
                  group by d1.mite_codigo
                 UNION
                select d1.mite_codigo, 0 as TOTAL, 0, sum((d1.dite_cantidad - coalesce(d2.dite_cantidad, 0)) * d1.dite_valounit) as dm2, 0 as dm3, 0 as dm4, 0 as dm5, 0 as dm6
                  from Mitems M, ditems d1
                   left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)
                  where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN '" + demandaM5D.ToString("yyyy-MM-dd") + @"' AND '" + demandaM6.ToString("yyyy-MM-dd") + @"' and d1.tmov_tipomovi in (80,90) 
                  group by d1.mite_codigo
                 UNION
                select d1.mite_codigo, 0 as TOTAL, 0, 0, sum((d1.dite_cantidad - coalesce(d2.dite_cantidad, 0)) * d1.dite_valounit) as dm3, 0 as dm4, 0 as dm5, 0 as dm6
                  from Mitems M, ditems d1
                   left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)
                 where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN '" + demandaM4D.ToString("yyyy-MM-dd") + @"' AND '" + demandaM5.ToString("yyyy-MM-dd") + @"' and d1.tmov_tipomovi in (80,90) 
                  group by d1.mite_codigo
                 UNION
                select d1.mite_codigo, 0 as TOTAL, 0, 0, 0, sum((d1.dite_cantidad - coalesce(d2.dite_cantidad, 0)) * d1.dite_valounit) as dm4, 0 as dm5, 0 as dm6
                  from Mitems M, ditems d1
                   left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)
                  where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN '" + demandaM3D.ToString("yyyy-MM-dd") + @"' AND '" + demandaM4.ToString("yyyy-MM-dd") + @"' and d1.tmov_tipomovi in (80,90) 
                  group by d1.mite_codigo
                 UNION
                select d1.mite_codigo, 0 as TOTAL, 0, 0, 0, 0, sum((d1.dite_cantidad - coalesce(d2.dite_cantidad, 0)) * d1.dite_valounit) as dm5, 0 as dm6
                  from Mitems M, ditems d1
                   left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)
                  where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN '" + demandaM2D.ToString("yyyy-MM-dd") + @"' AND '" + demandaM3.ToString("yyyy-MM-dd") + @"' and d1.tmov_tipomovi in (80,90) 
                  group by d1.mite_codigo
                 UNION
                select d1.mite_codigo, 0 as TOTAL, 0, 0, 0, 0, 0, sum((d1.dite_cantidad - coalesce(d2.dite_cantidad, 0)) * d1.dite_valounit) as dm6
                  from Mitems M, ditems d1
                   left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)
                  where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN '" + demandaM1D.ToString("yyyy-MM-dd") + @"' AND '" + demandaM2.ToString("yyyy-MM-dd") + @"' and d1.tmov_tipomovi in (80,90) 
                  group by d1.mite_codigo
                ) AS A
                group by item 
                order by 2 desc,1; ";

            }
            else if (ddlVariable.SelectedValue == "C")
            {
                // SE DESCARTAN LOS INSUMOS porque por sus cantidades tan grandes de gramos afectan el analasis del inventario
                consultaDetalleItems = @"select d1.mite_codigo, sum(d1.dite_cantidad) - coalesce(sum(d2.dite_cantidad),0) as TOTAL  
                                            from Mitems M, ditems d1 
                                                left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)   
                                           where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN  '" + fechaEstudio.ToString("yyyy-MM-dd") + "' AND '" + fechaActual.ToString("yyyy-MM-dd") + "' and d1.tmov_tipomovi in (80,90) group by d1.mite_codigo order by 2 desc,1;";

                consultaDetalleInsumos = @"select d1.mite_codigo, sum(d1.dite_cantidad) - coalesce(sum(d2.dite_cantidad),0) as TOTAL  
                                        from Mitems M, ditems d1 
                                            left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)  
                                       where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM = 'I' AND d1.dite_fechdocu BETWEEN  '" + fechaEstudio.ToString("yyyy-MM-dd") + "' AND '" + fechaActual.ToString("yyyy-MM-dd") + "' and d1.tmov_tipomovi in (80,90) group by d1.mite_codigo order by 2 desc,1;";
            }
            else if (ddlVariable.SelectedValue == "D")
            {
                //    consultaDetalleItems = "select d1.mite_codigo, sum(d1.dite_cantidad*d1.dite_valounit*(1-d1.dite_porcdesc*0.01)) - coalesce(sum(d2.dite_cantidad*d2.dite_valounit*(1-d2.dite_porcdesc*0.01)),0) as TOTAL " +
                //                           " from Mitems M, ditems d1 left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91) " +
                //                           "where M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN  '" + fechaEstudio.ToString("yyyy-MM-dd") + "' AND '" + fechaActual.ToString("yyyy-MM-dd") + "' and d1.tmov_tipomovi in (80,90) group by d1.mite_codigo order by 2 desc,1;";
                //    consultaDetalleInsumos = "select d1.mite_codigo, sum(d1.dite_cantidad*d1.dite_valounit*(1-d1.dite_porcdesc*0.01)) - coalesce(sum(d2.dite_cantidad*d2.dite_valounit*(1-d2.dite_porcdesc*0.01)),0) as TOTAL " +
                //                           " from Mitems M, ditems d1 left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91) " +
                //                           "where M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM = 'I' AND d1.dite_fechdocu BETWEEN  '" + fechaEstudio.ToString("yyyy-MM-dd") + "' AND '" + fechaActual.ToString("yyyy-MM-dd") + "' and d1.tmov_tipomovi in (80,90) group by d1.mite_codigo order by 2 desc,1;";
                consultaDetalleItems = @"select d1.mite_codigo, sum(d1.dite_cantidad*d1.dite_COSTPROM) - coalesce(sum(d2.dite_cantidad*d2.dite_COSTPROM),0) as TOTAL  
                                        from Mitems M, ditems d1 
                                             left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)  
                                       where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM <> 'I' AND d1.dite_fechdocu BETWEEN  '" + fechaEstudio.ToString("yyyy-MM-dd") + "' AND '" + fechaActual.ToString("yyyy-MM-dd") + "' and d1.tmov_tipomovi in (80,90) group by d1.mite_codigo order by 2 desc,1;";
                consultaDetalleInsumos = @"select d1.mite_codigo, sum(d1.dite_cantidad*d1.dite_COSTPROM) - coalesce(sum(d2.dite_cantidad*d2.dite_COSTPROM),0) as TOTAL  
                                        from Mitems M, ditems d1 
                                            left join ditems d2 on d1.mite_codigo = d2.mite_codigo and d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu = d2.dite_numedocurefe and d2.tmov_tipomovi in (81,91)  
                                       where M.PLIN_CODIGO = '"+ linea+"' and M.MITE_CODIGO = D1.MITE_CODIGO AND M.TITE_CODIITEM = 'I' AND d1.dite_fechdocu BETWEEN  '" + fechaEstudio.ToString("yyyy-MM-dd") + "' AND '" + fechaActual.ToString("yyyy-MM-dd") + "' and d1.tmov_tipomovi in (80,90) group by d1.mite_codigo order by 2 desc,1;";

            }

            //AHORA MOSTRAMOS LA CLASIFICACION
            //Ahora se realiza la clasificacion de los D que son los que no tienen movimiento y el tiempo de creacion es mayor a 4 meses
            if (ddlVariable.SelectedValue == "A")  // Opción Valores de venta y Unidades vendidas
                sqlStrings.Add("UPDATE mitems SET mite_clasabc = 'E' WHERE PLIN_CODIGO = '" + linea + "' and (CURRENT DATE - mite_creacion) > 120 ");
            else
                sqlStrings.Add("UPDATE mitems SET mite_clasabc = 'D' WHERE PLIN_CODIGO = '" + linea + "'  and (CURRENT DATE - mite_creacion) > 120 ");

            //Ahora se clasifican como N los que no tienen movimiento y su tiempo de creacion es menor o igual a 4 meses
            sqlStrings.Add("UPDATE mitems SET mite_clasabc = 'N' WHERE PLIN_CODIGO = '" + linea + "'  and (CURRENT DATE - mite_creacion) <= 120 ");

            //Se generan los valores de clasificacion con 85(A) 95(B) 98(C) 100(E) categorizacion A o 80(A), 95(B) y 100(C) para las otras selecciones
            //Ahora traemos todos los items con que se encuentran en macumuladoitem y empezamos a generar el acumulado de pivote
            DataSet dsItems = new DataSet();
			DBFunctions.Request(dsItems,IncludeSchema.NO,consultaDetalleItems);
            marcaritems(dsItems);

            if (ddlVariable.SelectedValue != "A")  // Opción Valores de venta y Unidades vendidas
            {
                DataSet dsInsumos = new DataSet();
                DBFunctions.Request(dsInsumos, IncludeSchema.NO, consultaDetalleInsumos);
                marcaritems(dsInsumos);
            }

            // Reclasificamos a N los items comprados dentro de los ultimos cuatro meses de clasificacion D 
            sqlStrings.Add("UPDATE mitems SET mite_clasabc = 'N' WHERE PLIN_CODIGO = '" + linea + "' and MITE_CODIGO IN (SELECT MI.MITE_CODIGO FROM MITEMS MI, MSALDOITEM MS WHERE MI.MITE_CODIGO = MS.MITE_CODIGO AND MI.MITE_CLASABC = 'D' AND MS.MSAL_ULTIINGR > CURRENT DATE - 4 MONTHS )");
            
			if(DBFunctions.Transaction(sqlStrings))
			{
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as codigo,MIT.mite_clasabc as clasificacion FROM mitems MIT INNER JOIN plineaitem PLIN ON MIT.plin_codigo = PLIN.plin_codigo WHERE MIT.mite_clasabc is not null ORDER BY MIT.mite_clasabc ASC");
				dgClasificacionABC.DataSource = ds.Tables[0];
				dgClasificacionABC.DataBind();
                Session["dtItems"] = ds.Tables[0];
			}
			else
				lbText.Text = DBFunctions.exceptions;
		}

        //Paginación de la grilla 
        protected void DgUpdate_Page(Object sender, DataGridPageChangedEventArgs e)
        {
            this.dgClasificacionABC.CurrentPageIndex = e.NewPageIndex;
            this.dgClasificacionABC.DataSource = Session["dtItems"];
            this.dgClasificacionABC.DataBind();
        }

        protected void marcaritems(DataSet dsItems)
        {
            double variableSumatoria = 0;
            double valorPivote = 0;
            int j = 0;

            DataTable table;
            table = dsItems.Tables[0];
            object sumObject;
            sumObject = table.Compute("sum(TOTAL)", "TOTAL <> 0");
            try
            {
                variableSumatoria = Convert.ToDouble(sumObject.ToString());
            }
            catch { };

            double[] valoresFrontera = new double[4];
            if (ddlVariable.SelectedValue == "A")  // Opción Valores de venta y Unidades vendidas
            {
                valoresFrontera[0] = variableSumatoria * (0.85);
                valoresFrontera[1] = variableSumatoria * (0.95);
                valoresFrontera[2] = variableSumatoria * (0.98);
                valoresFrontera[3] = variableSumatoria;
            }
            else
            { 
                valoresFrontera[0] = variableSumatoria * (0.8);
                valoresFrontera[1] = variableSumatoria * (0.95);
                valoresFrontera[2] = variableSumatoria;
                valoresFrontera[3] = 0;
            }

			string[] clasificacion = new string[4];
			clasificacion[0] = "A";
			clasificacion[1] = "B";
			clasificacion[2] = "C";
			clasificacion[3] = "D";
            string  valorAbc = "";
            int demandas = 0;
            string sDemanda = "";

            Hashtable clasificacionABC = new Hashtable();

            for (int i = 0; i < dsItems.Tables[0].Rows.Count; i++)
            {
                valorPivote += Convert.ToDouble(dsItems.Tables[0].Rows[i][1]);
                valorAbc = Convert.ToString(Convert.ToDouble(dsItems.Tables[0].Rows[i][1].ToString()).ToString("N"));
                demandas = 0;

                if (ddlVariable.SelectedValue == "A")
                {
                    for (int h = 0; h < 6; h++)
                    {
                        if (Convert.ToInt32(dsItems.Tables[0].Rows[i][h + 2]) > 0)
                            demandas += 1;
                    }
                    if (demandas == 6)
                        demandas = 1;
                    else
                        if (demandas == 5)
                            demandas = 2;
                    else
                        if (demandas == 4 || demandas == 3)
                            demandas = 3;
                    else
                        demandas = 4;
                    sDemanda = demandas.ToString();
                }

                if (j <= 2)
                {
                    if (valorPivote >= valoresFrontera[j])
                        j = j + 1;
                    if (j <= 2)
                        clasificacionABC.Add(dsItems.Tables[0].Rows[i][0].ToString(), clasificacion[j] + sDemanda + ' ' + valorAbc);
                    else
                        clasificacionABC.Add(dsItems.Tables[0].Rows[i][0].ToString(), clasificacion[j] + sDemanda + ' ' + valorAbc);
                }
                else
                {
                    if (valorPivote >= valoresFrontera[j] && j < 3)
                        j = j + 1;
                    clasificacionABC.Add(dsItems.Tables[0].Rows[i][0].ToString(), clasificacion[j] + sDemanda + ' ' + valorAbc);
                }
            }
			IDictionaryEnumerator cls = clasificacionABC.GetEnumerator();
			while(cls.MoveNext())
				sqlStrings.Add("UPDATE mitems SET mite_clasabc = '"+cls.Value+"' WHERE mite_codigo='"+cls.Key.ToString()+"'");
    }

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
