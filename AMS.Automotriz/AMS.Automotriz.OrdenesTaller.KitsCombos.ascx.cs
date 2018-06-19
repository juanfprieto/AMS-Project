    // created on 27/09/2004 at 10:37
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
using System.Configuration;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using AMS.Tools;
using System.Globalization;

namespace AMS.Automotriz
{
	public partial class KitsCombos : System.Web.UI.UserControl
	{
		#region Atributos
        protected string Multitaller = "";
        protected Control principal;
        protected Control controlDatosVehiculo;
        protected Control controlDatosOrigen;
		protected DataSet kitsGrupoCatalogo;
		protected DataTable items,operaciones,kitsTabla;
		protected ArrayList escogidos, escogidos2;
		protected string almTrans;
		protected uint mesInventario;
		protected int anoInventario;
		protected string cita;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        String codigoTempOp="";
        String nombreTempOp="";        
        #endregion

        #region Eventos
        protected void Page_Load(Object sender, System.EventArgs e)
		{
			principal = (this.Parent).Parent;
			controlDatosVehiculo = ((PlaceHolder)principal.FindControl("datosVehiculo")).Controls[0];
			controlDatosOrigen = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];

            //Control almacenTransferencia = controlDatosOrigen.FindControl("almacenTransferencia");
            //if ((DropDownList)almacenTransferencia == null)
            //    almTrans = DBFunctions.SingleData("SELECT palm_almacen, palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and pcen_centinv is not null fetch first row only;");
            //else
            //    almTrans = ((DropDownList)almacenTransferencia).SelectedValue;
			
            mesInventario = Convert.ToUInt32(DBFunctions.SingleData("SELECT pmes_mes FROM cinventario"));
			anoInventario = Convert.ToInt32(DBFunctions.SingleData("SELECT pano_ano FROM cinventario"));
			hdcat.Value=((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue;
			cita=((Label)controlDatosOrigen.FindControl("lbEstCita")).Text;
            ViewState["listaPrecios"] = ((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue;

            if(Session["items"]!=null)
			{	
				try{items = (DataTable)Session["items"];}
				catch(Exception ex){((Label)principal.FindControl("lb")).Text += "Error 3 :"+ex.ToString();}
			}
			else
				Preparar_Tabla_Items();

            if (Session["operaciones"] != null)
            {
                try
                { 
                    operaciones = (DataTable)Session["operaciones"];
                    if (operaciones.Rows.Count != kitsOperaciones.Items.Count)
                        BindDgOperaciones();
                }
                catch (Exception ex) { ((Label)principal.FindControl("lb")).Text += "Error 4 :" + ex.ToString(); }
            }
            else
                Preparar_Tabla_Operaciones();

            if (!IsPostBack)
            {
                escogidos = new ArrayList();
                items = new DataTable();
                kitsItems.DataSource = items;
                kitsItems.DataBind();
                kitsOperaciones.DataSource = operaciones;
                kitsOperaciones.DataBind();
            }
            else if (ViewState["listaPrecios"].ToString() != "0" && ViewState["cargado"] == null)
            {
                items = new DataTable();
                kitsItems.DataSource = items;
                kitsItems.DataBind();
                ViewState["cargado"] = 1;
            }

			if(Session["escogidos"]==null)
				escogidos = new ArrayList();
			else
			{
				try{escogidos = (ArrayList)Session["escogidos"];}
				catch(Exception ex){((Label)principal.FindControl("lb")).Text += "Error 1 : "+ex.ToString();}
			}
			if(Session["kitsTabla"]!=null)
				kitsTabla = (DataTable)Session["kitsTabla"];
			else
				LlenarKits(DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='"+((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue+"'"));
			

			if(Session["escogidos2"]==null)
				escogidos2=new ArrayList();
			else
			{
				try{escogidos2=(ArrayList)Session["escogidos2"];}
				catch(Exception ex){((Label)principal.FindControl("lb")).Text += "Error 5 : "+ex.ToString();}
			}
			Verificar_Grilla_Kits();
            //comento este parrafo porque si tiene campana est obligando siempre a esa operacion y no permite hacer nada se enloopa
            if(Session["garantia"] != null)
            {
                try
                {
                    Control controlOtrosDatos = ((PlaceHolder)principal.FindControl("otrosDatos")).Controls[0];
                    DropDownList ddlG = ((DropDownList)controlOtrosDatos.FindControl("ddltp"));
                    codigoTempOp = ddlG.SelectedValue.ToString();
                    nombreTempOp = ddlG.SelectedItem.Text;
                    kitsOperaciones.DataSource = operaciones;
                    DataRow laOperacion = operaciones.NewRow();
                    //laOperacion[0] = ;
                    //laOperacion[1] = ;
                    //laOperacion[2] = ;
                    //laOperacion[3] = ;
                    //laOperacion[4] = ;
                    //laOperacion[5] = ;
                    //laOperacion[6] = ;
                    //laOperacion[7] = ;
                    //laOperacion[8] = ;
                    //laOperacion[9] = ;
                    //laOperacion[10] = ;
                    //laOperacion[11] = ;
                    //laOperacion[12] = ;
                    //laOperacion[13] = ;
                    //((DropDownList)kitsOperaciones.Items[0].Cells[10].Controls[1]).SelectedValue = "G";

                    //confirmar que se agregará una garantía
                    Session["garantia"] = "2";
                    kitsOperaciones.DataBind();
                    
                }
                catch (Exception ex) { }
            }
            

        }

        protected void Confirmar_Kits(Object  Sender, EventArgs e)
		{
            if (operaciones.Rows.Count > 0)
            {
                for (int i = 0; i < operaciones.Rows.Count; i++)
                {
                    if (((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Text == "")
                    {
                        Utils.MostrarAlerta(Response, "El valor de la operación no puede estár vacio, Revise por favor!.. ");
                        return;
                    }
                }
            }
			//Debemos revisar si la orden de trabajo es de Peritaje
			Control principal = (this.Parent).Parent; //Control principal ascx
			Control controlDatosOrigen = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];

            string placaVeh = ((TextBox)controlDatosOrigen.FindControl("placa")).Text;
            Control controlDatosVehiculos = ((PlaceHolder)principal.FindControl("datosVehiculo")).Controls[0];
            string tipoOperacion = DBFunctions.SingleData("SELECT ttra_codigo FROM ttrabajoorden WHERE ttra_nombre='"+((DropDownList)controlDatosOrigen.FindControl("servicio")).SelectedItem.ToString()+"'");
            if (tipoOperacion != "P" && tipoOperacion != "C")
			{

				//Realizamos las validaciones correspondientes primero que existan las operaciones e items
				bool errorGrillas = false;
				bool errorBonos = false;
				bool errorFechas=false;
                bool errorCatalogo = false;

                if(!verificar_Catalogo(placaVeh))
                {
                    errorCatalogo = true;
                    Utils.MostrarAlerta(Response, "Existe un problema con la creación del vehículo, por favor valide de nuevo los datos del vehículo.");
                    control_Vehiculo();
                }
				if(!Validacion_Grillas())
				{
					errorGrillas = true;
                    Utils.MostrarAlerta(Response, "No hay Operaciones o Items para grabar en la Orden");
				}
				if(Verificar_Precios_Bonos())
				{
					errorBonos = true;
                    Utils.MostrarAlerta(Response, "Algun precio de operaciones por bono, no es válido");
				}
				if(!VerificarFechaHoraEntrega())
				{
					errorFechas=true;
                    Utils.MostrarAlerta(Response, "Existe alguno de los siguientes problemas : \\n1. La fecha o la hora estimada digitadas no tienen un formato válido \\n2. La fecha de entrega es menor a la fecha de la orden de trabajo \\n3. La combinación fecha-hora estimada de entrega es menor a la fecha-hora actual");
                }
				if((!errorGrillas)&&(!errorBonos) && !errorFechas && !errorCatalogo)
				{
                    ((Button)principal.FindControl("grabar")).Enabled = true;
                    Utils.MostrarAlerta(Response, "Ahora puede grabar la orden de trabajo!");
					Ocultar_Control();
                    //confirmar.Enabled = false;
				}
			}
			else
			{
				bool errorBonos = false;
				bool errorFechas=false;
				if(Verificar_Precios_Bonos())
				{
					errorBonos = true;
                    Utils.MostrarAlerta(Response, "Algun precio de operaciones por bono, no es válido");
				}
				if(!VerificarFechaHoraEntrega())
				{
					errorFechas=true;
                    Utils.MostrarAlerta(Response, "Existe alguno de los siguientes problemas : \\n1. La fecha o la hora estimada digitadas no tienen un formato válido \\n2. La fecha de entrega es menor a la fecha de la orden de trabajo \\n3. La combinación fecha-hora estimada de entrega es menor a la fecha-hora actual");
                }
				if(!errorBonos && !errorFechas)
				{
					//Response.Write("<script language:javascript>alert('En este momento puede llenar los datos del peritaje!');</script>");
					Ocultar_Control();
					Mostrar_Control_Peritaje();
                    ((Button)principal.FindControl("grabar")).Enabled = true;
                    Utils.MostrarAlerta(Response, "Ahora Puede Grabar la Orden de Trabajo. Después de grabar, espere e imprima los formatos.");
                }
			}

            if(operaciones.Rows.Count > 0)
            {
                if (!VerificarFechaHoraEntrega() || !Validacion_Grillas())
                {
                    return;
                }
                operaciones.Rows[operaciones.Rows.Count - 1][6] = ((DropDownList)kitsOperaciones.Items[operaciones.Rows.Count - 1].Cells[7].Controls[1]).SelectedValue;

                Control ctrlPadre = (this.Parent).Parent; //Control principal ascx
				string validaModificacion = ((Button)ctrlPadre.FindControl("grabar")).Text;

                if (DBFunctions.SingleData("select ctal_moditiempo from ctaller;") == "S" && validaModificacion != "Guardar Modificación")
                {
                    if (((TextBox)kitsOperaciones.Items[operaciones.Rows.Count - 1].Cells[4].Controls[1]).Text == "")
                    {
                        ((TextBox)kitsOperaciones.Items[operaciones.Rows.Count - 1].Cells[4].Controls[1]).Text = "0";
                    }
                    operaciones.Rows[operaciones.Rows.Count - 1][3] = ((TextBox)kitsOperaciones.Items[operaciones.Rows.Count - 1].Cells[4].Controls[1]).Text;
                    
                    operaciones.Rows[operaciones.Rows.Count - 1][4] = ((TextBox)kitsOperaciones.Items[operaciones.Rows.Count - 1].Cells[5].Controls[1]).Text;
                    BindDgOperaciones();
                }
            }
           
		}
		#endregion
		
		#region Manejo de Kits
		
		private void kitsCompletos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				//((Literal)e.Item.Cells[3].Controls[1]).Attributes.Add("OnClick","window.showModalDialog('AMS.Web.ModalDialogQuery.aspx?nomSecProc=CONSULTARITEMSKIT&numKeysEnc=1&nomCajNeg1=CODIGOKIT&EntCajNeg1="+e.Item.Cells[0].Text.Trim()+"','help','dialogHeight:350px;dialogWidth:350px;center:Yes; help:No;resizable:No;status:No;');");
				((Literal)e.Item.Cells[3].Controls[1]).Text = "<span onclick='javascript:window.open(\"AMS.Web.ModalDialogQuery.aspx?nomSecProc=CONSULTARITEMSKIT&numKeysEnc=1&nomCajNeg1=CODIGOKIT&EntCajNeg1="+e.Item.Cells[0].Text.Trim()+"\",\"help\",\"\");' style='cursor:pointer'>Ver Detalle</span>";
			}
		}

		protected void dgSeleccion_Previo(object sender, DataGridCommandEventArgs e) 
		{
			DataTable datosGrilla = new DataTable();
			datosGrilla = (DataTable)Session["kitsTabla"];
			string codigoKit = datosGrilla.Rows[e.Item.ItemIndex][0].ToString();
			if(((Button)e.CommandSource).CommandName == "Previo")
			{
				//Aqui me dispara la ventana emergente que me muestra cual es contenido de este combo								
				string select1 = "SELECT MI.mite_codigo ,MI.mite_nombre FROM mitems MI, mkititem MK WHERE MK.mkit_coditem = MI.mite_codigo AND MK.mkit_codikit=--"+codigoKit+"--"/*"SELECT mkit_coditem, mkit_cantitem FROM mkititem WHERE mkit_codikit=-"+codigoKit+"-"*/;
				string select2 = "SELECT PT.ptem_operacion ,PT.ptem_descripcion FROM ptempario PT, mkitoperacion MK WHERE PT.ptem_operacion=MK.mkit_operacion AND MK.mkit_codikitoper=--"+codigoKit+"--";
				Response.Write("<script language:javascript>window.showModalDialog('AMS.Web.ModalDialogQuery.aspx?cantSelec=2&cantColumn=2&column1=CODIGO&column2=DESCRIPCION&valColumn11=&valColumn12=&valColumn21=&valColumn22=&select1="+select1+"&select2="+select2+"&titulo=INFORMACION DEL KIT CON CODIGO "+codigoKit+"','help','dialogHeight:350px;dialogWidth:350px;center:Yes; help:No;resizable:No;status:No;');</script>");
				//lb.Text=select1+"<br>"+select2;
			}
			else if(((Button)e.CommandSource).CommandName == "Seleccionar")
			{
                //if(!confirmar.Enabled)
                //    confirmar.Enabled = true;
				Control principal = (this.Parent).Parent; //Control principal ascx
				Control controlDatosOrden = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
                Distribuir_Kit(codigoKit, ((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedItem.Text, ((DropDownList)controlDatosOrden.FindControl("listaPrecios")).SelectedValue, DBFunctions.SingleData("SELECT pgru_grupo FROM pkit WHERE PKIT_VIGENCIA = 'V' and pkit_codigo='" + codigoKit + "'"));
				((Button)e.CommandSource).Enabled = false;
				// Aqui Adicionamos al ArrayList el numero del elemento que ha sido escogido 				
				escogidos.Add(e.Item.DataSetIndex.ToString());
				escogidos2.Add(codigoKit);
				//this.Calcular_Salida();
				Verificar_Grilla_Operaciones();
				Session["escogidos"] = escogidos;
				Session["escogidos2"] = escogidos2;
			}
		}
		
		protected void Preparar_Tabla_Kits()
		{
			kitsTabla = new DataTable();
			kitsTabla.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			kitsTabla.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
		}
		
		protected void LlenarKits(string grupoCatalogo)
		{
			Preparar_Tabla_Kits();
			kitsGrupoCatalogo = new DataSet();
            DBFunctions.Request(kitsGrupoCatalogo, IncludeSchema.NO, "SELECT pkit_codigo, pkit_nombre FROM pkit WHERE PKIT_VIGENCIA = 'V' and pgru_grupo='" + grupoCatalogo + "'");
            if (kitsGrupoCatalogo.Tables.Count > 0)
            {
                for (int i = 0; i < kitsGrupoCatalogo.Tables[0].Rows.Count; i++)
                {
                    DataRow fila;
                    fila = kitsTabla.NewRow();
                    fila["CODIGO"] = kitsGrupoCatalogo.Tables[0].Rows[i][0].ToString();
                    fila["DESCRIPCION"] = kitsGrupoCatalogo.Tables[0].Rows[i][1].ToString();
                    kitsTabla.Rows.Add(fila);
                }
            }
			Session["kitsTabla"] = kitsTabla;
			kitsCompletos.DataSource = kitsTabla;
			kitsCompletos.DataBind();
		}
		
		public void Distribuir_Kit(string codigoKit, string cargo, string listaPrecios, string grupoCatalogo)
		{
            string errors = "";
			int i;
			double totalC = Convert.ToDouble(total.Text.Substring(1));
			Control principal = (this.Parent).Parent;
			Control controlDatosOrigen = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
			string cargoEscogidoDatosOrden = ((DropDownList)controlDatosOrigen.FindControl("cargo")).SelectedValue.Trim();
            Control almacenTransferencia = controlDatosOrigen.FindControl("almacenTransferencia");            
            almTrans = ((DropDownList)almacenTransferencia).SelectedValue;
            //Carga de los items que pertenecen a un Kit
            DataSet itemsKid = new DataSet();
            string listaPrecio = DBFunctions.SingleData("SELECT ppre_codigo FROM pkit WHERE PKIT_VIGENCIA = 'V' and pkit_codigo='" + codigoKit + "'");
			//DBFunctions.Request(itemsKid,IncludeSchema.NO,"SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(MKI.mkit_coditem,PLIN.plin_tipo), MIT.mite_nombre, TOR.tori_nombre, MPI.mpre_precio,CASE WHEN MIG.mig_cantidaduso IS NOT NULL THEN MIG.mig_cantidaduso WHEN MIG.mig_cantidaduso IS NULL THEN MIT.mite_usoxvehi END, MIT.pdes_codigo, MIT.piva_porciva, MIT.plin_codigo, MKI.mkit_coditem FROM mkititem MKI, mitems MIT, torigenitem TOR, mprecioitem MPI, mitemsgrupo MIG, plineaitem PLIN WHERE MKI.mkit_codikit='"+codigoKit+"' AND MIT.mite_codigo = MKI.mkit_coditem AND TOR.tori_codigo = MIT.tori_codigo AND MPI.mite_codigo = MIT.mite_codigo AND MPI.ppre_codigo = '"+listaPrecio+"' AND MIG.mite_codigo = MIT.mite_codigo AND MIG.pgru_grupo = '"+valToInsertar1EX.Text+"' AND MIT.plin_codigo = PLIN.plin_codigo");
			//
			DBFunctions.Request(itemsKid,IncludeSchema.NO, @"SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(MKI.mkit_coditem,PLIN.plin_tipo),MIT.mite_nombre, TOR.tori_nombre, COALESCE(MPI.mpre_precio,-1),
	                               MIT.pdes_codigo,MIT.piva_porciva,MIT.plin_codigo,MKI.mkit_coditem, mite_usoxvehi, COALESCE(mig_cantidaduso,MITE_USOXVEHI) as MITE_USOXGRUPO 
                            FROM dbxschema.mkititem MKI, dbxschema.torigenitem TOR, dbxschema.plineaitem PLIN, dbxschema.mitems MIT 
                             LEFT JOIN dbxschema.mprecioitem MPI ON MPI.ppre_codigo = '" + listaPrecio + @"' AND MPI.mite_codigo = MIT.mite_codigo 
   							 LEFT JOIN dbxschema.mitemsgrupo mig ON mig.mite_codigo=mig.mite_codigo AND pgru_grupo='" + valToInsertar1EX.Text + @"'
                      WHERE MKI.mkit_codikit = '" + codigoKit+@"' AND MIT.mite_codigo = MKI.mkit_coditem AND TOR.tori_codigo = MIT.tori_codigo
                             AND MIT.plin_codigo = PLIN.plin_codigo; ");
			for(i=0;i<itemsKid.Tables[0].Rows.Count;i++)
			{
				if(items.Columns.Count == 0)
					Preparar_Tabla_Items();
				//Primero debemos revisar si la operacion ya fue agregada, si es asi no la agregamos
				DataRow[] selection = items.Select("CODIGOREPUESTOORIG='"+itemsKid.Tables[0].Rows[i][7].ToString()+"'");
				if(selection.Length == 0)
				{
					DataRow fila = items.NewRow();
					fila[0] = codigoKit;
					fila[1] = itemsKid.Tables[0].Rows[i][0].ToString();
					fila[2] = itemsKid.Tables[0].Rows[i][1].ToString();
					fila[3] = itemsKid.Tables[0].Rows[i][2].ToString();
					fila[4] = Convert.ToDouble(itemsKid.Tables[0].Rows[i][3]);
					string cantidadGrupo = itemsKid.Tables[0].Rows[i]["MITE_USOXGRUPO"].ToString(); //DBFunctions.SingleData("SELECT mig_cantidaduso FROM mitemsgrupo WHERE mite_codigo='"+itemsKid.Tables[0].Rows[i][7].ToString()+"' AND pgru_grupo='"+valToInsertar1EX.Text+"'");
                    if (cantidadGrupo != "")
                        fila[5] = Convert.ToDouble(cantidadGrupo);
                    else
                        fila[5] = Convert.ToDouble(itemsKid.Tables[0].Rows[i]["MITE_USOXVEHI"].ToString()); // DBFunctions.SingleData("SELECT mite_usoxvehi FROM mitems WHERE mite_codigo='"+itemsKid.Tables[0].Rows[i][7].ToString()+"'"));
					fila[6] = itemsKid.Tables[0].Rows[i][4].ToString();
					fila[7] = Convert.ToDouble(itemsKid.Tables[0].Rows[i][5]);
					fila[8] = (Convert.ToDouble(fila[4])+(Convert.ToDouble(fila[4])*(Convert.ToDouble(fila[7])/100)))*Convert.ToDouble(fila[5]);
					fila[9] = cargo;
					fila[10] = itemsKid.Tables[0].Rows[i][7].ToString();
					fila[11] = itemsKid.Tables[0].Rows[i][6].ToString();
					fila[12] = Referencias.ConsultaSemaforoDisponibilidad(itemsKid.Tables[0].Rows[i][7].ToString(),almTrans,mesInventario,anoInventario);
					items.Rows.Add(fila);
					totalC += (Convert.ToDouble(fila[4])+(Convert.ToDouble(fila[4])*(Convert.ToDouble(fila[7])/100)))*Convert.ToDouble(fila[5]);
				}
			}
			BindDgItems();
			// Fin Carga de los items que pertenecen a un Kit
			////////////////////////////////////////////////////
			// Carga de las operaciones que pertenecen a un kit
			DataSet operacionesKid = new DataSet();
			double porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
			DBFunctions.Request(operacionesKid,IncludeSchema.NO,"SELECT DISTINCT MKO.mkit_operacion, PTE.ptem_descripcion, CASE WHEN MKO.mkit_operacion NOT IN (SELECT ptie_tempario FROM ptiempotaller WHERE ptie_grupcata='"+valToInsertar1EX.Text+"') THEN PTE.ptem_tiempoestandar ELSE PTI.ptie_tiemclie END, PTE.ttip_codiliqu, PTE.ptem_exceiva, PTE.tope_codigo FROM mkitoperacion MKO, ptempario PTE, ptiempotaller PTI WHERE MKO.mkit_codikitoper = '"+codigoKit+"' AND MKO.mkit_operacion = PTE.ptem_operacion AND ((PTI.ptie_tempario = MKO.mkit_operacion AND PTI.ptie_grupcata='"+valToInsertar1EX.Text+"') OR MKO.mkit_operacion NOT IN (SELECT ptie_tempario FROM ptiempotaller WHERE ptie_grupcata='"+valToInsertar1EX.Text+"')) ORDER BY MKO.mkit_operacion");
			for(i=0;i<operacionesKid.Tables[0].Rows.Count;i++)
			{
				if(operaciones.Columns.Count == 0)
					Preparar_Tabla_Operaciones();
				DataRow[] selection = operaciones.Select("CODIGOOPERACION='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 0)
				{
					string errorInt = "";
					DataRow fila = operaciones.NewRow();
					fila[0] = codigoKit;
					fila[1] = operacionesKid.Tables[0].Rows[i][0].ToString();
					fila[2] = operacionesKid.Tables[0].Rows[i][1].ToString();
					fila[3] = Convert.ToDouble(operacionesKid.Tables[0].Rows[i][2].ToString());
					if(operacionesKid.Tables[0].Rows[i][4].ToString() == "S")
						fila[5] = "SI";
					else if(operacionesKid.Tables[0].Rows[i][4].ToString() == "N")
						fila[5] = "NO";				
					//Rutina para calcular el valor de la operacion que se va a realizar
					double valorOperacion = 0;
					if(operacionesKid.Tables[0].Rows[i][3].ToString() =="F")
					{
						if(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'") == String.Empty)
							errorInt += "- La operación "+operacionesKid.Tables[0].Rows[i][0].ToString()+" no tiene configurado un valor en el tempario.\\n Operación no agregada.\\n";
						else
							valorOperacion = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'"));
					}
					else if(operacionesKid.Tables[0].Rows[i][3].ToString() == "T")
					{
						if(cargoEscogidoDatosOrden == "S" || cargoEscogidoDatosOrden == "C")
						{
							if(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'") == String.Empty || DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'") == String.Empty)
								errorInt += "- La operación "+operacionesKid.Tables[0].Rows[i][0].ToString()+" no tiene configurado un valor en la lista de precios "+listaPrecios+" o no tiene configurado un valor de tiempo para el grupo de catalogo "+grupoCatalogo+".\\n Operación no agregada.\\n";
							else
								valorOperacion = (Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'")))*(Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'")));
						}
						else if(cargoEscogidoDatosOrden == "A" || cargoEscogidoDatosOrden == "I" || cargoEscogidoDatosOrden == "T")
						{
							if(DBFunctions.SingleData("SELECT ppreta_valohorainte FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'") == String.Empty || DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'") == String.Empty)
								errorInt += "- La operación "+operacionesKid.Tables[0].Rows[i][0].ToString()+" no tiene configurado un valor en la lista de precios "+listaPrecios+" o no tiene configurado un valor de tiempo para el grupo de catalogo "+grupoCatalogo+".\\n Operación no agregada.\\n";
							else
								valorOperacion = (Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohorainte FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'")))*(Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'")));
						}
						else if(cargoEscogidoDatosOrden == "G")
						{
							if(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'") == String.Empty || DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'") == String.Empty)
								errorInt += "- La operación "+operacionesKid.Tables[0].Rows[i][0].ToString()+" no tiene configurado un valor en la lista de precios "+listaPrecios+" o no tiene configurado un valor de tiempo para el grupo de catalogo "+grupoCatalogo+".\\n Operación no agregada.\\n";
							else
								valorOperacion = (Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'")))*(Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+operacionesKid.Tables[0].Rows[i][0].ToString()+"'")));
						}
					}
					if(operacionesKid.Tables[0].Rows[i][3].ToString() != "B")
					{
						if(operacionesKid.Tables[0].Rows[i][4].ToString()=="N")
							valorOperacion = valorOperacion + (valorOperacion*(porcentajeIva/100));				
						fila[4] = valorOperacion;
					}				
					//Fin de la rutina que calcula el valor de la operacion a realizar
					fila[7] = cargo;
					string mecanicoResultado = Busqueda_Mecanico(operacionesKid.Tables[0].Rows[i][5].ToString());
					if(mecanicoResultado == "")
					{
						fila[6] = mecanicoResultado;
						fila[8] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaoper = 'S'");// Aun pendiente por asignación de mecanicos
					}
					else
					{
						fila[6] = mecanicoResultado;
						fila[8] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaoper = 'A'");// Se ha logrado asignar un mecacnico
					}
					fila[9] = "0";
					fila[10] = "0";
					fila[11] = "0";
					fila[12] = "0";			
					if(errorInt == String.Empty)
					{
						operaciones.Rows.Add(fila);
						totalC += valorOperacion;
					}
					else
						errors += errorInt;
				}
			}
			total.Text = totalC.ToString("C");
			BindDgOperaciones();
			Guardar_Datos_Operaciones();
			if(errors.Trim().Length > 0)
                Utils.MostrarAlerta(Response, "" + errors + "");
			Verificar_Grilla_Kits();
		}
		#endregion 
		
		#region Manejo de Repuestos
		protected void dgSeleccion_Bound(object sender, DataGridItemEventArgs e)
		{
			Control principal = (this.Parent).Parent;
			Control controlDatosOrigen = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
			Control controlDatosVehiculos = ((PlaceHolder)principal.FindControl("datosVehiculo")).Controls[0];
			DatasToControls bind = new DatasToControls();
			if(e.Item.ItemType == ListItemType.Footer)
			{
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[03].Controls[1]),"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[10].Controls[1]),"SELECT tcar_cargo, tcar_nombre FROM tcargoorden WHERE TCAR_CARGO <> 'X' ");
             //   hdcat.Value = ((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue;
			    ((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onclick","MostrarItems("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+",'"+hdcat.Value+"',"+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+",'"+ ((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue + "');");
				((DropDownList)e.Item.Cells[3].Controls[1]).Attributes.Add("onchange","LimpiarValores("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[2].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[4].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[6].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[7].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[8].Controls[1]).ClientID+");");
				((DropDownList)e.Item.Cells[10].Controls[1]).SelectedValue = ((DropDownList)controlDatosOrigen.FindControl("cargo")).SelectedValue;
			}
		}
		
		protected void dgSeleccion_Items(object sender, DataGridCommandEventArgs e) 
		{
			double totalC = 0;
			try{totalC = Convert.ToDouble(total.Text.Substring(1));}catch{}
			if(e.CommandName == "AddDatasRow1")
			{
                if (items.Columns.Count == 0)
                    Preparar_Tabla_Items(); //h

				if((((TextBox)e.Item.Cells[1].Controls[1]).Text) == "" || 
                    (((TextBox)e.Item.Cells[2].Controls[1]).Text) == "" || 
                    (((TextBox)e.Item.Cells[4].Controls[1]).Text) == "" || 
                    (((TextBox)e.Item.Cells[6].Controls[1]).Text) == "" || 
                    (((TextBox)e.Item.Cells[7].Controls[1]).Text) == "" || 
                    (((TextBox)e.Item.Cells[8].Controls[1]).Text) == "")
                    Utils.MostrarAlerta(Response, "Aun falta algun dato para poder adicionar un nuevo item");
				else
				{
					DataRow[] selection = this.items.Select("CODIGOREPUESTO='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'");
					if(selection.Length == 0)
					{
						string errors = "";
                        //if(!confirmar.Enabled)
                        //    confirmar.Enabled = true;
						Control principal = (this.Parent).Parent; //Control principal ascx
						Control controlDatosOrigen = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
						string cargoEscogidoDatosOrden = ((DropDownList)controlDatosOrigen.FindControl("cargo")).SelectedValue;
						string codI = "";
						Referencias.Guardar(((TextBox)e.Item.Cells[1].Controls[1]).Text,ref codI,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Split('-'))[1]);
						DataRow fila = items.NewRow();
						fila[0] = "";
						fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
						fila[2] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
						fila[3] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
						double valorFacturado = 0;
						if(DBFunctions.SingleData("SELECT mpre_precio FROM mprecioitem WHERE mite_codigo='"+codI+"' AND ppre_codigo='"+((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue+"'") == String.Empty)
							errors += "- El item "+((TextBox)e.Item.Cells[1].Controls[1]).Text+" no tiene configurado un precio en la lista de precios "+((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedItem.Text+"\\n";
						else
						{
							fila[4] = Convert.ToDouble(DBFunctions.SingleData("SELECT mpre_precio FROM mprecioitem WHERE mite_codigo='"+codI+"' AND ppre_codigo='"+((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue+"'"));
							valorFacturado = ((Convert.ToDouble(DBFunctions.SingleData("SELECT mpre_precio FROM mprecioitem WHERE mite_codigo='"+codI+"' AND ppre_codigo='"+((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue+"'"))*(Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text)/100))+(Convert.ToDouble(DBFunctions.SingleData("SELECT mpre_precio FROM mprecioitem WHERE mite_codigo='"+codI+"' AND ppre_codigo='"+((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue+"'"))))* Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
						}
						fila[5] = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
						fila[6] = ((TextBox)e.Item.Cells[7].Controls[1]).Text;
						fila[7] = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
						fila[8] = valorFacturado;
						if(RevisionCargo(cargoEscogidoDatosOrden,((DropDownList)e.Item.Cells[10].Controls[1]).SelectedValue))
						{
							fila[9] = ((DropDownList)e.Item.Cells[10].Controls[1]).SelectedItem.Text;
							fila[10] = codI;
							fila[11] = (((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Split('-'))[0];
							fila[12] = Referencias.ConsultaSemaforoDisponibilidad(codI,almTrans,mesInventario,anoInventario);
							if(errors.Trim().Length == 0)
							{
								items.Rows.Add(fila);
								totalC += valorFacturado;
								totalC += Agregar_Operaciones(codI,((DropDownList)e.Item.Cells[10].Controls[1]).SelectedItem.Text,((DropDownList)controlDatosOrigen.FindControl("listaPrecios")).SelectedValue,((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue);
								total.Text = totalC.ToString("C");
								BindDgItems();
							}
							else
                                Utils.MostrarAlerta(Response, "" + errors + "");
						}
						else
                            Utils.MostrarAlerta(Response, "Cargo No Válido");
					}
					else
                        Utils.MostrarAlerta(Response, "Este Item ya se ha agregado anteriormente");
				}
			}
		}
		
		protected void DgItems_Delete(Object sender, DataGridCommandEventArgs e)
		{
			double totalOT = 0;
			try{totalOT = Convert.ToDouble(total.Text.Substring(1));}catch{}
			try
			{
				totalOT = totalOT - Remover_Operaciones(items.Rows[e.Item.DataSetIndex][1].ToString());
				totalOT = totalOT - Convert.ToDouble(items.Rows[e.Item.DataSetIndex][8]);
				items.Rows[e.Item.DataSetIndex].Delete();
				BindDgItems();
				total.Text = totalOT.ToString("C");
			}
            catch (Exception ex)
            {
                Utils.MostrarAlerta(Response, "No se pudo eliminar la fila");lb.Text = ex.ToString();
            }
		}
		
		protected void Preparar_Tabla_Items()
		{
			items = new DataTable();
			items.Columns.Add(new DataColumn("CODIGOKIT",System.Type.GetType("System.String")));//0
			items.Columns.Add(new DataColumn("CODIGOREPUESTO",System.Type.GetType("System.String")));//1
			items.Columns.Add(new DataColumn("REFERENCIA",System.Type.GetType("System.String")));//2
			items.Columns.Add(new DataColumn("ORIGEN",System.Type.GetType("System.String")));//3
			items.Columns.Add(new DataColumn("PRECIO",System.Type.GetType("System.Double")));//4
			items.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));//5
			items.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//6
			items.Columns.Add(new DataColumn("IVA",System.Type.GetType("System.Double")));//7
			items.Columns.Add(new DataColumn("VALORFACTURADO",System.Type.GetType("System.Double")));//8
			items.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));//9
			items.Columns.Add(new DataColumn("CODIGOREPUESTOORIG",System.Type.GetType("System.String")));//10
			items.Columns.Add(new DataColumn("LINEABODEGA",System.Type.GetType("System.String")));//11
			items.Columns.Add(new DataColumn("SEMAFORODISPONIBILIDAD",System.Type.GetType("System.Int32")));//12
			Session["items"] = items;
		}
		
		protected void BindDgItems()
		{
			int i;
			kitsItems.DataSource = items;
			kitsItems.DataBind();
			Session["items"] = items;
			for(i=0;i<items.Rows.Count;i++)
			{
				if(items.Rows[i][12].ToString() == "0")
					kitsItems.Items[i].Cells[6].BackColor = Color.Red;
				else if(items.Rows[i][12].ToString() == "1")
					    kitsItems.Items[i].Cells[6].BackColor = Color.Yellow;
				else if(items.Rows[i][12].ToString() == "2")
					    kitsItems.Items[i].Cells[6].BackColor = Color.Green;
				else
					lb.Text += "<br>"+items.Rows[i][12].ToString();
			}
			DatasToControls.JustificacionGrilla(kitsItems,items);
		}
		
		protected double Agregar_Items(string codigoOperacion, string cargoAsociado, string listaPreciosItems, string listaPreciosOperaciones)
		{
			DataSet ds = new DataSet();
			double parcial = 0;
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DISTINCT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo), MIT.mite_nombre, TOR.tori_nombre, MPI.mpre_precio, CASE WHEN MIG.mig_cantidaduso IS NOT NULL THEN MIG.mig_cantidaduso WHEN MIG.mig_cantidaduso IS NULL THEN MIT.mite_usoxvehi END, MIT.pdes_codigo, MIT.piva_porciva, MIT.mite_codigo, MIT.plin_codigo FROM mitems MIT, torigenitem TOR, mprecioitem MPI, mitemsgrupo MIG, mitemoperacion MIO, plineaitem PLIN WHERE TOR.tori_codigo = MIT.tori_codigo AND MPI.mite_codigo = MIT.mite_codigo AND MPI.ppre_codigo = '"+listaPreciosItems+"' AND MIG.mite_codigo = MIT.mite_codigo AND MIG.pgru_grupo = '"+this.valToInsertar1EX.Text+"' AND (MIO.mite_codigo = MIT.mite_codigo AND MIO.ptem_operacion='"+codigoOperacion+"') AND MIT.plin_codigo=PLIN.plin_codigo");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] selection = this.items.Select("CODIGOREPUESTO='"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 0)
				{
					DataRow fila = items.NewRow();
					fila[0] = "";
					fila[1] = ds.Tables[0].Rows[i][0].ToString();
					fila[2] = ds.Tables[0].Rows[i][1].ToString();
					fila[3] = ds.Tables[0].Rows[i][2].ToString();
					fila[4] = Convert.ToDouble(ds.Tables[0].Rows[i][3]);
					fila[5] = Convert.ToDouble(ds.Tables[0].Rows[i][4]);
					fila[6] = ds.Tables[0].Rows[i][5].ToString();
					fila[7] = Convert.ToDouble(ds.Tables[0].Rows[i][6]);
					fila[8] = (Convert.ToDouble(fila[4])+(Convert.ToDouble(fila[4])*(Convert.ToDouble(fila[7])/100)))*Convert.ToDouble(fila[5]);
					fila[9] = cargoAsociado;
					fila[10] = ds.Tables[0].Rows[i][7].ToString();
					fila[11] = ds.Tables[0].Rows[i][8].ToString();
					fila[12] = Referencias.ConsultaSemaforoDisponibilidad(ds.Tables[0].Rows[i][7].ToString(),almTrans,mesInventario,anoInventario);
					items.Rows.Add(fila);
					parcial += (Convert.ToDouble(fila[4])+(Convert.ToDouble(fila[4])*(Convert.ToDouble(fila[7])/100)))*Convert.ToDouble(fila[5]);
					parcial += Agregar_Operaciones(fila[1].ToString(),cargoAsociado,listaPreciosOperaciones,listaPreciosItems);
				}
			}
			BindDgItems();
			return parcial;
		}
		
		protected double Remover_Items(string codigoOperacion)
		{
			int i,j;
			double valorRestar = 0;
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mite_codigo FROM mitemoperacion WHERE ptem_operacion='"+codigoOperacion+"'");
			for(i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] selection = this.items.Select("CODIGOREPUESTO = '"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 1)
				{
					bool eliminar = true;
					DataSet ds1 = new DataSet();
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT ptem_operacion FROM mitemoperacion WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND ptem_operacion <> '"+codigoOperacion+"'");
					for(j=0;j<ds1.Tables[0].Rows.Count;j++)
					{
						DataRow[] selection1 = this.operaciones.Select("CODIGOOPERACION = '"+ds1.Tables[0].Rows[j][0].ToString()+"'");
						if(selection1.Length == 1)
								eliminar = false;
					}
					if(eliminar)
					{
						for(j=0;j<this.items.Rows.Count;j++)
						{
							if(items.Rows[j] == selection[0])
							{
								valorRestar += Convert.ToDouble(this.items.Rows[j][8]);
								items.Rows[j].Delete();
							}
						}
					}
				}
			}
			BindDgItems();
			return valorRestar;
		}
		
		#endregion
		
		#region Manejo de Operaciones
		protected void dgSeleccion_Operaciones_Bound(object sender, DataGridItemEventArgs e)
		{
            DatasToControls bind = new DatasToControls();
            Control principal = (this.Parent).Parent;
			Control controlDatosOrigen = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
			
			if(e.Item.ItemType == ListItemType.Footer)
			{
				((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onclick","ModalDialogAT(this,'"+valToInsertar1EX.ClientID+ "','SELECT DISTINCT ptem_operacion AS CODIGO, ptem_descripcion AS NOMBRE, ptem_exceiva AS EXCENCION_IVA, ttip_detaliqu as liquidacion FROM TTIPOLIQUIDACIONTALLER t, ptempario p WHERE p.TTIP_CODILIQU = t.TTIP_CODILIQU and (ptem_operacion IN (SELECT ptie_tempario FROM ptiempotaller WHERE ptie_grupcata=\\'@\\') OR ptem_indigeneric=\\'S\\') AND TVIG_VIGENCIA <> \\'N\\' ORDER BY ptem_descripcion', new Array(),1,'Catalogo Nulo');");
                ((TextBox)e.Item.Cells[1].Controls[1]).Text = codigoTempOp;
                ((TextBox)e.Item.Cells[2].Controls[1]).Text = nombreTempOp;

                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[8].Controls[1]),"SELECT tcar_cargo, tcar_nombre FROM tcargoorden ORDER BY 2");
                //si es cargo garantía
                if (Session["garantia"] != null)
                {
                    if (Session["garantia"].ToString() == "2")
                        try
                        {
                            ((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue = "G";
                            Session["garantia"] = null;
                        }
                        catch
                        {

                        }
                }
                else
                {
                    string cargo = ((DropDownList)controlDatosOrigen.FindControl("cargo")).SelectedValue;
                    ((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue = cargo;
                }        
                //Establecemos por defecto el mismo cargo que se configuro inicialmente con la ot
                //((DropDownList)e.Item.Cells[7].Controls[1]).SelectedValue = ((DropDownList)controlDatosOrigen.FindControl("cargo")).SelectedValue;         

                String excentoIva = DBFunctions.SingleData("select ptem_exceiva from dbxschema.ptempario where ptem_operacion='" + codigoTempOp + "';" );
                ((TextBox)e.Item.Cells[6].Controls[1]).Text = excentoIva;

                //Se estable por defecto para creacion de OT el seleccionado en datos de la orden
                

			}
            else if (e.Item.ItemType == ListItemType.Item ||e.Item.ItemType == ListItemType.AlternatingItem)
            {
                 Single value = 0;
                 try
                 {
                     value = Single.Parse(((DataBoundLiteralControl)e.Item.Cells[5].Controls[2]).Text.ToString(), NumberStyles.Currency);
                 }
                 catch
                 {
                     value = 0;
                 }
                 ((TextBox)e.Item.Cells[5].Controls[1]).Text = value.ToString();
            }
		}
		
		protected void DgOperaciones_Delete(Object sender, DataGridCommandEventArgs e)
		{
			double totalOT = 0;
            try{totalOT = Convert.ToDouble(total.Text.Substring(1));}catch{}
			try
			{
                string valOper = this.operaciones.Rows[e.Item.DataSetIndex][4].GetType() != typeof(DBNull) ?
                    this.operaciones.Rows[e.Item.DataSetIndex][4].ToString() : "0";

				totalOT = totalOT - this.Remover_Items(this.operaciones.Rows[e.Item.DataSetIndex][1].ToString());
                totalOT = totalOT - System.Convert.ToDouble(valOper);
				operaciones.Rows[e.Item.DataSetIndex].Delete();
				BindDgOperaciones();
				Verificar_Grilla_Operaciones();
				total.Text = totalOT.ToString("C");
			}
            catch (Exception ex)
            {
                Utils.MostrarAlerta(Response, "No se pudo eliminar la fila");lb.Text = ex.ToString();
            }
		}
		
		protected void dgSeleccion_Operaciones(object sender, DataGridCommandEventArgs e) 
		{
            double totalC = 0;
            try {totalC = Convert.ToDouble(total.Text.Substring(1)); }
            catch { }
			if(e.CommandName == "AddDatasRow2")
			{
				if((((TextBox)e.Item.Cells[1].Controls[1]).Text)=="")
                    Utils.MostrarAlerta(Response, "Aun falta algun dato para poder adicionar un nuevo item");
				else
				{
                    DataRow[] selection = this.operaciones.Select("CODIGOOPERACION='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'");
					if(selection.Length == 0)
					{
                        //if(!confirmar.Enabled)
                        //    confirmar.Enabled = true;
						Guardar_Datos_Operaciones();
						//Se carga el control que nos informa que tipo de cargo se tiene seleccionado
						Control principal = (this.Parent).Parent;
						Control controlDatosOrigen = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
						string cargoEscogidoDatosOrden = ((DropDownList)controlDatosOrigen.FindControl("cargo")).SelectedValue;
						string listaPrecios  = ((DropDownList)controlDatosOrigen.FindControl("listaPrecios")).SelectedValue;
						string estaOperacion = ((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim();
						//Se carga el control que nos informa que tipo de vehiculo es para poder calcular el tiempo de la operacion
						Control controlDatosVehiculo = ((PlaceHolder)principal.FindControl("datosVehiculo")).Controls[0];
						string grupoCatalogo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='"+((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue+"'");
						DataRow fila = operaciones.NewRow();
                        DataSet dsOperacion = new DataSet();
                        string sqlOperacion = @"SELECT COALESCE(ptie_tiemclie,0) as tiempo_catalogo, COALESCE(ptem_tiempoestandar,1) as tiempo_standard, 
                                                       ttip_codiliqu as tipoLiquidacion,ce.cemp_porciva as iva, ptem_valooper as valorfijooperacion,  
	                                                   coalesce(tope_codigo,'MOB') as especialidad  
                                                  FROM cempresa ce, ptempario pt   
                                                  left join ptiempotaller ptt on ptie_grupcata='"+grupoCatalogo+@"' AND ptie_tempario = ptem_operacion  
                                                 WHERE pTEM_OPERACION ='"+estaOperacion+@"';  
                                                SELECT COALESCE(ppreta_valohoraclie,0)AS VALOR_CLIENTE,  
	                                                   COALESCE(ppreta_valohoraINTE,0)AS VALOR_INTERNO,  
	                                                   COALESCE(ppreta_valohoraGTIA,0)AS VALOR_GARANTIA  
                                                  FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"' ";
                        DBFunctions.Request(dsOperacion, IncludeSchema.NO, sqlOperacion);
                        fila[0] = "";
						fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
						fila[2] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
						//try{fila[3] = Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'").Trim());}
						//catch{fila[3] = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'"));}
                        if (Convert.ToDouble(dsOperacion.Tables[0].Rows[0][0].ToString()) != 0)
                             fila[3] = Convert.ToDouble(dsOperacion.Tables[0].Rows[0][0].ToString());
                        else fila[3] = Convert.ToDouble(dsOperacion.Tables[0].Rows[0][1].ToString());
                        
                        string excencion = ((TextBox)e.Item.Cells[6].Controls[1]).Text;
                        if(excencion=="S")
							fila[5] = "SI";
						else if(excencion=="N")
							    fila[5] = "NO";
						//string tipoLiquidacion = DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'");
						string tipoLiquidacion = dsOperacion.Tables[0].Rows[0][2].ToString();
                        //double porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
						double porcentajeIva = Convert.ToDouble(dsOperacion.Tables[0].Rows[0][3].ToString());
                        double valorOperacion = 0;
						if(tipoLiquidacion == "F")
					//		valorOperacion = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'"));
							valorOperacion = Convert.ToDouble( Convert.ToDouble(dsOperacion.Tables[0].Rows[0][4].ToString()));
						else if(tipoLiquidacion == "T")
						{
							double valorHora=0, tiempoCliente=0;
							if(((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue == "S" || ((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue == "C")
						//		valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'"));
							    valorHora = Convert.ToDouble(dsOperacion.Tables[1].Rows[0][0].ToString());
                            else if(((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue == "A" || ((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue == "I" || ((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue == "T")
						//		valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohorainte FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'"));
							    valorHora = Convert.ToDouble(dsOperacion.Tables[1].Rows[0][1].ToString());
                            else if(((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue == "G")
						//		valorHora = Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoragtia FROM ppreciotaller WHERE ppreta_codigo='"+listaPrecios+"'"));
							    valorHora = Convert.ToDouble(dsOperacion.Tables[1].Rows[0][2].ToString());
                        //    try{tiempoCliente=Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'"));}
						//	catch
						//	{
						//		if(Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'"))==0)
						//			tiempoCliente=1;
						//		else
						//			tiempoCliente=Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM dbxschema.ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'"));
						//	}
                            if (Convert.ToDouble(dsOperacion.Tables[0].Rows[0][0].ToString()) != 0)
                                tiempoCliente = Convert.ToDouble(dsOperacion.Tables[0].Rows[0][0].ToString());
                            else tiempoCliente = Convert.ToDouble(dsOperacion.Tables[0].Rows[0][1].ToString());
							valorOperacion=valorHora*tiempoCliente;
						}
						if(tipoLiquidacion!="B")
						{
						//	if(excencion=="N")
						//		valorOperacion = valorOperacion + (valorOperacion*(porcentajeIva/100));		// las operaciones e graban SIN IVA		
							fila[4] = valorOperacion;
						}
					    //	string especialidad = DBFunctions.SingleData("SELECT tope_codigo FROM ptempario WHERE ptem_operacion='"+((TextBox)e.Item.Cells[1].Controls[1]).Text+"'");
					    string especialidad = dsOperacion.Tables[0].Rows[0][5].ToString().Trim();
					
                        //////////////////////////////////////////////////////////////////////////////
						string mecanicoResultado = Busqueda_Mecanico(especialidad);
						if(mecanicoResultado=="")
						{
							fila[6] = mecanicoResultado;
							fila[8] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaoper = 'S'");// Aun pendiente por asigfnacion de mecanicos
						}
						else
						{
							fila[6] = mecanicoResultado;
							fila[8] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaoper = 'A'");// Aun pendiente por asigfnacion de mecanicos
						}
						fila[09] = "0";
						fila[10] = "0";
						fila[11] = "0";
						fila[12] = "0";
                        
						if(RevisionCargo(cargoEscogidoDatosOrden,((DropDownList)e.Item.Cells[8].Controls[1]).SelectedValue))
						{
							fila[7] = ((DropDownList)e.Item.Cells[8].Controls[1]).SelectedItem.Text;
							operaciones.Rows.Add(fila);
							BindDgOperaciones();
							Verificar_Grilla_Operaciones();
							totalC += valorOperacion;
							totalC += Agregar_Items(((TextBox)e.Item.Cells[1].Controls[1]).Text,((DropDownList)e.Item.Cells[8].Controls[1]).SelectedItem.Text,((DropDownList)controlDatosOrigen.FindControl("listaPreciosItems")).SelectedValue,((DropDownList)controlDatosOrigen.FindControl("listaPrecios")).SelectedValue);
							total.Text = totalC.ToString("C");
						}
						else
                            Utils.MostrarAlerta(Response, "Cargo No Válido");
					}
					else
                        Utils.MostrarAlerta(Response, "Esta Operación ya fue agregada");
				}
            }
		}
		
		protected void Preparar_Tabla_Operaciones()
		{
			operaciones = new DataTable();
			operaciones.Columns.Add(new DataColumn("CODIGOKIT",System.Type.GetType("System.String")));//0
			operaciones.Columns.Add(new DataColumn("CODIGOOPERACION",System.Type.GetType("System.String")));//1
			operaciones.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));//2
			operaciones.Columns.Add(new DataColumn("TIEMPOEST",System.Type.GetType("System.Double")));//3
			operaciones.Columns.Add(new DataColumn("VALOROPERACION",System.Type.GetType("System.Double")));//4
			operaciones.Columns.Add(new DataColumn("EXCENTOIVA",System.Type.GetType("System.String")));//5
			operaciones.Columns.Add(new DataColumn("TECNICO",System.Type.GetType("System.String")));//6
			operaciones.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));//7
			operaciones.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));//8
			operaciones.Columns.Add(new DataColumn("CODIGOINCIDENTE",System.Type.GetType("System.String")));//9
			operaciones.Columns.Add(new DataColumn("CODIGOGARANTIA",System.Type.GetType("System.String")));//10
			operaciones.Columns.Add(new DataColumn("CODIGOREMEDIO",System.Type.GetType("System.String")));//11
			operaciones.Columns.Add(new DataColumn("CODIGODEFECTO",System.Type.GetType("System.String")));//12
            operaciones.Columns.Add(new DataColumn("OBSERVACIONES", System.Type.GetType("System.String")));//13
			Session["operaciones"] = operaciones;
		}
		
		protected void BindDgOperaciones()
		{
			kitsOperaciones.DataSource = operaciones;
			kitsOperaciones.DataBind();
			Verificar_Grilla_Operaciones();
			Calcular_Salida();
			Session["operaciones"] = operaciones;
			DatasToControls.JustificacionGrilla(kitsOperaciones,operaciones);
		}
		
		string Busqueda_Mecanico(string especialidad)
		{
			string mecanico = "";
			double cantidadHoras = 100000;
			Control principal = (this.Parent).Parent; //Control principal ascx
			Control controlDatosOrden = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
			string taller = DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE palm_descripcion='"+((DropDownList)controlDatosOrden.FindControl("almacen")).SelectedItem.ToString()+"'");
			//Cargamos los mecanicos que pertenecen al taller escogido que tienen la especialidad solicitada por la operacion
			DataSet mecanicos = new DataSet();
			//DBFunctions.Request(mecanicos,IncludeSchema.NO,"SELECT PV.pven_codigo,PV.pven_nombre FROM pvendedor PV, pespecialidadmecanico PE WHERE PV.tvend_codigo='MG' AND PV.palm_almacen='"+taller+"' AND PE.pven_codigo=PV.pven_codigo AND PE.tope_codigo='"+especialidad+"' AND PV.pven_vigencia = 'V'");
             string sqlMecanico = @"SELECT PV.pven_codigo, PV.pven_nombre, COUNT(VMA.mord_numeorde)  
								FROM pvendedorALMACEN PVA, pvendedor PV 
                                    INNER JOIN pespecialidadmecanico PE ON PE.pven_codigo = PV.pven_codigo 
                                    LEFT JOIN vtaller_mecanicoasignadoot VMA ON PV.pven_codigo = VMA.pven_codigo  
							    WHERE PV.tvend_codigo in ('MG','TT') AND PVA.palm_almacen='" + taller + @"' AND PE.tope_codigo='" + especialidad + @"' AND PV.pven_vigencia = 'V'
                                AND   PV.PVEN_CODIGO = PVA.PVEN_CODIGO 
								GROUP BY PV.pven_codigo,PV.pven_nombre  
							    HAVING COUNT(VMA.mord_numeorde) < (SELECT COALESCE(ctal_maxopermc,4) FROM ctaller)";
            DBFunctions.Request(mecanicos,IncludeSchema.NO,sqlMecanico);
            //Ajuste a tecnicos en general

            Multitaller = DBFunctions.SingleData("SELECT CTAL_TECNMULTI FROM CTALLER");

            if (Multitaller == "")
                Multitaller = "N";

            //string sqlOperacion = "";
            //ViewState["sqlOperacion"] = "";
            if (Multitaller == "S")
            {
                ViewState["sqlOperacion"] = @"SELECT PV.pven_codigo,
                                    PV.pven_nombre,
                                    COUNT(VMA.mord_numeorde)
                                    FROM pespecialidadmecanico PE,pvendedor PV 
                                    LEFT JOIN vtaller_mecanicoasignadoot VMA
                                    ON PV.pven_codigo = VMA.pven_codigo 
                                    WHERE PV.tvend_codigo IN ('MG','TT') 
                                    AND   PV.pven_vigencia = 'V'
                                    AND   PE.tope_codigo = '" + especialidad + @"' 
                                    AND PE.pven_codigo = PV.pven_codigo  
                                    GROUP BY PV.pven_codigo,
                                    PV.pven_nombre
		                            HAVING COUNT(VMA.mord_numeorde) <(SELECT COALESCE(ctal_maxopermc,5)
                                    FROM ctaller)
                                    ORDER BY PVEN_NOMBRE";
            }
            else
                  // if (Multitaller =="N") // POR DEFECTo DEBE TOMAR N PORQUE SI NO PARMETRIZA SE REVIENTA....
            {

                ViewState["sqlOperacion"] = @"SELECT PV.pven_codigo, PV.pven_nombre, COUNT(VMA.mord_numeorde)  
								FROM pvendedorALMACEN PVA, pvendedor PV 
                                    INNER JOIN pespecialidadmecanico PE ON PE.pven_codigo = PV.pven_codigo 
                                    LEFT JOIN vtaller_mecanicoasignadoot VMA ON PV.pven_codigo = VMA.pven_codigo  
							    WHERE PV.tvend_codigo IN ('MG','TT') AND PVA.palm_almacen='" + taller + @"' AND PE.tope_codigo='" + especialidad + @"' AND PV.pven_vigencia = 'V' 
								AND   PV.PVEN_CODIGO = PVA.PVEN_CODIGO 
								GROUP BY PV.pven_codigo,PV.pven_nombre  
							    HAVING COUNT(VMA.mord_numeorde) < (SELECT COALESCE(ctal_maxopermc,4) FROM ctaller)";
            }


			//Ahora para cada mecanico encontrado vamos a cargar las operaciones que tiene asignadas en dordenoperacion
			for(int i=0;i<mecanicos.Tables[0].Rows.Count;i++)
			{
				DataSet operaciones = new DataSet();
                String sql = @"SELECT DOR.ptem_operacion, MC.pcat_codigo, SUM(COALESCE(DORD_TIEMPOPER,0.5)) AS TIEMPO_ASIGNADO   
                   FROM dordenoperacion DOR, morden MO, mcatalogovehiculo MC  
                  WHERE DOR.pven_codigo = '"+mecanicos.Tables[0].Rows[i].ItemArray[0].ToString()+@"'  
                   AND  DOR.test_estado = 'A'  
                   AND  MO.mord_numeorde = DOR.mord_numeorde  
                   AND  MO.pdoc_codigo = DOR.pdoc_codigo  
                   AND  MO.mcat_vin = MC.mcat_vin  
                   AND  MO.test_estado = 'A' and dor.test_estado in ('A','I','T') 
                  GROUP BY DOR.ptem_operacion, MC.pcat_codigo;";

				//Ahi que tener en cuenta que la operacion de peritaje no tiene tiempo establecido
				DBFunctions.Request(operaciones, IncludeSchema.NO, sql);
				if(operaciones.Tables[0].Rows.Count==0)
				{
					//mecanico = mecanicos.Tables[0].Rows[i][1].ToString(); //Se cambia por codigo para que la cargue en el DDL
                    mecanico = mecanicos.Tables[0].Rows[i][0].ToString();
					cantidadHoras = 0;
				}
				else
				{
					double horasMecanicoAsignadas = 0;
					for(int j=0;j<operaciones.Tables[0].Rows.Count;j++)
					{
						//string grupoCatalogo = valToInsertar1EX.Text;
						//try  {horasMecanicoAsignadas += Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(ptie_tiemclie,1)       FROM ptiempotaller WHERE ptie_tempario ='"+operaciones.Tables[0].Rows[j][0].ToString()+"' AND ptie_grupcata='"+grupoCatalogo+"'"));}
						//catch{horasMecanicoAsignadas += Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(ptem_tiempoestandar,1) FROM ptempario     WHERE ptem_operacion='"+operaciones.Tables[0].Rows[j][0].ToString()+"'"));}
					    horasMecanicoAsignadas += Convert.ToDouble(operaciones.Tables[0].Rows[j][2].ToString());
                    }
					if(horasMecanicoAsignadas<cantidadHoras)
					{
						//mecanico = mecanicos.Tables[0].Rows[i][1].ToString();
                        mecanico = mecanicos.Tables[0].Rows[i][0].ToString();
						cantidadHoras = horasMecanicoAsignadas;
					}
				}
			}                        
			return mecanico;            
		}
		
		protected void Verificar_Grilla_Operaciones()
		{
			//Para verificar los controles que debo cargar, recorro la grilla y miro de acuerdo a los eventos cuales debo activar y que debo activar			
            
            for(int i=0;i<operaciones.Rows.Count;i++)
			{
                string tipoLiquidacion = DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='"+operaciones.Rows[i].ItemArray[1].ToString()+"'");
				//Revisamos si alguna operacion es de tipo Liquidacion B
                
                switch(tipoLiquidacion)
                {
                    case "T":
                    case "F":
                        ((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Visible = true;
					    ((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Text    = operaciones.Rows[i][4].ToString();
                        ((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Enabled = false;
                        break;
                    case "B":
                        ((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Visible = true;
					    ((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Text    = operaciones.Rows[i][4].ToString();
                        ((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Enabled = true;
                        break;
                }

                DatasToControls bind = new DatasToControls();
                try
                {
                    bind.PutDatasIntoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[7].Controls[1]), ViewState["sqlOperacion"].ToString());
                    ((DropDownList)kitsOperaciones.Items[i].Cells[7].Controls[1]).SelectedValue = operaciones.Rows[i][6].ToString();
                }
                catch { }

                //tempario
                if (DBFunctions.SingleData("select ctal_moditiempo from ctaller;") == "S")
                    ((TextBox)kitsOperaciones.Items[i].Cells[4].Controls[1]).Text = operaciones.Rows[i][3].ToString();
                else
                    ((TextBox)kitsOperaciones.Items[i].Cells[4].Controls[1]).Enabled = false;

                //Poner observaciones
                ((TextBox)kitsOperaciones.Items[i].Cells[3].Controls[1]).Text = operaciones.Rows[i][13].ToString();

				//Ahora Revisamos si el cargo es igual a Gtía Fabrica para activar los TextBox del final de la Grilla
				if((operaciones.Rows[i][7].ToString().Trim())=="Gtía Fabrica")
				{
                    //DatasToControls bind = new DatasToControls();
					if(!((DropDownList)kitsOperaciones.Items[i].Cells[10].Controls[1]).Visible)
					{
						((DropDownList)kitsOperaciones.Items[i].Cells[10].Controls[1]).Visible = true;
                        bind.PutDatasIntoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[10].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'I' ORDER BY pgar_descripcion");
						DatasToControls.EstablecerDefectoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[10].Controls[1]),DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operaciones.Rows[i][9].ToString().Trim()+"'"));
					}
					if(!((DropDownList)kitsOperaciones.Items[i].Cells[11].Controls[1]).Visible)
					{
						((DropDownList)kitsOperaciones.Items[i].Cells[11].Controls[1]).Visible = true;
                        bind.PutDatasIntoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[11].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'C' ORDER BY pgar_descripcion");
						DatasToControls.EstablecerDefectoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[11].Controls[1]),DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operaciones.Rows[i][10].ToString().Trim()+"'"));
					}
					if(!((DropDownList)kitsOperaciones.Items[i].Cells[12].Controls[1]).Visible)
					{
						((DropDownList)kitsOperaciones.Items[i].Cells[12].Controls[1]).Visible = true;
                        bind.PutDatasIntoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[12].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'R' ORDER BY pgar_descripcion");
						DatasToControls.EstablecerDefectoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[12].Controls[1]),DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operaciones.Rows[i][11].ToString().Trim()+"'"));
					}
					if(!((DropDownList)kitsOperaciones.Items[i].Cells[13].Controls[1]).Visible)
					{
						((DropDownList)kitsOperaciones.Items[i].Cells[13].Controls[1]).Visible = true;
                        bind.PutDatasIntoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[13].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'D' ORDER BY pgar_descripcion");
						DatasToControls.EstablecerDefectoDropDownList(((DropDownList)kitsOperaciones.Items[i].Cells[13].Controls[1]),DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operaciones.Rows[i][12].ToString().Trim()+"'"));
	 				}
				}
			}
		}


		protected void Guardar_Datos_Operaciones()
		{
			for(int i=0;i<kitsOperaciones.Items.Count;i++)
			{
				if(((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Visible)
				{
					if(((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Text=="")
						((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Text = "0";
					operaciones.Rows[i][4] = System.Convert.ToDouble(((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Text);
				}

                operaciones.Rows[i][13] = ((TextBox)kitsOperaciones.Items[i].Cells[3].Controls[1]).Text;

                //tempario
                if (DBFunctions.SingleData("select ctal_moditiempo from ctaller;") == "S")
                    operaciones.Rows[i][3] = ((TextBox)kitsOperaciones.Items[i].Cells[4].Controls[1]).Text;

                if (((DropDownList)kitsOperaciones.Items[i].Cells[10].Controls[1]).Visible)
                    operaciones.Rows[i][9] = ((DropDownList)kitsOperaciones.Items[i].Cells[10].Controls[1]).SelectedValue;
                if (((DropDownList)kitsOperaciones.Items[i].Cells[11].Controls[1]).Visible)
                    operaciones.Rows[i][10] = ((DropDownList)kitsOperaciones.Items[i].Cells[11].Controls[1]).SelectedValue;
                if (((DropDownList)kitsOperaciones.Items[i].Cells[12].Controls[1]).Visible)
                    operaciones.Rows[i][11] = ((DropDownList)kitsOperaciones.Items[i].Cells[12].Controls[1]).SelectedValue;
                if (((DropDownList)kitsOperaciones.Items[i].Cells[13].Controls[1]).Visible)
                    operaciones.Rows[i][12] = ((DropDownList)kitsOperaciones.Items[i].Cells[13].Controls[1]).SelectedValue;

                try
                {
                    operaciones.Rows[i][6] = ((DropDownList)kitsOperaciones.Items[i].Cells[7].FindControl("mecanicoE")).SelectedValue;
                }
                catch (Exception err)
                {}
			}
		}
		
		//Funcion que nos permite calcular la fecha y hora estimada para la salida de un automovil del taller
		protected void Calcular_Salida()
		{
			string grupoCatalogoOrden =  valToInsertar1EX.Text;
			double duracionTrabajo = 0;
			if(operaciones.Rows.Count>0)
			{
				double cantidadHoras = 100000;
                //Sumar tiempos operaciones en duracionTrabajo
				for(int i=0;i<operaciones.Rows.Count;i++)
				{
					//Calculamos cuanto tiempo se demora esta orden en terminar
				//  try{duracionTrabajo += System.Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogoOrden+"' AND ptie_tempario='"+operaciones.Rows[i][1].ToString()+"'"));}
				// 	catch{duracionTrabajo += System.Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM ptempario WHERE ptem_operacion='"+operaciones.Rows[i][1].ToString()+"'"));}
                    duracionTrabajo += System.Convert.ToDouble(operaciones.Rows[i][3].ToString());
                    
                    //  traer el codigo del mecanico de la grilla porque puede haber dos mecanicos con el mismo nombre
					string codigoMecanico = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='"+operaciones.Rows[i][6].ToString().Trim()+"'");
					DataSet operacionesMecanico = new DataSet();
                    //Consultar operaciones pendientes del mecanico
                    string cargaTecnico = "SELECT DOR.mord_numeorde, DOR.ptem_operacion, SUM(COALESCE(DORD_TIEMPOPER,0.20)) FROM dordenoperacion DOR, morden MOR "+
                        " WHERE DOR.pven_codigo='" + codigoMecanico + "' AND MOR.test_estado='A' AND MOR.pdoc_codigo=DOR.pdoc_codigo AND MOR.mord_numeorde=DOR.mord_numeorde "+
                        " AND DOR.TEST_ESTADO IN ('A','I','T') GROUP BY DOR.mord_numeorde, DOR.ptem_operacion;";
					
					DBFunctions.Request(operacionesMecanico,IncludeSchema.NO,cargaTecnico);
					if(operacionesMecanico.Tables[0].Rows.Count>0)
					{
						double horasAsignadas = 0;
						///////cuarentena///////////////
						for(int j=0;j<operacionesMecanico.Tables[0].Rows.Count;j++)
						{
					//		string grupoCatalogo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo = (SELECT pcat_codigo FROM morden WHERE mord_numeorde="+operacionesMecanico.Tables[0].Rows[j][0].ToString()+")");
					//		try{horasAsignadas += System.Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_grupcata='"+grupoCatalogo+"' AND ptie_tempario='"+operacionesMecanico.Tables[0].Rows[j][1].ToString()+"'"));}
					//		catch{horasAsignadas += System.Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_tiempoestandar FROM ptempario WHERE ptem_operacion='"+operacionesMecanico.Tables[0].Rows[j][1].ToString()+"'"));}
						    horasAsignadas += System.Convert.ToDouble(operacionesMecanico.Tables[0].Rows[0][2].ToString());
                        }
						///////////////////////////////////
						if(horasAsignadas<cantidadHoras)
							cantidadHoras = horasAsignadas;
					}
					else if(operacionesMecanico.Tables[0].Rows.Count==0)
						    cantidadHoras =0;
				}
				//lb.Text += "<br>duracion trabajo : "+duracionTrabajo.ToString()+" - cantidad horas : "+cantidadHoras.ToString();
				/// Ahora vamos a calcular la hora en que se va a iniciar el trabajo del carro
				DateTime horaInicioOrden = DateTime.Now;
				DateTime horaFinalOrden = DateTime.Now;
				horaInicioOrden = DateTime.Now.AddHours(cantidadHoras);
				horaFinalOrden = DateTime.Now.AddHours(cantidadHoras+duracionTrabajo);
				TimeSpan duracionOrden = horaFinalOrden - horaInicioOrden;
				DateTime horaFinalTaller = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT ctal_hfmec FROM ctaller"));
				DateTime horaInicioTaller = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT ctal_himec FROM ctaller"));
				if(horaFinalOrden<horaFinalTaller)
				{
					horaFinalOrden = horaFinalOrden.AddMinutes(60);
					fechaEstimada.Text = horaFinalOrden.ToString("yyyy-MM-dd");
					horaEstimada.Text = horaFinalOrden.ToString("HH:mm:ss");
				}
				else
				{
					TimeSpan diferenciaTaller = horaFinalTaller - horaInicioTaller;
					DateTime fechaFinal = new DateTime();
					fechaFinal = horaInicioOrden;
					TimeSpan controlBucle = new TimeSpan(0,0,0);
					while(controlBucle < duracionOrden)
					{
						if(DateTime.Compare(fechaFinal,horaInicioOrden)==0)
						{
							TimeSpan diferencia = horaFinalTaller - horaInicioOrden;
							fechaFinal = fechaFinal.AddHours(24);
							fechaFinal = fechaFinal.Add(horaInicioTaller.TimeOfDay-horaInicioOrden.TimeOfDay);
							controlBucle += diferencia;
						}
						else
						{
							//Si requiere mas del tiempo disponible en un dia de trabajo
							if((duracionOrden-controlBucle)>=diferenciaTaller)
							{
								fechaFinal = fechaFinal.AddDays(1);
								controlBucle += diferenciaTaller;
							}
							//Si solo se requieren una cantidad de horas menor que un dia en el taller
							else
							{
								fechaFinal = fechaFinal.Add(duracionOrden-controlBucle);
								controlBucle += (duracionOrden-controlBucle);
							}
						}
                        //Saltar Domingos
                        while (fechaFinal.DayOfWeek == DayOfWeek.Sunday )
                            fechaFinal = fechaFinal.AddHours(24);
                        //Saltar Festivos
                        while (fechaFinal.DayOfWeek != DayOfWeek.Sunday && DBFunctions.RecordExist("SELECT PDF_SECUENCIA FROM PDIAFESTIVO WHERE PDF_FECH='" + fechaFinal.ToString("yyyy-MM-dd") + "';"))
                            fechaFinal = fechaFinal.AddHours(24);
				
					}
					fechaFinal = fechaFinal.AddMinutes(60);
					if(TimeSpan.Compare(fechaFinal.TimeOfDay,horaFinalTaller.TimeOfDay)==1)
					{
						fechaFinal = fechaFinal.AddDays(1);
						fechaFinal = fechaFinal.Add(horaInicioTaller.TimeOfDay-fechaFinal.TimeOfDay);
						fechaFinal = fechaFinal.AddMinutes(60);
					}
					fechaFinal = UtilitarioPlanning.ValidarParametrosFecha(fechaFinal);
					fechaEstimada.Text = fechaFinal.ToString("yyyy-MM-dd");
					horaEstimada.Text = fechaFinal.ToString("HH:mm:ss");
				}
			}
			else
			{
				DateTime fechaSalida = UtilitarioPlanning.ValidarParametrosFecha(DateTime.Now);
				fechaEstimada.Text = fechaSalida.ToString("yyyy-MM-dd");
				horaEstimada.Text = fechaSalida.ToString("HH:mm:ss");
			}
		}
		
		protected bool Verificar_Precios_Bonos()
		{
			bool malFormato = false;
			for(int i=0;i<kitsOperaciones.Items.Count;i++)
			{
				if(((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Visible==true)
				{
					try{double doubleException = System.Convert.ToDouble(((TextBox)kitsOperaciones.Items[i].Cells[5].Controls[1]).Text);}
					catch{malFormato = true;}
				}
			}
			return malFormato;
		}
		
        protected bool verificar_Catalogo(string placa)
        {
            bool rta = false;
            if(DBFunctions.RecordExist("SELECT MCAT_PLACA FROM MCATALOGOVEHICULO WHERE MCAT_PLACA = '" + placa + "'"))
            {
                rta = true;
            }
            return rta;
        }

        protected double Agregar_Operaciones(string codigoItem, string cargoAsociado, string listaPreciosOperaciones, string listaPreciosItems)
		{
			double parcial = 0;
			double porcentajeIva = System.Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DISTINCT PTE.ptem_operacion, PTE.ptem_descripcion, CASE WHEN PTE.ptem_operacion NOT IN (SELECT ptie_tempario FROM ptiempotaller WHERE ptie_grupcata='"+this.valToInsertar1EX.Text+"') THEN PTE.ptem_tiempoestandar ELSE PTI.ptie_tiemclie END, PTE.ttip_codiliqu, PTE.ptem_exceiva, PTE.tope_codigo FROM ptempario PTE, ptiempotaller PTI, mitemoperacion MIO WHERE ((PTI.ptie_tempario = PTE.ptem_operacion AND PTI.ptie_grupcata='"+this.valToInsertar1EX.Text+"') OR PTE.ptem_operacion NOT IN (SELECT ptie_tempario FROM ptiempotaller WHERE ptie_grupcata='"+this.valToInsertar1EX.Text+"')) AND PTE.ptem_operacion = MIO.ptem_operacion AND MIO.mite_codigo='"+codigoItem+"' ORDER BY PTE.ptem_operacion");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] selection = this.operaciones.Select("CODIGOOPERACION='"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 0)
				{
					DataRow fila = operaciones.NewRow();
					bool errorValor = false;
					fila[0] = "";
					fila[1] = ds.Tables[0].Rows[i][0].ToString();
					fila[2] = ds.Tables[0].Rows[i][1].ToString();
					fila[3] = Convert.ToDouble(ds.Tables[0].Rows[i][2].ToString());
					if(ds.Tables[0].Rows[i][4].ToString() == "S")
						fila[5] = "SI";
					else if(ds.Tables[0].Rows[i][4].ToString() == "N")
						fila[5] = "NO";				
					//Rutina para calcular el valor de la operacion que se va a realizar
					double valorOperacion = 0;
					if(ds.Tables[0].Rows[i][3].ToString() =="F")
						valorOperacion = Convert.ToDouble(DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='"+ds.Tables[0].Rows[i][0].ToString()+"'"));
					else if(ds.Tables[0].Rows[i][3].ToString() == "T")
					{
						try{valorOperacion = (Convert.ToDouble(DBFunctions.SingleData("SELECT ppreta_valohoraclie FROM ppreciotaller WHERE ppreta_codigo='"+listaPreciosOperaciones+"'")))*(Convert.ToDouble(fila[3]));}
						catch
						{
                            Utils.MostrarAlerta(Response, "La operación : " + ds.Tables[0].Rows[i][1].ToString() + ", no tiene un valor especificado dentro la lista de precios seleccionada");
							errorValor = true;
						}
					}
					if(ds.Tables[0].Rows[i][3].ToString() != "B")
					{
						if(ds.Tables[0].Rows[i][4].ToString()=="N")
							valorOperacion = valorOperacion + (valorOperacion*(porcentajeIva/100));				
						fila[4] = valorOperacion;
					}				
					//Fin de la rutina que calcula el valor de la operacion a realizar
					fila[7] = cargoAsociado;
					string mecanicoResultado = Busqueda_Mecanico(ds.Tables[0].Rows[i][5].ToString());
					if(mecanicoResultado=="")
					{
						fila[6] = mecanicoResultado;
						fila[8] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaoper = 'S'");
					}
					else
					{
						fila[6] = mecanicoResultado;
						fila[8] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaoper = 'A'");
					}
					fila[9] = "0";
					fila[10] = "0";
					fila[11] = "0";
					fila[12] = "0";
					if(!errorValor)
					{
						operaciones.Rows.Add(fila);
						parcial += Agregar_Items(fila[1].ToString(),cargoAsociado,listaPreciosItems,listaPreciosOperaciones);
						parcial += valorOperacion;
					}
				}
			}
			BindDgOperaciones();
			return parcial;
		}
		
		protected double Remover_Operaciones(string codigoItem)
		{
			double valorRestar = 0;
			int i,j;
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT ptem_operacion FROM mitemoperacion WHERE mite_codigo='"+codigoItem+"'");
			//Ya tenemos los codigos de las operaciones relacionadas, solo se podran eliminar las que no tengan otros items relacionados
			for(i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow[] selection = this.operaciones.Select("CODIGOOPERACION = '"+ds.Tables[0].Rows[i][0].ToString()+"'");
				if(selection.Length == 1)
				{
					//Se puede eliminar porque aun existe la operacion
					bool eliminar = true;
					DataSet ds1 = new DataSet();
					DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT mite_codigo FROM mitemoperacion WHERE ptem_operacion='"+ds.Tables[0].Rows[i][0].ToString()+"' AND mite_codigo <> '"+codigoItem+"'");
					for(j=0;j<ds1.Tables[0].Rows.Count;j++)
					{
						DataRow[] selection1 = this.items.Select("CODIGOREPUESTO = '"+ds1.Tables[0].Rows[j][0].ToString()+"'");
						if(selection1.Length == 1)
								eliminar = false;
					}
					if(eliminar)
					{
						for(j=0;j<this.operaciones.Rows.Count;j++)
						{
							if(this.operaciones.Rows[j] == selection[0])
							{
								valorRestar += Convert.ToDouble(this.operaciones.Rows[j][4]);
								this.operaciones.Rows[j].Delete();
							}
						}
					}
				}
			}
			BindDgOperaciones();
			Verificar_Grilla_Operaciones();
			return valorRestar;
		}
		#endregion
		
		#region Otros Metodos
		protected bool Validacion_Grillas()
		{
			//Vamos a realizar la validacion que si existan operaciones e items para grabar
			//Debemos tener en cuenta si se puede grabar una orden con solo operaciones o solo items
			//Por el momento la verificacion se hace teniendo en cuenta que no se permite guardar una orden
			//Cuando no haya items o no haya operaciones Anotacion hecha 27-10-2004
			bool existen = true;
			int i;
			if(kitsOperaciones.Items.Count==0)
				existen = false;
			else if(kitsOperaciones.Items.Count>0)
			{
				int cantidadRemovidos = 0;
				for(i=0;i<kitsOperaciones.Items.Count;i++)
                        if (((Button)kitsOperaciones.Items[i].Cells[14].Controls[1]).Text == "Restaurar")
                            cantidadRemovidos += 1;
					
				if(cantidadRemovidos==kitsOperaciones.Items.Count)
					existen = false;
			}
			return existen;			
		}
		
		protected void Ocultar_Control()
		{
			Control principal = (this.Parent).Parent; //Control principal ascx
            //HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
            //hdTabIndex.Value = "4";
			//((ImageButton)principal.FindControl("botonKits")).Enabled = false;
			//((Button)principal.FindControl("grabar")).Enabled = true;
		}

        protected void control_Vehiculo()
        {
            Control principal = (this.Parent).Parent; //Control principal ascx
            HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
            hdTabIndex.Value = "2";
        }


        protected void Mostrar_Control_Peritaje()
		{
			Control principal = (this.Parent).Parent; //Control principal ascx
			//((ImageButton)principal.FindControl("botonKits")).Enabled = false;
			//((Button)principal.FindControl("grabar")).Enabled = false;
			//((PlaceHolder)principal.FindControl("operacionesPeritaje")).Visible = true;
			//((ImageButton)principal.FindControl("opPeritaje")).ImageUrl="../img/AMS.BotonContraer.png";
			//((ImageButton)principal.FindControl("opPeritaje")).Enabled = false;
		}
		
		protected void Verificar_Grilla_Kits()
		{
			//A continuacion debemos apagar los botones de los que ya fueron escogidos
			for(int i=0;i<escogidos.Count;i++)
				((Button)kitsCompletos.Items[System.Convert.ToInt32(escogidos[i])].Cells[2].Controls[1]).Enabled = false;
		}
		
		protected bool RevisionCargo(string cargoPrincipalOT, string cargoEscogido)
		{
			bool cargoValido = true;
			if(cargoPrincipalOT == "S")
				cargoValido = true;
			else if(cargoPrincipalOT == "C")
			{
				if(cargoEscogido == "S")
					cargoValido = false;
			}
			else if(cargoPrincipalOT == "G")
			{
				if(cargoEscogido == "S" || cargoEscogido == "C")
					cargoValido = false;
			}
			else
			{
				if(cargoEscogido == "S" || cargoEscogido == "C" || cargoEscogido == "G")
					cargoValido = false;
			}
			return cargoValido;
		}

		public void InicializarGrillas()
		{
			items = new DataTable();
			if(operaciones==null)operaciones = new DataTable();
			kitsItems.DataSource = items;
			kitsOperaciones.DataSource = operaciones;
			kitsItems.DataBind();
			kitsOperaciones.DataBind();
		}

		private bool VerificarFechaHoraEntrega()
		{
			//Si lo digitado no es una fecha
			if(!DatasToControls.ValidarDateTime(fechaEstimada.Text))
				return false;
			//Si lo digitado no es una hora
			else if(!DatasToControls.ValidarDateTime(horaEstimada.Text))
				return false;
			//Si la fecha de entrada digitada es menor a la fecha de hoy
			//else if(Convert.ToDateTime(fechaEstimada.Text)<DateTime.Now)
			//	return false;
			//Si la combinación fecha-hora estimada es menor a la del momento
			else if(Convert.ToDateTime(fechaEstimada.Text+" "+horaEstimada.Text)<DateTime.Now)
				return false;
			else
				return true;
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
			this.kitsCompletos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.kitsCompletos_ItemDataBound);

		}
		#endregion
	}
}
