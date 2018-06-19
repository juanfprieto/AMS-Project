using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;
using System.Globalization;

namespace AMS.Finanzas.Tesoreria
{
	public partial class NotasBancarias : System.Web.UI.UserControl
	{
		protected DataTable tablaNotas;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
				//this.Cargar_Tabla();
				//this.Mostrar_Grid();
			}
			else
			{
				if(Session["tablaNotas"]!=null)
					tablaNotas=(DataTable)Session["tablaNotas"];
			}
		}
		
		protected void Cargar_Tabla()
		{
			this.tablaNotas=new DataTable();
			this.tablaNotas.Columns.Add("CUENTA",typeof(string));
			this.tablaNotas.Columns.Add("VALOR",typeof(double));
		}
		
		protected void Mostrar_Grid()
		{
			Session["tablaNotas"]=tablaNotas;
			gridNotas.DataSource=tablaNotas;
			gridNotas.DataBind();
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
			Session.Remove("tablaNotas");
			this.Cargar_Tabla();
			this.Mostrar_Grid();
			aceptar.Enabled=false;
			codigoCC.Enabled=false;
            fldTabla.Visible = true;
		}
		
        protected void griNotaExcel(DataSet dsExcel)
        {
            Control padre = (this.Parent).Parent;
            ((Label)padre.FindControl("lbDetalle")).Text = "Detalle Nota :";
            ((Label)padre.FindControl("lbValor")).Text = "Valor Total Nota :";
            ((TextBox)padre.FindControl("detalleTransaccion")).Visible = true;
            ((TextBox)padre.FindControl("valorConsignado")).Visible = true;
            for(int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
            {
                double val = Convert.ToDouble(dsExcel.Tables[0].Rows[i][1].ToString());
                //((TextBox)padre.FindControl("valorConsignado")).Text = (Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)) + val.ToString("C"));
                if(i == 0)
                {
                    ((TextBox)padre.FindControl("valorConsignado")).Text = (Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1)) + Convert.ToDouble(val)).ToString("C");
                }
                else
                {
                    ((TextBox)padre.FindControl("valorConsignado")).Text = (double.Parse(((TextBox)padre.FindControl("valorConsignado")).Text, NumberStyles.Currency) + val).ToString("C");
                }
            }
            ((TextBox)padre.FindControl("valorConsignado")).ReadOnly = true;
            ((TextBox)padre.FindControl("totalEfectivo")).Visible = false;
            ((Panel)padre.FindControl("panelValores")).Visible = true;
            ((Button)padre.FindControl("guardar")).Enabled = true;
            //tablaNotas.Rows.Add(fila);
            this.Mostrar_Grid();
            //gridNotas.DataSource = tablaNotas;
            //gridNotas.DataBind();
        }
		protected void gridNotas_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
			Control padre=(this.Parent).Parent;
			if(((Button)e.CommandSource).CommandName=="agregar")
			{
				if((((TextBox)e.Item.Cells[0].Controls[1]).Text=="")||(Convert.ToDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text)==0)||(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "Faltan Datos o el valor es cero");
				else
				{
					fila=this.tablaNotas.NewRow();
					fila[0]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila[1]=Convert.ToDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text);
					((Label)padre.FindControl("lbDetalle")).Text="Detalle Nota :";
					((Label)padre.FindControl("lbValor")).Text="Valor Total Nota :";
					((TextBox)padre.FindControl("detalleTransaccion")).Visible=true;
					((TextBox)padre.FindControl("valorConsignado")).Visible=true;
					//((TextBox)padre.FindControl("valorConsignado")).Text=(Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1))+Convert.ToDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text)).ToString("C");
                    ((TextBox)padre.FindControl("valorConsignado")).Text = (double.Parse(((TextBox)padre.FindControl("valorConsignado")).Text, NumberStyles.Currency) + Convert.ToDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text)).ToString("C");
					((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
					((TextBox)padre.FindControl("totalEfectivo")).Visible=false;
					((Panel)padre.FindControl("panelValores")).Visible=true;
					((Button)padre.FindControl("guardar")).Enabled=true;
					tablaNotas.Rows.Add(fila);
					this.Mostrar_Grid();
				}
			}
			else if(((Button)e.CommandSource).CommandName=="remover")
			{
				//((TextBox)padre.FindControl("valorConsignado")).Text=(Convert.ToDouble(((TextBox)padre.FindControl("valorConsignado")).Text.Substring(1))-Convert.ToDouble(this.tablaNotas.Rows[e.Item.DataSetIndex][1].ToString())).ToString("C");
                ((TextBox)padre.FindControl("valorConsignado")).Text = (double.Parse(((TextBox)padre.FindControl("valorConsignado")).Text, NumberStyles.Currency) - Convert.ToDouble(this.tablaNotas.Rows[e.Item.DataSetIndex][1].ToString())).ToString("C");
				if(((TextBox)padre.FindControl("valorConsignado")).Text=="$0.00")
					((Button)padre.FindControl("guardar")).Enabled=false;
				this.tablaNotas.Rows[e.Item.DataSetIndex].Delete();
				Session["tablaNotas"]=tablaNotas;
			}
            gridNotas.DataSource = tablaNotas;
            gridNotas.DataBind();
        }

        protected void btnCargar_Click1(object sender, System.EventArgs e)
        {
            Random num = new Random();
            DataSet dsNota = new DataSet();
            if (flArchivoExcel.PostedFile.FileName.ToString() == string.Empty)
            {
                Utils.MostrarAlerta(Response, "No ha seleccionado un archivo'");
                return;
            }
            else
            {
                try
                {
                    string[] file = flArchivoExcel.PostedFile.FileName.ToString().Split('\\');
                    string fileName = file[file.Length - 1];
                    string[] fileNameParts = fileName.Split('.');
                    string fileExtension = fileNameParts[fileNameParts.Length - 1];

                    if (fileExtension.ToUpper() != "XLS" && fileExtension.ToUpper() != "XLSX")
                    {
                        Utils.MostrarAlerta(Response, "No es un archivo de Excel");
                        return;
                    }

                    else
                    {
                        int numero = num.Next(0, 9999);
                        flArchivoExcel.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileNameParts[0] + "_" + numero + "." + fileExtension);
                        ExcelFunctions exc = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileNameParts[0] + "_" + numero + "." + fileExtension);
                        bool leiArchivo = false;
                        try
                        {
                            exc.Request(dsNota, IncludeSchema.NO, "SELECT * FROM CUENTA");
                            if (dsNota.Tables.Count == 0)
                            {
                                Utils.MostrarAlerta(Response, "No se encontró ninguna Tabla, verifique que haya seguido adecuadamente los pasos");
                                return;
                            }
                            if (dsNota.Tables[0].Rows.Count == 0)
                            {
                                Utils.MostrarAlerta(Response, "No se encontró ningún registro en la tabla, verifique que haya seguido adecuadamente los pasos");
                                return;
                            }
                            else
                            {

                                for (int i = 0; i < dsNota.Tables[0].Columns.Count; i++)
                                {
                                    dsNota.Tables[0].Columns[i].ColumnName = dsNota.Tables[0].Rows[0][i].ToString();
                                }
                                dsNota.Tables[0].Rows[0].Delete();
                                dsNota.Tables[0].Rows[0].AcceptChanges();//es como un flush, cierra la edición por lo que reajusta la tabla
                                leiArchivo = true;
                            }

                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "No se pudo leer ningún registro en el archivo de Excel");
                            return;
                        }
                        if (leiArchivo)
                        {
                            if (dsNota.Tables[0].Rows.Count == 0)
                            {
                                Utils.MostrarAlerta(Response, "No se encontro ningún registro en el archivo de Excel");
                                return;
                            }

                            else
                            {
                                tablaNotas.Clear();
                                lbError.Text = "";
                                for (int i = 0; i < dsNota.Tables[0].Rows.Count; i++)
                                {
                                    if(DBFunctions.RecordExist("SELECT mcue_codipuc AS Cuenta FROM mcuenta WHERE timp_codigo IN('A','P') AND MCUE_CODIPUC = '" + dsNota.Tables[0].Rows[i][0].ToString() + "'"))
                                    {
                                        double n;
                                        if(double.TryParse(dsNota.Tables[0].Rows[i][1].ToString(), out n))
                                        {
                                            DataRow dr = tablaNotas.NewRow();
                                            dr["CUENTA"] = dsNota.Tables[0].Rows[i][0].ToString();
                                            dr["VALOR"] = dsNota.Tables[0].Rows[i][1].ToString();
                                            tablaNotas.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            //lbError.Text += "No se agregó la cuenta " + dsNota.Tables[0].Rows[i][0].ToString() + " porque el valor [ " + dsNota.Tables[0].Rows[i][1].ToString() + " ] está mal escrito. Verifique que sólo contenga números. <br />";
                                            lbError.Text += "No se agregó la cuenta " + dsNota.Tables[0].Rows[i][0].ToString() + " porque el valor está mal escrito. Verifique que sólo contenga números. [Linea Excel: " + i + "]<br />";
                                        }
                                        
                                    }
                                    else
                                    {
                                        lbError.Text += "No se agregó la cuenta [ " + dsNota.Tables[0].Rows[i][0].ToString() + " ]  porque no existe, Por favor verifique.. [Linea Excel: " + i + "]<br />";
                                    }
                                    
                                }
                            }
                            griNotaExcel(dsNota);
                            gridNotas.DataSource = tablaNotas;
                            gridNotas.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.RegisterClientScriptBlock("status", ex.ToString());
                }
            }
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
