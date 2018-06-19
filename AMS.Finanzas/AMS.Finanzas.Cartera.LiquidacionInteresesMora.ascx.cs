using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class LiquidacionMora : System.Web.UI.UserControl
	{
		protected DataTable dtMora,dtPreLiq;
		protected DataSet ds;
		protected ArrayList facturas,detalles,final=new ArrayList();
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
						
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlAno,"SELECT pano_ano FROM pano ORDER BY pano_ano ASC");
				DatasToControls.EstablecerDefectoDropDownList(ddlAno,""+DateTime.Now.Year+"");
				bind.PutDatasIntoDropDownList(ddlMes,"SELECT pmes_mes,pmes_nombre FROM pmes WHERE pmes_mes BETWEEN 1 AND 12 ORDER BY pmes_mes ASC");
				DatasToControls.EstablecerDefectoDropDownList(ddlMes,DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes="+DateTime.Now.Month+""));
                //bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='FC' and tvig_vigencia = 'V' ");
                Utils.llenarPrefijos(Response, ref  ddlPrefijo , "%", "%", "FC");
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
			}
			else
			{
				if(Session["dtMora"]!=null)
					dtMora=(DataTable)Session["dtMora"];
				if(Session["dtPreLiq"]!=null)
					dtPreLiq=(DataTable)Session["dtPreLiq"];
				if(Session["detalles"]!=null)
					detalles=(ArrayList)Session["detalles"];
				if(Session["final"]!=null)
					final=(ArrayList)Session["final"];
				Cambiar_Pagina();
			}
		}
		
		protected void Cargar_dtMora()
		{
			//8 columnas 
			dtMora=new DataTable();
			dtMora.Columns.Add("PREFIJO",typeof(string));//0
			dtMora.Columns.Add("NUMERO",typeof(string));//1
			dtMora.Columns.Add("NIT",typeof(string));//2
			dtMora.Columns.Add("TIPO",typeof(string));//3
			dtMora.Columns.Add("VIGENCIA",typeof(string));//4
			dtMora.Columns.Add("VENCIMIENTO",typeof(string));//5
			dtMora.Columns.Add("FALTANTE",typeof(double));//6
			dtMora.Columns.Add("DIAS",typeof(string));//7
		}
		
		protected void Cargar_dtPreLiq()
		{
			dtPreLiq=new DataTable();
			dtPreLiq.Columns.Add("NIT",typeof(string));//0
			dtPreLiq.Columns.Add("FACREL",typeof(string));//1
			dtPreLiq.Columns.Add("FALTANTE",typeof(double));//2
			dtPreLiq.Columns.Add("INTERESES",typeof(double));//3
			dtPreLiq.Columns.Add("LIQUIDAR",typeof(bool));//4
		}
		
		protected void btnPreLiquidar_Click(object Sender,EventArgs e)
		{
			/*Función que cumple varias funciones, la primera de ellas es verificar que los valores ingresados
			 * por intereses y dias de gracia, sean iguales a los que ya se encuentran en CCARTERA. La segunda
			 * función es sacar de mfacturacliente, todas las facturas cuya fecha de vencimiento + los dias de
			 * gracia, sea menor a la fecha actual, luego llamo otras funciones que explico en su debido cuerpo*/
			double tasa=Convert.ToDouble(DBFunctions.SingleData("SELECT ccar_intmen FROM ccartera"));
			int diasGracia=Convert.ToInt32(DBFunctions.SingleData("SELECT ccar_diasgracia FROM ccartera"));
			DataRow fila;
			facturas=new ArrayList();
			detalles=new ArrayList();
			if(!cbNit.Checked)
			{
				if(!DatasToControls.ValidarDouble(tbTasa.Text) || (Convert.ToDouble(tbTasa.Text)>(5*tasa)))
                    Utils.MostrarAlerta(Response, "Tasa de Interes Invalida");
				else if(!DatasToControls.ValidarInt(tbGracia.Text) || (Convert.ToInt32(tbGracia.Text)!=diasGracia))
				{
                    Utils.MostrarAlerta(Response, "Los dias de gracia difieren del valor especificado en los parámetros de cartera.\\nSe tomara el valor inicialmente  registrado en cartera");
					tbGracia.Text=diasGracia.ToString();
				}
				else
				{
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante,SUM(DAYS(CURRENT DATE)-DAYS(mfac_vence + "+diasGracia+" days)) FROM dbxschema.mfacturacliente WHERE tvig_vigencia IN('V','A') AND mfac_tipodocu IN('C','F','N','A','I') AND mfac_vence + "+diasGracia+" DAYS < CURRENT DATE AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) > 1 GROUP BY pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) ORDER BY mnit_nit,pdoc_codigo,mfac_vence ASC");
					//lb.Text+="SELECT pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante,SUM(DAYS(CURRENT DATE)-DAYS(mfac_vence + "+diasGracia+" days)) FROM dbxschema.mfacturacliente WHERE tvig_vigencia IN('V','A') AND mfac_tipodocu IN('C','F','N','A','I') AND mfac_vence + "+diasGracia+" DAYS < CURRENT DATE AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) > 0 GROUP BY pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) ORDER BY mnit_nit,pdoc_codigo,mfac_vence ASC <br>";
					if(Session["dtMora"]==null)
						this.Cargar_dtMora();
					if(ds.Tables[0].Rows.Count!=0)
					{
						for(int i=0;i<ds.Tables[0].Rows.Count;i++)
						{
							fila=dtMora.NewRow();
							fila[0]=ds.Tables[0].Rows[i][0].ToString();
							fila[1]=ds.Tables[0].Rows[i][1].ToString();
							fila[2]=ds.Tables[0].Rows[i][2].ToString();
							fila[3]=ds.Tables[0].Rows[i][3].ToString();
							fila[4]=ds.Tables[0].Rows[i][4].ToString();
							fila[5]=Convert.ToDateTime(ds.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd");
							fila[6]=Convert.ToDouble(ds.Tables[0].Rows[i][6]);
							fila[7]=ds.Tables[0].Rows[i][7].ToString();
							dtMora.Rows.Add(fila);
						}
						Session["dtMora"]=dtMora;
						facturas=this.Cruzar_Documentos(ref dtMora);
						Session["cruces"]=facturas;
						lnbReporte.Visible=true;
						Llenar_dgPreLiq();
						btnLiquidar.Visible=true;
						btnPreLiquidar.Enabled=false;
					}
					else
                    Utils.MostrarAlerta(Response, "No se encontraron facturas");
				}
			}
			else
			{
				if(!DatasToControls.ValidarDouble(tbTasa.Text) || (Convert.ToDouble(tbTasa.Text)>(5*tasa)))
                    Utils.MostrarAlerta(Response, "Tasa de Interes Invalida");
				else if(!DatasToControls.ValidarInt(tbGracia.Text) || (Convert.ToInt32(tbGracia.Text)!=diasGracia))
				{
                    Utils.MostrarAlerta(Response, "Los dias de gracia difieren del valor especificado en los parámetros de cartera.\\nSe tomara el valor inicialmente  registrado en cartera");
					tbGracia.Text=diasGracia.ToString();
				}
				else
				{
					ds=new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante,SUM(DAYS(CURRENT DATE)-DAYS(mfac_vence + "+diasGracia+" days)) FROM dbxschema.mfacturacliente WHERE tvig_vigencia IN('V','A') AND mfac_tipodocu IN('C','F','N','A','I') AND mfac_vence + "+diasGracia+" DAYS < CURRENT DATE AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) > 1 AND mnit_nit='"+tbNit.Text+"' GROUP BY pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) ORDER BY mnit_nit,pdoc_codigo,mfac_vence ASC");
					//lb.Text+="SELECT pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) AS Faltante,SUM(DAYS(CURRENT DATE)-DAYS(mfac_vence + "+diasGracia+" days)) FROM dbxschema.mfacturacliente WHERE tvig_vigencia IN('V','A') AND mfac_tipodocu IN('C','F','N','A','I') AND mfac_vence + "+diasGracia+" DAYS < CURRENT DATE AND (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) > 0 GROUP BY pdoc_codigo,mfac_numedocu,mnit_nit,mfac_tipodocu,tvig_vigencia,mfac_vence,(mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete-mfac_valoabon) ORDER BY mnit_nit,pdoc_codigo,mfac_vence ASC <br>";
					if(Session["dtMora"]==null)
						this.Cargar_dtMora();
					if(ds.Tables[0].Rows.Count!=0)
					{
						for(int i=0;i<ds.Tables[0].Rows.Count;i++)
						{
							fila=dtMora.NewRow();
							fila[0]=ds.Tables[0].Rows[i][0].ToString();
							fila[1]=ds.Tables[0].Rows[i][1].ToString();
							fila[2]=ds.Tables[0].Rows[i][2].ToString();
							fila[3]=ds.Tables[0].Rows[i][3].ToString();
							fila[4]=ds.Tables[0].Rows[i][4].ToString();
							fila[5]=Convert.ToDateTime(ds.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd");
							fila[6]=Convert.ToDouble(ds.Tables[0].Rows[i][6]);
							fila[7]=ds.Tables[0].Rows[i][7].ToString();
							dtMora.Rows.Add(fila);
						}
						Session["dtMora"]=dtMora;
						facturas=this.Cruzar_Documentos(ref dtMora);
						Session["cruces"]=facturas;
						lnbReporte.Visible=true;
						Llenar_dgPreLiq();
						btnLiquidar.Visible=true;
						btnPreLiquidar.Enabled=false;
					}
					else
                    Utils.MostrarAlerta(Response, "No se encontraron facturas para el nit especificado");
				}
			}
		}
		
		protected void btnLiquidar_Click(object Sender,EventArgs e)
		{
			ArrayList sqls=new ArrayList();
			if(Liquidar_Intereses(ref sqls))
			{
				if(DBFunctions.Transaction(sqls))
				{
					//lb.Text+=DBFunctions.exceptions+"<br>";
					Response.Redirect(indexPage+"?process=Cartera.LiquidacionInteresesMora.Impresion");
				}
				else
					lb.Text+="Error "+DBFunctions.exceptions+"<br>";
					/*for(int i=0;i<sqls.Count;i++)
					lb.Text+=sqls[i].ToString()+"<br>";*/
			}
			else
            Utils.MostrarAlerta(Response, "Debe seleccionar por lo menos un nit para liquidar");
		}
		
		protected bool Validar_PreLiq()
		{
			bool exito=false;
			int cont=0;
			for(int i=0;i<dtPreLiq.Rows.Count;i++)
			{
				if(!Convert.ToBoolean(dtPreLiq.Rows[i][4]))
					cont++;
			}
			if(cont==dtPreLiq.Rows.Count)
				exito=true;
			return exito;
		}
		
		protected bool Liquidar_Intereses(ref ArrayList sqls)
		{
			int i,j;
			double valIva=0;
			bool exito=true;
			FacturaCliente miFactura=new FacturaCliente();
			uint numero=Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"'"));
			if(!Validar_PreLiq())
			{
				for(i=0;i<dtPreLiq.Rows.Count;i++)
				{
					if(Convert.ToBoolean(dtPreLiq.Rows[i][4]))
					{
						numero++;
						if(DBFunctions.SingleData("SELECT ccar_cobivaintmor FROM ccartera")=="S")
						{
							valIva=Convert.ToDouble(dtPreLiq.Rows[i][3])*0.16;
							miFactura=new FacturaCliente("FC",ddlPrefijo.SelectedValue,dtPreLiq.Rows[i][0].ToString(),ddlAlmacen.SelectedValue,"F",numero,
							                             5,DateTime.Now,DateTime.Now,Convert.ToDateTime(null),Convert.ToDouble(dtPreLiq.Rows[i][3]),valIva,
                                                         0, 0, 0, 0, DBFunctions.SingleData("SELECT pcen_centcart FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + ddlAlmacen.SelectedValue + "'"),
						    	                         "Factura de Intereses por Mora",DBFunctions.SingleData("SELECT pven_codigo FROM ccartera"),HttpContext.Current.User.Identity.Name.ToLower(),null);
							final.Add(miFactura.PrefijoFactura+"-"+miFactura.NumeroFactura);
							Session["final"]=final;
						}
						else
						{
							miFactura=new FacturaCliente("FC",ddlPrefijo.SelectedValue,dtPreLiq.Rows[i][0].ToString(),ddlAlmacen.SelectedValue,"F",numero,
							                             5,DateTime.Now,DateTime.Now,Convert.ToDateTime(null),Convert.ToDouble(dtPreLiq.Rows[i][3]),0,
                                                         0, 0, 0, 0, DBFunctions.SingleData("SELECT pcen_centcart FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + ddlAlmacen.SelectedValue + "'"),
					    		                         "Factura de Intereses por Mora",DBFunctions.SingleData("SELECT pven_codigo FROM ccartera"),HttpContext.Current.User.Identity.Name.ToLower(),null);
							final.Add(miFactura.PrefijoFactura+"-"+miFactura.NumeroFactura);
							Session["final"]=final;
						}
						for(j=0;j<detalles.Count;j++)
						{
							string[] partes=null;
							partes=detalles[j].ToString().Split('@');
							if(dtPreLiq.Rows[i][0].ToString()==partes[0].ToString())
								miFactura.SqlRels.Add("INSERT INTO dfacturaclientemora VALUES('@1',@2,'"+partes[1].ToString()+"',"+partes[2].ToString()+","+Convert.ToDouble(partes[4])+","+Convert.ToInt32(partes[5])+")");
						}
						if(miFactura.GrabarFacturaCliente(false))
							Sacar_Sqls(miFactura,ref sqls);
						else
							sqls.Add(miFactura.ProcessMsg);
					}
				}
			}
			else
				exito=false;
			return exito;
		}
		
		protected void Sacar_Sqls(FacturaCliente fc,ref ArrayList sqlStrings)
		{
			for(int i=0;i<fc.SqlStrings.Count;i++)
				sqlStrings.Add(fc.SqlStrings[i]);
		}
		
		protected void lnbReporte_Click(object Sender,EventArgs e)
		{
			Response.Write("<script language:javascript>w=window.open('AMS.Cartera.ReporteCruces.aspx')</script>");
		}
		
		protected void Cambiar_Pagina()
		{
			for(int i=0;i<dgPreLiq.Items.Count;i++)
			{
				if(dgPreLiq.CurrentPageIndex==0)
				{
					if(((CheckBox)dgPreLiq.Items[i].Cells[4].Controls[1]).Checked)
					{
						dtPreLiq.Rows[i][4]=true;
						Session["dtPreLiq"]=dtPreLiq;
					}
					else
					{
						dtPreLiq.Rows[i][4]=false;
						Session["dtPreLiq"]=dtPreLiq;
					}	
				}
				else
				{
					if(i==0)
					{
						if(((CheckBox)dgPreLiq.Items[i].Cells[4].Controls[1]).Checked)
						{
							dtPreLiq.Rows[5*dgPreLiq.CurrentPageIndex][4]=true;
							Session["dtPreLiq"]=dtPreLiq;
						}
						else
						{
							dtPreLiq.Rows[5*dgPreLiq.CurrentPageIndex][4]=false;
							Session["dtPreLiq"]=dtPreLiq;
						}	
					}
					else
					{
						if(((CheckBox)dgPreLiq.Items[i].Cells[4].Controls[1]).Checked)
						{
							dtPreLiq.Rows[(5*dgPreLiq.CurrentPageIndex)+i][4]=true;
							Session["dtPreLiq"]=dtPreLiq;
						}
						else
						{
							dtPreLiq.Rows[(5*dgPreLiq.CurrentPageIndex)+i][4]=false;
							Session["dtPreLiq"]=dtPreLiq;
						}
					}
				}
			}
		}
		
		protected void dgPreLiq_PageIndexChanged(object Sender,DataGridPageChangedEventArgs e)
		{							
			Cambiar_Pagina();
			dgPreLiq.CurrentPageIndex=e.NewPageIndex;
			dgPreLiq.DataSource=dtPreLiq;
			dgPreLiq.DataBind();
		}
		
		protected void dgPreLiq_DataBound(object Sender,DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.AlternatingItem || e.Item.ItemType==ListItemType.Item)
			{
				if(Convert.ToBoolean(dtPreLiq.Rows[e.Item.DataSetIndex][4]))
					((CheckBox)e.Item.Cells[4].Controls[1]).Checked=true;
				else
					((CheckBox)e.Item.Cells[4].Controls[1]).Checked=false;
			}
		}
		
		protected void dgPreLiq_ItemCommand(object Sender,DataGridCommandEventArgs e)
		{
			if(e.CommandName=="Liquidar_Todos")
			{
				for(int i=0;i<dtPreLiq.Rows.Count;i++)
				{
					dtPreLiq.Rows[i][4]=true;
					Session["dtPreLiq"]=dtPreLiq;
				}
				dgPreLiq.DataSource=dtPreLiq;
				dgPreLiq.DataBind();
			}
		}
		
		protected void Llenar_dgPreLiq()
		{
			ArrayList nit=new ArrayList();
			detalles=new ArrayList();
			Sacar_Nits(dtMora,ref nit);
			int i,j;
			DataRow fila;
			for(i=0;i<nit.Count;i++)
			{
				DataRow[] facturas=dtMora.Select("TIPO='F' AND NIT='"+nit[i].ToString()+"' AND VIGENCIA IN('V','A') AND FALTANTE>=1");
				string facRels="";
				double valAcu=0,valInt=0;
				if(facturas.Length>0)
				{
					for(j=0;j<facturas.Length;j++)
					{
						facRels+=" "+facturas[j][0].ToString()+"-"+facturas[j][1].ToString();
						valAcu=valAcu+Convert.ToDouble(facturas[j][6]);
						//Interes=Monto*Tasa*(dias/360)
						double intereses=(Convert.ToDouble(facturas[j][6])*(Convert.ToDouble(tbTasa.Text)/100)*(Convert.ToDouble(facturas[j][7])/360));
						valInt=valInt+intereses;
						if(valInt>=1)
							detalles.Add(nit[i].ToString()+"@"+facturas[j][0].ToString()+"@"+facturas[j][1].ToString()+"@"+facturas[j][6].ToString()+"@"+intereses.ToString()+"@"+facturas[j][7].ToString());
							//					nit				pref fac rel					num fac rel						valor fac				valor inter						#dias
						Session["detalles"]=detalles;
					}
					if(Session["dtPreLiq"]==null)
						Cargar_dtPreLiq();
					fila=dtPreLiq.NewRow();
					fila[0]=nit[i].ToString();
					fila[1]=facRels;
					fila[2]=valAcu;
					fila[3]=valInt;
					fila[4]=false;
					dtPreLiq.Rows.Add(fila);
					dgPreLiq.DataSource=dtPreLiq;
					dgPreLiq.DataBind();
					Session["dtPreLiq"]=dtPreLiq;
				}
			}
		}
		
		protected ArrayList Cruzar_Documentos(ref DataTable dtMora)
		{
			/*Esta es LA función, lo que hace es recibir la tabla que lleno en btnPreLiquidar_Click y saca por
			 * aparte las notas y las facturas, luego llamo la función Sacar_Nits y empiezo a recorrer por cada
			 * nit todas las notas y las facturas que tenga y cruzo las mas antiguas (vienen ordenadas por fecha),
			 * al final retorno un arraylist con las facturas que resultaron canceladas o abonadas con los cruces*/
			DataRow[] facturas=null;
			DataRow[] notas=null;
			double valorNota=0,valorFactura=0;
			ArrayList nit=new ArrayList();
			ArrayList facCan=new ArrayList();
			this.Sacar_Nits(dtMora,ref nit);
			for(int i=0;i<nit.Count;i++)
			{
				facturas=dtMora.Select("TIPO='F' AND NIT='"+nit[i]+"'");
				notas=dtMora.Select("TIPO='N' AND NIT='"+nit[i]+"'");
				for(int j=0;j<facturas.Length;j++)
				{
					valorFactura=Convert.ToDouble(facturas[j][6]);
					if(valorFactura>0)
					{
						for(int k=0;k<notas.Length;k++)
						{
							valorNota=Convert.ToDouble(notas[k][6]);
							if(valorNota>0)
							{
								if(valorFactura>valorNota)
								{
									if(facCan.BinarySearch(nit[i]+"@"+facturas[j][0]+"@"+facturas[j][1]+"@"+facturas[j][6]+"@"+notas[k][0]+"@"+notas[k][1]+"@"+notas[k][6])<0)
												//			 nit	    prefijo fac		    numero fac		      valor			prefijo not		numero not		valor		
										facCan.Add(nit[i]+"@"+facturas[j][0]+"@"+facturas[j][1]+"@"+facturas[j][6]+"@"+notas[k][0]+"@"+notas[k][1]+"@"+notas[k][6]);
									valorFactura=valorFactura-valorNota;
									valorNota=0;
									facturas[j][6]=valorFactura;
									notas[k][6]=valorNota;
									facturas[j][4]="A";
									notas[k][4]="C";
								}
								else if(valorFactura<valorNota)
								{
									if(facCan.BinarySearch(nit[i]+"@"+facturas[j][0]+"@"+facturas[j][1]+"@"+facturas[j][6]+"@"+notas[k][0]+"@"+notas[k][1]+"@"+facturas[j][6])<0)
												//			 nit	    prefijo fac		    numero fac		      valor			prefijo not		numero not		    valor
										facCan.Add(nit[i]+"@"+facturas[j][0]+"@"+facturas[j][1]+"@"+facturas[j][6]+"@"+notas[k][0]+"@"+notas[k][1]+"@"+facturas[j][6]);
									valorNota=valorNota-valorFactura;
									valorFactura=0;
									facturas[j][6]=valorFactura;
									notas[k][6]=valorNota;
									facturas[j][4]="C";
									notas[k][4]="A";
									break;
								}
								else if(valorFactura==valorNota)
								{
									if(facCan.BinarySearch(nit[i]+"@"+facturas[j][0]+"@"+facturas[j][1]+"@"+facturas[j][6]+"@"+notas[k][0]+"@"+notas[k][1]+"@"+facturas[j][6])<0)
												//			 nit	    prefijo fac		    numero fac		      valor			prefijo not		numero not		    valor
										facCan.Add(nit[i]+"@"+facturas[j][0]+"@"+facturas[j][1]+"@"+facturas[j][6]+"@"+notas[k][0]+"@"+notas[k][1]+"@"+facturas[j][6]);
									valorFactura=0;
									valorNota=0;
									facturas[j][6]=valorFactura;
									notas[k][6]=valorNota;
									facturas[j][4]="C";
									notas[k][4]="C";
									break;
								}
							}
						}
					}
				}
			}
			return facCan;
		}
		
		protected void Sacar_Nits(DataTable dt,ref ArrayList nit)
		{
			for(int i=0;i<dt.Rows.Count;i++)
			{
				if(nit.BinarySearch(dt.Rows[i][2].ToString())<0)
					nit.Add(dt.Rows[i][2].ToString());
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

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Cartera.LiquidacionInteresesMora");
		}
	}
}
