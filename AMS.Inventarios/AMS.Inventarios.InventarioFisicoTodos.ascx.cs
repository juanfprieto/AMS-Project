
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using System.Collections;
using AMS.Tools;

namespace AMS.Inventarios
{
	/// <summary>
	///		Descripción breve de AMS_Inventarios_InventarioFisicoTodos.
	/// </summary>
	public partial class AMS_Inventarios_InventarioFisicoTodos : System.Web.UI.UserControl
	{
		protected DataTable tablaItems;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
                Utils.MostrarAlerta(Response, "El Personal Para el Inventario Fisico Ha sido Guardado");
				DatasToControls bind = new DatasToControls();
			}
		}
		protected void guardar_tabla(object sender, EventArgs e)
		{
			string posicion="";
			int grupo=Convert.ToInt32(Request.QueryString["grupoinv"]);	
			// Esta funcion guarda la tabla d los items
			dgitems.Visible=true;
			ArrayList pos=new ArrayList();
			ArrayList pos1=new ArrayList();
			string usuario=HttpContext.Current.User.Identity.Name.ToLower();
			int num=Convert.ToInt32(DBFunctions.SingleData("select pdoc_ultidocu from dbxschema.pdocumento where pdoc_codigo='INVF'"));
			string [] posiciones;
			
			for (int x=0;x<dgitems.Items.Count;x++)
			{
				string pi=dgitems.Items[x].Cells[0].Text.Substring(0,1);
				if(pi.Equals("<"))
				{
					posiciones=dgitems.Items[x].Cells[0].Text.Split('>');
					posiciones=posiciones[1].Split('<');
					posicion=posiciones[0];
					
				}

				DBFunctions.NonQuery("insert into dbxschema.dinventariofisico values ('INVF',"+num+",0,default,"+dgitems.Items[x].Cells[0].Text+",'"+dgitems.Items[x].Cells[1].Text+"','"+dgitems.Items[x].Cells[2].Text+"','"+dgitems.Items[x].Cells[3].Text+"',"+dgitems.Items[x].Cells[4].Text+","+dgitems.Items[x].Cells[5].Text+",0,0,0,null,null,'"+posicion+"',"+grupo+")");
			}
            Utils.MostrarAlerta(Response, "LA LISTA DE ITEMS ESCOGIDOS FUE GUARDADA Y ESTA LISTA PARA REALIZAR EL CONTEO");
			dgitems.Visible=false;
			
			
		}
		protected void generar_Inventario(object sender, EventArgs e)
		{
			int grupo1=Convert.ToInt32(Request.QueryString["grupoinv"]);	
			int num1=Convert.ToInt32(DBFunctions.SingleData("select pdoc_ultidocu from dbxschema.pdocumento where pdoc_codigo='INVF'"));
			string alm="";		
	
			// Esta funcion genera los items que se encuentran almacenados en un determinado almacen
			this.Preparar_Tabla_Items();
			DataSet items = new DataSet();
			DataSet padres = new DataSet();
				
			DBFunctions.Request(padres,IncludeSchema.NO,"Select pubi_codigo from DBXSCHEMA.PUBICACIONITEM WHERE PUBI_UBICESPACIAL IS NULL and pubi_codpad is null ");
			
			//SACAR LOS PADRES DEL ALMACEN
			if(ddltarjeta.SelectedValue.Equals("2"))
			{
				if(lbrango.Items.Count!=0 && lbrangof.Items.Count!=0)
				{  
					DataSet rangob = new DataSet();
					string []g=lbrango.SelectedValue.Split('-');
					string []t=lbrangof.SelectedValue.Split('-');
					int nu=Convert.ToInt32(g[2].ToString());
					int nu1=Convert.ToInt32(t[2].ToString());
					int r=Convert.ToInt32(DBFunctions.SingleData("select pubi_codpad from dbxschema.pubicacionitem where pubi_codigo="+nu+""));
					int l=Convert.ToInt32(DBFunctions.SingleData("select pubi_codpad from dbxschema.pubicacionitem where pubi_codigo="+nu1+""));
			
					if(r==l)
					{
						DBFunctions.Request(rangob,IncludeSchema.NO,"select * from (Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+r+") a where a.pubi_codigo between "+nu+" and "+nu1+"");	
						DataSet subpadres = new DataSet();
						//sacar los hijos del almacen (subpadres)
						//recorrer todos los hijos de los subpadres
						if(rangob.Tables[0].Rows.Count!=0)
						{
							for (int i=0;i<rangob.Tables[0].Rows.Count;i++)
							{ 
								this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+rangob.Tables[0].Rows[i][1].ToString()+"</p>","-------------------","------------------------------------------","-------------","-----------","------------------");
								DataSet hijos = new DataSet();
								DBFunctions.Request(hijos,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial,pubi_codpad from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+rangob.Tables[0].Rows[i][0]+" order by pubi_nombre asc ");
								for (int j=0;j<hijos.Tables[0].Rows.Count;j++)
								{
									this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+hijos.Tables[0].Rows[j][1].ToString()+"</p>","-------------------","------------------------------------------","--------------","-----------","------------------");		
									DataSet items2 = new DataSet();
									DBFunctions.Request(items2,IncludeSchema.NO,"Select * from DBXSCHEMA.VINVENTARIOS_ITEMSUBISALDO where pubi_codigo="+hijos.Tables[0].Rows[j][0].ToString()+"");
						
									for (int y=0;y<items2.Tables[0].Rows.Count;y++)
									{
										alm=items2.Tables[0].Rows[y][6].ToString();
										if(Convert.ToInt32(items2.Tables[0].Rows[y][0])!=0)
										{
											this.ingresar_datos_Operaciones(items2.Tables[0].Rows[y][0].ToString(),items2.Tables[0].Rows[y][1].ToString(),items2.Tables[0].Rows[y][2].ToString(),items2.Tables[0].Rows[y][6].ToString(),items2.Tables[0].Rows[y][7].ToString(),items2.Tables[0].Rows[y][8].ToString());		
										}
									}
								}
							}
							btnSave.Visible=true;
						}
						else
						{
                            Utils.MostrarAlerta(Response, "EL RANGO ESTA INCORRECTO POR FAVOR VERIFIQUE");
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
								{
									cadena.Add(Convert.ToString(padres.Tables[0].Rows[q][0]));
								}
							}
						
							for(int m=0;m<cadena.Count;m++)
							{
								DataSet subpadres = new DataSet();
								//sacar los hijos del almacen (subpadres)
								DBFunctions.Request(subpadres,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+cadena[m]+"");
								//recorrer todos los hijos de los subpadres
								for (int i=0;i<subpadres.Tables[0].Rows.Count;i++)
								{
									this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+subpadres.Tables[0].Rows[i][1].ToString()+"</p>","-------------------","------------------------------------------","-------------","-----------","------------------");
									DataSet hijos = new DataSet();
									DBFunctions.Request(hijos,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+subpadres.Tables[0].Rows[i][0].ToString()+" order by pubi_nombre asc ");
									for (int j=0;j<hijos.Tables[0].Rows.Count;j++)
									{
										this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+hijos.Tables[0].Rows[j][1].ToString()+"</p>","-------------------","------------------------------------------","--------------","-----------","------------------");		
										DataSet items2 = new DataSet();
										DBFunctions.Request(items2,IncludeSchema.NO,"Select * from DBXSCHEMA.VINVENTARIOS_ITEMSUBISALDO where pubi_codigo="+hijos.Tables[0].Rows[j][0].ToString()+"");
								
										for (int y=0;y<items2.Tables[0].Rows.Count;y++)
										{
											alm=items2.Tables[0].Rows[y][6].ToString();
											if(Convert.ToInt32(items2.Tables[0].Rows[y][0])!=0)
											{
												this.ingresar_datos_Operaciones(items2.Tables[0].Rows[y][0].ToString(),items2.Tables[0].Rows[y][1].ToString(),items2.Tables[0].Rows[y][2].ToString(),items2.Tables[0].Rows[y][6].ToString(),items2.Tables[0].Rows[y][7].ToString(),items2.Tables[0].Rows[y][8].ToString());		
											}
										}
									}
								}
							}
							btnSave.Visible=true;
						}
						else
						{
                            Utils.MostrarAlerta(Response, "EL RANGO ESTA INCORRECTO POR FAVOR VERIFIQUE");
						}
					}
				}
				else
				{
					//AQUI SE SACAN TODOS LOS PADRES DE TODOS LOS ALMACENES Y SE GENERA TODA LA CONSUTLA SOBRE TODOS LOS ALMACENES
					if(cbalmacen.Checked==true)
					{
						if(padres.Tables[0].Rows.Count!=0)
						{
							for (int p=0;p<padres.Tables[0].Rows.Count;p++)
							{
								DataSet subpadres = new DataSet();
								//sacar los hijos del almacen (subpadres)
								DBFunctions.Request(subpadres,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+padres.Tables[0].Rows[p][0].ToString()+"");
								//recorrer todos los hijos de los subpadres
								for (int i=0;i<subpadres.Tables[0].Rows.Count;i++)
								{
									this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+subpadres.Tables[0].Rows[i][1].ToString()+"</p>","-------------------","------------------------------------------","-------------","-----------","------------------");
									DataSet hijos = new DataSet();
									DBFunctions.Request(hijos,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+subpadres.Tables[0].Rows[i][0].ToString()+" order by pubi_nombre asc ");
									for (int j=0;j<hijos.Tables[0].Rows.Count;j++)
									{
										this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+hijos.Tables[0].Rows[j][1].ToString()+"</p>","-------------------","------------------------------------------","--------------","-----------","------------------");		
										DataSet items2 = new DataSet();
										DBFunctions.Request(items2,IncludeSchema.NO,"Select * from DBXSCHEMA.VINVENTARIOS_ITEMSUBISALDO where pubi_codigo="+hijos.Tables[0].Rows[j][0].ToString()+"");
										for (int y=0;y<items2.Tables[0].Rows.Count;y++)
										{
											alm=items2.Tables[0].Rows[y][6].ToString();
											if(Convert.ToInt32(items2.Tables[0].Rows[y][0])!=0)
											{
												this.ingresar_datos_Operaciones(items2.Tables[0].Rows[y][0].ToString(),items2.Tables[0].Rows[y][1].ToString(),items2.Tables[0].Rows[y][2].ToString(),items2.Tables[0].Rows[y][6].ToString(),items2.Tables[0].Rows[y][7].ToString(),items2.Tables[0].Rows[y][8].ToString());		
											}
										}
									}
								}
							}
							btnSave.Visible=true;
						}
						else
						{
                            Utils.MostrarAlerta(Response, "NO SE ENCUENTRAN REGISTROS DE ALGUN ITEM");
						}
					}
				}
			}// aqui temrina si se desea generar las tarjetas por ubicacion
			else
			{
				if(ddltarjeta.SelectedValue.Equals("1"))
				{
					if(padres.Tables[0].Rows.Count!=0)
					{
						for (int p=0;p<padres.Tables[0].Rows.Count;p++)
						{
							DataSet subpadres = new DataSet();
							//sacar los hijos del almacen (subpadres)
							DBFunctions.Request(subpadres,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+padres.Tables[0].Rows[p][0].ToString()+"");
							//recorrer todos los hijos de los subpadres
							for (int i=0;i<subpadres.Tables[0].Rows.Count;i++)
							{
								this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+subpadres.Tables[0].Rows[i][1].ToString()+"</p>","-------------------","------------------------------------------","-------------","-----------","------------------");
								DataSet hijos = new DataSet();
								DBFunctions.Request(hijos,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre,pubi_ubicespacial from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+subpadres.Tables[0].Rows[i][0].ToString()+" order by pubi_nombre asc ");
								for (int j=0;j<hijos.Tables[0].Rows.Count;j++)
								{
									this.ingresar_datos_Operaciones("<p style=\"COLOR: red\">"+hijos.Tables[0].Rows[j][1].ToString()+"</p>","-------------------","------------------------------------------","--------------","-----------","------------------");		
									DataSet items2 = new DataSet();
									DBFunctions.Request(items2,IncludeSchema.NO,"Select * from DBXSCHEMA.VINVENTARIOS_ITEMSUBISALDO where pubi_codigo="+hijos.Tables[0].Rows[j][0].ToString()+" order by mite_codigo asc");
									for (int y=0;y<items2.Tables[0].Rows.Count;y++)
									{
										alm=items2.Tables[0].Rows[y][6].ToString();
										if(Convert.ToInt32(items2.Tables[0].Rows[y][0])!=0)
										{
											this.ingresar_datos_Operaciones(items2.Tables[0].Rows[y][0].ToString(),items2.Tables[0].Rows[y][1].ToString(),items2.Tables[0].Rows[y][2].ToString(),items2.Tables[0].Rows[y][6].ToString(),items2.Tables[0].Rows[y][7].ToString(),items2.Tables[0].Rows[y][8].ToString());		
										}
									}
								}
							}
						}
					}
					btnSave.Visible=true;	
				}
			}
			for(int h=1;h<=Convert.ToInt32(altas.Text);h++)
			{
				this.ingresar_datos_Operaciones(""+h+"","Tarjeta de Alta","Tarjeta de Alta",""+alm+"","0","0");
			}
			dgitems.Visible=true;
		}
	
		protected void Preparar_Tabla_Items()
		{
			tablaItems = new DataTable();
			tablaItems.Columns.Add(new DataColumn("PUBI_CODIGO",System.Type.GetType("System.String")));//0
			tablaItems.Columns.Add(new DataColumn("MITE_CODIGO",System.Type.GetType("System.String"))); //1
			tablaItems.Columns.Add(new DataColumn("MITE_NOMBRE",System.Type.GetType("System.String")));//2
			tablaItems.Columns.Add(new DataColumn("PALM_ALMACEN",System.Type.GetType("System.String")));//3
			tablaItems.Columns.Add(new DataColumn("MSAL_CANTACTUAL",System.Type.GetType("System.String")));//4
			tablaItems.Columns.Add(new DataColumn("MSAL_COSTPROM",System.Type.GetType("System.String")));//5
			
		}
		protected void ingresar_datos_Operaciones(string pubi_codigo,string mite_codigo,string mite_nombre,string palm_almacen,string msal_cantactual,string msal_costprom)
		{
			DataRow fila = tablaItems.NewRow();
			fila["PUBI_CODIGO"]=pubi_codigo;
			fila["MITE_CODIGO"]=mite_codigo;
			fila["MITE_NOMBRE"]=mite_nombre;
			fila["PALM_ALMACEN"]=palm_almacen;
			fila["MSAL_CANTACTUAL"]=msal_cantactual;
			fila["MSAL_COSTPROM"]=msal_costprom;
			tablaItems.Rows.Add(fila);
			this.dgitems.DataSource= tablaItems;
			dgitems.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(dgitems);
			DatasToControls.JustificacionGrilla(dgitems,tablaItems);
		}
		public void ddltarjeta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(ddltarjeta.SelectedValue.Equals("2"))
			{
				ran.Visible=true;
				lbrango.Visible=true;
				ranf.Visible=true;
				lbrangof.Visible=true;
				DataSet almacen=new DataSet();
                DBFunctions.Request(almacen, IncludeSchema.NO, "select palm_almacen,palm_descripcion from dbxschema.palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");
				for(int i=0;i<almacen.Tables[0].Rows.Count;i++)
				{
					DataSet papa = new DataSet();
				
					DBFunctions.Request(papa,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre from DBXSCHEMA.PUBICACIONITEM WHERE PUBI_UBICESPACIAL IS NULL and pubi_codpad is null and palm_almacen='"+almacen.Tables[0].Rows[i][0]+"'");
					for (int p=0;p<papa.Tables[0].Rows.Count;p++)
					{
						DataSet subpa = new DataSet();
						//sacar los hijos del almacen (subpadres)
						DBFunctions.Request(subpa,IncludeSchema.NO,"Select pubi_codigo,pubi_nombre from DBXSCHEMA.PUBICACIONITEM WHERE pubi_codpad="+papa.Tables[0].Rows[p][0].ToString()+"");
						//recorrer todos los hijos de los subpadres
						for (int z=0;z<subpa.Tables[0].Rows.Count;z++)
						{
							lbrango.Items.Add(subpa.Tables[0].Rows[z][1].ToString()+"-"+papa.Tables[0].Rows[p][1].ToString()+"-"+subpa.Tables[0].Rows[z][0].ToString());
							lbrangof.Items.Add(subpa.Tables[0].Rows[z][1].ToString()+"-"+papa.Tables[0].Rows[p][1].ToString()+"-"+subpa.Tables[0].Rows[z][0].ToString());	
						}
					}
				}
			}
			else
			{
				if(ddltarjeta.SelectedValue.Equals("1"))
				{
					ran.Visible=false;
					lbrango.Visible=false;
					ranf.Visible=false;
					lbrangof.Visible=false;
				}
			}
		}

		protected void cbalmacen_CheckedChanged(object sender, System.EventArgs e)
		{
			if(cbalmacen.Checked==true)
			{
				ran.Visible=false;
				lbrango.Visible=false;
				ranf.Visible=false;
				lbrangof.Visible=false;
			}
			else
			{
				ran.Visible=true;
				lbrango.Visible=true;
				ranf.Visible=true;
				lbrangof.Visible=true;
			}
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
