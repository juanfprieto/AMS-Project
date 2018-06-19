using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial  class ReporteOrdenSalida : System.Web.UI.UserControl
	{
		protected DataTable dtOrdSal,dtFacturas;
		
		protected void Page_Load(object Sender, EventArgs e)
		{
            if (!IsPostBack && Request.QueryString["error"]==null)
			{
				if(Session["dtVehiculos"]!=null)
				{
					this.Llenar_dtOrdSal();
					lbInfo.Text="Se autoriza la salida del vehículo";
					if(Request["mens"]!=null && Request["mens"]=="1")
						lbCond.Text="NO SE CALCULO CARTERA PARA LA EMISION DE ESTA ORDEN DE SALIDA";
					lbProc.Text="Fecha Y Hora de Procesado "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}
				else
                Utils.MostrarAlerta(Response, "Error Interno. Reinicie el proceso");
			}
			else if(!IsPostBack && Request.QueryString["error"]!=null)
			{
				this.Llenar_dtOrdSal();
				this.Llenar_dtFacturas();
				lbInfo.Text="No se da la autorización de orden de salida porque el cliente tiene las siguientes facturas pendientes de pago : ";
				lbProc.Text="Fecha Y Hora de Procesado "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
            string urlIconoEmpresa = DBFunctions.SingleDataGlobal("SELECT GEMP_iCONO FROM GEMPRESA WHERE GEMP_NOMBRE='" + GlobalData.getEMPRESA() + "';");
            ImgLogo.ImageUrl = urlIconoEmpresa;
            Session["rep"]=RenderHtml();
            //traer logo dinàmicamente
            
		}
		
		protected void Cargar_dtOrdSal()
		{
			dtOrdSal=new DataTable();
			dtOrdSal.Columns.Add("CC",typeof(string));
			dtOrdSal.Columns.Add("NOMBRE",typeof(string));
			dtOrdSal.Columns.Add("CATALOGO",typeof(string));
			dtOrdSal.Columns.Add("VIN",typeof(string));
			dtOrdSal.Columns.Add("PLACA",typeof(string));
			dtOrdSal.Columns.Add("MOTOR",typeof(string));
			dtOrdSal.Columns.Add("COLOR",typeof(string));
		}
		
		protected void Cargar_dtFacturas()
		{
			dtFacturas=new DataTable();
			dtFacturas.Columns.Add("PREFIJO",typeof(string));
			dtFacturas.Columns.Add("NUMERO",typeof(string));
			dtFacturas.Columns.Add("VALOR",typeof(double));
		}
		
		protected void Llenar_dtFacturas()
		{
			DataTable temp=new DataTable();
			DataRow fila;
			temp=(DataTable)Session["error"];
			for(int i=0;i<temp.Rows.Count;i++)
			{
				if(temp.Rows[i][2].ToString()=="F")
				{
					if(Session["dtFacturas"]==null)
						this.Cargar_dtFacturas();
					fila=dtFacturas.NewRow();
					fila[0]=temp.Rows[i][0].ToString();
					fila[1]=temp.Rows[i][1].ToString();
					fila[2]=Convert.ToDouble(temp.Rows[i][3]);
					dtFacturas.Rows.Add(fila);
					dgFacturas.DataSource=dtFacturas;
					dgFacturas.DataBind();
					Session["dtFacturas"]=dtFacturas;
				}
			}
		}
		
		protected void Llenar_dtOrdSal()
		{
			dtOrdSal=(DataTable)Session["dtVehiculos"];
			for(int i=0;i<dtOrdSal.Rows.Count;i++)
			{
			if(!Convert.ToBoolean(dtOrdSal.Rows[i][9]))
					dtOrdSal.Rows[i].Delete();
			}
			dgOrdSal.DataSource=dtOrdSal;
			dgOrdSal.DataBind();
			Session["dtVehiculos"]=dtOrdSal;
		}
		
		protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
        	phInfo.RenderControl(htmlTW);
        	return SB.ToString();
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
            lb.Text = "";
            //MailMessage MyMail = new MailMessage();
            //		  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
            //  		MyMail.To = tbEmail.Text;
            //MyMail.Subject = "Proceso : Orden Salida Vehiculo";
            //MyMail.Body = (RenderHtml());
            //   		MyMail.BodyFormat = MailFormat.Html;
            //try{
            // 	   		SmtpMail.Send(MyMail);}
            // 		catch(Exception e){
            // 	 	  lb.Text = e.ToString();
            // 		}
            string result = "";
            if (tbEmail.Text == "")
            {
                Utils.MostrarAlerta(Response, "Debe de ingresar un correo. Revise por favor");
                return;
            }
            try
            {
                result = Tools.AMS_Tools_Email.enviarMail(tbEmail.Text, "Ha Recibido un Reporte: " + "ORDEN DE SALIDA DE VEHICULO", RenderHtml(), TipoCorreo.HTML, "");
                if (result == "")
                {
                    Utils.MostrarAlerta(Response, "Email con Reporte ha sido enviado correctamente a: " + tbEmail.Text);
                }
                else
                    lb.Text = result;
            }
            catch (Exception z)
            {
                lb.Text = z.Message;
            }
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
