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
	public partial class NoCausados : System.Web.UI.UserControl
	{
		protected DataTable tablaNC;
		protected Control padre;
		protected Control encabezado,varios,pagos,documentos;
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];

		protected void Page_Load(object Sender,EventArgs e)
		{
			padre=(this.Parent).Parent;
			encabezado=((PlaceHolder)padre.FindControl("phEncabezado")).Controls[0];
			documentos=((PlaceHolder)padre.FindControl("phDocumentos")).Controls[0];
			varios=((PlaceHolder)padre.FindControl("phVarios")).Controls[0];
			pagos=((PlaceHolder)padre.FindControl("phPagos")).Controls[0];
            plcExcelImporter.Controls.Add(LoadControl(pathToControls + "AMS.Tools.ExcelimportData.ascx"));

			if(!IsPostBack)
			{
				totalDNC.Text=totalCNC.Text="$0.00";
				this.Cargar_tablaNC();
				this.Mostrar_gridNC();
			}
			else
			{
				if(Session["tablaNC"]!=null)
					tablaNC=(DataTable)Session["tablaNC"];
			}
		}
		
		public void Cargar_tablaNC()
		{
			tablaNC=new DataTable();
			tablaNC.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("CUENTA", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("SEDE", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("CENTROCOSTO", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("NUMERO", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("NIT", typeof(string)));
			tablaNC.Columns.Add(new DataColumn("VALORDEBITO", typeof(double)));
			tablaNC.Columns.Add(new DataColumn("VALORCREDITO", typeof(double)));
			tablaNC.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
		}
		
		public void Mostrar_gridNC()
		{
			Session["tablaNC"]=tablaNC;
			gridNC.DataSource=tablaNC;
			gridNC.DataBind();
		}
		
		protected bool Buscar_Documento(string cuentapuc, string prefijo, int numero, string nit, string sede, string cc)
		{
			DataRow[] docs=tablaNC.Select("CUENTA='"+cuentapuc+"' AND PREFIJO='"+prefijo+"' AND NUMERO='"+numero+"' AND NIT ='"+nit+"' AND SEDE ='"+sede+"' AND CENTROCOSTO ='"+cc+"'");
			if(docs.Length!=0)
				return true;
			else
				return false;
		}
		
		protected bool Validar_Datos(DataGridCommandEventArgs e)
		{
			bool error=false;	
			//Si hay algun campo en blanco o no son validos los valores...
			if((((TextBox)e.Item.Cells[0].Controls[1]).Text=="")||(((TextBox)e.Item.Cells[1].Controls[1]).Text=="")||(((TextBox)e.Item.Cells[2].Controls[1]).Text=="")||(((TextBox)e.Item.Cells[3].Controls[1]).Text=="")||(((TextBox)e.Item.Cells[4].Controls[1]).Text=="")||(!DatasToControls.ValidarInt(((TextBox)e.Item.Cells[5].Controls[1]).Text))||(((TextBox)e.Item.Cells[6].Controls[1]).Text=="")||(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text))||(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text))||(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text)))
			{
                Utils.MostrarAlerta(Response, "Falta un Campo por Llenar o las entradas son Invalidos. Revisa tus Datos");
				error=true;
			}
				//Si en los dos campos valor Debito y valor Credito hay un valor distinto de cero
			else if(((((TextBox)e.Item.Cells[7].Controls[1]).Text!="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text!="0")))
			{
                Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener un valor de 0. Revisa tus Datos");
				error=true;
			}
				//Si en ninguno de los dos campos hay valor
			else if((((((TextBox)e.Item.Cells[7].Controls[1]).Text=="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text=="0"))))
			{
                Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener valor. Revisa tus Datos");
				error=true;
			}
				//Si ya se ingreso el prefijo y numero de documento de referencia
			else if(Buscar_Documento(((TextBox)e.Item.Cells[4].FindControl("cuentatxt")).Text,((TextBox)e.Item.Cells[4].FindControl("prefijotxt")).Text,Convert.ToInt32(((TextBox)e.Item.Cells[4].FindControl("numdocutxt")).Text.ToString()), ((TextBox)e.Item.Cells[4].FindControl("edit_numero")).Text, ((TextBox)e.Item.Cells[4].FindControl("edit_sede")).Text, ((TextBox)e.Item.Cells[4].FindControl("centrocostotxt")).Text))
			{
                Utils.MostrarAlerta(Response, "El prefijo y el número de documento de referencia ya fueron ingresados anteriormente"); 
				error=true; 
			}
				//Si el nit digitado no existe
			else if(!DBFunctions.RecordExist("SELECT mnit_nit FROM mnit WHERE mnit_nit='"+((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text+"'"))
			{
                Utils.MostrarAlerta(Response, "El nit especificado no existe");

				error=true;
			}
				//Si la cuenta digitada no existe o ES SOLO niif
            else if (!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='" + ((TextBox)e.Item.Cells[1].FindControl("cuentatxt")).Text + "' and timp_codigo in ('A','P') and tCUE_codigo IN ('F', 'G') "))
			{
                Utils.MostrarAlerta(Response, "La cuenta especificada no existe o es NO Imputable o es solo NIIF o es solo Fiscal");
				error=true;
			}
			return error;
		}

        protected bool Validar_Datos2(ArrayList arraySettingRow)
        {
            bool error = false;

            //Si hay algun campo en blanco o no son validos los valores...
            if ((arraySettingRow[0].ToString() == "") || (arraySettingRow[1].ToString() == "") || (arraySettingRow[2].ToString() == "") || (arraySettingRow[3].ToString() == "") || (arraySettingRow[4].ToString() == "") || (!DatasToControls.ValidarInt(arraySettingRow[5].ToString())) || (arraySettingRow[6].ToString() == "") || (!DatasToControls.ValidarDouble(arraySettingRow[7].ToString())) || (!DatasToControls.ValidarDouble(arraySettingRow[8].ToString())) || (!DatasToControls.ValidarDouble(arraySettingRow[9].ToString())))
            {
                Utils.MostrarAlerta(Response, "Falta un Campo por Llenar o las entradas son Invalidos. Revisa tus Datos");
                error = true;
            }
            //Si en los dos campos valor Debito y valor Credito hay un valor distinto de cero
            else if (((arraySettingRow[7].ToString() != "0") && (arraySettingRow[8].ToString() != "0")))
            {
                Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener un valor de 0. Revisa tus Datos");
                error = true;
            }
            //Si en ninguno de los dos campos hay valor
            else if ((((arraySettingRow[7].ToString() == "0") && (arraySettingRow[8].ToString() == "0"))))
            {
                Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener valor. Revisa tus Datos");
                error = true;
            }
            //Si ya se ingreso el prefijo y numero de documento de referencia
            else if (Buscar_Documento(arraySettingRow[1].ToString(), arraySettingRow[4].ToString(), Convert.ToInt32(arraySettingRow[5].ToString()), arraySettingRow[6].ToString(), arraySettingRow[2].ToString(), arraySettingRow[3].ToString()))
            {
                Utils.MostrarAlerta(Response, "El prefijo y el número de documento de referencia ya fueron ingresados anteriormente");
                error = true;
            }
            //Si el nit digitado no existe
            else if (!DBFunctions.RecordExist("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + arraySettingRow[6].ToString() + "'"))
            {
                Utils.MostrarAlerta(Response, "El nit especificado no existe");

                error = true;
            }
            //Si la cuenta digitada no existe
            else if (!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='" + arraySettingRow[1].ToString() + "' and timp_codigo in ('A','P') and tCUE_codigo IN ('F','G') "))
            {
                Utils.MostrarAlerta(Response, "La cuenta especificada no existe o es NO Imputable o es solo Niif");
                error = true;
            }
            //Si la sede no existe
            else if (!DBFunctions.RecordExist("SELECT palm_almacen FROM palmacen where (pcen_centcart is not null or pcen_centteso is not null) and palm_almacen='" + arraySettingRow[2].ToString() + "' order by palm_descripcion;"))
            {
                Utils.MostrarAlerta(Response, "La sede especificada no existe");
                error = true;
            }
            //Si el centro de costo no existe
            else if (!DBFunctions.RecordExist("SELECT pcen_codigo AS Codigo FROM pcentrocosto where timp_codigo <> 'N' and pcen_codigo = '" + arraySettingRow[3].ToString() + "' order by 1;"))
            {
                Utils.MostrarAlerta(Response, "El centro de costo especificado no existe");
                error = true;
            }
            return error;
        }
		
		protected void gridNC_Item(object Sender,DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName=="AgregarFilas")
			{
                ArrayList arraySettingRow = new ArrayList();
                for (int h = 0; h < e.Item.Cells.Count-2; h++)
                {
                    arraySettingRow.Add(((TextBox)e.Item.Cells[h].Controls[1]).Text);
                }

                DataRow newFila = validarRegistroFila(arraySettingRow);

                if (newFila != null)
                {
                    totalDNC.Text = (Convert.ToDouble(totalDNC.Text.Substring(1)) + Convert.ToDouble(arraySettingRow[7])).ToString("C");
                    totalCNC.Text = (Convert.ToDouble(totalCNC.Text.Substring(1)) + Convert.ToDouble(arraySettingRow[8])).ToString("C");

                    //Debe recibir la fila retornada del validarRegistroFila()
                    tablaNC.Rows.Add(newFila);
                    gridNC.DataSource = tablaNC;
                    gridNC.DataBind();
                    Session["tablaNC"] = tablaNC;
                }
                
                //Todo este bloque es reemplazado por la funcion  validarRegistroFila()... se dejo comentado para usarse como referencia en otros grids...
                //if (!Validar_Datos2(arraySettingRow))
                //{
                //    if(Session["tablaNC"]==null)
                //        this.Cargar_tablaNC();
                //    fila=tablaNC.NewRow();
                //    fila[0]=((TextBox)e.Item.Cells[0].Controls[1]).Text;
                //    fila[1]=((TextBox)e.Item.Cells[1].Controls[1]).Text;
                //    fila[2]=((TextBox)e.Item.Cells[2].Controls[1]).Text;
                //    fila[3]=((TextBox)e.Item.Cells[3].Controls[1]).Text;
                //    fila[4]=((TextBox)e.Item.Cells[4].Controls[1]).Text;
                //    fila[5]=((TextBox)e.Item.Cells[5].Controls[1]).Text;
                //    fila[6]=((TextBox)e.Item.Cells[6].Controls[1]).Text;
                //    //Si el valor introducido es debito
                //    if((((TextBox)e.Item.Cells[7].Controls[1]).Text!="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text=="0"))
                //    {
                //        fila[7]=Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
                //        fila[8]=Convert.ToDouble("0");
                //    }
                //    //Si el valor introducido es credito
                //    else if((((TextBox)e.Item.Cells[7].Controls[1]).Text=="0")&&(((TextBox)e.Item.Cells[8].Controls[1]).Text!="0"))
                //    {
                //        fila[7]=System.Convert.ToDouble("0");
                //        fila[8]=System.Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
						
                //    }
                //    //Ahora verificamos los valores del valor base
                //    verBase=DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc='"+(((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString()+"'");
                //    //Si la cuenta no soporta valor base y hay algun valor distinto de cero en ese campo
                //    if(verBase=="N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text!="0"))
                //    {
                //        Utils.MostrarAlerta(Response, "La cuenta afectada no soporta Valor Base por tanto se guardara un valor de 0");
                //        fila[9]=System.Convert.ToDouble("0");
                //    }
                //    //Si la cuenta no soporta valor base y hay un valor de cero en ese campo
                //    else if(verBase=="N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text=="0"))
                //        fila[9]=System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                //    //Si la cuenta afectada soporta valor base
                //    else if(verBase=="B")
                //    {
                //        //Convierto a double el valor base
                //        vb1=System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                //        //Miro en la base de datos cual es el porcentaje de valor base
                //        //lo convierto a double y lo divido por 100 para saber el verdadero valor
                //        vporcBase=System.Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc='"+(((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString()+"'"));
                //        if (vporcBase == 0)
                //        {
                //            Utils.MostrarAlerta(Response, "No es posible agregar este concepto, debido a que el valor base definido para esta cuenta es 0.");
                //            return;
                //        }
                //        //Si el valor introducido es debito entonces se calcula el valor base con base en este
                //        if(((TextBox)e.Item.Cells[7].Controls[1]).Text!="0")
                //        {
                //            res=System.Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text)*100/vporcBase;
                //            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                //                fila[9]=vb1;
                //            else
                //            {
                //                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                //                fila[9]=res;
                //            }
                //        }
                //        //Si el valor introducido es credito entonces se calcula el valor base con base en este
                //        else if(((TextBox)e.Item.Cells[8].Controls[1]).Text!="0")
                //        {
                //            res=System.Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text)*100/vporcBase;
                //            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                //                fila[9]=vb1;
                //            else
                //            {
                //                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                //                fila[9]=res;
                //            }
                //        }
                //    }
                //    if (((TextBox)e.Item.Cells[1].Controls[1]).Text.Substring(0, 2) == "13" || ((TextBox)e.Item.Cells[1].Controls[1]).Text.Substring(0, 4) == "2205" || ((TextBox)e.Item.Cells[1].Controls[1]).Text.Substring(0, 4) == "2805")
                //    {
                //        Utils.MostrarAlerta(Response, "La cuenta afectada solo debe manejar DOCUMENTOS CAUSADOS, esta imputación está errada; es su responsabilidad ..!");
                //    }
                //    totalDNC.Text=(Convert.ToDouble(totalDNC.Text.Substring(1))+Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text)).ToString("C");
                //    totalCNC.Text=(Convert.ToDouble(totalCNC.Text.Substring(1))+Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text)).ToString("C");
                //    tablaNC.Rows.Add(fila);
                //    gridNC.DataSource=tablaNC;
                //    gridNC.DataBind();
                //    Session["tablaNC"]=tablaNC;
                //}				
			}
			else if(((Button)e.CommandSource).CommandName=="RemoverFilas")
			{
				//Lo unico que hacemos es restar el valor al total y borramos la fila
				totalDNC.Text=((System.Convert.ToDouble(totalDNC.Text.Substring(1)))-(System.Convert.ToDouble(tablaNC.Rows[e.Item.DataSetIndex][7]))).ToString("C");
				totalCNC.Text=((System.Convert.ToDouble(totalCNC.Text.Substring(1)))-(System.Convert.ToDouble(tablaNC.Rows[e.Item.DataSetIndex][8]))).ToString("C");
				tablaNC.Rows[e.Item.DataSetIndex].Delete();
				gridNC.DataSource=this.tablaNC;
				gridNC.DataBind();
				Session["tablaNC"]=this.tablaNC;
			}
            else if (((Button)e.CommandSource).CommandName == "CopiarFilas")
            {
                Session["indexCopy"] = e.Item.DataSetIndex;
                gridNC.DataSource = this.tablaNC;
                gridNC.DataBind();
            }
		}
		
		protected void Esconder_Controles()
		{
			((PlaceHolder)padre.FindControl("phNoCausados")).Visible=false;
			((PlaceHolder)padre.FindControl("phCancelacionObligFin")).Visible=false;
			((ImageButton)padre.FindControl("btnNoCausados")).ImageUrl="../img/AMS.BotonExpandir.png";
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
            if (Request.QueryString["tipo"] == "RC" || Request.QueryString["tipo"] == "RP")
			{
				if((Convert.ToDouble(totalCNC.Text.Substring(1)))<(Convert.ToDouble(totalDNC.Text.Substring(1))))
                    Utils.MostrarAlerta(Response, "Los débitos son mayores que los créditos en la relación de Conceptos Ingresos - Egresos");
				else
				{
					this.Esconder_Controles();
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
					((Label)pagos.FindControl("lbConceptos")).Text="Total Diferencia Conceptos Varios : "+(Convert.ToDouble(this.totalCNC.Text.Substring(1))-Convert.ToDouble(this.totalDNC.Text.Substring(1))).ToString("C");
				}
			}
			else if(Request.QueryString["tipo"]=="CE")
			{
				if((Convert.ToDouble(totalCNC.Text.Substring(1)))>(Convert.ToDouble(totalDNC.Text.Substring(1))))
                    Utils.MostrarAlerta(Response, "Los créditos son mayores que los débitos en la relación de Conceptos Ingresos - Egresos");
				else
				{
					this.Esconder_Controles();
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
					((Label)pagos.FindControl("lbConceptos")).Text="Total Diferencia Conceptos Varios : "+(Convert.ToDouble(this.totalDNC.Text.Substring(1))-Convert.ToDouble(this.totalCNC.Text.Substring(1))).ToString("C");
				}
			}
		}

		protected void gridNC_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Footer)
            {
                ((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text = ((TextBox)encabezado.FindControl("datBen")).Text;

                //Copia de fila sobre el footer.
                if (Session["indexCopy"] != null && Session["indexCopy"] != "")
                {
                    int indiceCop = Convert.ToInt16(Session["indexCopy"]);

                    ((TextBox)e.Item.Cells[0].FindControl("descripciontxt")).Text = tablaNC.Rows[indiceCop][0].ToString();
                    ((TextBox)e.Item.Cells[1].FindControl("cuentatxt")).Text = tablaNC.Rows[indiceCop][1].ToString();
                    ((TextBox)e.Item.Cells[2].FindControl("almacentxt")).Text = tablaNC.Rows[indiceCop][2].ToString();
                    ((TextBox)e.Item.Cells[3].FindControl("centrocostotxt")).Text = tablaNC.Rows[indiceCop][3].ToString();
                    ((TextBox)e.Item.Cells[4].FindControl("prefijotxt")).Text = tablaNC.Rows[indiceCop][4].ToString();
                    ((TextBox)e.Item.Cells[5].FindControl("numdocutxt")).Text = tablaNC.Rows[indiceCop][5].ToString();
                    ((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text = tablaNC.Rows[indiceCop][6].ToString();
                    ((TextBox)e.Item.Cells[7].FindControl("valordebitotxt")).Text = tablaNC.Rows[indiceCop][7].ToString();
                    ((TextBox)e.Item.Cells[8].FindControl("valorcreditotxt")).Text = tablaNC.Rows[indiceCop][8].ToString();
                    ((TextBox)e.Item.Cells[9].FindControl("valorbasetxt")).Text = tablaNC.Rows[indiceCop][9].ToString();

                    Session.Remove("indexCopy");
                }
            }
		}

        public void gridNC_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            totalDNC.Text = ViewState["totalDNCNOEdit"].ToString();
            totalCNC.Text = ViewState["totalCNCNOEdit"].ToString();

            gridNC.EditItemIndex = -1;
            gridNC.DataSource = this.tablaNC;
            gridNC.DataBind();
            Session["tablaNC"] = this.tablaNC;
        }

        public void gridNC_Edit(Object sender, DataGridCommandEventArgs e)
        {
            if (tablaNC.Rows.Count > 0)
                gridNC.EditItemIndex = (int)e.Item.ItemIndex;

            ViewState["totalDNCNOEdit"] = totalDNC.Text;
            ViewState["totalCNCNOEdit"] = totalCNC.Text;

            totalDNC.Text = ((System.Convert.ToDouble(totalDNC.Text.Substring(1))) - (System.Convert.ToDouble(tablaNC.Rows[e.Item.DataSetIndex][7]))).ToString("C");
            totalCNC.Text = ((System.Convert.ToDouble(totalCNC.Text.Substring(1))) - (System.Convert.ToDouble(tablaNC.Rows[e.Item.DataSetIndex][8]))).ToString("C");
			
            gridNC.DataSource = this.tablaNC;
            gridNC.DataBind();
            Session["tablaNC"] = this.tablaNC;

        }

        public void gridNC_Update(Object sender, DataGridCommandEventArgs e)
        {
            tablaNC.Rows[gridNC.EditItemIndex][0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][2] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][3] = ((TextBox)e.Item.Cells[3].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][6] = ((TextBox)e.Item.Cells[6].Controls[1]).Text;
            tablaNC.Rows[gridNC.EditItemIndex][7] = double.Parse(((TextBox)e.Item.Cells[7].Controls[1]).Text, NumberStyles.Currency);
            tablaNC.Rows[gridNC.EditItemIndex][8] = double.Parse(((TextBox)e.Item.Cells[8].Controls[1]).Text, NumberStyles.Currency);
            tablaNC.Rows[gridNC.EditItemIndex][9] = double.Parse(((TextBox)e.Item.Cells[9].Controls[1]).Text, NumberStyles.Currency);

            totalDNC.Text = (Convert.ToDouble(totalDNC.Text.Substring(1)) + Convert.ToDouble(tablaNC.Rows[gridNC.EditItemIndex][7])).ToString("C");
            totalCNC.Text = (Convert.ToDouble(totalCNC.Text.Substring(1)) + Convert.ToDouble(tablaNC.Rows[gridNC.EditItemIndex][8])).ToString("C");

            gridNC.EditItemIndex = -1;
            gridNC.DataSource = this.tablaNC;
            gridNC.DataBind();
            Session["tablaNC"] = this.tablaNC;
        }

        protected void VincularExcel(object Sender, EventArgs e)
        {
            if (Session["dtExcel"] != null)
            {
                DataTable tablaNC_aux = (DataTable)Session["dtExcel"];
                tablaNC.Clear();
                totalDNC.Text = "$0";
                totalCNC.Text = "$0";
                btnVincularExcel.Visible = false;
                Control ctrlExcel = plcExcelImporter.Controls[0];
                ((Label)ctrlExcel.FindControl("lblResultado")).Visible = false;
                
                ArrayList arraySettingRow = new ArrayList();
                //bool verificador = true;
                DataRow newFila = tablaNC.NewRow();
                int lineaError = 0;
                for (int g = 0; g < tablaNC_aux.Rows.Count && newFila != null; g++)
                {
                    for (int h = 0; h < tablaNC_aux.Columns.Count; h++)
                    {
                        arraySettingRow.Add(tablaNC_aux.Rows[g][h]);
                    }
                    newFila = validarRegistroFila(arraySettingRow);

                    if (newFila != null)
                    {
                        //Recibe la fila del validarRegistroFila() y adiciona en tabla
                        totalDNC.Text = (Convert.ToDouble(totalDNC.Text.Substring(1)) + Convert.ToDouble(arraySettingRow[7])).ToString("C");
                        totalCNC.Text = (Convert.ToDouble(totalCNC.Text.Substring(1)) + Convert.ToDouble(arraySettingRow[8])).ToString("C");

                        tablaNC.Rows.Add(newFila);
                        gridNC.DataSource = tablaNC;
                        gridNC.DataBind();
                        Session["tablaNC"] = tablaNC;
                    }
                   
                    arraySettingRow.Clear();
                    lineaError = g+1;
                }

                Session.Remove("dtExcel");

                if (newFila == null)
                {
                    ((Label)plcExcelImporter.FindControl("lblInforme")).Text = "*Error presentado en la Fila No." + lineaError + " del archivo Excel. <br>Por favor revisar linea y recargar Excel nuevamente.";
                }
                else
                {
                    ((Label)plcExcelImporter.FindControl("lblInforme")).Text = "";
                }
                
            }
        }


        //funcion para validar el registro de una fila (nueva, modificada, cargada mediante excel)
        protected DataRow validarRegistroFila(ArrayList arraySettingRow)
        {
            //bool correcto = true;
            DataRow fila;
            if (!Validar_Datos2(arraySettingRow))
            {
                string verBase;
                double vb1 = 0, vporcBase, res;

                if (Session["tablaNC"] == null)
                    this.Cargar_tablaNC();
                fila = tablaNC.NewRow();
                fila[0] = arraySettingRow[0];
                fila[1] = arraySettingRow[1];
                fila[2] = arraySettingRow[2];
                fila[3] = arraySettingRow[3];
                fila[4] = arraySettingRow[4];
                fila[5] = arraySettingRow[5];
                fila[6] = arraySettingRow[6];

                //Si el valor introducido es debito
                if (arraySettingRow[7].ToString() != "0" && arraySettingRow[8].ToString() == "0")
                {
                    fila[7] = Convert.ToDouble(arraySettingRow[7]);
                    fila[8] = Convert.ToDouble("0");
                }
                //Si el valor introducido es credito
                else if (arraySettingRow[7].ToString() == "0" && arraySettingRow[8].ToString() != "0")
                {
                    fila[7] = System.Convert.ToDouble("0");
                    fila[8] = System.Convert.ToDouble(arraySettingRow[8]);
                }

                //Ahora verificamos los valores del valor base
                verBase = DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc='" + arraySettingRow[1] + "'");
                //Si la cuenta no soporta valor base y hay algun valor distinto de cero en ese campo
                if (verBase == "N" && arraySettingRow[9].ToString() != "0")
                {
                    Utils.MostrarAlerta(Response, "La cuenta afectada no soporta Valor Base por tanto se guardara un valor de 0");
                    fila[9] = System.Convert.ToDouble("0");
                }
                //Si la cuenta no soporta valor base y hay un valor de cero en ese campo
                else if (verBase == "N" && arraySettingRow[9].ToString() == "0")
                    fila[9] = System.Convert.ToDouble(arraySettingRow[9]);
                //Si la cuenta afectada soporta valor base
                else if (verBase == "B")
                {
                    //Convierto a double el valor base
                    vb1 = System.Convert.ToDouble(arraySettingRow[9]);

                    //Miro en la base de datos cual es el porcentaje de valor base
                    //lo convierto a double y lo divido por 100 para saber el verdadero valor
                    vporcBase = System.Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc='" + arraySettingRow[1] + "'"));
                    if (vporcBase == 0)
                    {
                        Utils.MostrarAlerta(Response, "No es posible agregar este concepto, debido a que el valor base definido para esta cuenta es 0.");
                        return null;
                    }

                    //Si el valor introducido es debito entonces se calcula el valor base con base en este
                    if (arraySettingRow[7].ToString() != "0")
                    {
                        res = System.Convert.ToDouble(arraySettingRow[7]) * 100 / vporcBase;
                        if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                            fila[9] = vb1;
                        else
                        {
                            Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                            fila[9] = res;
                        }
                    }
                    //Si el valor introducido es credito entonces se calcula el valor base con base en este
                    else if (arraySettingRow[8].ToString() != "0")
                    {
                        res = System.Convert.ToDouble(arraySettingRow[8]) * 100 / vporcBase;
                        if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                            fila[9] = vb1;
                        else
                        {
                            Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                            fila[9] = res;
                        }
                    }
                }

                if (arraySettingRow[1].ToString().Substring(0, 2) == "13" || arraySettingRow[1].ToString().Substring(0, 4) == "2205" || arraySettingRow[1].ToString().Substring(0, 4) == "2805")
                {
                    Utils.MostrarAlerta(Response, "La cuenta afectada solo debe manejar DOCUMENTOS CAUSADOS, esta imputación está errada; es su responsabilidad ..!");
                }

                //totalDNC.Text = (Convert.ToDouble(totalDNC.Text.Substring(1)) + Convert.ToDouble(arraySettingRow[7])).ToString("C");
                //totalCNC.Text = (Convert.ToDouble(totalCNC.Text.Substring(1)) + Convert.ToDouble(arraySettingRow[8])).ToString("C");

                //tablaNC.Rows.Add(fila);
                //gridNC.DataSource = tablaNC;
                //gridNC.DataBind();
                //Session["tablaNC"] = tablaNC;
            }
            else
                fila = null;

            return fila;
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
