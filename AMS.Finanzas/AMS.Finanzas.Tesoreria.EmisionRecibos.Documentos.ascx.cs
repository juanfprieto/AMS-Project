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
using Ajax;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class Documentos : System.Web.UI.UserControl
	{
		protected DataTable tablaDocumentos,tablaPagar,tablaDocDis;
		protected Button cargarDocs;
		protected Control padre;
		protected Control datosCliente,encabezado,varios,pagos,noCausados;
		protected string nitCliente,valorPrefijo,valorNumero;
		protected AMS.Finanzas.Tesoreria.Pagos controlPagos;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Finanzas.Tesoreria.Documentos));
			padre       =(this.Parent).Parent;
			encabezado  =((PlaceHolder)padre.FindControl("phEncabezado")).Controls[0];
			noCausados  =((PlaceHolder)padre.FindControl("phNoCausados")).Controls[0];
			varios      =((PlaceHolder)padre.FindControl("phVarios")).Controls[0];
			pagos       =((PlaceHolder)padre.FindControl("phPagos")).Controls[0];
			controlPagos=((AMS.Finanzas.Tesoreria.Pagos)((PlaceHolder)padre.FindControl("phPagos")).Controls[0]);
			DatasToControls bind=new DatasToControls();
			if(!IsPostBack)
				totalCli.Text=totalPro.Text=totalCruce.Text="$0.00";
			else
			{
				if(Session["tablaDocumentos"]!=null)
					tablaDocumentos=(DataTable)Session["tablaDocumentos"];
				if(Session["tablaPagar"]!=null)
					tablaPagar=(DataTable)Session["tablaPagar"];
				if(Session["tablaDocDis"]!=null)
					tablaDocDis=(DataTable)Session["tablaDocDis"];
				valorPrefijo=Request.Form[ddlPrefFac.UniqueID];
				valorNumero=Request.Form[ddlNumFac.UniqueID];
			}
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
            string sqlProveedor = @"SELECT mf.mnit_nit,pdoc_codiordepago,mfac_numeordepago,
				(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),
				(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),
				CASE WHEN tdoc_tipodocu='FP' THEN 'F' WHEN tdoc_tipodocu='NP' THEN 'N' END AS mfac_indidocu,  
                vm.mnit_nit concat '-' concat vm.nombre concat ' [' concat mfac_prefdocu concat '-' concat mfac_numedocu concat '] Vence:' concat mf.mfac_vence 
                concat ' Retcs Causadas ' concat VARCHAR_FORMAT (mfac_valorete,'$999,999,999.99') as factura    
				FROM dbxschema.mfacturaproveedor mf, dbxschema.pdocumento pd, dbxschema.vmnit vm
				WHERE mf.mnit_nit IN ('" + ((TextBox)encabezado.FindControl("datBen")).Text + @"','" + ((TextBox)encabezado.FindControl("datCli")).Text + @"')  
                AND vm.mnit_nit =  mf.mnit_nit AND 
                mf.tvig_vigencia<>'C' AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 AND 
				mfac_indidocu IN ('C','A','N','F','I') AND pd.pdoc_codigo=mf.pdoc_codiordepago  
                ORDER BY factura;";
          
            //Proveedores
            DBFunctions.Request(ds, IncludeSchema.NO, sqlProveedor);
              
            //Clientes
			DBFunctions.Request(ds,IncludeSchema.NO,
                @"SELECT mnit_nit,mfacturacliente.pdoc_codigo,mfac_numedocu, 
				(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete), 
				(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon), 
				CASE WHEN pdocumento.tdoc_tipodocu='FC' THEN 'F' WHEN pdocumento.tdoc_tipodocu='NC' THEN 'N' END AS mfac_tipodocu, 
				 ' Retcs Causadas ' concat VARCHAR_FORMAT (mfac_valorete,'$999,999,999.99') 
                FROM mfacturacliente, pdocumento  
				WHERE mnit_nit IN ('" + ((TextBox)encabezado.FindControl("datBen")).Text+ @"','" + ((TextBox)encabezado.FindControl("datCli")).Text+ @"')  
				AND mfacturacliente.tvig_vigencia<>'C' AND  
				mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0  
				AND mfac_tipodocu IN('C','F','N','A','I') and mfacturacliente.pdoc_codigo = pdocumento.pdoc_codigo 
                ORDER BY mfacturacliente.pdoc_codigo, mfac_numedocu;");
            
			if(Session["tablaDocumentos"]==null)
				this.Cargar_tablaDocumentos();

			
            double credAprob = 0;
            if (((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue.Equals("F"))
                credAprob = Convert.ToDouble(DBFunctions.SingleData("SELECT MCRED_VALOAPROB FROM MCREDITOFINANCIERA WHERE MCRED_CODIGO=" + (((DropDownList)encabezado.FindControl("ddlCredito")).SelectedValue + ";")));

            //Si hay solo facturas de proveedor
            if((ds.Tables[0].Rows.Count!=0)&&(ds.Tables[1].Rows.Count==0))
			{
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					fila=tablaDocumentos.NewRow();
                    fila["TIPO"] = "Proveedor"; 
					fila["NIT"]=ds.Tables[0].Rows[i][0].ToString();
					fila["PREFIJO"]=ds.Tables[0].Rows[i][1].ToString();
					fila["NUMERODOCUMENTO"]=ds.Tables[0].Rows[i][2].ToString();
					fila["VALORDOCUMENTO"]=Convert.ToDouble(ds.Tables[0].Rows[i][3].ToString());
                    if (credAprob > 0)
                        fila["VALORABONADO"] = credAprob;
                    else
                        fila["VALORABONADO"] = Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
					fila["TIPODOCU"]=ds.Tables[0].Rows[i][5].ToString();
                    fila["FACTURA"] = ds.Tables[0].Rows[i][6].ToString();
					tablaDocumentos.Rows.Add(fila);
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
				}
			}
            //Si hay solo facturas de cliente	
			else if((ds.Tables[1].Rows.Count!=0)&&(ds.Tables[0].Rows.Count==0))
			{
				for(i=0;i<ds.Tables[1].Rows.Count;i++)
				{
					fila=tablaDocumentos.NewRow();
					fila["TIPO"]="Cliente";
					fila["NIT"]=ds.Tables[1].Rows[i][0].ToString();
					fila["PREFIJO"]=ds.Tables[1].Rows[i][1].ToString();
					fila["NUMERODOCUMENTO"]=ds.Tables[1].Rows[i][2].ToString();
					fila["VALORDOCUMENTO"]=Convert.ToDouble(ds.Tables[1].Rows[i][3].ToString());
                    if (credAprob > 0)
                        fila["VALORABONADO"] = credAprob;
                    else
					    fila["VALORABONADO"]=Convert.ToDouble(ds.Tables[1].Rows[i][4].ToString());
					fila["TIPODOCU"]=ds.Tables[1].Rows[i][5].ToString();
                    fila["FACTURA"] = ds.Tables[1].Rows[i][6].ToString();
                    tablaDocumentos.Rows.Add(fila);
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
				}
			}
			//Si hay ambas tanto cliente como proveedor
			else if((ds.Tables[0].Rows.Count!=0)&&(ds.Tables[1].Rows.Count!=0))
			{
                LlenarTablaDocumentos(Request.QueryString["tipo"].ToString(), ds, credAprob);

                gridDocumentos.DataSource = tablaDocumentos;
                gridDocumentos.DataBind();
                Session["tablaDocumentos"] = tablaDocumentos;

                //for(i=0;i<ds.Tables[1].Rows.Count;i++)
                //{
                //    fila=tablaDocumentos.NewRow();
                //    fila["TIPO"]="Proveedor";
                //    fila["NIT"]=ds.Tables[1].Rows[i][0].ToString();
                //    fila["PREFIJO"]=ds.Tables[1].Rows[i][1].ToString();
                //    fila["NUMERODOCUMENTO"]=ds.Tables[1].Rows[i][2].ToString();
                //    fila["VALORDOCUMENTO"]=Convert.ToDouble(ds.Tables[1].Rows[i][3].ToString());
                //    if (credAprob > 0)
                //        fila["VALORABONADO"] = credAprob;
                //    else
                //        fila["VALORABONADO"]=Convert.ToDouble(ds.Tables[1].Rows[i][4].ToString());
                //    fila["TIPODOCU"]=ds.Tables[1].Rows[i][5].ToString();
                //    fila["FACTURA"] = ds.Tables[1].Rows[i][6].ToString();
                //    tablaDocumentos.Rows.Add(fila);
                //    gridDocumentos.DataSource=tablaDocumentos;
                //    gridDocumentos.DataBind();
                //    Session["tablaDocumentos"]=tablaDocumentos;
                //}

                //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    fila = tablaDocumentos.NewRow();
                //    fila["TIPO"] = "Cliente";
                //    fila["NIT"] = ds.Tables[0].Rows[i][0].ToString();
                //    fila["PREFIJO"] = ds.Tables[0].Rows[i][1].ToString();
                //    fila["NUMERODOCUMENTO"] = ds.Tables[0].Rows[i][2].ToString();
                //    fila["VALORDOCUMENTO"] = Convert.ToDouble(ds.Tables[0].Rows[i][3].ToString());
                //    if (credAprob > 0)
                //        fila["VALORABONADO"] = credAprob;
                //    else
                //        fila["VALORABONADO"] = Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
                //    fila["TIPODOCU"] = ds.Tables[0].Rows[i][5].ToString();
                //    tablaDocumentos.Rows.Add(fila);
                //    gridDocumentos.DataSource = tablaDocumentos;
                //    gridDocumentos.DataBind();
                //    Session["tablaDocumentos"] = tablaDocumentos;
                //}
			}
				//Si no hay ninguna
			else if((ds.Tables[0].Rows.Count==0)&&(ds.Tables[1].Rows.Count==0))
                    Utils.MostrarAlerta(Response, "No existen facturas de este nit en el sistema");
		}
		
		public void CargarGrillaDocumentos()
		{
			this.Llenar_gridDocumentos();
			pnlFacDis.Visible=true;
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlPrefFac,"SELECT DISTINCT PDOC.pdoc_codigo,PDOC.pdoc_codigo CONCAT ' - ' CONCAT PDOC.pdoc_descripcion FROM dbxschema.mfacturacliente MFAC,dbxschema.pdocumento PDOC WHERE MFAC.pdoc_codigo=PDOC.pdoc_codigo AND MFAC.mnit_nit NOT IN ('"+((TextBox)encabezado.FindControl("datBen")).Text+"','"+((TextBox)encabezado.FindControl("datCli")).Text+"')");
			bind.PutDatasIntoDropDownList(ddlNumFac,"SELECT mfac_numedocu FROM dbxschema.mfacturacliente WHERE pdoc_codigo='"+ddlPrefFac.SelectedValue+"' AND tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND mfac_tipodocu IN ('C','F','N','A','I')");
			EnlazarDropDownListPref();
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
		
		protected void gridDocumentos_Item(object Sender,DataGridCommandEventArgs e)
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
						Cruzar_Documentos(e.Item,tablaDocumentos);
						tablaDocumentos.Rows[e.Item.DataSetIndex].Delete();
						gridDocumentos.DataSource=tablaDocumentos;
						gridDocumentos.CurrentPageIndex=0;
						gridDocumentos.DataBind();
						Session["tablaDocumentos"]=tablaDocumentos;
						EnlazarDropDownListPref();
					}
				}
			}
			else if(e.CommandName=="addall")
			{
				if(tablaDocumentos.Rows.Count!=0)
				{
					//Response.Write("<script language:javascript>alert('Se cargaran todas las facturas con sus saldos totales');</script>");
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
						if(Request.QueryString["tipo"]=="RC")
						{
							//Si está en mfacturacliente y es una factura o un abono, sumo a totalcli
							if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && (tablaDocumentos.Rows[i][6].ToString()=="F" || tablaDocumentos.Rows[i][6].ToString()=="A"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, resto a total cli
							else if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && tablaDocumentos.Rows[i][6].ToString()=="N")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es un factura o un abono, resto a totalpro
							else if(tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && (tablaDocumentos.Rows[i][6].ToString()=="F" || tablaDocumentos.Rows[i][6].ToString()=="A"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, suma a totalpro
							else if((tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && tablaDocumentos.Rows[i][6].ToString()=="N"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
							totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text)) - (this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
						}
						else if(Request.QueryString["tipo"]=="CE")
						{
							//Si está en mfacturacliente y es una factura o un abono, resto a totalcli
							if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && (tablaDocumentos.Rows[i][6].ToString()=="F" || tablaDocumentos.Rows[i][6].ToString()=="A"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, sumo a total cli
							else if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && tablaDocumentos.Rows[i][6].ToString()=="N")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es un factura o un abono, sumo a totalpro
							else if(tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && (tablaDocumentos.Rows[i][6].ToString()=="F" || tablaDocumentos.Rows[i][6].ToString()=="A"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, resto a totalpro
							else if((tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && tablaDocumentos.Rows[i][6].ToString()=="N"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaDocumentos.Rows[i][5].ToString()))).ToString("C");
                            totalCruce.Text = ((this.Identificar_Signo_Abono(totalPro.Text)) - (this.Identificar_Signo_Abono(totalCli.Text))).ToString("C");
						}
					}

					tablaDocumentos.Clear();
					gridDocumentos.ShowFooter=false;
					gridDocumentos.DataSource=tablaDocumentos;
					gridDocumentos.DataBind();
					Session["tablaDocumentos"]=tablaDocumentos;
					EnlazarDropDownListPref();
				}
				else
					gridDocumentos.ShowFooter=false;
			}
            else if (e.CommandName == "addChecked")
            {
                ArrayList indexDelete = new ArrayList();
                int c = 0;
                foreach (DataGridItem item in gridDocumentos.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        if (((CheckBox)item.Cells[9].FindControl("cbRows")).Checked)
                        {
                            
                            //if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text))
                            if (!DatasToControls.ValidarDouble(((TextBox)item.Cells[6].FindControl("valpagtxt")).Text))
                            {
                                Utils.MostrarAlerta(Response, "Valor Invalido");
                                return;
                            }
                            else if (((TextBox)item.Cells[6].FindControl("valpagtxt")).Text == "0")
                            {
                                Utils.MostrarAlerta(Response, "¡No puede abonar $0 a una factura!");
                                return;
                            }
                            else
                            {
                                //Si el valor a abonar (escrito) es mayor que el saldo
                                double valorAb = Math.Round(Convert.ToDouble(tablaDocumentos.Rows[item.DataSetIndex][5]), 2);
                                if ((valorAb) < (Convert.ToDouble(((TextBox)item.Cells[6].FindControl("valpagtxt")).Text)))
                                {
                                    Utils.MostrarAlerta(Response, "El valor a Abonar es mayor que el Saldo Total. Revise sus Datos");
                                    return;
                                }
                                //Sino que haga todo lo que tiene que hacer
                                else
                                {
                                    Cruzar_Documentos(item, tablaDocumentos);
                                    indexDelete.Add(item.DataSetIndex);
                                    //tablaDocumentos.Rows[item.DataSetIndex].Delete();
                                }
                            }

                        }
                    }
                }
                for (int k = 0; k < indexDelete.Count; k++)
                {
                    int indexNum = Convert.ToInt16(indexDelete[k]);
                    tablaDocumentos.Rows[indexNum-k].Delete();
                }
                gridDocumentos.DataSource = tablaDocumentos;
                gridDocumentos.DataBind();
                Session["tablaDocumentos"] = tablaDocumentos;
                EnlazarDropDownListPref();
            }
		}

		protected void gridDocumentos_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				((Button)e.Item.Cells[7].FindControl("addAll")).Attributes.Add("onClick","return confirm('Esta seguro de cargar todas las facturas?');");
			}
		}

		protected void gridPagar_ItemDataBound(object Sender,DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				((Button)e.Item.Cells[0].FindControl("remAll")).Attributes.Add("onClick","return confirm('Esta seguro de remover todas las facturas?');");
			}
		}
		
		protected void gridPagar_Item(object Sender,DataGridCommandEventArgs e)
		{
			DataRow fila;
			if(((Button)e.CommandSource).CommandName=="removerFilas")
			{
				if((tablaPagar.Rows[e.Item.DataSetIndex][1].ToString())!=(((TextBox)encabezado.FindControl("datBen")).Text) && (tablaPagar.Rows[e.Item.DataSetIndex][1].ToString())!=(((TextBox)encabezado.FindControl("datCli")).Text))
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
						if(Request.QueryString["tipo"]=="RC")
						{
							//Si está en mafcturacliente y es una factura o una nota, resto de totalcli
							if(tablaPagar.Rows[i][0].ToString()=="Cliente" && (tablaPagar.Rows[i][6].ToString()=="F" || tablaPagar.Rows[i][6].ToString()=="A"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, sumo a totalcli
							else if(tablaPagar.Rows[i][0].ToString()=="Cliente" && tablaPagar.Rows[i][6].ToString()=="N")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una factura o un abono, sumo a totalpro
							else if(tablaPagar.Rows[i][0].ToString()=="Proveedor" && (tablaPagar.Rows[i][6].ToString()=="F" || tablaPagar.Rows[i][6].ToString()=="A"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, resto a totalpro
							else if((tablaPagar.Rows[i][0].ToString()=="Proveedor" && tablaPagar.Rows[i][6].ToString()=="N"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
							totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
						}
						else if(Request.QueryString["tipo"]=="CE")
						{
							//Si está en mafcturacliente y es una factura o una nota, sumo a totalcli
							if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && (tablaDocumentos.Rows[i][6].ToString()=="F" || tablaDocumentos.Rows[i][6].ToString()=="A"))
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturacliente y es una nota, resto de totalcli
							else if(tablaDocumentos.Rows[i][0].ToString()=="Cliente" && tablaDocumentos.Rows[i][6].ToString()=="N")
								totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una factura o un abono, resto de totalpro
							else if(tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && (tablaDocumentos.Rows[i][6].ToString()=="F" || tablaDocumentos.Rows[i][6].ToString()=="A"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
								//Si está en mfacturaproveedor y es una nota, sumo a totalpro
							else if((tablaDocumentos.Rows[i][0].ToString()=="Proveedor" && tablaDocumentos.Rows[i][6].ToString()=="N"))
								totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[i][5].ToString()))).ToString("C");
							totalCruce.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(this.Identificar_Signo_Abono(totalCli.Text))).ToString("C");
						}
					}
					tablaPagar.Clear();
					gridPagar.DataSource=tablaPagar;
					gridPagar.ShowFooter=false;
					gridPagar.DataBind();
					Session["tablaPagar"]=tablaPagar;
					EnlazarDropDownListPref();
				}
				else
					gridPagar.ShowFooter=false;
			}
		}
		
		protected void Esconder_Controles()
		{
			((PlaceHolder)padre.FindControl("phDocumentos")).Visible=false;
			((ImageButton)padre.FindControl("btnDocumentos")).ImageUrl="../img/AMS.BotonExpandir.png";
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
			if(Request.QueryString["tipo"]=="RC")
			{
				if(this.Identificar_Signo_Abono(totalCruce.Text)<0)
                    Utils.MostrarAlerta(Response, "Faltan " + (this.Identificar_Signo_Abono(totalCruce.Text) * -1).ToString("C") + " puede:\\n\\n- Agregar Facturas de Cliente o Notas de Proveedor o\\n- Retirar Facturas de Proveedor o Notas de Cliente");
				else
				{
					this.Cargar_Opcion();
				}
			}
			else if(Request.QueryString["tipo"]=="CE")
			{
				if(this.Identificar_Signo_Abono(totalCruce.Text)<0)
                    Utils.MostrarAlerta(Response, "Faltan " + (this.Identificar_Signo_Abono(totalCruce.Text) * -1).ToString("C") + " puede:\\n\\n- Agregar Facturas de Proveedor o Notas de Cliente o\\n- Retirar Facturas de Cliente o Notas de Proveedor");
				else
				{
					this.Cargar_Opcion();
				}
			}
		}
		
		protected void Cargar_Opcion()
		{
			((Label)((PlaceHolder)padre.FindControl("phPagos")).Controls[0].FindControl("lbRet")).Text="Retenciones";
			((DataGrid)((PlaceHolder)padre.FindControl("phPagos")).Controls[0].FindControl("gridRtns")).Visible=true;
			if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="I" || ((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="E")
			{
				//Ingreso Definitivo
				this.Esconder_Controles();
				((PlaceHolder)padre.FindControl("phNoCausados")).Visible=true;
				if(Session["TIPO_COMPROBANTE"].ToString().Equals("O"))
					((PlaceHolder)padre.FindControl("phCancelacionObligFin")).Visible=true;
				((ImageButton)padre.FindControl("btnNoCausados")).ImageUrl="../img/AMS.BotonContraer.png";
				((ImageButton)padre.FindControl("btnNoCausados")).Enabled=true;
				((Label)pagos.FindControl("lbDocs")).Text="Total Faltante Cruces/Abonos a Documentos : "+(this.Identificar_Signo_Abono(totalCruce.Text)).ToString("C");
			}
			else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="R")
			{
				//Reconsignación Cheques Devueltos
				this.Esconder_Controles();
				((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
				((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
				((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
				controlPagos.Cargar_Grilla_Pagos();
				((DataGrid)pagos.FindControl("gridPagos")).ShowFooter=false;
				((DataGrid)pagos.FindControl("gridPagos")).Columns[8].Visible=false;
				((DataGrid)pagos.FindControl("gridPagos")).Columns[9].Visible=true;
				((Label)pagos.FindControl("lbDocs")).Text="Total Faltante Cruces/Abonos a Documentos : "+(this.Identificar_Signo_Abono(totalCruce.Text)).ToString("C");
			}
			else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="P")
			{
				//Legalización de Provisionales
				this.Esconder_Controles();
				((PlaceHolder)padre.FindControl("phNoCausados")).Visible=true;
				if(Session["TIPO_COMPROBANTE"].ToString().Equals("O"))
					((PlaceHolder)padre.FindControl("phCancelacionObligFin")).Visible=true;
				((ImageButton)padre.FindControl("btnNoCausados")).ImageUrl="../img/AMS.BotonContraer.png";
				((ImageButton)padre.FindControl("btnNoCausados")).Enabled=true;
				controlPagos.Cargar_Grilla_Pagos();
				((DataGrid)pagos.FindControl("gridPagos")).ShowFooter=false;
				((DataGrid)pagos.FindControl("gridPagos")).Columns[8].Visible=false;
				((DataGrid)pagos.FindControl("gridPagos")).Columns[9].Visible=true;
				((Label)pagos.FindControl("lbDocs")).Text="Total Faltante Cruces/Abonos a Documentos : "+(this.Identificar_Signo_Abono(totalCruce.Text)).ToString("C");
			}
		}
		
		protected void aceptarFactura_Click(object Sender,EventArgs e)
		{
			DataRow fila;
			DataSet ds=new DataSet();
			if(valorNumero!=string.Empty)
			{
				if(rb1.Checked)
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mnit_nit, MF.pdoc_codigo,mfac_numedocu,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),CASE WHEN pd.tdoc_tipodocu='FC' THEN 'F' WHEN pd.tdoc_tipodocu='NC' THEN 'N' END AS mfac_tipodocu           FROM dbxschema.mfacturacliente MF, PDOCUMENTO PD   WHERE MF.pdoc_codigo=pd.pdoc_codigo And MF.pdoc_codigo='"+valorPrefijo+"' AND mfac_numedocu="+valorNumero+"");
				else
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mnit_nit, MF.pdoc_codiordepago,mfac_numeordepago,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),CASE WHEN pd.tdoc_tipodocu='FP' THEN 'F' WHEN pd.tdoc_tipodocu='NP' THEN 'N' END AS mfac_INDIdocu FROM dbxschema.mfacturaproveedor MF, PDOCUMENTO PD WHERE MF.pdoc_codiordepago=pd.pdoc_codigo and MF.pdoc_codiordepago='"+valorPrefijo+"' AND mfac_numeordepago="+valorNumero+"");
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
		
		[Ajax.AjaxMethod]
		public DataSet Cargar_Facturas_Externas(string tipo,string prefijo,string cliente,string beneficiario)
		{
			DataSet facs=new DataSet();
			if(tipo=="rb1")
			{
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT DISTINCT PDOC.pdoc_codigo AS PREFIJO,PDOC.pdoc_codigo CONCAT ' - ' CONCAT PDOC.pdoc_descripcion AS DESCRIPCION FROM dbxschema.mfacturacliente MFAC,dbxschema.pdocumento PDOC WHERE MFAC.pdoc_codigo=PDOC.pdoc_codigo AND MFAC.mnit_nit NOT IN ('"+cliente+"','"+beneficiario+"')");
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT mfac_numedocu AS NUMERO FROM dbxschema.mfacturacliente WHERE pdoc_codigo='"+facs.Tables[0].Rows[0][0].ToString()+"' and mnit_nit NOT IN ('"+cliente+"','"+beneficiario+"') AND tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND mfac_tipodocu IN ('C','F','N','A','I') ORDER BY mfac_numedocu");
			}
			else if(tipo=="rb2")
			{
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT DISTINCT PDOC.pdoc_codigo AS PREFIJO,PDOC.pdoc_codigo CONCAT ' - ' CONCAT PDOC.pdoc_descripcion AS DESCRIPCION FROM dbxschema.mfacturaproveedor MFAC,dbxschema.pdocumento PDOC WHERE MFAC.pdoc_codiordepago=PDOC.pdoc_codigo AND MFAC.mnit_nit NOT IN ('"+cliente+"','"+beneficiario+"')");
				DBFunctions.Request(facs,IncludeSchema.NO,"SELECT mfac_numeordepago AS NUMERO FROM dbxschema.mfacturaproveedor WHERE pdoc_codiordepago='"+facs.Tables[0].Rows[0][0].ToString()+"' and mnit_nit NOT IN ('"+cliente+"','"+beneficiario+"') AND tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND mfac_indidocu IN ('C','F','N','A','I') ORDER BY mfac_numeordepago");
			}
			return facs;
		}

		[Ajax.AjaxMethod]
		public DataSet Cargar_Numeros(string tipo,string prefijo,string cliente,string beneficiario)
		{
			DataSet nums=new DataSet();
			if(tipo=="rb1")
				DBFunctions.Request(nums,IncludeSchema.NO,"SELECT mfac_numedocu AS NUMERO     FROM dbxschema.mfacturacliente   WHERE pdoc_codigo='"+prefijo+"' and mnit_nit NOT IN ('"+cliente+"','"+beneficiario+"') AND tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND mfac_tipodocu IN('C','F','N','A','I') ORDER BY mfac_numedocu");
			else if(tipo=="rb2")
				DBFunctions.Request(nums,IncludeSchema.NO,"SELECT mfac_numeordepago AS NUMERO FROM dbxschema.mfacturaproveedor WHERE pdoc_codiordepago='"+prefijo+"' and mnit_nit NOT IN ('"+cliente+"','"+beneficiario+"') AND tvig_vigencia <> 'C' AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)>0 AND mfac_indidocu IN('C','F','N','A','I') ORDER BY mfac_numeordepago");
			return nums;
		}

        protected void Cruzar_Documentos(DataGridItem item, DataTable tablaDocumentos)
		{
            DataRow fila;
            if (Session["tablaPagar"] == null)
                this.Cargar_tablaPagar();
            fila = tablaPagar.NewRow();
            fila[0] = tablaDocumentos.Rows[item.DataSetIndex][0].ToString();//Tipo
            fila[1] = tablaDocumentos.Rows[item.DataSetIndex][1].ToString();//Nit
            fila[2] = tablaDocumentos.Rows[item.DataSetIndex][2].ToString();//Prefijo
            fila[3] = tablaDocumentos.Rows[item.DataSetIndex][3].ToString();//Número Documento
            fila[4] = Convert.ToDouble(tablaDocumentos.Rows[item.DataSetIndex][5].ToString());//Saldo
            fila[5] = Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text);//Valor a Abonar
            fila[6] = tablaDocumentos.Rows[item.DataSetIndex][6].ToString();//Tipo F, A, N
            tablaPagar.Rows.Add(fila);
            gridPagar.DataSource = tablaPagar;
            gridPagar.ShowFooter = true;
            gridPagar.DataBind();
            Session["tablaPagar"] = tablaPagar;
            //Si es como en un recibo de caja
            if (Request.QueryString["tipo"] == "RC")
            {
                //Si esta mfacturacliente y es una factura o un abono, sumo en totalcli
                if (tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Cliente" && (tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "F" || tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "A"))
                    totalCli.Text = ((this.Identificar_Signo_Abono(totalCli.Text)) + (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                //Si esta en mfacturacliente y es un nota, resto en totalcli
                else if (tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Cliente" && tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "N")
                    totalCli.Text = ((this.Identificar_Signo_Abono(totalCli.Text)) - (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                //Si esta en mfacturaproveedor y es una factura o un abono, resto en totalpro
                else if (tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Proveedor" && (tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "F" || tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "A"))
                    totalPro.Text = ((this.Identificar_Signo_Abono(totalPro.Text)) + (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                //Si esta en mfacturaproveedor y es una nota, sumo en totalpro
                else if ((tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Proveedor" && tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "N"))
                    totalPro.Text = ((this.Identificar_Signo_Abono(totalPro.Text)) - (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                totalCruce.Text = ((this.Identificar_Signo_Abono(totalCli.Text)) - (this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
            }
            else if (Request.QueryString["tipo"] == "CE")
            {
                //Si esta mfacturacliente y es una factura o un abono, resto en totalcli
                if (tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Cliente" && (tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "F" || tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "A"))
                    totalCli.Text = ((this.Identificar_Signo_Abono(totalCli.Text)) + (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                //Si esta en mfacturacliente y es una nota,sumo en totalcli
                else if (tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Cliente" && tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "N")
                    totalCli.Text = ((this.Identificar_Signo_Abono(totalCli.Text)) - (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                //Si esta en mfacturaproveedor y es una factura o un abono, sumo en totalpro
                else if (tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Proveedor" && (tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "F" || tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "A"))
                    totalPro.Text = ((this.Identificar_Signo_Abono(totalPro.Text)) + (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                //Si esta en mfacturaproveedor y es una nota, sumo en totalpro
                else if ((tablaDocumentos.Rows[item.DataSetIndex][0].ToString() == "Proveedor" && tablaDocumentos.Rows[item.DataSetIndex][6].ToString() == "N"))
                    totalPro.Text = ((this.Identificar_Signo_Abono(totalPro.Text)) - (Convert.ToDouble(((TextBox)item.Cells[6].Controls[1]).Text))).ToString("C");
                totalCruce.Text = ((this.Identificar_Signo_Abono(totalPro.Text)) - (this.Identificar_Signo_Abono(totalCli.Text))).ToString("C");
            }
		}
		
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
			if(Request.QueryString["tipo"]=="RC")
			{
				//Si esta en mfacturacliente y es una factura o un abono, resto de totalcli
				if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="F" || tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="A"))
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				//Si está en mfacturacliente y es una nota, sumo a totalCli
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="N")
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				//Si esta en mafcturaproveedor y es una factura o un abono, sumo a totalpro
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="F" || tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="A"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				//Si está en mafcturaproveedor y es una nota, resto de totalpro
				else if((tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="N"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				totalCruce.Text=((this.Identificar_Signo_Abono(totalCli.Text)) - (this.Identificar_Signo_Abono(totalPro.Text))).ToString("C");
			}
			else if(Request.QueryString["tipo"]=="CE")
			{
				//Si esta en mfacturacliente y es una factura o una abono, sumo a totalcli
				if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="F" || tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="A"))
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				//Si está en mfacturacliente y es una nota, resto de totalCli
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Cliente" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="N")
					totalCli.Text=((this.Identificar_Signo_Abono(totalCli.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				//Si esta en mafcturaproveedor y es una factura o un abono, resto de totalpro
				else if(tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && (tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="F" || tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="A"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))-(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				//Si está en mafcturaproveedor y es una nota, sumo a totalpro
				else if((tablaPagar.Rows[e.Item.DataSetIndex][0].ToString()=="Proveedor" && tablaPagar.Rows[e.Item.DataSetIndex][6].ToString()=="N"))
					totalPro.Text=((this.Identificar_Signo_Abono(totalPro.Text))+(Convert.ToDouble(tablaPagar.Rows[e.Item.DataSetIndex][5].ToString()))).ToString("C");
				totalCruce.Text=((this.Identificar_Signo_Abono(totalPro.Text)) - (this.Identificar_Signo_Abono(totalCli.Text))).ToString("C");
			}
			tablaPagar.Rows[e.Item.DataSetIndex].Delete();
			gridPagar.DataSource=tablaPagar;
			gridPagar.DataBind();
			Session["tablaPagar"]=tablaPagar;
			EnlazarDropDownListPref();
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
						Cruzar_Documentos(e.Item,tablaDocDis);
						tablaDocDis.Rows[e.Item.DataSetIndex].Delete();
						gridDocDis.DataSource=tablaDocDis;
						gridDocDis.DataBind();
						Session["tablaDocDis"]=tablaDocDis;
					}
				}
			}
		}
		
		protected void gridDocumentos_PageIndexChange(object Sender,DataGridPageChangedEventArgs e)
		{
			gridDocumentos.CurrentPageIndex=e.NewPageIndex;
			gridDocumentos.DataSource=tablaDocumentos;
			gridDocumentos.DataBind();
			Session["tablaDocumentos"]=tablaDocumentos;
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

        protected void click_MostrarLista(object sender, System.EventArgs e)
		{
            gridDocumentos.AllowPaging = false;
            gridDocumentos.Columns[9].Visible = true;
            tablaDocumentos = (DataTable)Session["tablaDocumentos"];
		    gridDocumentos.DataSource=tablaDocumentos;
		    gridDocumentos.DataBind();
		}

        protected void click_MostrarListaAll(object sender, System.EventArgs e)
		{
            DatasToControls bind = new DatasToControls();
            DataSet dsAllProv = new DataSet();
            DataSet facturas = new DataSet();

            DBFunctions.Request(facturas, IncludeSchema.NO, @"SELECT distinct pd.pdoc_codigo, pd.pdoc_codigo concat ' - ' concat pd.pdoc_nombre as PDOC_NOMBRE
				FROM dbxschema.mfacturaproveedor mf, dbxschema.pdocumento pd, dbxschema.vmnit vm
				WHERE mf.mnit_nit = vm.mnit_nit AND 
                mf.tvig_vigencia<>'C' AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 AND 
				mfac_indidocu IN ('C','A','N','F','I') AND pd.pdoc_codigo=mf.pdoc_codiordepago  
                ORDER BY 1;");

            //Proveedores
            DBFunctions.Request(dsAllProv, IncludeSchema.NO,
               @"SELECT mf.mnit_nit,pdoc_codiordepago,mfac_numeordepago,
				(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete),
				(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon),
				CASE WHEN tdoc_tipodocu='FP' THEN 'F' WHEN tdoc_tipodocu='NP' THEN 'N' END AS mfac_indidocu,  
                vm.mnit_nit concat '-' concat vm.nombre concat ' [' concat mfac_prefdocu concat '-' concat mfac_numedocu concat '] Vence:' concat mf.mfac_vence as factura   
				FROM dbxschema.mfacturaproveedor mf, dbxschema.pdocumento pd, dbxschema.vmnit vm
				WHERE mf.mnit_nit = vm.mnit_nit AND " +
                @"mf.tvig_vigencia<>'C' AND mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 AND 
				mfac_indidocu IN ('C','A','N','F','I') AND pd.pdoc_codigo=mf.pdoc_codiordepago  
                ORDER BY factura; ");

            //Clientes
            DBFunctions.Request(dsAllProv, IncludeSchema.NO,
                "SELECT mnit_nit,mfacturacliente.pdoc_codigo,mfac_numedocu," +
                "(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete)," +
                "(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon)," +
                "CASE WHEN pdocumento.tdoc_tipodocu='FC' THEN 'F' WHEN pdocumento.tdoc_tipodocu='NC' THEN 'N' END AS mfac_tipodocu " +
                "FROM mfacturacliente, pdocumento " +
                "WHERE mfacturacliente.tvig_vigencia<>'C' AND " +
                "mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon > 0 " +
                "AND mfac_tipodocu IN('C','F','N','A','I') and mfacturacliente.pdoc_codigo = pdocumento.pdoc_codigo ORDER BY mfacturacliente.pdoc_codigo, mfac_numedocu; ");


            double credAprob = 0;
            if (((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue.Equals("F"))
                credAprob = Convert.ToDouble(DBFunctions.SingleData("SELECT MCRED_VALOAPROB FROM MCREDITOFINANCIERA WHERE MCRED_CODIGO=" + (((DropDownList)encabezado.FindControl("ddlCredito")).SelectedValue + ";")));
            
            this.Cargar_tablaDocumentos();

            LlenarTablaDocumentos(Request.QueryString["tipo"].ToString(), dsAllProv ,credAprob);

            gridDocumentos.AllowPaging = false;
            gridDocumentos.Columns[9].Visible = true;
            gridDocumentos.DataSource = tablaDocumentos;
		    gridDocumentos.DataBind();
            Session["tablaDocumentos"] = tablaDocumentos;
            //bind.PutDatasIntoDropDownList(((DropDownList)encabezado.FindControl("tipoRecibo")), ((DropDownList)dsAllProv.Tables[2]));
            //((DropDownList)encabezado.FindControl("tipoRecibo")).DataTextField = "PDOC_CODIGO";
            //((DropDownList)encabezado.FindControl("tipoRecibo")).DataValueField = "PDOC_NOMBRE";
            //((DropDownList)encabezado.FindControl("tipoRecibo")).DataSource = facturas.Tables[0];
            //((DropDownList)encabezado.FindControl("tipoRecibo")).DataBind();
            ddlPrefBus.DataValueField = "PDOC_CODIGO";
            ddlPrefBus.DataTextField = "PDOC_NOMBRE";
            ddlPrefBus.DataSource = facturas.Tables[0];
            ddlPrefBus.DataBind();

        }

        protected void LlenarTablaDocumentos(string tipoRecibo, DataSet dsFacturas, double credAprob)
        {
            int cont = 0;
            int indiceTipo = 0; //Proveedores
            if(tipoRecibo == "RC")
            {
                indiceTipo = 1;  //Clientes
            }

            while (cont < 2)
            {
                for (int i = 0; i < dsFacturas.Tables[indiceTipo].Rows.Count; i++)
                {
                    DataRow fila;
                    fila = tablaDocumentos.NewRow();

                    if (indiceTipo == 0)
                        fila["TIPO"] = "Proveedor";
                    else
                        fila["TIPO"] = "Cliente";

                    fila["NIT"] = dsFacturas.Tables[indiceTipo].Rows[i][0].ToString();
                    fila["PREFIJO"] = dsFacturas.Tables[indiceTipo].Rows[i][1].ToString();
                    fila["NUMERODOCUMENTO"] = dsFacturas.Tables[indiceTipo].Rows[i][2].ToString();
                    fila["VALORDOCUMENTO"] = Convert.ToDouble(dsFacturas.Tables[indiceTipo].Rows[i][3].ToString());
                    if (credAprob > 0)
                        fila["VALORABONADO"] = credAprob;
                    else
                        fila["VALORABONADO"] = Convert.ToDouble(dsFacturas.Tables[indiceTipo].Rows[i][4].ToString());
                    fila["TIPODOCU"] = dsFacturas.Tables[indiceTipo].Rows[i][5].ToString();

                //    if (indiceTipo == 0)
                        fila["FACTURA"] = dsFacturas.Tables[indiceTipo].Rows[i][6].ToString();

                    tablaDocumentos.Rows.Add(fila);
                    Session["tablaDocumentos"] = tablaDocumentos;
                }
                cont++;
                if (indiceTipo == 0)
                    indiceTipo++;
                else
                    indiceTipo--;
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
