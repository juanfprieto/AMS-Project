// created on 30/10/2003 at 9:31
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Ajax;
using AMS.Forms;
using AMS.Reportes;
using AMS.Documentos;
using AMS.DB;
using AMS.Tools;

//using System.Windows.Forms;



namespace AMS.Inventarios
{

	public partial class ReporteItems : System.Web.UI.UserControl
	{
		#region Controles
		protected DataGrid grAct;//DAtos Basicos
		protected DataTable dtInserts;
		protected DataSet ds;
		protected Table tabPreHeader,tabFirmas;
		protected string reportTitle="Inventarios: Reporte Items";
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        
        #endregion


        protected void Page_Load(object sender, System.EventArgs e)
		{
            //try {string Permiso = Request.QueryString["Permiso"]; Session["Permiso"] = Permiso;}
            //catch { };
            // Enable the GridView paging option and  
            // specify the page size. 
            GenKarPru.AllowPaging = true;
            GenKarPru.PageSize = 150;

            // Enable the GridView sorting option. 
            GenKarPru.AllowSorting = true;

            // Initialize the sorting expression.
            if (!IsPostBack)
            {  
            ViewState["SortExpression"] = "Documento Cruce ASC";
           ;
            }

            // Populate the GridView. 
            //BindGridView(); 

			Ajax.Utility.RegisterTypeForAjax(typeof(ReporteItems));
			this.ClearChildViewState();
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlLineas, "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");//Linea de Bodega
				//OJOOOO REVISARRRRR DESPUES PORQUE QUEDO LIGADO A UNA LISTA DE PRECIOS FIJA : PO
				txtRef.Attributes.Add("ondblclick","CargaITEM("+txtRef.ClientID+","+ddlLineas.ClientID+",'"+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+"');");
				txtRef.Attributes.Add("onkeyup","ItemMask("+txtRef.ClientID+","+ddlLineas.ClientID+");");
				ddlLineas.Attributes.Add("onchange","ChangeLine("+ddlLineas.ClientID+","+txtRef.ClientID+");");
				bind.PutDatasIntoDropDownList(ddlCantAno, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC"); //Año Utilizado para el reporte de los saldos y costos del item
                bind.PutDatasIntoDropDownList(ddlResMesAno, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");//Año Utilizado para el reporte de resumen del mes
				bind.PutDatasIntoDropDownList(ddlEstAno, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");//Año Utilizado para el reporte de estadisticas
				bind.PutDatasIntoDropDownList(ddlPedAno, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");//Año Utilizado para el reporte de pedidos
				bind.PutDatasIntoDropDownList(ddlResAnoAno, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");//Año Utilizado para el reporte de resumen del año
				bind.PutDatasIntoDropDownList(ddlAnoDistrib, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");//Año Utilizado para el reporte de distribucion
				bind.PutDatasIntoDropDownList(ddlKarAno, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");//Año Utilizado para el reporte de kardex

                bind.PutDatasIntoDropDownList(ddlKarMes, "SELECT pmes_mes,pmes_nombre FROM pmes ORDER BY PMES_MES");//Mes Utilizado para el reporte de kardex
                bind.PutDatasIntoDropDownList(ddlPedMes, "SELECT pmes_mes,pmes_nombre FROM pmes ORDER BY PMES_MES");//Mes Utilizado para el reporte de pedidos
				bind.PutDatasIntoDropDownList(ddlResMesMes, "SELECT pmes_mes,pmes_nombre FROM pmes ORDER BY pmes_mes ASC");//Mes Utilizado para el reporte de resumen del mes

                bind.PutDatasIntoDropDownList(ddlPedAlm,  "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");//Almacen utilizado para el reporte de pedidos
                bind.PutDatasIntoDropDownList(ddlCantAlm, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");//Almacen Utilizado para el reporte de los saldos y costos del item
                bind.PutDatasIntoDropDownList(ddlCedes,   "SELECT palm_almacen, palm_descripcion FROM palmacen where pcen_centinv is not null and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION;");//Movimientos de Kardex

                bind.PutDatasIntoDropDownList(ddlKarTMov, "SELECT tmov_tipomovi, tmov_nombmovi from tmovikard  ORDER BY 2");//Movimientos de Kardex
                DataSet dsAM =new DataSet();
				dsAM=DBFunctions.Request(dsAM, IncludeSchema.NO, "SELECT t1.PANO_ANO,t1.PMES_MES,t2.PMES_NOMBRE FROM CINVENTARIO as t1, PMES as t2 where t1.pmes_mes=t2.pmes_mes");
				string mes="",ano="",mesN="";
				try
				{
					ano=dsAM.Tables[0].Rows[0][0].ToString();
					mes=dsAM.Tables[0].Rows[0][1].ToString();
					mesN=dsAM.Tables[0].Rows[0][2].ToString();
					ddlCantAno.SelectedIndex=ddlCantAno.Items.IndexOf(new ListItem(ano,ano));
					ddlResMesAno.SelectedIndex=ddlResMesAno.Items.IndexOf(new ListItem(ano,ano));
					ddlResAnoAno.SelectedIndex=ddlResAnoAno.Items.IndexOf(new ListItem(ano,ano));
					ddlPedAno.SelectedIndex=ddlPedAno.Items.IndexOf(new ListItem(ano,ano));
					ddlEstAno.SelectedIndex=ddlEstAno.Items.IndexOf(new ListItem(ano,ano));
					ddlResMesMes.SelectedIndex=ddlResMesMes.Items.IndexOf(new ListItem(mesN,mes));
					ddlPedMes.SelectedIndex=ddlPedMes.Items.IndexOf(new ListItem(mesN,mes));
					ddlKarAno.SelectedIndex=ddlKarAno.Items.IndexOf(new ListItem(ano,ano));
					ddlAnoDistrib.SelectedIndex=ddlAnoDistrib.Items.IndexOf(new ListItem(ano,ano));
					ddlKarMes.SelectedIndex=ddlKarMes.Items.IndexOf(new ListItem(mesN,mes));
				}
				catch{}
				tbDate1.Text = DateTime.Now.GetDateTimeFormats()[6];
				calDate1.SelectedDate=DateTime.Now;
				tbDate2.Text = DateTime.Now.GetDateTimeFormats()[6];
				calDate2.SelectedDate=DateTime.Now;
				Esconder();
			}
			else
			{
				lbCosto.Text=Request.Form[hdncosto.UniqueID];
				try{lbValor.Text=Request.Form[hdnvalor.UniqueID];}
				catch{lbValor.Text=Request.Form[hdnvalor.UniqueID];}
                try { lbValorIva.Text = Request.Form[hdnvalorIva.UniqueID]; }
                catch { lbValorIva.Text = Request.Form[hdnvalorIva.UniqueID]; }
                lbCantidad.Text=Request.Form[hdncant.UniqueID];
				try{lbSustituciones.Text=Request.Form[hdnsust.UniqueID];}
				catch{lbSustituciones.Text=Request.Form[hdnsust.UniqueID];}
				try{lbUbicacion.Text=Request.Form[hdnubi.UniqueID];}
				catch{lbUbicacion.Text=Request.Form[hdnubi.UniqueID];}
				lbSerial.Text=Request.Form[hdnser.UniqueID];
                try { lbAplicacion.Text = Request.Form[hdAplic.UniqueID]; }
                catch { lbAplicacion.Text = Request.Form[hdAplic.UniqueID]; }
			}


		}

		protected bool RevisarNit()
		{
			if(txtRef.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Primero debe seleccionar un item!");
				return(false);
			}
			return(true);
		}
		
		protected void Esconder()
		{
			plhDatosBasicos.Visible=plhResumenMes.Visible=plhCantidades.Visible=plhResumenAno.Visible=plhKar.Visible=plhPed.Visible=plhEst.Visible=plhDistrib.Visible=plhApli.Visible=false;
			btnDatBas.BackColor=btnCants.BackColor=btnResMes.BackColor=btnResAno.BackColor=btnKar.BackColor=btnEst.BackColor=btnPed.BackColor=Color.Empty;
		}
		
		protected void LlenarTabla()
		{
			DataRow dr;
			DataSet da;
			int n;
			ArrayList fTabs = new ArrayList();
			ArrayList fAtr = new ArrayList();
			ArrayList pAtr = new ArrayList();
			for(n=0;n<ds.Tables[1].Rows.Count;n++)
			{
				fTabs.Add(ds.Tables[1].Rows[n][1].ToString().Trim());//Nombre tablas foraneas
				fAtr.Add(ds.Tables[1].Rows[n][0].ToString().Trim());//Nombre campo referencia
				pAtr.Add(ds.Tables[1].Rows[n][2].ToString().Trim());//Nombre campo local
			}
			int p = 0;
			string nm;
			if(ds.Tables[2].Rows.Count > 0)
			{
				for(n=0;n<ds.Tables[0].Rows.Count;n++)
				{	
					//lb.Text+=ds.Tables[0].Rows[n][1].ToString()+" - "+ds.Tables[2].Rows[0][n].ToString()+"<br>";
					//Recorrer columnas que componen la tabla de la primer consulta, que son las columnas de la tabla consultada
					dr = dtInserts.NewRow();//Creamos la nueva fila
					nm = ds.Tables[0].Rows[n][0].ToString();//Colocamos el comentario de la columna que se encuentra en la base de datos
					string nomCol = ds.Tables[0].Rows[n][1].ToString().Trim();
					if(nm.Length==0)
						dr[0]="?";
					else
						dr[0]=ds.Tables[0].Rows[n][0];//Remarks
					if(nomCol == "MITE_CODIGO")
						dr[1] = txtRef.Text.Trim();
					else if(nomCol == "MITE_COSTREPO" || nomCol == "MSAL_COSTPROM" || nomCol == "MSAL_COSTPROMHIST" || nomCol == "MSAL_COSTHIST" || nomCol == "MSAL_COSTHISTHIST" || nomCol == "MSAL_ULTICOST" )
						dr[1] = Convert.ToDouble(ds.Tables[2].Rows[0][n]).ToString("C");//Valor
					else if(ds.Tables[0].Rows[n][2].ToString().Trim() == "DATE" && ds.Tables[2].Rows[0][n].ToString()!="")
						dr[1] = Convert.ToDateTime(ds.Tables[2].Rows[0][n]).ToString("yyyy-MM-dd");//Fecha
					else
						dr[1] = ds.Tables[2].Rows[0][n].ToString();//Valor
					p = pAtr.IndexOf(ds.Tables[0].Rows[n][1].ToString().Trim());//Es foranea?
					if(p>=0)
					{
						da = new DataSet();
						string tp = ds.Tables[0].Rows[n][2].ToString().Trim();//Que tipo de dato es la llave foranea
						string sqlp = "";
						if(fTabs[p].ToString() == "MPROVEEDOR")
							sqlp = "SELECT MNI.mnit_nit, MNI.mnit_nombres CONCAT ' ' CONCAT MNI.mnit_apellidos FROM mnit MNI, mproveedor MPR WHERE MNI.mnit_nit = MPR.mnit_nit AND MPR.mnit_nit='"+ds.Tables[2].Rows[0][n]+"'";
						else
						{
							if(tp == "VARCHAR" || tp == "CHAR")
								sqlp = "SELECT * from "+fTabs[p].ToString()+" WHERE "+fAtr[p].ToString()+"='"+ds.Tables[2].Rows[0][n]+"'";
							else
								sqlp = "SELECT * from "+fTabs[p].ToString()+" WHERE "+fAtr[p].ToString()+"="+ds.Tables[2].Rows[0][n]+"";
						}
						da = DBFunctions.Request(da,IncludeSchema.NO,sqlp);
						try{dr[2] = da.Tables[0].Rows[0][1].ToString();}
						catch{dr[2] = "No definido";};
					}
					dtInserts.Rows.Add(dr);
				}
			}
			else
			{
				//Si no se encontro informacion en la tabla respectiva sobre la solictud se coloca que no ahi información disponible
				dr = dtInserts.NewRow();
				dr[0] = "No hay Datos!";
				dtInserts.Rows.Add(dr);			
			}
		}
		
		//******************************************************************************
		//Aqui podemos observar los datos basicos sobre el item que tenemos seleccionado
		//******************************************************************************
		protected void GenDatBas(Object Sender, EventArgs E)
		{
			if(!RevisarNit())
				return;
			Esconder();
			plhDatosBasicos.Visible=true;
			string codI = "";
			if(!Referencias.Guardar(txtRef.Text,ref codI,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineas.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
				return;
			}
			string codITmp = "";
			Referencias.Editar(codI,ref codITmp,(ddlLineas.SelectedValue.Split('-'))[1]);
			if(codITmp != txtRef.Text)
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
				txtRef.Text = codITmp;
			}
			txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
			//Campos de la grilla que se mostrara en pantalla
			dtInserts = new DataTable();
			dtInserts.Columns.Add(new DataColumn("Propiedades", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Valor", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Significado", typeof(string)));
			//Traemos la estructura de la tabla de los items
			ds = new DataSet();
			ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='MITEMS' ORDER BY COLNO; "+
				"SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='MITEMS'; SELECT * FROM MITEMS WHERE mite_codigo='"+codI+"';");
			LlenarTabla();
            grMITEM.DataSource = dtInserts;
			grMITEM.DataBind();
			for(int i=1;i<=2;i++)
                for(int j=0;j<grMITEM.Items.Count;j++)
                    grMITEM.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			CrearReporte(grMITEM);
			toolsHolder.Visible = true;
			btnDatBas.BackColor = Color.LightGray;
		}
		
		//***********************************
		//Saldos y Costos del respectivo item
		protected void GenCants(Object Sender, EventArgs E)
		{
			if(!RevisarNit())
				return;
			Esconder();
			plhCantidades.Visible=true;
			string codI = "";
			if(!Referencias.Guardar(txtRef.Text,ref codI,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineas.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
				return;
			}
			string codITmp = "";
			Referencias.Editar(codI,ref codITmp,(ddlLineas.SelectedValue.Split('-'))[1]);
			if(codITmp != txtRef.Text)
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
				txtRef.Text = codITmp;
			}
			txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
			//MSALDOITEM
			dtInserts = new DataTable();
			dtInserts.Columns.Add(new DataColumn("propiedades", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Valor", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Significado", typeof(string)));
			ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='MSALDOITEM' ORDER BY COLNO; " +
				"SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='MSALDOITEM'; SELECT * FROM MSALDOITEM WHERE mite_codigo='"+codI+"' AND pano_ano="+ddlCantAno.SelectedValue+";");
			DataRow drT = dtInserts.NewRow();
			drT[0] = "TOTALES ACUMULADOS EMPRESA:";
			dtInserts.Rows.Add(drT);
			LlenarTabla();
			//MSALDOITEMALMACEN
			ds = new DataSet();
			ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='MSALDOITEMALMACEN'ORDER BY COLNO; "+
				"SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='MSALDOITEMALMACEN'; SELECT * FROM MSALDOITEMALMACEN WHERE mite_codigo='"+codI+"' AND pano_ano="+ddlCantAno.SelectedValue+" AND PALM_ALMACEN='"+ddlCantAlm.SelectedValue+"';");
			drT = dtInserts.NewRow();
			drT[0] = "ALMACEN:";
			dtInserts.Rows.Add(drT);
			LlenarTabla();
			grCants.DataSource = dtInserts;
			grCants.DataBind();
			for(int i=1;i<=2;i++)
				for(int j=0;j<grCants.Items.Count;j++)
					grCants.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			CrearReporte(grCants);
			toolsHolder.Visible = true;
			btnCants.BackColor = Color.LightGray;
		}
		
		//***********************************
		//Resumen del Mes del item respectivo
		protected void GenResMes(Object Sender, EventArgs E)
		{
			if(!RevisarNit())
				return;
			Esconder();
			plhResumenMes.Visible = true;
			string codI = "";
			if(!Referencias.Guardar(txtRef.Text,ref codI,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineas.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
				return;
			}
			string codITmp = "";
			Referencias.Editar(codI,ref codITmp,(ddlLineas.SelectedValue.Split('-'))[1]);
			if(codITmp != txtRef.Text)
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
				txtRef.Text = codITmp;
			}
			txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
			dtInserts = new DataTable();
			dtInserts.Columns.Add(new DataColumn("Tipo Movimiento/Sede", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Cantidad", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Costo", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Precio", typeof(string)));
			ds = new DataSet();
			ds = DBFunctions.Request(ds, IncludeSchema.NO,"SELECT t1.tmov_tipomovi,t1.macu_cantidad, t1.macu_costo,t1.macu_precio, t2.tmov_nombmovi from MACUMULADOITEM as t1, TMOVIKARD as t2 WHERE t1.tmov_tipomovi=t2.tmov_tipomovi AND t1.mite_codigo='"+codI+"' AND t1.pano_ano="+ddlResMesAno.SelectedValue+" AND t1.pmes_mes="+ddlResMesMes.SelectedValue+";");
			DataRow drT = dtInserts.NewRow();
			if(ds.Tables[0].Rows.Count<1)
			{
				drT = dtInserts.NewRow();
				drT[0] = "No hay datos:";
				dtInserts.Rows.Add(drT);
			}
			int n,m;
			DataSet da;
			for(n=0;n<ds.Tables[0].Rows.Count;n++)
			{
				drT = dtInserts.NewRow();
				drT[0] = ds.Tables[0].Rows[n][4].ToString().ToUpper()+" ("+ds.Tables[0].Rows[n][0].ToString()+")";
				dtInserts.Rows.Add(drT);
				drT = dtInserts.NewRow();
				drT[0] = "> Acumulado:";
				drT[1] = Convert.ToDouble(ds.Tables[0].Rows[n][1]).ToString("N");
				drT[2] = Convert.ToDouble(ds.Tables[0].Rows[n][2]).ToString("C");
				drT[3] = Convert.ToDouble(ds.Tables[0].Rows[n][3]).ToString("C");
				dtInserts.Rows.Add(drT);
				da = new DataSet();
                da = DBFunctions.Request(da, IncludeSchema.NO, "SELECT t2.PALM_DESCRIPCION, t1.palm_almacen,t1.macu_cantidad, t1.macu_costo,t1.macu_precio from MACUMULADOITEMALMACEN as t1, PALMACEN AS t2 WHERE tvig_vigencia='V' and t2.PALM_ALMACEN=t1.PALM_ALMACEN AND t1.mite_codigo='" + codI + "' AND t1.pano_ano=" + ddlResMesAno.SelectedValue + " AND t1.pmes_mes=" + ddlResMesMes.SelectedValue + " AND t1.tmov_tipomovi=" + ds.Tables[0].Rows[n][0] + ";");
				for(m=0;m<da.Tables[0].Rows.Count;m++)
				{
					drT = dtInserts.NewRow();
					drT[0] = ">> "+da.Tables[0].Rows[m][0].ToString()+":";
					drT[1] = Convert.ToDouble(da.Tables[0].Rows[m][2]).ToString("N");
					drT[2] = Convert.ToDouble(da.Tables[0].Rows[m][3]).ToString("C");
					drT[3] = Convert.ToDouble(da.Tables[0].Rows[m][4]).ToString("C");
					dtInserts.Rows.Add(drT);
				}
			}
			grResMes.DataSource = dtInserts;
			grResMes.DataBind();
			for(int i=1;i<=3;i++)
				for(int j=0;j<grResMes.Items.Count;j++)
					grResMes.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			CrearReporte(grResMes);
			toolsHolder.Visible = true;
			btnResMes.BackColor = Color.LightGray;
		}
	    
		//***********************************
		//Resumen del Año del item respectivo
		protected void GenResAno(Object Sender, EventArgs E)
		{
			if(!RevisarNit())
				return;
			Esconder();
			plhResumenAno.Visible=true;
			dtInserts = new DataTable();
			dtInserts.Columns.Add(new DataColumn("Tipo Movimiento / Sede", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Enero", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Febrero", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Marzo", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Abril", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Mayo", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Junio", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Julio", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Agosto", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Septiembre", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Octubre", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Noviembre", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Diciembre", typeof(string)));
			string codI = "";
			if(!Referencias.Guardar(txtRef.Text,ref codI,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineas.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
				return;
			}
			string codITmp = "";
			Referencias.Editar(codI,ref codITmp,(ddlLineas.SelectedValue.Split('-'))[1]);
			if(codITmp != txtRef.Text)
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
				txtRef.Text = codITmp;
			}
			txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
			ds = new DataSet();
			ds = DBFunctions.Request(ds, IncludeSchema.NO,"SELECT t1.tmov_tipomovi,t1.macu_cantidad, t1.macu_costo,t1.macu_precio, t2.tmov_nombmovi,t3.PMES_NOMBRE,t1.pmes_mes from MACUMULADOITEM as t1, TMOVIKARD as t2, PMES as t3 WHERE t1.tmov_tipomovi=t2.tmov_tipomovi AND t1.pmes_mes=t3.pmes_mes AND t1.mite_codigo='"+codI+"' AND t1.pano_ano="+ddlResAnoAno.SelectedValue+" order by t1.tmov_tipomovi, t1.pmes_mes;");
			DataRow drT1 = dtInserts.NewRow();
			DataRow drT2 = dtInserts.NewRow();
			int n;
			string mvA = "",mvN = "";
			DataSet da;
			int mes;
			if(ds.Tables[0].Rows.Count<1)
			{
				drT1 = dtInserts.NewRow();
				drT1[0] = "No hay datos:";
				dtInserts.Rows.Add(drT1);
			}
			else
			{
				for(n=0;n<ds.Tables[0].Rows.Count;n++)
				{	//Recorrer tipos y meses
					mvN = ds.Tables[0].Rows[n][4].ToString().ToUpper();//Nombre o Descripción del movimiento
					if(mvA != mvN)
					{	//TipoMovi
						if(mvA.Length > 0)
						{	//Titulo tipo movi
							dtInserts.Rows.Add(drT1);
							drT1=dtInserts.NewRow();
						}
						drT1[0] = mvN+" ("+ds.Tables[0].Rows[n][0].ToString()+")";
						dtInserts.Rows.Add(drT1);
						drT1 = dtInserts.NewRow();
						drT1[0] = "> Acumulado:";
						mvA = mvN;
						//Almacenes
						da = new DataSet();
                        da = DBFunctions.Request(da, IncludeSchema.NO, "SELECT t1.macu_cantidad, t1.macu_costo,t1.macu_precio,t1.palm_almacen,t1.pmes_mes, t2.palm_descripcion from MACUMULADOITEMALMACEN as t1, palmacen as t2 WHERE tvig_vigencia='V' and t1.mite_codigo='" + codI + "' AND t1.pano_ano=" + ddlResAnoAno.SelectedValue + " AND t1.tmov_tipomovi=" + ds.Tables[0].Rows[n][0] + " and t1.palm_almacen=t2.palm_almacen order by palm_almacen;");
						if(da.Tables[0].Rows.Count>0)
						{
							string iAlm = "",iAlmN = "";
							int mesA = 0;
							for(int m=0;m<da.Tables[0].Rows.Count;m++)
							{
								iAlmN = da.Tables[0].Rows[m][3].ToString().ToUpper();
								if(m>0 && iAlm!=iAlmN)
								{
									dtInserts.Rows.Add(drT2);
									drT2 = dtInserts.NewRow();
									iAlm = iAlmN;		    					
								}
								mesA = Convert.ToInt16(da.Tables[0].Rows[m][4]);//mes almacen
								drT2[0] = ">> "+da.Tables[0].Rows[m][5];
								drT2[mesA] = Convert.ToDouble(da.Tables[0].Rows[m][0]).ToString("N")+"<br>"+Convert.ToDouble(da.Tables[0].Rows[m][1]).ToString("C")+"<br>"+da.Tables[0].Rows[m][2];
								if(m == da.Tables[0].Rows.Count-1)
								{
									dtInserts.Rows.Add(drT2);
									drT2=dtInserts.NewRow();
								}
							}
						}
					}
					mes = Convert.ToInt16(ds.Tables[0].Rows[n][6]);//Acumulado del mes
					drT1[mes] = Convert.ToDouble(ds.Tables[0].Rows[n][1]).ToString("N")+"<br>"+Convert.ToDouble(ds.Tables[0].Rows[n][2]).ToString("C")+"<br>"+Convert.ToDouble(ds.Tables[0].Rows[n][3]).ToString("C");
				}
				if(n>0)
					dtInserts.Rows.Add(drT1);
			}
			//Por orden debemos llenar de ceros el resto de la tabla que no tenga datos
			for(int k=0;k<dtInserts.Rows.Count;k++)
			{
				if(dtInserts.Rows[k][0].ToString().IndexOf(">")!=-1)
				{
					for(int l=1;l<=12;l++)
						if(dtInserts.Rows[k][l].ToString() == "")
							dtInserts.Rows[k][l] = Convert.ToDouble("0").ToString("N")+"<br>"+Convert.ToDouble("0").ToString("C")+"<br>"+Convert.ToDouble("0").ToString("C");
				}
			}
			grResAno.DataSource=dtInserts;
			grResAno.DataBind();
			for(int k=0;k<grResAno.Items.Count;k++)
				for(int l=1;l<=12;l++)
					grResAno.Items[k].Cells[l].HorizontalAlign = HorizontalAlign.Right;
			CrearReporte(grResAno);
			toolsHolder.Visible = true;
			btnResAno.BackColor = Color.LightGray;
		}
        //***********************************
        //Consulta de Aplicación
        protected void GenApli(Object Sender, EventArgs E)
        {
            if (!RevisarNit())
                return;
            Esconder();
            plhApli.Visible = true;
            string codI = "";
            if (!Referencias.Guardar(txtRef.Text, ref codI, (ddlLineas.SelectedValue.Split('-'))[1]))
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
                return;
            }
            if (!Referencias.RevisionSustitucion(ref codI, (ddlLineas.SelectedValue.Split('-'))[0]))
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
                return;
            }
            string codITmp = "";
            Referencias.Editar(codI, ref codITmp, (ddlLineas.SelectedValue.Split('-'))[1]);
            if (codITmp != txtRef.Text)
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
                txtRef.Text = codITmp;
            }
            ds = new DataSet();
            string sqlMiteGrupo = "SELECT MIG.MITE_CODIGO, MIG.PGRU_GRUPO, PGC.PGRU_NOMBRE, MIG.MIG_CANTIDADUSO FROM MITEMSGRUPO MIG, PGRUPOCATALOGO PGC WHERE MIG.PGRU_GRUPO = PGC.PGRU_GRUPO AND MIG.MITE_CODIGO = '" + txtRef.Text + "';";
            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='MITEMSGRUPO' ORDER BY COLNO;" + sqlMiteGrupo + "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='PGRUPOCATALOGO' ORDER BY COLNO;");
            dtInserts = new DataTable();
            dtInserts = ds.Tables[1];
            dtInserts.Columns[0].ColumnName = ds.Tables[0].Rows[0][0].ToString();
            dtInserts.Columns[1].ColumnName = ds.Tables[0].Rows[1][0].ToString();
            dtInserts.Columns[2].ColumnName = ds.Tables[2].Rows[1][0].ToString();
            dtInserts.Columns[3].ColumnName = ds.Tables[0].Rows[2][0].ToString();
            grAplic.DataSource = dtInserts;
            grAplic.DataBind();      
        }

        // GridView.Sorting Event 
        protected void GenKarPru_Sorting(object sender, GridViewSortEventArgs e)
        {
            string strSortExpression = ViewState["SortExpression"].ToString();
            bool asc = false;
            if (strSortExpression.Contains("ASC"))
            {
                asc = true;
                strSortExpression = strSortExpression.Replace(" ASC", "");
            }
            else
                strSortExpression = strSortExpression.Replace(" DESC", "");

            // If the sorting column is the same as the previous one,  
            // then change the sort order. 
            if (strSortExpression == e.SortExpression)
            {
                if (asc)
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
                }
            }
            // If sorting column is another column,   
            // then specify the sort order to "Ascending". 
            else
            {
                ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
            }


            // Rebind the GridView control to show sorted data. 
            BindGridView(sender, e);
        }

        protected void pruebaMethod(Object Sender, GridViewPageEventArgs e)
        {
            // Cancel the paging operation if the user attempts to navigate
            // to another page while the GridView control is in edit mode. 
            if (GenKarPru.EditIndex != -1)
            {
                // Use the Cancel property to cancel the paging operation.
                e.Cancel = true;

                // Display an error message.
                int newPageNumber = e.NewPageIndex + 1;
                //Message.Text = "Please update the record before moving to page " +
                  //newPageNumber.ToString() + ".";
            }
            else
            {

                GenKarPru.PageIndex = e.NewPageIndex;
                //GenKarPru.DataBind();
                plhKar.Visible = true;
                GenKarPru.DataSource = Session["view"];
                GenKarPru.DataBind();
                //GenKarPru.SetPageIndex(e.NewPageIndex);
                //BindGridView(Sender, e);
                // Clear the error message.
               // Message.Text = "";
            }
            //GenKarPru.PageIndex = e.NewPageIndex;
            //BindGridView();
            //GenKarPru.DataSource = Session["view"];
            //BindGridView();
            //GenKarPru.DataBind();
            //Utils.MostrarAlerta(Response, "Hola Mundo!");
        }

        //***********************************
        //Consulta del Kardex

        public void BindGridView(Object Sender, EventArgs E)
        {
            Esconder();
            plhKar.Visible = true;
            // Get the connection string from Web.config.  
            // When we use Using statement,  
            // we don't need to explicitly dispose the object in the code,  
            // the using statement takes care of it. 

            // Create a DataSet object. 
            string codI = "";
            try
            {
                if (!Referencias.Guardar(txtRef.Text, ref codI, (ddlLineas.SelectedValue.Split('-'))[1]))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
                    return;
                }
                //if (!Referencias.RevisionSustitucion(ref codI, (ddlLineas.SelectedValue.Split('-'))[0]))
                //{
                //    Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
                //    return;
                //}
                string codITmp = "";
                Referencias.Editar(codI, ref codITmp, (ddlLineas.SelectedValue.Split('-'))[1]);
                if (codITmp != txtRef.Text)
                {
                    Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
                    txtRef.Text = codITmp;
                }
                txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='" + codI + "'");
            }
            catch (Exception)
            { }
            string sqlDI = "";
            DataSet sqlPru = new DataSet();

            sqlDI = String.Format(@"SELECT case when di.TMOV_TIPOMOVI in (11,30) then COALESCE(mf.mfac_prefdocu concat ' - ' concat mf.mfac_numedocu, 'No Definido') else '' end,  
                     di.PDOC_CODIGO  || ' ' ||COALESCE(PD.PDOC_NOMBRE,''), 
                     di.DITE_NUMEDOCU,   
                     di.MITE_CODIGO, 
                     di.DITE_PREFDOCUREFE,  
                     di.DITE_NUMEDOCUREFE,   
                     di.TMOV_TIPOMOVI|| ' ' ||COALESCE(t.tmov_NOMBmovi,''),    
                     vn.NIT          || ' ' ||COALESCE(VN.NOMBRE,''),  
                     di.PALM_ALMACEN || ' ' ||COALESCE(PA.PALM_DESCRIPCION,''),  
                     di.PANO_ANO,  
                     di.PMES_MES,   
                     di.DITE_FECHDOCU,  
                     di.DITE_CANTIDAD,  
                     di.DITE_VALOUNIT,   
                     di.DITE_COSTPROM,  
                     di.DITE_COSTPROMALMA,  
                     di.DITE_COSTPROMHIS,  
                     di.DITE_COSTPROMHISALMA,  
                     di.PIVA_PORCIVA,  
                     di.DITE_PORCDESC,   
                     di.PVEN_CODIGO  || ' ' ||COALESCE(PV.PVEN_NOMBRE,''),   
                     di.DITE_CANTDEVO,  
                     di.TCAR_CARGO,  
                     di.DITE_VALOPUBL,  
                     di.DITE_INVEINIC,  
                     di.DITE_INVEINICALMA,  
                     di.PCEN_CODIGO,  
                     di.DITE_PROCESO,  
                     mor.pdoc_codigo,  
                     mor.mord_numeorde,  
                     COALESCE(TC.TCAR_NOMBRE,'')  
                FROM DBXSCHEMA.DITEMS di   
                 LEFT JOIN mfacturaproveedor mf ON di.pdoc_codigo = mf.pdoc_codiordepago AND di.dite_numedocu = mf.mfac_numeordepago  
                 LEFT JOIN mordentransferencia mor 
                            LEFT JOIN TCARGOORDEN TC ON MOR.TCAR_CARGO = TC.TCAR_CARGO
                        ON di.pdoc_codigo= mor.pdoc_factura and di.dite_numedocu=mor.mfac_numero  
                 LEFT JOIN vmnit VN      ON DI.MNIT_NIT = VN.MNIT_nIT
                 LEFT JOIN palmacen pa   ON di.palm_almacen = pa.palm_almacen 
                 LEFT JOIN pDOCUMENTO PD ON di.pdoc_codigo = pd.pdoc_codigo 
                 LEFT JOIN tmovikard  t  ON di.TMOV_TIPOMOVI = t.TMOV_TIPOMOVI 
                 LEFT JOIN PVENDEDOR  PV ON di.PVEN_CODIGO = PV.PVEN_CODIGO 
               WHERE di.mite_codigo = '{0}' AND ", codI);
            if (rblKarTMov.SelectedIndex == 1)
                sqlDI += "tmov_tipomovi=" + ddlKarTMov.SelectedValue + " AND ";
            if (rblKarFec.SelectedIndex == 0)
                sqlDI += "pano_ano=" + ddlKarAno.SelectedValue;
            if (rblKarFec.SelectedIndex == 1)
                sqlDI += "pano_ano=" + ddlKarAno.SelectedValue + " AND pmes_mes=" + ddlKarMes.SelectedValue;
            if (rblKarFec.SelectedIndex == 2)
            {
                sqlDI += "DI.DITE_FECHDOCU BETWEEN '" + tbDate1.Text + "' AND '" + tbDate2.Text + "' ";
                /*DateTime f1 = Convert.ToDateTime(tbDate1.Text);//calDate1.SelectedDate;
                DateTime f2 = Convert.ToDateTime(tbDate2.Text);//calDate1.SelectedDate;
                DateTime fa;
                if (f2 > f1)
                {
                    fa = f1;
                    f1 = f2;
                    f2 = fa;
                }*/

                //sqlDI += "pano_ano >= " + f1.Year.ToString() + " AND pano_ano <= " + f2.Year.ToString() + " AND pmes_mes >= " + f1.Month.ToString() + " AND pmes_mes <= " + f2.Month.ToString();
            }
            if (rbCedes.SelectedIndex == 1)
            {
                sqlDI += " AND pa.palm_almacen='" + ddlCedes.SelectedValue + "' ";
            }
            // estas lineas son las que permiten que se cargue el Kardex dejando porfuera las pre-recepciones que aun han sido legalizadas 
            //    sqlDI += " AND DI.DITE_NUMEDOCU NOT IN  (SELECT D.DITE_NUMEDOCUREFE FROM DITEMS D WHERE D.mite_codigo = '" + codI + "'  AND D.TMOV_TIPOMOVI = '11' AND D.PDOC_CODIGO='EA-PK')";
            sqlDI += " ORDER BY  di.DITE_FECHDOCU, di.DITE_PROCESO;";

            try
            {
                ds = new DataSet();
                ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='DITEMS' ORDER BY COLNO;" + sqlDI);
                int n, m;
                dtInserts = new DataTable();
                for (n = 0; n < ds.Tables[0].Rows.Count; n++)
                    if (n == 0)
                        dtInserts.Columns.Add(new DataColumn("Documento Cruce", typeof(string)));
                    else
                    {
                        //if (ds.Tables[0].Rows[n]["COLTYPE"].ToString().Trim() == "INTEGER")
                        //{
                        //    dtInserts.Columns.Add(new DataColumn(ds.Tables[0].Rows[n][0].ToString(), typeof(int)));
                        //}
                        ////else if (ds.Tables[0].Rows[n]["COLTYPE"].ToString().Trim() == "SMALLINT")
                        ////{
                        ////    dtInserts.Columns.Add(new DataColumn(ds.Tables[0].Rows[n][0].ToString(), typeof(int)));
                        ////}
                        //else if(ds.Tables[0].Rows[n]["COLTYPE"].ToString().Contains("DECIMAL"))
                        //{
                        //    dtInserts.Columns.Add(new DataColumn(ds.Tables[0].Rows[n][0].ToString(), typeof(decimal)));
                        //}
                        //else
                        //{
                            dtInserts.Columns.Add(new DataColumn(ds.Tables[0].Rows[n][0].ToString(), typeof(string)));
                        //}
            }  
                if (ds.Tables.Count > 1)
                {
                    for (n = 0; n < ds.Tables[1].Rows.Count; n++)
                    {
                        DataRow drT = dtInserts.NewRow();
                        for (m = 0; m < ds.Tables[0].Rows.Count; m++)
                        {
                            if (ds.Tables[0].Rows[m][1].ToString().Trim() == "MITE_CODIGO")
                                drT[m] = txtRef.Text.Trim();
                            else
                            {
                                //Debemos revisar si este campo es una llave foranea
                                //if(DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'"))
                                //{
                                //    DataSet da = new DataSet();
                                //    string nombreTabla = DBFunctions.SingleData("SELECT reftbname FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'");
                                //    string pkNombre = DBFunctions.SingleData("SELECT pkcolnames FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'");
                                //    string tipoDato = ds.Tables[0].Rows[m][2].ToString().Trim();
                                //    if(nombreTabla == "MNIT")
                                //        drT[m] = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+ds.Tables[1].Rows[n][m]+"'");
                                //    else
                                //    {
                                //        if(tipoDato == "VARCHAR" || tipoDato == "CHAR" || tipoDato == "DATE")
                                //            DBFunctions.Request(da,IncludeSchema.NO,"SELECT * FROM "+nombreTabla+" WHERE "+pkNombre+"='"+ds.Tables[1].Rows[n][m]+"'");
                                //        else
                                //            DBFunctions.Request(da,IncludeSchema.NO,"SELECT * FROM "+nombreTabla+" WHERE "+pkNombre+"="+ds.Tables[1].Rows[n][m]+"");
                                //        try{drT[m]= da.Tables[0].Rows[0][1].ToString();}
                                //        catch{drT[m] = "No definido";}
                                //    }
                                //}
                                //else
                                {
                                    if (ds.Tables[0].Rows[m][2].ToString().Contains("DECIMAL") || ds.Tables[0].Rows[m][2].ToString().Trim() == "DOUBLE")
                                    {
                                        if (m >= 13 && m <= 17)
                                            drT[m] = Convert.ToDouble(ds.Tables[1].Rows[n][m]).ToString("C");
                                        else
                                            drT[m] = Convert.ToDouble(ds.Tables[1].Rows[n][m]).ToString("N");
                                    }
                                    else if (ds.Tables[0].Rows[m][2].ToString().Trim() == "DATE" || ds.Tables[0].Rows[m][2].ToString().Trim().IndexOf("TIME") != -1)
                                        drT[m] = Convert.ToDateTime(ds.Tables[1].Rows[n][m]).ToString("yyyy-MM-dd");
                                    else
                                    {
                                        int tipoMov = Convert.ToInt32(ds.Tables[1].Rows[n][6].ToString().Trim().Substring(0, 2));
                                        if ((tipoMov == 80 || tipoMov == 81) && m == 0)
                                            drT[m] = ds.Tables[1].Rows[n][28].ToString() + "-" +
                                                    ds.Tables[1].Rows[n][29].ToString() + "-" +
                                                    ds.Tables[1].Rows[n][30].ToString();
                                        else
                                            drT[m] = ds.Tables[1].Rows[n][m];
                                    }
                                }
                            }
                        }
                        dtInserts.Rows.Add(drT);

                        GenKarPru.DataSource = dtInserts;
                        Session["view1"] = dtInserts;


                        GenKarPru.DataBind();
                        for (int k = 0; k < GenKarPru.Rows.Count; k++)
                            for (int l = 12; l <= 27; l++)
                            {
                                if ((l >= 12 && l <= 19) || l == 21 || (l >= 23 && l <= 25))
                                    GenKarPru.Rows[k].Cells[l].HorizontalAlign = HorizontalAlign.Right;
                            }
                    }


                    //sqlPru = DBFunctions.Request(sqlPru, IncludeSchema.NO, sqlDI);
                    // Get the DataView from Person DataTable. 
                    DataView dvPerson = dtInserts.DefaultView;
                    //dvPerson.Table.Columns[3].Expression = "Numero";
                    dvPerson.Sort = ViewState["SortExpression"].ToString();

                    // Bind the GridView control. 
                    GenKarPru.DataSource = dvPerson;
                    Session["view"] = dvPerson;
                    //GridView a = new GridView ();                                                     
                    GenKarPru.DataBind();
                    // Set the sort column and sort order. 
                    //toolsHolder.Visible = true;

                }
            }
            catch (Exception i)
            { }
        }
        /// <summary>
       //////***************************
       ///drag and drop
       ///
       private void GenKarPru_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                            // Proceed with the drag and drop, passing in the list item.                    
                           // System.Windows.Forms.DragDropEffects dropEffect = GenKarPru.DoDragDrop(GenKarPru.Rows[rowIndexFromMouseDown],
                         // System.Windows.Forms.DragDropEffects.Move);
                }
            }
        }
 
private void GenKarPru_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
{
            // Get the index of the item the mouse is below.
            //rowIndexFromMouseDown = GenKarPru.HitTest(e.X, e.Y).RowIndex;
 
    if (rowIndexFromMouseDown != -1)
    {
        // Remember the point where the mouse down occurred. 
    // The DragSize indicates the size that the mouse can move 
    // before a drag event should be started.                
        Size dragSize = System.Windows.Forms.SystemInformation.DragSize;
 
        // Create a rectangle using the DragSize, with the mouse position being
        // at the center of the rectangle.
        dragBoxFromMouseDown = new Rectangle(
                  new Point(
                    e.X - (dragSize.Width / 2),
                    e.Y - (dragSize.Height / 2)),
              dragSize);
    }
    else
        // Reset the rectangle if the mouse is not over an item in the ListBox.
        dragBoxFromMouseDown = Rectangle.Empty;
}
 
private void GenKarPru_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
{
    e.Effect = System.Windows.Forms.DragDropEffects.Move;
}
 
//private void GenKarPru_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
//{
//    // The mouse locations are relative to the screen, so they must be 
//    // converted to client coordinates.
//    Point clientPoint = GenKarPru.PointToClient(new Point(e.X, e.Y));
 
//    // Get the row index of the item the mouse is below. 
//    rowIndexOfItemUnderMouseToDrop = GenKarPru.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
 
//    // If the drag operation was a move then remove and insert the row.
//    if (e.Effect== System.Windows.Forms.DragDropEffects.Move)
//    {
//        System.Windows.Forms.DataGridViewRow rowToMove = e.Data.GetData(typeof(System.Windows.Forms.DataGridViewRow)) as System.Windows.Forms.DataGridViewRow;
//        GenKarPru.Rows.RemoveAt(rowIndexFromMouseDown);
//        GenKarPru.Rows.inserts(rowIndexOfItemUnderMouseToDrop, rowToMove);
 
//    }
//}
        
        

        //***********************************
        //Consulta del Kardex
        protected void GenKar(Object Sender, EventArgs E)
        {
            //BindGridView();

            if (!RevisarNit())
                return;
            Esconder();
            plhKar.Visible = true;
            string codI = "";
            if (!Referencias.Guardar(txtRef.Text, ref codI, (ddlLineas.SelectedValue.Split('-'))[1]))
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
                return;
            }
            if (!Referencias.RevisionSustitucion(ref codI, (ddlLineas.SelectedValue.Split('-'))[0]))
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
                return;
            }
            string codITmp = "";
            Referencias.Editar(codI, ref codITmp, (ddlLineas.SelectedValue.Split('-'))[1]);
            if (codITmp != txtRef.Text)
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
                txtRef.Text = codITmp;
            }
            txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='" + codI + "'");

            string sqlDI = String.Format(@"SELECT case when di.TMOV_TIPOMOVI in (11,30) then COALESCE(mf.mfac_prefdocu concat ' - ' concat mf.mfac_numedocu, 'No Definido') else '' end,  
                     di.PDOC_CODIGO  || ' ' ||COALESCE(PD.PDOC_NOMBRE,''), 
                     di.DITE_NUMEDOCU,   
                     di.MITE_CODIGO, 
                     di.DITE_PREFDOCUREFE,  
                     di.DITE_NUMEDOCUREFE,   
                     di.TMOV_TIPOMOVI|| ' ' ||COALESCE(t.tmov_NOMBmovi,''),    
                     vn.NIT          || ' ' ||COALESCE(VN.NOMBRE,''),  
                     di.PALM_ALMACEN || ' ' ||COALESCE(PA.PALM_DESCRIPCION,''),  
                     di.PANO_ANO,  
                     di.PMES_MES,   
                     di.DITE_FECHDOCU,  
                     di.DITE_CANTIDAD,  
                     di.DITE_VALOUNIT,   
                     di.DITE_COSTPROM,  
                     di.DITE_COSTPROMALMA,  
                     di.DITE_COSTPROMHIS,  
                     di.DITE_COSTPROMHISALMA,  
                     di.PIVA_PORCIVA,  
                     di.DITE_PORCDESC,   
                     di.PVEN_CODIGO  || ' ' ||COALESCE(PV.PVEN_NOMBRE,''),   
                     di.DITE_CANTDEVO,  
                     di.TCAR_CARGO,  
                     di.DITE_VALOPUBL,  
                     di.DITE_INVEINIC,  
                     di.DITE_INVEINICALMA,  
                     di.PCEN_CODIGO,  
                     di.DITE_PROCESO,  
                     mor.pdoc_codigo,  
                     mor.mord_numeorde,  
                     COALESCE(TC.TCAR_NOMBRE,'')  
                FROM DBXSCHEMA.DITEMS di   
                 LEFT JOIN mfacturaproveedor mf ON di.pdoc_codigo = mf.pdoc_codiordepago AND di.dite_numedocu = mf.mfac_numeordepago  
                 LEFT JOIN mordentransferencia mor 
                            LEFT JOIN TCARGOORDEN TC ON MOR.TCAR_CARGO = TC.TCAR_CARGO
                        ON di.pdoc_codigo= mor.pdoc_factura and di.dite_numedocu=mor.mfac_numero  
                 LEFT JOIN vmnit VN      ON DI.MNIT_NIT = VN.MNIT_nIT
                 LEFT JOIN palmacen pa   ON di.palm_almacen = pa.palm_almacen 
                 LEFT JOIN pDOCUMENTO PD ON di.pdoc_codigo = pd.pdoc_codigo 
                 LEFT JOIN tmovikard  t  ON di.TMOV_TIPOMOVI = t.TMOV_TIPOMOVI 
                 LEFT JOIN PVENDEDOR  PV ON di.PVEN_CODIGO = PV.PVEN_CODIGO 
               WHERE di.mite_codigo = '{0}' AND ", codI);
            if (rblKarTMov.SelectedIndex == 1)
                sqlDI += "di.tmov_tipomovi=" + ddlKarTMov.SelectedValue + " AND ";
            if (rblKarFec.SelectedIndex == 0)
                sqlDI += "pano_ano=" + ddlKarAno.SelectedValue;
            if (rblKarFec.SelectedIndex == 1)
                sqlDI += "pano_ano=" + ddlKarAno.SelectedValue + " AND pmes_mes=" + ddlKarMes.SelectedValue;
            if (rblKarFec.SelectedIndex == 2)
            {
                DateTime f1 = calDate1.SelectedDate;
                DateTime f2 = calDate1.SelectedDate;
                DateTime fa;
                if (f2 > f1)
                {
                    fa = f1;
                    f1 = f2;
                    f2 = fa;
                }
                sqlDI += "pano_ano>=" + f1.Year.ToString() + " AND pano_ano<=" + f2.Year.ToString() + " AND pmes_mes>=" + f1.Month.ToString() + " AND pmes_mes<=" + f2.Month.ToString();
            }
            if (rbCedes.SelectedIndex == 1)
            {
                sqlDI += " AND pa.palm_almacen='" + ddlCedes.SelectedValue + "' ";
            }
            // estas lineas son las que permiten que se cargue el Kardex dejando porfuera las pre-recepciones que aun han sido legalizadas 
        //    sqlDI += " AND DI.DITE_NUMEDOCU NOT IN  (SELECT D.DITE_NUMEDOCUREFE FROM DITEMS D WHERE D.mite_codigo = '" + codI + "'  AND D.TMOV_TIPOMOVI = '11' AND D.PDOC_CODIGO='EA-PK')";
            sqlDI += " ORDER BY  di.DITE_FECHDOCU, di.DITE_PROCESO;";  // se debe ordenar por cualquier columna

            ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='DITEMS' ORDER BY COLNO;" + sqlDI);
            int n, m;
            dtInserts = new DataTable();
            for (n = 0; n < ds.Tables[0].Rows.Count; n++)
                if (n == 0)
                    dtInserts.Columns.Add(new DataColumn("Documento Cruce", typeof(string)));
                else
                    dtInserts.Columns.Add(new DataColumn(ds.Tables[0].Rows[n][0].ToString(), typeof(string)));
            if (ds.Tables.Count > 1)
            {
                for (n = 0; n < ds.Tables[1].Rows.Count; n++)
                {
                    DataRow drT = dtInserts.NewRow();
                    for (m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {
                        if (ds.Tables[0].Rows[m][1].ToString().Trim() == "MITE_CODIGO")
                            drT[m] = txtRef.Text.Trim();
                        else
                        {
                            //Debemos revisar si este campo es una llave foranea
                            //if(DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'"))
                            //{
                            //    DataSet da = new DataSet();
                            //    string nombreTabla = DBFunctions.SingleData("SELECT reftbname FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'");
                            //    string pkNombre = DBFunctions.SingleData("SELECT pkcolnames FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'");
                            //    string tipoDato = ds.Tables[0].Rows[m][2].ToString().Trim();
                            //    if(nombreTabla == "MNIT")
                            //        drT[m] = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+ds.Tables[1].Rows[n][m]+"'");
                            //    else
                            //    {
                            //        if(tipoDato == "VARCHAR" || tipoDato == "CHAR" || tipoDato == "DATE")
                            //            DBFunctions.Request(da,IncludeSchema.NO,"SELECT * FROM "+nombreTabla+" WHERE "+pkNombre+"='"+ds.Tables[1].Rows[n][m]+"'");
                            //        else
                            //            DBFunctions.Request(da,IncludeSchema.NO,"SELECT * FROM "+nombreTabla+" WHERE "+pkNombre+"="+ds.Tables[1].Rows[n][m]+"");
                            //        try{drT[m]= da.Tables[0].Rows[0][1].ToString();}
                            //        catch{drT[m] = "No definido";}
                            //    }
                            //}
                            //else
                            {
                                if (ds.Tables[0].Rows[m][2].ToString().Trim() == "DECIMAL" || ds.Tables[0].Rows[m][2].ToString().Trim() == "DOUBLE")
                                {
                                    if (m >= 13 && m <= 17)
                                        drT[m] = Convert.ToDouble(ds.Tables[1].Rows[n][m]).ToString("C");
                                    else
                                        drT[m] = Convert.ToDouble(ds.Tables[1].Rows[n][m]).ToString("N");
                                }
                                else if (ds.Tables[0].Rows[m][2].ToString().Trim() == "DATE" || ds.Tables[0].Rows[m][2].ToString().Trim().IndexOf("TIME") != -1)
                                    drT[m] = Convert.ToDateTime(ds.Tables[1].Rows[n][m]).ToString("yyyy-MM-dd");
                                else
                                {
                                    int tipoMov = Convert.ToInt32(ds.Tables[1].Rows[n][6].ToString().Trim().Substring(0, 2));
                                    if ((tipoMov == 80 || tipoMov == 81) && m == 0)
                                        drT[m] = ds.Tables[1].Rows[n][28].ToString() + "-" +
                                                ds.Tables[1].Rows[n][29].ToString() + "-" +
                                                ds.Tables[1].Rows[n][30].ToString();
                                    else
                                        drT[m] = ds.Tables[1].Rows[n][m];
                                }
                            }
                        }
                    }
                    dtInserts.Rows.Add(drT);
                }
                //grKar.AllowPaging = true;
                //grKar.AllowCustomPaging = true;
                //grKar.AllowSorting = true;
                grKar.DataSource = dtInserts;
                grKar.DataBind();
                for (int k = 0; k < grKar.Items.Count; k++)
                    for (int l = 12; l <= 27; l++)
                    {
                        if ((l >= 12 && l <= 19) || l == 21 || (l >= 23 && l <= 25))
                            grKar.Items[k].Cells[l].HorizontalAlign = HorizontalAlign.Right;
                    }
                CrearReporte(grKar);
                toolsHolder.Visible = true;
                btnKar.BackColor = Color.LightGray;
            }
        }

    	//Kardex num 2
//        protected void GenKar2(Object Sender, EventArgs E)
//        {
//            if (!RevisarNit())
//                return;
//            Esconder();
//            plhKar.Visible = true;
//            string codI = "";
//            if (!Referencias.Guardar(txtRef.Text, ref codI, (ddlLineas.SelectedValue.Split('-'))[1]))
//            {
//                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
//                return;
//            }
//            if (!Referencias.RevisionSustitucion(ref codI, (ddlLineas.SelectedValue.Split('-'))[0]))
//            {
//                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
//                return;
//            }
//            string codITmp = "";
//            Referencias.Editar(codI, ref codITmp, (ddlLineas.SelectedValue.Split('-'))[1]);
//            if (codITmp != txtRef.Text)
//            {
//                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
//                txtRef.Text = codITmp;
//            }
//            txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='" + codI + "'");

//            string sqlDI = String.Format(@"SELECT case when di.TMOV_TIPOMOVI in (11,30) then COALESCE(mf.mfac_prefdocu concat ' - ' concat mf.mfac_numedocu, 'No Definido') else '' end,  
//                     di.PDOC_CODIGO  || ' ' ||COALESCE(PD.PDOC_NOMBRE,''), 
//                     di.DITE_NUMEDOCU,   
//                     di.MITE_CODIGO, 
//                     di.DITE_PREFDOCUREFE,  
//                     di.DITE_NUMEDOCUREFE,   
//                     di.TMOV_TIPOMOVI|| ' ' ||COALESCE(t.tmov_NOMBmovi,''),    
//                     vn.NIT          || ' ' ||COALESCE(VN.NOMBRE,''),  
//                     di.PALM_ALMACEN || ' ' ||COALESCE(PA.PALM_DESCRIPCION,''),  
//                     di.PANO_ANO,  
//                     di.PMES_MES,   
//                     di.DITE_FECHDOCU,  
//                     di.DITE_CANTIDAD,  
//                     di.DITE_VALOUNIT,   
//                     di.DITE_COSTPROM,  
//                     di.DITE_COSTPROMALMA,  
//                     di.DITE_COSTPROMHIS,  
//                     di.DITE_COSTPROMHISALMA,  
//                     di.PIVA_PORCIVA,  
//                     di.DITE_PORCDESC,   
//                     di.PVEN_CODIGO  || ' ' ||COALESCE(PV.PVEN_NOMBRE,''),   
//                     di.DITE_CANTDEVO,  
//                     di.TCAR_CARGO,  
//                     di.DITE_VALOPUBL,  
//                     di.DITE_INVEINIC,  
//                     di.DITE_INVEINICALMA,  
//                     di.PCEN_CODIGO,  
//                     di.DITE_PROCESO,  
//                     mor.pdoc_codigo,  
//                     mor.mord_numeorde,  
//                     COALESCE(TC.TCAR_NOMBRE,'')  
//                FROM DBXSCHEMA.DITEMS di   
//                 LEFT JOIN mfacturaproveedor mf ON di.pdoc_codigo = mf.pdoc_codiordepago AND di.dite_numedocu = mf.mfac_numeordepago  
//                 LEFT JOIN mordentransferencia mor 
//                            LEFT JOIN TCARGOORDEN TC ON MOR.TCAR_CARGO = TC.TCAR_CARGO
//                        ON di.pdoc_codigo= mor.pdoc_factura and di.dite_numedocu=mor.mfac_numero  
//                 LEFT JOIN vmnit VN      ON DI.MNIT_NIT = VN.MNIT_nIT
//                 LEFT JOIN palmacen pa   ON di.palm_almacen = pa.palm_almacen 
//                 LEFT JOIN pDOCUMENTO PD ON di.pdoc_codigo = pd.pdoc_codigo 
//                 LEFT JOIN tmovikard  t  ON di.TMOV_TIPOMOVI = t.TMOV_TIPOMOVI 
//                 LEFT JOIN PVENDEDOR  PV ON di.PVEN_CODIGO = PV.PVEN_CODIGO 
//               WHERE di.mite_codigo = '{0}' AND ", codI);
//            if (rblKarTMov.SelectedIndex == 1)
//                sqlDI += "tmov_tipomovi=" + ddlKarTMov.SelectedValue + " AND ";
//            if (rblKarFec.SelectedIndex == 0)
//                sqlDI += "pano_ano=" + ddlKarAno.SelectedValue;
//            if (rblKarFec.SelectedIndex == 1)
//                sqlDI += "pano_ano=" + ddlKarAno.SelectedValue + " AND pmes_mes=" + ddlKarMes.SelectedValue;
//            if (rblKarFec.SelectedIndex == 2)
//            {
//                DateTime f1 = calDate1.SelectedDate;
//                DateTime f2 = calDate1.SelectedDate;
//                DateTime fa;
//                if (f2 > f1)
//                {
//                    fa = f1;
//                    f1 = f2;
//                    f2 = fa;
//                }
//                sqlDI += "pano_ano>=" + f1.Year.ToString() + " AND pano_ano<=" + f2.Year.ToString() + " AND pmes_mes>=" + f1.Month.ToString() + " AND pmes_mes<=" + f2.Month.ToString();
//            }
//            if (rbCedes.SelectedIndex == 1)
//            {
//                sqlDI += " AND pa.palm_almacen='" + ddlCedes.SelectedValue + "' ";
//            }
//            // estas lineas son las que permiten que se cargue el Kardex dejando porfuera las pre-recepciones que aun han sido legalizadas 
//            //    sqlDI += " AND DI.DITE_NUMEDOCU NOT IN  (SELECT D.DITE_NUMEDOCUREFE FROM DITEMS D WHERE D.mite_codigo = '" + codI + "'  AND D.TMOV_TIPOMOVI = '11' AND D.PDOC_CODIGO='EA-PK')";
//            sqlDI += " ORDER BY  di.DITE_FECHDOCU, di.DITE_PROCESO;";  // se debe ordenar por cualquier columna

//            ds = new DataSet();
//            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='DITEMS' ORDER BY COLNO;" + sqlDI);
//            int n, m;
//            dtInserts = new DataTable();
//            for (n = 0; n < ds.Tables[0].Rows.Count; n++)
//                if (n == 0)
//                    dtInserts.Columns.Add(new DataColumn("Documento Cruce", typeof(string)));
//                else
//                    dtInserts.Columns.Add(new DataColumn(ds.Tables[0].Rows[n][0].ToString(), typeof(string)));
//            if (ds.Tables.Count > 1)
//            {
//                for (n = 0; n < ds.Tables[1].Rows.Count; n++)
//                {
//                    DataRow drT = dtInserts.NewRow();
//                    for (m = 0; m < ds.Tables[0].Rows.Count; m++)
//                    {
//                        if (ds.Tables[0].Rows[m][1].ToString().Trim() == "MITE_CODIGO")
//                            drT[m] = txtRef.Text.Trim();
//                        else
//                        {
//                            //Debemos revisar si este campo es una llave foranea
//                            //if(DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'"))
//                            //{
//                            //    DataSet da = new DataSet();
//                            //    string nombreTabla = DBFunctions.SingleData("SELECT reftbname FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'");
//                            //    string pkNombre = DBFunctions.SingleData("SELECT pkcolnames FROM sysibm.sysrels WHERE tbname='DITEMS' AND fkcolnames LIKE '%"+ds.Tables[0].Rows[m][1].ToString().Trim()+"%'");
//                            //    string tipoDato = ds.Tables[0].Rows[m][2].ToString().Trim();
//                            //    if(nombreTabla == "MNIT")
//                            //        drT[m] = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+ds.Tables[1].Rows[n][m]+"'");
//                            //    else
//                            //    {
//                            //        if(tipoDato == "VARCHAR" || tipoDato == "CHAR" || tipoDato == "DATE")
//                            //            DBFunctions.Request(da,IncludeSchema.NO,"SELECT * FROM "+nombreTabla+" WHERE "+pkNombre+"='"+ds.Tables[1].Rows[n][m]+"'");
//                            //        else
//                            //            DBFunctions.Request(da,IncludeSchema.NO,"SELECT * FROM "+nombreTabla+" WHERE "+pkNombre+"="+ds.Tables[1].Rows[n][m]+"");
//                            //        try{drT[m]= da.Tables[0].Rows[0][1].ToString();}
//                            //        catch{drT[m] = "No definido";}
//                            //    }
//                            //}
//                            //else
//                            {
//                                if (ds.Tables[0].Rows[m][2].ToString().Trim() == "DECIMAL" || ds.Tables[0].Rows[m][2].ToString().Trim() == "DOUBLE")
//                                {
//                                    if (m >= 13 && m <= 17)
//                                        drT[m] = Convert.ToDouble(ds.Tables[1].Rows[n][m]).ToString("C");
//                                    else
//                                        drT[m] = Convert.ToDouble(ds.Tables[1].Rows[n][m]).ToString("N");
//                                }
//                                else if (ds.Tables[0].Rows[m][2].ToString().Trim() == "DATE" || ds.Tables[0].Rows[m][2].ToString().Trim().IndexOf("TIME") != -1)
//                                    drT[m] = Convert.ToDateTime(ds.Tables[1].Rows[n][m]).ToString("yyyy-MM-dd");
//                                else
//                                {
//                                    int tipoMov = Convert.ToInt32(ds.Tables[1].Rows[n][6].ToString().Trim().Substring(0, 2));
//                                    if ((tipoMov == 80 || tipoMov == 81) && m == 0)
//                                        drT[m] = ds.Tables[1].Rows[n][28].ToString() + "-" +
//                                                ds.Tables[1].Rows[n][29].ToString() + "-" +
//                                                ds.Tables[1].Rows[n][30].ToString();
//                                    else
//                                        drT[m] = ds.Tables[1].Rows[n][m];
//                                }
//                            }
//                        }
//                    }
//                    dtInserts.Rows.Add(drT);
//                }
//                //grKar.AllowPaging = true;
//                //grKar.AllowCustomPaging = true;
//                //grKar.AllowSorting = true;
//                grKar2.DataSource = dtInserts;
//                grKar2.DataBind();
//                for (int k = 0; k < grKar2.Items.Count; k++)
//                    for (int l = 12; l <= 27; l++)
//                    {
//                        if ((l >= 12 && l <= 19) || l == 21 || (l >= 23 && l <= 25))
//                            grKar2.Items[k].Cells[l].HorizontalAlign = HorizontalAlign.Right;
//                    }
//                CrearReporte(grKar2);
//                toolsHolder.Visible = true;
//                btnKar.BackColor = Color.LightGray;
//            }
//        }
		//***********************************
		//Consulta de Estadisticas
		protected void GenEst(Object Sender, EventArgs E)
		{
			if(!RevisarNit())
				return;
			Esconder();
			plhEst.Visible = true;
			int n;
			ds = new DataSet();
			dtInserts = new DataTable();
			dtInserts.Columns.Add(new DataColumn("Sede", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Enero", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Febrero", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Marzo", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Abril", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Mayo", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Junio", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Julio", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Agosto", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Septiembre", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Octubre", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Noviembre", typeof(string)));
			dtInserts.Columns.Add(new DataColumn("Diciembre", typeof(string)));
			string codI = "";
			if(!Referencias.Guardar(txtRef.Text,ref codI,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineas.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
				return;
			}
			string codITmp = "";
			Referencias.Editar(codI,ref codITmp,(ddlLineas.SelectedValue.Split('-'))[1]);
			if(codITmp != txtRef.Text)
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
				txtRef.Text = codITmp;
			}
			txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pmes_mes, mDEM_cantidad from MDEMANDAITEM where mite_codigo='" + codI + "' AND pano_ano=" + ddlEstAno.SelectedValue + " ;" + "SELECT t1.pmes_mes,t1.mDEM_cantidad,t1.palm_almacen,t2.palm_descripcion from MDEMANDAITEMALMACEN as t1, palmacen as t2 where tvig_vigencia='V' and t1.palm_almacen=t2.palm_almacen and t1.mite_codigo='" + txtRef.Text + "' AND t1.pano_ano=" + ddlEstAno.SelectedValue + "  order by t1.palm_almacen;");
			DataRow drT1 = dtInserts.NewRow();
			DataRow drT2 = dtInserts.NewRow();
			int mes = 0;
			if(ds.Tables[0].Rows.Count == 0)
			{
				drT1 = dtInserts.NewRow();
				drT1[0] = "Acumulado: No hay Datos!";
			}
			else
			{
				drT1[0] = "Acumulado:";
				for(n=0;n<ds.Tables[0].Rows.Count;n++)
				{
					mes = Convert.ToInt16(ds.Tables[0].Rows[n][0]);//Acumulado del mes
					drT1[mes] = ds.Tables[0].Rows[n][1];
				}
			}
			dtInserts.Rows.Add(drT1);    	
			if(ds.Tables[1].Rows.Count == 0)
			{
				drT2 = dtInserts.NewRow();
				drT2[0] = "Almacen: No hay Datos!";
			}
			else
			{
				string iAlm = "", iAlmN = "";
				for(int m=0;m<ds.Tables[1].Rows.Count;m++)
				{
					iAlmN = ds.Tables[1].Rows[m][2].ToString().ToUpper();
					if(m>0 && iAlm!=iAlmN)
					{
						dtInserts.Rows.Add(drT2);
						drT2 = dtInserts.NewRow();
						iAlm = iAlmN;		    					
					}
					mes = Convert.ToInt16(ds.Tables[1].Rows[m][0]);//mes almacen
					drT2[0] = ds.Tables[1].Rows[m][3]+":";
					drT2[mes] = ds.Tables[1].Rows[m][1];
					if(m == ds.Tables[1].Rows.Count-1)
					{
						dtInserts.Rows.Add(drT2);
						drT2 = dtInserts.NewRow();
					}
				}
			}
			//Revisamos si se requiere llenar la tabla con ceros o no
			for(int k=0;k<dtInserts.Rows.Count;k++)
				for(int l=1;l<=12;l++)
					if(dtInserts.Rows[k][l].ToString() == "")
						dtInserts.Rows[k][l] = Convert.ToDouble("0").ToString("N");
			grEst.DataSource = dtInserts;
			grEst.DataBind();
			for(int k=0;k<grEst.Items.Count;k++)
				for(int l=1;l<=12;l++)
					grEst.Items[k].Cells[l].HorizontalAlign = HorizontalAlign.Right;
			CrearReporte(grEst);
			toolsHolder.Visible = true;
			btnEst.BackColor = Color.LightGray;
		}
		//***********************************
		//Generar Consulta de Distribucion
		protected void GenDisAno(Object Sender, EventArgs E){
			string codI = "";
			if(!Referencias.Guardar(txtRef.Text,ref codI,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineas.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
				return;
			}
			string codITmp = "";
			Referencias.Editar(codI,ref codITmp,(ddlLineas.SelectedValue.Split('-'))[1]);
            if (codITmp != txtRef.Text)
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
				txtRef.Text = codITmp;
			}
			txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
			string sqlDI = @"SELECT ms.PALM_ALMACEN, pa.PALM_DESCRIPCION, coalesce(ms.MSAL_CANTASIG,0), coalesce(ms.MSAL_CANTACTUAL,0),  
				       coalesce(ms.MSAL_COSTPROM,0), coalesce(ms.MSAL_COSTPROMHIST,0), coalesce(ms.MSAL_CANTPENDIENTE,0), coalesce(ms.MSAL_CANTTRANSITO,0), coalesce(ms.MSAL_CANTINVEINIC,0)  
				 FROM  PALMACEN pa, MSALDOITEMALMACEN ms  
				 WHERE ms.PANO_ANO="+ddlAnoDistrib.SelectedValue+" AND pa.PALM_ALMACEN=ms.PALM_ALMACEN AND ms.MITE_CODIGO='"+codI+"';";
			ds = new DataSet();
			ds = DBFunctions.Request(ds, IncludeSchema.NO, sqlDI);
			dtInserts = new DataTable();
			dtInserts.Columns.Add("Cod.", typeof(string));
			dtInserts.Columns.Add("Almacén", typeof(string));
			dtInserts.Columns.Add("Cant. Asignada", typeof(string));
			dtInserts.Columns.Add("Cant. Actual", typeof(string));
			dtInserts.Columns.Add("Costo Prom.", typeof(string));
			dtInserts.Columns.Add("Costo Prom. Hist.", typeof(string));
			dtInserts.Columns.Add("Cant. Pend.", typeof(string));
			dtInserts.Columns.Add("Cant. Trans.", typeof(string));
			dtInserts.Columns.Add("Cant. Inv. Ini.", typeof(string));
			DataRow dr;
			if(ds.Tables[0].Rows.Count == 0)
			{
				dr = dtInserts.NewRow();
				dr[0] = "No hay Datos!";
				dtInserts.Rows.Add(dr);
			}
			else
			{
				for(int n=0;n<ds.Tables[0].Rows.Count;n++)
				{
					dr = dtInserts.NewRow();
					dr[0] = ds.Tables[0].Rows[n][0];//Cod
					dr[1] = ds.Tables[0].Rows[n][1];//Descripcion
					dr[2] = Convert.ToDouble(ds.Tables[0].Rows[n][2]).ToString("N");//Cant. Asig
					dr[3] = Convert.ToDouble(ds.Tables[0].Rows[n][3]).ToString("N");//Cant. Act
					dr[4] = Convert.ToDouble(ds.Tables[0].Rows[n][4]).ToString("C");//Cost. Prom
					dr[5] = Convert.ToDouble(ds.Tables[0].Rows[n][5]).ToString("C");//Cost. Prom Hist.
					dr[6] = Convert.ToDouble(ds.Tables[0].Rows[n][6]).ToString("N");//Cant. Pend
					dr[7] = Convert.ToDouble(ds.Tables[0].Rows[n][7]).ToString("N");//Cant. Trans
					dr[8] = Convert.ToDouble(ds.Tables[0].Rows[n][8]).ToString("N");//Cant. Inve Ini
					dtInserts.Rows.Add(dr);
				}
			}
			grDistrib.DataSource=dtInserts;
			grDistrib.DataBind();
			for(int k=0;k<grPed.Items.Count;k++)
				for(int l=2;l<9;l++)
					grPed.Items[k].Cells[l].HorizontalAlign = HorizontalAlign.Right;
			CrearReporte(grPed);
			toolsHolder.Visible=true;
			btnPed.BackColor = Color.LightGray;
		}
		//***********************************
		//Consulta de Distribucion
		protected void GenDist(Object Sender, EventArgs E)
		{
			if(!RevisarNit())
				return;
			Esconder();
			plhDistrib.Visible=true;
			string codI = "";
			if(!Referencias.Guardar(txtRef.Text,ref codI,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
				return;
			}
			if(!Referencias.RevisionSustitucion(ref codI,(ddlLineas.SelectedValue.Split('-'))[0]))
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
				return;
			}
			string codITmp = "";
			Referencias.Editar(codI,ref codITmp,(ddlLineas.SelectedValue.Split('-'))[1]);
			if(codITmp != txtRef.Text)
			{
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
				txtRef.Text = codITmp;
			}
			txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
		}
		//***********************************
		//Consulta de Pedidos
        protected void GenPed(Object Sender, EventArgs E)
        {
            if (!RevisarNit())
                return;
            Esconder();
            plhPed.Visible = true;
            string fls = "";
            string codI = "";
            if (!Referencias.Guardar(txtRef.Text, ref codI, (ddlLineas.SelectedValue.Split('-'))[1]))
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no es valido para la linea de bodega " + ddlLineas.SelectedItem.Text + ".\\nRevise Por Favor.!");
                return;
            }
            if (!Referencias.RevisionSustitucion(ref codI, (ddlLineas.SelectedValue.Split('-'))[0]))
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " no se encuentra registrado.\\nRevise Por Favor.!");
                return;
            }
            string codITmp = "";
            Referencias.Editar(codI, ref codITmp, (ddlLineas.SelectedValue.Split('-'))[1]);
            if (codITmp != txtRef.Text)
            {
                Utils.MostrarAlerta(Response, "El codigo " + txtRef.Text + " ha sido sustituido.El codigo actual es " + codITmp + "!");
                txtRef.Text = codITmp;
            }
            txtRefa.Text = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='" + codI + "'");
            int n;
            if (rblPedFec.SelectedIndex == 1)
                fls += " AND year(t2.mped_pedido)=" + ddlPedAno.SelectedValue;
            if (rblPedFec.SelectedIndex == 2)
                fls += " AND year(t2.mped_pedido)=" + ddlPedAno.SelectedValue + " AND  month(t2.mped_pedido)=" + ddlPedMes.SelectedValue;
            if (rblPedAlm.SelectedIndex == 1)
                fls += " AND t2.palm_almacen='" + ddlPedAlm.SelectedValue + "'";
            //PROVEEDOR
            string sqlDI = @"SELECT DISTINCT VN.nit || ' ' || VN.NOMBRE,t1.pped_codigo,t1.mped_numepedi,t2.mped_pedido,t1.dped_cantpedi,t1.dped_cantasig,t1.dped_cantfact,
                                (t1.dped_cantpedi-t1.dped_cantfact),t1.dped_valounit,t1.dped_porcdesc,t1.piva_porciva,t3.pdoc_codigo,t3.dite_numedocu,t3.dite_fechdocu 
                            FROM dbxschema.VMNIT As VN, dbxschema.mpedidoitem as t2 
                              LEFT JOIN dbxschema.ditems as t3 ON t2.pped_codigo=t3.dite_prefdocurefe AND t2.mped_numepedi=t3.dite_numedocurefe,dbxschema.DPEDIDOITEM as t1  
		                    WHERE t1.MPED_CLASREGI=t2.MPED_CLASEREGI AND t1.MNIT_NIT=t2.MNIT_NIT AND t1.PPED_CODIGO=t2.PPED_CODIGO AND t1.MPED_NUMEPEDI=t2.MPED_NUMEPEDI 
                                  AND t1.MPED_CLASREGI='P' AND t1.mite_codigo='" + codI + "' AND t1.mnit_nit = VN.MNIT_NIT;";
            sqlDI += fls + ";";
            ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sqlDI);
            dtInserts = new DataTable();
            dtInserts.Columns.Add("NIT", typeof(string));
            dtInserts.Columns.Add("Codigo", typeof(string));
            dtInserts.Columns.Add("Numero", typeof(string));
            dtInserts.Columns.Add("Fecha Pedido", typeof(string));
            dtInserts.Columns.Add("Cant. Ped.", typeof(string));
            dtInserts.Columns.Add("Cant. Asig.", typeof(string));
            dtInserts.Columns.Add("Cant. Fact.", typeof(string));
            dtInserts.Columns.Add("Cant. Back-Order", typeof(string));
            dtInserts.Columns.Add("Val. Unit.", typeof(string));
            dtInserts.Columns.Add("% Desc.", typeof(string));
            dtInserts.Columns.Add("% IVA", typeof(string));
            dtInserts.Columns.Add("Cod. Fact", typeof(string));
            dtInserts.Columns.Add("Num. Fact", typeof(string));
            dtInserts.Columns.Add("Fecha Factura", typeof(string));
            DataRow dr;
            dr = dtInserts.NewRow();
            dr[0] = "PROVEEDORES:";
            dtInserts.Rows.Add(dr);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    dr = dtInserts.NewRow();
                    dr[0] = "No hay Datos!";
                    dtInserts.Rows.Add(dr);
                }
                else
                {
                    for (n = 0; n < ds.Tables[0].Rows.Count; n++)
                    {
                        dr = dtInserts.NewRow();
                        dr[0] = ds.Tables[0].Rows[n][0];//NIT
                        dr[1] = ds.Tables[0].Rows[n][1];//Codigo
                        dr[2] = ds.Tables[0].Rows[n][2];//Numero
                        dr[3] = Convert.ToDateTime(ds.Tables[0].Rows[n][3]).ToString("yyyy-MM-dd");//Fecha Ped
                        dr[4] = Convert.ToDouble(ds.Tables[0].Rows[n][4]).ToString("N");//Cant. Ped
                        dr[5] = Convert.ToDouble(ds.Tables[0].Rows[n][5]).ToString("N");//Cant. Asig
                        dr[6] = Convert.ToDouble(ds.Tables[0].Rows[n][6]).ToString("N");//Cant. Fact
                        dr[7] = Convert.ToDouble(ds.Tables[0].Rows[n][7]).ToString("N");//Cant. Back Order
                        dr[8] = Convert.ToDouble(ds.Tables[0].Rows[n][8]).ToString("C");//Val. Unit
                        dr[9] = Convert.ToDouble(ds.Tables[0].Rows[n][9]).ToString("N");//% Desc
                        dr[10] = Convert.ToDouble(ds.Tables[0].Rows[n][10]).ToString("N");//%IVA
                        dr[11] = ds.Tables[0].Rows[n][11];//Cod. Fact
                        dr[12] = ds.Tables[0].Rows[n][12];//Num. Fact
                        try { dr[13] = Convert.ToDateTime(ds.Tables[0].Rows[n][13]).ToString("yyyy-MM-dd"); }//Fecha Fact
                        catch { dr[13] = ds.Tables[0].Rows[n][13].ToString(); }
                        dtInserts.Rows.Add(dr);
                    }
                }
            }
            //CLIENTE
            sqlDI = @"SELECT CASE WHEN PP.TPED_CODIGO = 'T' AND T1.PDOC_CODIGO IS NOT NULL THEN VN.nit || ' ' || VN.NOMBRE || ' ' || mt.pdoc_codigo || '-' || mt.mord_numeorde ELSE VN.nit || ' ' || VN.NOMBRE END, 
                        t1.pped_codigo,t1.mped_numepedi,t2.mped_pedido,t1.dped_cantpedi,t1.dped_cantasig,t1.dped_cantfact,
						CASE WHEN TPED_BACKORDER = 'S' THEN(t1.dped_cantpedi - t1.dped_cantasig - t1.dped_cantFACT) ELSE 0 END AS BACKORDER,
                        t1.dped_valounit,t1.dped_porcdesc,t1.piva_porciva,t1.pdoc_codigo,t1.mfac_numedocu,t2.MPED_PEDIDO
                    FROM dbxschema.VMNIT As VN, dbxschema.Ppedido as PP, dbxschema.Tpedido as TP,  dbxschema.mpedidoitem as t2, dbxschema.DPEDIDOITEM as t1
                     left join mordentransferencia mt on t1.pdoc_codigo = mt.pdoc_factura and t1.mfac_numedocu = mt.mfac_numero
                    WHERE t1.MPED_CLASREGI = t2.MPED_CLASEREGI AND t1.MNIT_NIT = t2.MNIT_NIT AND t1.PPED_CODIGO = t2.PPED_CODIGO AND t1.MPED_NUMEPEDI = t2.MPED_NUMEPEDI
                    AND PP.TPED_CODIGO = TP.TPED_CODIGO
                    AND t1.MPED_CLASREGI IN ('C', 'M') AND t1.mite_codigo = '" + codI + @"' AND t1.mnit_nit = VN.MNIT_NIT AND t1.pped_codigo = PP.pped_codigo
                    ORDER BY T2.MPED_PEDIDO; ";
            sqlDI += fls + ";";
            ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sqlDI);
            dr = dtInserts.NewRow();
            dr[0] = "CLIENTES:";
            dtInserts.Rows.Add(dr);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    dr = dtInserts.NewRow();
                    dr[0] = "No hay Datos!";
                    dtInserts.Rows.Add(dr);
                }
                else
                {
                    for (n = 0; n < ds.Tables[0].Rows.Count; n++)
                    {
                        dr = dtInserts.NewRow();
                        dr[0] = ds.Tables[0].Rows[n][0];//NIT
                        dr[1] = ds.Tables[0].Rows[n][1];//Codigo
                        dr[2] = ds.Tables[0].Rows[n][2];//Numero
                        dr[3] = Convert.ToDateTime(ds.Tables[0].Rows[n][3]).ToString("yyyy-MM-dd");//Fecha Ped
                        dr[4] = Convert.ToDouble(ds.Tables[0].Rows[n][4]).ToString("N");//Cant. Ped
                        dr[5] = Convert.ToDouble(ds.Tables[0].Rows[n][5]).ToString("N");//Cant. Asig
                        dr[6] = Convert.ToDouble(ds.Tables[0].Rows[n][6]).ToString("N");//Cant. Fact
                        dr[7] = Convert.ToDouble(ds.Tables[0].Rows[n][7]).ToString("N");//Cant. Back Order
                        dr[8] = Convert.ToDouble(ds.Tables[0].Rows[n][8]).ToString("C");//Val. Unit
                        dr[9] = Convert.ToDouble(ds.Tables[0].Rows[n][9]).ToString("N");//% Desc
                        dr[10] = Convert.ToDouble(ds.Tables[0].Rows[n][10]).ToString("N");//%IVA
                        dr[11] = ds.Tables[0].Rows[n][11];//Cod. Fact
                        dr[12] = ds.Tables[0].Rows[n][12];//Num. Fact
                        try { dr[13] = Convert.ToDateTime(ds.Tables[0].Rows[n][13]).ToString("yyyy-MM-dd"); }//Fecha Fact
                        catch { dr[13] = ds.Tables[0].Rows[n][13].ToString(); }
                        dtInserts.Rows.Add(dr);
                    }
                }
                grPed.DataSource = dtInserts;
                grPed.DataBind();
                for (int k = 0; k < grPed.Items.Count; k++)
                    for (int l = 4; l <= 10; l++)
                        grPed.Items[k].Cells[l].HorizontalAlign = HorizontalAlign.Right;
                CrearReporte(grPed);
                toolsHolder.Visible = true;
                btnPed.BackColor = Color.LightGray;
            }
        }

	    
		protected void ChangeDate1(Object Sender, EventArgs E)
		{
			tbDate1.Text = calDate1.SelectedDate.GetDateTimeFormats()[6];
		}
		
		protected void ChangeDate2(Object Sender, EventArgs E)
		{
			tbDate2.Text = calDate2.SelectedDate.GetDateTimeFormats()[6];
		}
		
        //funciones para el datagrid view
        
        //protected void grKar_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    string[] strSortExpression = ViewState["SortExpression"].ToString().Split(' ');


        //    // If the sorting column is the same as the previous one,  
        //    // then change the sort order. 
        //    if (strSortExpression[0] == e.SortExpression)
        //    {
        //        if (strSortExpression[1] == "ASC")
        //        {
        //            ViewState["SortExpression"] = e.SortExpression + " " + "DESC";
        //        }
        //        else
        //        {
        //            ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
        //        }
        //    }
        //    // If sorting column is another column,   
        //    // then specify the sort order to "Ascending". 
        //    else
        //    {
        //        ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
        //    }


        //    // Rebind the GridView control to show sorted data. 
        //    DataBind();
        //}


		private void CrearReporte(DataGrid report)
		{
			string []pr=new string[2];
			tabPreHeader=new Table();
			tabFirmas=new Table();
			pr[0]=pr[1]="";
			Press frontEnd = new Press(new DataSet(), reportTitle);
			frontEnd.PreHeader(tabPreHeader, report.Width, pr);
			frontEnd.Firmas(tabFirmas,report.Width);
			StringBuilder SB=new StringBuilder();
			StringWriter SW=new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			tabPreHeader.RenderControl(htmlTW);
			report.RenderControl(htmlTW);
			tabFirmas.RenderControl(htmlTW);
			string strRep;
			strRep=SB.ToString();
			Session.Clear();
			Session["Rep"]=strRep;
			grAct=report;
		}

        [Ajax.AjaxMethod()]
        public string ConsultarSustitucion(string itmO, string linea)
		{
			string susItm="";
            string codI = "";
            Referencias.Guardar(itmO, ref codI, linea);
            susItm = DBFunctions.SingleData("SELECT DBXSCHEMA.EDITARREFERENCIAS(msus_codACTUAL, '" + linea + "') FROM MSUSTITUCION WHERE msus_codANTERIOR='" + codI + "' ");
			if(susItm.Length>0)
				return(susItm);
			else
				return(itmO);
		}

		[Ajax.AjaxMethod]
		public string TraerNombreReferencia(string item,string linea)
		{
			string nombre="";
			string codI="";
			Referencias.Guardar(item,ref codI,linea);
			nombre=DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
			return nombre;
		}

        [Ajax.AjaxMethod]
        public string TraerInformacionAplicacion(string item)
        {
            string aplicacion = "";
            try
            {
                aplicacion = DBFunctions.SingleData("SELECT mite_desccomp FROM MITEMS WHERE mite_codigo='" + item + "'");
            }
            catch
            {
                aplicacion = "";
            }

            return aplicacion;
        }

		[Ajax.AjaxMethod]
		public string TraerInformacionCosto(string item,string linea, string permiso)
		{
            
            string costo="";
			string codI="";
           
            Referencias.Guardar(item,ref codI,linea);

			double valorCosto=0;

			try
			{
                if (permiso !="G" && permiso =="C")
               { 
				valorCosto = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM dbxschema.msaldoitem AS MSI, dbxschema.cinventario AS CI WHERE CI.pano_ano = MSI.pano_ano AND MSI.mite_codigo='"+codI+"'"));
				costo=valorCosto.ToString("C");
               }
                else
                    costo = "0.00";

            }
			catch
			{
				costo="No registra";
			}

			return costo;
		}

		[Ajax.AjaxMethod]
		public string TraerInformacionValor(string item,string linea)
		{
			string valor="";
			string codI="";
            string permiso = "";
            //permiso = ViewState["Permiso"];

            Referencias.Guardar(item,ref codI,linea);

			DataSet ds=new DataSet();

			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT ppre_codigo,ppre_nombre FROM pprecioitem");
			
			valor += "";
			
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				valor += "["+ds.Tables[0].Rows[i][1].ToString()+" : ";

				try
				{
					valor += Convert.ToDouble(DBFunctions.SingleData("SELECT mpre_precio FROM mprecioitem WHERE mite_codigo='"+codI+"' AND ppre_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"'")).ToString("C")+"] ";
				}
				catch
				{
					valor+="No registra] ";
				}
			}

			return valor;
		}

        [Ajax.AjaxMethod]
        public string TraerInformacionValorIva(string item, string linea)
        {
            string valor = "";
            string codI = "";

            Referencias.Guardar(item, ref codI, linea);

            DataSet ds = new DataSet();

            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT ppre_codigo,ppre_nombre FROM pprecioitem");

            valor += "";

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                valor += "[" + ds.Tables[0].Rows[i][1].ToString() + " : ";

                try
                {
                    valor += Convert.ToDouble(DBFunctions.SingleData("SELECT  ROUND (MP.mpre_precio * (1 + (MI.PIVA_PORCIVA * 0.01)), 0) FROM mprecioitem MP, MITEMS MI WHERE MP.mite_codigo='" + codI + "' AND MI.MITE_CODIGO = MP.MITE_CODIGO AND ppre_codigo='" + ds.Tables[0].Rows[i][0].ToString() + "'")).ToString("C")  + "]";
                }                                    
                catch
                {
                    valor += "No registra] ";
                }
            }

            return valor;
        }


        [Ajax.AjaxMethod]
		public string TraerInformacionCantidad(string item,string linea)
		{
			string cantidad="";
			string codI="";
			Referencias.Guardar(item,ref codI,linea);
                
            bool existe = DBFunctions.RecordExist("SELECT msal_cantactual FROM dbxschema.msaldoitem AS MSI, dbxschema.cinventario AS CI WHERE CI.pano_ano = MSI.pano_ano AND MSI.mite_codigo='"+codI+"'");
            
            if (existe)
            {
                DataSet ds = new DataSet();
                //DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT palm_DESCRIPCION, msal_cantactual 
	               //                                         FROM   dbxschema.msaldoitemalmacen AS MSI, dbxschema.cinventario AS CI, dbxschema.PALMACEN AS PA 
	               //                                         WHERE  MSI.PALM_ALMACEN = PA.PALM_ALMACEN AND CI.pano_ano = MSI.pano_ano AND MSI.mite_codigo='" + codI + "'");

                DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT palm_DESCRIPCION as ALMACEN , case when MSAL_CANTASIG > 0 then  ' Act ' || varchar_format (msal_cantactual,'9999') || ' Asg ' || varchar_format (msal_cantasig,'9999') || ' Disp ' || varchar_format (msal_cantactual - msal_cantasig,'9999') 
                                                else  ' Act ' || varchar_format (msal_cantactual,'9999') end AS CANTIDAD
	                                                        FROM   dbxschema.msaldoitemalmacen AS MSI, dbxschema.cinventario AS CI, dbxschema.PALMACEN AS PA 
	                                                        WHERE  MSI.PALM_ALMACEN = PA.PALM_ALMACEN AND CI.pano_ano = MSI.pano_ano AND MSI.mite_codigo='" + codI + @"'
                                                            GROUP BY palm_DESCRIPCION, msal_cantactuaL,MSAL_CANTASIG;");

                DataRow row;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    row = ds.Tables[0].Rows[i];
                    cantidad += "\n" + row[0].ToString() + ": " + row[1].ToString();
                }
            }
            else
                cantidad = "No registra";

			return cantidad;
		}

		[Ajax.AjaxMethod]
		public string TraerInformacionSustituciones(string item,string linea)
		{
			string sustituciones="";
			string codI="";

			Referencias.Guardar(item,ref codI,linea);

			if(DBFunctions.RecordExist("SELECT * FROM msustitucion WHERE msus_codactual='"+codI+"'"))
			{
				DataSet ds=new DataSet();

				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT dbxschema.editarreferencias(msus.msus_codorigen,plin.plin_tipo) msus_codorigen,dbxschema.editarreferencias(msus.msus_codsustit,plin.plin_tipo) msus_codsustit,dbxschema.editarreferencias(msus.msus_codactual,plin.plin_tipo) msus_codactual FROM dbxschema.msustitucion msus inner join dbxschema.plineaitem plin on msus.plin_codigo = plin.plin_codigo WHERE msus_codactual='"+codI+"'");
				
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					sustituciones+="Sustitución: "+(i+1).ToString()+" [Origen: "+ds.Tables[0].Rows[i][0].ToString()+"] [Sustituido: "+ds.Tables[0].Rows[i][1].ToString()+"] [Actual: "+ds.Tables[0].Rows[i][2].ToString()+"]  ";
			}
			else
				sustituciones="No registra";

			return sustituciones;
		}

		[Ajax.AjaxMethod]
		public string TraerInformacionUbicacion(string item,string linea)
		{
			string ubicacion="";
			string codI="";
			
			Referencias.Guardar(item,ref codI,linea);

			if(DBFunctions.RecordExist("SELECT * FROM mubicacionitem WHERE mite_codigo='"+codI+"'"))
			{
				
				DataSet ds=new DataSet();

                DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT PUBI.pubi_codigo, coalesce(PUBI.pubi_nombre,''), PUBI.PUBI_CODPAD, PUBI.palm_almacen 
                                                          FROM  DBXSCHEMA.mubicacionitem MUBI, DBXSCHEMA.pubicacionitem PUBI 
                                                          WHERE PUBI.pubi_codigo=MUBI.pubi_codigo AND MUBI.mite_codigo='" + codI+"'");
				
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					ubicacion+="[Almacén : "+ds.Tables[0].Rows[i][3].ToString()+"] [ "+ds.Tables[0].Rows[i][1].ToString()+"]  ";
				}
			}
			else
				ubicacion="No registra ";

			return ubicacion;
		}

		[Ajax.AjaxMethod]
		public string TraerInformacionSerial(string item,string linea)
		{
			string serial="";
			string codI="";
			Referencias.Guardar(item,ref codI,linea);
			serial=DBFunctions.SingleData("SELECT mite_refefabr FROM mitems WHERE mite_codigo='"+codI+"'");
			if(serial==string.Empty)
				serial="No registra";
			return serial;
		}
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			if(btnDatBas.BackColor==Color.Gray)GenDatBas(Sender,E);
			if(btnCants.BackColor==Color.Gray)GenCants(Sender,E);
			if(btnResMes.BackColor==Color.Gray)GenResMes(Sender,E);
			if(btnResAno.BackColor==Color.Gray)GenResAno(Sender,E);
			if(btnKar.BackColor==Color.Gray)GenKar(Sender,E);
			if(btnEst.BackColor==Color.Gray)GenEst(Sender,E);
			if(btnPed.BackColor==Color.Gray)GenPed(Sender,E);
            Utils.MostrarAlerta(Response, "" + Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, grAct) + "");
			
		}

		private void InitializeComponent()
		{

		}
		#endregion

		protected void btnPed_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
