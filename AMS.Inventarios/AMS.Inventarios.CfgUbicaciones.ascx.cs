using System;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class ConfiguradorUbicaciones : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataTable dtUbicaciones;
		protected string codigoUbicacion;
        protected string itemubicacion;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private int maximoSpan, pixelesCeldas = 170;
		protected System.Web.UI.WebControls.LinkButton LinkButton1;
		#endregion		
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			codigoUbicacion = Request.QueryString["codUbi"];
            itemubicacion = Request.QueryString["codItem"];
            int nivel = Ubicacion.DeterminarNivel(codigoUbicacion);

			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlLineaBdg,"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				tbCodigoItem.Attributes.Add("onkeyup","ItemMask("+tbCodigoItem.ClientID+","+ddlLineaBdg.ClientID+");");
				ddlLineaBdg.Attributes.Add("onchange","ChangeLine("+ddlLineaBdg.ClientID+","+tbCodigoItem.ClientID+");");
				tbCodigoItem.Attributes.Add("ondblclick","MostrarRefs("+tbCodigoItem.ClientID+","+ddlLineaBdg.ClientID+");");
				plInfoUbiNiv2.Visible = plInfoDimBod.Visible = plInfoDimNiv2.Visible = plInfoUbiNiv3.Visible = false;
				
				if(nivel == 2)
					FilasNivel3(ddlFilElim,codigoUbicacion);

				if(Request.QueryString["msg"] != null)
				{
					if(Request.QueryString["msg"] == "SUS")
                        Utils.MostrarAlerta(Response, "El código " + Request.QueryString["cod1"] + " se ha sustituido.\\nEl codigo actual es " + Request.QueryString["cod2"] + "!");
				}
			}

			if(nivel == 1)
			{
				lbNomUbiNiv1.Text = DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo="+codigoUbicacion+"");
				plInfoUbiNiv2.Visible = plInfoDimBod.Visible = true;
			}
			else if(nivel == 2)
			{
				dgUbicaciones.ShowHeader = false;
				plInfoDimNiv2.Visible = plInfoUbiNiv3.Visible = true;
				
			}

			//Ahora preparamos la tabla que maneja las ubicaciones
			PublicarInfoUbicacion(codigoUbicacion);
			LlenarGrilla(codigoUbicacion);
			FormatoPrimerColumna();
		}
		
		protected void CrearUbicacionNivel2(Object sender, CommandEventArgs e)
		{
			//Revisamos si se ha ingresado un nombre para la ubicación
			if(tbNomUbiNiv2.Text == "")
			{
                Utils.MostrarAlerta(Response, "No se ha ingresado un nombre para la nueva ubicación!");
				return;
			}

			//Si existe creamos la nueva ubicacion
			if(DBFunctions.NonQuery("INSERT INTO pubicacionitem (pubi_codigo, pubi_nombre, pubi_codpad, pubi_ubicespacial, palm_almacen) VALUES(default,'"+tbNomUbiNiv2.Text+"',"+codigoUbicacion+",'"+e.CommandArgument+"','"+DBFunctions.SingleData("SELECT palm_almacen FROM pubicacionitem WHERE pubi_codigo="+codigoUbicacion+"")+"')") > 0)
				Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+codigoUbicacion);
			else
				lb.Text += "<br>ERROR "+DBFunctions.exceptions;
		}
		
		protected void EliminarUbicacionNivel2(Object sender, CommandEventArgs e)
		{
			string ubicacionEspacial = e.CommandArgument.ToString().Trim();

			Response.Redirect(indexPage + "?process=Inventarios.EliminarUbicaciones&codUbi="+codigoUbicacion+"&tipUbi=estante&ubiEsp="+ubicacionEspacial);
		}
		
		protected void ConfigurarUbicacionNivel2(Object sender, CommandEventArgs e)
		{
            itemubicacion = Request.QueryString["codItem"];
            Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codItem=" + itemubicacion + "&codUbi=" + DBFunctions.SingleData("SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codpad="+codigoUbicacion+" AND pubi_ubicespacial='"+e.CommandArgument.ToString().Trim()+"'"));
        }
		
		protected void VolverAnterior(object Sender, EventArgs E)
		{
            int nivel = 1;
            nivel = Ubicacion.DeterminarNivel(codigoUbicacion);
			if(nivel == 1)
				Response.Redirect(indexPage + "?process=Inventarios.AdminUbicaciones");
			else
				Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codigoUbicacion+""));
		}

        protected void RetornarUbicacion(object Sender, EventArgs E)
        {
            itemubicacion = Request.QueryString["codItem"];
            //Response.Close();
            //Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx?Vals="+'N'+ "&cierre=1&odItem=" + itemubicacion + "&codUbi=" + DBFunctions.SingleData("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=" + codigoUbicacion + ""));
            //Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx?Vals=" + 'N' + "&cierre=1&codUbi=" + codigoUbicacion +"&itemUbi=" + DBFunctions.SingleData("SELECT PUBI_NOMBRE CONCAT ' ' CONCAT PUBI_UBICESPACIAL FROM PUBICACIONITEM WHERE PUBI_CODIGO = " + codigoUbicacion + ";"));
            //Response.Redirect("AMS.Web.ModalDialogUbicacionesIFrame.aspx");
            string parametros = codigoUbicacion + "*" +
                                DBFunctions.SingleData("SELECT PUBI_NOMBRE CONCAT '*' CONCAT PUBI_UBICESPACIAL FROM PUBICACIONITEM WHERE PUBI_CODIGO = " + codigoUbicacion + ";");                        

            Response.Redirect("AMS.Web.ModalDialogShow.aspx?Vals=" + parametros);
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
                Utils.MostrarAlerta(Response, "El valor de cajones es invalida revise por favor!");
				return;
			}
			int cantidadCajones = Convert.ToInt32(tbDivCeldas.Text.Trim());
			if(cantidadCajones<1)
			{
                Utils.MostrarAlerta(Response, "El valor de cajones es invalida revise por favor!");
				return;
			}
			//Ahora Debemos crear las ubicaciones de acuerdo a el numero de cajones
			bool error = false;
			for(int i=1;i<=cantidadCajones;i++)
			{
				string almacen    = DBFunctions.SingleData("SELECT palm_almacen FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codigoUbicacion+")");
				string nombreUbic = DBFunctions.SingleData("SELECT trim(PUBI_NOMBRE) FROM pubicacionitem WHERE pubi_codigo="+codigoUbicacion+" ");
                //Debemos revisar que no exista ninguna ubicacion dentro de una ubicacion de nivel2 con la ubicacionespacial repetida
				if(!DBFunctions.RecordExist("SELECT * FROM pubicacionitem WHERE pubi_codpad="+codigoUbicacion+" AND pubi_ubicespacial LIKE '"+(dtUbicaciones.Rows.Count+1)+"-"+i+"'"))
				{
                    if (cantidadCajones > 1)
                        nombreUbic += nombreFila + "-" + i;
                    else
                        nombreUbic += nombreFila;
                    if (DBFunctions.NonQuery("INSERT INTO pubicacionitem (pubi_codigo, pubi_nombre, pubi_codpad, pubi_ubicespacial, palm_almacen) VALUES(DEFAULT ,'" +nombreUbic + "'," + codigoUbicacion + ",'" + (dtUbicaciones.Rows.Count + 1) + "-" + i + "','" + almacen + "')") != 1)
					{
						lb.Text += "<br>Error "+DBFunctions.exceptions;
						error = true;
					}
				}
			}
			//Si no existio ningun error en la creacion de la nueva fila redireccionamos asi mismo
			if(!error)
				Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+codigoUbicacion);
		}
		
		protected void EliminarFila(object Sender, EventArgs E)
		{
			//Primero tomamos el numero de la ubicacion de esta fila
			int ubicacionFilaEliminar = Convert.ToInt32(ddlFilElim.SelectedValue);
			ArrayList sqlStrings = new ArrayList();
			//Traemos todas la ubicaciones dependientes de codigoUbicacion
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pubi_codigo, pubi_ubicespacial FROM pubicacionitem WHERE pubi_codpad="+codigoUbicacion+"");
			//Revisamos cada ubicacion
			//Si el numero de fila es menor que ubicacionFilaEliminar, no se hace nada
			//Si el numero de fila es igual que ubicacionFilaEliminar, debemos eliminar los registros de mubicacionitem  y los de pubicacionitem
			//Si el numero de fila es mayor que ubicacionFilaEliminar, debemos actualizar ese numero de fila 
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				int numeroFila = Convert.ToInt32((ds.Tables[0].Rows[i][1].ToString().Trim().Split('-'))[0]);
				int numeroColumna = Convert.ToInt32((ds.Tables[0].Rows[i][1].ToString().Trim().Split('-'))[1]);
				if(numeroFila == ubicacionFilaEliminar)
				{
					//Eliminamos los registros de pubicacionitem y de mubicacionitem del codigo de ubicacionque llevamos dentro de la iteracion
					sqlStrings.Add("DELETE FROM mubicacionitem WHERE pubi_codigo="+ds.Tables[0].Rows[i][0].ToString().Trim()+"");
					sqlStrings.Add("DELETE FROM pubicacionitem WHERE pubi_codigo="+ds.Tables[0].Rows[i][0].ToString().Trim()+"");
				}
				else if(numeroFila > ubicacionFilaEliminar)
					sqlStrings.Add("UPDATE pubicacionitem SET pubi_ubicespacial='"+(numeroFila-1)+"-"+numeroColumna+"' WHERE pubi_codigo="+ds.Tables[0].Rows[i][0].ToString().Trim()+"");
			}
			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+codigoUbicacion);
			else
				lb.Text += "<br>ERROR "+DBFunctions.exceptions;
		}
		
		protected void AgregarItemNivel3(Object sender, CommandEventArgs e)
		{
			//Revisamos si han ingresado el item o no
			if(tbCodigoItem.Text == "")
			{
                Utils.MostrarAlerta(Response, "No se ha ingresado ningun item!");
				return;
			}
			string codI = "";
			if(!Referencias.Guardar(tbCodigoItem.Text.Trim(),ref codI,(ddlLineaBdg.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo de item " + tbCodigoItem.Text.Trim() + " para la linea de bodega " + ddlLineaBdg.SelectedItem.Text + " es no valido.\\nRevise Por favor!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineaBdg.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo de item " + tbCodigoItem.Text.Trim() + " no se encuentra registrado.\\nRevise Por favor!");
				return;
			}
			string codI2 = "";
			bool sust = false;
			Referencias.Editar(codI,ref codI2,(ddlLineaBdg.SelectedValue.Split('-'))[1]);
			if(tbCodigoItem.Text.Trim() != codI2)
				sust = true;
			//Ahora agregamos el item a la ubicación			
			if(DBFunctions.NonQuery("INSERT INTO mubicacionitem VALUES("+Request["__EVENTARGUMENT"]+",'"+codI+"')") != 1)
				//if(DBFunctions.NonQuery("INSERT INTO mubicacionitem VALUES("+e.CommandArgument.ToString().Trim()+",'"+codI+"')") != 1)
				lb.Text +="<br>"+DBFunctions.exceptions;
			else
			{
				if(!sust)
					Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+codigoUbicacion);
				else
					Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+codigoUbicacion+"&msg=SUS&cod1="+tbCodigoItem.Text.Trim()+"&cod2="+codI2+"");
			}
		}
		
		protected void EliminarItemNivel3(Object sender, CommandEventArgs e)
		{
			//string[] sep = e.CommandArgument.ToString().Split('+');
			string[] sep = Request["__EVENTARGUMENT"].Split('+');

			if(DBFunctions.NonQuery("DELETE FROM mubicacionitem WHERE pubi_codigo="+sep[1].Trim()+" AND mite_codigo='"+sep[0].Trim()+"'") == 1)
				Response.Redirect(indexPage + "?process=Inventarios.CfgUbicaciones&codUbi="+codigoUbicacion);
			else
				lb.Text +="<br>"+DBFunctions.exceptions;
		}
		
		protected void EliminarUbicacionNivel3(Object sender, CommandEventArgs e)
		{
			string codigoUbicNiv3 = Request["__EVENTARGUMENT"];

			//Debemos revisar si existe solo una ubicacion para esta fila no podemos eliminar la ubicacion
			//Traemos la ubicacion y revisamos
			string ubicFila = (DBFunctions.SingleData("SELECT pubi_ubicespacial FROM pubicacionitem WHERE pubi_codigo="+codigoUbicNiv3+"").Trim().Split('-'))[0];
			string nomFila = (DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo="+codigoUbicNiv3+"").Trim().Split('-'))[0];
			int cantidadCols = Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(pubi_codigo) FROM pubicacionitem WHERE pubi_codpad="+codigoUbicacion+" AND pubi_ubicespacial LIKE '"+ubicFila+"-%'"));
			
			if(cantidadCols == 1)
                Utils.MostrarAlerta(Response, "Se esta intentando eliminar una ubicación unica.\\nPor favor intente eliminar la fila "+nomFila+"!");
			else
				Response.Redirect(indexPage + "?process=Inventarios.EliminarUbicaciones&codUbi="+codigoUbicacion+"&tipUbi=cajon&ubiEsp="+codigoUbicNiv3);
		}
		
		#endregion
		
		#region Metodos		
		//Metodo que permite mostrar la información de la ubicación que vamos a configurar.
		protected void PublicarInfoUbicacion(string codUbi)
		{
            int nivel = Ubicacion.DeterminarNivel(codUbi);
			int filas = 0, columnas = 3;

			if(nivel == 1)
				filas = 2;
			else if(nivel == 2)
				filas = 4;

			for(int i=0;i<filas;i++)
			{
				TableRow tr = new TableRow();

				for(int j=0;j<columnas;j++)
				{
					TableCell tc = new TableCell();

					if(i == 0 && j == 0)
					{
						tc.ColumnSpan = 3;
						tc.ForeColor = Color.RoyalBlue;
						tc.Text = "INFORMACIÓN DE LA BODEGA :";
					}

					if(i == 1 && j == 0)
					{
						if(nivel == 1)
							tc.Text = "Código Ubicación : "+codUbi;
						else if(nivel == 2)
							tc.Text = "Código Ubicación : "+DBFunctions.SingleData("SELECT pubi_codigo FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+")");
					}

					if(i == 1 && j == 1)
					{
						if(nivel == 1)
							tc.Text = "Nombre Ubicación : "+DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo="+codUbi+"");
						else if(nivel == 2)
							tc.Text = "Código Ubicación : "+DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+codUbi+")");
					}

					if(i == 1 && j == 2)
					{
						if(nivel == 1)
                            tc.Text = "Almacen Relacionado : " + DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM pubicacionitem WHERE pubi_codigo=" + codUbi + ")");
						else if(nivel == 2)
                            tc.Text = "Almacen Relacionado : " + DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=" + codUbi + "))");
					}

					if(i == 2 && j == 0)
					{
						tc.ColumnSpan = 3;
						tc.ForeColor = Color.RoyalBlue;
						tc.Text = "INFORMACIÓN DEL ESTANTE :";
					}

					if(i == 3 && j == 0)
						tc.Text = "Código Ubicación : "+codUbi;

					if(i == 3 && j == 1)
						tc.Text = "Nombre Ubicación : "+DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo="+codUbi+"");
					
					if(i == 3 && j == 2)
						tc.Text = "Ubicación Espacial : "+DBFunctions.SingleData("SELECT pubi_ubicespacial FROM pubicacionitem WHERE pubi_codigo="+codUbi+"");
					
					tr.Cells.Add(tc);
				}
                if (itemubicacion != "")
                {
                    tbCodigoItem.Text = itemubicacion;
                    btnVolver.Visible = false;
                    btnretubi.Visible = true;
                }

                tblInfo.Rows.Add(tr);
			}
		}
		
		protected DataTable PrepararDtUbicaciones(string codUbi)
		{
			dtUbicaciones = new DataTable();
			
			DataTable colspan = PrepararTablaColspan();

			int nivel = Ubicacion.DeterminarNivel(codUbi);
			int numFilas = 0,numCols = 0, i = 0, j = 0;
			
			switch(nivel)
			{
				case 1:
				{
					numFilas = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_filmatbod FROM cinventario"));
					numCols  = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_colmatbod FROM cinventario"));
					
					lbMaxFilas.Text = numFilas.ToString();
					lbMaxCols.Text = numCols.ToString();
					
					//Primero debemos agregar la columna que nos indique las filas
					dtUbicaciones.Columns.Add(new DataColumn("FILA/ESTANTE",System.Type.GetType("System.String")));//0
					
					//Ahora agregamos las columnas que tenemos configuradas en cinventario
					for(i=0;i<numCols;i++)
						dtUbicaciones.Columns.Add(new DataColumn("Estante "+(i+1),System.Type.GetType("System.String")));
					
					//Ahora agregamos las filas que tenemos configuradas en cinventario
					for(i=0;i<numFilas;i++)
					{
						DataRow fila = dtUbicaciones.NewRow();
						fila[0] = "Fila "+(i+1);
						dtUbicaciones.Rows.Add(fila);
					}
				}
					break;
				case 2:
				{
					#region Revisar el procedimiento que muestra los ítems y las ubicaciones de un estante.
					//Si no existe ninguna ubicacion dependiente codUbi se coloca un aviso que diga que no se ha configurado
					if(!DBFunctions.RecordExist("SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codpad="+codUbi+""))
					{
                        Utils.MostrarAlerta(Response, "No se ha configurado el estante : " + DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo=" + codUbi + "") + "!");
						lbFilas.Text = "0";
					}
					else
					{
						//Se realiza una consulta de los items almacenados dentro de las ubicaciones 
						DataSet dsItemsUbicaciones = new DataSet();
						
						DBFunctions.Request(dsItemsUbicaciones,IncludeSchema.NO,"SELECT PUBI.pubi_codigo as codigo_ubicacion,PUBI.pubi_nombre as nombre_ubicacion,PUBI.pubi_ubicespacial as ubicacion_espacial, cast(RTRIM(SUBSTR(pubi_ubicespacial,1,LOCATE('-',pubi_ubicespacial,1)-1)) as integer) as parametro_busqueda, cast(RTRIM(SUBSTR(pubi_ubicespacial,LOCATE('-',pubi_ubicespacial,1)+1,LENGTH(pubi_ubicespacial)-LOCATE('-',pubi_ubicespacial,1)+1)) as integer) as numero_columna,  MUBI.mite_codigo as referencia_original, DBXSCHEMA.EDITARREFERENCIAS(MUBI.mite_codigo,PLIN.plin_tipo) as referencia_modificada "+
							"FROM pubicacionitem PUBI INNER JOIN mubicacionitem MUBI ON PUBI.pubi_codigo = MUBI.pubi_codigo INNER JOIN mitems MIT ON MUBI.mite_codigo = MIT.mite_codigo INNER JOIN plineaitem PLIN ON MIT.plin_codigo = PLIN.plin_codigo "+
                            "WHERE PUBI.pubi_codpad = " + codUbi + " ORDER BY pubi_ubicespacial, PUBI_NOMBRE ;" +
							"SELECT pubi_codigo as codigo_ubicacion, pubi_nombre as nombre_ubicacion, cast(RTRIM(SUBSTR(pubi_ubicespacial,1,LOCATE('-',pubi_ubicespacial,1)-1)) as integer) as parametro_filas, cast(RTRIM(SUBSTR(pubi_ubicespacial,LOCATE('-',pubi_ubicespacial,1)+1,LENGTH(pubi_ubicespacial)-LOCATE('-',pubi_ubicespacial,1)+1)) as integer) as numero_columna "+
							"FROM pubicacionitem "+
                            "WHERE pubi_codpad = " + codUbi + " ORDER BY pubi_ubicespacial, PUBI_NOMBRE ;");
						
						ArrayList elemFilas = new ArrayList();
						
						//Primero debemos agregar la columna que nos indique las filas
						dtUbicaciones.Columns.Add(new DataColumn("FILAS",System.Type.GetType("System.String")));//0
						
						//Ahora miramos el numero de columnas que debemos agregar que siempre es el maximo 
						int maxColumns = CalculoDimensionUbiNiv2(codUbi,ref elemFilas);
						maximoSpan = maxColumns;
						//dgUbicaciones.Width = maxColumns * pixelesCeldas;
						lbFilas.Text = elemFilas.Count.ToString();
						
						//Creamos la cantidad de columnas que nos indica el maxColumns
						for(i=1;i<=maxColumns;i++)
							dtUbicaciones.Columns.Add(new DataColumn(i.ToString(),System.Type.GetType("System.String")));//i
						
						//Ahora creamos el numero de filas de acuerdo que nos indica elemFilas
						for(i=0;i<elemFilas.Count;i++)
						{
							DataRow fila = dtUbicaciones.NewRow();
						//	fila[0] = (dsItemsUbicaciones.Tables[1].Select("parametro_filas="+elemFilas[i].ToString()+""))[0][1].ToString();  // mostar solo titulo de la fila
                            fila[0] = (dsItemsUbicaciones.Tables[1].Select("parametro_filas=" + (elemFilas[i].ToString() + ""))[0][1].ToString().Split('-'))[0];
                            DataRow[] nombrePosiciones = dsItemsUbicaciones.Tables[1].Select("parametro_filas=" + elemFilas[i].ToString() + "");
							DataRow[] select = dsItemsUbicaciones.Tables[0].Select("parametro_busqueda="+elemFilas[i].ToString()+"","referencia_original asc");
							
							for(j=0;j<nombrePosiciones.Length;j++)
						//		fila[Convert.ToInt32(nombrePosiciones[j][3])] = fila[Convert.ToInt32(nombrePosiciones[j][3])].ToString() + "<span style='color: #FF0000'>"+(nombrePosiciones[j][1].ToString().Split('-'))[0]+"</span>  <p><a style='cursor:pointer' onclick=\"__doPostBack('_ctl1$lnkAgregarItem','"+nombrePosiciones[j][0]+"');\">Agregar Item</a></p><p><a style='cursor:pointer' onclick=\"__doPostBack('_ctl1$lnkEliminarUbicacion','"+nombrePosiciones[j][0]+"');\">Eliminar Ubicación</a></p>" + ConstruirHTMLCeldaReferencias(select,Convert.ToInt32(nombrePosiciones[j][3]));
                              fila[Convert.ToInt32(nombrePosiciones[j][3])] = fila[Convert.ToInt32(nombrePosiciones[j][3])].ToString() + "<span style='color: #FF0000'>" + nombrePosiciones[j][1].ToString() + "</span>  <p><a style='cursor:pointer' onclick=\"__doPostBack('_ctl1$lnkAgregarItem','" + nombrePosiciones[j][0] + "');\">Agregar Item</a></p><p><a style='cursor:pointer' onclick=\"__doPostBack('_ctl1$lnkEliminarUbicacion','" + nombrePosiciones[j][0] + "');\">Eliminar Ubicación</a></p>" + ConstruirHTMLCeldaReferencias(select, Convert.ToInt32(nombrePosiciones[j][3]));

							#region Código candidato a eliminación
							//Se consulta que items estan relacionados en a la ubicacion configurada dependiendo de la fila
							/*if(select.Length > 0)
								fila[Convert.ToInt32((select[0][2].ToString().Split('-'))[1].Trim())] = fila[Convert.ToInt32((select[0][2].ToString().Split('-'))[1].Trim())].ToString()+ ConstruirHTMLCeldaReferencias(select,Convert.ToInt32((select[0][2].ToString().Split('-'))[1].Trim()));*/
							//Se determina el colspan de las columnas dependiendo de la cantidad de columans especificadas para esa fila
							#endregion

							int colspanInst = maxColumns;
							int cantidadColumnasFila = maxColumns;
							
							try
							{
								cantidadColumnasFila = Convert.ToInt32(dsItemsUbicaciones.Tables[1].Compute("MAX(numero_columna)","parametro_filas="+elemFilas[i].ToString()+""));
								colspanInst = (int)((double)maxColumns/(double)cantidadColumnasFila);
							}
							catch { }
							
							DataRow dr = colspan.NewRow();
							dr[0] = Convert.ToInt32(elemFilas[i]);
							dr[1] = colspanInst;
							dr[2] = cantidadColumnasFila;
							colspan.Rows.Add(dr);
							
							//Ahora vamos a determinar el colspan de la columna determinado 
							dtUbicaciones.Rows.Add(fila);
						}
					}
					#endregion
				}
					break;
			}

			return colspan;
		}
		
		protected void BindDgUbicaciones()
		{
			Session["dtUbicaciones"] = dtUbicaciones;
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
		
		protected void LlenarGrilla(string codUbi)
		{
			DataTable colspan = PrepararDtUbicaciones(codUbi);

			int nivel = Ubicacion.DeterminarNivel(codUbi);
			DataSet ds = new DataSet();

			switch(nivel)
			{
				case 1:
				{
					int i,j;
					//Traemos la cantidad de filas y columnas maximas permitidas
					int numFilas = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_filmatbod FROM cinventario"));
					int numCols = Convert.ToInt32(DBFunctions.SingleData("SELECT cinv_colmatbod FROM cinventario"));
					
					//Debemos traer las ubicaciones que tienen como ubicacion padre la de codigo codUbi
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pubi_nombre, TRIM(pubi_ubicespacial) FROM pubicacionitem WHERE pubi_codpad=" + codUbi + " AND pubi_ubicespacial is not null");
					
                    //dentro de dtUbicaciones, guardamos las ubicaciones que existen
					//La ubicacion espacial se almacen Filas-Columnas
                    if (ds.Tables[0].Rows.Count > dtUbicaciones.Rows.Count * (dtUbicaciones.Columns.Count -1))
                    {
                        Utils.MostrarAlerta(Response, "La configuracion de filas es menor a la generada !");
                        return;
                    }

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

                                //Contenedor de los objetos dentro de la tabla y grafica de la bodega. 
                                Panel contenedor = new Panel();
                                contenedor.CssClass = "imgBodega";

                                //Label de la sección.
                                Panel contSeccion = new Panel();
                                contSeccion.CssClass = "seccion";
                                Label literal = new Label();
                                literal.Text = dtUbicaciones.Rows[i][j].ToString();
                                contSeccion.Controls.Add(literal);
                                contenedor.Controls.Add(contSeccion);

                                //Link para ubicar configuración.
                                Panel contConfigurar = new Panel();
                                contConfigurar.CssClass = "configurar";
                                LinkButton lnkConfigurar = new LinkButton();
                                lnkConfigurar.Text = "Configurar";
                                lnkConfigurar.Attributes.Add("onclick", "espera()");
                                lnkConfigurar.CommandArgument = (i + 1) + "-" + j;
                                lnkConfigurar.Command += new CommandEventHandler(ConfigurarUbicacionNivel2);
                                contConfigurar.Controls.Add(lnkConfigurar);
                                contenedor.Controls.Add(contConfigurar);

                                //Link para ubicar la eliminación.
                                Panel contEliminar = new Panel();
                                contEliminar.CssClass = "eliminar";
                                LinkButton lnkEliminar = new LinkButton();
                                lnkEliminar.Text = "Eliminar";
                                lnkEliminar.CommandArgument = (i + 1) + "-" + j;
                                lnkEliminar.Command += new CommandEventHandler(EliminarUbicacionNivel2);
                                contEliminar.Controls.Add(lnkEliminar);
                                contenedor.Controls.Add(contEliminar);
                                
                                dgUbicaciones.Items[i].Cells[j].Controls.Add(contenedor);
							}

							dgUbicaciones.Items[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
							dgUbicaciones.Items[i].Cells[j].ForeColor = Color.Black;
						}
					}
				}
					break;
				case 2:
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
					break;
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

			try
			{
				retorno = Convert.ToInt32(ds.Tables[0].Compute("MAX(numero_columna)",""));
			}
			catch { }

			return retorno;
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
		
		protected void FilasNivel3(DropDownList ddl, string codUbi)
		{
            if (DBFunctions.RecordExist("SELECT * FROM pubicacionitem WHERE pubi_codpad=" + codUbi + ""))
			{
				ddl.Items.Clear();

				ArrayList elemFil = new ArrayList();

				CalculoDimensionUbiNiv2(codUbi,ref elemFil);

                for(int i=0;i<elemFil.Count;i++)
				{
                    string nomFila = (DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codpad=" + codUbi + " AND pubi_ubicespacial LIKE '" + elemFil[i].ToString() + "-%'").Trim().Split('-'))[0];
                    ddl.Items.Add(new ListItem(nomFila, elemFil[i].ToString()));
				}
			}
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
					salida += "<tr><td>"+items[i][6]+"</td><td align='right'><img src='../img/equis.png' title='Eliminar' border='0' style='cursor:pointer' onclick=\"EliminarItemUbi('_ctl1$lnkEliminarItem','"+items[i][5]+"+"+items[i][0]+"')\"/></td></tr>";
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
			this.lnkEliminarItem.Command += new System.Web.UI.WebControls.CommandEventHandler(this.EliminarItemNivel3);
			this.lnkAgregarItem.Command += new System.Web.UI.WebControls.CommandEventHandler(this.AgregarItemNivel3);
			this.lnkEliminarUbicacion.Command += new System.Web.UI.WebControls.CommandEventHandler(this.EliminarUbicacionNivel3);

		}
		#endregion
	}
}
