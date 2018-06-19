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
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;
using AMS.Contabilidad;
using AMS.Tools;
using System.Globalization;

namespace AMS.Finanzas.Tesoreria
{
	public partial class ConsignacionCheques : System.Web.UI.UserControl
	{
      
        protected DataTable tablaDatos;
		protected Button aceptarValores;
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
		protected string pathToMain=ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoConsignacion;
        ProceHecEco contaOnline = new ProceHecEco();
	
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                if (almacen.Items.Count > 1)
                    almacen.Items.Insert(0, "Seleccione..");
           
                bind.PutDatasIntoDropDownList(tipoConsignacion,"SELECT tcau_codigo,tcau_nombre FROM ttipoconsignacion WHERE tcau_codigo<>8");
                if (tipoConsignacion.Items.Count > 1)
                    tipoConsignacion.Items.Insert(0, "Seleccione..");
                
            //    bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CS' and tvig_vigencia = 'V' ");
                else 
                    Cambiar_Accion(null, null);
              	holderConsignacionCC.Visible=holderDevolucionCC.Visible=holderRemisionFinanciera.Visible=holderDevolucionFinanciera.Visible=holderTrasladoCCCarta.Visible=holderTrasladoCCCheque.Visible=holderNotasBancarias.Visible=holderChequesProveedores.Visible=false;
				valorConsignado.Text=totalEfectivo.Text="$0.00";
				if(Request.QueryString["prefD"]!=null && Request.QueryString["numD"]!=null)
				{
                    Utils.MostrarAlerta(Response, "Se ha creado el documento " + Request.QueryString["prefD"] + "-" + Request.QueryString["numD"] + "");
					try
					{
						formatoConsignacion=new FormatosDocumentos();
						formatoConsignacion.Prefijo=Request.QueryString["prefD"];
						formatoConsignacion.Numero=Convert.ToInt32(Request.QueryString["numD"]);
						formatoConsignacion.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefD"]+"'");
						if(formatoConsignacion.Codigo!=string.Empty)
						{
							if(formatoConsignacion.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoConsignacion.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
					}
					catch
					{
						lb.Text+="Error al generar la impresión. Detalles : "+formatoConsignacion.Mensajes+"<br>";
					}
				}
			}
            // este cargue solo debe aplicarlo unicamente al proceso seleccionado 
			holderConsignacionCC.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.ConsignacionCC.ascx"));
			holderDevolucionCC.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.DevolucionCC.ascx"));
			holderRemisionFinanciera.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.RemCheqFinan.ascx"));
			holderDevolucionFinanciera.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.DevCheqFinan.ascx"));
			holderTrasladoCCCarta.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.TrasFondosCCCarta.ascx"));
			holderTrasladoCCCheque.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.TrasFondosCCCheque.ascx"));
			holderNotasBancarias.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.NotasBancarias.ascx"));
			holderChequesProveedores.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.EntrChequesProv.ascx"));
		}
		
		protected void Cambiar_Accion(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			//Si es una consignación a cuenta corriente
			if(tipoConsignacion.SelectedValue=="1")
			{
				if(!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CS'"))
				{
                    Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
					prefijoDocumento.Enabled=false;
                    prefijoDocumento.Items.Clear();
                    numeroTesoreria.Text = "";
				}
				else
				{
                    //bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CS' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento , "%", "%", "CS");
					numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
			//Si es una devolución de cuentas corrientes
			else if(tipoConsignacion.SelectedValue=="2")
			{
				if(!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CB'")) //HECTOR tipo CB por DE
				{
                    Utils.MostrarAlerta(Response, "No hay documentos del tipo CB");
					prefijoDocumento.Enabled=false;
                    prefijoDocumento.Items.Clear();
                    numeroTesoreria.Text = "";
					aceptar.Enabled=false;
				}
				else
				{
                    //bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CB' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento , "%", "%", "CB");
					numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
			//Si es una remisión de cheques a financieras
			else if(tipoConsignacion.SelectedValue=="3")
			{
				if(!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='RF'"))
				{
                    Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
					prefijoDocumento.Enabled=false;
                    prefijoDocumento.Items.Clear();
                    numeroTesoreria.Text = "";
					aceptar.Enabled=false;
				}
				else
				{
                    //bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='RF' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento , "%", "%", "RF");
					numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
			//Si es una devolución de cheques de financieras
			else if(tipoConsignacion.SelectedValue=="4")
			{
				if(!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='DF'"))
				{
                    Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
					prefijoDocumento.Enabled=false;
                    prefijoDocumento.Items.Clear();
                    numeroTesoreria.Text = "";
					aceptar.Enabled=false;
				}
				else
				{
					//bind.PutDatasIntoDropDownList(prefijoDocumento,"SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='DF' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento, "%", "%", "DF");
					numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
			//Si es un traslado de fondos entre cuentas con carta
			else if((tipoConsignacion.SelectedValue=="5")||(tipoConsignacion.SelectedValue=="6"))
			{
				if(!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='TC'"))
				{
                    Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
					prefijoDocumento.Enabled=false;
                    prefijoDocumento.Items.Clear();
                    numeroTesoreria.Text = "";
					aceptar.Enabled=false;
				}
				else
				{
                    //bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='TC' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento , "%", "%", "TC");
					numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
			//Si es una nota bancaria
			else if(tipoConsignacion.SelectedValue=="7")
			{
				if(!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu IN ('ND','NR')"))
				{
                    Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
					prefijoDocumento.Enabled=false;
                    prefijoDocumento.Items.Clear();
                    numeroTesoreria.Text = "";
					aceptar.Enabled=false;
				}
				else
				{
                    bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu IN ('ND','NR') and tvig_vigencia = 'V' ");
					numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
			//Si es una entrega de cheques a proveedores
			else if(tipoConsignacion.SelectedValue=="9")
			{
				if(!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='EC'"))
				{
                    Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
					prefijoDocumento.Enabled=false;
                    prefijoDocumento.Items.Clear();
                    numeroTesoreria.Text = "";
					aceptar.Enabled=false;
				}
				else
				{
                    //bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='EC' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento, "%", "%", "EC");
					numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
            if (prefijoDocumento.Items.Count == 0)  // esta validacion de debe aplicar también a Almacenes
            {
                Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
                prefijoDocumento.Enabled = false;
                prefijoDocumento.Items.Clear();
                numeroTesoreria.Text = "";
            }
            /*
            else
            {
                prefijoDocumento.Enabled = true;
                aceptar.Enabled = true;
                if (prefijoDocumento.Items.Count > 1)
                    prefijoDocumento.Items.Insert(0, "Seleccione..");
                else
                    numeroTesoreria.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE tdoc_tipodocu='CS' AND pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
            }
            */
	
		}
		
		protected void Cambiar_Prefijo(object Sender,EventArgs e)
		{
			numeroTesoreria.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
		}
		



		protected void Aceptar_Valores(object Sender,EventArgs e)
		{
                


			if(tipoConsignacion.SelectedValue=="1")
			{
				holderConsignacionCC.Visible=true;
				holderDevolucionCC.Visible=false;
				holderRemisionFinanciera.Visible=false;
				holderDevolucionFinanciera.Visible=false;
				holderTrasladoCCCarta.Visible=false;
				holderTrasladoCCCheque.Visible=false;
				holderNotasBancarias.Visible=false;
				holderChequesProveedores.Visible=false;
			}
			else if(tipoConsignacion.SelectedValue=="2")
			{
				holderConsignacionCC.Visible=false;
				holderDevolucionCC.Visible=true;
				holderRemisionFinanciera.Visible=false;
				holderDevolucionFinanciera.Visible=false;
				holderTrasladoCCCarta.Visible=false;
				holderTrasladoCCCheque.Visible=false;
				holderNotasBancarias.Visible=false;
				holderChequesProveedores.Visible=false;
			}
			else if(tipoConsignacion.SelectedValue=="3")
			{
				holderConsignacionCC.Visible=false;
				holderDevolucionCC.Visible=false;
				holderRemisionFinanciera.Visible=true;
				holderDevolucionFinanciera.Visible=false;
				holderTrasladoCCCarta.Visible=false;
				holderTrasladoCCCheque.Visible=false;
				holderNotasBancarias.Visible=false;
				holderChequesProveedores.Visible=false;
			}
			else if(tipoConsignacion.SelectedValue=="4")
			{
				holderConsignacionCC.Visible=false;
				holderDevolucionCC.Visible=false;
				holderRemisionFinanciera.Visible=false;
				holderDevolucionFinanciera.Visible=true;
				holderTrasladoCCCarta.Visible=false;
				holderTrasladoCCCheque.Visible=false;
				holderNotasBancarias.Visible=false;
				holderChequesProveedores.Visible=false;
			}
			else if(tipoConsignacion.SelectedValue=="5")
			{
				holderConsignacionCC.Visible=false;
				holderDevolucionCC.Visible=false;
				holderRemisionFinanciera.Visible=false;
				holderDevolucionFinanciera.Visible=false;
				holderTrasladoCCCarta.Visible=true;
				holderTrasladoCCCheque.Visible=false;
				holderNotasBancarias.Visible=false;
				holderChequesProveedores.Visible=false;
			}
			else if(tipoConsignacion.SelectedValue=="6")
			{
				holderConsignacionCC.Visible=false;
				holderDevolucionCC.Visible=false;
				holderRemisionFinanciera.Visible=false;
				holderDevolucionFinanciera.Visible=false;
				holderTrasladoCCCarta.Visible=false;
				holderTrasladoCCCheque.Visible=true;
				holderNotasBancarias.Visible=false;
				holderChequesProveedores.Visible=false;
			}
			else if(tipoConsignacion.SelectedValue=="7")
			{
				holderConsignacionCC.Visible=false;
				holderDevolucionCC.Visible=false;
				holderRemisionFinanciera.Visible=false;
				holderDevolucionFinanciera.Visible=false;
				holderTrasladoCCCarta.Visible=false;
				holderTrasladoCCCheque.Visible=false;
				holderNotasBancarias.Visible=true;
				holderChequesProveedores.Visible=false;
			}
			else if(tipoConsignacion.SelectedValue=="9")
			{
				holderConsignacionCC.Visible=false;
				holderDevolucionCC.Visible=false;
				holderRemisionFinanciera.Visible=false;
				holderDevolucionFinanciera.Visible=false;
				holderTrasladoCCCarta.Visible=false;
				holderTrasladoCCCheque.Visible=false;
				holderNotasBancarias.Visible=false;
				holderChequesProveedores.Visible=true;
			}
			aceptar.Enabled=false;
		}
		
		protected void Guardar_Accion(object Sender,EventArgs e)
		{
			double valorEfectivo=0;
			Control hijo;
			DataTable tablaDatos=new DataTable();
			Consignacion miConsignacion=new Consignacion();
            string usuario = HttpContext.Current.User.Identity.Name.ToString();
			if(detalleTransaccion.Text=="")
                Utils.MostrarAlerta(Response, "Debe especificar un detalle");
			else
			{
				if(tipoConsignacion.SelectedValue=="1")  // Consignacion en Cuenta Bancaria
				{
					hijo=holderConsignacionCC.Controls[0];
					if(DatasToControls.ValidarDouble(totalEfectivo.Text))
						valorEfectivo=System.Convert.ToDouble(totalEfectivo.Text);
					tablaDatos=(DataTable)Session["tablaDatos"];
					if(tablaDatos==null && valorEfectivo==0)
                        Utils.MostrarAlerta(Response, "No hay documentos ni efectivo para consignar");
					else
					{
						miConsignacion=new Consignacion(tablaDatos);
						miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
						miConsignacion.NumeroTesoreria=System.Convert.ToInt32(this.numeroTesoreria.Text);
						miConsignacion.Almacen=this.almacen.SelectedValue;
						miConsignacion.Detalle=this.detalleTransaccion.Text;
						miConsignacion.TotalEfectivo=valorEfectivo;
						miConsignacion.TotalConsignado=Convert.ToDouble(this.valorConsignado.Text.Substring(1));
						miConsignacion.Total=valorEfectivo+System.Convert.ToDouble(this.valorConsignado.Text.Substring(1));

                           
                        //miConsignacion.RegistrarSaldoCaja(usuario, "CE", miConsignacion.Total);  //Registro de saldos de Tesoreria
						miConsignacion.CodigoCuenta=((TextBox)hijo.FindControl("codigoCC")).Text;
						miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
						miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();

                        //test
                        if (tablaDatos != null)
                        {
                            for (int j = 0; j < tablaDatos.Rows.Count; j++)
                            {
                                string usuarioFecha = DBFunctions.SingleData("select mcaj_usuario CONCAT '@' from mcaja where pdoc_codigo='" + tablaDatos.Rows[j][0] + "' and mcaj_numero=" + tablaDatos.Rows[j][1]) + miConsignacion.Fecha;
                                miConsignacion.RegistrarSaldoCaja(usuarioFecha, "CE", double.Parse(tablaDatos.Rows[j][5].ToString(), NumberStyles.Currency));  //Registro de saldos de Tesoreria
                            }
                        }

                        if (valorEfectivo > 0)
                        {
                            string usuarioFecha = miConsignacion.Usuario + '@' + miConsignacion.Fecha;
                            miConsignacion.RegistrarSaldoCaja(usuarioFecha, "CE", double.Parse(miConsignacion.TotalEfectivo.ToString(), NumberStyles.Currency));  //Registro de saldos de Tesoreria
                        }
                
						if(miConsignacion.Guardar_Consignacion())
						{
                            // contabiizacion ON LINE
                            contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                            lb.Text = miConsignacion.Mensajes;
							Session.Clear();
							Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
						}
						else
							lb.Text=miConsignacion.Mensajes;
					}
				}
				else if(tipoConsignacion.SelectedValue=="2")  // Devolución de cuenta Bancaria
				{
					hijo=holderDevolucionCC.Controls[0];
					tablaDatos=(DataTable)Session["tablaDatosDev"];
					miConsignacion=new Consignacion(tablaDatos);
					miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
					miConsignacion.NumeroTesoreria=System.Convert.ToInt32(this.numeroTesoreria.Text);
					miConsignacion.Almacen=this.almacen.SelectedValue;
					miConsignacion.Detalle=this.detalleTransaccion.Text;
					miConsignacion.Total=System.Convert.ToDouble(this.valorConsignado.Text.Substring(1))*-1;
 					//miConsignacion.CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+((DropDownList)hijo.FindControl("prefijoConsignacion")).SelectedValue+"' AND mtes_numero="+((DropDownList)hijo.FindControl("numeroConsignacion")).SelectedValue+"");
                    miConsignacion.CodigoCuenta = DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='" + Session["prefConsig"].ToString() + "' AND mtes_numero=" + Session["numConsig"].ToString() + "");
                    //miConsignacion.PrefijoConsignacion=((DropDownList)hijo.FindControl("prefijoConsignacion")).SelectedValue;
					//miConsignacion.NumeroConsignacion=Convert.ToInt32(((DropDownList)hijo.FindControl("numeroConsignacion")).SelectedValue);
                    miConsignacion.PrefijoConsignacion =Session["prefConsig"].ToString();
                    miConsignacion.NumeroConsignacion = Convert.ToInt32(Session["numConsig"].ToString());
                    miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
					miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
                    
                    miConsignacion.RegistrarSaldoCaja(usuario+"@"+ miConsignacion.Fecha, "RC", miConsignacion.Total * -1);  //Registro de saldos de Tesoreria
                    
                    if (miConsignacion.Guardar_Devolucion())
					{
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text = miConsignacion.Mensajes;
						Session.Clear();
						Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
					}
					else
						lb.Text=miConsignacion.Mensajes;
				}
				else if(tipoConsignacion.SelectedValue=="3")
				{
					hijo=this.holderRemisionFinanciera.Controls[0];
					tablaDatos=(DataTable)Session["tablaCheques"];
					miConsignacion=new Consignacion(tablaDatos);
                    //miConsignacion.RegistrarSaldoCaja(usuario, "RC", valorEfectivo);  //Registro de saldos de Tesoreria
					miConsignacion.Almacen=this.almacen.SelectedValue;
					miConsignacion.CodigoCuenta=((TextBox)hijo.FindControl("codigoCF")).Text;
					miConsignacion.Detalle=this.detalleTransaccion.Text;
					miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
					miConsignacion.Nit=((TextBox)hijo.FindControl("nitFinanciera")).Text;
					miConsignacion.NumeroTesoreria=Convert.ToInt32(this.numeroTesoreria.Text);
					miConsignacion.PrefijoConsignacion=((DropDownList)hijo.FindControl("prefijoFactura")).SelectedValue;
					miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
					miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					miConsignacion.Total=Convert.ToDouble(this.valorConsignado.Text.Substring(1));
					miConsignacion.TotalConsignado=(Convert.ToDouble(this.valorConsignado.Text.Substring(1)))-(Convert.ToDouble(this.totalEfectivo.Text.Substring(1)));
					if(miConsignacion.Guardar_Remision())
					{
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoConsignacion.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text = miConsignacion.Mensajes;
						Session.Clear();
						Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
					}
					else
						lb.Text=miConsignacion.Mensajes;
				}
				else if(tipoConsignacion.SelectedValue=="4")
				{
					hijo=this.holderDevolucionFinanciera.Controls[0];
					tablaDatos=(DataTable)Session["tablaDatos"];
					miConsignacion=new Consignacion(tablaDatos);
                    //miConsignacion.RegistrarSaldoCaja(usuario, "CE", valorEfectivo);  //Registro de saldos de Tesoreria
					miConsignacion.Almacen=this.almacen.SelectedValue;
					miConsignacion.CodigoCuenta=((TextBox)hijo.FindControl("tbCuenta")).Text;
					miConsignacion.Detalle=this.detalleTransaccion.Text;
					miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
					miConsignacion.Nit=((TextBox)hijo.FindControl("tbNitFin")).Text;
					miConsignacion.NumeroTesoreria=Convert.ToInt32(this.numeroTesoreria.Text);
					miConsignacion.PrefijoConsignacion=((DropDownList)hijo.FindControl("ddlPrefijo")).SelectedValue;
					miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
					miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					miConsignacion.Total=Convert.ToDouble(this.valorConsignado.Text.Substring(1));
					if(miConsignacion.Guardar_DevFinanciera())
					{
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoConsignacion.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text = miConsignacion.Mensajes;
						Session.Clear();
						Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
					}
					else
						lb.Text=miConsignacion.Mensajes;
				}
				else if(tipoConsignacion.SelectedValue=="5")
				{
					hijo=holderTrasladoCCCarta.Controls[0];
					miConsignacion=new Consignacion();
					miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
					miConsignacion.NumeroTesoreria=Convert.ToInt32(this.numeroTesoreria.Text);
					miConsignacion.Almacen=this.almacen.SelectedValue;
					miConsignacion.Detalle=this.detalleTransaccion.Text;
					miConsignacion.TotalConsignado=Convert.ToDouble(this.valorConsignado.Text.Substring(1))+Convert.ToDouble(this.totalEfectivo.Text.Substring(1));
					miConsignacion.Total=Convert.ToDouble(this.valorConsignado.Text.Substring(1));
					miConsignacion.CodigoCuenta=((TextBox)hijo.FindControl("codigoCCO")).Text;
					miConsignacion.CodigoCuentaDestino=((TextBox)hijo.FindControl("codigoCCD")).Text;
					miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
					miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					if(((TextBox)hijo.FindControl("tbSobregiro")).Text!="")
						miConsignacion.AutorizacionSobregiro=((TextBox)hijo.FindControl("tbSobregiro")).Text;
					if(miConsignacion.Guardar_Transferencia())
					{
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text = miConsignacion.Mensajes;
						Session.Clear();
						Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
					}
					else
						lb.Text=miConsignacion.Mensajes;
				}
				else if(tipoConsignacion.SelectedValue=="6")
				{
					hijo=holderTrasladoCCCheque.Controls[0];
					miConsignacion=new Consignacion();
					tablaDatos=(DataTable)Session["tablaDatos"];
					miConsignacion=new Consignacion(tablaDatos);
					miConsignacion.Almacen=this.almacen.SelectedValue;
					miConsignacion.CodigoCuenta=((TextBox)hijo.FindControl("codigoCCO")).Text;
					miConsignacion.CodigoCuentaDestino=((TextBox)hijo.FindControl("codigoCCD")).Text;
					miConsignacion.Detalle=this.detalleTransaccion.Text;
					miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
					miConsignacion.NumeroTesoreria=Convert.ToInt32(this.numeroTesoreria.Text);
					miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
					miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					miConsignacion.Total=Convert.ToDouble(this.valorConsignado.Text.Substring(1))+Convert.ToDouble(this.totalEfectivo.Text.Substring(1));
					if(miConsignacion.Guardar_Transferencia_Cheque())
					{
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text = miConsignacion.Mensajes;
						Session.Clear();
						Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
					}
					else
						lb.Text=miConsignacion.Mensajes;
				}
				else if(tipoConsignacion.SelectedValue=="7")
				{
					hijo=this.holderNotasBancarias.Controls[0];
					miConsignacion=new Consignacion();
					tablaDatos=(DataTable)Session["tablaNotas"];
					miConsignacion=new Consignacion(tablaDatos);
					miConsignacion.Almacen=this.almacen.SelectedValue;
					miConsignacion.CodigoCuenta=((TextBox)hijo.FindControl("codigoCC")).Text;
					miConsignacion.Detalle=this.detalleTransaccion.Text;
					miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
					miConsignacion.NumeroTesoreria=Convert.ToInt32(this.numeroTesoreria.Text);
					miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
					miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					//Si es una nota debito
                    double valorConsig = double.Parse(valorConsignado.Text, NumberStyles.Currency);
					if((DBFunctions.SingleData("SELECT tdoc_tipodocu FROM dbxschema.pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'"))=="ND")
						miConsignacion.Total=valorConsig;
						//Si es una nota credito
					else if((DBFunctions.SingleData("SELECT tdoc_tipodocu FROM dbxschema.pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'"))=="NR")
						    miConsignacion.Total=valorConsig*-1;
					if(miConsignacion.Guardar_Nota())
					{
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text += miConsignacion.Mensajes;
						Session.Clear();
						Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
					}
					else
						lb.Text+=miConsignacion.Mensajes;
				}
				else if(tipoConsignacion.SelectedValue=="9")
				{
					hijo=holderChequesProveedores.Controls[0];
					miConsignacion=new Consignacion();
					tablaDatos=(DataTable)Session["tablaDatos"];
					miConsignacion=new Consignacion(tablaDatos);
					miConsignacion.Almacen=this.almacen.SelectedValue;
					miConsignacion.CodigoCuenta=((TextBox)hijo.FindControl("codigoCC")).Text;
					miConsignacion.Detalle=this.detalleTransaccion.Text;
					miConsignacion.Fecha=((TextBox)hijo.FindControl("fecha")).Text;
					miConsignacion.NumeroTesoreria=Convert.ToInt32(this.numeroTesoreria.Text);
					miConsignacion.PrefijoDocumento=this.prefijoDocumento.SelectedValue;
					miConsignacion.Proceso=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					miConsignacion.Usuario=HttpContext.Current.User.Identity.Name.ToLower();
					miConsignacion.Total=Convert.ToDouble(this.valorConsignado.Text.Substring(1))+Convert.ToDouble(this.totalEfectivo.Text.Substring(1));
					if(miConsignacion.Guardar_Entrega())
					{
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text = miConsignacion.Mensajes;
						Session.Clear();
						Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques&prefD="+miConsignacion.PrefijoDocumento+"&numD="+miConsignacion.NumeroTesoreria+"");
					}
					else
						lb.Text=miConsignacion.Mensajes;
				}
			}
		}
		
		protected void Cancelar_Accion(object Sender,EventArgs e)
		{
			Session.Clear();
			Response.Redirect(pathToMain+"?process=Tesoreria.ConsignaCheques");
		}
		
		////////////////////////////////////////////////
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
