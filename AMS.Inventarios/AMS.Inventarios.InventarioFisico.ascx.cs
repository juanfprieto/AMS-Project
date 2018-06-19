using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using System.Collections;
using AMS.Tools;
using Ajax;

namespace AMS.Inventarios
{
	/// <summary>
	///		Descripción breve de AMS_Inventarios_InventarioFisico.
	/// </summary>
	public partial class AMS_Inventarios_InventarioFisico : System.Web.UI.UserControl
	{
		#region Constantes

		private const string sqlItemsUbicaciones = @"Select 
	   mubi.PUBI_CODIGO,
	   mubi.MITE_CODIGO,
	   mite.MITE_NOMBRE,
	   pubi.PUBI_CODPAD,
	   pubi.PUBI_NOMBRE,
	   pubi.PUBI_UBICESPACIAL,
	   pubi.PALM_ALMACEN,
	   msal.MSAL_CANTACTUAL,
	   msal.MSAL_COSTPROM,
	   DBXSCHEMA.EDITARREFERENCIAS(mite.mite_codigo,plin.plin_tipo) AS MITE_CODIGOEDITADO
from dbxschema.msaldoitemalmacen msal
     inner join dbxschema.cinventario cinv	  on msal.pano_ano    = cinv.pano_ano	 
	 inner join dbxschema.mitems mite	      on msal.mite_codigo = mite.mite_codigo
	 inner join	dbxschema.plineaitem plin	  on plin.plin_codigo = mite.plin_codigo
	 left join 	dbxschema.mubicacionitem mubi on mubi.mite_codigo = msal.mite_codigo
	 inner join dbxschema.pubicacionitem pubi on pubi.pubi_codigo = mubi.pubi_codigo and pubi.palm_almacen = msal.palm_almacen
where
	 pubi.pubi_codigo = {0} AND
	   msal.msal_cantactual >{1} 0 and msal.palm_almacen='{2}'
order by  pubi.PUBI_NOMBRE, mite.mite_codigo;";

        private const string sqlGralItemsUbicaciones = @"Select 
	   mubi.PUBI_CODIGO,
	   mubi.MITE_CODIGO,
	   mite.MITE_NOMBRE,
	   pubi.PUBI_CODPAD,
	   pubi.PUBI_NOMBRE,
	   pubi.PUBI_UBICESPACIAL,
	   pubi.PALM_ALMACEN,
	   msal.MSAL_CANTACTUAL,
	   msal.MSAL_COSTPROM,
	   DBXSCHEMA.EDITARREFERENCIAS(mite.mite_codigo,plin.plin_tipo) AS MITE_CODIGOEDITADO
from dbxschema.msaldoitemalmacen msal
     inner join dbxschema.cinventario cinv	  on msal.pano_ano    = cinv.pano_ano	 
	 inner join dbxschema.mitems mite	      on msal.mite_codigo = mite.mite_codigo
	 inner join	dbxschema.plineaitem plin	  on plin.plin_codigo = mite.plin_codigo
	 left join 	dbxschema.mubicacionitem mubi on mubi.mite_codigo = msal.mite_codigo
	 inner join dbxschema.pubicacionitem pubi on pubi.pubi_codigo = mubi.pubi_codigo and pubi.palm_almacen = msal.palm_almacen
where  msal.msal_cantactual >{0} 0 and msal.palm_almacen='{1}'
order by pubi.PUBI_NOMBRE, mite.mite_codigo;";



        private const string sqlItemsSinUbicaciones = @"Select 
	   mite.MITE_CODIGO,
	   mite.MITE_NOMBRE,
	   msal.PALM_ALMACEN,
	   msal.MSAL_CANTACTUAL,
	   msal.MSAL_COSTPROM,
	   DBXSCHEMA.EDITARREFERENCIAS(mite.mite_codigo,plin.plin_tipo) AS MITE_CODIGOEDITADO
 from dbxschema.msaldoitemalmacen msal
     inner join dbxschema.cinventario cinv on msal.pano_ano    = cinv.pano_ano	 
	 inner join dbxschema.mitems mite	   on msal.mite_codigo = mite.mite_codigo
	 inner join dbxschema.plineaitem plin  on plin.plin_codigo = mite.plin_codigo
    where
	 msal.palm_almacen = '{0}' AND 
     msal.msal_cantactual >{1} 0 AND
	 msal.mite_codigo not in(
	 	select mu1.mite_codigo
		from mubicacionitem mu1, pubicacionitem pu1 
		where pu1.palm_almacen='{0}' AND mu1.pubi_codigo=pu1.pubi_codigo
	 ) 
    order by mite.mite_codigo;";
        #region anterior
        /*"Select 
	   mite.MITE_CODIGO,
	   mite.MITE_NOMBRE,
	   msal.PALM_ALMACEN,
	   msal.MSAL_CANTACTUAL,
	   msal.MSAL_COSTPROM,
	   DBXSCHEMA.EDITARREFERENCIAS(mite.mite_codigo,plin.plin_tipo) AS MITE_CODIGOEDITADO
from 
	 dbxschema.msaldoitemalmacen msal
     inner join
 	 dbxschema.cinventario cinv
	 on
	   msal.pano_ano = cinv.pano_ano	 
	 inner join
 	 dbxschema.mitems mite
	 on
	   msal.mite_codigo = mite.mite_codigo
	 inner join
	 dbxschema.plineaitem plin
	 on
	   plin.plin_codigo = mite.plin_codigo
	 left join 
	 dbxschema.mubicacionitem mubi
	 on
	   mubi.mite_codigo = msal.mite_codigo
where
     mubi.pubi_codigo is null AND
	 msal.palm_almacen = '{0}' AND 
     msal.msal_cantactual >{1} 0 
order by
	 mite.mite_codigo";*/
        #endregion
        #endregion

        #region Atributos
        protected System.Web.UI.WebControls.TextBox prefijo;
		protected DataTable tablaItems;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion		

		#region Eventos

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Inventarios_InventarioFisico));	

			if(!IsPostBack)
			{
				btnRealizar.Attributes.Add("onclick","return ValidarInfoInicial();");
				
				DatasToControls	bind = new DatasToControls();

                if (ddlAlmacenes.Items.Count==0)
				{
					// Carga el DropDownList con el listado de almacenes.
                    bind.PutDatasIntoDropDownList(ddlAlmacenes, "select palm_almacen, palm_descripcion from palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
                    if (ddlAlmacenes.Items.Count > 1)
                    {
                        ListItem it = new ListItem("--Seleccione ALMACEN --", "");
                        ddlAlmacenes.Items.Insert(0, it);
                    }
				}

				if(ddlInventario.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(ddlInventario,"select pdoc_codigo CONCAT '-' CONCAT cast(minf_numeroinv as char(10)), cast(minf_fechainicio as char(10)) concat ' Inventario Número : ' concat pdoc_codigo concat '-' concat cast(minf_numeroinv as char(10)) from MINVENTARIOFISICO where (pdoc_codigo, minf_numeroinv) not in (SELECT pdoc_codigo, minf_numeroinv FROM dinventariofisico WHERE dinv_conteoactual > 0)");

					ListItem it=new ListItem("--Seleccione INVENTARIO --","");

					ddlInventario.Items.Insert(0,it);
				}
				if(Request.QueryString["rmpI"]!=null && Request.QueryString["rmpF"]!=null)
				{
					if(Request.QueryString["rmpI"]!="0")
					{
						lblInfo.Text="Debe reimprimir las tarjetas "+Request.QueryString["rmpI"]+" - "+Request.QueryString["rmpF"];
						Response.Write("<script>alert('Debe reimprimir las tarjetas "+Request.QueryString["rmpI"]+" - "+Request.QueryString["rmpF"]+".');</script>");
					}
				}
			}
		}

        protected void guardar_tabla(object sender, EventArgs e)
        {
            if (ddlProceso.SelectedValue == "1")
                Guardar();
            else if (ddlProceso.SelectedValue == "2")
                    Revisar();
		}
		private void Guardar(){


            string prefijoInv;
            int numeroInv;
           
            if (ddlInventario.SelectedValue.Split('-').Length == 3)
            {
                prefijoInv = (ddlInventario.SelectedValue.Split('-'))[0] + "-" + (ddlInventario.SelectedValue.Split('-'))[1];
                numeroInv = Convert.ToInt32((ddlInventario.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInv = (ddlInventario.SelectedValue.Split('-'))[0];
                numeroInv = Convert.ToInt32((ddlInventario.SelectedValue.Split('-'))[1].Trim());
            }

			int contar=0;

			DataSet hijos = new DataSet();
			
			// Esta función guarda la tabla de los ítems.
			dgitems.Visible=true;
			
			ArrayList pos=new ArrayList();
			ArrayList pos1=new ArrayList();
			
			string usuario=HttpContext.Current.User.Identity.Name.ToLower();
			
			if(ddlInventario.SelectedValue != "0")
			{
				if(prefijoInv != "")
				{
					InventarioFisico inst = new InventarioFisico(prefijoInv,numeroInv);

					// Borramos las tarjetas de conteo generadas previamente.
					inst.ItemsInventario.Clear();
					
					for (int x=0;x<dgitems.Items.Count;x++)
					{
						if(dgitems.Items[x].Cells[0].Text != "&nbsp;")
							inst.ItemsInventario.Add(new ItemsInventarioFisico(
								Convert.ToInt32(ValidarCampoEntero(dgitems.Items[x].Cells[8].Text)),
								dgitems.Items[x].Cells[7].Text,
								ValidarCampoCadena(dgitems.Items[x].Cells[3].Text),
								dgitems.Items[x].Cells[4].Text,
								Convert.ToDouble(ValidarCampoEntero(dgitems.Items[x].Cells[5].Text)),
								Convert.ToDouble(ValidarCampoEntero(dgitems.Items[x].Cells[6].Text)),
								ValidarCampoCadena(dgitems.Items[x].Cells[1].Text),
								string.Empty));
					}

					if(inst.CrearItemsInventario())
					{
						Response.Write("<script>alert('La lista de ítems escogidos fue guardada y esta lista para realizar el conteo.');</script>");
						Response.Write("<script>alert('Se encontraron "+contar+" ítems los cuales no contenian cantidad ni costo promedio, por esta razón se les agrego cero a ambos campos.');</script>");
						
						dgitems.Visible=false;
						btnSave.Visible=false;
						
						Response.Redirect(indexPage + "?path="+Request.QueryString["path"]+"&process=Inventarios.InventarioFisico");
					}
					else
					{
						Response.Write(inst.ProcessMsg);
					}
				}
				else
					Response.Write("<script language:javascript>alert('Ya se ha guardado un inventario con este número.');</script>");
			}
			else
				Response.Write("<script language:javascript>alert('No se ha escogido un inventario para guardar los items escogidos.');</script>");
		}

		protected void Revisar()
		{
            string prefijoInv;
            int numeroInv;

            if (ddlInventario.SelectedValue.Split('-').Length == 3)
            {
                prefijoInv = (ddlInventario.SelectedValue.Split('-'))[0] + "-" + (ddlInventario.SelectedValue.Split('-'))[1];
                numeroInv = Convert.ToInt32((ddlInventario.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInv = (ddlInventario.SelectedValue.Split('-'))[0];
                numeroInv = Convert.ToInt32((ddlInventario.SelectedValue.Split('-'))[1].Trim());
            }

			int contar=0;

			DataSet hijos = new DataSet();
			
			// Esta función guarda la tabla de los ítems.
			dgitems.Visible=true;
			
			ArrayList pos =new ArrayList();
			ArrayList pos1=new ArrayList();
			
			string usuario=HttpContext.Current.User.Identity.Name.ToLower();
			
			if(ddlInventario.SelectedValue != "0")
			{
				if(prefijoInv != "")
				{
					InventarioFisico inst = new InventarioFisico(prefijoInv,numeroInv);

					// Borramos las tarjetas de conteo generadas previamente.
					inst.ItemsInventario.Clear();
					
					for (int x=0;x<dgitems.Items.Count;x++)
					{
						if(dgitems.Items[x].Cells[0].Text != "&nbsp;")
							inst.ItemsInventario.Add(new ItemsInventarioFisico(
								Convert.ToInt32(ValidarCampoEntero(dgitems.Items[x].Cells[8].Text)),
								dgitems.Items[x].Cells[7].Text,
								ValidarCampoCadena(dgitems.Items[x].Cells[3].Text),
								dgitems.Items[x].Cells[4].Text,
								Convert.ToDouble(ValidarCampoEntero(dgitems.Items[x].Cells[5].Text)),
								Convert.ToDouble(ValidarCampoEntero(dgitems.Items[x].Cells[6].Text)),
								ValidarCampoCadena(dgitems.Items[x].Cells[1].Text),
								string.Empty));
					}

					if(inst.ActualizarItemsInventario())
					{
						Response.Write("<script>alert('Los ítems y cantidades del inventario físico han sido actualizados.');</script>");
						//Mostrar cuales reimprime
						
						dgitems.Visible=false;
						btnSave.Visible=false;
						
						Response.Redirect(indexPage + "?path="+Request.QueryString["path"]+"&process=Inventarios.InventarioFisico&rmpI="+inst.numeroTarjetaActDesde+"&rmpF="+inst.numeroTarjetaActHasta);
					}
					else
					{
						Response.Write(inst.ProcessMsg);
					}
				}
				else
					Response.Write("<script language:javascript>alert('Ya se ha guardado un inventario con este número.');</script>");
			}
			else
				Response.Write("<script language:javascript>alert('No se ha escogido un inventario para actualizar.');</script>");
		}

		protected void generar_Inventario(object sender, EventArgs e)
		{
            
               
			string prefijoInv;
			int numeroInv; 
            
            if (ddlInventario.SelectedValue.Split('-').Length == 3)
            {
                prefijoInv = (ddlInventario.SelectedValue.Split('-'))[0] + "-" + (ddlInventario.SelectedValue.Split('-'))[1];
                numeroInv = Convert.ToInt32((ddlInventario.SelectedValue.Split('-'))[2]);

            }
            else
            {
               prefijoInv = (ddlInventario.SelectedValue.Split('-'))[0];
			   numeroInv = Convert.ToInt32((ddlInventario.SelectedValue.Split('-'))[1].Trim());
            }

			string simbolo="";
            ArrayList itmsUsados = new ArrayList();
            string cantItm = "";
			if(ddlProceso.SelectedValue=="2")simbolo="=";

			string alm="";
			int contador = 0;
			int numero = 0;
	
			// Esta funcion genera los items que se encuentran almacenados en un determinado almacen.
			this.Preparar_Tabla_Items();

			DataSet items = new DataSet();
			DataSet padres = new DataSet();

			DBFunctions.Request(items,IncludeSchema.NO,"Select * from PUBICACIONITEM where palm_almacen='"+ddlAlmacenes.SelectedValue+"' order by pubi_codigo asc");
			
			if(ddlAlmacenes.SelectedIndex > 0)
                DBFunctions.Request(padres, IncludeSchema.NO, "Select pubi_codigo from PUBICACIONITEM WHERE (PUBI_UBICESPACIAL = '' or PUBI_UBICESPACIAL is null) and pubi_codpad is null and palm_almacen='" + ddlAlmacenes.SelectedValue + "'");
			else
                DBFunctions.Request(padres, IncludeSchema.NO, "Select pubi_codigo from PUBICACIONITEM WHERE (PUBI_UBICESPACIAL = '' or PUBI_UBICESPACIAL is null) AND pubi_codpad is null");
			
			if(padres.Tables.Count > 0)
			{
				#region Opción por Ubicaciones
				switch (ddlCriterioSeleccionUbicaciones.SelectedValue)
				{
					case "1":
					{
						if(lbrango.Items.Count!=0 && lbrangof.Items.Count!=0)
						{
							DataSet rangob = new DataSet();

							int nu = -1, nu1  = -1;

							if(lbrango.SelectedIndex != -1)
								nu = Convert.ToInt32(lbrango.SelectedValue);
							else
								nu = Convert.ToInt32(lbrango.Items[0].Value);

							if(lbrangof.SelectedIndex != -1)
								nu1 = Convert.ToInt32(lbrangof.SelectedValue);
							else
								nu1 = Convert.ToInt32(lbrangof.Items[lbrango.Items.Count-1].Value);

							int r = Convert.ToInt32(DBFunctions.SingleData("select pubi_codpad from pubicacionitem where pubi_codigo="+nu+""));
							int l = Convert.ToInt32(DBFunctions.SingleData("select pubi_codpad from pubicacionitem where pubi_codigo="+nu1+""));

							if(r==l)
							{
								DBFunctions.Request(rangob,IncludeSchema.NO,"select * from (Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+r+") a where a.pubi_codigo between "+nu+" and "+nu1+"");	
						
								DataSet subpadres = new DataSet();
						
								//sacar los hijos del almacen (subpadres)
								//recorrer todos los hijos de los subpadres
								if(rangob.Tables.Count > 0)
								{
									if(rangob.Tables[0].Rows.Count!=0)
									{
										#region Ingresar Datos Operaciones
										numero = 1;

										for (int i=0;i<rangob.Tables[0].Rows.Count;i++)
										{ 
											this.ingresar_datos_Operaciones("","","<p style=\"COLOR: red\">"+rangob.Tables[0].Rows[i][1].ToString()+"</p>","","","","","","");
								
											DataSet hijos = new DataSet();
								
											DBFunctions.Request(hijos,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial,pubi_codpad from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+rangob.Tables[0].Rows[i][0]+" order by pubi_nombre asc ");
								
											if(hijos.Tables.Count > 0)
											{
												for (int j=0;j<hijos.Tables[0].Rows.Count;j++)
												{
													this.ingresar_datos_Operaciones("","","<p style=\"COLOR: red\">"+hijos.Tables[0].Rows[j][1].ToString()+"</p>","","","","","","");		
									
													DataSet items2 = new DataSet();
									
													DBFunctions.Request(items2,IncludeSchema.NO,string.Format(sqlItemsUbicaciones,hijos.Tables[0].Rows[j][0].ToString(),simbolo,ddlAlmacenes.SelectedValue));
							
													if(items2.Tables.Count > 0)
													{
														for (int y=0;y<items2.Tables[0].Rows.Count;y++)
														{
															alm=items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString();
										
															if(Convert.ToInt32(items2.Tables[0].Rows[y]["PUBI_CODIGO"])!=0)
															{
                                                                cantItm = items2.Tables[0].Rows[y]["MSAL_CANTACTUAL"].ToString();
                                                                if (itmsUsados.Contains(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString()))
                                                                    cantItm = "0";
                                                                else
                                                                    itmsUsados.Add(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString());
                                                                this.ingresar_datos_Operaciones(
																	numero.ToString(),
																	items2.Tables[0].Rows[y]["PUBI_NOMBRE"].ToString(),
																	items2.Tables[0].Rows[y]["MITE_CODIGOEDITADO"].ToString(),
																	items2.Tables[0].Rows[y]["MITE_NOMBRE"].ToString(),
																	items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString(),
																	cantItm,
																	items2.Tables[0].Rows[y]["MSAL_COSTPROM"].ToString(),
																	items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString(),
																	items2.Tables[0].Rows[y]["PUBI_CODIGO"].ToString()
																	);		
																numero++;
															}
														}
													}
												}
											}
										}
										#endregion

										btnSave.Visible=true;
									}
									else
									{
										Response.Write("<script language:javascript>alert('El rango esta incorrecto por favor verifique.');</script>");
							
										contador=1;
									}
								}
							}
							else
							{
								if(nu<nu1)
								{
									ArrayList cadena=new ArrayList();

									for(int q=0;q<padres.Tables[0].Rows.Count;q++)
									{
										if(nu1>Convert.ToInt32(padres.Tables[0].Rows[q][0]))
											cadena.Add(Convert.ToString(padres.Tables[0].Rows[q][0]));
									}

									#region Ingresar Datos Operaciones
									for(int m=0;m<cadena.Count;m++)
									{
										DataSet subpadres = new DataSet();
								
										//sacar los hijos del almacen (subpadres)
										DBFunctions.Request(subpadres,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+cadena[m]+"");
								
										numero = 1;
	
										//recorrer todos los hijos de los subpadres
										if(subpadres.Tables.Count > 0)
										{
											for (int i=0;i<subpadres.Tables[0].Rows.Count;i++)
											{
												this.ingresar_datos_Operaciones("","","<p style=\"COLOR: red\">"+subpadres.Tables[0].Rows[i][1].ToString()+"</p>","","","","","","");
									
												DataSet hijos = new DataSet();
									
												DBFunctions.Request(hijos,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+subpadres.Tables[0].Rows[i][0].ToString()+" order by pubi_nombre asc ");
									
												if(hijos.Tables.Count > 0)
												{
													for (int j=0;j<hijos.Tables[0].Rows.Count;j++)
													{
														this.ingresar_datos_Operaciones("","","<p style=\"COLOR: red\">"+hijos.Tables[0].Rows[j][1].ToString()+"</p>","","","","","","");
										
														DataSet items2 = new DataSet();
										
														DBFunctions.Request(items2,IncludeSchema.NO,string.Format(sqlItemsUbicaciones,hijos.Tables[0].Rows[j][0].ToString(),simbolo,ddlAlmacenes.SelectedValue));
									
														if (items2.Tables.Count>0)
														{
															for (int y=0;y<items2.Tables[0].Rows.Count;y++)
															{
																alm=items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString();
											
																if(Convert.ToInt32(items2.Tables[0].Rows[y]["PUBI_CODIGO"])!=0)
																{
                                                                    cantItm = items2.Tables[0].Rows[y]["MSAL_CANTACTUAL"].ToString();
                                                                    if (itmsUsados.Contains(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString()))
                                                                        cantItm = "0";
                                                                    else
                                                                        itmsUsados.Add(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString());
                                                                    this.ingresar_datos_Operaciones(
																		numero.ToString(),
																		items2.Tables[0].Rows[y]["PUBI_NOMBRE"].ToString(),
																		items2.Tables[0].Rows[y]["MITE_CODIGOEDITADO"].ToString(),
																		items2.Tables[0].Rows[y]["MITE_NOMBRE"].ToString(),
																		items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString(),
																		cantItm,
																		items2.Tables[0].Rows[y]["MSAL_COSTPROM"].ToString(),
																		items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString(),
																		items2.Tables[0].Rows[y]["PUBI_CODIGO"].ToString()
																		);		
																	numero++;
																}
															}
														}
													}
												}
											}
										}
									}
									#endregion

									btnSave.Visible=true;
								}
								else
								{
									Response.Write("<script language:javascript>alert('El rango esta incorrecto por favor verifique.');</script>");
							
									contador=1;
								}
							}
						}
					}
						break;
					case "2":
					{
						ArrayList ubicacionesChequeadas = new ArrayList();

						for(int i = 0; i < cblUbicaciones.Items.Count; i++)
						{
							if (cblUbicaciones.Items[i].Selected == true)
								ubicacionesChequeadas.Add(cblUbicaciones.Items[i].Value);
						}

						if(ubicacionesChequeadas.Count > 0)
						{
							numero = 1;

							for (int j=0;j<ubicacionesChequeadas.Count;j++)
							{
								string[] valores = ((string)ubicacionesChequeadas[j]).Split('-');

								string pubi_codigo = valores[0];
								string pubi_nombre = valores[1];

								this.ingresar_datos_Operaciones("","","<p style=\"COLOR: red\">"+pubi_nombre+"</p>","","","","","","");		
									
								DataSet items2 = new DataSet();

                                DBFunctions.Request(items2, IncludeSchema.NO, string.Format(sqlItemsUbicaciones, pubi_codigo, simbolo, ddlAlmacenes.SelectedValue));
							
								if(items2.Tables.Count > 0)
								{
									for (int y=0;y<items2.Tables[0].Rows.Count;y++)
									{
										alm=items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString();
										
										if(Convert.ToInt32(items2.Tables[0].Rows[y]["PUBI_CODIGO"])!=0)
										{
                                            cantItm = items2.Tables[0].Rows[y]["MSAL_CANTACTUAL"].ToString();
                                            if (itmsUsados.Contains(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString()))
                                                cantItm = "0";
                                            else
                                                itmsUsados.Add(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString());
                                            this.ingresar_datos_Operaciones(
												numero.ToString(),
												items2.Tables[0].Rows[y]["PUBI_NOMBRE"].ToString(),
												items2.Tables[0].Rows[y]["MITE_CODIGOEDITADO"].ToString(),
												items2.Tables[0].Rows[y]["MITE_NOMBRE"].ToString(),
												items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString(),
												cantItm,
												items2.Tables[0].Rows[y]["MSAL_COSTPROM"].ToString(),
												items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString(),
												items2.Tables[0].Rows[y]["PUBI_CODIGO"].ToString()
												);		
											numero++;
										}
									}
								}
							}

							if(numero == 1)
							{
								Response.Write("<script language:javascript>alert('No se encontraron ítems dentro de las ubicaciones seleccionadas con cantidades en existencias.');</script>");

								contador = 1;
							}
						}
						else
						{
							Response.Write("<script language:javascript>alert('Debe seleccionar ubicaciones para generar las tarjetas de conteo.');</script>");

							contador = 1;
						}
					}
						break;

                    case "3":   // Todas las ubicaciones
					{
						if(padres.Tables[0].Rows.Count!=0)
						{
							#region Ingresar Datos Operaciones
							numero = 1;
                            DataSet subpadres = new DataSet();
                            DataSet hijos = new DataSet();
                            DataSet items2 = new DataSet();

                            DBFunctions.Request(items2, IncludeSchema.NO, string.Format(sqlGralItemsUbicaciones, simbolo, ddlAlmacenes.SelectedValue));

                            if (items2.Tables.Count > 0)
                            {
                                for (int y = 0; y < items2.Tables[0].Rows.Count; y++)
                                {
                                    alm = items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString();

                                    if (Convert.ToInt32(items2.Tables[0].Rows[y]["PUBI_CODIGO"]) != 0)
                                    {
                                        cantItm = items2.Tables[0].Rows[y]["MSAL_CANTACTUAL"].ToString();
                                        if (itmsUsados.Contains(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString()))
                                            cantItm = "0";
                                        else
                                            itmsUsados.Add(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString());
                                        if (y > 1 && items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString() == items2.Tables[0].Rows[y - 1]["MITE_CODIGO"].ToString())
                                            cantItm = "0";
                                        this.ingresar_datos_Operaciones(
                                            numero.ToString(),
                                            items2.Tables[0].Rows[y]["PUBI_NOMBRE"].ToString(),
                                            items2.Tables[0].Rows[y]["MITE_CODIGOEDITADO"].ToString(),
                                            items2.Tables[0].Rows[y]["MITE_NOMBRE"].ToString(),
                                            items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString(),
                                            cantItm,
                                            items2.Tables[0].Rows[y]["MSAL_COSTPROM"].ToString(),
                                            items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString(),
                                            items2.Tables[0].Rows[y]["PUBI_CODIGO"].ToString()
                                            );
                                        numero++;
                                    }
                                }
                            }
							#endregion

							btnSave.Visible=true;
						}
						else
						{
							//Response.Write("<script language:javascript>alert('No se encuentran registros de algún item.');</script>");
                            Utils.MostrarAlerta(Response, "No se encuentran registros de algún item.");
							contador = 1;
						}
					}
						break;
				}
			
				#endregion
			}

			// Genera tarjetas de conteo que no tienen ubicaciones.
			if(chbItemsSinUbicacion.Checked)
			{
				this.ingresar_datos_Operaciones("","","<p style=\"COLOR: red\">Ítems Sin Ubicación</p>","","","","","","");		
									
				DataSet items2 = new DataSet();
									
				DBFunctions.Request(items2,IncludeSchema.NO,string.Format(sqlItemsSinUbicaciones,ddlAlmacenes.SelectedValue,simbolo));
							
				if(items2.Tables.Count > 0)
				{
					for (int y=0;y<items2.Tables[0].Rows.Count;y++)
					{
						alm = items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString();
                        cantItm = items2.Tables[0].Rows[y]["MSAL_CANTACTUAL"].ToString();
                        if (itmsUsados.Contains(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString()))
                            cantItm = "0";
                        else
                            itmsUsados.Add(items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString());
						this.ingresar_datos_Operaciones(
							numero.ToString(),
							string.Empty,
							items2.Tables[0].Rows[y]["MITE_CODIGOEDITADO"].ToString(),
							items2.Tables[0].Rows[y]["MITE_NOMBRE"].ToString(),
							items2.Tables[0].Rows[y]["PALM_ALMACEN"].ToString(),
							cantItm,
							items2.Tables[0].Rows[y]["MSAL_COSTPROM"].ToString(),
							items2.Tables[0].Rows[y]["MITE_CODIGO"].ToString(),
							string.Empty
							);		
						numero++;
					}
				}
			}

			if(contador != 1)
			{
				#region Código candidato a eliminación
				/*
				//Generar las tarjetas de alta
				this.ingresar_datos_Operaciones("","","<p style=\"COLOR: red\">Tarjetas de Alta</p>","","","","","","");

				for(int h=1;h<=Convert.ToInt32(altas.Text);h++)
					this.ingresar_datos_Operaciones(h.ToString(),"Tajeta de Alta","Tarjeta de Alta","",alm,"","","","");
				*/
				#endregion

				dgitems.Visible = btnSave.Visible = btnCancelar.Visible = true;
				btnRealizar.Visible = ddlProceso.Enabled = ddlInventario.Enabled = ddlAlmacenes.Enabled = ddlCriterioSeleccionUbicaciones.Enabled = cblUbicaciones.Enabled = lbrango.Enabled = lbrangof.Enabled = false;
			}
			else
			{
				dgitems.DataSource = null;
				dgitems.Visible = false;
			}

			MostrarControlesSeleccionUbicaciones();
        }

        public void ddlProceso_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            if (ddlProceso.SelectedValue == "1")
                btnRealizar.Text = "Generar";
            else if (ddlProceso.SelectedValue == "2")
                    btnRealizar.Text = "Actualizar";
        }
		
		public void ddlAlmacenes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(ddlAlmacenes.SelectedValue != String.Empty)
			{
				lbrango.Items.Clear();
				lbrangof.Items.Clear();
				cblUbicaciones.Items.Clear();

				DataSet papa = new DataSet();
			
				DBFunctions.Request(papa,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre from DBXSCHEMA.PUBICACIONITEM WHERE PUBI_UBICESPACIAL IS NULL and pubi_codpad is null and palm_almacen='"+ddlAlmacenes.SelectedValue+"'");
			
				if(papa.Tables[0].Rows.Count!=0)
				{
					// Carga los estantes en los cuadros de lista del intervalo de ubicaciones.
					for (int p=0;p<papa.Tables[0].Rows.Count;p++)
					{
						DataSet subpa = new DataSet();

						DBFunctions.Request(subpa,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+papa.Tables[0].Rows[p][0].ToString()+"");
						
						for (int i=0;i<subpa.Tables[0].Rows.Count;i++)
						{
							lbrango.Items.Add(new ListItem("["+subpa.Tables[0].Rows[i][1]+"] - ["+papa.Tables[0].Rows[p][1]+"]",subpa.Tables[0].Rows[i][0].ToString()));
							lbrangof.Items.Add(new ListItem("["+subpa.Tables[0].Rows[i][1]+"] - ["+papa.Tables[0].Rows[p][1]+"]",subpa.Tables[0].Rows[i][0].ToString()));
						}
					}

					// Cargar todas las ubicaciones del almacén seleccionado.
					DataSet padres = new DataSet();

					if(ddlAlmacenes.SelectedIndex > 0)
						DBFunctions.Request(padres,IncludeSchema.NO,"Select pubi_codigo from PUBICACIONITEM WHERE PUBI_UBICESPACIAL IS NULL and pubi_codpad is null and palm_almacen='"+ddlAlmacenes.SelectedValue+"'");
					
					for (int p=0;p<padres.Tables[0].Rows.Count;p++)
					{
						DataSet subpadres = new DataSet();
								
						//sacar los hijos del almacen (subpadres)
						DBFunctions.Request(subpadres,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+padres.Tables[0].Rows[p][0].ToString()+"");
								
						//recorrer todos los hijos de los subpadres
						for (int i=0;i<subpadres.Tables[0].Rows.Count;i++)
						{
							DataSet hijos = new DataSet();
									
							DBFunctions.Request(hijos,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+subpadres.Tables[0].Rows[i][0].ToString()+" order by pubi_nombre asc");
									
							for (int j=0;j<hijos.Tables[0].Rows.Count;j++)
								cblUbicaciones.Items.Add(new ListItem("["+hijos.Tables[0].Rows[j][1].ToString()+"] - ["+subpadres.Tables[0].Rows[i][1].ToString()+"]",hijos.Tables[0].Rows[j][0].ToString()+"-"+hijos.Tables[0].Rows[j][1].ToString()));
						}
					}

					btnRealizar.Visible=true;
				}
				else
				{
					Response.Write("<script language:javascript>alert('No se encuentran estantes en esta bodega.');</script>");
				}
			}

			// Se altera el estilo de las divisiones para el cuadro de selección
			// porque el estado del estilo por defecto es 'none'.
			div_lbAlmacenes.Attributes.Add("style","display: ");
			div_ddlAlmacenes.Attributes.Add("style","display: ");

			MostrarControlesSeleccionUbicaciones();
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage + "?path="+Request.QueryString["path"]+"&process=Inventarios.InventarioFisico");
		}
		
		#endregion
		
		#region Metodos
		private string ValidarCampoEntero(string valor)
		{
			string valorValidado = valor;

			if(valor == "&nbsp;")
				valorValidado = "-1";

			return valorValidado;
		}

		private string ValidarCampoCadena(string valor)
		{
			string valorValidado = valor;

			if(valor == "&nbsp;")
				valorValidado = string.Empty;

			return valorValidado;
		}

		protected void Preparar_Tabla_Items()
		{
			tablaItems = new DataTable();
			tablaItems.Columns.Add(new DataColumn("NUMERO",System.Type.GetType("System.String")));//0
			tablaItems.Columns.Add(new DataColumn("PUBI_NOMBRE",System.Type.GetType("System.String")));//0
			tablaItems.Columns.Add(new DataColumn("MITE_CODIGOEDITADO",System.Type.GetType("System.String"))); //1
			tablaItems.Columns.Add(new DataColumn("MITE_NOMBRE",System.Type.GetType("System.String")));//2
			tablaItems.Columns.Add(new DataColumn("PALM_ALMACEN",System.Type.GetType("System.String")));//3
			tablaItems.Columns.Add(new DataColumn("MSAL_CANTACTUAL",System.Type.GetType("System.String")));//4
			tablaItems.Columns.Add(new DataColumn("MSAL_COSTPROM",System.Type.GetType("System.String")));//5
			tablaItems.Columns.Add(new DataColumn("MITE_CODIGO",System.Type.GetType("System.String"))); //1
			tablaItems.Columns.Add(new DataColumn("PUBI_CODIGO",System.Type.GetType("System.String"))); //1
		}

		protected void ingresar_datos_Operaciones(
			string numero,
			string pubi_nombre,
			string mite_codigoEditado,
			string mite_nombre,
			string palm_almacen,
			string msal_cantactual,
			string msal_costprom,
			string mite_codigo,
			string pubi_codigo)
		{
			DataRow fila = tablaItems.NewRow();

			fila["NUMERO"]=numero;
			fila["PUBI_NOMBRE"]=pubi_nombre;
			fila["MITE_CODIGOEDITADO"]=mite_codigoEditado;
			fila["MITE_NOMBRE"]=mite_nombre;
			fila["PALM_ALMACEN"]=palm_almacen;
			fila["MSAL_CANTACTUAL"]=msal_cantactual;
			fila["MSAL_COSTPROM"]=msal_costprom;
			fila["MITE_CODIGO"]=mite_codigo;
			fila["PUBI_CODIGO"]=pubi_codigo;

			double cantidad = 0;

			try
			{
				cantidad = Convert.ToDouble(msal_cantactual);
			}
			catch { }
			
			tablaItems.Rows.Add(fila);

			dgitems.DataSource= tablaItems;
			dgitems.DataBind();

			DatasToControls.Aplicar_Formato_Grilla(dgitems);
			DatasToControls.JustificacionGrilla(dgitems,tablaItems);
		}

		private void MostrarControlesSeleccionUbicaciones()
		{
			switch(ddlCriterioSeleccionUbicaciones.SelectedValue)
			{
				case "-1":
					div_lbUbicaciones.Attributes.Add("style","display: none");
					div_cblUbicaciones.Attributes.Add("style","DISPLAY: none; OVERFLOW: auto; WIDTH: 320px; HEIGHT: 300px");
					div_lbrango.Attributes.Add("style","display: none");
					div_ran.Attributes.Add("style","display: none");
					div_lbrangof.Attributes.Add("style","display: none");
					div_ranf.Attributes.Add("style","display: none");
					break;
				case "1":
					div_lbUbicaciones.Attributes.Add("style","display: none");
					div_cblUbicaciones.Attributes.Add("style","DISPLAY: none; OVERFLOW: auto; WIDTH: 320px; HEIGHT: 300px");
					div_lbrango.Attributes.Add("style","display: ");
					div_ran.Attributes.Add("style","display: ");
					div_lbrangof.Attributes.Add("style","display: ");
					div_ranf.Attributes.Add("style","display: ");
					break;
				case "2":
					div_lbUbicaciones.Attributes.Add("style","display: ");
					div_cblUbicaciones.Attributes.Add("style","DISPLAY: ; OVERFLOW: auto; WIDTH: 320px; HEIGHT: 300px");
					div_lbrango.Attributes.Add("style","display: none");
					div_ran.Attributes.Add("style","display: none");
					div_lbrangof.Attributes.Add("style","display: none");
					div_ranf.Attributes.Add("style","display: none");
					break;
				case "3":
					div_lbUbicaciones.Attributes.Add("style","display: none");
					div_cblUbicaciones.Attributes.Add("style","DISPLAY: none; OVERFLOW: auto; WIDTH: 320px; HEIGHT: 300px");
					div_lbrango.Attributes.Add("style","display: none");
					div_ran.Attributes.Add("style","display: none");
					div_lbrangof.Attributes.Add("style","display: none");
					div_ranf.Attributes.Add("style","display: none");
					break;
			}
		}

		#endregion

		#region Metodos Ajax

		[Ajax.AjaxMethod]
		public string CargarTipoInventarioFisicoUbicacion(string prefijoInv, int numeroInv)
		{
			return DBFunctions.SingleData("Select tifu_codigo from dbxschema.minventariofisico where pdoc_codigo = '"+prefijoInv+"' and minf_numeroinv = "+numeroInv.ToString());
		}

		#endregion

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
