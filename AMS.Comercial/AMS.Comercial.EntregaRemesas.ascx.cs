namespace AMS.Comercial
{
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Globalization;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.Mail;
	using System.Web.UI;
	using System;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;
	using Ajax;
	//Realizado por:
	//Ing.German Lopera
	//2007-03-16
	/// <summary>
	///		Descripción breve de AMS_Comercial_EntregaRemesas.
	/// </summary>
	public class AMS_Comercial_EntregaRemesas : System.Web.UI.UserControl
	{	
		//esta clase se encarga de traer los datos de las remesas pendientes de entregar
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label CodigoLabel;
		protected System.Web.UI.WebControls.DropDownList remesaspendientes;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label NombreLabel2;
		protected System.Web.UI.WebControls.Label OrigenLabel;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label destinoLabel;
		protected System.Web.UI.WebControls.Label NombreLabel;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label descripcionLabel;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label ContenidoLabel;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label unidadesLabel;
		protected System.Web.UI.WebControls.Button Entregar;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			//se carga el dropdownlist  con todos los numeros de remesas cuyo estado sea "no despachada"
			DatasToControls bind = new DatasToControls();
			if(remesaspendientes.Items.Count==0)
			{
				bind.PutDatasIntoDropDownList(remesaspendientes,"select NUM_REM FROM DBXSCHEMA.MREMESA WHERE MREM_ESTADO=1 ");
				ListItem it=new ListItem("--Remesas --","0");
				remesaspendientes.Items.Insert(0,it);
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
			this.remesaspendientes.SelectedIndexChanged += new System.EventHandler(this.remesaspendientes_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void remesaspendientes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			string nomOr=DBFunctions.SingleData("select NOM_EMISOR FROM DBXSCHEMA.MREMESA WHERE NUM_REM ='"+remesaspendientes.SelectedValue.ToString()+"' ");
			string nomDe=DBFunctions.SingleData("select NOM_DESTINO FROM DBXSCHEMA.MREMESA WHERE NUM_REM='"+remesaspendientes.SelectedValue.ToString()+"' ");
			string Ori=	DBFunctions.SingleData("select ORIGEN FROM DBXSCHEMA.MREMESA WHERE NUM_REM ='"+remesaspendientes.SelectedValue.ToString()+"' ");
			string Dest=DBFunctions.SingleData("select DESTINO FROM DBXSCHEMA.MREMESA WHERE NUM_REM ='"+remesaspendientes.SelectedValue.ToString()+"' ");
			int codigoTipoRemesa=Convert.ToInt32(DBFunctions.SingleData("select TREM_CODIGO FROM DBXSCHEMA.MREMESA WHERE NUM_REM ='"+remesaspendientes.SelectedValue.ToString()+"' "));			
			string descripcion=DBFunctions.SingleData("Select TREM_DESCRIPCION from DBXSCHEMA.TREMESA WHERE TREM_CODIGO="+codigoTipoRemesa+" ");
			//se cargan en las variables los datos relacionados a la remesa
			descripcionLabel.Text=descripcion.ToString();
			CodigoLabel.Text=remesaspendientes.SelectedValue.ToString();
			NombreLabel.Text=nomOr.ToString();
			NombreLabel2.Text=nomDe.ToString();
			OrigenLabel.Text=Ori.ToString();
			destinoLabel.Text=Dest.ToString();
			ContenidoLabel.Text=DBFunctions.SingleData("select MREM_CONTENIDO FROM DBXSCHEMA.MREMESA WHERE NUM_REM ='"+remesaspendientes.SelectedValue.ToString()+"' ");
			unidadesLabel.Text=DBFunctions.SingleData("select UNIDADES FROM DBXSCHEMA.MREMESA WHERE NUM_REM ='"+remesaspendientes.SelectedValue.ToString()+"' ");
            Utils.MostrarAlerta(Response, "Favor Verificar Datos");
			//cargamos todos los datos relacionados con la remesa seleccionada,para verificar con el destinatario

		}

		

		public void Entregar_click(object sender, System.EventArgs e)
		{
			DBFunctions.NonQuery("UPDATE DBXSCHEMA.MREMESA SET MREM_ESTADO=2 WHERE NUM_REM='"+remesaspendientes.SelectedValue.ToString()+"' ");
			//DB.Text=DBFunctions.exceptions;
            Utils.MostrarAlerta(Response, "Remesa Despachada");
			//simplemente se acuraliza el estado de la remesas a despachada
		}

		

	}
}