using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.UI;
using AMS.DB;
using AMS.Forms;
using System.Web.Mail;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using AMS.Tools;
using System.Threading;

namespace AMS.Nomina
{
	public class ImpComprobantesPago : System.Web.UI.UserControl
	{
        protected DataSet ds = new DataSet();
		protected TextBox tbEmail;
        protected Label lbError;
		protected DropDownList DDLEMPLEADO;
		protected PlaceHolder phcomprobantepago;
		protected DropDownList DDLQUINCENA;
		protected Button BTNGENERAR;
		string main=ConfigurationManager.AppSettings["PathToControls"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DatasToControls param = new DatasToControls();
                param.PutDatasIntoDropDownList(DDLQUINCENA, "select mqui_codiquin,cast (mqui_codiquin as char(4)) concat '-' concat cast (mqui_anoquin as char(4)) concat '-' concat CAST(mqui_mesquin AS char(4)) CONCAT '-' CONCAT CAST(mqui_tpernomi AS char(2)) from dbxschema.mquincenas where mqui_estado=2");
                param.PutDatasIntoDropDownList(DDLEMPLEADO, "SELECT M.MEMP_CODIEMPL, N.MNIT_APELLIDOS CONCAT ' ' CONCAT N.MNIT_APELLIDO2 CONCAT ' ' CONCAT N.MNIT_NOMBRES FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado='1' order by N.MNIT_APELLIDOS");
            }
            phcomprobantepago.Controls.Add(LoadControl(main + "AMS.Nomina.FormatoComprobantePago.ascx"));
            
        }
        protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
        	phcomprobantepago.RenderControl(htmlTW);
        	return SB.ToString();
		}
        public void enviarATodos(Object Sender, ImageClickEventArgs E) 
        {
            lbError.Text = "";
            Control uno = phcomprobantepago.Controls[0];

            string enviado = "";
            string noEnviado = "";
            string fUsuario = "";
            bool falloEnvio = false;
            DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT M.MEMP_CODIEMPL as CODIGO, N.MNIT_APELLIDOS CONCAT ' ' CONCAT N.MNIT_APELLIDO2 CONCAT ' ' CONCAT N.MNIT_NOMBRES as NOMBRE, N.MNIT_EMAIL
            FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado='1' order by N.MNIT_APELLIDOS");
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                string email = ds.Tables[0].Rows[j]["MNIT_EMAIL"].ToString();
                bool esCorreo = email.Contains("@");

                if (ds.Tables[0].Rows[j]["MNIT_EMAIL"].ToString() == "" || esCorreo == false)
                {
                    noEnviado += ds.Tables[0].Rows[j]["NOMBRE"] + " - ";
                    falloEnvio = true;
                }
            }

            //Revisamos que existan correos para todos lo empleados, o no se enviará nada :)
            if (falloEnvio == true || noEnviado != "")
            {
                noEnviado += "@";
                noEnviado = noEnviado.Replace("- @", "");
                Utils.MostrarAlerta(Response, "Falta o está mal escrito el correo de los siguientes empleados: " + noEnviado + " Revise por favor..");
                return;
            }

            //se recorren los empleados y se envia correo a cada uno
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string mailEmp = ds.Tables[0].Rows[i]["MNIT_EMAIL"].ToString();
                Session["codEmp"] = ((String)ds.Tables[0].Rows[i]["CODIGO"]);

                generar(Sender, E);

                try
                {
                    
                    Tools.Utils.EnviarMail(mailEmp, "Ha Recibido un Reporte: " + "Comprobantes de pago", RenderHtml(), TipoCorreo.HTML, "");
                    Thread.Sleep(500);
                    enviado += mailEmp + " - ";

                } catch (Exception e) //Si existen todos los correos pero uno específico falló en el envio, se siguen enviando a todos los correos
                {
                    fUsuario += mailEmp + " - "; //correo al q le falló el envio se va acumulando
                    lbError.Text += e.Message + " - ";
                }
            }
            if (lbError.Text.Length > 0)
            {
                lbError.Text += "@";
                lbError.Text = lbError.Text.Replace("- @", "");
            }

            if(fUsuario.Length > 0)
            {
                fUsuario += "@";
                fUsuario = fUsuario.Replace("- @", "");
                Utils.MostrarAlerta(Response, "Ha fallado el envio de los siguientes correos: " + fUsuario);
            }
            if (enviado != "")
            {
                enviado += "@";
                enviado = enviado.Replace("- @", "");
                Utils.MostrarAlerta(Response, "Email con Reporte ha sido enviado correctamente a: " + enviado);
            }
            phcomprobantepago.Visible = false;
            Session["codEmp"] = null;
        }

		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
            string result = "";
            if (tbEmail.Text == "")
            {
                Utils.MostrarAlerta(Response, "Debe de ingresar un correo. Revise por favor");
                return;
            }
            try
            {
                result = Tools.AMS_Tools_Email.enviarMail(tbEmail.Text, "Ha Recibido un Reporte: " + "Comprobantes de pago", RenderHtml(), TipoCorreo.HTML, "");
                 if (result == "")
                {
                    Utils.MostrarAlerta(Response, "Email con Reporte ha sido enviado correctamente a: " + tbEmail.Text);
                }
                else
                    lbError.Text = result;
            }catch(Exception z)
            {
                lbError.Text = z.Message;
            }
            
            
            //MailMessage MyMail = new MailMessage();
            //MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
            //MyMail.To = tbEmail.Text;
            //MyMail.Subject = "Proceso : Comprobantes de Pago";
            //MyMail.Body = (RenderHtml());
            //MyMail.BodyFormat = MailFormat.Html;
            //try{
            //    SmtpMail.Send(MyMail);}
            //catch{
            //  //lbInfo.Text = e.ToString();
            //}
		}
		
		
		protected void generar(object sender,EventArgs e)
		{
            
			double subtotalapagar=0,subtotaladescontar=0,neto=0; 
			int i;
			Control uno = phcomprobantepago.Controls[0];
			DataSet dquincena = new DataSet();
			DataSet datosempleado = new DataSet();
			DataSet mquincena = new DataSet();

            if (Session["codEmp"] != null)
            {
                DBFunctions.Request(mquincena, IncludeSchema.NO, "select mqui_anoquin,mqui_mesquin,mqui_tpernomi from dbxschema.mquincenas where mqui_codiquin=" + DDLQUINCENA.SelectedValue + " ");
                DBFunctions.Request(datosempleado, IncludeSchema.NO, "select M.mnit_nombres,M.mnit_apellidos,M.mnit_nit,E.memp_suelactu from dbxschema.mempleado E, dbxschema.mnit M where E.memp_codiempl='" + Session["codEmp"] + "' and E.mnit_nit=M.mnit_nit ");
                DBFunctions.Request(dquincena, IncludeSchema.NO, "select PCON.pcon_concepto concat ' ' concat PCON.pcon_nombconc as CONCEPTO,DQUI.dqui_valevento AS VALOR_EVENTO,DQUI.dqui_canteventos AS CANTIDAD,DQUI.dqui_apagar AS DEVENGADOS,DQUI.dqui_adescontar AS DEDUCCIONES,DQUI.dqui_saldo AS SALDO from dbxschema.dquincena DQUI , dbxschema.pconceptonomina PCON where  DQUI.pcon_concepto=PCON.pcon_concepto  and DQUI.memp_codiempl='" + Session["codEmp"] + "' and DQUI.mqui_codiquin=" + DDLQUINCENA.SelectedValue + "  ");
                ((DataGrid)uno.FindControl("DATAGRIDPAGO")).DataSource = dquincena.Tables[0];
                ((Label)uno.FindControl("LBCODIGO")).Text = (String)Session["codEmp"];
                ((Label)uno.FindControl("LBNOMBRE")).Text = datosempleado.Tables[0].Rows[0][0].ToString() + " " + datosempleado.Tables[0].Rows[0][1].ToString();
                ((Label)uno.FindControl("LBCEDULA")).Text = datosempleado.Tables[0].Rows[0][2].ToString();
                ((Label)uno.FindControl("LBFECHA")).Text = mquincena.Tables[0].Rows[0][0].ToString() + "-" + mquincena.Tables[0].Rows[0][1].ToString() + "-" + mquincena.Tables[0].Rows[0][2].ToString();
                ((Label)uno.FindControl("LBSUELDO")).Text = double.Parse(datosempleado.Tables[0].Rows[0][3].ToString()).ToString("n");           
            }
            else
            {
                DBFunctions.Request(mquincena, IncludeSchema.NO, "select mqui_anoquin,mqui_mesquin,mqui_tpernomi from dbxschema.mquincenas where mqui_codiquin=" + DDLQUINCENA.SelectedValue + " ");
                DBFunctions.Request(datosempleado, IncludeSchema.NO, "select M.mnit_nombres,M.mnit_apellidos,M.mnit_nit,E.memp_suelactu from dbxschema.mempleado E, dbxschema.mnit M where E.memp_codiempl='" + DDLEMPLEADO.SelectedValue + "' and E.mnit_nit=M.mnit_nit ");
                DBFunctions.Request(dquincena, IncludeSchema.NO, "select PCON.pcon_concepto concat ' ' concat PCON.pcon_nombconc as CONCEPTO,DQUI.dqui_valevento AS VALOR_EVENTO,DQUI.dqui_canteventos AS CANTIDAD,DQUI.dqui_apagar AS DEVENGADOS,DQUI.dqui_adescontar AS DEDUCCIONES,DQUI.dqui_saldo AS SALDO from dbxschema.dquincena DQUI , dbxschema.pconceptonomina PCON where  DQUI.pcon_concepto=PCON.pcon_concepto  and DQUI.memp_codiempl='" + DDLEMPLEADO.SelectedValue + "' and DQUI.mqui_codiquin=" + DDLQUINCENA.SelectedValue + "  ");
                ((DataGrid)uno.FindControl("DATAGRIDPAGO")).DataSource = dquincena.Tables[0];
                ((Label)uno.FindControl("LBCODIGO")).Text = DDLEMPLEADO.SelectedValue.ToString();
                ((Label)uno.FindControl("LBNOMBRE")).Text = datosempleado.Tables[0].Rows[0][0].ToString() + " " + datosempleado.Tables[0].Rows[0][1].ToString();
                ((Label)uno.FindControl("LBCEDULA")).Text = datosempleado.Tables[0].Rows[0][2].ToString();
                ((Label)uno.FindControl("LBFECHA")).Text = mquincena.Tables[0].Rows[0][0].ToString() + "-" + mquincena.Tables[0].Rows[0][1].ToString() + "-" + mquincena.Tables[0].Rows[0][2].ToString();
                ((Label)uno.FindControl("LBSUELDO")).Text = double.Parse(datosempleado.Tables[0].Rows[0][3].ToString()).ToString("n");
            }
			//SACAR LOS SUBTOTALES Y EL NETO
			for (i=0;i<dquincena.Tables[0].Rows.Count;i++)
			{
				subtotalapagar+=double.Parse(dquincena.Tables[0].Rows[i][3].ToString());
				subtotaladescontar+=double.Parse(dquincena.Tables[0].Rows[i][4].ToString());
				
			}
			neto=subtotalapagar-subtotaladescontar;
			((Label)uno.FindControl("LBSUBTP")).Text=subtotalapagar.ToString("c");
			((Label)uno.FindControl("LBSUBTD")).Text=subtotaladescontar.ToString("c");
			((Label)uno.FindControl("LBNETO")).Text=neto.ToString("c");
			((DataGrid)uno.FindControl("DATAGRIDPAGO")).DataBind();
            try
            {
                ((System.Web.UI.WebControls.Image)uno.FindControl("imgLogo1")).ImageUrl = "http://ecas.co/images/" + GlobalData.EMPRESA.ToLower() + ".png";
                ((System.Web.UI.WebControls.Image)uno.FindControl("imgLogo1")).Visible = true;
            }
            catch(Exception z)
            {
                ((System.Web.UI.HtmlControls.HtmlImage)uno.FindControl("imgLogo")).Src = "http://ecas.co/images/" + GlobalData.EMPRESA.ToLower() + ".png";
                ((System.Web.UI.HtmlControls.HtmlImage)uno.FindControl("imgLogo")).Visible = true;
            }
            
            phcomprobantepago.Visible = true;
       
			Session["rep"]=RenderHtml();
			
		}
		
		
			
		
		
		
		//protected HtmlInputFile fDocument;
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	
		
	}
	
	
}

