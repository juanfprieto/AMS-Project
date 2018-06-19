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

	/// <summary>
	///		Descripción breve de AMS_Comercial_RemesasImpresion.
	/// </summary>
	public class AMS_Comercial_RemesasImpresion : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label NumeroRemesaLabel;
		protected System.Web.UI.WebControls.Label NumeroRemesa;
		protected System.Web.UI.WebControls.Label FechaLabel;
		protected System.Web.UI.WebControls.Label Fecha;
		protected System.Web.UI.WebControls.Label TipoRemesaLabel;
		protected System.Web.UI.WebControls.Label OrigenLabel;
		protected System.Web.UI.WebControls.Label DestinoLabel;
		protected System.Web.UI.WebControls.Label RutaLabel;
		protected System.Web.UI.WebControls.Label BusLabel;
		protected System.Web.UI.WebControls.Label DirOrigenLabel;
		protected System.Web.UI.WebControls.Label TelOrigenLabel;
		protected System.Web.UI.WebControls.Label NomEmisorLabel;
		protected System.Web.UI.WebControls.Label DirDestinoLabel;
		protected System.Web.UI.WebControls.Label TelDestinoLabel;
		protected System.Web.UI.WebControls.Label NomDestinoLabel;
		protected System.Web.UI.WebControls.Label ValoDecLabel;
		protected System.Web.UI.WebControls.Label UnidadesLabel;
		protected System.Web.UI.WebControls.Label PesoLabel;
		protected System.Web.UI.WebControls.Label VolumenLabe;
		protected System.Web.UI.WebControls.Button calcular;
		protected System.Web.UI.WebControls.Label ValoFleteLabel;
		protected System.Web.UI.WebControls.Label MonedaLabel;
		protected System.Web.UI.HtmlControls.HtmlTable TableRemesa;
		protected System.Web.UI.WebControls.Label TRemesa;
		protected System.Web.UI.WebControls.Label ciudad1;
		protected System.Web.UI.WebControls.Label ciudad2;
		protected System.Web.UI.WebControls.Label ruta;
		protected System.Web.UI.WebControls.Label telorigen;
		protected System.Web.UI.WebControls.Label dirorigen;
		protected System.Web.UI.WebControls.Label emisor;
		protected System.Web.UI.WebControls.Label dirdestino;
		protected System.Web.UI.WebControls.Label destinatario;
		protected System.Web.UI.WebControls.Label valordeclarado;
		protected System.Web.UI.WebControls.Label unidades;
		protected System.Web.UI.WebControls.Label peso;
		protected System.Web.UI.WebControls.Label volumen;
		protected System.Web.UI.WebControls.Label total;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList NUMREMESA;
		protected System.Web.UI.WebControls.Label bus;
		protected System.Web.UI.WebControls.Label teldestino;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Label Label;
		protected System.Web.UI.WebControls.Label DescripcionLabel;
		protected System.Web.UI.WebControls.Label RemeasLabel;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			DatasToControls bind = new DatasToControls();
			if(NUMREMESA.Items.Count==0)
			{
				bind.PutDatasIntoDropDownList(NUMREMESA,"Select NUM_REM from DBXSCHEMA.MREMESA ORDER BY NUM_REM");
				ListItem it=new ListItem("--Seleccione Remesa --","0");
				NUMREMESA.Items.Insert(0,it);
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void NUMREMESa_proceso(object sender, System.EventArgs e)
		{
			NumeroRemesa.Text=DBFunctions.SingleData("Select NUM_REM from DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+" ");
			Fecha.Text=DBFunctions.SingleData("SELECT MREM_FECHA FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+" ");
			TRemesa.Text=DBFunctions.SingleData("SELECT MRUT_CODIGO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+" ");
			ciudad1.Text=DBFunctions.SingleData("SELECT ORIGEN FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+" ");
			ciudad2.Text=DBFunctions.SingleData("SELECT DESTINO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+" ");
			int codruta=Convert.ToInt32(DBFunctions.SingleData("SELECT MRUT_CODIGO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+" "));
			ruta.Text=DBFunctions.SingleData("SELECT MRUT_DESCRIPCION FROM MRUTA WHERE MRUT_CODIGO="+codruta+" ");
			bus.Text=DBFunctions.SingleData("SELECT MCAT_PLACA FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			dirorigen.Text=DBFunctions.SingleData("SELECT DIR_ORIGEN FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			telorigen.Text=DBFunctions.SingleData("SELECT TEL_ORIGEN FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			emisor.Text=DBFunctions.SingleData("SELECT NOM_EMISOR FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			dirdestino.Text=DBFunctions.SingleData("SELECT DIR_DESTINO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			teldestino.Text=DBFunctions.SingleData("SELECT TEL_DESTINO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			destinatario.Text=DBFunctions.SingleData("SELECT NOM_DESTINO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			valordeclarado.Text=DBFunctions.SingleData("SELECT VALOR_DEC FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			unidades.Text=DBFunctions.SingleData("SELECT UNIDADES FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			peso.Text=DBFunctions.SingleData("SELECT PESO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			volumen.Text=DBFunctions.SingleData("SELECT VOLUMEN FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			total.Text=DBFunctions.SingleData("SELECT VALO_FLET FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			DescripcionLabel.Text=DBFunctions.SingleData("SELECT MREM_CONTENIDO FROM DBXSCHEMA.MREMESA WHERE NUM_REM="+NUMREMESA.SelectedValue+"");
			
		}
	}
}
