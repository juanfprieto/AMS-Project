// created on 10/12/2004 at 10:19

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
using AMS.Forms;

namespace AMS.Automotriz
{

	public partial class Categorizacion : System.Web.UI.UserControl
	{
		protected DateTime []intervalos;
		protected DataTable tablaAsociada, tablaResultado;
		protected ArrayList estadosCategoria, nivelCategoria;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lb.Text = "";
			intervalos = new DateTime[5];
			if(Session["tablaAsociada"]!=null)
				tablaAsociada = (DataTable)Session["tablaAsociada"];
			this.Construir_Niveles_Categoria();
		}
		
		protected void Categorizar_Clientes(Object  Sender, EventArgs e)
		{
			this.Llenar_Grilla_Lista(opcionesTiempo.SelectedValue.ToString());
			this.Llenar_Grilla_Resultado();
			labelInformativo.Text = "<br>Cantidad de Clientes : "+DBFunctions.SingleData("SELECT COUNT(DISTINCT mnit_nit) FROM mcatalogovehiculo");
			labelInformativo.Text +="<br>Cantidad de Vehiculos : "+DBFunctions.SingleData("SELECT COUNT(*) FROM mcatalogovehiculo");
			labelInformativo.Text +="<br>Cantidad de Ordenes :"+DBFunctions.SingleData("SELECT COUNT(*) FROM morden WHERE mord_entrada>'"+intervalos[0].Date.ToString("yyyy-MM-dd")+"'");
			labelInformativo.Text +="<br>Procesado el "+DateTime.Now.ToString();
		}
		
		protected void Mostrar_Lista(Object  Sender, EventArgs e)
		{
			if(grillaCategorizacion.Visible==false)
				grillaCategorizacion.Visible = true;
			else
				grillaCategorizacion.Visible = false;			
		}
		
		protected void Preparar_Tabla_Asociada(string opcion)
		{
			tablaAsociada = new DataTable();
			tablaAsociada.Columns.Add(new DataColumn("NIT PROPIETARIO",System.Type.GetType("System.String")));
			tablaAsociada.Columns.Add(new DataColumn("PLACA DEL AUTO",System.Type.GetType("System.String")));
			DateTime fechaActual = DateTime.Now;
			for(int i=0;i<5;i++)
			{
				DateTime Inicio = new DateTime();
				DateTime Final = new DateTime();
				if(opcion=="semestres")
				{
					Inicio = fechaActual.AddMonths(((5-i)*6)*-1);
					Final = Inicio.AddMonths(6);
				}
				else if(opcion=="anos")
				{
					Inicio = fechaActual.AddYears((5-i)*-1);
					Final = Inicio.AddYears(1);
				}
				intervalos[i] = Inicio;
				//lb.Text += "<br>inicio "+Inicio.Date.ToString("yyyy-MM-dd")+" a "+Final.Date.ToString("yyyy-MM-dd");
				tablaAsociada.Columns.Add(new DataColumn(Inicio.Date.ToString("yyyy-MM-dd")+" A "+Final.Date.ToString("yyyy-MM-dd"),System.Type.GetType("System.String")));
			}
			tablaAsociada.Columns.Add(new DataColumn("CATEGORIA CLIENTE",System.Type.GetType("System.String")));
		}
		
		protected void Llenar_Grilla_Lista(string opcion)
		{
			this.Preparar_Tabla_Asociada(opcion);
			DataSet vehiculosRegistrados = new DataSet();
            //DBFunctions.Request(vehiculosRegistrados,IncludeSchema.NO,"SELECT mcat_placa, mnit_nit FROM mcatalogovehiculo ORDER BY mnit_nit");
            //for(int i=0;i<vehiculosRegistrados.Tables[0].Rows.Count;i++)
            //{
            //    ArrayList resultados = new ArrayList();
            //    DataRow fila = tablaAsociada.NewRow();
            //    string placa = vehiculosRegistrados.Tables[0].Rows[i][0].ToString();
            //    fila["NIT PROPIETARIO"] = vehiculosRegistrados.Tables[0].Rows[i][1].ToString();
            //    fila["PLACA DEL AUTO"] = placa;
            //    for(int j=0;j<5;j++)
            //    {
            //        if(opcion=="semestres")
            //        {
            //            if(DBFunctions.RecordExist("SELECT * FROM morden WHERE mord_entrada>='"+intervalos[j].Date.ToString("yyyy-MM-dd")+"' AND mord_entrada<'"+(intervalos[j].AddMonths(6)).Date.ToString("yyyy-MM-dd")+"' AND mcat_vin='"+DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'")+"'"))
            //                fila[intervalos[j].Date.ToString("yyyy-MM-dd")+" A "+(intervalos[j].AddMonths(6)).Date.ToString("yyyy-MM-dd")] = "SI";
            //            else
            //                fila[intervalos[j].Date.ToString("yyyy-MM-dd")+" A "+(intervalos[j].AddMonths(6)).Date.ToString("yyyy-MM-dd")] = "NO";
            //            resultados.Add(fila[intervalos[j].Date.ToString("yyyy-MM-dd")+" A "+(intervalos[j].AddMonths(6)).Date.ToString("yyyy-MM-dd")].ToString());
            //        }
            //        else if(opcion=="anos")
            //        {
            //            if(DBFunctions.RecordExist("SELECT * FROM morden WHERE mord_entrada>='"+intervalos[j].Date.ToString("yyyy-MM-dd")+"' AND mord_entrada<'"+(intervalos[j].AddYears(1)).Date.ToString("yyyy-MM-dd")+"' AND mcat_vin='"+DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'")+"'"))
            //                fila[intervalos[j].Date.ToString("yyyy-MM-dd")+" A "+(intervalos[j].AddYears(1)).Date.ToString("yyyy-MM-dd")] = "SI";
            //            else
            //                fila[intervalos[j].Date.ToString("yyyy-MM-dd")+" A "+(intervalos[j].AddYears(1)).Date.ToString("yyyy-MM-dd")] = "NO";
            //            resultados.Add(fila[intervalos[j].Date.ToString("yyyy-MM-dd")+" A "+(intervalos[j].AddYears(1)).Date.ToString("yyyy-MM-dd")].ToString());
            //        }					
            //    }			
            //    fila["CATEGORIA CLIENTE"] = this.Encontrar_Categoria(resultados);
            //    tablaAsociada.Rows.Add(fila);
            //}

            if (opcion == "semestres")
            {
                DBFunctions.Request(vehiculosRegistrados, IncludeSchema.NO,
                                    "select " +
                                    "mcv.mnit_nit, " +
                                    "CASE  WHEN tf1.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F1, " +
                                    "CASE  WHEN tf2.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F2, " +
                                    "CASE  WHEN tf3.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F3, " +
                                    "CASE  WHEN tf4.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F4, " +
                                    "CASE  WHEN tf5.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F5 " +
                                    "FROM " +
                                    "(SELECT DISTINCT mnit_nit FROM DBXSCHEMA.mcatalogovehiculo ORDER BY mnit_nit) mcv " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[0].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[0].AddMonths(6)).Date.ToString("yyyy-MM-dd") + "') tf1 on mcv.mnit_nit= tf1.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[1].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[1].AddMonths(6)).Date.ToString("yyyy-MM-dd") + "') tf2 on mcv.mnit_nit= tf2.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[2].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[2].AddMonths(6)).Date.ToString("yyyy-MM-dd") + "') tf3 on mcv.mnit_nit= tf3.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[3].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[3].AddMonths(6)).Date.ToString("yyyy-MM-dd") + "') tf4 on mcv.mnit_nit= tf4.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[4].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[4].AddMonths(6)).Date.ToString("yyyy-MM-dd") + "') tf5 on mcv.mnit_nit= tf5.mnit_nit " +
                                    "ORDER BY mcv.mnit_nit");
            }
            else if (opcion == "anos")
            {
                DBFunctions.Request(vehiculosRegistrados, IncludeSchema.NO,
                                    "select " +
                                    "mcv.mnit_nit, " +
                                    "CASE  WHEN tf1.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F1, " +
                                    "CASE  WHEN tf2.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F2, " +
                                    "CASE  WHEN tf3.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F3, " +
                                    "CASE  WHEN tf4.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F4, " +
                                    "CASE  WHEN tf5.mnit_nit IS NOT NULL THEN 'SI' ELSE 'NO' END AS F5 " +
                                    "FROM " +
                                    "(SELECT DISTINCT mnit_nit FROM DBXSCHEMA.mcatalogovehiculo ORDER BY mnit_nit) mcv " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[0].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[0].AddYears(1)).Date.ToString("yyyy-MM-dd") + "') tf1 on mcv.mnit_nit= tf1.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[1].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[1].AddYears(1)).Date.ToString("yyyy-MM-dd") + "') tf2 on mcv.mnit_nit= tf2.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[2].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[2].AddYears(1)).Date.ToString("yyyy-MM-dd") + "') tf3 on mcv.mnit_nit= tf3.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[3].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[3].AddYears(1)).Date.ToString("yyyy-MM-dd") + "') tf4 on mcv.mnit_nit= tf4.mnit_nit " +
                                    "left join (select DISTINCT mnit_nit from morden where mord_entrada>='" + intervalos[4].Date.ToString("yyyy-MM-dd") + "'  AND mord_entrada<'" + (intervalos[4].AddYears(1)).Date.ToString("yyyy-MM-dd") + "') tf5 on mcv.mnit_nit= tf5.mnit_nit " +
                                    "ORDER BY mcv.mnit_nit");
            }

            //Consulta optimizada
            for (int i = 0; i < vehiculosRegistrados.Tables[0].Rows.Count; i++)
            {
                ArrayList resultados = new ArrayList();
                DataRow fila = tablaAsociada.NewRow();
                fila["NIT PROPIETARIO"] = vehiculosRegistrados.Tables[0].Rows[i][0].ToString();
                for (int j = 0; j < 5; j++)
                {
                    if (opcion == "semestres")
                    {
                        fila[intervalos[j].Date.ToString("yyyy-MM-dd") + " A " + (intervalos[j].AddMonths(6)).Date.ToString("yyyy-MM-dd")] = vehiculosRegistrados.Tables[0].Rows[i][j + 1].ToString();
                        resultados.Add(fila[intervalos[j].Date.ToString("yyyy-MM-dd") + " A " + (intervalos[j].AddMonths(6)).Date.ToString("yyyy-MM-dd")].ToString());
                    }
                    else if (opcion == "anos")
                    {
                        fila[intervalos[j].Date.ToString("yyyy-MM-dd") + " A " + (intervalos[j].AddYears(1)).Date.ToString("yyyy-MM-dd")] = vehiculosRegistrados.Tables[0].Rows[i][j+1].ToString();
                        resultados.Add(fila[intervalos[j].Date.ToString("yyyy-MM-dd") + " A " + (intervalos[j].AddYears(1)).Date.ToString("yyyy-MM-dd")].ToString());
                    }
                }
                fila["CATEGORIA CLIENTE"] = this.Encontrar_Categoria(resultados);
                tablaAsociada.Rows.Add(fila);
            }

			grillaCategorizacion.DataSource = tablaAsociada;
			grillaCategorizacion.DataBind();
			Session["tablaAsociada"] = tablaAsociada;
			this.Realizar_Actualizacion();
		}
		
		protected void Preparar_Tabla_Resultado()
		{
			tablaResultado = new DataTable();
			tablaResultado.Columns.Add(new DataColumn("PERIODO 1",System.Type.GetType("System.String")));
			tablaResultado.Columns.Add(new DataColumn("PERIODO 2",System.Type.GetType("System.String")));
			tablaResultado.Columns.Add(new DataColumn("PERIODO 3",System.Type.GetType("System.String")));
			tablaResultado.Columns.Add(new DataColumn("PERIODO 4",System.Type.GetType("System.String")));
			tablaResultado.Columns.Add(new DataColumn("PERIODO 5",System.Type.GetType("System.String")));
			tablaResultado.Columns.Add(new DataColumn("CATEGORIA CLIENTE",System.Type.GetType("System.String")));
			tablaResultado.Columns.Add(new DataColumn("TOTAL",System.Type.GetType("System.String")));
			tablaResultado.Columns.Add(new DataColumn("PORCENTAJE",System.Type.GetType("System.String")));

		}
		
		protected void Llenar_Grilla_Resultado()
		{
			this.Preparar_Tabla_Resultado();
			double total = System.Convert.ToDouble(tablaAsociada.Rows.Count);
			//Tenemos en cuenta la cantidad de posibilidades y la forma de construirlos
			for(int i=0;i<12;i++)
			{
				DataRow fila = tablaResultado.NewRow();
				if(i==0)
				{
					fila["PERIODO 1"] = "SI";
					fila["PERIODO 2"] = "SI";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "SI";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "A";
					fila["TOTAL"] =  this.Contar_Cantidad_Categoria("A").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("A")/total)*100).ToString()+"%";
				}
				else if(i==1)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "SI";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "SI";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "B";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("B").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("B")/total)*100).ToString()+"%";
				}
				else if(i==2)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "NO";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "SI";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "C";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("C").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("C")/total)*100).ToString()+"%";
				}
				else if(i==3)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "NO";
					fila["PERIODO 3"] = "NO";
					fila["PERIODO 4"] = "SI";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "D";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("D").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("D")/total)*100).ToString()+"%";
				}
				else if(i==4)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "NO";
					fila["PERIODO 3"] = "NO";
					fila["PERIODO 4"] = "NO";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "E";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("E").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("E")/total)*100).ToString()+"%";
				}
				else if(i==5)
				{
					fila["PERIODO 1"] = "SI";
					fila["PERIODO 2"] = "SI";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "NO";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "F";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("F").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("F")/total)*100).ToString()+"%";
				}
				else if(i==6)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "SI";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "NO";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "G";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("G").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("G")/total)*100).ToString()+"%";
				}
				else if(i==7)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "NO";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "NO";
					fila["PERIODO 5"] = "SI";
					fila["CATEGORIA CLIENTE"] = "H";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("H").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("H")/total)*100).ToString()+"%";
				}
				else if(i==8)
				{
					fila["PERIODO 1"] = "SI";
					fila["PERIODO 2"] = "SI";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "NO";
					fila["PERIODO 5"] = "NO";
					fila["CATEGORIA CLIENTE"] = "I";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("I").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("I")/total)*100).ToString()+"%";
				}
				else if(i==9)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "SI";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "NO";
					fila["PERIODO 5"] = "NO";
					fila["CATEGORIA CLIENTE"] = "J";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("J").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("J")/total)*100).ToString()+"%";
				}
				else if(i==10)
				{
					fila["PERIODO 1"] = "NO";
					fila["PERIODO 2"] = "NO";
					fila["PERIODO 3"] = "SI";
					fila["PERIODO 4"] = "NO";
					fila["PERIODO 5"] = "NO";
					fila["CATEGORIA CLIENTE"] = "K";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("K").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("K")/total)*100).ToString()+"%";
				}
				else if(i==11)
				{
					fila["PERIODO 1"] = "?";
					fila["PERIODO 2"] = "?";
					fila["PERIODO 3"] = "?";
					fila["PERIODO 4"] = "?";
					fila["PERIODO 5"] = "?";
					fila["CATEGORIA CLIENTE"] = "Z";
					fila["TOTAL"] = this.Contar_Cantidad_Categoria("Z").ToString();
					fila["PORCENTAJE"] = ((this.Contar_Cantidad_Categoria("Z")/total)*100).ToString()+"%";
				}
				tablaResultado.Rows.Add(fila);
			}
			grillaResultado.DataSource = tablaResultado;
			grillaResultado.DataBind();
		}
		
		protected string Encontrar_Categoria(ArrayList resultados)
		{
			string resultado = "";
			if(resultados.Count!=5)
				lb.Text += "error";
			else
			{
				if((resultados[0].ToString()=="SI")&&(resultados[1].ToString()=="SI")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="SI")&&(resultados[4].ToString()=="SI"))
					resultado = "A";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="SI")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="SI")&&(resultados[4].ToString()=="SI"))
					resultado = "B";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="NO")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="SI")&&(resultados[4].ToString()=="SI"))
					resultado = "C";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="NO")&&(resultados[2].ToString()=="NO")&&(resultados[3].ToString()=="SI")&&(resultados[4].ToString()=="SI"))
					resultado = "D";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="NO")&&(resultados[2].ToString()=="NO")&&(resultados[3].ToString()=="NO")&&(resultados[4].ToString()=="SI"))
					resultado = "E";
				else if((resultados[0].ToString()=="SI")&&(resultados[1].ToString()=="SI")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="NO")&&(resultados[4].ToString()=="SI"))
					resultado = "F";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="SI")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="NO")&&(resultados[4].ToString()=="SI"))
					resultado = "G";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="NO")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="NO")&&(resultados[4].ToString()=="SI"))
					resultado = "H";
				else if((resultados[0].ToString()=="SI")&&(resultados[1].ToString()=="SI")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="NO")&&(resultados[4].ToString()=="NO"))
					resultado = "I";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="SI")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="NO")&&(resultados[4].ToString()=="NO"))
					resultado = "J";
				else if((resultados[0].ToString()=="NO")&&(resultados[1].ToString()=="NO")&&(resultados[2].ToString()=="SI")&&(resultados[3].ToString()=="NO")&&(resultados[4].ToString()=="NO"))
					resultado = "K";
				else
					resultado = "Z";				
			}
			return resultado;
		}
		
		protected int Contar_Cantidad_Categoria(string categoria)
		{
			int cantidadContada = 0;
			for(int i=0;i<tablaAsociada.Rows.Count;i++)
			{
				if(tablaAsociada.Rows[i][7].ToString()==categoria)
					cantidadContada+=1;
			}
			return cantidadContada;
		}
		
		protected void Construir_Niveles_Categoria()
		{
			estadosCategoria = new ArrayList();
			nivelCategoria = new ArrayList();
			for(int i=0;i<12;i++)
			{
				if(i==0)
					estadosCategoria.Add("A");
				else if(i==1)
					estadosCategoria.Add("B");
				else if(i==2)
					estadosCategoria.Add("C");	
				else if(i==3)
					estadosCategoria.Add("D");
				else if(i==4)
					estadosCategoria.Add("E");
				else if(i==5)
					estadosCategoria.Add("F");
				else if(i==6)
					estadosCategoria.Add("G");
				else if(i==7)
					estadosCategoria.Add("H");
				else if(i==8)
					estadosCategoria.Add("I");
				else if(i==9)
					estadosCategoria.Add("J");
				else if(i==10)
					estadosCategoria.Add("K");
				else if(i==11)
					estadosCategoria.Add("Z");
				nivelCategoria.Add(i);
			}
		}
		
		protected int Traer_Nivel_Categoria(string categoria)
		{
			int nivel = 15;
            //for(int i=0;i<estadosCategoria.Count;i++)
            //{
            //    if(estadosCategoria[i].ToString()==categoria)
            //        nivel = System.Convert.ToInt32(nivelCategoria[i]);
            //}

            if (estadosCategoria[11].ToString() == categoria) nivel = 11;
            else if (estadosCategoria[0].ToString() == categoria) nivel = 0;
            else if (estadosCategoria[1].ToString() == categoria) nivel = 1;
            else if (estadosCategoria[2].ToString() == categoria) nivel = 2;
            else if (estadosCategoria[3].ToString() == categoria) nivel = 3;
            else if (estadosCategoria[4].ToString() == categoria) nivel = 4;
            else if (estadosCategoria[5].ToString() == categoria) nivel = 5;
            else if (estadosCategoria[6].ToString() == categoria) nivel = 6;
            else if (estadosCategoria[7].ToString() == categoria) nivel = 7;
            else if (estadosCategoria[8].ToString() == categoria) nivel = 8;
            else if (estadosCategoria[9].ToString() == categoria) nivel = 9;
            else if (estadosCategoria[10].ToString() == categoria) nivel = 10;
      
			return nivel;
		}
		
		protected void Realizar_Actualizacion()
		{
			DataSet nitsDuenos = new DataSet();
            DBFunctions.Request(nitsDuenos, IncludeSchema.NO, "SELECT DISTINCT mnit_nit FROM mcatalogovehiculo ORDER BY mnit_nit");
			for(int i=0;i<nitsDuenos.Tables[0].Rows.Count;i++)
			{
				string resultadoCategoria = this.Buscar_Categoria_Actualizacion(nitsDuenos.Tables[0].Rows[i][0].ToString(),i);
				if(resultadoCategoria=="NO")
					lb.Text += "error : NO SIRVIO";
				else
				{
					DBFunctions.NonQuery("UPDATE mcatalogovehiculo SET mcat_categoria='"+resultadoCategoria+"' WHERE mnit_nit='"+nitsDuenos.Tables[0].Rows[i][0].ToString()+"'");
				}
			}
		}
		
		protected string Buscar_Categoria_Actualizacion(string nitPropietario, int p)
		{
			int resultadoCategoria = 10000;
            //for(int i=0;i<tablaAsociada.Rows.Count;i++)
            //{

			if(tablaAsociada.Rows[p][0].ToString()==nitPropietario)
            {
				int resultadoTemporal = this.Traer_Nivel_Categoria(tablaAsociada.Rows[p][7].ToString());
				if(resultadoTemporal<resultadoCategoria)
					resultadoCategoria = resultadoTemporal;
			}
            
            //}
			string retorno = "";
			if(resultadoCategoria==1000)
				retorno = "NO";
			else
			{
				retorno = estadosCategoria[resultadoCategoria].ToString();
			}				
			return retorno;
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

		}
		#endregion
	}
}
