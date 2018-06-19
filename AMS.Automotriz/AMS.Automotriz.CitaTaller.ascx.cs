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
	public partial class CitaTaller : System.Web.UI.UserControl
	{
		protected DataTable dtCitas;
		protected Label perrito,Label1;
		protected ArrayList recepcionistas,horasAlmuerzo;
		protected string fecha, sqlRecepcionistas;
		protected Button []btns;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
		int cantidadBotones=0,mn=0;
		protected System.Web.UI.WebControls.ImageButton origen;
		protected System.Web.UI.WebControls.PlaceHolder datosOrigen;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.ImageButton propietario;
		protected System.Web.UI.WebControls.PlaceHolder datosPropietario;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.ImageButton vehiculo;
		protected System.Web.UI.WebControls.PlaceHolder datosVehiculo;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.ImageButton otros;
		protected System.Web.UI.WebControls.PlaceHolder otrosDatos;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.ImageButton botonKits;
		protected System.Web.UI.WebControls.PlaceHolder kitsCombos;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.ImageButton opPeritaje;
		protected System.Web.UI.WebControls.PlaceHolder operacionesPeritaje;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.ImageButton estOrdenes;
		protected System.Web.UI.WebControls.PlaceHolder estadoOrdenes;
		protected eWorld.UI.CalendarPopup CalendarPopup1;
		protected System.Web.UI.WebControls.Button grabar;

        private DatosCliente datosCliente;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				CargarDatosIniciales();
			}

            cargarPanelCliente();

			if (ViewState["Consulta"] == null)
			{
				Session.Remove("dtCitas");
				Session.Remove("opcion");
			}
			this.PrepararTabla(taller.SelectedValue);
			this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "NO", "", "");


		}
		
		protected void Consultar_Citas(Object  Sender, EventArgs e)
		{
			Session.Remove("dtCitas");
			Session.Remove("opcion");
            if (taller.SelectedItem != null)
            {
                this.PrepararTabla(taller.SelectedItem.ToString());
                this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "NO", "", "");
            }
			ViewState["Consulta"] = true;
		}
		
		protected void Cambio_Taller(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
            sqlRecepcionistas = @"SELECT pven_nombre FROM pvendedor PV, PVENDEDORALMACEN PVA, palmacEN pa 
                                WHERE tvend_codigo='RT' AND pven_vigencia='V' AND pv.pven_codigo = pva.pven_codigo AND PVA.palm_almacen = PA.palm_almacen 
                                AND PA.TVIG_VIGENCIA = 'V' and palm_descripcion='" + taller.SelectedItem.ToString() + @"' ORDER BY pven_nombre ASC";

            bind.PutDatasIntoDropDownList(recepcion, sqlRecepcionistas);
			Session.Remove("dtCitas");
			Session.Remove("opcion");
			this.PrepararTabla(taller.SelectedItem.ToString());
			this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"),"NO","","");
		}
		
		protected  void  Cambio_Vehiculo(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(servicio, "SELECT pkit_codigo, pkit_nombre FROM pkit WHERE PKIT_VIGENCIA = 'V' and pgru_grupo='" + DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='" + vehiculos.SelectedValue + "'") + "'");
			servicio.Items.Insert(0, new ListItem("--No Asignado--", ""));
            bind.PutDatasIntoDropDownList(ddlPrecios, "select PPRETA_CODIGO, PPRETA_NOMBRE from ppreciotaller;");
            ddlPrecios.Items.Insert(0, new ListItem("Seleccione...", ""));
        }
		
		public  void  Consultar_Vehiculo(Object  Sender, EventArgs e)
		{
			if(DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text+"'"))
			{
				if(DBFunctions.SingleData("SELECT mcat_password WHERE mcat_placa='"+placa.Text+"'")==clave.Text)
				{
					//DatasToControls.EstablecerDefectoDropDownList(vehiculos,DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text+"'"));
					vehiculos.Enabled = true;
					vehiculos.SelectedValue = DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text+"'");
					vehiculos.Enabled = false;
					
                    string nit = DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text.Trim()+"'");

                    if (datosCliente == null)
                        cargarPanelCliente();
                    datosCliente.CargarDatosIniciales(nit);
                    
                    //nombre.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='"+nit+"'");
                    //telefono.Text = DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='"+nit+"'");
                    //celular.Text = DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='"+nit+"'");
                    //correo.Text = DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='"+nit+"'");
					Cambio_Vehiculo(Sender,e);
                    Consultar_Citas(Sender, e);
				}
				else
                    Utils.MostrarAlerta(Response, "Clave Invalida Para Este Vehiculo");
			}
			else
                Utils.MostrarAlerta(Response, "Este vehiculo no se encuentra registrado");
		}
		
        //protected  void  Guardar_Cita(Object  Sender, EventArgs e)
        //{
            
        //    datosCliente = (DatosCliente)phDatosCliente.Controls[0];
        //    Control controlHiddencolores = (Control)phDatosCliente.Controls[0];
        //    if ((placa.Text == "") || !datosCliente.TieneSuficientesDatos())
        //        Utils.MostrarAlerta(Response, "Hace Falta Algún Dato. Revise Por Favor");
        //    else
        //    {
        //        ArrayList sqlStrings = new ArrayList();
        //        string codigoRecepcionista = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='"+recepcion.SelectedItem.ToString()+"'");
        //        string fechaGrabacion = calFecha.SelectedDate.ToString("yyyy-MM-dd");
        //        if(codigoRecepcionista == null || codigoRecepcionista.Equals(""))
        //        {
        //            Utils.MostrarAlerta(Response, "Por favor seleccione un recepcionista");
        //            return;
        //        }

        //        DateTime horaAlmuerzo = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT pven_horalmuerzo FROM pvendedor WHERE pven_codigo='"+codigoRecepcionista+"'"));
        //        DateTime horaFinal = horaAlmuerzo.AddMinutes(59);
        //        DateTime hora = System.Convert.ToDateTime(horaEsc.SelectedItem.ToString());
        //        string kit = (servicio.SelectedValue.Length==0) ? "NULL" : "'"+servicio.SelectedValue+"'";
        //        if((hora>=horaAlmuerzo)&&(hora<horaFinal))
        //            Utils.MostrarAlerta(Response, "Lo sentimos ha esta hora el recepcionista se encuentra reservado");
        //            //lbErr.Text = "Lo sentimos ha esta hora el recepcionista se encuentra reservado";
        //        else if((!DBFunctions.RecordExist("SELECT * FROM mcitataller WHERE mcit_fecha='"+fechaGrabacion+"' AND mcit_hora='"+horaEsc.SelectedItem.ToString()+"' AND mcit_codven='"+codigoRecepcionista+"'"))&&(!DBFunctions.RecordExist("SELECT * FROM mcitataller where mcit_fecha='"+fechaGrabacion+"' AND mcit_placa='"+placa.Text+"'")))
        //        {
        //            sqlStrings.Add("INSERT INTO mcitataller VALUES('" + fechaGrabacion + "','" + horaEsc.SelectedItem.ToString() + "','" + codigoRecepcionista + "','" + vehiculos.SelectedValue + "','" + placa.Text.ToString() + "','" + datosCliente.Nombrecompleto + "','" + datosCliente.Telefono + "','" + datosCliente.Celular + "','" + datosCliente.Email + "'," + kit + ",'N','" + tbObservaciones.Text + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + taller.SelectedValue + "')");
        //            if (DBFunctions.Transaction(sqlStrings))
        //            {
        //                lbErr.Text = "";
        //                Utils.MostrarAlerta(Response, "Su CITA ha sido creada SATISFACTORIAMENTE .. lo esperamos !");
        //            }
        //            else
        //            {
        //                lbErr.Text = "Error :" + DBFunctions.exceptions;
        //                Utils.MostrarAlerta(Response, "La CITA no ha podido ser registrada. Por favor revise los datos de ingreso.");
        //            }
                    
        //            this.PrepararTabla(taller.SelectedItem.ToString());
        //            this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"), "SI", horaEsc.SelectedItem.ToString(), codigoRecepcionista);
        //        }
        //        else
        //            Utils.MostrarAlerta(Response, "Usted ha escogido una opcion invalida o ya tiene una cita programada para hoy");
        //    }
        //}

        protected void Guardar_Cita_dgCitas(Object sender, CommandEventArgs e)
        {
            DataTable dtPrueba = new DataTable();
            dtPrueba = (DataTable)Session["dtCitas"];
            //string nombreRecepcionista = dtPrueba.Columns[];
            
            datosCliente = (DatosCliente)phDatosCliente.Controls[0];
            if ((placa.Text == "") || !datosCliente.TieneSuficientesDatos())
                Utils.MostrarAlerta(Response, "Hace Falta Algún Dato. Revise Por Favor");
            else
            {
                ArrayList sqlStrings = new ArrayList();
                string pruebaRecepcionista = (e.CommandArgument).ToString();
                string codigoRecepcionista = pruebaRecepcionista.Substring(0, pruebaRecepcionista.IndexOf("_"));
                //string codigoRecepcionista = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + recepcion.SelectedItem.ToString() + "'");
                string fechaGrabacion = calFecha.SelectedDate.ToString("yyyy-MM-dd");
                

                if (codigoRecepcionista == null || codigoRecepcionista.Equals(""))
                {
                    Utils.MostrarAlerta(Response, "Por favor seleccione un recepcionista");
                    return;
                }

                DateTime horaAlmuerzo = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT pven_horalmuerzo FROM pvendedor WHERE pven_codigo='" + codigoRecepcionista + "'"));
                DateTime horaFinal = horaAlmuerzo.AddMinutes(59);
                DateTime hora = System.Convert.ToDateTime(horaEsc.SelectedItem.ToString());
                string kit = (servicio.SelectedValue.Length == 0) ? "NULL" : "'" + servicio.SelectedValue + "'";
                if ((hora >= horaAlmuerzo) && (hora < horaFinal))
                    Utils.MostrarAlerta(Response, "Lo sentimos ha esta hora el recepcionista se encuentra reservado");
            }
                       
            datosCliente = (DatosCliente)phDatosCliente.Controls[0];

            Control controlHiddencolores =(Control)phDatosCliente.Controls[0];

            string colores =  ((HiddenField)controlHiddencolores.FindControl("HiColores")).Value;
            if (colores == "verde")
            {
                colores = "V";
            }
            else if (colores == "rojo")
            {
                colores = "R";
            }
            else if (colores == "amarillo")
            {
                colores = "A";
            }
            else
            {
                colores = "V";
            }
            if ((placa.Text == "") && !datosCliente.TieneSuficientesDatos())
                Utils.MostrarAlerta(Response, "Falta algun dato por entrar. Por favor revise");
            else
            {
				ArrayList sqlStrings = new ArrayList();
                
				string opc = (e.CommandArgument).ToString();
                //string opc = ((CommandEventArgs)e).CommandArgument.ToString();
				string codigoRecepcionista = opc.Substring(0,opc.IndexOf("_"));
				string horaGrabacion = opc.Substring(opc.IndexOf("_")+1);
				string fechaGrabacion = calFecha.SelectedDate.ToString("yyyy-MM-dd");
				string kit = (servicio.SelectedValue.Length == 0) ? "NULL" : "'" + servicio.SelectedValue + "'";
                string Color = colores;
                

				if(!DBFunctions.RecordExist("SELECT * FROM mcitataller where mcit_fecha='"+fechaGrabacion+"' AND mcit_placa='"+placa.Text+"'"))
				{
                    sqlStrings.Add("INSERT INTO mcitataller VALUES('" + fechaGrabacion + "','" + horaGrabacion + "','" + codigoRecepcionista + "','" + vehiculos.SelectedValue + "','" + placa.Text.ToString() + "','" + datosCliente.Nombrecompleto + "','" + datosCliente.Telefono + "','" + datosCliente.Celular + "','" + datosCliente.Email + "'," + kit + ",'N','" + tbObservaciones.Text + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + taller.SelectedValue +  "','" + Color + "');");
                    if (DBFunctions.Transaction(sqlStrings))
                    {
                        lbErr.Text = "";
                        Utils.MostrarAlerta(Response, "Su CITA ha sido creada SATISFACTORIAMENTE .. lo esperamos !");
                    }
                    else
                    {
                        lbErr.Text = "Error :" + DBFunctions.exceptions;
                        Utils.MostrarAlerta(Response, "La CITA no ha podido ser registrada. Por favor revise los datos de ingreso.");
                    }
                    this.PrepararTabla(taller.SelectedItem.ToString());
					this.LlenarCitas(calFecha.SelectedDate.ToString("yyyy-MM-dd"),"SI",horaGrabacion,codigoRecepcionista);					
				}
				else
                    Utils.MostrarAlerta(Response, "Usted ya tiene una cita para el dia de hoy");
            }			
		}
					
		//PREPARAR DATATABLE QUE CONTENDRA LAS dtCitas ASIGNADAS
		protected void PrepararTabla(string taller)
		{
			int up=3,low=51; //Cantidad de rangos arriba y abajo
			recepcionistas = new ArrayList();
			horasAlmuerzo = new ArrayList();
            string codigoTaller = DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE TVIG_VIGENCIA = 'V' and palm_descripcion='" + taller + "'");
            if (codigoTaller == "")
                codigoTaller = taller;
            DataSet recepcionistasC = new DataSet();
            sqlRecepcionistas = @"SELECT PV.pven_codigo, pven_nombre, pven_horalmuerzo FROM pvendedor PV, PVENDEDORALMACEN PVA, palmacEN pa 
                                WHERE tvend_codigo='RT' AND pven_vigencia='V' AND pv.pven_codigo = pva.pven_codigo AND PVA.palm_almacen = PA.palm_almacen 
                                AND PA.TVIG_VIGENCIA = 'V' and PVA.palm_almacen = '" + codigoTaller + "' ORDER BY pven_nombre ASC";

			//DBFunctions.Request(recepcionistasC,IncludeSchema.NO,"SELECT pven_codigo, pven_nombre, pven_horalmuerzo FROM pvendedor WHERE tvend_codigo='RT' AND pven_vigencia='V' AND palm_almacen = '"+codigoTaller+"' ORDER BY pven_nombre ASC");
            DBFunctions.Request(recepcionistasC, IncludeSchema.NO, sqlRecepcionistas);
          
			for(int i=0;i<recepcionistasC.Tables[0].Rows.Count;i++)
			{
				//Guardamos en el ArrayList el codigo de los recepcionistas
				recepcionistas.Add(recepcionistasC.Tables[0].Rows[i].ItemArray[0].ToString());
				horasAlmuerzo.Add(recepcionistasC.Tables[0].Rows[i].ItemArray[2].ToString());
			}
			if(Session["dtCitas"] == null)
			{
				dtCitas = new DataTable();
				//Se crea la columna de las horas
				dtCitas.Columns.Add(new DataColumn("HORA",System.Type.GetType("System.String")));//0
				//Se crean las columnas correspondientes a cada recepcionista
				for(int i=0;i<recepcionistasC.Tables[0].Rows.Count;i++)
					dtCitas.Columns.Add(new DataColumn(recepcionistasC.Tables[0].Rows[i][0].ToString() + " - " + recepcionistasC.Tables[0].Rows[i][1].ToString(),System.Type.GetType("System.String")));//0
				//Se crean las filas que corresponden a los horarios de atencion definidos
				DataSet horas = new DataSet();
				DBFunctions.Request(horas,IncludeSchema.NO,"SELECT ctal_hirec, ctal_hfrec, ctal_citaporhora FROM ctaller");
				DateTime inicio = Convert.ToDateTime(horas.Tables[0].Rows[0][0].ToString());
				DateTime final = Convert.ToDateTime(horas.Tables[0].Rows[0][1].ToString());
				DateTime baseC = Convert.ToDateTime(this.horaPref.SelectedValue);
				double tamIntervalo = this.CalculoIntervalo(System.Convert.ToDouble(horas.Tables[0].Rows[0].ItemArray[2].ToString()));
				DateTime movil;
				//Se construyen las filas que van arriba
				int j;
				for(j=1;j<=up;j++)
				{
					movil = baseC.AddMinutes((4-j)*(-1)*tamIntervalo);
					if(DateTime.Compare(movil,inicio)>0)
					{
						DataRow filaM;
						filaM = dtCitas.NewRow();
						filaM[0] = movil.TimeOfDay.ToString()+" - "+movil.AddMinutes(tamIntervalo).TimeOfDay.ToString();
						dtCitas.Rows.Add(filaM);
					}
				}
				//Se construye la fila correspondiente a la hora escogida
				DataRow filaE;
				filaE = dtCitas.NewRow();
				filaE[0] = baseC.TimeOfDay.ToString()+" - "+baseC.AddMinutes(tamIntervalo).TimeOfDay.ToString();
				dtCitas.Rows.Add(filaE);
				//Se contruyen las filas que van por abajo
				for(j=1;j<=low;j++)
				{
					movil = baseC.AddMinutes(j*tamIntervalo);
					if(DateTime.Compare(movil,final)<0)
					{
						DataRow filaM;
						filaM = dtCitas.NewRow();
						filaM[0] = movil.TimeOfDay.ToString()+" - "+movil.AddMinutes(tamIntervalo).TimeOfDay.ToString();
						dtCitas.Rows.Add(filaM);
					}
				}
				dgCitas.DataSource = dtCitas;
				Session["dtCitas"] = dtCitas;
				dgCitas.DataBind();
			}
			else
				dtCitas = (DataTable)Session["dtCitas"];
			///////////////////////////////////////////////
		}
		
		//FUNCION QUE NOS PERMITE CALCULAR CUANTOS MINUTOS DEBE DURAR UNA CITA CON EL RECEPCIONISTA
		protected  double CalculoIntervalo(double cantidad)
		{
			double factor = (1/cantidad);
			return (factor*60);
		}
		
		//FUNCION QUE NOS LLENA LAS CITAS DENTRO DEL DATAGRID PARA QUE PUEDAN SER OBSERVADAS
		protected void LlenarCitas(string fecha , string opcion, string horaR, string codRecep)
		{
			int i,j,k,bt=0,cantidadColumnas;
			mn+=1;
			DataSet conjuntodtCitas = new DataSet();
			DBFunctions.Request(conjuntodtCitas,IncludeSchema.NO,"SELECT mcit_fecha, mcit_hora, mcit_codven, mcit_placa FROM mcitataller WHERE mcit_fecha='"+fecha+"'");
			cantidadColumnas = recepcionistas.Count;
			cantidadBotones = (dgCitas.Items.Count)*cantidadColumnas;
			btns = new Button[cantidadBotones];
			for(i=0;i<dgCitas.Items.Count;i++)
			{
				string intervalo = dgCitas.Items[i].Cells[0].Text;
				string parte1 = intervalo.Substring(0,intervalo.IndexOf(" "));
				string parte2 = intervalo.Substring(intervalo.IndexOf("-")+2);
				DateTime inicio = Convert.ToDateTime(parte1);
				DateTime final = Convert.ToDateTime(parte2);
				for(j=0;j<recepcionistas.Count;j++)
				{
					DateTime horaInicioAlmuerzo = System.Convert.ToDateTime(horasAlmuerzo[j].ToString());
					DateTime horaFinalAlmuerzo = horaInicioAlmuerzo.AddMinutes(59);
					if(conjuntodtCitas.Tables[0].Rows.Count==0)
					{
						if((horaInicioAlmuerzo)<=(inicio)&&((horaFinalAlmuerzo)>(inicio)))
						{
							dgCitas.Items[i].Cells[j+1].CssClass = "Celdas";
							dgCitas.Items[i].Cells[j+1].BackColor = Color.Orange;
							dgCitas.Items[i].Cells[j+1].HorizontalAlign = HorizontalAlign.Center;
							dgCitas.Items[i].Cells[j+1].Text = "RESERVADO";
						}
						else
						{
							dgCitas.Items[i].Cells[j+1].CssClass = "Celdas";
							dgCitas.Items[i].Cells[j+1].BackColor = Color.LightGreen;
							dgCitas.Items[i].Cells[j+1].HorizontalAlign = HorizontalAlign.Center;
							Button btn = new Button();
							//btn.CommandArgument = recepcionistas[j].ToString()+"_"+inicio.TimeOfDay.ToString();
                            String hora = dgCitas.Items[i].Cells[0].Text.Substring(0,8);
                            btn.CommandArgument = recepcionistas[j].ToString() + "_" + hora;
							btn.BackColor = Color.LightGreen;
							btn.Text = "LIBRE";
							btn.Width = dgCitas.Items[i].Cells[j+1].Width;
                            btn.Command += new CommandEventHandler(this.Guardar_Cita_dgCitas);
                            //btn.Click += new EventHandler(this.Guardar_Cita_dgCitas);
							btns[bt] = btn;
							dgCitas.Items[i].Cells[j+1].Controls.Add(btns[bt]);
							bt+=1;
						}		
											
					}
					else
					{
						bool cita = false;
						for(k=0;k<conjuntodtCitas.Tables[0].Rows.Count;k++)
						{
							DateTime hora = System.Convert.ToDateTime(conjuntodtCitas.Tables[0].Rows[k].ItemArray[1].ToString());
                            String placa = conjuntodtCitas.Tables[0].Rows[k].ItemArray[3].ToString();
                            
                            if((horaInicioAlmuerzo)<=(inicio)&&((horaFinalAlmuerzo)>(inicio)))
							{
								cita = true;
								dgCitas.Items[i].Cells[j+1].CssClass = "Celdas";
								dgCitas.Items[i].Cells[j+1].BackColor = Color.Orange;
								dgCitas.Items[i].Cells[j+1].HorizontalAlign = HorizontalAlign.Center;
								dgCitas.Items[i].Cells[j+1].Text = "RESERVADO";
							}
							else if((conjuntodtCitas.Tables[0].Rows[k].ItemArray[2].ToString()==recepcionistas[j].ToString())&&(hora>=inicio)&&(hora<final))
							{
								cita=true;
								dgCitas.Items[i].Cells[j+1].HorizontalAlign = HorizontalAlign.Center;
								dgCitas.Items[i].Cells[j+1].CssClass = "Celdas";
								dgCitas.Items[i].Cells[j+1].Font.Bold = true;
								string cmp = inicio.TimeOfDay.ToString();
								if((recepcionistas[j].ToString()==codRecep)&&(cmp==horaR)&&(opcion=="SI"))
								{
									dgCitas.Items[i].Cells[j+1].BackColor = Color.Yellow;
									dgCitas.Items[i].Cells[j+1].Text = "SU CITA";								
								}
								else
								{
									dgCitas.Items[i].Cells[j+1].BackColor = Color.Tomato;
                                    dgCitas.Items[i].Cells[j + 1].Text = "OCUPADO<BR>" + placa;								
								}						
							}
							else if(!cita)
							{
								dgCitas.Items[i].Cells[j+1].HorizontalAlign = HorizontalAlign.Center;
								dgCitas.Items[i].Cells[j+1].CssClass = "Celdas";
								dgCitas.Items[i].Cells[j+1].BackColor = Color.LightGreen;
								if((bt<cantidadBotones)&&(dgCitas.Items[i].Cells[j+1].Controls.Count==0))
								{
									Button btn = new Button();
									//btn.CommandArgument = recepcionistas[j].ToString()+"_"+inicio.TimeOfDay.ToString();
									btn.CommandArgument = recepcionistas[j].ToString() + "_" + dgCitas.Items[i].Cells[0].Text.Substring(0, 8);
									btn.BackColor = Color.LightGreen;
									btn.Text = "LIBRE";
									btn.Command += new CommandEventHandler(this.Guardar_Cita_dgCitas);
									btns[bt] = btn;
									dgCitas.Items[i].Cells[j+1].Controls.Add(btns[bt]);
									bt+=1;
								}								
							}						
							
						}
					}
					
				}
			}
		}
		
		//FUNCION PARA LLENAR EL LISTADO DE LAS HORAS PARA LOS CLIENTES
		protected void HorasAtencion()
		{
			DataSet intervalo = new DataSet();
			DBFunctions.Request(intervalo,IncludeSchema.NO,"SELECT ctal_hirec, ctal_hfrec, ctal_citaporhora FROM ctaller");
			DateTime inicio = System.Convert.ToDateTime(intervalo.Tables[0].Rows[0].ItemArray[0].ToString());
			DateTime final = System.Convert.ToDateTime(intervalo.Tables[0].Rows[0].ItemArray[1].ToString());
			double tamIntervalo = this.CalculoIntervalo(System.Convert.ToDouble(intervalo.Tables[0].Rows[0].ItemArray[2].ToString()));
			while(DateTime.Compare(final,inicio)>0)
			{
				horaPref.Items.Add(new ListItem(inicio.TimeOfDay.ToString(),inicio.TimeOfDay.ToString()));
				horaEsc.Items.Add(new ListItem(inicio.TimeOfDay.ToString(),inicio.TimeOfDay.ToString()));
				inicio = inicio.AddMinutes(tamIntervalo);
			}
		}
		
		protected void Borrar_dgCitas()
		{
			string opcion = (Session["opcion"]).ToString();
			if (opcion=="consulta")
			{
				dgCitas.DataSource = null;
				dgCitas.DataBind();
				Array.Clear(btns,0,btns.Length);
			}
		}

        public void CargarDatosIniciales()
        {
            DateTime diaActual = DateTime.Now;

            Utils.FillDll(taller, "SELECT PALM_ALMACEN, palm_descripcion FROM palmacen where (PCEN_CENTTAL IS NOT NULL  or pcen_centcoli is not null) and TVIG_VIGENCIA = 'V' ORDER BY palm_descripcion ASC", false);
            this.HorasAtencion();
            calFecha.SelectedDate = DateTime.Now;

            sqlRecepcionistas = @"SELECT pven_nombre FROM pvendedor PV, PVENDEDORALMACEN PVA, palmacEN pa 
                                WHERE tvend_codigo='RT' AND pven_vigencia='V' AND pv.pven_codigo = pva.pven_codigo AND PVA.palm_almacen = PA.palm_almacen 
                                AND PA.TVIG_VIGENCIA = 'V' and palm_descripcion='" + taller.SelectedItem.ToString() + @"' ORDER BY pven_nombre ASC";

            Utils.FillDll(recepcion, sqlRecepcionistas, true);
            Utils.FillDll(vehiculos, "SELECT pcat_codigo, pcat_codigo CONCAT ' ' CONCAT pcat_descripcion FROM pcatalogovehiculo ORDER BY pcat_codigo", false);
            Utils.FillDll(servicio, "SELECT pkit_codigo, pkit_nombre FROM pkit WHERE PKIT_VIGENCIA = 'V' and pgru_grupo='" + DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='" + vehiculos.SelectedValue + "'") + "' ORDER BY pkit_codigo", true);

			btns = new Button[0];
        }

        public void cargarPanelCliente()
        {
            datosCliente = (DatosCliente)LoadControl(pathToControls + "AMS.Automotriz.DatosCliente.ascx");
            phDatosCliente.Controls.Add(datosCliente);
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
