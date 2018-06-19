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

namespace AMS.Finanzas.Cartera
{
	public partial  class ReporteCruces : System.Web.UI.Page
	{
		protected DataTable dtCruces;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(Session["cruces"]!=null)
			{
				Session.Remove("dtCruces");
				this.Llenar_dgCruces((ArrayList)Session["cruces"]);
			}
		}
		
		protected void Cargar_dtCruces()
		{
			dtCruces=new DataTable();
			dtCruces.Columns.Add("NIT",typeof(string));
			dtCruces.Columns.Add("PREFIJO",typeof(string));
			dtCruces.Columns.Add("NUMERO",typeof(string));
			dtCruces.Columns.Add("VALOR",typeof(double));
			dtCruces.Columns.Add("CRUCE",typeof(string));
			dtCruces.Columns.Add("PREFIJON",typeof(string));
			dtCruces.Columns.Add("NUMERON",typeof(string));
			dtCruces.Columns.Add("VALORN",typeof(double));
		}
	
		protected void Llenar_dgCruces(ArrayList facCan)
		{
			string[] datos;
			string temp;
			DataRow fila;
			if(facCan!=null)
			{
				for(int i=0;i<facCan.Count;i++)
				{
					if(Session["dtCruces"]==null)
						this.Cargar_dtCruces();
					temp=facCan[i].ToString();
					datos=temp.Split('@');
					fila=dtCruces.NewRow();
					fila[0]=datos[0];//nit
					fila[1]=datos[1];//prefijo
					fila[2]=datos[2];//factura
					fila[3]=Convert.ToDouble(datos[3]);
					fila[4]="Cruza con";
					fila[5]=datos[4];//prefijo
					fila[6]=datos[5];//factura
					fila[7]=Convert.ToDouble(datos[6]);
					dtCruces.Rows.Add(fila);
					dgCruces.DataSource=dtCruces;
					dgCruces.DataBind();
					Session["dtCruces"]=dtCruces;
				}
				lbInfo.Text="Se recomienda realizar los siguientes cruces de documentos antes de continuar con el proceso, esto con el fin de obtener un dato real de la cartera de clientes";
			}
			else
				lbInfo.Text="Error Interno";
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
