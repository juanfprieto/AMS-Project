// created on 30/10/2003 at 9:31

using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
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
using Ajax;
using AMS.DB;
using AMS.Documentos;
using AMS.Contabilidad;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class AjustesInv : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataTable dtInserts;
		protected DataView dvInserts;
		protected DataRow dr;
		protected DataColumn dc;
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
		protected DataSet ds;
		private FormatosDocumentos formatoFactura=new FormatosDocumentos();
        protected static bool registroConsecutivo = false;
        protected DataSet dsComprobante = new DataSet();

        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
		
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjustesInv));
			//this.ClearChildViewState();
			LoadDataColumns();
			if(!IsPostBack)
			{
				Session.Clear();
				dgItems.EditItemIndex=-1;
				LoadDataTable();
				BindDatas(); 

                Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", "%", "AJ");
                Utils.FillDll(ddlAlmacen,  "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY palm_descripcion", false);
                Utils.FillDll(ddlCentro,   "SELECT pcen_codigo, pcen_nombre FROM pcentrocosto where TIMP_CODIGO <> 'N' ORDER BY pcen_nombre", false);
                Utils.FillDll(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor where (TVEND_CODIGO = 'VM' or TVEND_CODIGO = 'TT') and pven_vigencia='V' ORDER BY pven_nombre", false);
				
                IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				tbDate.Text = DateTime.Now.GetDateTimeFormats()[6];
				calDate.SelectedDate=DateTime.Now;
				if(Request.QueryString["prefA"]!=null && Request.QueryString["numA"]!=null)
				{
					Utils.MostrarAlerta(Response, "Se ha generado la el ajuste con prefijo "+Request.QueryString["prefA"]+" y número "+Request.QueryString["numA"]);
                    Imprimir.ImprimirRPT(Response, Request.QueryString["prefA"], Convert.ToInt32(Request.QueryString["numA"]), true);
				}
			}
			if(Session["dtInsertsInv"] == null)
				LoadDataTable();
			else
				dtInserts = (DataTable)Session["dtInsertsInv"];
			lbNumDoc.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlCodDoc.SelectedValue+"'");
		}

        [Ajax.AjaxMethod()]
        public string Cambio_nombre(string objeto, string textNom)
        {
            string nombre = "";
            nombre =DBFunctions.SingleData("SELECT MIT.mite_nombre AS NOMBRE FROM dbxschema.mitems MIT, dbxschema.plineaitem PLIN WHERE MIT.plin_codigo = PLIN.plin_codigo AND MIT.mite_codigo = '"+ objeto + "'");

            string var = nombre + "-" + textNom;

            return var;
        }
    	
		protected void DgItemsDataBound(object sender, DataGridItemEventArgs e)
		
        {
			if(e.Item.ItemType == ListItemType.Footer)
			{                                       
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]),"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("ondblclick", "MostrarRefs("+ ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + ");");
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onblur", "cargarNombre(this," + ((TextBox)e.Item.Cells[1].Controls[1]).ClientID.ToString() + ");");
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onkeyup","ItemMask("+((TextBox)e.Item.Cells[0].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[2].Controls[1]).ClientID+");");
				((DropDownList)e.Item.Cells[2].Controls[1]).Attributes.Add("onchange","ChangeLine("+((DropDownList)e.Item.Cells[2].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[0].Controls[1]).ClientID+");");
            
          } 
		}
    	
		protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
		{	//Agreaga, elimina Item //Reiniciar
			if(((Button)e.CommandSource).CommandName == "ClearRows")
				dtInserts.Rows.Clear();
			//AgregarItem			
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValues(e))
			{
				//Revisamos que los valores de documento de referencia sean validos
				if(this.ddlCodDoc.SelectedItem.Text != "Seleccione..." && this.txObservaciones.Text != "" && this.tbPrefDocRef.Text != "" && DatasToControls.ValidarInt(this.tbPrefNumRef.Text))
				{
                    if(((TextBox)e.Item.Cells[4].Controls[1]).Text.Trim() == "")
                    {
                        Utils.MostrarAlerta(Response, "El valor de la unidad no puede ir vacio, revise por favor..");
                        return;
                    }
                    ds = new DataSet();
					string Icod = "";
					string IcodEdit = ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim();
					//Aqui revisamos que el codigo del item es valido
					bool estCod = Referencias.Guardar(IcodEdit,ref Icod,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]);
					double cantN=0 ,valN=0;	//Valores y cantidades
					bool valsN = (((TextBox)e.Item.Cells[3].Controls[1]).Text.Length>0);//Se dio cantidad?
					string AnoAct = DBFunctions.SingleData("SELECT pano_ano from cinventario");//Año actual contabilidad
					if(valsN)
					{	//Se dieron valores:
						try
						{
							cantN=Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
						}
						catch
						{
							valsN=false;
						};
						try
						{
							valN=Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);
						}
						catch
						{};
					}
					if(estCod)
					{
						//Aqui revisamos si es un codigo de sustitución o no
						string icodTmp = Icod;
						if(!Referencias.RevisionSustitucion(ref Icod,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[0]))
						{
							Response.Write("<script language='javascript'>alert('El codigo "+IcodEdit+" no se encuentra registrado.\\nRevise Por Favor.!');</script>");
							return;
						}
						string icodTmp2 = "";
						Referencias.Editar(Icod,ref icodTmp2,(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]);
						if(icodTmp != Icod)
							Response.Write("<script language='javascript'>alert('El codigo "+IcodEdit+" ha sido sustituido.El codigo actual es "+icodTmp2+"!');</script>");
						//Se dio codigo de item
						for(int i=0;i<dtInserts.Rows.Count;i++)//Revisar que no este ya el item en la lista
						{
							if(icodTmp2 == dtInserts.Rows[i][0].ToString().Trim())
							{
								BindDatas();
								Response.Write("<script language='javascript'>alert('El item ya esta en la lista, intente actualizarlo!');</script>");
								return;
							}
						}	
						//Crear msaldoitem almacen si no existe
						if(!DBFunctions.RecordExist("SELECT * FROM msaldoitemalmacen WHERE mite_codigo = '" + Icod + "' AND PALM_ALMACEN='"+ddlAlmacen.SelectedItem.Value+"' AND PANO_ANO="+AnoAct))
						{
							ArrayList sqlL=new ArrayList();
							sqlL.Add("insert into msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_cantpendiente,msal_canttransito,msal_cantinveinic) values ('"+Icod+"','"+ddlAlmacen.SelectedItem.Value+"',"+AnoAct+",0,0,0,0,0,0,0)");
							DBFunctions.Transaction(sqlL);
							sqlL.Clear();
						}
						//Traer datos del item
						DBFunctions.Request(ds, IncludeSchema.NO,"select mitems.mite_codigo, mitems.mite_nombre, msaldoitemalmacen.msal_cantasig, msaldoitemalmacen.msal_costprom, msaldoitemalmacen.msal_cantactual,mitems.plin_codigo from mitems,msaldoitemalmacen where mitems.mite_codigo=msaldoitemalmacen.mite_codigo and mitems.mite_codigo='" +Icod+"' and msaldoitemalmacen.PALM_ALMACEN='"+ddlAlmacen.SelectedItem.Value+"' AND msaldoitemalmacen.PANO_ANO="+AnoAct);
						//														0					1						2								3									4							 5	
						if(ds.Tables[0].Rows.Count>0)
						{
							double cosP = Convert.ToDouble(ds.Tables[0].Rows[0][3]);//Costo promedio
							dr = dtInserts.NewRow();
							string codList = "";
							Referencias.Editar(ds.Tables[0].Rows[0][0].ToString(),ref codList,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+ds.Tables[0].Rows[0][5].ToString()+"'"));
							dr[0] = codList;	//Cod
							dr[1] = ds.Tables[0].Rows[0][1];	//Nombre
							if(cantN<0)	//Validar cantidad
								if(Math.Abs(cantN)<=Convert.ToDouble(ds.Tables[0].Rows[0][4]))//Si la cantidad es <0 abs(cantidad) debe ser mayor que la cantidad actual /****Rta:Si la cantidad asignada para el ajuste (digitada en la grilla) es mayor que la cantidad actual del almacen le asigna 0, sino asigna la digitada
									dr[2]=cantN;//Cantidad valida
								else
									dr[2]=0;	//No hay existencias
							else
								dr[2]=cantN; //No entiendo si es mayor que 0 cero, lo asigna de una vez
							//if(valsN && (valN>0))//ASIGNACION COSTO PROMEDIO Si es valido el valor digitado en la grilla celda 2 (cantidad) Y el valor unitario es mayor que cero 
							if(valsN && (cantN>0))
								dr[3]=valN;
							else
							{
								dr[3]=cosP;	//costo promedio
								Response.Write("<script language:javascript>alert('El ajuste es negativo por tanto se tomara el costo promedio como valor unidad');</script>");
							}
							if(Convert.ToDouble(dr[2])==0)
							{
								if(cantN==0)
									Response.Write("<script language='javascript'>alert('La cantidad no puede ser 0!');</script>");
								else 
									Response.Write("<script language='javascript'>alert('No hay suficiente existencia!');</script>");
								return;
							}
							dr[4]=ds.Tables[0].Rows[0][4];	//cantactual
							dr[5]=Convert.ToDouble(dr[2])*Convert.ToDouble(dr[3]);	//total
							dr[6]=ds.Tables[0].Rows[0][5].ToString();
							dtInserts.Rows.Add(dr);
						}
					}
					else
						Response.Write("<script language='javascript'>alert('El codigo "+IcodEdit+" es invalido para la linea de bodega "+((DropDownList)e.Item.Cells[2].Controls[1]).SelectedItem.Text+".\\nRevise Por Favor!');</script>");
				}
				else
					Response.Write("<script language='javascript'>alert('Error en datos: El Cod. Documento, la Observación o el Prefijo Documento Referencia estan vacios o el Número Documento Referencia no es numérico. Revise Por Favor');</script>");
			}
			//EliminarItem
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
				try{dtInserts.Rows.Remove(dtInserts.Rows[e.Item.ItemIndex]);}
				catch{};
			BindDatas();
		}
    	
		protected void ChangeDate(Object Sender, EventArgs E)
		{
			if(dtInserts.Rows.Count==0)
				tbDate.Text = calDate.SelectedDate.GetDateTimeFormats()[6];
		}

		protected void ChangeDocument(Object Sender, EventArgs E)
		{
			lbNumDoc.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo = '" + ddlCodDoc.SelectedValue + "'");
		}
		
		public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgItems.EditItemIndex=-1;
			BindDatas();
		}
		
		public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtInserts.Rows.Count>0)
				dgItems.EditItemIndex=(int)e.Item.ItemIndex;
			BindDatas();
		}
		
		public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
		{	//Este selecciona la fila
			if(dtInserts.Rows.Count==0) 
				return;
			int cant=0;
			try{cant=Convert.ToInt32(((TextBox)e.Item.Cells[3].Controls[1]).Text);}
			catch{cant=Convert.ToInt32(dtInserts.Rows[dgItems.EditItemIndex][2]);}
			double pr=0;
			try{pr=Convert.ToDouble(((TextBox)e.Item.Cells[4].Controls[1]).Text);}
			catch{pr=Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][3]);};
			if(cant<=0)
			{
				if(!(Math.Abs(cant)<=Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][4])))
				{//Si la cantidad es <0 abs(cantidad) debe ser mayor que la cantidad actual
					if(cant==0)
						Response.Write("<script language='javascript'>alert('La cantidad no puede ser 0!');</script>");
					else 
						Response.Write("<script language='javascript'>alert('No hay suficiente existencia!');</script>");
					cant=Convert.ToInt32(dtInserts.Rows[dgItems.EditItemIndex][2]);//no cambiar, no alcanza
				}
				pr=Convert.ToInt32(dtInserts.Rows[dgItems.EditItemIndex][3]);
			}
			if(pr<=0)
				pr=Convert.ToInt32(dtInserts.Rows[dgItems.EditItemIndex][3]);
			double tot=cant*pr;
			dtInserts.Rows[dgItems.EditItemIndex][2]=cant;
			dtInserts.Rows[dgItems.EditItemIndex][3]=pr;
			dtInserts.Rows[dgItems.EditItemIndex][5]=tot;
			dgItems.EditItemIndex=-1;
			BindDatas();
		}    

		protected void NewAjust(Object Sender, EventArgs E)
		{
            //int ultimoDoc = Convert.ToInt32(lbNumDoc.Text);
            int ultimoDoc = Convert.ToInt32(lbNumDoc.Text) + 1;
            if(dtInserts.Rows.Count>0)
            {
                //    int ultiDocu = Convert.ToInt32(DBFunctions.SingleData("SELECT PDOC_ULTIDOCU FROM PDOCUMENTO WHERE PDOC_CODIGO = '" + ddlCodDoc.SelectedValue + "'"));
                //    if(Convert.ToInt32(lbNumDoc.Text) == ultiDocu)
                //    {
                //        //Utils.MostrarAlerta(Response, "el prefijo" + lbNumDoc.Text + "aumentará en 1 debido a que ya existe!");
                //        ultimoDoc += 1;
                //        lbNumDoc.Text = Convert.ToString(ultimoDoc);
                //    }
            
                string msjError = lbInfo.Text;

                bool procesoRealizado = RealizarAjusteInventario(
                    ddlCodDoc.SelectedValue,
                    Convert.ToInt32(lbNumDoc.Text),
                    tbPrefDocRef.Text,
                    Convert.ToInt32(this.tbPrefNumRef.Text.Trim()),
                    ddlAlmacen.SelectedValue,
                    calDate.SelectedDate,
                    ddlVendedor.SelectedValue,
                    ddlCentro.SelectedValue,
                    txObservaciones.Text,
                    dtInserts,
                    ref msjError
                    );

                if (procesoRealizado)
				{
					string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    Session.Clear();
                    if (registroConsecutivo) lbNumDoc.Text = Convert.ToString(ultimoDoc);
					Response.Redirect(""+indexPage+"?process=Inventarios.AjustesInv&path=Ajustes al Inventario&prefA="+ddlCodDoc.SelectedValue+"&numA="+lbNumDoc.Text+"");
				}
				else
                    lbInfo.Text += msjError;
			}
			else
				Utils.MostrarAlerta(Response, "'Primero debe agregar items a la lista!");
		}
		#endregion

		#region Metodos
		protected void BindDatas()
		{
			int n,m;
			dgItems.EnableViewState=true;
			dvInserts = new DataView(dtInserts);
			dgItems.DataSource = dtInserts;
			dgItems.DataBind();
			for(n=0;n<dgItems.Items.Count;n++)
				for(m=3;m<=5;m++)
					dgItems.Items[n].Cells[m].HorizontalAlign = HorizontalAlign.Right;
			DatasToControls.JustificacionGrilla(dgItems,dtInserts);
			double t=0;
			int ni=0;
			if(dtInserts.Rows.Count>0)
				for(n=0;n<dtInserts.Rows.Count;n++)
				{
					t+=Convert.ToDouble(dtInserts.Rows[n][5]);//total costos
					ni+=Convert.ToInt32(dtInserts.Rows[n][2]);//total de cantidad asignada
				}
			txtTotal.Text=t.ToString("C");
			txtNumItem.Text=dtInserts.Rows.Count.ToString();
			txtNumUnid.Text=ni.ToString();
			if(dtInserts.Rows.Count==0)
			{
				dgItems.EditItemIndex=-1;
				ddlAlmacen.Enabled= ddlCentro.Enabled = ddlVendedor.Enabled = ddlCodDoc.Enabled = tbDate.Enabled = tbPrefDocRef.Enabled = tbPrefNumRef.Enabled = true;
			}
			else
				ddlAlmacen.Enabled = ddlCentro.Enabled = ddlVendedor.Enabled = ddlCodDoc.Enabled = tbDate.Enabled = tbPrefDocRef.Enabled = tbPrefNumRef.Enabled = false;
		}

        protected void btnCargar_Click(object sender, System.EventArgs e)
        {
            string errorItem = "";
            string errorSaldo = "";
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
                        flArchivoExcel.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileName);
                        ExcelFunctions exc = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileName);
                        bool leiArchivo = false;
                        try
                        {
                            exc.Request(dsComprobante, IncludeSchema.NO, "SELECT * FROM TABLA");
                            if (dsComprobante.Tables[0].Rows.Count == 0)
                            {
                                Utils.MostrarAlerta(Response, "No se encontró ninguna tabla, verifique que haya seguido adecuadamente los pasos");
                                return;
                            }
                            else
                            {
                                for (int i = 0; i < dsComprobante.Tables[0].Columns.Count; i++)
                                {
                                        dsComprobante.Tables[0].Columns[i].ColumnName = dsComprobante.Tables[0].Rows[0][i].ToString();
                                }
                                DataTable dt = dsComprobante.Tables[0];
                                dsComprobante.Tables[0].Rows[0].Delete();
                                dsComprobante.Tables[0].Rows[0].AcceptChanges();//es como un flush, cierra la edición por lo que reajusta la tabla
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
                            if (dsComprobante.Tables[0].Rows.Count == 0)
                            {
                                Utils.MostrarAlerta(Response, "No se encontro ningún registro en el archivo de Excel");
                                return;
                            }
                            else
                            {
                                dtInserts.Clear();
                                for (int i = 0; i < dsComprobante.Tables[0].Rows.Count; i++)
                                {
                                    DataRow dr = dtInserts.NewRow();
                                    dr["mite_codigo"] = dsComprobante.Tables[0].Rows[i][0].ToString();
                                    dr["mite_nombre"] = dsComprobante.Tables[0].Rows[i][1].ToString();
                                    dr["mite_linea"] = dsComprobante.Tables[0].Rows[i][2].ToString();
                                    dr["msal_cantasig"] = dsComprobante.Tables[0].Rows[i][3].ToString();
                                    dr["msal_costprom"] = dsComprobante.Tables[0].Rows[i][4].ToString();
                                    dr["msal_tot"] = ((Double)dr["msal_cantasig"] * (Double)dr["msal_costprom"]);
                                    
                                    if (!DBFunctions.RecordExist("Select mite_codigo from mitems where mite_codigo = '" + dsComprobante.Tables[0].Rows[i][0].ToString() + "'"))
                                    {
                                        errorItem += dsComprobante.Tables[0].Rows[i][0].ToString() + "\\n";
                                    }
                                    else if (DBFunctions.RecordExist("Select msal_cantactual from msaldoitems ms, cinventario ci where mite_codigo = '" + dsComprobante.Tables[0].Rows[i][0].ToString() + "' and ms.pano_ano = ci.pano_ano and ms.msal_cantactual + (" + Convert.ToDouble(dsComprobante.Tables[0].Rows[i][3].ToString()) + ") < 0"))
                                         {
                                                errorSaldo += dsComprobante.Tables[0].Rows[i][0].ToString() + "\\n";
                                         }
                                         else if (DBFunctions.RecordExist("Select msal_cantactual from msaldoitemsALMACEN ms, cinventario ci where mite_codigo = '" + dsComprobante.Tables[0].Rows[i][0].ToString() + "' and ms.pano_ano = ci.pano_ano and ms.msal_cantactual + (" + Convert.ToDouble(dsComprobante.Tables[0].Rows[i][3].ToString()) + ") < 0 AND MS.PALM_ALMACEN = '" + ddlAlmacen.SelectedValue + "' "))
                                         {
                                                errorSaldo += dsComprobante.Tables[0].Rows[i][0].ToString() + "\\n";
                                         }
                                   
                                    dtInserts.Rows.Add(dr);
                                }
                            }
                            //Diference();
                            BindDatas();
                            if (errorItem != "" || errorSaldo != "")
                            {
                                btnAjus.Enabled = false;
                                if(errorItem != "")
                                    errorItem = errorItem + " NO estan registrados en el catálogo de partes \\n";
                                if (errorSaldo != "")
                                    errorSaldo = errorSaldo + " Con esta cantidad a ajustar quedan con saldo NEGATIVO y NO es permitido \\n";
                                errorItem = errorItem + " " + errorSaldo +"   Por favor, corrija las inconsistencias y ejecute nuevamente el proceso desde el Menú";
                                Utils.MostrarAlerta(Response, errorItem);
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.RegisterClientScriptBlock("status", ex.ToString());
                }
            }
        }

        protected void LoadDataColumns()
		{
			lbFields.Add("mite_codigo");	types.Add(typeof(string));//0
			lbFields.Add("mite_nombre");	types.Add(typeof(string));//1
			lbFields.Add("msal_cantasig");	types.Add(typeof(double));//2
			lbFields.Add("msal_costprom");	types.Add(typeof(double));//3
			lbFields.Add("msal_cantactual");types.Add(typeof(double));//4
			lbFields.Add("msal_tot");		types.Add(typeof(double));//5
			lbFields.Add("mite_linea");		types.Add(typeof(string));//6
		}

		protected void LoadDataTable()
		{
			int i;
			dtInserts = new DataTable();
			for(i=0; i<lbFields.Count; i++)
				dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
			Session["dtInsertsInv"] = dtInserts;
		}

		protected bool CheckValues(DataGridCommandEventArgs e)
		{
			bool check=true;
			if(((TextBox)e.Item.Cells[0].Controls[1]).Text == "") 
				check=false;
			return check;
		}

        public static bool RealizarAjusteInventario(string prefijo, int numero, string prefReferencia, int numReferencia, 
            string almacen, DateTime fechaDocumento, string vendedor, string centroCosto, string observaciones, DataTable dtItems, ref string msjError)
        {
            double cantidad = 0;
            double valorUnitario = 0;
            double costoPromedio = 0;
            double costoPromedioAlmacen = 0;
            double porcentajeIVA = 0;
            double porcentajeDescuento = 0;
            double cantidadDevolucion = 0;
            double costoPromedioHistorico = 0;
            double costoPromedioHistoricoAlmacen = 0;
            double valorPublico = 0;
            double inventarioInicial = 0;
            double inventarioInicialAlmacen = 0;
            int tipoMovimiento = 50;
            string nit = DBFunctions.SingleData("SELECT mnit_nit from cempresa");
            string cargo = DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='" + vendedor + "'");
            string año = DBFunctions.SingleData("SELECT pano_ano FROM cinventario");


            
            
            Movimiento Mov = new Movimiento(
                prefijo,
                (uint) numero,
                prefReferencia,
                (uint) numReferencia, 
                tipoMovimiento, 
                nit,
                almacen,
                fechaDocumento,
                vendedor, 
                cargo,
                centroCosto, 
                "N","");

            Mov.Observaciones = observaciones;

            for (int i = 0; i < dtItems.Rows.Count; i++)
            {
                string codItem = "";
                string lineaItem = DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtItems.Rows[i]["mite_linea"].ToString() + "'");
                Referencias.Guardar(dtItems.Rows[i]["mite_codigo"].ToString().Trim(), ref codItem, lineaItem);
                cantidad = Convert.ToDouble(dtItems.Rows[i]["msal_cantasig"]);//cantidfad facturada
                valorUnitario = Convert.ToDouble(dtItems.Rows[i]["msal_costprom"]);//valor unidad

                string sqlSaldos = String.Format(
                    "SELECT msal_costprom, \n" +
                    "       msal_costpromhist, \n" +
                    "       msal_cantactual \n" +
                    "FROM msaldoitem \n" +
                    "WHERE pano_ano = {0} \n" +
                    "AND   mite_codigo = '{1}'"
                    , año
                    , codItem);

                string sqlSaldosAlmacen = String.Format(
                    "SELECT msal_costprom, \n" +
                    "       msal_costpromhist, \n" +
                    "       msal_cantactual \n" +
                    "FROM msaldoitemalmacen \n" +
                    "WHERE pano_ano = {0} \n" +
                    "AND   palm_almacen = '{1}' \n" +
                    "AND   mite_codigo = '{2}'"
                    , año
                    , almacen
                    , codItem);

                ArrayList arrSaldos = DBFunctions.RequestAsCollection(sqlSaldos);
                ArrayList arrSaldosAlmacen = DBFunctions.RequestAsCollection(sqlSaldosAlmacen);

                if (arrSaldos.Count > 0)
                {
                    Hashtable hashSaldos = (Hashtable)arrSaldos[0];
                    costoPromedio = Convert.ToDouble(hashSaldos["MSAL_COSTPROM"]);
                    costoPromedioHistorico = Convert.ToDouble(hashSaldos["MSAL_COSTPROMHIST"]);
                    inventarioInicial = Convert.ToDouble(hashSaldos["MSAL_CANTACTUAL"]);
                }
                else
                {
                    costoPromedio = 0;
                    costoPromedioHistorico = 0;
                    inventarioInicial = 0;
                }

                if (arrSaldosAlmacen.Count > 0)
                {
                    Hashtable hashSaldosAlmacen = (Hashtable)arrSaldosAlmacen[0];
                    costoPromedioAlmacen = Convert.ToDouble(hashSaldosAlmacen["MSAL_COSTPROM"]);
                    costoPromedioHistoricoAlmacen = Convert.ToDouble(hashSaldosAlmacen["MSAL_COSTPROMHIST"]);
                    inventarioInicialAlmacen = Convert.ToDouble(hashSaldosAlmacen["MSAL_CANTACTUAL"]);
                }
                else
                {
                    costoPromedioAlmacen = 0;
                    costoPromedioHistoricoAlmacen = 0;
                    inventarioInicialAlmacen = 0;
                }

                valorPublico = 0;
                porcentajeIVA = 0;
                porcentajeDescuento = 0; 
                cantidadDevolucion = 0;

                Mov.InsertaFila(codItem, cantidad, valorUnitario, costoPromedio, costoPromedioAlmacen, 
                    porcentajeIVA, porcentajeDescuento, cantidadDevolucion, costoPromedioHistorico, costoPromedioHistoricoAlmacen, 
                    valorPublico, inventarioInicial, inventarioInicialAlmacen, 0,"","");
            }
            if (Mov.RealizarMov(true))
            {
                int ultiDocu = Convert.ToInt32(DBFunctions.SingleData("SELECT PDOC_ULTIDOCU FROM PDOCUMENTO WHERE PDOC_CODIGO = '" + prefijo + "'"));
                if (ultiDocu != numero)
                {
                    DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu=" + numero + " WHERE pdoc_codigo='" + prefijo + "'");
                }
                else if (ultiDocu == numero)
                {
                    numero += 1;
                    DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu=" + numero + " WHERE pdoc_codigo='" + prefijo + "'");

                    registroConsecutivo = true;
                }


                

                ProceHecEco contaOnline = new ProceHecEco();
                contaOnline.contabilizarOnline(prefijo, numero, DateTime.Now, "");
                
                return true;
            }
            else
                msjError += Mov.ProcessMsg;

            return false;
        }

		#endregion

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
