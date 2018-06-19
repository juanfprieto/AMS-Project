// created on 14/03/2005 at 17:28
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Contabilidad;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class EntregaVehiculosProceso : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        DatasToControls bind = new DatasToControls();
        ProceHecEco contaOnline = new ProceHecEco();
        protected void Page_Load(object sender, System.EventArgs e)
		{           
            
            if (!IsPostBack)
			{

                if (Request.QueryString["programarEntrega"] != null)
                {                    
                    plhEntrega.Visible = false;
                    fldProgramacionEntrega.Visible = true;
                    bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT PALM_ALMACEN, PALM_ALMACEN || ' - ' || PALM_DESCRIPCION FROM PALMACEN WHERE TVIG_VIGENCIA='V';");
                    bind.PutDatasIntoDropDownList(ddlCiudadMatricula, "SELECT P.PCIU_CODIGO, P.PCIU_NOMBRE FROM PCIUDAD P;");
                    ddlAlmacen.Items.Insert(0, new ListItem("Seleccione...", "0"));
                    string invVehiculo = Request.QueryString["vehiInv"];
                    ViewState["invVehiculo"] = invVehiculo;
                    ViewState["vinVehiculo"] = DBFunctions.SingleData("SELECT MCAT_VIN FROM MVEHICULO WHERE MVEH_INVENTARIO = " + invVehiculo + "");
                    string[] datos = DBFunctions.SingleData("SELECT MCAT_PLACA || '~' || coalesce(MCAT_MATRICULA, '') || '~' || MCAT_VENTA FROM MCATALOGOVEHICULO WHERE MCAT_VIN = '" + ViewState["vinVehiculo"] + "'").Split('~');
                    lbVehiculo.Text = "<b>" + ViewState["vinVehiculo"] + "</b>";
                    lbPrefijo.Text = "<b> DEV </b>";
                    //lbNumero.Text = "<b> 1 </b>";
                    txtPlaca.Text = datos[0];
                    txtInventario.Text = invVehiculo;
                    txtMatricula.Text = datos[1]; // == ""? "No aplica" : datos[1];
                    txtFechaMatricula.Text = datos[2];
                }
                else
                {                    
                    DatasToControls bind = new DatasToControls();
                    bind.PutDatasIntoDropDownList(ddlAlmacendefini, "SELECT PALM_ALMACEN, PALM_ALMACEN || ' - ' || PALM_DESCRIPCION FROM PALMACEN WHERE TVIG_VIGENCIA='V';");
                    bind.PutDatasIntoDropDownList(ddlCiudadMatriculadefini, "SELECT P.PCIU_CODIGO, P.PCIU_NOMBRE FROM PCIUDAD P;");
                    ddlAlmacendefini.Items.Insert(0, new ListItem("Seleccione...", "0"));
                    bind.PutDatasIntoDropDownList(prefijoEntrega, string.Format(Documento.DOCUMENTOSTIPO, "EN"));
                    //if (prefijoEntrega.Items.Count > 1)
                    //    prefijoEntrega.Items.Insert(0, "Seleccione:..");
                    string invVehiculo = Request.QueryString["numeInv"];
                    ViewState["vinVehiculo"] = DBFunctions.SingleData("SELECT MCAT_VIN FROM MVEHICULO WHERE MVEH_INVENTARIO = " + invVehiculo + "");
                    vehiculo.Text = DBFunctions.SingleData("SELECT pcat_codigo CONCAT ' - ' CONCAT MV.mcat_vin FROM mvehiculo MV, Mcatalogovehiculo mc WHERE mveh_inventario=" + Request.QueryString["numeInv"] + " AND MC.MCAT_VIN = MV.MCat_VIN");
                    txtPlacaE.Text = DBFunctions.SingleData("SELECT mcat_placa FROM mvehiculo MV, Mcatalogovehiculo mc WHERE mveh_inventario=" + Request.QueryString["numeInv"] + " AND MC.MCAT_VIN = MV.MCat_VIN");
                    txtMatriculaE.Text = DBFunctions.SingleData("SELECT mcat_MATRICULA FROM mvehiculo MV, Mcatalogovehiculo mc WHERE mveh_inventario=" + Request.QueryString["numeInv"] + " AND MC.MCAT_VIN = MV.MCat_VIN");
                    txtFechaMatriculaE.Text = Convert.ToDateTime(DBFunctions.SingleData("SELECT CAST(mcat_VENTA AS CHAR(10)) FROM mvehiculo MV, Mcatalogovehiculo mc WHERE mveh_inventario=" + Request.QueryString["numeInv"] + " AND MC.MCAT_VIN = MV.MCat_VIN")).Date.ToString("yyyy-MM-dd");
                    txtFechaMatriculaE.Text = (DBFunctions.SingleData("SELECT CAST(mcat_VENTA AS CHAR(10)) FROM mvehiculo MV, Mcatalogovehiculo mc WHERE mveh_inventario=" + Request.QueryString["numeInv"] + " AND MC.MCAT_VIN = MV.MCat_VIN"));
                    fechaEntrega.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    if (DBFunctions.RecordExist("SELECT MVEH_INVENTARIO FROM MVEHICULOENTREGA WHERE MVEH_INVENTARIO = " + Request.QueryString["numeInv"] + " "))
                    {
                        DataSet entregavehiculo = new DataSet();
                        DBFunctions.Request(entregavehiculo,IncludeSchema.NO, "SELECT HOUR(MVE.MVEE_HORAENTREGA) AS HORA,MINUTE(MVE.MVEE_HORAENTREGA) AS MINUTO,MVE.MVEE_NITENTREGA,MVE.MVEE_NITRECIBE,MVE.MVEE_NOMBNITRECIBE,MVE.PALM_ALMACEN,MVE.MVEE_CIUDADMATRICULA, VMN.NOMBRE FROM MVEHICULOENTREGA MVE, VMNIT VMN WHERE MVE.MVEE_NITENTREGA=VMN.MNIT_NIT AND MVEH_INVENTARIO =" + Request.QueryString["numeInv"] + " ");
                        txtNitEntregadefini.Text = entregavehiculo.Tables[0].Rows[0][2].ToString();
                        txtNitEntregadefinia.Text = entregavehiculo.Tables[0].Rows[0][7].ToString();
                        txtNitRecibedefini.Text = entregavehiculo.Tables[0].Rows[0][3].ToString();
                        txtNombreRecibedefini.Text = entregavehiculo.Tables[0].Rows[0][4].ToString();
                        txthoraendefinitiva.Text = entregavehiculo.Tables[0].Rows[0][0].ToString();
                        txtmindefinitiva.Text = entregavehiculo.Tables[0].Rows[0][1].ToString();
                        ddlAlmacendefini.SelectedValue = entregavehiculo.Tables[0].Rows[0][5].ToString();
                        ddlCiudadMatriculadefini.SelectedValue = entregavehiculo.Tables[0].Rows[0][6].ToString();
                    }
                    else
                    { }
                 }

			}
		}
		
		protected void Efectuar_Entrega(Object  Sender, EventArgs e)
		{
                if (DatasToControls.ValidarDateTime(fechaEntrega.Text) && validarPlaca(txtPlacaE.Text, txtInventario.ToString()))
                {
                    string originales = "";
                    ArrayList sqlStrings = new ArrayList();
                    DataSet dtOriginales = new DataSet();
                    DBFunctions.Request(dtOriginales, IncludeSchema.NO, "select * from dbxschema.mvehiculo WHERE mveh_inventario=" + Request.QueryString["numeInv"] + ";");
                    for (int i = 0; i < dtOriginales.Tables[0].Columns.Count; i++)
                    {
                        originales += dtOriginales.Tables[0].Rows[0][i].ToString() + ",";
                    }

                    string prefijo = prefijoEntrega.SelectedValue;
                    int num = Convert.ToInt16(DBFunctions.SingleData("SELECT PDOC_ULTIDOCU+1 FROM DBXSCHEMA.PDOCUMENTO WHERE PDOC_CODIGO = '" + prefijo + "'; "));
                    originales = originales.Substring(0, originales.Length - 2);
                    sqlStrings.Add("UPDATE DBXSCHEMA.PDOCUMENTO SET PDOC_ULTIDOCU = " + num + " WHERE PDOC_CODIGO = '" + prefijo + "'");
                    sqlStrings.Add("INSERT INTO MHISTORIAL_CAMBIOS " +
                                "(MHST_COD, TABLA, OPERACION, ORIGINALES, SUSU_USUARIO, FECHA) " +
                                "VALUES( DEFAULT, 'MVEHICULO', 'U', '" + originales + "', '" + HttpContext.Current.User.Identity.Name.ToLower() + "', '" + DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                    sqlStrings.Add("UPDATE DBXSCHEMA.MCATALOGOVEHICULO SET MCAT_PLACA = '" + txtPlacaE.Text + "', MCAT_VENTA = '" + txtFechaMatriculaE.Text + "', MCAT_MATRICULA = '" + txtMatriculaE.Text + "' WHERE MCAT_VIN = '" + ViewState["vinVehiculo"] + "'");

                    //  Validacion de la hora de entrega
                    string horaentrega = txthoraendefinitiva.Text == "" ? "12" : txthoraendefinitiva.Text;
                    horaentrega += ":";
                    if (txtmindefinitiva.Text.Length < 2)
                        horaentrega += txtmindefinitiva.Text == "" ? "00" : "0" + txtmindefinitiva.Text;
                    else
                        horaentrega += txtmindefinitiva.Text;
                    //  Se valida si ya existe el registro en la programacion de entregas entonces se actualiza, de no existir si inserte
                    if (!DBFunctions.RecordExist("SELECT MVEH_INVENTARIO FROM MVEHICULOENTREGA WHERE MVEH_INVENTARIO = " + Request.QueryString["numeInv"] + " "))
                        sqlStrings.Add(@"INSERT INTO MVEHICULOENTREGA (MVEH_INVENTARIO, PDOC_CODIGO, MVEE_NUMERO, MVEE_FECHAENTREGA,MVEE_HORAENTREGA,MVEE_NITENTREGA,MVEE_NITRECIBE,MVEE_NOMBNITRECIBE,PALM_ALMACEN,MVEE_CIUDADMATRICULA,MVEE_USUARIO) 
                                     VALUES (" + Request.QueryString["numeInv"] + ",'" + prefijo + "'," + num.ToString() + ",'" + fechaEntrega.Text + "','" + horaentrega + "','" + txtNitEntregadefini.Text + "','" + txtNitRecibedefini.Text + "','" + txtNombreRecibedefini.Text + "','" + ddlAlmacendefini.SelectedValue + "','" + ddlCiudadMatriculadefini.SelectedValue + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "')");
                    else
                        sqlStrings.Add(@"UPDATE MVEHICULOENTREGA SET PDOC_CODIGO= '" + prefijo + "', MVEE_NUMERO= '" + num.ToString() + "', MVEE_FECHAENTREGA = '" + fechaEntrega.Text + "', MVEE_HORAENTREGA = '" + horaentrega + "', MVEE_NITENTREGA = '" + txtNitEntregadefini.Text + "', MVEE_NITRECIBE = '" + txtNitRecibedefini.Text + "', MVEE_NOMBNITRECIBE = '" + txtNombreRecibedefini.Text + "', PALM_ALMACEN = '" + ddlAlmacendefini.SelectedValue + "', MVEE_CIUDADMATRICULA = '" + ddlCiudadMatriculadefini.SelectedValue + "', MVEE_USUARIO = '" + HttpContext.Current.User.Identity.Name.ToLower() + "' " +
                            " WHERE MVEH_INVENTARIO = " + Request.QueryString["numeInv"] + " ");



                    if (DBFunctions.Transaction(sqlStrings))
                    {
                        contaOnline.contabilizarOnline(prefijo.ToString(), Convert.ToInt32(num.ToString()), Convert.ToDateTime(fechaEntrega.Text), "");
                        DBFunctions.NonQuery("UPDATE mvehiculo SET test_tipoesta=60, mveh_fechentr='" + Convert.ToDateTime(fechaEntrega.Text).ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE mveh_inventario=" + Request.QueryString["numeInv"] + "");
                        Response.Redirect("" + indexPage + "?process=Vehiculos.EntregaVehiculos&prefDev=" + prefijo + "&numDev=" + num + "&alerta=ok ");
                        Utils.MostrarAlerta(Response, "Registro Creado satisfactoriamente");
                }
                    else
                    {
                        Utils.MostrarAlerta(Response, "Error al actualizar. Por favor revisar que los sdatos sean correctos.");
                    }
                }
                else
                    Utils.MostrarAlerta(Response, "Fecha Invalida");
        }

        protected void cargarCiudades(object sender, EventArgs z)
        {
            bind.PutDatasIntoDropDownList(ddlCiudadMatricula, "SELECT P.PCIU_CODIGO, P.PCIU_NOMBRE FROM PCIUDAD P;");
            bind.PutDatasIntoDropDownList(ddlCiudadMatriculadefini, "SELECT P.PCIU_CODIGO, P.PCIU_NOMBRE FROM PCIUDAD P;");
            
        }
        protected void programarEntrega(object sender, EventArgs z)
        {
            if(validarCampos())
            {
                 ArrayList sqlStrings = new ArrayList();

                //unas cuantas validaciones
                string hora = txtHoraEntrega.Text == ""? "12": txtHoraEntrega.Text;
                hora += ":";
                if(txtMinEntrega.Text.Length < 2 )
                    hora +=  txtMinEntrega.Text == ""?"00": "0" + txtMinEntrega.Text;
                else
                    hora += txtMinEntrega.Text;

                //modificar placa, # matricula, fecha matricula
                sqlStrings.Add("UPDATE DBXSCHEMA.MCATALOGOVEHICULO SET MCAT_PLACA = '" + txtPlaca.Text + "', MCAT_VENTA = '" + txtFechaMatricula.Text + "', MCAT_MATRICULA = '" + txtMatricula.Text + "' WHERE MCAT_VIN = '" + ViewState["vinVehiculo"] + "'");
                //grabar en mvehiculoentrega
                if (!DBFunctions.RecordExist("SELECT MVEH_INVENTARIO FROM MVEHICULOENTREGA WHERE MVEH_INVENTARIO = " + ViewState["invVehiculo"].ToString() + " "))
                    sqlStrings.Add(@"INSERT INTO MVEHICULOENTREGA (MVEH_INVENTARIO, PDOC_CODIGO, MVEE_NUMERO, MVEE_FECHAENTREGA,MVEE_HORAENTREGA,MVEE_NITENTREGA,MVEE_NITRECIBE,MVEE_NOMBNITRECIBE,PALM_ALMACEN,MVEE_CIUDADMATRICULA,MVEE_USUARIO) 
                                     VALUES ('" + Request.QueryString["vehiInv"] + "',null,null,'" + txtFechaEntrega.Text + "','" + hora + "','" + txtNitEntrega.Text + "','" + txtNitRecibe.Text + "','" + txtNombreRecibe.Text + "','" + ddlAlmacen.SelectedValue + "','" + ddlCiudadMatricula.SelectedValue + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "')");
                else
                    sqlStrings.Add(@"UPDATE MVEHICULOENTREGA SET MVEE_FECHAENTREGA = '" + txtFechaEntrega.Text + "', MVEE_HORAENTREGA = '" + hora + "', MVEE_NITENTREGA = '" + txtNitEntrega.Text + "', MVEE_NITRECIBE = '" + txtNitRecibe.Text + "', MVEE_NOMBNITRECIBE = '" + txtNombreRecibe.Text + "', PALM_ALMACEN = '" + ddlAlmacen.SelectedValue + "', MVEE_CIUDADMATRICULA = '" + ddlCiudadMatricula.SelectedValue + "', MVEE_USUARIO = '" + HttpContext.Current.User.Identity.Name.ToLower() + "' " +
                        " WHERE MVEH_INVENTARIO = " + ViewState["invVehiculo"].ToString() + " ");
                if (DBFunctions.Transaction(sqlStrings))
                {
                    Response.Redirect(""+indexPage+"?process=Vehiculos.EntregaVehiculos&exito=1&prefDev=null&numDev=null");
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Se presentó un problema al tratar de realizar el proceso, contacte al administrador del sistema para más información.");
                    string debug = DBFunctions.exceptions;
                }
            }
        }
        protected bool validarCampos()
        {

            if (txtPlaca.Text == "" || txtInventario.Text == "" || /*txtMatricula.Text == "" ||*/ txtFechaMatricula.Text == "" || txtNitEntrega.Text == ""
                || txtNitRecibe.Text == "" || txtNombreRecibe.Text == "" || ddlAlmacen.SelectedValue == "0" || txtFechaEntrega.Text == "")
            {
                Utils.MostrarAlerta(Response, "Faltan campos por llenar!");
                return false;
            }
            else if(DBFunctions.RecordExist("select MCAT_PLACA FROM MCATALOGOVEHICULO WHERE mcat_placa = '"+ txtPlaca.Text + "' and MCAT_VIN != '" + ViewState["vinVehiculo"] + "' "))
            {
                Utils.MostrarAlerta(Response, "Esta placa ya esta registrada para en el sistema para otro VIN " );
                return false;
            }
            else
                return true;
        }

        protected bool validarPlaca(string txtPlaca, string inventario)
        {

            if (DBFunctions.RecordExist("select MCAT_PLACA FROM MCATALOGOVEHICULO WHERE mcat_placa = '" + txtPlaca + "' and MCAT_VIN != '" + ViewState["vinVehiculo"] + "' "))
            {
                Utils.MostrarAlerta(Response, "Esta placa ya esta registrada para en el sistema para otro VIN ");
                return false;
            }
            else
                return true;
        }

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

