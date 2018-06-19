using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;

namespace AMS.Web
{
	public partial  class ModalDialogUbicaciones : System.Web.UI.Page
	{
		#region Atributos
		protected string tipoProceso, codigoAlmacen, codItem, codOriginal, codigoUbicacion;
		protected int nivel;
		protected DataTable dtUbicaciones;
		private int maximoSpan, pixelesCeldas = 170;
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			tipoProceso = Request.QueryString["tipProc"];
			codigoAlmacen = Request.QueryString["codAlma"];
			codItem = Request.QueryString["codItem"];
			codOriginal = Request.QueryString["codOri"];
			nivel = 0;
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlAlmacen,"SELECT palm_almacen, palm_descripcion FROM palmacen");
				DatasToControls.EstablecerDefectoDropDownList(ddlAlmacen,DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE palm_almacen='"+codigoAlmacen+"'"));
				bind.PutDatasIntoDropDownList(ddlUbiNiv1,"SELECT pubi_codigo, pubi_nombre FROM pubicacionitem WHERE palm_almacen='"+ddlAlmacen.SelectedValue+"' AND pubi_codpad IS NULL");
				lbCodI.Text = codItem;
				lbNomI.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codOriginal+"'");
				lbLinBod.Text = DBFunctions.SingleData("SELECT plin_nombre FROM plineaitem WHERE plin_codigo=(SELECT plin_codigo FROM mitems WHERE mite_codigo='"+codOriginal+"')");
				plInfoUbiNiv2.Visible = plInfoUbiNiv3.Visible = false;
				if(tipoProceso == "N")
					ddlAlmacen.Enabled = false;
			}
			//Manejo de Nivel
			if(Request.QueryString["codUbicacion"] == null || Request.QueryString["codUbicacion"] == "")
			{
				//Debemos cargar la configuracion de la ubicacion de nivel 1 que se encuentra dentro de ddlUbiNiv1
				if(ddlUbiNiv1.Items.Count == 0)
					Response.Write("<script language='javascript'>alert('No se tiene ninguna configurada ninguna ubicación de nivel 1 para el almacen "+ddlAlmacen.SelectedItem.Text+".\\nRevise Por Favor!');</script>");
				else
				{
					codigoUbicacion = ddlUbiNiv1.SelectedValue;
					nivel = DeterminarNivel(codigoUbicacion);
					plInfoUbiNiv2.Visible = true;
				}
			}
			else
			{
				codigoUbicacion = Request.QueryString["codUbicacion"];
				nivel = DeterminarNivel(codigoUbicacion);
				plInfoUbiNiv3.Visible = lnkVolver.Visible = true;
			}
			//Fin manejo de nivel			
			//Manejo de grilla
			if(nivel != 0)
			{
				LlenarGrilla(codigoUbicacion);
				FormatoPrimerColumna();
			}
			//Fin Manejo de grilla
		}
		
		protected void CrearUbicacionNivel2(Object sender, CommandEventArgs e)
		{
			//Revisamos si se ha ingresado un nombre para la ubicación
			if(tbNomUbiNiv2.Text == "")
			{
				Response.Write("<script language='javascript'>alert('No se ha ingresado un nombre para la nueva ubicación!');</script>");
				return;
			}
			//Si existe creamos la nueva ubicacion
			if(DBFunctions.NonQuery("INSERT INTO pubicacionitem VALUES(default,"+codigoUbicacion+",'"+tbNomUbiNiv2.Text+"','"+e.CommandArgument+"','"+DBFunctions.SingleData("SELECT palm_almacen FROM pubicacionitem WHERE pubi_codigo="+codigoUbicacion+"")+"')") > 0)
				Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx?tipProc=" + tipoProceso + "&codAlma=" + this.ddlAlmacen.SelectedValue + "&codItem=" + codItem + "&codOri=" + codOriginal);
			else
				lb.Text += "<br>ERROR "+DBFunctions.exceptions;
		}
		
		protected void ConfigurarUbicacionNivel2(Object sender, CommandEventArgs e)
		{
			Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx?tipProc=" + tipoProceso + "&codAlma=" + this.ddlAlmacen.SelectedValue + "&codItem=" + codItem + "&codOri=" + codOriginal +"&codUbicacion=" +DBFunctions.SingleData("SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad="+codigoUbicacion+" AND pubi_ubicespacial='"+e.CommandArgument.ToString().Trim()+"'"));
		}
		
		protected void AgregarFilaNiv2(object Sender, EventArgs E)
		{
			string nombreFila = "";
			//revisamos si se ha digitado un nombre de fila 
			if(tbNomFil.Text != "")
				nombreFila = tbNomFil.Text;
			else
			{
				int cantidadFilas = dtUbicaciones.Rows.Count;
				char val = (char)(65+cantidadFilas);
				nombreFila = val.ToString();
			}
			if(!DatasToControls.ValidarInt(tbDivCeldas.Text.Trim()))
			{
				Response.Write("<script language='javascript'>alert('El valor de cajones es invalida revise por favor!');</script>");
				return;
			}
			int cantidadCajones = Convert.ToInt32(tbDivCeldas.Text.Trim());
			if(cantidadCajones<1)
			{
				Response.Write("<script language='javascript'>alert('El valor de cajones es invalida revise por favor!');</script>");
				return;
			}
			//Ahora Debemos crear las ubicaciones de acuerdo a el numero de cajones
			bool error = false;
			for(int i=1;i<=cantidadCajones;i++)
			{
				string almacen = DBFunctions.SingleData("SELECT palm_almacen FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codigoUbicacion+")");
				//Debemos revisar que no exista ninguna ubicacion dentro de una ubicacion de nivel2 con la ubicacionespacial repetida
				if(!DBFunctions.RecordExist("SELECT * FROM pubicacionitem WHERE pubi_codpad="+codigoUbicacion+" AND pubi_ubicespacial LIKE '"+(dtUbicaciones.Rows.Count+1)+"-"+i+"'"))
				{
					if(DBFunctions.NonQuery("INSERT INTO pubicacionitem VALUES(DEFAULT,"+codigoUbicacion+",'"+nombreFila+"-"+i+"','"+(dtUbicaciones.Rows.Count+1)+"-"+i+"','"+almacen+"')") != 1)
					{
						lb.Text += "<br>Error "+DBFunctions.exceptions;
						error = true;
					}
				}
			}
			//Si no existio ningun error en la creacion de la nueva fila redireccionamos asi mismo
			if(!error)
				Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx?tipProc=" + tipoProceso + "&codAlma=" + this.ddlAlmacen.SelectedValue + "&codItem=" + codItem + "&codOri=" + codOriginal +"&codUbicacion=" +codigoUbicacion);
		}
		
		protected void Volver(object Sender, EventArgs E)
		{
			Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx?tipProc=" + tipoProceso + "&codAlma=" + this.ddlAlmacen.SelectedValue + "&codItem=" + codItem + "&codOri=" + codOriginal);
		}
		
		protected void AgregarItemNivel3(Object sender, CommandEventArgs e)
		{
			//Ahora agregamos el item a la ubicación
			if(DBFunctions.NonQuery("INSERT INTO mubicacionitem VALUES("+Request["__EVENTARGUMENT"]+",'"+codOriginal+"')") != 1)
				lb.Text +="<br>"+DBFunctions.exceptions;
			else
			{
				//Construimos el Vals para ese codigo de ubicacion
				string Vals = Request["__EVENTARGUMENT"]+"*";
				Vals += DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+Request["__EVENTARGUMENT"]+"))").Trim()+",";
				Vals += DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+Request["__EVENTARGUMENT"]+")").Trim()+",";
				Vals += DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo="+Request["__EVENTARGUMENT"]+"").Trim();
				Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx?tipProc=" + tipoProceso + "&codAlma=" + this.ddlAlmacen.SelectedValue + "&codItem=" + codItem + "&codOri=" + codOriginal +"&Vals="+Vals);
			}
		}
		#endregion
		
		#region Metodos
		protected int DeterminarNivel(string codUbi)
		{
			int nivel = 0;
			if(DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+"") == "")
				nivel = 1;
			else if(DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+")") == "")
				nivel = 2;
			else if(DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+")") != "")
				nivel = 3;
			return nivel;
		}
		
		protected DataTable PrepararDtUbicaciones(string codUbi)
		{
			dtUbicaciones = new DataTable();
			DataTable colspan = PrepararTablaColspan();
			int numFilas = 0, numCols = 0, i = 0, j=0;
			if(nivel == 1)
			{
				numFilas = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_filmatbod FROM cinventario"));
				numCols = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_colmatbod FROM cinventario"));
				//Primero debemos agregar la columna que nos indique las filas
				dtUbicaciones.Columns.Add(new DataColumn("FILA/COLUMNA",System.Type.GetType("System.String")));//0
				//Ahora agregamos las columnas que tenemos configuradas en cinventario
				for(i=0;i<numCols;i++)
					dtUbicaciones.Columns.Add(new DataColumn("Columna "+(i+1),System.Type.GetType("System.String")));
				//Ahora agregamos las filas que tenemos configuradas en cinventario
				for(i=0;i<numFilas;i++)
				{
					DataRow fila = dtUbicaciones.NewRow();
					fila[0] = "Fila "+(i+1);
					dtUbicaciones.Rows.Add(fila);
				}
			}
			else if(nivel == 2)
			{
				//Si no existe ninguna ubicacion dependiente codUbi se coloca un aviso que diga que no se ha configurado
				if(!DBFunctions.RecordExist("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codpad="+codUbi+""))
					Response.Write("<script language='javascript'>alert('No se ha configurado la ubicación de nivel 2 : "+DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo="+codUbi+"")+"!');</script>");
				else
				{
					//Se realiza una consulta de los items almacenados dentro de las ubicaciones 
					DataSet dsItemsUbicaciones = new DataSet();
					DBFunctions.Request(dsItemsUbicaciones,IncludeSchema.NO,"SELECT PUBI.pubi_codigo as codigo_ubicacion,PUBI.pubi_nombre as nombre_ubicacion,PUBI.pubi_ubicespacial as ubicacion_espacial, cast(RTRIM(SUBSTR(pubi_ubicespacial,1,LOCATE('-',pubi_ubicespacial,1)-1)) as integer) as parametro_busqueda, cast(RTRIM(SUBSTR(pubi_ubicespacial,LOCATE('-',pubi_ubicespacial,1)+1,LENGTH(pubi_ubicespacial)-LOCATE('-',pubi_ubicespacial,1)+1)) as integer) as numero_columna,  MUBI.mite_codigo as referencia_original, DBXSCHEMA.EDITARREFERENCIAS(MUBI.mite_codigo,PLIN.plin_tipo) as referencia_modificada "+
																		    "FROM pubicacionitem PUBI INNER JOIN mubicacionitem MUBI ON PUBI.pubi_codigo = MUBI.pubi_codigo INNER JOIN mitems MIT ON MUBI.mite_codigo = MIT.mite_codigo INNER JOIN plineaitem PLIN ON MIT.plin_codigo = PLIN.plin_codigo "+
																			"WHERE PUBI.pubi_codpad = "+codUbi+";"+
																			"SELECT pubi_codigo as codigo_ubicacion, pubi_nombre as nombre_ubicacion, cast(RTRIM(SUBSTR(pubi_ubicespacial,1,LOCATE('-',pubi_ubicespacial,1)-1)) as integer) as parametro_filas, cast(RTRIM(SUBSTR(pubi_ubicespacial,LOCATE('-',pubi_ubicespacial,1)+1,LENGTH(pubi_ubicespacial)-LOCATE('-',pubi_ubicespacial,1)+1)) as integer) as numero_columna "+
																			"FROM pubicacionitem "+
																			"WHERE pubi_codpad = "+codUbi+";");
					ArrayList elemFilas = new ArrayList();
					//Primero debemos agregar la columna que nos indique las filas
					dtUbicaciones.Columns.Add(new DataColumn("FILAS",System.Type.GetType("System.String")));//0
					//Ahora miramos el numero de columnas que debemos agregar que siempre es el maximo 
					int maxColumns = CalculoDimensionUbiNiv2(codUbi,ref elemFilas);
					maximoSpan = maxColumns;
					dgUbicaciones.Width = maxColumns*pixelesCeldas;
					//Creamos la cantidad de columnas que nos indica el maxColumns
					for(i=1;i<=maxColumns;i++)
						dtUbicaciones.Columns.Add(new DataColumn(i.ToString(),System.Type.GetType("System.String")));//i
					//Ahora creamos el numero de filas de acuerdo que nos indica elemFilas
					for(i=0;i<elemFilas.Count;i++)
					{
						DataRow fila = dtUbicaciones.NewRow();
						fila[0] = (dsItemsUbicaciones.Tables[1].Select("parametro_filas="+elemFilas[i].ToString()+""))[0][1].ToString();
						DataRow[] nombrePosiciones = dsItemsUbicaciones.Tables[1].Select("parametro_filas="+elemFilas[i].ToString()+"");
						DataRow[] select = dsItemsUbicaciones.Tables[0].Select("parametro_busqueda="+elemFilas[i].ToString()+"");
						for(j=0;j<nombrePosiciones.Length;j++)
						{
							fila[Convert.ToInt32(nombrePosiciones[j][3])] = fila[Convert.ToInt32(nombrePosiciones[j][3])].ToString() + "<span style='color: #FF0000'>"+(nombrePosiciones[j][1].ToString().Split('-'))[0]+"-"+nombrePosiciones[j][3]+"</span>  "+"<a style='cursor:pointer' onclick=\"__doPostBack('lnkAgregarItem','"+nombrePosiciones[j][0]+"');\">Asignar Ubicación</a>";
							fila[Convert.ToInt32(nombrePosiciones[j][3])] = fila[Convert.ToInt32(nombrePosiciones[j][3])].ToString() + ConstruirHTMLCeldaReferencias(select,Convert.ToInt32(nombrePosiciones[j][3]));
						}
						//Se consulta que items estan relacionados en a la ubicacion configurada dependiendo de la fila
						/*if(select.Length > 0)
							fila[Convert.ToInt32((select[0][2].ToString().Split('-'))[1].Trim())] = fila[Convert.ToInt32((select[0][2].ToString().Split('-'))[1].Trim())].ToString()+ ConstruirHTMLCeldaReferencias(select,Convert.ToInt32((select[0][2].ToString().Split('-'))[1].Trim()));*/
						//Se determina el colspan de las columnas dependiendo de la cantidad de columans especificadas para esa fila
						int colspanInst = maxColumns;
						int cantidadColumnasFila = maxColumns;
						try
						{
							cantidadColumnasFila = Convert.ToInt32(dsItemsUbicaciones.Tables[1].Compute("MAX(numero_columna)","parametro_filas="+elemFilas[i].ToString()+""));
							colspanInst = (int)((double)maxColumns/(double)cantidadColumnasFila);
						}
						catch{}
						DataRow dr = colspan.NewRow();
						dr[0] = Convert.ToInt32(elemFilas[i]);
						dr[1] = colspanInst;
						dr[2] = cantidadColumnasFila;
						colspan.Rows.Add(dr);
						//Ahora vamos a determinar el colspan de la columna determinado 
						dtUbicaciones.Rows.Add(fila);
					}
				}
			}
			return colspan;
		}
		
		protected void LlenarGrilla(string codUbi)
		{
			DataTable colspan = PrepararDtUbicaciones(codUbi);
			DataSet ds = new DataSet();
			if(nivel == 1)
			{
				int i,j;
				//Traemos la cantidad de filas y columnas maximas permitidas
				int numFilas = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_filmatbod FROM cinventario"));
				int numCols = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_colmatbod FROM cinventario"));
				//Debemos traer las ubicaciones que tienen como ubicacion padre la de codigo codUbi
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pubi_nombre, pubi_ubicespacial FROM pubicacionitem WHERE pubi_codpad="+codUbi+"");
				//dentro de dtUbicaciones, guardamos las ubicaciones que existen
				//La ubicacion espacial se almacen Filas-Columnas
				for(i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					string[] ubicEspacial = ds.Tables[0].Rows[i][1].ToString().Split('-');
					dtUbicaciones.Rows[Convert.ToInt32(ubicEspacial[0].Trim())-1][Convert.ToInt32(ubicEspacial[1].Trim())] = ds.Tables[0].Rows[i][0].ToString();
				}
				BindDgUbicaciones();
				//Ahora revisamos que ubicaciones estan libres y podemos asignar una ubicacion de nivel 2
				for(i=0;i<numFilas;i++)
				{
					for(j=1;j<=numCols;j++)
					{
						if(dtUbicaciones.Rows[i][j].ToString() == "")
						{
							dgUbicaciones.Items[i].Cells[j].BackColor = Color.LightGreen;
							LinkButton lnkCrear = new LinkButton();
							lnkCrear.Text = "Crear";
							lnkCrear.BackColor = Color.Transparent;
							lnkCrear.CommandArgument = (i+1)+"-"+j;
							lnkCrear.Command += new CommandEventHandler(CrearUbicacionNivel2);
							dgUbicaciones.Items[i].Cells[j].Controls.Add(lnkCrear);
						}
						else
						{
							dgUbicaciones.Items[i].Cells[j].BackColor = Color.Orange;
							dgUbicaciones.Items[i].Cells[j].Controls.Add(new LiteralControl(dtUbicaciones.Rows[i][j]+"<br>"));
							LinkButton lnkConfigurar = new LinkButton();
							lnkConfigurar.Text = "Configurar";
							lnkConfigurar.BackColor = Color.Transparent;
							lnkConfigurar.CommandArgument = (i+1)+"-"+j;
							lnkConfigurar.Command += new CommandEventHandler(ConfigurarUbicacionNivel2);
							dgUbicaciones.Items[i].Cells[j].Controls.Add(lnkConfigurar);
						}
						dgUbicaciones.Items[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
						dgUbicaciones.Items[i].Cells[j].ForeColor = Color.Black;
					}
				}
			}
			else if(nivel == 2)
			{
				int i,j;
				if(DBFunctions.RecordExist("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codpad="+codUbi+""))
				{
					BindDgUbicaciones();
					for(i=0;i<dgUbicaciones.Items.Count;i++)
					{
						int contSpan = 0;
						for(j=1;j<dgUbicaciones.Items[i].Cells.Count;j++)
						{
							if(contSpan < maximoSpan && j < Convert.ToInt32(colspan.Rows[i][2]))
							{
								FormatoCeldaNivel3(i,j,Convert.ToInt32(colspan.Rows[i][1]));
								contSpan += Convert.ToInt32(colspan.Rows[i][1]);
							}
							else if(contSpan < maximoSpan && j >= Convert.ToInt32(colspan.Rows[i][2]))
							{
								FormatoCeldaNivel3(i,j,maximoSpan - contSpan);
								contSpan = maximoSpan;
							}
							else
								FormatoCeldaNivel3(i,j,0);
						}
					}
				}
			}
		}
		
		protected int CalculoDimensionUbiNiv2(string codUbi, ref ArrayList elemFils)
		{
			 //Traemos todas las ubicaciones que depende de codUbi
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pubi_ubicespacial, cast(RTRIM(SUBSTR(pubi_ubicespacial,LOCATE('-',pubi_ubicespacial,1)+1,LENGTH(pubi_ubicespacial)-LOCATE('-',pubi_ubicespacial,1)+1)) as integer) as numero_columna FROM pubicacionitem WHERE pubi_codpad="+codUbi+" ORDER BY pubi_ubicespacial");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				//Dividimos la ubicacionespacial en sus dos partes fila-columna
				string[] divUbicEsp = ds.Tables[0].Rows[i][0].ToString().Split('-');
				int indiceFila = elemFils.BinarySearch(Convert.ToInt32(divUbicEsp[0].Trim()));
				//Revisamos si esta ubicacion de fila no se ha agregado
				if(indiceFila < 0)
					elemFils.Add(Convert.ToInt32(divUbicEsp[0].Trim()));
				elemFils.Sort();
			}
			elemFils.Sort();
			int retorno = -1;
			try{retorno = Convert.ToInt32(ds.Tables[0].Compute("MAX(numero_columna)",""));}
			catch{}
			return retorno;
		}
		
		protected void BindDgUbicaciones()
		{
			dgUbicaciones.DataSource = dtUbicaciones;
			dgUbicaciones.DataBind();
		}
		
		protected void FormatoPrimerColumna()
		{
			for(int i=0;i<dgUbicaciones.Items.Count;i++)
			{
				dgUbicaciones.Items[i].Cells[0].BackColor = Color.FromArgb(0,0,132);
				dgUbicaciones.Items[i].Cells[0].ForeColor = Color.White;
				dgUbicaciones.Items[i].Cells[0].HorizontalAlign = HorizontalAlign.Center;
			}
		}
		
		protected void FormatoCeldaNivel3(int numeroFilaGrilla, int numeroColumnaGrilla, int colspan)
		{
			dgUbicaciones.Items[numeroFilaGrilla].Cells[numeroColumnaGrilla].ColumnSpan = colspan;
			if(colspan == 0)
				dgUbicaciones.Items[numeroFilaGrilla].Cells[numeroColumnaGrilla].Visible = false;
			dgUbicaciones.Items[numeroFilaGrilla].Cells[numeroColumnaGrilla].BackColor = Color.LightGray;
			dgUbicaciones.Items[numeroFilaGrilla].Cells[numeroColumnaGrilla].HorizontalAlign = HorizontalAlign.Center;
			dgUbicaciones.Items[numeroFilaGrilla].Cells[numeroColumnaGrilla].ForeColor = Color.Black;
		}

		private DataTable PrepararTablaColspan()
		{
			DataTable dtColspan = new DataTable();
			dtColspan.Columns.Add(new DataColumn("NumeroFila",typeof(int)));//0
			dtColspan.Columns.Add(new DataColumn("ValorColspan",typeof(int)));//1
			dtColspan.Columns.Add(new DataColumn("MaximoColumnas",typeof(int)));//2
			return dtColspan;
		}

		private string ConstruirHTMLCeldaReferencias(DataRow[] items, int columna)
		{
			string salida = "<table width='"+pixelesCeldas+"px' border='0' cellspacing='0' cellspading='0' class='tablewhite'>";
			for(int i=0;i<items.Length;i++)
			{
				if(Convert.ToInt32(items[i][4]) == columna)
					salida += "<tr><td>"+items[i][6]+"</td></tr>";
			}	
			salida += "</table>";
			return salida;
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
			this.lnkAgregarItem.Command += new System.Web.UI.WebControls.CommandEventHandler(this.AgregarItemNivel3);

		}
		#endregion
	}
}
