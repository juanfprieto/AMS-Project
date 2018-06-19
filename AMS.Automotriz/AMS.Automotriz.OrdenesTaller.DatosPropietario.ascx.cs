// created on 20/09/2004 at 15:22
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
using AMS.Tools;
using AMS.Forms;
using Ajax;
using AMS.Automotriz;

// Se debe modificar el javascript para poder cargar todos los datos del usuario cuando se de click en 
// la imagen dentro del control.
// Se debe seguir con la idea que genero Daniel sobre el nombre de los TextBox.

namespace AMS.Automotriz
{
	public partial class DatosPropietario : System.Web.UI.UserControl
	{  
		#region Atributos
        protected DropDownList datosd, tipid;
        #endregion
		protected System.Web.UI.WebControls.RequiredFieldValidator validatorDatosd;
		string strDatosC,strDatosE,strDatosF,strDatosG,strDatosH;
        DatosVehiculo datosVehiculo;
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Automotriz.DatosPropietario));// cambio realizado 1 febrero
			strDatosC=Request.Form[datosc.UniqueID];
			strDatosE=Request.Form[datose.UniqueID];
			strDatosF=Request.Form[datosf.UniqueID];
			strDatosG=Request.Form[datosg.UniqueID];
			strDatosH=Request.Form[datosh.UniqueID];
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoRadioButtonList(tipoCliente,"SELECT tpro_codigo, tpro_nombre FROM tpropietariotaller");
				bind.PutDatasIntoDropDownList(tipoPago,"SELECT ttip_codigo ,ttip_nombre FROM dbxschema.ttipopago where ttip_codigo<>'DL' and ttip_codigo <> 'DC'");
                tipoPago.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(datosd,"SELECT pciu_codigo AS CODIGO,pciu_nombre AS CIUDAD FROM dbxschema.pciudad ORDER BY pciu_nombre asc");
                datosd.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(tipid, "SELECT TNIT_TIPONIT, TNIT_NOMBRE from TNIT order by 2");
                tipid.Items.Insert(0,  "");
                DatasToControls.EstablecerDefectoRadioButtonList(tipoCliente,"Propietario");
				DatasToControls.EstablecerDefectoDropDownList(tipoPago,"Efectivo");
			}
		}
		[Ajax.AjaxMethod]
		public DataSet BuscarCedula(string Cedula)
		{
			DataSet Vins= new DataSet();
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mnit_nit from dbxschema.mnit where mnit_nit like '"+Cedula+"%';select mnit_nombres || ' ' || coalesce(mnit_nombre2,'') as NOMBRE, mnit_apellidos || ' ' || coalesce(mnit_apellido2,'') as APELLIDOS,mnit_direccion as DIRECCION,mnit_telefono as TELEFONO,mnit_celular as CELULAR,mnit_email as EMAIL,mnit_web as WEB,pciu_codigo as CIUDAD from dbxschema.mnit where mnit_nit='"+Cedula+"'");
			return Vins;
			
		}
		public void Confirmar(Object  Sender, EventArgs e)
		{
            Control principal = (this.Parent).Parent;//Control principal ascx
			//Aqui vamos a realizar la actualizacion de datos si es necesaria
			string numeroIdentificacion = datos.Text.ToString();
            DataSet datosPropietario = new DataSet();
            DBFunctions.Request(datosPropietario, IncludeSchema.NO, "select * FROM mnit WHERE mnit_nit='" + numeroIdentificacion + "' ");

            string update = "UPDATE mnit SET";
			int primero = 1;
            bool error = false;

            if (datosPropietario.Tables.Count > 0)
            {
                if ((datosc.Text.Trim()) != datosPropietario.Tables[0].Rows[0]["MNIT_DIRECCION"].ToString()) //(DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit='" + numeroIdentificacion + "'")))
                {
                    if (primero == 1)
                    {
                        update += " mnit_direccion='" + datosc.Text.Trim() + "'";
                        primero += 1;
                    }
                    else
                        update += ", mnit_direccion='" + datosc.Text.Trim() + "'";

                }
                if (datosd.SelectedValue.ToString() != datosPropietario.Tables[0].Rows[0]["PCIU_CODIGO"].ToString())  //Functions.SingleData("SELECT pciu_codigo FROM mnit WHERE mnit_nit='" + numeroIdentificacion + "'")))
                {
                    if (primero == 1)
                    {
                        update += " pciu_codigo='" + datosd.SelectedValue + "'";
                        primero += 1;
                    }
                    else
                        update += ", pciu_codigo='" + datosd.SelectedValue + "'";
                }
                if ((strDatosE.Trim()) != datosPropietario.Tables[0].Rows[0]["MNIT_TELEFONO"].ToString())  //DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='" + numeroIdentificacion + "'")))
                {
                    if (primero == 1)
                    {
                        update += " mnit_telefono='" + strDatosE.Trim() + "'";
                        primero += 1;
                    }
                    else
                        update += ", mnit_telefono='" + strDatosE.Trim() + "'";
                }
                if ((strDatosF.Trim()) != datosPropietario.Tables[0].Rows[0]["MNIT_CELULAR"].ToString())  //(DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='" + numeroIdentificacion + "'")))
                {
                    if (primero == 1)
                    {
                        update += " mnit_celular='" + strDatosF.Trim() + "'";
                        primero += 1;
                    }
                    else
                        update += ", mnit_celular='" + strDatosF.Trim() + "'";
                }
                if ((strDatosG.Trim()) != datosPropietario.Tables[0].Rows[0]["MNIT_EMAIL"].ToString())  //(DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='" + numeroIdentificacion + "'")))
                {
                    if (primero == 1)
                    {
                        update += " mnit_email='" + strDatosG.Trim() + "'";
                        primero += 1;
                    }
                    else
                        update += ", mnit_email='" + strDatosG.Trim() + "'";
                }
                if ((strDatosH.Trim()) != datosPropietario.Tables[0].Rows[0]["MNIT_WEB"].ToString())  //(DBFunctions.SingleData("SELECT mnit_web FROM mnit WHERE mnit_nit='" + numeroIdentificacion + "'")))
                {
                    if (primero == 1)
                    {
                        update += " mnit_web='" + strDatosH.Trim() + "'";
                        primero += 1;
                    }
                    else
                        update += ", mnit_web='" + strDatosH.Trim() + "'";
                }
                if (primero > 1)
                {
                    update += " WHERE mnit_nit='" + numeroIdentificacion + "'";
                    ArrayList updateArray = new ArrayList();
                    updateArray.Add(update);
                    if (DBFunctions.Transaction(updateArray))
                        lb.Text = "<br>registro actualizado";
                    else
                    {
                        lb.Text = DBFunctions.exceptions;
                        error = true;
                    }
                }
                string bloqueado = datosPropietario.Tables[0].Rows[0]["TVIG_VIGENCIA"].ToString(); // DBFunctions.SingleData("SELECT TVIG_VIGENCIA FROM MNIT WHERE MNIT_NIT = '" + numeroIdentificacion + "' ");
                if (bloqueado == "B" || bloqueado == "C")
                {
                    lb.Text += "Este cliente está BLoqueado, favor comunicarse con Cartera";
                    Utils.MostrarAlerta(Response, "Este cliente está BLoqueado, favor comunicarse con Cartera");
                    error = true;
                }
            }
			if(!error)
			{
				//vamos a ocultar este control y a mostrar el siguiente, ademas activar el boton	
                if (numeroIdentificacion != "")
                {
                    HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
                    hdTabIndex.Value = "2";
                }
                
                //datosVehiculo.FindControl("confirmar").EnableTheming = true;
                //datosVehiculo.FindControl("confirmar").EnableViewState = true;
				//((PlaceHolder)this.Parent).Visible = false;
				//((PlaceHolder)principal.FindControl("datosVehiculo")).Visible = true;
				//((ImageButton)principal.FindControl("vehiculo")).Enabled = true;
				//((ImageButton)principal.FindControl("propietario")).ImageUrl="../img/AMS.BotonExpandir.png";
				//((ImageButton)principal.FindControl("vehiculo")).ImageUrl="../img/AMS.BotonContraer.png";			
			}
            
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
