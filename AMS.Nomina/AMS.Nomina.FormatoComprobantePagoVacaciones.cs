using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web.Mail;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Nomina
{

	public class FormatoComprobantePagoVacaciones : System.Web.UI.UserControl
	{
		
		protected TextBox tbEmail;
		protected Label LBCODIGO,LBNOMBRE,LBCEDULA,LBFECHA,LBSUELDO,LBSUBTP,LBSUBTD,LBNETO;
		protected Label LBDTVACACIONES,LBVACAAPAGAR,LBPERIODO,LBDIASEFECTIVOS;
		protected DataGrid DATAGRIDPAGOVACACIONES;
		protected PlaceHolder phFormatoPagoVacaciones;
		string main=ConfigurationManager.AppSettings["PathToControls"];
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected DataTable cambio;
		
		protected void Page_Load(object sender , EventArgs e)
		{
			double subtotalapagar=0,subtotaladescontar=0,neto=0;
			if (Session["DataTable1"]!=null)
			{
				this.Llenar_Tabla();
				DATAGRIDPAGOVACACIONES.DataSource=cambio;
				DATAGRIDPAGOVACACIONES.DataBind();
				
			}
			DataSet datosempleado= new DataSet();
			DataSet mquincena= new DataSet();
			
			//DBFunctions.Request(datosempleado,IncludeSchema.NO,"select M.mnit_nombres,M.mnit_apellidos,M.mnit_nit,E.memp_suelactu from dbxschema.mempleado E, dbxschema.mnit M where E.memp_codiempl='"+DDLEMPLEADO.SelectedValue+"' and E.mnit_nit=M.mnit_nit ");
			
			LBDTVACACIONES.Text=Request.QueryString["DIAS"];
			LBVACAAPAGAR.Text=Request.QueryString["VALORVACA"];
			LBPERIODO.Text=Request.QueryString["FECHA"].Replace('*','-');
			LBPERIODO.Visible=true;
			LBDIASEFECTIVOS.Text=Request.QueryString["DIASEFECTIVOS"];
			DBFunctions.Request(datosempleado,IncludeSchema.NO,"select M.mnit_nombres,M.mnit_apellidos,M.mnit_nit,E.memp_suelactu from dbxschema.mempleado E, dbxschema.mnit M where E.memp_codiempl='"+(string)Session["codigoempleado"]+"' and E.mnit_nit=M.mnit_nit ");
			string codquincena=DBFunctions.SingleData("select max(mqui_codiquin) from mquincenas");
			DBFunctions.Request(mquincena,IncludeSchema.NO,"select mqui_anoquin,mqui_mesquin,mqui_tpernomi from dbxschema.mquincenas where mqui_codiquin="+codquincena+" ");
			LBCODIGO.Text=(string)Session["codigoempleado"];
			LBNOMBRE.Text=datosempleado.Tables[0].Rows[0][0].ToString() + " " +  datosempleado.Tables[0].Rows[0][1].ToString();
			LBCEDULA.Text=datosempleado.Tables[0].Rows[0][2].ToString();
			LBFECHA.Text=mquincena.Tables[0].Rows[0][0].ToString()+ "-" + mquincena.Tables[0].Rows[0][1].ToString() + "-" + mquincena.Tables[0].Rows[0][2].ToString()  ;
			LBSUELDO.Text=double.Parse(datosempleado.Tables[0].Rows[0][3].ToString()).ToString("n");
			//SACAR LOS SUBTOTALES Y EL NETO
			for (int i=0;i<cambio.Rows.Count;i++)
			{
				if(cambio.Rows[i][0].ToString()=="--")
				{
					subtotalapagar+=double.Parse(cambio.Rows[i][4].ToString());
					subtotaladescontar+=double.Parse(cambio.Rows[i][5].ToString());
				}
			}
			neto=subtotalapagar-subtotaladescontar;
			LBSUBTP.Text=subtotalapagar.ToString("c");
			LBSUBTD.Text=subtotaladescontar.ToString("c");
			LBNETO.Text=neto.ToString("c");
			
			Session["rep"]= RenderHtml();
						
		}
		
		protected void Preparar_Tabla()
		{
			cambio=new DataTable();
			cambio.Columns.Add("CONCEPTO",typeof(string));
			cambio.Columns.Add("DESCRIPCION",typeof(string));
			cambio.Columns.Add("CANT EVENTOS",typeof(string));
			cambio.Columns.Add("VALOR EVENTO",typeof(double));
			cambio.Columns.Add("A PAGAR",typeof(double));			
			cambio.Columns.Add("A DESCONTAR",typeof(double));
			cambio.Columns.Add("TIPO EVENTO",typeof(string));
			cambio.Columns.Add("SALDO",typeof(double));
			cambio.Columns.Add("DOC REFERENCIA",typeof(string));
			
		}
			
		protected void Llenar_Tabla()
		{
			DataRow fila;
			if(cambio==null)
				this.Preparar_Tabla();
			for(int i=0;i<((DataTable)Session["DataTable1"]).Rows.Count;i++)
			{
				fila=cambio.NewRow();
				fila[0]=((DataTable)Session["DataTable1"]).Rows[i][0].ToString();
				fila[1]=((DataTable)Session["DataTable1"]).Rows[i][8].ToString();
				fila[2]=((DataTable)Session["DataTable1"]).Rows[i][1].ToString();
				fila[3]=((DataTable)Session["DataTable1"]).Rows[i][2].ToString();
				fila[4]=((DataTable)Session["DataTable1"]).Rows[i][3].ToString();
				fila[5]=((DataTable)Session["DataTable1"]).Rows[i][4].ToString();
				fila[6]=((DataTable)Session["DataTable1"]).Rows[i][5].ToString();
				fila[7]=((DataTable)Session["DataTable1"]).Rows[i][6].ToString();
				fila[8]=((DataTable)Session["DataTable1"]).Rows[i][7].ToString();
				cambio.Rows.Add(fila);
			}
		}
		
		protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
        	phFormatoPagoVacaciones.RenderControl(htmlTW);
        	return SB.ToString();
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
   		  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
     		MyMail.To = tbEmail.Text;
			MyMail.Subject = "Proceso : Comprobantes de Pago";
			MyMail.Body = (RenderHtml());
      		MyMail.BodyFormat = MailFormat.Html;
			try{
    	   		SmtpMail.Send(MyMail);}
    		catch{
    	 	  //lbInfo.Text = e.ToString();
    		}
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
