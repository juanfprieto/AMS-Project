namespace AMS.Contabilidad
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Web;
	using System.Web.Mail;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using Tools;
	using System.Collections;

	/// <summary>
	///		
	/// </summary>
	public partial class ProceHecEco : System.Web.UI.UserControl
	{
		#region Controles, campos
		protected System.Web.UI.WebControls.DropDownList Perfil;
		protected string SqlLog,sql,Sql;
		protected string[] tem;
		protected DataSet DsCab = new DataSet();
		protected DataSet DsDet = new DataSet();
		protected DataSet DsDet1 = new DataSet();
		protected DataSet DsDet2 = new DataSet();
		protected DataSet DsDet3 = new DataSet();
		protected DataSet DsDocsCab = new DataSet();
		protected DataSet DsDocsDet = new DataSet();
		protected DataSet DsCentro = new DataSet();
		protected DataSet DsResumen = new DataSet();
		protected DataTable DataTable3=new DataTable();
		protected ArrayList resumenCabezera=new ArrayList();
		protected ArrayList resumenDetalle=new ArrayList();
		protected ArrayList resumen=new ArrayList();
		protected int totalr = 0;
		protected Seleccionar Seleccion = new Seleccionar();
		protected Seleccionar Seleccion1 = new Seleccionar();
		protected DataTable dtMovs,dtCabeceras,dtDetalles;
		protected double valorDebito=0,valorCredito=0;
		private InterfaceContable miInterface=new InterfaceContable();
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles, campos
		/// <summary>
		/// Page_Load : Evento que se ejecuta al cargar la página
		/// </summary>
		/// <param name="sender">object que hizo el envio al servidor</param>
		/// <param name="e">EventArgs que permite manipular los argumentos del evento</param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Seleccion.Visible = true;
			Seleccion1.Visible = true;

			Seleccion.ECargarG += new Seleccionar.DCargarG(Seleccion_ECargarG);
			Seleccion1.ECargarG += new Tools.Seleccionar.DCargarG(Seleccion1_ECargarG);
			Seleccion.AsignarEstados(true,true,true,true);
			Seleccion1.AsignarEstados(true,true,true,true);
            Seleccion.AsignarTextoBoton("Seleccionar");
            Seleccion1.AsignarTextoBoton("Imputar");
            Seleccion.AsignarTextoLabels("Procesos Disponibles","Procesos Seleccionados");
			Seleccion1.AsignarTextoLabels("Hechos Económicos que Aplica","Hechos Económicos Seleccionados");

            if (!IsPostBack)
			{
				if(Request.QueryString["ex"]!=null)
                Utils.MostrarAlerta(Response, "Proceso Finalizado Satisfactoriamente");
				Session.Clear();
				BindGrid();
				Cargar();
			}
			else
			{
				if(Session["dtCabeceras"]!=null)
					dtCabeceras=(DataTable)Session["dtCabeceras"];
				if(Session["dtDetalles"]!=null)
					dtDetalles=(DataTable)Session["dtDetalles"];
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

		/// <summary>
		/// BindGrid : función que carga el ListBox origen con los procesos
		/// disponibles para contabilizar
		/// </summary>
		private void BindGrid()
		{
			DataSet ds = new DataSet();
		
			string Sql = "SELECT tpro_proceso ,tpro_nombre FROM tPROCESO order by tpro_nombre;";
			try
			{
				DBFunctions.Request(ds,IncludeSchema.NO,Sql);
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					Seleccion.CargarOrigen(i,ds.Tables[0].Rows[i][1].ToString(),ds.Tables[0].Rows[i][0].ToString(),"Rojo");
				}
			}
			catch (Exception Ex)
			{
				lbInfo.Text = "Error : " + Ex.Message;
			}
			ds.Dispose();
		}

		/// <summary>
		/// Cargar : función que llena los dropdownlist al comienzo de la ejecución
		/// </summary>
		private void Cargar()
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(año,"SELECT pano_ano, pano_detalle FROM pano where pano_ano >= (select pano_ano from ccontabilidad) order by pano_ano");
            bind.PutDatasIntoDropDownList(Mes,"SELECT pmes_mes, pmes_nombre FROM pmes where pmes_mes > 0 and pmes_mes < 13 order by pmes_mes");
            bind.PutDatasIntoDropDownList(sede,"SELECT palm_almacen, palm_descripcion FROM palmacen order by palm_descripcion");
            bind.PutDatasIntoDropDownList(DiaInicial,"SELECT tdia_dia,tdia_nombre FROM tdia order by tdia_dia");
            bind.PutDatasIntoDropDownList(DiaFinal,"SELECT tdia_dia,tdia_nombre FROM tdia order by tdia_dia");
			año.SelectedIndex=año.Items.IndexOf(año.Items.FindByValue(DBFunctions.SingleData("SELECT pano_ano FROM ccontabilidad;").Trim()));
			Mes.SelectedIndex=Mes.Items.IndexOf(Mes.Items.FindByValue(DBFunctions.SingleData("SELECT pmes_mes FROM ccontabilidad;").Trim()));
			Todas.Checked=true;
		}

		private void Entidad_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Seleccion.LimpiarTodo();
			BindGrid();
		}

		/// <summary>
		/// Seleccion_ECargarG : función delegada del evento DCargarG
		/// </summary>
		/// <param name="Inicio">ListBox origen</param>
		/// <param name="Final">ListBox destino</param>
		private void Seleccion_ECargarG(ListBox Inicio, ListBox Final)
		{
			Seleccion1.LimpiarTodo();


			DataSet ds1 = new DataSet();
			string cuales = "";

			for(int x=0;x<Final.Items.Count;x++)
			{
				cuales += "'" + Final.Items[x].Value.ToString().Trim() + "',";
			}		
			if(cuales.Length==0)
            {
                Utils.MostrarAlerta(Response, "No se seleccionaron procesos, accion cancelada");
				return;
			}

			Sql = "Select a.pdoc_codigo as Codigo,a.pdoc_codigo CONCAT ' - ' CONCAT b.pdoc_descripcion as Nombre from ppuchechoscabecera a, pdocumento b where a.pdoc_codigo = b.pdoc_codigo and a.tpro_proceso in (" + cuales.Substring(0,cuales.Length -1) + ") order by Nombre;";
			try
			{
				DBFunctions.Request(ds1,IncludeSchema.NO,Sql);
				for(int i=0;i<ds1.Tables[0].Rows.Count;i++)
				{
					Seleccion1.CargarOrigen(i,ds1.Tables[0].Rows[i][1].ToString(),ds1.Tables[0].Rows[i][0].ToString(),"Rojo");
				}
			}
			catch (Exception Ex)
			{
				lbInfo.Text = "Error : " + Ex.Message;
			}
			ds1.Dispose();

		}

		/// <summary>
		/// Seleccion1_ECargarG : función delegada del evento DCargarG
		/// </summary>
		/// <param name="Inicio">ListBox origen</param>
		/// <param name="Final">ListBox destino</param>
		private void Seleccion1_ECargarG(ListBox Inicio, ListBox Final)
		{
			//Cambio hecho porque no se validan las fechas de ingreso, y cuando estan fuera de rango generan error
			bool fechaValida = true;
			try{DateTime fecha1 = Convert.ToDateTime(this.año.SelectedValue+"-"+this.Mes.SelectedValue+"-"+this.DiaInicial.SelectedValue);}catch{fechaValida=false;}
			try{DateTime fecha1 = Convert.ToDateTime(this.año.SelectedValue+"-"+this.Mes.SelectedValue+"-"+this.DiaFinal.SelectedValue);}catch{fechaValida=false;}
			if(!fechaValida)
			{
                Utils.MostrarAlerta(Response, "El rango de fechas ingresado no es valido. Revise por favor!");
				return;
			}
			int ini = Int32.Parse(DiaInicial.SelectedValue.ToString());
			int fin = Int32.Parse(DiaFinal.SelectedValue.ToString());
            DateTime fechaCont=new DateTime(Convert.ToInt32(DBFunctions.SingleData("SELECT PANO_ANO FROM CCONTABILIDAD")),Convert.ToInt32(DBFunctions.SingleData("SELECT PMES_MES FROM CCONTABILIDAD")),1);
            DateTime fechaContI=new DateTime(Convert.ToInt32(año.SelectedValue),Convert.ToInt32(Mes.SelectedValue),1);
            string prefE = "", errC = "", numE = "", errS="", errE="";
			if (ini > fin )
			{
				lbInfo.Text = "El dia Inicial no puede ser mayor al dia Final";
                return;
			}
            if (ConfigurationManager.AppSettings["AMSDebug"]==null && DateTime.Compare(fechaContI, fechaCont) == -1)
            {
                if (!DBFunctions.RecordExist("SELECT TTIPE_CODIGO FROM SUSUARIO WHERE TTIPE_CODIGO = 'AS' AND SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name + "' "))
          //          Utils.MostrarAlerta(Response, "Atención, este mes ya está cerrado, su perfil SI permite realizar este proceso");
          //      else
                {
                    lbInfo.Text = errE = "El usuario " + HttpContext.Current.User.Identity.Name + " ha utilizado una fecha no válida.";
                    goto Errores;
                }
            }
        //    else
            {
                //
                if (Final.Items.Count > 0)
                {
                    ArrayList prefijos = new ArrayList();
                    for (int i = 0; i < Final.Items.Count; i++)
                        prefijos.Add(Final.Items[i].Value);
                    miInterface = new InterfaceContable();
                    miInterface.Almacen = this.sede.SelectedValue;
                    if (Todas.Checked)
                        miInterface.TodosAlmacenes = true;
                    else
                        miInterface.TodosAlmacenes = false;
                    miInterface.Anio        = Convert.ToInt32(this.año.SelectedValue);
                    miInterface.DiaInicial  = Convert.ToInt32(this.DiaInicial.SelectedValue);
                    miInterface.DiaFinal    = Convert.ToInt32(this.DiaFinal.SelectedValue);
                    miInterface.Mes         = Convert.ToInt32(this.Mes.SelectedValue);
                    miInterface.Usuario     = HttpContext.Current.User.Identity.Name.ToLower();
                    miInterface.Procesado   = DateTime.Now.Date;
                    miInterface.Prefijos    = prefijos;
                    //Hago un for para recorrer los distintos prefijos escogidos
                    for (int i = 0; i < miInterface.Prefijos.Count; i++)
                    {
                        //Ahora por cada prefijo armo las distintas cabeceras de
                        //los comprobantes
                        prefE = prefijos[i].ToString();
                        try
                        {
                            miInterface.Armar_Cabecera(prefijos[i].ToString());
                        }
                        catch (Exception ex)
                        {
                            errC = lbInfoCab.Text = ex.Source + " " + ex.StackTrace + " " + ex.Message;
                            goto Errores;
                        }
                        //Si se produjo algún error al crear la cabecera lo muestro
                        //en el label y rompo ese ciclo para no seguir generando
                        //cabeceras
                        if (miInterface.MensajesCabecera != string.Empty)
                        {
                            errC = lbInfoCab.Text = miInterface.MensajesCabecera;
                            goto Errores;
                        }
                        //Si no se produjo error al generar cabeceras, empiezo a recorrer dichas cabeceras
                        //para sacar los distintos detalles que existan para cada cabecera
						int numeroInicial = 999999999;
						int numeroFinal = 0;
                        string tipoProceso = "";
                        for (int j = 0; j < miInterface.TablaCabecera.Rows.Count; j++)
                        {
                            if (miInterface.TablaCabecera.Rows[j][0].ToString() == prefE)
                            {
				                if (Convert.ToInt32(miInterface.TablaCabecera.Rows[j][1]) > numeroFinal)
					                numeroFinal = Convert.ToInt32(miInterface.TablaCabecera.Rows[j][1]);
				                if (Convert.ToInt32(miInterface.TablaCabecera.Rows[j][1]) < numeroInicial)
					                numeroInicial = Convert.ToInt32(miInterface.TablaCabecera.Rows[j][1]);
                                tipoProceso = miInterface.TablaCabecera.Rows[j][9].ToString();
                                numE = miInterface.TablaCabecera.Rows[j][1].ToString();
                            }
                        }
                            // un solo ciclo por todo el prefijo
                        if (numeroInicial != 999999999 && numeroFinal != 0)
                        {
                            try
                            {
                        //          miInterface.Armar_Detalles(miInterface.TablaCabecera.Rows[j][0].ToString(), Convert.ToInt32(miInterface.TablaCabecera.Rows[j][1]), miInterface.TablaCabecera.Rows[j][9].ToString());
                                miInterface.Armar_Detalles(prefijos[i].ToString(), numeroInicial, numeroFinal, tipoProceso);
                            }
                            catch (Exception ex)
                            {
                               errS = this.lbInfo.Text = ex.Source + " " + ex.StackTrace + " " + ex.Message;
                               goto Errores;
                            }
                        }    
                         
                        if (miInterface.erroresSql.Length > 0)
                        {
                            errS = this.lbInfo.Text = miInterface.erroresSql;
                            goto Errores;
                        }
                    //    }   aqui estaba el fin del ciclo for
                        // UN SOLO CICLO POR TODO EL PREFIJO
                    
                     
                    string pref, nume;
                    //Anulaciones Caja
                    if (miInterface.AnulacionesCaja)
                    {
                        int desdeC = miInterface.TablaCabecera.Rows.Count;
                        Hashtable prefijosA = miInterface.Armar_Cabecera_AnulacionesCaja(prefijos[i].ToString());
                        int hastaC = miInterface.TablaCabecera.Rows.Count;
                        for (int j = desdeC; j < hastaC; j++)
                        {
                            pref = miInterface.TablaCabecera.Rows[j][0].ToString();
                            nume = miInterface.TablaCabecera.Rows[j][1].ToString();
                            string[] tem = prefijosA[pref + "," + nume].ToString().Split(',');
                            miInterface.Armar_Detalles_Anulaciones(tem[0], Convert.ToInt32(tem[1]), pref, Convert.ToInt32(nume),"CJ");
                        }
                    }
                    //Anulaciones Tesoreria
                    if (miInterface.AnulacionesTesoreria)
                    {
                        int desdeC = miInterface.TablaCabecera.Rows.Count;
                        Hashtable prefijosA = miInterface.Armar_Cabecera_AnulacionesTesoreria(prefijos[i].ToString());
                        int hastaC = miInterface.TablaCabecera.Rows.Count;
                        for (int j = desdeC; j < hastaC; j++)
                        {
                            pref = miInterface.TablaCabecera.Rows[j][0].ToString();
                            nume = miInterface.TablaCabecera.Rows[j][1].ToString();
                            string[] tem = prefijosA[pref + "," + nume].ToString().Split(',');
                            miInterface.Armar_Detalles_Anulaciones(tem[0], Convert.ToInt32(tem[1]), pref, Convert.ToInt32(nume),"TS");
                        }
                    }

                    }   //  aqui termina el ciclo del for ?

                    dtCabeceras             = miInterface.TablaCabecera;
                    dtDetalles              = miInterface.TablaDetalles;
                    Session["dtCabeceras"]  = dtCabeceras;
                    Session["dtDetalles"]   = dtDetalles;
                    ConstruirGrilla();
                    Session["Rep"] = RenderHtml();
                    RenderHtml();
                    DeshabilitarControles();
                }
                else
                    Utils.MostrarAlerta(Response, "No se seleccionaron hechos que procesar, no se inicio el proceso");
            }            
            return;

         Errores:
            //MailMessage MyMail = new MailMessage();
            //string msgMail = "";
            //MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
            //MyMail.To = ConfigurationManager.AppSettings["EmailContabilidad"];
            //MyMail.Subject = "AMS - Error Contabilizacion Hechos Economicos (" + this.año.SelectedValue + ", " + this.Mes.SelectedValue + ", " + this.DiaInicial.SelectedValue + " a " + DiaFinal.SelectedValue + ")";
            //msgMail += "Se ha presentado un error en la contabilización de los hechos economicos:<br>";
            //msgMail += "Fecha: " + this.año.SelectedValue + ", " + this.Mes.SelectedValue + ", " + this.DiaInicial.SelectedValue + " a " + DiaFinal.SelectedValue + "<br>";
            //if (errE.Length > 0) msgMail += errE + "<br>";
            //if (prefE.Length > 0) msgMail += "Prefijo: " + prefE + "<br>";
            //if (numE.Length > 0) msgMail += "Número: " + numE + "<br>";
            //if (errC.Length > 0) msgMail += miInterface.MensajesCabecera + "<br>";
            //if (errS.Length > 0) msgMail += "Error al generar el detalle: " + errS + "<br>";
            //MyMail.Body = (msgMail);
            //MyMail.BodyFormat = MailFormat.Html;
            try
            {
                //SmtpMail.Send(MyMail);
            }
            catch (Exception e)
            {
                //lbInfo.Text = "No se pudo enviar el correo informando el siguiente error:<br>" + msgMail;
            }
		}

        //agreagdo para llevar la contabiliada inmediatamente  (Online)

        public void contabilizarOnline(String prefijo, int numero, DateTime fecha, String Almacen)
        {
            if (ConfigurationManager.AppSettings["ContabilidadAutomatica"] == null)
                return;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ContabilidadAutomatica"]) == false)
                return;
            //    debe contabilizar SIEMPRE TODOS los documentos que envian a este proceso 
            if (DBFunctions.RecordExist("select mnit_nit from CEMPRESA WHERE MNIT_NIT = '891409088' ")) // AYCO SI PREGUNTA POR LA PARAMETRIZACION DEL PREFIJO PARA CONTABILIZAR
            {  
                  if (!DBFunctions.RecordExist("select pdoc_codigo from PPUCHECHOSCABECERA where pdoc_codigo = '" + prefijo + "'"))
                      return;               
            }
            string errC, tipoProceso = ""; 
            ArrayList prefijos = new ArrayList();
            miInterface     = new InterfaceContable();
    //        miInterface.Almacen = sede;
            miInterface.Anio = (int)fecha.Year;
            miInterface.DiaInicial = (int)fecha.Day;
            miInterface.DiaFinal = (int)fecha.Day;
            miInterface.Mes = (int)fecha.Month;  //DateTime.Now.Month;
            //miInterface.Mes = fecha.Month;
            miInterface.Usuario = HttpContext.Current.User.Identity.Name.ToLower();
            miInterface.Procesado = DateTime.Now.Date;
            prefijos.Add(prefijo);
            miInterface.Prefijos = prefijos;

            try
            {
                tipoProceso = DBFunctions.SingleData("SELECT COALESCE(TPRO_PROCESO,'') FROM PPUCHECHOSCABECERA where PDOC_CODIGO='" + prefijo + "';");
            }
            catch
            {
                tipoProceso = ""; // SE toma este proceso por defecto
                Utils.MostrarAlerta(Response, "El tipo de proceso aun no se ha creado.");
            }
           
            try
            {
                miInterface.Armar_Cabecera(prefijo, numero);
            }
            catch (Exception ex)
            {
                errC = lbInfoCab.Text = ex.Source + " " + ex.StackTrace + " " + ex.Message;
                //goto Errores;
            }

            try
            {
                //
                miInterface.Armar_Detalles(prefijo, numero, numero, tipoProceso);
            }
            catch (Exception ex)
            {
                errC = lbInfoCab.Text = ex.Source + " " + ex.StackTrace + " " + ex.Message;
                //goto Errores;
            }
            try
            {
                if (tipoProceso == "CJ")
                {
                    string temP = DBFunctions.SingleData("SELECT PDOC_CODIGO             FROM MANULACIONCAJA WHERE PDOC_CODIANUL = '" + prefijo + "' AND MANU_NUMEANUL = " + numero + " ");
                    string temN = DBFunctions.SingleData("SELECT coalesce(MCAJ_NUMERO,0) FROM MANULACIONCAJA WHERE PDOC_CODIANUL = '" + prefijo + "' AND MANU_NUMEANUL = " + numero + " ");
                    miInterface.Armar_Detalles_Anulaciones(temP, Convert.ToInt32(temN), prefijo, Convert.ToInt32(numero), "CJ");
                    miInterface.Armar_Cabecera(prefijo, numero);
                }
            }
            catch 
            {
               
            }

            try
            {
                if (tipoProceso == "TS")
                {
                    string temP = DBFunctions.SingleData("SELECT MTES_CODIGO               FROM DTESORERIAANULACION WHERE PDOC_CODIGO = '" + prefijo + "' AND MTES_NUMERO = " + numero + " ");
                    string temN = DBFunctions.SingleData("SELECT coalesce(MTES_NUMEANUL,0) FROM DTESORERIAANULACION WHERE PDOC_CODIGO = '" + prefijo + "' AND MTES_NUMERO = " + numero + " ");
                    miInterface.Armar_Detalles_Anulaciones(temP, Convert.ToInt32(temN), prefijo, Convert.ToInt32(numero), "TS");
                    miInterface.Armar_Cabecera(prefijo, numero);
                }
            }
            catch 
            {
               
            }

            if (!miInterface.GuardarInterface())
            {
                if (lbInfo != null)
                    lbInfo.Text = miInterface.Mensajes;
                Tools.Mail envMail = new Mail();
  //              envMail.EnviarMail()
            }
        }


        public void anularComprobante(String prefijo, int numero)
        {
            string anoFact = DBFunctions.SingleData("SELECT PANO_ANO FROM MCOMPROBANTE WHERE pdoc_codigo = '" + prefijo + "' and mcom_numedocu = " + numero + " ").ToString();
            string mesFact = DBFunctions.SingleData("SELECT PMES_MES FROM MCOMPROBANTE WHERE pdoc_codigo = '" + prefijo + "' and mcom_numedocu = " + numero + " ").ToString();
            ArrayList eliminar = new ArrayList();
            eliminar.Add ( @"update dcuenta set dcue_valodebi = 0, dcue_valocred = 0, dcue_niifdebi = 0, dcue_niifcred = 0, dcue_DETALLE = 'Elminada ' || dcue_DETALLE
                                where pdoc_codigo = '" + prefijo + "' and mcom_numedocu = " + numero + " ");
            eliminar.Add(" CALL DBXSCHEMA.ACTUALIZACION_CONTABILIDAD_MES("+anoFact+","+mesFact+") ");
            if (!DBFunctions.Transaction(eliminar))
                Utils.MostrarAlerta(Response, "El Comprobante se ha borrado correctamente de la Contabilidad ");
            else
                Utils.MostrarAlerta(Response, "ERROR, El Comprobante NO se ha borrado la Contabilidad, ELIMINELO  ");

        }

/*
        public void contabilizarOnlineSeguros(String prefijo, int numeroAseguradora, int numeroAsegurado,  DateTime fecha, String Almacen)
        {
            if (ConfigurationManager.AppSettings["ContabilidadAutomatica"] == null)
                return;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ContabilidadAutomatica"]) == false)
                return;

            string errC, tipoProceso = ""; ;
            ArrayList prefijos = new ArrayList();
            miInterface = new InterfaceContable();
            //        miInterface.Almacen = sede;
            miInterface.Anio = (int)fecha.Year;
            miInterface.DiaInicial = (int)fecha.Day;
            miInterface.DiaFinal = (int)fecha.Day;
            miInterface.Mes = fecha.Month;
            miInterface.Usuario = HttpContext.Current.User.Identity.Name.ToLower();
            miInterface.Procesado = DateTime.Now.Date;
            prefijos.Add(prefijo);
            miInterface.Prefijos = prefijos;

            try
            {
                tipoProceso = DBFunctions.SingleData("SELECT TPRO_PROCESO FROM PPUCHECHOSCABECERA where PDOC_CODIGO='" + prefijo + "';");
            }
            catch
            {
                Utils.MostrarAlerta(Response, "El tipo de proceso aun no se ha creado.");
            }

            try
            {
                miInterface.Armar_Cabecera(prefijo, numeroAseguradora);
                miInterface.Armar_Cabecera(prefijo, numeroAsegurado);
            }
            catch (Exception ex)
            {
                errC = lbInfoCab.Text = ex.Source + " " + ex.StackTrace + " " + ex.Message;
                //goto Errores;
            }

            try
            {
                //
                miInterface.Armar_Detalles(prefijo, numeroAseguradora, numeroAsegurado, tipoProceso);
            }
            catch (Exception ex)
            {
                errC = lbInfoCab.Text = ex.Source + " " + ex.StackTrace + " " + ex.Message;
                //goto Errores;
            }

            if (miInterface.GuardarInterface())
            {

            }

            else
            {
                if (lbInfo != null)
                    lbInfo.Text = miInterface.Mensajes;
            }
        }
*/

   		protected void CmdContabilizar_Click(object sender, System.EventArgs e)
		{
			miInterface              = new InterfaceContable();
			miInterface.Anio         = Convert.ToInt32(this.año.SelectedValue);
			miInterface.DiaInicial   = Convert.ToInt32(this.DiaInicial.SelectedValue);
			miInterface.DiaFinal     = Convert.ToInt32(this.DiaFinal.SelectedValue);
			miInterface.Mes          = Convert.ToInt32(this.Mes.SelectedValue);
			miInterface.Usuario      = HttpContext.Current.User.Identity.Name.ToLower();
			miInterface.Procesado    = DateTime.Now.Date;
			miInterface.TablaCabecera= dtCabeceras;
			miInterface.TablaDetalles= dtDetalles;
			if(miInterface.GuardarInterface())
				Response.Redirect(indexPage+"?process=Contabilidad.ProceHecEco&ex=1");
			else
				lbInfo.Text=miInterface.Mensajes;
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs e)
		{
            string result = "";
            if (tbEmail.Text == "")
            {
                Utils.MostrarAlerta(Response, "Debe ingresar un correo. Revise por favor");
                return;
            }
            try
            {
                Tools.AMS_Tools_Email envio = new AMS_Tools_Email();
                //string mensajeExcel =
                //        @"<div style='position: absolute; background-color:#EEEFD9;width: 35%;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
                // 	    <img style='width: 20%; position: absolute; right: 2%;' src='" + urlImagen + @"' /><br><br>
                //      <b><font size='5'>Excel Generado:</font></b>
                //      <br>" + nombre.Split('_')[0] +
                //           @"<br><br>
                //      <b>Reciba un cordial saludo</b>, <br>
                //      Ha recibido un Excel usando el Sistema Ecas <br>
                //            Dicho Excel se encuentra disponible como archivo <br>
                //            adjunto en este correo.
                //      <br><br>
                //         <b>Gracias por su atención.</b>
                //      <br>
                //      <i>eCAS-AMS.</i>
                //     </div>
                //        <br><br>";
                //string empresa = DBFunctions.SingleDataGlobal("select gemp_descripcion from gempresa where gemp_nombre='" + GlobalData.getEMPRESA() + "';");
                DataSet ds = new DataSet();
                ds.Tables.Add((DataTable)Session["dtDetalles"]);
                ds.DataSetName = "TablaContabilidad";
                envio.Correo = tbEmail.Text;
                envio.DsExcel = ds;//new DataSet().Tables.Add((DataTable)Session["dtDetalles"]);
                envio.ImageButton1_Click(Sender, e);
                
                //result = Tools.AMS_Tools_Email.enviarMail(tbEmail.Text, "Ha recibido un Reporte Excel de " + empresa, mensajeExcel, TipoCorreo.HTML, bytes);
                if (result == "")
                {
                    Utils.MostrarAlerta(Response, "Email con Reporte ha sido enviado correctamente a: " + tbEmail.Text);
                }
            }
            catch (Exception z)
            {

            }
            /*MailMessage MyMail  = new MailMessage();
			MyMail.From         = ConfigurationManager.AppSettings["EmailFrom"];
			MyMail.To           = tbEmail.Text;
			MyMail.Subject      = "Proceso : Hechos Económicos";
			MyMail.Body         = (RenderHtml());
			MyMail.BodyFormat   = MailFormat.Html;
			try
			{
				SmtpMail.Send(MyMail);
			}
			catch(Exception e)
			{
				lbInfo.Text     = e.ToString();
			}*/
        }

		protected string RenderHtml()
		{
			StringBuilder SB    = new StringBuilder();
			StringWriter SW     = new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			phGrilla.RenderControl(htmlTW);
			return SB.ToString();
          //  return "";
		}
		
		protected void ConstruirGrilla()
		{
			bool deb=false,cre=false;
			DatasToControls.Aplicar_Formato_Grilla(dgMovs);
			Estructura_DtMovs();
			for(int i=0;i<dtCabeceras.Rows.Count;i++)
			{
				DataRow[] hijos=dtDetalles.Select("PREFIJO='"+dtCabeceras.Rows[i][0].ToString()+"' AND NUMERO='"+dtCabeceras.Rows[i][1].ToString()+"'");
				valorCredito=valorDebito=0;
				if(hijos.Length!=0)
				{
					ConstruirHeader(dtCabeceras.Rows[i][0].ToString(),dtCabeceras.Rows[i][1].ToString(),dtCabeceras.Rows[i][2].ToString(),dtCabeceras.Rows[i][3].ToString(),dtCabeceras.Rows[i][4].ToString(),dtCabeceras.Rows[i][5].ToString(),dtCabeceras.Rows[i][6].ToString(),dtCabeceras.Rows[i][7].ToString(),dtCabeceras.Rows[i][8].ToString());
					for(int j=0;j<hijos.Length;j++)
					{
						deb=cre=false;
						if(hijos[j][9].ToString()!=string.Empty) //Debito
						{
							try
							{
								valorDebito=valorDebito+Math.Round(Convert.ToDouble(hijos[j][9].ToString().Substring(1)),2);
								deb=true;
							}
							catch{valorDebito=valorDebito+0;}
						}
						if(hijos[j][10].ToString()!=string.Empty)//Credito
						{
							try
							{
								valorCredito=valorCredito+Math.Round(Convert.ToDouble(hijos[j][10].ToString().Substring(1)),2);
								cre=true;
							}
							catch{valorCredito=valorCredito+0;}
						}
						if(deb && cre)
						{
                            if (Convert.ToDouble(hijos[j][9].ToString().Substring(1)) != 0 || Convert.ToDouble(hijos[j][10].ToString().Substring(1)) != 0)
                            { // para no sobre cargart (temporal}... se usa la intruccion de abajo.
                            }
                                ConstruirItems(hijos[j][2].ToString(),hijos[j][3].ToString(),hijos[j][4].ToString(),hijos[j][5].ToString(),hijos[j][6].ToString(),hijos[j][7].ToString(),hijos[j][8].ToString(),hijos[j][9].ToString(),hijos[j][10].ToString(),hijos[j][11].ToString(), hijos[j][12].ToString(), hijos[j][13].ToString(), dtCabeceras.Rows[i][0].ToString(),dtCabeceras.Rows[i][1].ToString());
						}
					}
					ConstruirTotalComprobante();
				}
			}
            dgMovs.DataSource=dtMovs;
            dgMovs.DataBind();
			if(hdnCont.Value=="S")
				CmdContabilizar.Visible = lnkExportarExcel.Visible = true;
		}

		protected void ConstruirHeader(string prefijo,string numero,string anio,string mes,string fecha,string razon,string procesado,string usuario,string valor)
		{
			DataRow fila,filaTitulos;
			fila=dtMovs.NewRow();
			filaTitulos=dtMovs.NewRow();
			if(DBFunctions.RecordExist("SELECT * FROM DBXSCHEMA.MCOMPROBANTE WHERE PDOC_CODIGO='"+prefijo+"' AND MCOM_NUMEDOCU="+numero))
            {
				fila[0]="<p style=\"COLOR: green\">"+prefijo+"</p>";
				fila[1]="<p style=\"COLOR: green\">"+numero+"</p>";
            }
			else
            {
				fila[0]=prefijo;
				fila[1]=numero;
            }
			fila[2] = anio;
			fila[3] = mes;
			fila[4] = fecha;
			fila[5] = razon;
			fila[6] = procesado;
			fila[7] = usuario;
			fila[8] = valor;
            fila[9] = "";
            fila[10] = "";
            filaTitulos[0] = "<p style=\"COLOR: navy\">Cuenta</p>";
			filaTitulos[1] = "<p style=\"COLOR: navy\">Referencia Pref-Num</p>";
			filaTitulos[2] = "<p style=\"COLOR: navy\">Nit</p>";
			filaTitulos[3] = "<p style=\"COLOR: navy\">Almacén</p>";
			filaTitulos[4] = "<p style=\"COLOR: navy\">Centro Costo</p>";
			filaTitulos[5] = "<p style=\"COLOR: navy\">Razón</p>";
			filaTitulos[6] = "<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Débito</p>";
			filaTitulos[7] = "<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Crédito</p>";
			filaTitulos[8] = "<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Base</p>";
            filaTitulos[9] = "<p style=\"COLOR: navy; TEXT-ALIGN: right\">Vr Débito NIIF</p>";
            filaTitulos[10] = "<p style=\"COLOR: navy; TEXT-ALIGN: right\">Vr Crédito NIIF</p>";

            dtMovs.Rows.Add(fila);
			dtMovs.Rows.Add(filaTitulos);
		}

		protected void ConstruirItems(string cuenta,string prefijoRef,string numeroRef,string nit,string almacen,string centroCosto,string razon,string debito,string credito,string valorBase, string debitoNIIF, string creditoNIIF, string prefijoPadre, string numeroPadre)
		{
			if(cuenta == null || cuenta == String.Empty)
				hdSvrErrores.Value += "Por favor revise el comprobante de "+prefijoPadre+"-"+numeroPadre+" algun renglon no tiene cuenta\\n";
			if(almacen == null || almacen == String.Empty)
				hdSvrErrores.Value += "Por favor revise el comprobante de "+prefijoPadre+"-"+numeroPadre+" algun renglon no tiene almacen\\n";
			if(centroCosto == null || centroCosto == String.Empty)
				hdSvrErrores.Value += "Por favor revise el comprobante de "+prefijoPadre+"-"+numeroPadre+" algun renglon no tiene centro de costo\\n";
			DataRow fila;
			fila=dtMovs.NewRow();
			fila[0] = VerificarErrorConsulta(cuenta.Replace("'",""));
			fila[1] = VerificarErrorConsulta(prefijoRef)+"-"+VerificarErrorConsulta(numeroRef);
			fila[2] = VerificarErrorConsulta(nit);
			fila[3] = VerificarErrorConsulta(almacen.Replace("'",""));
			fila[4] = VerificarErrorConsulta(centroCosto.Replace("'",""));
			fila[5] = VerificarErrorConsulta(razon.Replace("'",""));
			fila[6] = "<p style=\"TEXT-ALIGN: right\">"+debito+"</p>";
			fila[7] = "<p style=\"TEXT-ALIGN: right\">"+credito+"</p>";
			fila[8] = "<p style=\"TEXT-ALIGN: right\">"+valorBase+"</p>";
            fila[9] = "<p style=\"TEXT-ALIGN: right\">" + debitoNIIF + "</p>";
            fila[10] = "<p style=\"TEXT-ALIGN: right\">" + creditoNIIF + "</p>";
            dtMovs.Rows.Add(fila);
		}

		protected void ConstruirTotalComprobante()
		{
			DataRow fila,filaIntermedio;
			fila=dtMovs.NewRow();
			filaIntermedio=dtMovs.NewRow();
			fila[0]="<p style=\"COLOR: navy\">Total Comprobante</p>";
			if(Math.Round(valorDebito,2) != Math.Round(valorCredito,2))
				fila[5]="<p style=\"COLOR: red\">Comprobante Descuadrado</p>";
			else
			{
				fila[5]="<p style=\"COLOR: green\">Sumas Iguales</p>";
				hdnCont.Value="S";
			}
			fila[6]="<p style=\"TEXT-ALIGN: right\">"+Math.Round(valorDebito,2).ToString("C")+"</p>";
			fila[7]="<p style=\"TEXT-ALIGN: right\">"+Math.Round(valorCredito,2).ToString("C")+"</p>";
			dtMovs.Rows.Add(fila);
			dtMovs.Rows.Add(filaIntermedio);
		}

		protected void Estructura_DtMovs()
		{
			dtMovs=new DataTable();
			dtMovs.Columns.Add("PREFIJO",typeof(string));//0
			dtMovs.Columns.Add("NUMERO",typeof(string));//1
			dtMovs.Columns.Add("AÑO",typeof(string));//2
			dtMovs.Columns.Add("MES",typeof(string));//3
			dtMovs.Columns.Add("FECHA DOC",typeof(string));//4
			dtMovs.Columns.Add("RAZON",typeof(string));//5
			dtMovs.Columns.Add("PROCESADO",typeof(string));//6
			dtMovs.Columns.Add("USUARIO",typeof(string));//7
			dtMovs.Columns.Add("VALOR",typeof(string));//8
            dtMovs.Columns.Add("D_NIIF", typeof(string));//9
            dtMovs.Columns.Add("C_NIIF", typeof(string));//10
        }

		protected void DeshabilitarControles()
		{
			año.Enabled=Mes.Enabled=DiaInicial.Enabled=DiaFinal.Enabled=sede.Enabled=Todas.Enabled=false;
			Seleccion.AsignarEstados(false,false,false,false);
			Seleccion1.AsignarEstados(false,false,false,false);
		}

		protected void HabilitarControles()
		{
			año.Enabled=Mes.Enabled=DiaInicial.Enabled=DiaFinal.Enabled=sede.Enabled=Todas.Enabled=true;
			Seleccion.AsignarEstados(true,true,true,true);
			Seleccion1.AsignarEstados(true,true,true,true);
		}

		private string VerificarErrorConsulta(string valor)
		{
			string nuevoValor="";
			if(valor.IndexOf("Error")!=-1)
				nuevoValor="<p style=\"COLOR: red\">"+valor+"</p>";
			else
				nuevoValor=valor;
			return nuevoValor;
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			HabilitarControles();
			Estructura_DtMovs();
			dgMovs.DataSource=dtMovs;
			dgMovs.DataBind();
			Session.Clear();
			lbInfoCab.Text=lbInfo.Text="";
		}

		protected void lnkExportarExcel_Click(object sender, System.EventArgs e)
		{
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename=interfase.xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			System.IO.StringWriter stringWrite = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
			dgMovs.RenderControl(htmlWrite);
			Response.Write(stringWrite.ToString());
			Response.End();
		}

	}
}
