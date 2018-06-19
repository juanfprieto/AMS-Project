// created on 06/09/2004 at 9:44

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
using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
    public partial class CitaTallerWeb : System.Web.UI.UserControl
    {
        protected DataTable dtCitas;
        protected Label perrito,Label1;
        protected ArrayList recepcionistas,horasAlmuerzo;
        protected string fecha;
        protected Button []btns;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected int cantidadBotones=0,mn=0;
        protected string pGrupoWeb = ConfigurationManager.AppSettings["GrupoCatalogoWeb"];
        protected string pCatalogoDefecto = ConfigurationManager.AppSettings["CatalogoCitasWeb"];
        protected ClientScriptManager clientScript;

        protected void Page_Load(object sender, System.EventArgs e)
        {                 

            clientScript = this.Page.ClientScript;
            if (!IsPostBack)
            {
                DateTime diaActual = DateTime.Now;
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlTaller, "SELECT palm_almacen, palm_descripcion FROM palmacen where TVIG_VIGENCIA='V' and (PCEN_CENTTAL IS NOT NULL or pcen_centcoli is not null) and pcen_centcart is not null ORDER BY palm_descripcion ASC");
                calFecha.SelectedDate = DateTime.Now;
                bind.PutDatasIntoDropDownList(servicio, "SELECT pkit_codigo, pkit_nombre FROM pkit WHERE PKIT_VIGENCIA = 'V' and pgru_grupo='" + pGrupoWeb + "' ORDER BY pkit_codigo");
                servicio.Items.Insert(0, new ListItem("--No Asignado--", ""));
                btns = new Button[0];
                this.PrepararTabla(ddlTaller.SelectedValue);
                this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "NO", "", "");
            }
            if (ViewState["Consulta"] == null)
            {
                Session.Remove("dtCitas");
                Session.Remove("opcion");
            }
            this.PrepararTabla(ddlTaller.SelectedValue);
            this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "NO", "", "");
        }

        public void Consultar_Vehiculo(Object Sender, EventArgs e)
        {
            placa.Text = placa.Text.ToUpper().Replace(" ", "").Replace("-", "");

            if (DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_placa='" + placa.Text + "'"))
            {
                txtVehiculo.Enabled = true;
                string sql = String.Format("SELECT pc.pcat_descripcion \n" +
                 "FROM mcatalogovehiculo mc  \n" +
                 "  LEFT JOIN pcatalogovehiculo pc ON mc.pcat_codigo = pc.pcat_codigo \n" +
                 "WHERE mcat_placa = '{0}'", placa.Text);
                txtVehiculo.Text = DBFunctions.SingleData(sql).Trim();
                txtVehiculo.Enabled = false;

                string nit = DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='" + placa.Text.Trim() + "'");
                nombre.Text = DBFunctions.SingleData("SELECT COALESCE(mnit_nombres,'') CONCAT ' ' CONCAT COALESCE(mnit_nombre2,'') CONCAT ' ' CONCAT " +
                                "COALESCE(mnit_apellidos,'') CONCAT ' ' CONCAT COALESCE(mnit_apellido2,'') FROM mnit WHERE mnit_nit='" + nit + "'");
                telefono.Text = DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='" + nit + "'");
                celular.Text = DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='" + nit + "'");
                correo.Text = DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='" + nit + "'");

                lblNombre.Visible = false;
                lblTelefono.Visible = false;
                lblCelular.Visible = false;
                lblCorreo.Visible = false;

                nombre.Visible = false;
                telefono.Visible = false;
                celular.Visible = false;
                correo.Visible = false;

                Utils.MostrarAlerta(Response, "Sus datos se encuentran en nuestra base de datos. Por favor seleccione la hora y fecha de su cita");
                
            }
            else
            {
                lblNombre.Visible = true;
                lblTelefono.Visible = true;
                lblCelular.Visible = true;
                lblCorreo.Visible = true;

                nombre.Visible = true;
                telefono.Visible = true;
                celular.Visible = true;
                correo.Visible = true;

                txtVehiculo.Enabled = true;
                txtVehiculo.Text = "";
                nombre.Text = "";
                telefono.Text = "";
                celular.Text = "";
                correo.Text = "";

                Utils.MostrarAlerta(Response, "Este vehiculo no se encuentra registrado, por favor ingrese sus datos");

                
            }
        }
        
        protected void Guardar_Cita_dgCitas(Object sender, CommandEventArgs e)
        {
            placa.Text = placa.Text.ToUpper().Replace(" ", "").Replace("-", "");

            if ((placa.Text == "") || (nombre.Text == "") || (telefono.Text == ""))
                Utils.MostrarAlerta(Response, "Falta algun dato por entrar. Por favor revise");
    
            else
            {
                string opc = (e.CommandArgument).ToString();
                string codigoRecepcionista = opc.Substring(0, opc.IndexOf("_"));
                string horaGrabacion = opc.Substring(opc.IndexOf("_") + 1);
                string fechaGrabacion = calFecha.SelectedDate.ToString("yyyy-MM-dd");
                string kit = (servicio.SelectedValue.Length == 0) ? "NULL" : "'" + servicio.SelectedValue + "'";
                if (!DBFunctions.RecordExist("SELECT * FROM mcitataller where mcit_fecha='" + fechaGrabacion + "' AND mcit_placa='" + placa.Text + "'"))
                {
                    string sqlInsert = 
                        "INSERT INTO mcitataller VALUES('" + 
                        fechaGrabacion + "','" + 
                        horaGrabacion + "','" + 
                        codigoRecepcionista + "','" +
                        pCatalogoDefecto + "','" + 
                        placa.Text.ToString() + "','" + 
                        nombre.Text.ToString() + "','" + 
                        telefono.Text.ToString() + "','" +
                        celular.Text.ToString() + "','" + 
                        correo.Text.ToString() + "'," + 
                        kit + "," + 
                        "'N'," +
                        String.Format("'Catálogo: {0} .:. Notas: {1}','", txtVehiculo.Text, tbObservaciones.Text) +
                        HttpContext.Current.User.Identity.Name.ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";

                    if (DBFunctions.NonQuery(sqlInsert) == 1)
                        lbErr.Text = "";
                    else
                        lbErr.Text = "Error :" + DBFunctions.exceptions;

                    this.PrepararTabla(ddlTaller.SelectedValue);
                    this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "SI", horaGrabacion, codigoRecepcionista);
                    Utils.MostrarAlerta(Response, "Su cita quedó guardada para el {0} a las {1}');  " +
                        "window.location.href ='http://www.caldasmotor.com");   
                    
                }
                else
                    Utils.MostrarAlerta(Response, "Usted ya tiene una cita para el dia de hoy");
                    
            }
        }

        //PREPARAR DATATABLE QUE CONTENDRA LAS dtCitas ASIGNADAS
        protected void PrepararTabla(string codigoTaller)
        {
            recepcionistas = new ArrayList();
            horasAlmuerzo = new ArrayList();
            DataSet recepcionistasC = new DataSet();
            DBFunctions.Request(recepcionistasC, IncludeSchema.NO, "SELECT pven_codigo, pven_nombre, pven_horalmuerzo FROM pvendedor WHERE tvend_codigo='RT' AND pven_vigencia='V' AND palm_almacen = '" + codigoTaller + "' ORDER BY pven_nombre ASC");
            for (int i=0; i < recepcionistasC.Tables[0].Rows.Count; i++)
            {
                //Guardamos en el ArrayList el codigo de los recepcionistas
                recepcionistas.Add(recepcionistasC.Tables[0].Rows[i].ItemArray[0].ToString());
                horasAlmuerzo.Add(recepcionistasC.Tables[0].Rows[i].ItemArray[2].ToString());
            }
            if (Session["dtCitas"] == null)
            {
                dtCitas = new DataTable();
                //Se crea la columna de las horas
                dtCitas.Columns.Add(new DataColumn("HORA", System.Type.GetType("System.String")));//0
                //Se crean las columnas correspondientes a cada recepcionista
                for (int i=0; i < recepcionistasC.Tables[0].Rows.Count; i++)
                    dtCitas.Columns.Add(new DataColumn(recepcionistasC.Tables[0].Rows[i][1].ToString(), System.Type.GetType("System.String")));//0

                //Se crean las filas que corresponden a los horarios de atencion definidos
                DataSet horas = new DataSet();
                DBFunctions.Request(horas, IncludeSchema.NO, "SELECT ctal_hirec, ctal_hfrec, ctal_citaporhora FROM ctaller");
                DateTime inicio = Convert.ToDateTime(horas.Tables[0].Rows[0][0].ToString());
                DateTime final = Convert.ToDateTime(horas.Tables[0].Rows[0][1].ToString());
                double tamIntervalo = this.CalculoIntervalo(System.Convert.ToDouble(horas.Tables[0].Rows[0].ItemArray[2].ToString()));

            for (DateTime dt = inicio; dt < final; dt = dt.AddMinutes(tamIntervalo))
            {
                DataRow filaM;
                filaM = dtCitas.NewRow();
                filaM[0] = dt.TimeOfDay.ToString() + " - " + dt.AddMinutes(tamIntervalo).TimeOfDay.ToString();
                dtCitas.Rows.Add(filaM);
            }

                dgCitas.DataSource = dtCitas;
                //Session["dtCitas"] = dtCitas;
                dgCitas.DataBind();
            }
            else
                dtCitas = (DataTable)Session["dtCitas"];
        }

        //FUNCION QUE NOS PERMITE CALCULAR CUANTOS MINUTOS DEBE DURAR UNA CITA CON EL RECEPCIONISTA
        protected double CalculoIntervalo(double cantidad)
        {
            double factor = (1 / cantidad);
            return (factor * 60);
        }

        //FUNCION QUE NOS LLENA LAS CITAS DENTRO DEL DATAGRID PARA QUE PUEDAN SER OBSERVADAS
        protected void LlenarCitas(string fecha, string opcion, string horaR, string codRecep)
        {
            int i,j,k,bt=0,cantidadColumnas;
            mn += 1;

            DataSet conjuntodtCitas = new DataSet();
            DBFunctions.Request(conjuntodtCitas, IncludeSchema.NO, "SELECT mcit_fecha, mcit_hora, mcit_codven FROM mcitataller WHERE mcit_fecha='" + fecha + "'");

            cantidadColumnas = recepcionistas.Count;
            cantidadBotones = (dgCitas.Items.Count) * cantidadColumnas;
            btns = new Button[cantidadBotones];
            
            for (i = 0; i < dgCitas.Items.Count; i++)
            {
                string intervalo = dgCitas.Items[i].Cells[0].Text;
                string parte1 = intervalo.Substring(0, intervalo.IndexOf(" "));
                string parte2 = intervalo.Substring(intervalo.IndexOf("-") + 2);
                DateTime inicio = Convert.ToDateTime(parte1);
                DateTime final = Convert.ToDateTime(parte2);
                
                for (j = 0; j < recepcionistas.Count; j++)
                {
                    dgCitas.Items[i].Cells[j + 1].CssClass = "Celdas";
                    dgCitas.Items[i].Cells[j + 1].HorizontalAlign = HorizontalAlign.Center;

                    DateTime horaInicioAlmuerzo = System.Convert.ToDateTime(horasAlmuerzo[j].ToString());
                    DateTime horaFinalAlmuerzo = horaInicioAlmuerzo.AddMinutes(59);
                    string codRecepcionista = recepcionistas[j].ToString();

                    if ((horaInicioAlmuerzo) <= (inicio) && ((horaFinalAlmuerzo) > (inicio)))
                    {
                        dgCitas.Items[i].Cells[j + 1].Attributes.Add("style",
                            "background-image:url(../img/reservado_bg.png);background-repeat: repeat;background-color: transparent;");
                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                        img.ImageUrl = "../img/Ocupado.png";
                        img.Attributes.Add("style", "background-color: transparent;");
                        dgCitas.Items[i].Cells[j + 1].Controls.Add(img);
                    }
                    else
                    {
                        bool tieneCita = false;
                        for (k = 0; k < conjuntodtCitas.Tables[0].Rows.Count; k++)
                        {
                            DateTime hora = System.Convert.ToDateTime(conjuntodtCitas.Tables[0].Rows[k].ItemArray[1].ToString());
                            string codRecCita = conjuntodtCitas.Tables[0].Rows[k].ItemArray[2].ToString();

                            if ((codRecCita == codRecepcionista) && (hora >= inicio) && (hora < final))
                            {
                                tieneCita = true;
                                break;
                            }
                        }

                        if (!tieneCita)
                        {
                            Button btn = new Button();
                            btn.Width = 22;
                            btn.Height = 19;
                            btn.BorderWidth = 0;

                            btn.CommandArgument = recepcionistas[j].ToString() + "_" + dgCitas.Items[i].Cells[0].Text.Substring(0, 8);
                            btn.Attributes.Add("style", "background-image:url(../img/Libre.png);background-color: transparent;");
                            btn.Command += new CommandEventHandler(this.Guardar_Cita_dgCitas);

                            btns[bt] = btn;
                            dgCitas.Items[i].Cells[j + 1].Controls.Add(btns[bt]);
                            bt += 1;
                        }
                        else
                        {
                            dgCitas.Items[i].Cells[j + 1].Attributes.Add("style",
                                "background-image:url(../img/reservado_bg.png);background-repeat: repeat;background-color: transparent;");
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            img.ImageUrl = "../img/Ocupado.png";
                            img.Attributes.Add("style", "background-color: transparent;");
                            dgCitas.Items[i].Cells[j + 1].Controls.Add(img);
                        }
                    }
                }
            }
        }

        protected void Consultar_Citas(Object Sender, EventArgs e)
        {
            Session.Remove("dtCitas");
            Session.Remove("opcion");
            this.PrepararTabla(ddlTaller.SelectedValue);
            this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "NO", "", "");
            ViewState["Consulta"] = true;
        }

        protected void Cambio_Taller(Object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            Session.Remove("dtCitas");
            Session.Remove("opcion");
            this.PrepararTabla(ddlTaller.SelectedValue);
            this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "NO", "", "");
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
