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
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;
using Ajax;
using AMS.Tools;
using AMS.Contabilidad;


namespace AMS.Finanzas.Tesoreria
{
	public partial class CruceDocumentos : System.Web.UI.UserControl
	{
		#region Atributos

		protected DataTable tablaDocumentos,tablaPagar,tablaDocDis;
		protected Button btnAceptar,aceptar;
		protected string valorPrefijo,valorNumero,numero;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo=new FormatosDocumentos();
        ProceHecEco contaOnline = new ProceHecEco();


		#endregion

		#region Eventos

		protected void Page_Load(object Sender,EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(CruceDocumentos));
			if(!IsPostBack)
			{
				if(Request["prefC"]!=null && Request["numC"]!=null)
				{
                    Utils.MostrarAlerta(Response, "Se ha creado el cruce de documentos con prefijo " + Request["prefC"] + " y número " + Request["numC"] + "");
                    try
					{
						formatoRecibo.Prefijo=Request["prefC"];
						formatoRecibo.Numero=Convert.ToInt32(Request["numC"]);
						formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request["prefC"]+"'");
						if(formatoRecibo.Codigo!=string.Empty)
						{
							if(formatoRecibo.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
                            // Contabilizacion On Line
                            //string fecha = DBFunctions.SingleData("SELECT mcru_fecha FROM mcrucedocumento WHERE pdoc_codigo='" + formatoRecibo.Prefijo.ToString() + "' and mcru_numero = " + Convert.ToInt32(formatoRecibo.Numero.ToString() + " "));
           
                            //contaOnline.contabilizarOnline(formatoRecibo.Prefijo.ToString(), Convert.ToInt32(formatoRecibo.Numero.ToString()), Convert.ToDateTime(fecha.ToString()) , "");
						}
					}
					catch
					{
						lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
					}
				}
				Session.Clear();
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				totalCli.Text=totalPro.Text=totalCruce.Text="$0.00";
				DatasToControls bind=new DatasToControls();
                //bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CX' and tvig_vigencia = 'V' order by pdoc_codigo");
                Utils.llenarPrefijos(Response, ref ddlPrefijo , "%", "%", "CX");
				if(ddlPrefijo.Items.Count!=0) 
				{
					ListItem item = new ListItem("--Seleccione un Prefijo--", "pre"); 
					ddlPrefijo.Items.Insert(0,item);
				}
				ddlPrefijo.SelectedValue="pre"; 
				lbNumero.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");
                datCli.Enabled = false;
            }
			else
			{
				if(Session["tablaDocumentos"]!=null)
					tablaDocumentos=(DataTable)Session["tablaDocumentos"];
				if(Session["tablaPagar"]!=null)
					tablaPagar=(DataTable)Session["tablaPagar"];
				if(Session["tablaDocDis"]!=null)
					tablaDocDis=(DataTable)Session["tablaDocDis"];
				valorPrefijo =Request.Form[ddlPrefFac.UniqueID];
				valorNumero  =Request.Form[ddlNumFac.UniqueID];
				lbNumero.Text=Request.Form[hdnnum.UniqueID];
				datClia.Text=Request.Form[hdnnom.UniqueID];
			}
		}

		protected void btnCargar_Click(object sender, System.EventArgs e)
		{
            if (!Tools.General.validarCierreFinanzas(txtFecha.Text, "C"))
            {
                Utils.MostrarAlerta(Response, "La fecha del documento no corresponde a la vigencia del sistema de cartera. Por favor revise.");
                return;
            }
            if (rbCliente.Checked || rbProveedor.Checked) 
			{
				if (ddlPrefijo.SelectedValue=="pre") 
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de prefijo");
				else 
				{
					Llenar_gridDocumentos();
					EnlazarDropDownListPref();
				}
			}
			else
            Utils.MostrarAlerta(Response, "Debe escoger un tipo de cruce");
		}

		protected void gridDocumentos_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				((Button)e.Item.Cells[7].FindControl("addAll")).Attributes.Add("onClick","return confirm('Esta seguro de cargar todos los documentos?');");
			}
		}

		protected void gridPagar_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				((Button)e.Item.Cells[0].FindControl("remAll")).Attributes.Add("onClick","return confirm('Esta seguro de remover todos los documentos?');");
			}
		}

		private void gridDocumentos_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(e.CommandName=="adicionarFilas")
			{
				if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else if(((TextBox)e.Item.Cells[6].Controls[1]).Text=="0")
                    Utils.MostrarAlerta(Response, "¡No puede abonar $0 a una factura!");
				else
				{
					//Si el valor a abonar (escrito) es mayor que el saldo
					double valorAb=Math.Round(Convert.ToDouble(tablaDocumentos.Rows[e.Item.DataSetIndex][5]),2);
					if((valorAb)<(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text)))
                        Utils.MostrarAlerta(Response, "El valor a Abonar es mayor que el Saldo Total. Revise sus Datos");
						//Sino que haga todo lo que tiene que hacer
					else
					{
						Cruzar_Documentos(e,tablaDocumentos);
						tablaDocumentos.Rows[e.Item.DataSetIndex].Delete();
						gridDocumentos.DataSource=tablaDocumentos;
						gridDocumentos.CurrentPageIndex=0;
						gridDocumentos.DataBind();
						Session["tablaDocumentos"]=tablaDocumentos;
					}
				}
			}
			else if(e.CommandName=="addall")
			{
				if(tablaDocumentos.Rows.Count!=0)
				{
					for(int i=0;i<tablaDocumentos.Rows.Count;i++)
					{
						if(Session["tablaPagar"]==null)
							this.Cargar_tablaPagar();
						fila=tablaPagar.NewRow();
						fila[0]=tablaDocumentos.Rows[i][0].ToString();//Tipo
						fila[1]=tablaDocumentos.Rows[i][1].ToString();//Nit
						fila[2]=tablaDocumentos.Rows[i][2].ToString();//Prefijo
						fila[3]=tablaDocumentos.Rows[i][3].ToString();//Número Documento
						fila[4]=Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString());//Saldo
						fila[5]=Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString());//Valor a Abonar
						fila[6]=tablaDocumentos.Rows[i][6].ToString();//Tipo F, A, N
						tablaPagar.Rows.Add(fila);
						gridPagar.DataSource=tablaPagar;
						gridPagar.ShowFooter=true;
						gridPagar.DataBind();
						Session["tablaPagar"]=tablaPagar;
						if(rbCliente.Checked)
						{
							//Si está en mfacturacliente y es una factura o un abono, sumo a totalcli
							if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && (tablaDocumentos.Rows[i][6].ToString()=="FC"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, resto a total cli
							else if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && tablaDocumentos.Rows[i][6].ToString()=="NC")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es un factura o un abono, resto a totalpro
							else if(tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && (tablaDocumentos.Rows[i][6].ToString()=="FP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, suma a totalpro
							else if((tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && tablaDocumentos.Rows[i][6].ToString()=="NP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
							totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
						}
						else if(rbProveedor.Checked)
						{
							//Si está en mfacturacliente y es una factura o un abono, resto a totalcli
							if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && (tablaDocumentos.Rows[i][6].ToString()=="FC"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, sumo a total cli
							else if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && tablaDocumentos.Rows[i][6].ToString()=="NC")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es un factura o un abono, sumo a totalpro
							else if(tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && (tablaDocumentos.Rows[i][6].ToString()=="FP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, resto a totalpro
							else if((tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && tablaDocumentos.Rows[i][6].ToString()=="NP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
							totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
						}
					}
					tablaDocumentos.Clear();
					gridDocumentos.ShowFooter=false;
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
				}
				else
					gridDocumentos.ShowFooter=false;
			}
		}

		protected void gridDocumentos_PageIndexChange(object Sender,DataGridPageChangedEventArgs e)
		{
			gridDocumentos.CurrentPageIndex=e.NewPageIndex;
			gridDocumentos.DataSource=tablaDocumentos;
			gridDocumentos.DataBind();
			Session["tablaDocumentos"]=tablaDocumentos;
		}
		
		protected void gridPagar_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="removerFilas")
			{
				if((tablaPagar.Rows[e.Item.DataSetIndex][1].ToString())!=datCli.Text && (tablaPagar.Rows[e.Item.DataSetIndex][1].ToString())!=datCli.Text)
					this.Devolver_Documentos(e,tablaDocDis,gridDocDis,"tablaDocDis");
				else
					this.Devolver_Documentos(e,tablaDocumentos,gridDocumentos,"tablaDocumentos");
			}
			else if(((Button)e.CommandSource).CommandName=="remAll")
			{
				if(tablaPagar.Rows.Count!=0)
				{
					for(int i=0;i<tablaPagar.Rows.Count;i++)
					{
						fila=tablaDocumentos.NewRow();
						fila[0]=tablaPagar.Rows[i][0].ToString();
						fila[1]=tablaPagar.Rows[i][1].ToString();
						fila[2]=tablaPagar.Rows[i][2].ToString();
						fila[3]=tablaPagar.Rows[i][3].ToString();
						if(tablaPagar.Rows[i][0].ToString()=="Cliente")
							fila[4]=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM mfacturacliente WHERE pdoc_codigo='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaPagar.Rows[i][3].ToString()+""));
						else if(tablaPagar.Rows[i][0].ToString()=="Proveedor")
							fila[4]=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM mfacturaproveedor WHERE pdoc_codiordepago='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+tablaPagar.Rows[i][3].ToString()+""));
						fila[5]=Convert.ToDouble(tablaPagar.Rows[i][4].ToString());
						fila[6]=tablaPagar.Rows[i][6].ToString();
						tablaDocumentos.Rows.Add(fila);
						gridDocumentos.DataSource=tablaDocumentos;
						gridDocumentos.ShowFooter=true;
						gridDocumentos.DataBind();
						Session["tablaDocumentos"]=tablaDocumentos;
						if(rbCliente.Checked)
						{
							//Si está en mafcturacliente y es una factura o una nota, resto de totalcli
							if(tablaPagar.Rows[i][0].ToString()=="Cliente" && (tablaPagar.Rows[i][6].ToString()=="FC"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, sumo a totalcli
							else if(tablaPagar.Rows[i][0].ToString()=="Cliente" && tablaPagar.Rows[i][6].ToString()=="NC")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una factura o un abono, sumo a totalpro
							else if(tablaPagar.Rows[i][0].ToString()=="Proveedor" && (tablaPagar.Rows[i][6].ToString()=="FP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, resto a totalpro
							else if((tablaPagar.Rows[i][0].ToString()=="Proveedor" && tablaPagar.Rows[i][6].ToString()=="NP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
							totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
						}
						else if(rbProveedor.Checked)
						{
							//Si está en mafcturacliente y es una factura o una nota, sumo a totalcli
							if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && (tablaDocumentos.Rows[i][6].ToString()=="FC"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, resto de totalcli
							else if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && tablaDocumentos.Rows[i][6].ToString()=="NC")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una factura o un abono, resto de totalpro
							else if(tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && (tablaDocumentos.Rows[i][6].ToString()=="FP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, sumo a totalpro
							else if((tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && tablaDocumentos.Rows[i][6].ToString()=="NP"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
							totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
						}
					}
					tablaPagar.Clear();
					gridPagar.DataSource=tablaPagar;
					gridPagar.ShowFooter=false;
					gridPagar.DataBind();
					Session["tablaPagar"]=tablaPagar;
				}
				else
					gridPagar.ShowFooter=false;
			}
		}
		
		protected void aceptarFactura_Click(object Sender,EventArgs e)
		{
			DataRow fila;
			DataSet ds=new DataSet();
			if(valorNumero!=string.Empty)
			{
				if(rb1.Checked)
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MF.mnit_nit,MF.pdoc_codigo,      MF.mfac_numedocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),    TDOC_TIPODOCU FROM dbxschema.mfacturacliente MF, dbxschema.PDOCUMENTO PD   WHERE MF.PDOC_CODIGO = PD.PDOC_CODIGO AND MF.pdoc_codigo='"+valorPrefijo+"' AND mfac_numedocu="+valorNumero+"");
				else
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MF.mnit_nit,MF.pdoc_codiordepago,MF.mfac_numeordepago,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),TDOC_TIPODOCU FROM dbxschema.mfacturaproveedor MF, dbxschema.PDOCUMENTO PD WHERE MF.PDOC_CODIORDEPAGO = PD.PDOC_CODIGO AND MF.pdoc_codiordepago='"+valorPrefijo+"' AND mfac_numeordepago="+valorNumero+"");
				if(ds.Tables[0].Rows.Count!=0)
				{
					if(Session["tablaDocDis"]==null)
						this.Cargar_tablaDocDis();
					fila=tablaDocDis.NewRow();
					if(rb1.Checked)
						fila[0]="Cliente";
					else
						fila[0]="Proveedor";
					fila[1]=ds.Tables[0].Rows[0][0].ToString();
					fila[2]=ds.Tables[0].Rows[0][1].ToString();
					fila[3]=ds.Tables[0].Rows[0][2].ToString();
					fila[4]=Convert.ToDouble(ds.Tables[0].Rows[0][3]);
					fila[5]=Convert.ToDouble(ds.Tables[0].Rows[0][4]);
					fila[6]=ds.Tables[0].Rows[0][5].ToString();
					tablaDocDis.Rows.Add(fila);
					gridDocDis.DataSource=tablaDocDis;
					gridDocDis.DataBind();
					Session["tablaDocDis"]=tablaDocDis;
				}
			}
			else
            Utils.MostrarAlerta(Response, "No hay facturas de ese prefijo escoja otro");
		}
		
		protected void gridDocDis_Item(object Sender,DataGridCommandEventArgs e)
		{
			if(e.CommandName=="agregar")
			{
				if(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
				else if(((TextBox)e.Item.Cells[6].Controls[1]).Text=="0")
                    Utils.MostrarAlerta(Response, "¡No puede abonar $0 a una factura!");
				else
				{
					if((Convert.ToDouble(tablaDocDis.Rows[e.Item.DataSetIndex][5].ToString()))<(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text)))
                        Utils.MostrarAlerta(Response, "El valor a Abonar es mayor que el Saldo Total. Revise sus Datos");
						//Sino que haga todo lo que tiene que hacer
					else
					{
						Cruzar_Documentos(e,tablaDocDis);
						tablaDocDis.Rows[e.Item.DataSetIndex].Delete();
						gridDocDis.DataSource=tablaDocDis;
						gridDocDis.DataBind();
						Session["tablaDocDis"]=tablaDocDis;
					}
				}
			}
		}

		protected void btnGuardar_Click(object sender, System.EventArgs e)
		{
			if(rbCliente.Checked)
			{
				if(tablaPagar==null || tablaPagar.Rows.Count==0)
                    Utils.MostrarAlerta(Response, "No hay facturas ni notas. Agregue");
					//Si el faltante del cruce es negativo, entonces se deben agregar FC o NP
				else if(this.Identificar_Signo_Abono(totalCruce.Text)<0)
                    Utils.MostrarAlerta(Response, "Faltan "+(this.Identificar_Signo_Abono(totalCruce.Text)*-1).ToString("C")+" puede:\\n\\n- Agregar Facturas de Cliente o Notas de Proveedor o\\n- Retirar Facturas de Proveedor o Notas de Cliente'");
					//Si el faltante del cruce es positivo, entonces se deben agregar FP o NC
				else if(this.Identificar_Signo_Abono(totalCruce.Text)>0)
                    Utils.MostrarAlerta(Response, "Sobran "+this.Identificar_Signo_Abono(totalCruce.Text).ToString("C")+" puede:\\n\\n- Retirar Facturas de Cliente o Notas de Proveedor o\\n\\n- Agregar Facturas de Proveedor o Notas de Cliente");
				else
				{
                    if (DBFunctions.Transaction(Guardar_Cruce(tablaPagar)))
                    {
                        // Contabilizacion On Line
                        string fecha = DBFunctions.SingleData("SELECT mcru_fecha FROM mcrucedocumento WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue.ToString() + "' and mcru_numero = " + Convert.ToInt32(lbNumero.Text.ToString() + " "));
                        contaOnline.contabilizarOnline(ddlPrefijo.SelectedValue.ToString(), Convert.ToInt32(lbNumero.Text.ToString()), Convert.ToDateTime(fecha.ToString()), "");
                        Response.Redirect(indexPage + "?process=Tesoreria.CruceDocs&prefC=" + ddlPrefijo.SelectedValue + "&numC=" + lbNumero.Text + "");
                    }
						
					else
						lb.Text=DBFunctions.exceptions;
				}
			}
			else if(rbProveedor.Checked)
			{
				if(tablaPagar==null || tablaPagar.Rows.Count==0)
                    Utils.MostrarAlerta(Response, "No hay facturas ni notas. Agregue");
					//Si el faltante del cruce es negativo, entonces se deben agregar NC o FP
				else if(this.Identificar_Signo_Abono(totalCruce.Text)<0)
                    Utils.MostrarAlerta(Response, "Faltan "+(this.Identificar_Signo_Abono(totalCruce.Text)*-1).ToString("C")+" puede:\\n\\n- Agregar Facturas de Proveedor o Notas de Cliente o\\n- Retirar Facturas de Cliente o Notas de Proveedor");
					//Si el faltante del cruce es positivo, entonces se deben agregar NP o FC
				else if(this.Identificar_Signo_Abono(totalCruce.Text)>0)
                    Utils.MostrarAlerta(Response, "Sobran "+this.Identificar_Signo_Abono(totalCruce.Text).ToString("C")+" puede:\\n\\n- Retirar Facturas de Proveedor o Notas de Cliente o\\n- Agregar Facturas de Cliente o Notas de Proveedor");
				else
				{
                    if (DBFunctions.Transaction(Guardar_Cruce(tablaPagar)))
                    {
                        // Contabilizacion On Line
                        string fecha = DBFunctions.SingleData("SELECT mcru_fecha FROM mcrucedocumento WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue.ToString() + "' and mcru_numero = " + Convert.ToInt32(lbNumero.Text.ToString() + " "));
                        contaOnline.contabilizarOnline(ddlPrefijo.SelectedValue.ToString(), Convert.ToInt32(lbNumero.Text.ToString()), Convert.ToDateTime(fecha.ToString()), "");
                        Response.Redirect(indexPage + "?process=Tesoreria.CruceDocs&prefC=" + ddlPrefijo.SelectedValue + "&numC=" + lbNumero.Text + "");
                    }
                    else
                        lb.Text = DBFunctions.exceptions;                    
				}
			}
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tesoreria.CruceDocs");
		}

		protected void ddlPrefBus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			EnlazarDropDownListNum(ddlPrefBus.SelectedValue);
		}

		protected void btnBus_Click(object sender, System.EventArgs e)
		{
			int pagina=EncontrarPagina(BuscarPosicionGrilla(ddlPrefBus.SelectedValue,ddlNumBus.SelectedValue));
			if(pagina!=-1)
			{
				gridDocumentos.CurrentPageIndex=pagina-1;
				gridDocumentos.DataSource=tablaDocumentos;
				gridDocumentos.DataBind();
                Utils.MostrarAlerta(Response, "El documento " + ddlPrefBus.SelectedValue + "-" + ddlNumBus.SelectedValue + " se encuentra en la fila " + BuscarPosicionPagina(BuscarPosicionGrilla(ddlPrefBus.SelectedValue, ddlNumBus.SelectedValue)) + " de la tabla");
			}
		}

        protected void rbCliente_CheckedChanged(object sender, EventArgs e)
        {
            datCli.Enabled = true;

            string atributo;

            if (rbCliente.Checked)
                atributo = "ModalDialog(this, \'SELECT mnit_nit as nit, mnit_apellidos || \\' \\' || coalesce(mnit_apellido2,\\'\\') || \\' \\' || mnit_nombres || \\' \\' || coalesce(mnit_nombre2,\\'\\') as nombre FROM mnit ORDER BY mnit_nit;\',1,new Array())";
            else
                atributo = "ModalDialog(this, \'SELECT MN.mnit_nit as nit, mnit_apellidos || \\' \\' || coalesce(mnit_apellido2,\\'\\') || \\' \\' || mnit_nombres || \\' \\' || coalesce(mnit_nombre2,\\'\\') as nombre FROM mnit MN, MPROVEEDOR MP WHERE MN.MNIT_NIT = MP.MNIT_NIT ORDER BY MN.mnit_nit;\',1,new Array())";

            datCli.Attributes.Add("ondblclick", atributo);
        }

        protected void rbProveedor_CheckedChanged(object sender, EventArgs e)
        {
            datCli.Enabled = true;

            string atributo;

            if (rbCliente.Checked)
                atributo = "ModalDialog(this, \'SELECT mnit_nit as nit, mnit_apellidos || \\' \\' || coalesce(mnit_apellido2,\\'\\') || \\' \\' || mnit_nombres || \\' \\' || coalesce(mnit_nombre2,\\'\\') as nombre FROM mnit WHERE tnit_tiponit=\\'T\\' OR tnit_tiponit=\\'C\\' OR tnit_tiponit=\\'E\\' OR tnit_tiponit=\\'P\\' ORDER BY mnit_nit\',1,new Array())";
            else
                atributo = "ModalDialog(this, \'SELECT mnit_nit as nit, mnit_apellidos || \\' \\' || coalesce(mnit_apellido2,\\'\\') || \\' \\' || mnit_nombres || \\' \\' || coalesce(mnit_nombre2,\\'\\') as nombre FROM mnit WHERE tnit_tiponit=\\'N\\' OR tnit_tiponit=\\'R\\' ORDER BY mnit_nit\',1,new Array())";

            datCli.Attributes.Add("ondblclick", atributo);
        }

		#endregion

		#region Métodos

		protected void Devolver_Documentos(DataGridCommandEventArgs e,DataTable dt,DataGrid dg,string nombreTabla)
		{
			DataRow fila;
			fila=dt.NewRow();
			fila[0]=tablaPagar.Rows[e.Item.DataSetIndex][0].ToString();
			fila[1]=tablaPagar.Rows[e.Item.DataSetIndex][1].ToString();
			fila[2]=tablaPagar.Rows[e.Item.DataSetIndex][2].ToString();
			fila[3]=tablaPagar.Rows[e.Item.DataSetIndex][3].ToString();
			if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente")
				fila[4]=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM mfacturacliente WHERE pdoc_codigo='"+tablaPagar.Rows[e.Item.DataSetIndex][2].ToString()+"' AND mfac_numedocu="+tablaPagar.Rows[e.Item.DataSetIndex][3].ToString()+""));
			else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor")
				fila[4]=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM mfacturaproveedor WHERE pdoc_codiordepago='"+tablaPagar.Rows[e.Item.DataSetIndex][2].ToString()+"' AND mfac_numeordepago="+tablaPagar.Rows[e.Item.DataSetIndex][3].ToString()+""));
			fila[5]=Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][4].ToString());
			fila[6]=tablaPagar.Rows[e.Item.DataSetIndex][6].ToString();
			dt.Rows.Add(fila);
			dg.DataSource=dt;
			dg.ShowFooter=true;
			dg.DataBind();
			Session[nombreTabla]=dt;
			if(rbCliente.Checked)
			{
				//Si esta en mfacturacliente y es una factura o un abono, resto de totalcli
				if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="FC"))
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
					//Si está en mfacturacliente y es una nota, sumo a totalCli
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="NC")
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
					//Si esta en mafcturaproveedor y es una factura o un abono, sumo a totalpro
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="FP"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
					//Si está en mafcturaproveedor y es una nota, resto de totalpro
				else if((tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="NP"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
			}
			else if(rbProveedor.Checked)
			{
				//Si esta en mfacturacliente y es una factura o una abono, sumo a totalcli
				if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="FC"))
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
					//Si está en mfacturacliente y es una nota, resto de totalCli
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="NC")
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
					//Si esta en mafcturaproveedor y es una factura o un abono, resto de totalpro
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="FP"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
					//Si está en mafcturaproveedor y es una nota, sumo a totalpro
				else if((tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="NP"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
			}
			tablaPagar.Rows[e.Item.DataSetIndex].Delete();
			gridPagar.DataSource=tablaPagar;
			gridPagar.DataBind();
			Session["tablaPagar"]=tablaPagar;
		}
		
		protected void Cargar_tablaDocumentos()
		{
			tablaDocumentos=new DataTable();
			tablaDocumentos.Columns.Add(new DataColumn("TIPO", typeof(string)));
			tablaDocumentos.Columns.Add(new DataColumn("NIT", typeof(string)));
			tablaDocumentos.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			tablaDocumentos.Columns.Add(new DataColumn("NUMERODOCUMENTO", typeof(string)));
			tablaDocumentos.Columns.Add(new DataColumn("VALORDOCUMENTO", typeof(double)));
			tablaDocumentos.Columns.Add(new DataColumn("VALORABONADO",typeof(double)));
			tablaDocumentos.Columns.Add(new DataColumn("TIPODOCU",typeof(string)));
            tablaDocumentos.Columns.Add(new DataColumn("FACTURA", typeof(string)));
            tablaDocumentos.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
		}
		
		protected void Cargar_tablaPagar()
		{
			tablaPagar=new DataTable();
			tablaPagar.Columns.Add(new DataColumn("TIPO", typeof(string)));
			tablaPagar.Columns.Add(new DataColumn("NIT", typeof(string)));
			tablaPagar.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			tablaPagar.Columns.Add(new DataColumn("NUMERODOCUMENTO", typeof(string)));
			tablaPagar.Columns.Add(new DataColumn("VALORABONADO", typeof(double)));
			tablaPagar.Columns.Add(new DataColumn("VALORABONAR", typeof(double)));
			tablaPagar.Columns.Add(new DataColumn("TIPODOCU",typeof(string)));
		}

		protected void Cargar_tablaDocDis()
		{
			tablaDocDis=new DataTable();
			tablaDocDis.Columns.Add(new DataColumn("TIPO", typeof(string)));
			tablaDocDis.Columns.Add(new DataColumn("NIT", typeof(string)));
			tablaDocDis.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
			tablaDocDis.Columns.Add(new DataColumn("NUMERODOCUMENTO", typeof(string)));
			tablaDocDis.Columns.Add(new DataColumn("VALORDOCUMENTO", typeof(double)));
			tablaDocDis.Columns.Add(new DataColumn("VALORABONADO",typeof(double)));
			tablaDocDis.Columns.Add(new DataColumn("TIPODOCU",typeof(string)));
		}

		protected void Llenar_gridDocumentos()
		{
			int i;
			DataSet ds=new DataSet();
			DataRow fila;

            //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mnit_nit,MF.pdoc_codigo,mfac_numedocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),TDOC_TIPODOCU FROM dbxschema.mfacturacliente MF, PDOCUMENTO PD             WHERE MF.PDOC_CODIGO = PD.PDOC_CODIGO AND mnit_nit='"+datCli.Text+"' AND MF.tvig_vigencia<>'C' AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 AND TDOC_TIPODOCU IN ('FC','NC') ORDER BY MF.pdoc_codigo, mfac_numedocu");
            DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT mnit_nit,MF.pdoc_codigo,mfac_numedocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),
                (mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete - mfac_valoabon), TDOC_TIPODOCU, mf.mfac_factura CONCAT ' - ' CONCAT PP.mcue_codipuc CONCAT ' - ' CONCAT mc.mcue_nombre
                FROM dbxschema.mfacturacliente MF left join ppucdocumento PP on PP.pdoc_codigo = MF.pdoc_codigo and tcon_codigo = 'CXC'
                left join mcuenta mc on mc.mcue_codipuc = PP.mcue_codipuc
                , PDOCUMENTO PD
                WHERE MF.PDOC_CODIGO = PD.PDOC_CODIGO
                AND mnit_nit = '" + datCli.Text + @"'
                AND MF.tvig_vigencia <> 'C' AND mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete - mfac_valoabon > 0
                AND TDOC_TIPODOCU IN('FC', 'NC')
                ORDER BY MF.pdoc_codigo, mfac_numedocu; ");

            //DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mnit_nit,MF.pdoc_codiordepago,mfac_numeordepago,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),TDOC_TIPODOCU, mfac_prefdocu concat '-' concat mfac_numedocu as factura  FROM dbxschema.mfacturaproveedor MF, PDOCUMENTO PD WHERE MF.PDOC_CODIORDEPAGO = PD.PDOC_CODIGO AND mnit_nit='" + datCli.Text + "' AND MF.tvig_vigencia<>'C' AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 AND TDOC_TIPODOCU IN ('FP','NP') ORDER BY pdoc_codiordepago,mfac_numeordepago");
            DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT mnit_nit, MF.pdoc_codiordepago,mfac_numeordepago,(mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete),
                (mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete - mfac_valoabon), TDOC_TIPODOCU, mfac_prefdocu concat '-' concat mfac_numedocu as factura,
                mf.mfac_factura CONCAT ' - ' CONCAT PP.mcue_codipuc CONCAT ' - ' CONCAT mc.mcue_nombre
                FROM dbxschema.mfacturaproveedor MF left join ppucdocumento PP on PP.pdoc_codigo = MF.pdoc_codiordepago and tcon_codigo = 'CXP'
                left join mcuenta mc on mc.mcue_codipuc = PP.mcue_codipuc,
                PDOCUMENTO PD WHERE MF.PDOC_CODIORDEPAGO = PD.PDOC_CODIGO AND mnit_nit = '" + datCli.Text + @"' AND MF.tvig_vigencia <> 'C'
                AND mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete - mfac_valoabon > 0 AND TDOC_TIPODOCU IN('FP', 'NP') ORDER BY pdoc_codiordepago, mfac_numeordepago;");
            
            //0			1			2											3																			4
			if(Session["tablaDocumentos"]==null)
				this.Cargar_tablaDocumentos();
			//Si hay solo facturas de cliente
			if((ds.Tables[0].Rows.Count!=0)&&(ds.Tables[1].Rows.Count==0))
			{
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=tablaDocumentos.NewRow();
					fila["TIPO"]="Cliente";
					fila["NIT"]=ds.Tables[0].Rows[i][0].ToString();
					fila["PREFIJO"]=ds.Tables[0].Rows[i][1].ToString();
					fila["NUMERODOCUMENTO"]=ds.Tables[0].Rows[i][2].ToString();
					fila["VALORDOCUMENTO"]=Convert.ToDouble(ds.Tables[0].Rows[i][3].ToString());
					fila["VALORABONADO"]=Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
					fila["TIPODOCU"]=ds.Tables[0].Rows[i][5].ToString();
                    fila["DESCRIPCION"] = ds.Tables[0].Rows[i][6].ToString();
                    tablaDocumentos.Rows.Add(fila);
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
				}
				pnlInfo.Visible=true;
				rbCliente.Enabled=rbProveedor.Enabled=ddlPrefijo.Enabled=datCli.Enabled=datClia.Enabled=btnCargar.Enabled=false;
				CargarControlesDistintos();
			}
			//Si hay solo facturas de proveedor
			else if((ds.Tables[1].Rows.Count!=0)&&(ds.Tables[0].Rows.Count==0))
			{
				for(i=0;i<ds.Tables[1].Rows.Count;i++)
				{
					fila=tablaDocumentos.NewRow();
					fila["TIPO"]="Proveedor";
					fila["NIT"]=ds.Tables[1].Rows[i][0].ToString();
					fila["PREFIJO"]=ds.Tables[1].Rows[i][1].ToString();
					fila["NUMERODOCUMENTO"]=ds.Tables[1].Rows[i][2].ToString();
					fila["VALORDOCUMENTO"]=Convert.ToDouble(ds.Tables[1].Rows[i][3].ToString());
					fila["VALORABONADO"]=Convert.ToDouble(ds.Tables[1].Rows[i][4].ToString());
					fila["TIPODOCU"]=ds.Tables[1].Rows[i][5].ToString();
                    fila["FACTURA"] = ds.Tables[1].Rows[i][6].ToString();
                    fila["DESCRIPCION"] = ds.Tables[1].Rows[i][7].ToString();
                    tablaDocumentos.Rows.Add(fila);
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
				}
				pnlInfo.Visible=true;
				rbCliente.Enabled=rbProveedor.Enabled=ddlPrefijo.Enabled=datCli.Enabled=datClia.Enabled=btnCargar.Enabled=false;
				CargarControlesDistintos();
			}
			//Si hay ambas tanto cliente como proveedor
			else if((ds.Tables[0].Rows.Count!=0)&&(ds.Tables[1].Rows.Count!=0))
			{
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=tablaDocumentos.NewRow();
					fila["TIPO"]="Cliente";
					fila["NIT"]=ds.Tables[0].Rows[i][0].ToString();
					fila["PREFIJO"]=ds.Tables[0].Rows[i][1].ToString();
					fila["NUMERODOCUMENTO"]=ds.Tables[0].Rows[i][2].ToString();
					fila["VALORDOCUMENTO"]=Convert.ToDouble(ds.Tables[0].Rows[i][3].ToString());
					fila["VALORABONADO"]=Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
					fila["TIPODOCU"]=ds.Tables[0].Rows[i][5].ToString();
                    fila["DESCRIPCION"] = ds.Tables[0].Rows[i][6].ToString();
                    tablaDocumentos.Rows.Add(fila);
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
				}
				for(i=0;i<ds.Tables[1].Rows.Count;i++)
				{
					fila=tablaDocumentos.NewRow();
					fila["TIPO"]="Proveedor";
					fila["NIT"]=ds.Tables[1].Rows[i][0].ToString();
					fila["PREFIJO"]=ds.Tables[1].Rows[i][1].ToString();
					fila["NUMERODOCUMENTO"]=ds.Tables[1].Rows[i][2].ToString();
					fila["VALORDOCUMENTO"]=Convert.ToDouble(ds.Tables[1].Rows[i][3].ToString());
					fila["VALORABONADO"]=Convert.ToDouble(ds.Tables[1].Rows[i][4].ToString());
					fila["TIPODOCU"]=ds.Tables[1].Rows[i][5].ToString();
                    fila["FACTURA"] = ds.Tables[1].Rows[i][6].ToString();
                    fila["DESCRIPCION"] = ds.Tables[1].Rows[i][7].ToString();
                    tablaDocumentos.Rows.Add(fila);
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
				}
				pnlInfo.Visible=true;
				rbCliente.Enabled=rbProveedor.Enabled=ddlPrefijo.Enabled=datCli.Enabled=datClia.Enabled=btnCargar.Enabled=false;
				CargarControlesDistintos();
			}
			//Si no hay ninguna
			else if((ds.Tables[0].Rows.Count==0)&&(ds.Tables[1].Rows.Count==0))
                    Utils.MostrarAlerta(Response, "No existen documentos de este nit en el sistema");
		}

		protected void CargarControlesDistintos()
		{
			DatasToControls bind=new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlPrefFac, "SELECT DISTINCT PDOC.pdoc_codigo,PDOC.pdoc_codigo CONCAT ' - ' CONCAT PDOC.pdoc_descripcion FROM dbxschema.mfacturacliente MFAC,dbxschema.pdocumento PDOC WHERE MFAC.pdoc_codigo=PDOC.pdoc_codigo AND MFAC.mnit_nit<>'" + datCli.Text + "' order by PDOC.pdoc_codigo");
			bind.PutDatasIntoDropDownList(ddlNumFac,  "SELECT mfac_numedocu FROM dbxschema.mfacturacliente MF, PDOCUMENTO PD WHERE MF.pdoc_codigo=PD.PDOC_CODIGO AND MF.pdoc_codigo='"+ddlPrefFac.SelectedValue+"' AND MF.tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND TDOC_CODIGO IN ('FC','NC') AND mnit_nit<>'"+datCli.Text+"'");
		}
		
		protected double Identificar_Signo_Abono(string valor)
		{
			double res=0;
			if(valor.StartsWith("$"))
			{
				res=Convert.ToDouble(valor.Substring(1));
			}
			else if(valor.StartsWith("("))
			{
				res=Convert.ToDouble(valor.Substring(2,valor.Length-3))*-1;
			}
			return res;
		}
		
		protected ArrayList Guardar_Cruce(DataTable tablaPagar)
		{
			int i;
			double valorAbonado=0,valorFactura=0;
			ArrayList sqlStrings=new ArrayList();
			DateTime fechaC;
			try{fechaC=Convert.ToDateTime(txtFecha.Text);}
			catch{fechaC=DateTime.Now;}
			sqlStrings.Add("INSERT INTO mcrucedocumento VALUES('"+this.ddlPrefijo.SelectedValue+"',"+this.lbNumero.Text+",'"+fechaC.ToString("yyyy-MM-dd")+"','"+this.datCli.Text+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+HttpContext.Current.User.Identity.Name+"')");
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+lbNumero.Text+" WHERE pdoc_codigo='"+this.ddlPrefijo.SelectedValue+"'");
			for(i=0;i<tablaPagar.Rows.Count;i++)
			{
				if(tablaPagar.Rows[i][0].ToString()=="Cliente")
				{
					valorAbonado=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturacliente WHERE pdoc_codigo='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaPagar.Rows[i][3].ToString()+""));
					valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM mfacturacliente WHERE pdoc_codigo='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaPagar.Rows[i][3].ToString()+""));
					sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(tablaPagar.Rows[i][5])+" WHERE pdoc_codigo='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaPagar.Rows[i][3].ToString()+"");
					sqlStrings.Add("INSERT INTO dcrucedocumento VALUES('"+this.ddlPrefijo.SelectedValue+"',"+this.lbNumero.Text+","+i+",'"+tablaPagar.Rows[i][2].ToString()+"',"+tablaPagar.Rows[i][3].ToString()+",null,null,"+Convert.ToDouble(tablaPagar.Rows[i][5].ToString())+")");
					if((valorAbonado+Convert.ToDouble(tablaPagar.Rows[i][5]))==valorFactura)
						sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE pdoc_codigo='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaPagar.Rows[i][3].ToString()+"");
					else
						sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A' WHERE pdoc_codigo='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaPagar.Rows[i][3].ToString()+"");
				}
				else if(tablaPagar.Rows[i][0].ToString()=="Proveedor")
				{
					valorAbonado=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturaproveedor WHERE pdoc_codiordepago='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+tablaPagar.Rows[i][3].ToString()+""));
					valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM mfacturaproveedor WHERE pdoc_codiordepago='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+tablaPagar.Rows[i][3].ToString()+""));
					sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(tablaPagar.Rows[i][5])+" WHERE pdoc_codiordepago='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+tablaPagar.Rows[i][3].ToString()+"");
					sqlStrings.Add("INSERT INTO dcrucedocumento VALUES('"+this.ddlPrefijo.SelectedValue+"',"+this.lbNumero.Text+","+i+",null,null,'"+tablaPagar.Rows[i][2].ToString()+"',"+tablaPagar.Rows[i][3].ToString()+","+Convert.ToDouble(tablaPagar.Rows[i][5].ToString())+")");
					if((valorAbonado+Convert.ToDouble(tablaPagar.Rows[i][5]))==valorFactura)
						sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C' WHERE pdoc_codiordepago='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+tablaPagar.Rows[i][3].ToString()+"");
					else
						sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A' WHERE pdoc_codiordepago='"+tablaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+tablaPagar.Rows[i][3].ToString()+"");
				}
			}
			return sqlStrings;
		}

		protected void Cruzar_Documentos(DataGridCommandEventArgs e,DataTable tablaDocumentos)
		{
			DataRow fila;
			if(Session["tablaPagar"]==null)
				this.Cargar_tablaPagar();
			fila=tablaPagar.NewRow();
			fila[0]=tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString();//Tipo
			fila[1]=tablaDocumentos.Rows[e.Item.DataSetIndex][1].ToString();//Nit
			fila[2]=tablaDocumentos.Rows[e.Item.DataSetIndex][2].ToString();//Prefijo
			fila[3]=tablaDocumentos.Rows[e.Item.DataSetIndex][3].ToString();//Número Documento
			fila[4]=Convert.ToDouble(tablaDocumentos.Rows[e.Item.DataSetIndex][5].ToString());//Saldo
			fila[5]=Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);//Valor a Abonar
			fila[6]=tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString();//Tipo F, A, N
			tablaPagar.Rows.Add(fila);
			gridPagar.DataSource=tablaPagar;
			gridPagar.ShowFooter=true;
			gridPagar.DataBind();
			Session["tablaPagar"]=tablaPagar;
			//Si es como en un recibo de caja
			if(rbCliente.Checked)
			{
				//Si esta mfacturacliente y es una factura o un abono, sumo en totalcli
				if(tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && (tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="FC"))
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
					//Si esta en mfacturacliente y es un nota, resto en totalcli
				else if(tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="NC")
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
					//Si esta en mfacturaproveedor y es una factura o un abono, resto en totalpro
				else if(tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && (tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="FP"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
					//Si esta en mfacturaproveedor y es una nota, sumo en totalpro
				else if((tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="NP"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
				totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
			}
			else if(rbProveedor.Checked)
			{
				//Si esta mfacturacliente y es una factura o un abono, resto en totalcli
				if((tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente") && (tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="FC"))
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
					//Si esta en mfacturacliente y es una nota,sumo en totalcli
				else if((tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente") && (tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="NC"))
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
					//Si esta en mfacturaproveedor y es una factura o un abono, sumo en totalpro
		 		else if((tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor") && (tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="FP"))
			  		totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
					//Si esta en mfacturaproveedor y es una nota, sumo en totalpro
				else if((tablaDocumentos.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor") && (tablaDocumentos.Rows[e.Item.DataSetIndex][6].ToString()=="NP"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))).ToString("C");
				totalCruce.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(this.Identificar_Signo_Abono(totalCli.Text))).ToString("C");
			}
		}

		protected void EnlazarDropDownListPref()
		{
			ListItem miLI=new ListItem();
			string desc="";
			for(int i=0;i<tablaDocumentos.Rows.Count;i++)
			{
				if(!ExistePrefijoDropDownList(ddlPrefBus,tablaDocumentos.Rows[i][2].ToString()))
				{
					desc=DBFunctions.SingleData("SELECT pdoc_codigo ||'-'|| pdoc_descripcion FROM pdocumento WHERE pdoc_codigo='"+tablaDocumentos.Rows[i][2].ToString()+"'");
					miLI=new ListItem(desc,tablaDocumentos.Rows[i][2].ToString());
					ddlPrefBus.Items.Add(miLI);
				}
			}
			EnlazarDropDownListNum(ddlPrefBus.SelectedValue);
		}

		protected void EnlazarDropDownListNum(string valor)
		{
			ListItem miLi=new ListItem();
			ddlNumBus.Items.Clear();
			DataRow[] numeros=tablaDocumentos.Select("PREFIJO='"+valor+"'");
			for(int i=0;i<numeros.Length;i++)
			{
				if(!ExisteNumeroTablaPagar(valor,numeros[i][3].ToString()))
				{
					miLi=new ListItem(numeros[i][3].ToString(),numeros[i][3].ToString());
					ddlNumBus.Items.Add(miLi);
				}
			}
		}

		protected bool ExistePrefijoDropDownList(DropDownList ddl,string prefijo)
		{
			bool existe=false;
			for(int i=0;i<ddl.Items.Count;i++)
			{
				if(ddl.Items[i].Value==prefijo)
				{
					existe=true;
					break;
				}
			}
			return existe;
		}

		protected bool ExisteNumeroTablaPagar(string prefijo,string numero)
		{
			bool existe=false;
			if(tablaPagar!=null)
			{
				DataRow[] num=tablaPagar.Select("PREFIJO='"+prefijo+"' AND NUMERODOCUMENTO='"+numero+"'");
				if(num.Length!=0)
					existe=true;
			}
			return existe;
		}

		private int EncontrarPagina(int pos)
		{
			int pag=-1;
			double cantItePag=gridDocumentos.PageSize;
			if(pos!=-1)
				pag=(int)Math.Ceiling(pos/cantItePag);
			return pag;
		}

		private int BuscarPosicionGrilla(string prefijo,string numero)
		{
			int pos=-1;
			for(int i=0;i<tablaDocumentos.Rows.Count;i++)
			{
				if(tablaDocumentos.Rows[i][2].ToString()==prefijo && tablaDocumentos.Rows[i][3].ToString()==numero)
				{
					pos=i+1;
					break;
				}
			}
			return pos;
		}

		private int BuscarPosicionPagina(int pos)
		{
			int posi=-1;
			int cantItePag=gridDocumentos.PageSize;
			posi=pos%cantItePag;
			if(posi==0)
				posi=10;
			return posi;
		}

		#endregion

		#region Métodos Ajax

		[Ajax.AjaxMethod]
		public string CargarNumeroCruce(string prefijo)
		{
			return DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
		}

		[Ajax.AjaxMethod]
		public string Retornar_Nombre(string valor)
		{
			string nombre="";
			nombre=DBFunctions.SingleData("SELECT mnit_apellidos || ' ' || coalesce(mnit_apellido2,'') || ' ' || mnit_nombres || ' ' || coalesce(mnit_nombre2,'') FROM mnit WHERE mnit_nit='"+valor+"'");
			if(nombre=="")
				nombre+="Error";
			return nombre;
		}
		
		[Ajax.AjaxMethod]
		public DataSet Cargar_Facturas_Externas(string tipo,string prefijo,string cliente)
		{
			DataSet facs=new DataSet();
			if(tipo=="rb1")
			{
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT DISTINCT PDOC.pdoc_codigo AS PREFIJO,PDOC.pdoc_codigo CONCAT ' - ' CONCAT PDOC.pdoc_descripcion AS DESCRIPCION FROM dbxschema.mfacturacliente MFAC,dbxschema.pdocumento PDOC WHERE MFAC.pdoc_codigo=PDOC.pdoc_codigo AND MFAC.mnit_nit<>'"+cliente+"' order by PDOC.pdoc_codigo");
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT mfac_numedocu AS NUMERO FROM dbxschema.mfacturacliente MF, dbxschema.PDOCUMENTO PD WHERE MF.pdoc_codigo=PD.pdoc_codigo AND MF.pdoc_codigo='"+facs.Tables[0].Rows[0][0].ToString()+"' and mnit_nit<>'"+cliente+"' AND pd.tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND pd.TDOC_tipodocu IN ('FC','NC') ORDER BY mfac_numedocu");
			}
			else if(tipo=="rb2")
			{
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT DISTINCT PDOC.pdoc_codigo AS PREFIJO,PDOC.pdoc_codigo CONCAT ' - ' CONCAT PDOC.pdoc_descripcion AS DESCRIPCION FROM dbxschema.mfacturaproveedor MFAC,dbxschema.pdocumento PDOC WHERE MFAC.pdoc_codiordepago=PDOC.pdoc_codigo AND MFAC.mnit_nit<>'"+cliente+"' order by PDOC.pdoc_codigo");
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT mfac_numeordepago AS NUMERO FROM dbxschema.mfacturaproveedor WHERE pdoc_codiordepago='"+facs.Tables[0].Rows[0][0].ToString()+"' and mnit_nit<>'"+cliente+"' AND tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND mfac_indidocu IN('C','F','N','A','I') ORDER BY mfac_numeordepago");
			}
			return facs;
		}

		[Ajax.AjaxMethod]
		public DataSet Cargar_Numeros(string tipo,string prefijo,string cliente)
		{
			DataSet nums=new DataSet();
			if(tipo=="rb1")
				DBFunctions.Request(nums,IncludeSchema.NO,"SELECT mfac_numedocu AS NUMERO FROM dbxschema.mfacturacliente MF, dbxschema.PDOCUMENTO PD       WHERE MF.pdoc_codigo=PD.PDOC_CODIGO AND MF.pdoc_codigo='"+prefijo+"' and mnit_nit<>'"+cliente+"' AND MF.tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) > 0 AND TDOC_TIPODOCU IN ('FC','NC') ORDER BY mfac_numedocu");
			else if(tipo=="rb2")
				DBFunctions.Request(nums,IncludeSchema.NO,"SELECT mfac_numeordepago AS NUMERO FROM dbxschema.mfacturaproveedor MF, dbxschema.PDOCUMENTO PD WHERE MF.pdoc_codiordepago=PD.PDOC_CODIGO AND MF.pdoc_codiordepago='"+prefijo+"' and mnit_nit<>'"+cliente+"' AND MF.tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) > 0 AND TDOC_TIPODOCU IN ('FP','NP') ORDER BY mfac_numeordepago");
			return nums;
		}

        protected void click_MostrarLista(object sender, System.EventArgs e)
        {
            gridDocumentos.AllowPaging = false;
            tablaDocumentos = (DataTable)Session["tablaDocumentos"];
            gridDocumentos.DataSource = tablaDocumentos;
            gridDocumentos.DataBind();
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
			this.gridDocumentos.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridDocumentos_ItemCommand);
			this.gridDocumentos.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.gridDocumentos_PageIndexChange);
			this.gridDocumentos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridDocumentos_ItemDataBound);
			this.gridPagar.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridPagar_Item);
			this.gridPagar.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridPagar_ItemDataBound);
			this.gridDocDis.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridDocDis_Item);

		}
		#endregion	
        
	}
}
