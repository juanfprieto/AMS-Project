namespace AMS.Comercial
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Drawing.Text;
	using System.Drawing.Drawing2D;
	using System.Drawing.Imaging;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text;
	using AMS.DB;
	using AMS.Forms;

	/// <summary>
	///		Descripción breve de AMS_Comercial_RetirarBusAfiliado.
	/// </summary>
	public class AMS_Comercial_RetirarBusAfiliado : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtFechaRetiro;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtFechaDevolucion;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox txtDestino;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox txtLibro;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.TextBox txtObservaciones;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.TextBox txtPlacaBus;
		protected System.Web.UI.WebControls.TextBox txtPlacaBusa;
		protected System.Web.UI.WebControls.Label lblError;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if(Request.QueryString["act"]!=null)
					Response.Write("<script language='javascript'>alert('El bus ha sido retirado.');</script>");
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			//Validar
			#region Validaciones
			string placa=txtPlacaBus.Text.Trim();
			string destino=txtDestino.Text.Trim();
			string libro=txtLibro.Text.Trim();
			string observaciones=txtObservaciones.Text.Trim();
			string numBus;
			DateTime fechaRetiro,fechaDevolucion,fechaIngreso;
			string estado=DBFunctions.SingleData("SELECT TESTA_CODIGO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';");
			if(estado.Length==0||estado.Equals("-1")){
				Response.Write("<script language='javascript'>alert('El vehículo no existe o ya ha sido retirado.');</script>");
				return;
			}
			fechaIngreso=Convert.ToDateTime(DBFunctions.SingleData("SELECT FEC_INGRESO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';"));
			numBus=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';");
			try{
				fechaRetiro=Convert.ToDateTime(txtFechaRetiro.Text);
			}
			catch{
				Response.Write("<script language='javascript'>alert('Fecha de retiro no valida.');</script>");
				return;
			}
			try{
				fechaDevolucion=Convert.ToDateTime(txtFechaDevolucion.Text);
			}
			catch{
				Response.Write("<script language='javascript'>alert('Fecha de devolución no valida.');</script>");
				return;
			}
			if(destino.Length==0){
				Response.Write("<script language='javascript'>alert('Debe dar el destino del vehículo.');</script>");
				return;
			}
			if(libro.Length==0){
				Response.Write("<script language='javascript'>alert('Debe ingresar el libro.');</script>");
				return;
			}
			if(observaciones.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe ingresar las observaciones.');</script>");
				return;
			}
			#endregion Validaciones
			
			ArrayList arSql=new ArrayList();
			//Guardar Retiro
			arSql.Add("INSERT INTO DBXSCHEMA.MBUSRETIRADO VALUES(DEFAULT,'"+placa+"',"+numBus+",'"+fechaIngreso.ToString("yyyy-MM-dd")+"','"+fechaRetiro.ToString("yyyy-MM-dd")+"','"+fechaDevolucion.ToString("yyyy-MM-dd")+"','"+destino+"','"+libro+"','"+observaciones+"');");
			//Guardar Historial numero
			//arSql.Add("insert into dbxschema.MBUS_NUMERO_INTERNO values ("+numBus+",'"+placa+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
			//Actualizar bus, estado, numero
			arSql.Add("update dbxschema.mbusafiliado set testa_codigo=-1, mbus_numero=0 where mcat_placa='"+placa+"';");
			
			if(DBFunctions.Transaction(arSql))
				Response.Redirect(indexPage+"?process=Comercial.RetirarBusAfiliado&path="+Request.QueryString["path"]+"&act=1");
			else
				lblError.Text=DBFunctions.exceptions;
		}
	}
}
