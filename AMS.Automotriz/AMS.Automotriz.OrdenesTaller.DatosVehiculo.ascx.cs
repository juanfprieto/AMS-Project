// created on 23/09/2004 at 15:13
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	public partial class DatosVehiculo : System.Web.UI.UserControl
	{
		#region Atributos
        DataTable dtDocumentos;
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//botones q hace la confirmacion de los datos y cancela el boton para q no se produzca. doble facturacion o etc
			//confirmar.Attributes.Add("Onclick","document.getElementById('"+confirmar.ClientID+"').disabled = true;"+this.Page.GetPostBackEventReference(confirmar)+";");
			//confirmar.Attributes.Add("onClick","confirm('Esta seguro que desea Continuar?');");
            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(modelo, "SELECT pcat_codigo,pcat_descripcion CONCAT ' -  Catálogo: ' CONCAT pcat_codigo CONCAT '  -  Vin ' CONCAT pcat_vinbasico FROM pcatalogovehiculo ORDER BY 2");
            //    modelo.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(color, "SELECT pcol_codigo, pcol_descripcion FROM pcolor ORDER BY pcol_descripcion");
                color.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(tipo, "SELECT tser_tiposerv, tser_nombserv FROM tserviciovehiculo order by tser_nombserv");
                tipo.Items.Insert(0, "Seleccione..");
                //Cargamos el vin basico del catalogo especifico
                identificacion.Text = DBFunctions.SingleData("SELECT pcat_vinbasico FROM pcatalogovehiculo WHERE pcat_codigo='" + modelo.SelectedValue + "'");
                PrepararTablaDocumentos();
                Bind_Documentos();
            }
            else
            {
                dtDocumentos = (DataTable)ViewState["DT_DOCUMENTOS"];
            }
		}
        private void PrepararTablaDocumentos()
        {
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add(new DataColumn("PDOC_CODIGO", System.Type.GetType("System.String")));//0
            dtDocumentos.Columns.Add(new DataColumn("PDOC_NOMBRE", System.Type.GetType("System.String")));//1
            dtDocumentos.Columns.Add(new DataColumn("MVEH_NUMEDOCU", System.Type.GetType("System.String")));//2
            dtDocumentos.Columns.Add(new DataColumn("MVEH_FECHENTREGA", System.Type.GetType("System.DateTime")));//3
            dtDocumentos.Columns.Add(new DataColumn("MVEH_FECHINGRESO", System.Type.GetType("System.DateTime")));//4
            ViewState["DT_DOCUMENTOS"] = dtDocumentos;
        }
        public void dgEvento_Grilla(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "AgregarDocumento")
            {
                if (((((TextBox)e.Item.Cells[0].Controls[1]).Text) == "") || ((((TextBox)e.Item.Cells[1].Controls[1]).Text) == "") || (!DatasToControls.ValidarDateTime(((TextBox)e.Item.Cells[2].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "Debe ingresar el documento y la fecha de vencimiento");
                else
                {
                    string docN=((TextBox)e.Item.Cells[0].Controls[1]).Text;
                    DataRow fila = dtDocumentos.NewRow();
                    fila[0] = docN;
                    fila[1] = DBFunctions.SingleData("SELECT PDOC_NOMBRE FROM PDOCUMENTOVEHICULO WHERE PDOC_CODIGO='"+docN+"';");
                    fila[2] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                    fila[3] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
                    fila[4] = ((TextBox)e.Item.Cells[3].Controls[1]).Text;
                    dtDocumentos.Rows.Add(fila);
                    Bind_Documentos();
                }
            }
            else if (((Button)e.CommandSource).CommandName == "QuitarDocumento")
            {
                    dtDocumentos.Rows[e.Item.ItemIndex].Delete();
                    Bind_Documentos();
            }
        }
        protected void Bind_Documentos()
        {
            ViewState["DT_DOCUMENTOS"] = dtDocumentos;
            dgrDocumentos.DataSource = dtDocumentos;
            dgrDocumentos.DataBind();
        }
		protected void CambioCatalogo(Object  Sender, EventArgs e)
		{
			Session["kitsTabla"] = null;
			identificacion.Text = DBFunctions.SingleData("SELECT pcat_vinbasico FROM pcatalogovehiculo WHERE pcat_codigo='"+modelo.SelectedValue+"'");
		}
		
		public void Confirmar(Object  Sender, EventArgs e)
		{
            Control principal = (this.Parent).Parent; //Control principal ascx
            if (fechaCompra.Text.ToString() == "" || fechaCompra.Text.ToString() == null)
            {
                Utils.MostrarAlerta(Response, "fecha de venta inválida.") ;
                return;
            }
       
            if (modelo.SelectedValue == "Seleccione.." || color.SelectedValue == "Seleccione.." || tipo.SelectedValue == "Seleccione.." || consVendedor.ToString() == "" || kilometrajeCompra.ToString() == "")
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar una opción en el modelo, color, tipo, concesionario vddr, fecha de compra o Kilometraje de compra.");
                return;
            }

			// Debemos cargar el control que tiene la placa del vehiculo
			// Response.Write("<script language:javascript> if(!confirm('¿Seguro que ha leido las condiciones de la orden de servicio?'))this.form.submit(); </script>"); 
			
			
			Control controlDatosOrden = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
			Control controlDatosPropietario = ((PlaceHolder)principal.FindControl("datosPropietario")).Controls[0];
			Control controlKitsCombos = ((PlaceHolder)principal.FindControl("kitsCombos")).Controls[0];
            Control controlOtrosDatos = null;
            if ((PlaceHolder)principal.FindControl("otrosDatos") != null)
                controlOtrosDatos = ((PlaceHolder)principal.FindControl("otrosDatos")).Controls[0];

			string placa = ((TextBox)controlDatosOrden.FindControl("placa")).Text;
			string cargo = ((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedValue;
			string nit = ((TextBox)controlDatosPropietario.FindControl("datos")).Text.Trim();
			bool cargoError = false;
			//Realizamos las comprobaciones de los cargos respectivos
			if(cargo == "S")
			{
				if(nitAseguradora.Text == "") 
					cargoError = true;
				if(nitAseguradora.Text.Trim() == nit.Trim())
					cargoError = true;
                if(porcentajeDeducible.Text == "")
                    porcentajeDeducible.Text = "0";
                if(valorMinDeducible.Text == "")
                    valorMinDeducible.Text = "0";
				double porcIVA = -1;
				try{porcIVA = Convert.ToDouble(porcentajeDeducible.Text);}
				catch{cargoError = true;}
				if(porcIVA < 0 || porcIVA > 100)
					cargoError = true;
			}
			else if(cargo == "G")
			{
				if(nitCompania.Text == "" ) //|| numeroAutorizacionGarant.Text == "")
					cargoError = true;
			}
			if(DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'"))
			{
				if(!cargoError)
				{
					if(Actualizar_Datos(placa))
                        Utils.MostrarAlerta(Response, "Kilometraje Invalido");
					else
					{
						Ocultar_Control();
					}
				}
				else
                    Utils.MostrarAlerta(Response, "Existe un problema con la información relacionada con el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + cargo + "'").Trim() + ".\\nRevise por favor que todos los valores se hayan ingresado, que los valores sean validos y \\nque el nit de la aseguradora no sea el mismo nit de la orden de trabajo");
			}
			else
			{
				if(!cargoError)
				{
					string resultadoNuevoAuto = Crear_Nuevo_Auto(placa,nit);
					if(resultadoNuevoAuto == "")
					{
						((TextBox)controlKitsCombos.FindControl("valToInsertar1EX")).Text = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='"+modelo.SelectedValue+"'");
						//((TextBox)controlKitsCombos.FindControl("valToInsertar2EX")).Text = modelo.SelectedValue;
						Session["grupoCatalogo"] = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='"+modelo.SelectedValue+"'");
						Ocultar_Control();
					}
					else
                    {
                        Utils.MostrarAlerta(Response, "" + resultadoNuevoAuto + "");
                        control_vehiculo();
                    }
                        
				}
				else
                    Utils.MostrarAlerta(Response, "Existe un problema con la información relacionada con el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + cargo + "'").Trim() + ".\\nRevise por favor que todos los valores se hayan ingresado y que los valores sean validos");
			}
            //Garantia y Plan Post Venta
            DatasToControls bind = new DatasToControls();
            DropDownList ddlG;
            if (controlOtrosDatos != null)
            {
                ddlG = ((DropDownList)controlOtrosDatos.FindControl("ddltp"));
                bind.PutDatasIntoDropDownList(ddlG,
                    @"SELECT pt.ptem_operacion,pt.ptem_descripcion, mpla_numerepocasamatr  
                    FROM  MPLANGARANTIA mp, PTEMPARIO pt  
                    WHERE pt.ptem_operacion=mp.ptem_operacion AND  
                    mp.MCAT_VIN='" + identificacion.Text + @"' AND  
                    (mp.PDOC_CODIGO IS NULL OR mp.PDOC_CODIGO = '') AND (mp.MORD_NUMEORDE IS NULL OR mp.MORD_NUMEORDE = '');");
                if (ddlG.Items.Count > 0)
                {
                    string campania = "";
                    for (int i = 0; i < ddlG.Items.Count; i++)    // la idea es meter en la variable campania
                        campania += ddlG.SelectedItem.ToString() + " - ";
                    campania = campania.Substring(0, campania.Length - 3);
                    Utils.MostrarAlerta(Response, "Este vehículo tiene CAMPAÑA: " + campania + ", la cual será incluida en las operaciones con cargo Garantía de Fábrica");
                    //aquí agregar código de adición?
                    //insertar ptem_operacion en la grilla de operacion la cua lestá en ptempario
                    Session["garantia"] = "1";
                }
                ((TextBox)controlOtrosDatos.FindControl("tpp")).Text = DBFunctions.SingleData(
                    @"SELECT PP.PPLAN_CODIGO  
                    FROM  PPLANPOSTVENTA PP, MPLANPOSTVENTA MP  
                    WHERE MP.PPLAN_CODIGO=PP.PPLAN_CODIGO AND  
                    MP.MCAT_VIN='" + identificacion.Text + "';");
                ((TextBox)controlOtrosDatos.FindControl("tppVal")).Text = DBFunctions.SingleData(
                    @"SELECT PP.PPLAN_DESCRIPCION  
                    FROM  PPLANPOSTVENTA PP, MPLANPOSTVENTA MP  
                    WHERE MP.PPLAN_CODIGO=PP.PPLAN_CODIGO AND  
                    MP.MCAT_VIN='" + identificacion.Text + "';");
            }

		}

   		#endregion
		
		#region Metodos
		protected bool Actualizar_Datos(string placa)
		{
            DataSet datosVehic = new DataSet();
            DBFunctions.Request(datosVehic, IncludeSchema.NO, "SELECT mcat_numeultikilo, mcat_fechultikilo, mcat_numeradio FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "' ");

            string update = "UPDATE mcatalogovehiculo SET ";
			int primero = 1;
			bool error = false;
			if(datosVehic.Tables.Count > 0 && kilometraje.Text!= datosVehic.Tables[0].Rows[0]["MCAT_NUMEULTIKILO"].ToString() )  // (DBFunctions.SingleData("SELECT mcat_numeultikilo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'")))
			{
                double ultimoKilometraje = Convert.ToDouble(datosVehic.Tables[0].Rows[0]["MCAT_NUMEULTIKILO"].ToString());  //DBFunctions.SingleData("SELECT mcat_numeultikilo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'"));

                if (ultimoKilometraje>Convert.ToDouble(kilometraje.Text.Trim()))
					error=true;
				else
				{
					if(primero==1)
					{
                        update += " mcat_numeultikilo=" + Convert.ToDouble(kilometraje.Text.Trim()).ToString() + ", mcat_fechultikilo='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
						primero +=1;
					}
					else
                        update += ", mcat_numeultikilo=" + Convert.ToDouble(kilometraje.Text.Trim()).ToString() + ", mcat_fechultikilo='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
				}			
			}
			if(datosVehic.Tables.Count > 0 && codRadio.Text!= datosVehic.Tables[0].Rows[0]["MCAT_NUMERADIO"].ToString())  //(DBFunctions.SingleData("SELECT mcat_numeradio FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'")))
			{
				if(primero==1)
				{
					update += " mcat_numeradio='"+codRadio.Text.Trim()+"'";
					primero +=1;
				}
				else
					update += ", mcat_numeradio='"+codRadio.Text.Trim()+"'";				
			}

            ArrayList updateArray = new ArrayList();

			if((primero>1)&&(!error))
			{
				update += "	WHERE mcat_placa='"+placa+"'";
				updateArray.Add(update);
            }
            if (!error){
                //Documentos
                // updateArray.Add("DELETE FROM MVEHICULODOCUMENTO WHERE MCAT_VIN='" + identificacion.Text + "' ");
                for (int n = 0; n < dtDocumentos.Rows.Count; n++)
                {
                    updateArray.Add(
                    @"DELETE FROM MVEHICULODOCUMENTO  
                     WHERE MCAT_VIN='" + identificacion.Text + "' and pdoc_codidocu = '" + dtDocumentos.Rows[n]["PDOC_CODIGO"] + "' ");
                    updateArray.Add(
                        @"INSERT INTO MVEHICULODOCUMENTO " +
                        "VALUES(DEFAULT, '" + identificacion.Text + "', " +
                        "'" + dtDocumentos.Rows[n]["PDOC_CODIGO"] + "', " +
                        "'" + dtDocumentos.Rows[n]["MVEH_NUMEDOCU"] + "'," +
                        "'" + Convert.ToDateTime(dtDocumentos.Rows[n]["MVEH_FECHINGRESO"]).ToString("yyyy-MM-dd") + "',0," +
                        "'" + Convert.ToDateTime(dtDocumentos.Rows[n]["MVEH_FECHENTREGA"]).ToString("yyyy-MM-dd") + "'," +
                        "NULL,NULL,NULL,NULL" +
                        ");"
                    );
                }
            }
            if(updateArray.Count>0){
				if(DBFunctions.Transaction(updateArray))
					lb.Text += "";
				else
					lb.Text += DBFunctions.exceptions;
			}
			return error;
		}
		
		protected string Crear_Nuevo_Auto(string placa, string nit)
		{
			string insert;
			string error = "";
			//if(VerificarVinBasico())
			//{
			//Verifico q ese catalogo y ese vin no existan en mcatalogovehiculo, mejor
			//dicho q ese vehiculo no exista
			if(!DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_vin='"+this.identificacion.Text+"'"))
			{
                string tipoVehiculo = "V";
                try
                {
                    tipoVehiculo = DBFunctions.SingleData("select TTIP_CODIGO FROM PCATALOGOVEHICULO WHERE PCAT_CODIGO = '"+ modelo.SelectedValue + "' ");
                }
                catch
                {
                    tipoVehiculo = "V";  // V = Vehículo Automotor, M = Maquinaria, C = Componente o elemento no valida VIN de 17 caracteres 
                };
                if (Convert.ToInt16(anoModelo.Text) >= 2000 && identificacion.Text.Length != 17 && tipoVehiculo != "C") 
                {
                    Utils.MostrarAlerta(Response, "Debe ingresar el VIN del vehículo correcto a 17 caracteres.");
                }
                else
                {
                    //Si el vehiculo no existe, aun verifico q no exista con la placa proporcionada
                    if (!DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "'"))
                    {
                        //Verifico q ese motor no exista en mcatalogovehiculo
                        if (!DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_motor='" + this.motor.Text + "'"))
                        {
                            //Validar VIN
                            /*if(!AMS.Tools.VINs.ValidarVIN(identificacion.Text.Trim()))
							    return("El VIN no es correcto");*///cambio realizado 1 febrero 2010
                            insert = "INSERT INTO mcatalogovehiculo VALUES('" + modelo.SelectedValue + "','" +
                                                    identificacion.Text + "','" +
                                                    placa + "','" +
                                                    motor.Text + "','" +
                                                    nit + "','" +
                                                    serie.Text +
                                                    "',null,'" +
                                                    color.SelectedValue + "'," +
                                                    anoModelo.Text + ",'" +
                                                    tipo.SelectedValue +
                                                    "',NULL,'" +
                                                    ((consVendedor.Text.Length > 20) ? consVendedor.Text.Substring(0, 19) : consVendedor.Text) + "','" +
                                                    fechaCompra.Text + "'," +
                                                    Convert.ToDouble(kilometrajeCompra.Text.Trim()) + ",'" +
                                                    codRadio.Text + "'," +
                                                    Convert.ToDouble(kilometraje.Text.Trim()) +
                                                    ",0,'C',null,'" + DateTime.Now.ToString("yyyy-MM-dd") + "',null)";
                            ArrayList sql = new ArrayList();
                            sql.Add(insert);
                            if (!DBFunctions.Transaction(sql))
                                error = DBFunctions.exceptions;
                        }
                        else
                            error = "Ya se ha ingresado un vehículo con el número de motor especificado.\\nPlaca: " + DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_motor='" + this.motor.Text + "'") + "\\nMotor : " + DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE mcat_motor='" + this.motor.Text + "'") + " \\nCatálogo: " + DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_motor='" + this.motor.Text + "'") + " \\nVIN : " + DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_motor='" + this.motor.Text + "'") + ". \\nNo se puede guardar la informacion sobre este vehiculo";
                    }
                    else
                        error = "Ya se ha ingresado un vehiculo con esta placa. \\nPlaca: " + DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "'") + "\\nMotor : " + DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "'") + " \\nCatálogo: " + DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='" + placa + "'") + " \\nVIN : " + DBFunctions.SingleData("SELECT mcat_vin FROM  mcatalogovehiculo WHERE mcat_placa='" + placa + "'") + ". \\nNo se puede guardar la informacion sobre este vehículo";
                }
			}
			else
				error = "Este vehículo ya existe con la siguiente información \\nPlaca: "+DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE pcat_codigo='"+this.modelo.SelectedValue+"' AND mcat_vin='"+this.identificacion.Text+"'")+"\\nMotor : "+DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE pcat_codigo='"+this.modelo.SelectedValue+"' AND mcat_vin='"+this.identificacion.Text+"'")+" \\nCatálogo: "+this.modelo.SelectedValue+" \\nVIN : "+this.identificacion.Text+". \\nNo se puede guardar la informacion sobre este vehiculo";
			//}
			//else
			//	error = "Se ha modificado el VIN basico del vehiculo o no se ha terminado de completar. No se puede guardar la informacion sobre este vehiculo";
			return error;
		}
		
		protected bool VerificarVinBasico()
		{
			bool validarVin = false;
			string vinBasico = DBFunctions.SingleData("SELECT pcat_vinbasico FROM pcatalogovehiculo WHERE pcat_codigo='"+modelo.SelectedValue+"'");
			if(identificacion.Text.Length>vinBasico.Length)
				if(identificacion.Text.Substring(0,vinBasico.Length)==vinBasico)
					validarVin = true;

            if(identificacion.Text.Length != 17)
            {
                Utils.MostrarAlerta(Response, "El VIN ingresado debe tener 17 caracteres! Por favor revisar.");
            }

            return validarVin;
		}
		
		protected void Ocultar_Control()
		{
			Control principal = (Parent).Parent; //Control principal ascx
            HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
            hdTabIndex.Value = "3";

            if ((ImageButton)principal.FindControl("otros") != null) // normal
            {
                ((PlaceHolder)principal.FindControl("otrosDatos")).Visible = true;
                //((ImageButton)principal.FindControl("otros")).ImageUrl = "../img/AMS.BotonContraer.png";
            }
            else // cotizaciones
            {
                //((PlaceHolder)principal.FindControl("kitsCombos")).Visible = true;
                //((ImageButton)principal.FindControl("botonKits")).Enabled = true;
                //((ImageButton)principal.FindControl("botonKits")).ImageUrl = "../img/AMS.BotonContraer.png";

            }
			//((PlaceHolder)Parent).Visible = false;
			//((ImageButton)principal.FindControl("vehiculo")).ImageUrl="../img/AMS.BotonExpandir.png";
		}

        protected void control_vehiculo()
        {
            Control principal = (Parent).Parent; //Control principal ascx
            HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
            hdTabIndex.Value = "2";
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

		}
		#endregion

	}
}
