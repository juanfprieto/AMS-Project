using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.Reportes;
using AMS.DB;
using AMS.Tools;


namespace AMS.Contabilidad
{
	
	
	public partial class DefCueAnexos : System.Web.UI.UserControl 
	{
		protected DataSet lineas;
		protected DataTable resultado;
		protected System.Web.UI.HtmlControls.HtmlGenericControl P1;	
		protected string reportTitle="Anexos";	
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(year,  "SELECT pano_ano FROM pano order by 1 desc");
				bind.PutDatasIntoDropDownList(month, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 13 order by 1");				
				bind.PutDatasIntoDropDownList(anexo, "SELECT pane_nombanex FROM panexobalance order by 1");
                bind.PutDatasIntoDropDownList(clase, "SELECT TNII_CODIGO, TNII_NOMBRE FROM tniif order by 1");
            }	
		}
	
	
		protected  void  generar(Object  Sender, EventArgs e)
		{
			string []pr=new string[2];
			double SaldoParcial=0;
			double SaldoFinal= 0;
			string SaldoFinal1=null;
			
			pr[0]=pr[1]="";
			Press frontEnd = new Press(new DataSet(), reportTitle);
			frontEnd.PreHeader(tabPreHeader, Grid.Width, pr);
			frontEnd.Firmas(tabFirmas,Grid.Width);

			lb.Text="";
			bool Acumulado = false;
            bool biMestre = false;
			//Se revisa si el usuario desea el anexo acumulado o al mes escogido
            if (opciones.SelectedItem.Text == "Acumulado Año")
                Acumulado = true;
            else
            {
                if (opciones.SelectedItem.Text == "Bimestre")
                    biMestre = true;
            }
			///////////////////////////////////////////////////////
			this.PrepararTabla();
			lineas = new DataSet();
			DBFunctions.Request(lineas,IncludeSchema.NO,"SELECT PL.pane_codianex, PL.plin_linea, PL.plin_nombline, PL.plin_tipolinea, PL.plin_formula, PL.activa FROM plineanexo PL, panexobalance PX WHERE PL.pane_codianex=PX.pane_codianex AND PX.pane_nombanex='"+anexo.SelectedItem.ToString()+"' order by 1,2,3");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
				double saldoLinea = 0;//Variable para ir almacenado el saldo de la linea
				string numero=lineas.Tables[0].Rows[i].ItemArray[1].ToString();
				//Tipo=cuenta
				if((lineas.Tables[0].Rows[i].ItemArray[3].ToString().Trim())=="C")
				{
					DataSet caractLineas = new DataSet();
					DBFunctions.Request(caractLineas,IncludeSchema.NO,"SELECT plin_iinicial, plin_ifinal, plin_operacion, plin_tipmov FROM pcuentaanexo WHERE pane_codianex='"+lineas.Tables[0].Rows[i].ItemArray[0].ToString()+"'AND plin_linea='"+lineas.Tables[0].Rows[i].ItemArray[1].ToString()+"'");
                    for (int j = 0; j < caractLineas.Tables[0].Rows.Count; j++)
                    {
                        DataSet cuentAfectadas = new DataSet();
                        string mes = DBFunctions.SingleData("SELECT pmes_mes FROM pmes WHERE pmes_nombre='" + month.SelectedItem.ToString() + "'");
                        Int16 mesInicial = 0;
                        Int16 mesFinal = 0;
                        Int16 claseCta = Convert.ToInt16(clase.SelectedValue.ToString());  // 1=Legal, 2=Niif
                        if (!Acumulado && !biMestre) // un mes
                            DBFunctions.Request(cuentAfectadas, IncludeSchema.NO, "SELECT m.mcue_codipuc, case when '" + claseCta + "' = 1 then msal_valodebi else msal_niifdebi end, case when '" + claseCta + "' = 1 then msal_valocred else msal_niifcred end, tnat_codigo FROM msaldocuenta ms, mcuenta m WHERE m.mcue_codipuc = ms.mcue_codipuc and pano_ano=" + year.SelectedItem.ToString() + " AND pmes_mes=" + mes + " AND M.mcue_codipuc>='" + caractLineas.Tables[0].Rows[j].ItemArray[0].ToString() + "' AND M.mcue_codipuc<='" + caractLineas.Tables[0].Rows[j].ItemArray[1].ToString() + "'");
                        else if (Acumulado) // Acumulado del año hasta el mes 
                            DBFunctions.Request(cuentAfectadas, IncludeSchema.NO,
                                @"SELECT m.mcue_codipuc, case when '" + claseCta + "' = 1 then msal_valodebi else msal_niifdebi end, case when '" + claseCta + "' = 1 then msal_valocred else msal_niifcred end, tnat_codigo FROM msaldocuenta ms, mcuenta m WHERE m.mcue_codipuc = ms.mcue_codipuc and pano_ano=" + year.SelectedItem.ToString() + " AND pmes_mes<=" + mes + " AND m.mcue_codipuc>='" + caractLineas.Tables[0].Rows[j].ItemArray[0].ToString() + "' AND m.mcue_codipuc<='" + caractLineas.Tables[0].Rows[j].ItemArray[1].ToString() + "' ");
                        else
                        {
                            if (mes == "1" || mes == "3" || mes == "5" || mes == "7" || mes == "9" || mes == "11")
                            {
                                mesInicial = Convert.ToInt16(mes);
                                mesFinal = Convert.ToInt16(mesInicial + 1);
                            }
                            else
                            {
                                mesFinal = Convert.ToInt16(mes);
                                mesInicial = Convert.ToInt16(mesFinal - 1);
                            }
                            DBFunctions.Request(cuentAfectadas, IncludeSchema.NO,
                                @"SELECT m.mcue_codipuc, case when '"+claseCta+"' = 1 then msal_valodebi else msal_niifdebi end, case when '"+claseCta +"'= 1 then msal_valocred else msal_niifcred end, tnat_codigo FROM mcuenta m, msaldocuenta ms WHERE m.mcue_codipuc = ms.mcue_codipuc and pano_ano=" + year.SelectedItem.ToString() + " AND pmes_mes>=" + mesInicial + " AND pmes_mes<=" + mesFinal + " AND m.mcue_codipuc>='" + caractLineas.Tables[0].Rows[j].ItemArray[0].ToString() + "' AND m.mcue_codipuc<='" + caractLineas.Tables[0].Rows[j].ItemArray[1].ToString() + "'");
                        }
                        /////////////////Consulta de saldos//////////////////
                        if (cuentAfectadas.Tables.Count > 0)
                        {
                            for (int k = 0; k < cuentAfectadas.Tables[0].Rows.Count; k++)
                            {
                                if ((caractLineas.Tables[0].Rows[j].ItemArray[2].ToString().Trim()) == "S")//Si es SUMA
                                {
                                    if ((caractLineas.Tables[0].Rows[j].ItemArray[3].ToString().Trim()) == "S")//Si es el saldo
                                    {
                                        string naturaleza = cuentAfectadas.Tables[0].Rows[k].ItemArray[3].ToString(); //DBFunctions.SingleData("SELECT tnat_codigo FROM mcuenta WHERE mcue_codipuc='"+cuentAfectadas.Tables[0].Rows[k].ItemArray[0].ToString()+"'");

                                        if (naturaleza == "D")
                                            saldoLinea += System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[1]) - System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[2]);
                                        else if (naturaleza == "C")
                                            saldoLinea += System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[2]) - System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[1]);
                                    }
                                    else if ((caractLineas.Tables[0].Rows[j].ItemArray[3].ToString().Trim()) == "C")//Si es el credito
                                        saldoLinea += System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[2]);

                                    else if ((caractLineas.Tables[0].Rows[j].ItemArray[3].ToString().Trim()) == "D")//Si es el debito
                                        saldoLinea += System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[1]);
                                }

                                ////EN CASO QUE LA OPERACION SEA UNA RESTA
                                else if ((caractLineas.Tables[0].Rows[j].ItemArray[2].ToString().Trim()) == "R")
                                {
                                    if ((caractLineas.Tables[0].Rows[j].ItemArray[3].ToString().Trim()) == "S")//Si es el saldo
                                    {
                                        string naturaleza = cuentAfectadas.Tables[0].Rows[k].ItemArray[3].ToString(); //  DBFunctions.SingleData("SELECT tnat_codigo FROM mcuenta WHERE mcue_codipuc='"+cuentAfectadas.Tables[0].Rows[k].ItemArray[0].ToString()+"'");
                                        if (naturaleza == "D")
                                            saldoLinea -= System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[1]) - System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[2]);
                                        else if (naturaleza == "C")
                                            saldoLinea -= System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[2]) - System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[1]);
                                    }
                                    else if ((caractLineas.Tables[0].Rows[j].ItemArray[3].ToString().Trim()) == "C")//Si es el credito
                                        saldoLinea -= System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[2]);

                                    else if ((caractLineas.Tables[0].Rows[j].ItemArray[3].ToString().Trim()) == "D")//Si es el debito
                                        saldoLinea -= System.Convert.ToDouble(cuentAfectadas.Tables[0].Rows[k].ItemArray[1]);
                                }
                            }
                        }
                    }
					SaldoParcial=saldoLinea+SaldoParcial;
					SaldoFinal1=String.Format("{0:C}",saldoLinea);
										
				}//Fin sentencia If para discriminar el tipo de linea del renglon 
				//Tipo=Renglon
				if((lineas.Tables[0].Rows[i].ItemArray[3].ToString().Trim())=="R")
					//la pocision del itemarray[] es el numero de la columna en la base de datos siendo 0 la primera
				{
					
					SaldoFinal=SaldoParcial;
					SaldoParcial= 0;
					saldoLinea=SaldoFinal;
					SaldoFinal=0;
					SaldoFinal1=String.Format("{0:C}",saldoLinea);
				}

				//Tipo=Formula?
				if((lineas.Tables[0].Rows[i].ItemArray[3].ToString().Trim())=="L")
				{
					lb.Text+="<BR>formula="+lineas.Tables[0].Rows[i].ItemArray[3].ToString();
				}

				//Tipo=Sentencia
				if((lineas.Tables[0].Rows[i].ItemArray[3].ToString().Trim())=="S")
				{
					SaldoFinal1=DBFunctions.SingleData(lineas.Tables[0].Rows[i]["plin_formula"].ToString());
				}
				//Tipo=Titulo
				if((lineas.Tables[0].Rows[i].ItemArray[3].ToString().Trim())=="T")
				{
					SaldoFinal1="";
					numero="";
				}

			
				//Vamos a crear una fila para nuestro DataTable resultado, que almacene los resultados de las operaciones anteriores
				DataRow fila;
				
				fila= resultado.NewRow();
				fila["NOMBRE"] = lineas.Tables[0].Rows[i].ItemArray[2].ToString();
				fila["NUMERO"]	= numero;
				fila["TOTAL"] = SaldoFinal1;
							
				//Linea Activa?
				if((lineas.Tables[0].Rows[i].ItemArray[5].ToString().Trim())=="S")
					resultado.Rows.Add(fila);
						

			}//fin sentencia FOR	
			Grid.DataSource = resultado;
			Grid.DataBind();	
			StringBuilder SB=new StringBuilder();
			StringWriter SW=new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			tabPreHeader.RenderControl(htmlTW);
			Grid.RenderControl(htmlTW);
			tabFirmas.RenderControl(htmlTW);
			string strRep;
			strRep=SB.ToString();
			Session.Clear();
			Session["Rep"]=strRep;
			toolsHolder.Visible = true;
		}

		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			generar(Sender, E);
            Utils.MostrarAlerta(Response, Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, Grid));
			Grid.EnableViewState=false;
		}
		
		
		///FUNCION QUE PREPARA EL DATATABLE RESULTADO PARA SER UTILIZADO
		/// ADICIONA LAS COLUMNAS NECESARIAS PARA ALMACENAR LOS RESULTADOS
		public void PrepararTabla()
		{
			resultado = new DataTable();
			//Adicionamos una columna que almacene el nombre de la linea
			DataColumn nombre = new DataColumn();
			nombre.DataType = System.Type.GetType("System.String");
			nombre.ColumnName = "NOMBRE";
			nombre.ReadOnly=true;
			resultado.Columns.Add(nombre);
			//Adicionamos una columna que almacene el numero de la linea
			DataColumn numero = new DataColumn();
			numero.DataType = System.Type.GetType("System.String");
			numero.ColumnName = "NUMERO";
			numero.ReadOnly=true;
			resultado.Columns.Add(numero);
			//Adicionamos una columna que almacene el total de la linea
			DataColumn total = new DataColumn();
			total.DataType = System.Type.GetType("System.String");
			total.ColumnName = "TOTAL";
			total.ReadOnly=true;
			resultado.Columns.Add(total);
		
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

		protected void Grid_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
		}
		
		
	}
	

	
	
	
}