namespace AMS.Automotriz
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
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Automotriz_ConsultaSustitucion.
	/// </summary>
	public partial class AMS_Automotriz_ConsultaSustitucion : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
		}
		public void Buscar_Click(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			Panel1.Visible=true;
			string repuesto=Repuesto.Text.ToString();
			string codigo1=null;
			string codigo2=null;
			
			int numSus=0;

			codigo1=DBFunctions.SingleData("select MSUS_CODANTERIOR from DBXSCHEMA.MSUSTITUCION WHERE MSUS_CODACTUAL='"+repuesto.ToString()+"' AND MSUS_CODANTERIOR<>'--'");
			codigo2=codigo1;
			if (codigo1.Equals(null) || codigo1.Equals(""))
			{
				codigo1="No Existen Sustituciones";

			}
			
			while(codigo1!=String.Empty /*|| codigo1.Equals("")*/)
			{
				codigo2=codigo1;
				codigo1=DBFunctions.SingleData("select MSUS_CODANTERIOR from DBXSCHEMA.MSUSTITUCION WHERE MSUS_CODACTUAL='"+codigo2.ToString()+"' AND MSUS_CODANTERIOR<>'--'");
				numSus++;
				
			}
			
			NumSus.Text=numSus.ToString();
			CodAnt.Text=codigo2.ToString();
			NomLabel.Text =DBFunctions.SingleData("Select MITE_NOMBRE    from DBXSCHEMA.MITEMS       where MITE_CODIGO   ='"+repuesto+"'");
			CodActual.Text=DBFunctions.SingleData("select MSUS_CODACTUAL from DBXSCHEMA.MSUSTITUCION where MSUS_CODSUSTIT='"+codigo2+"' ");
			CodSus.Text=repuesto.ToString();
			CodActual.Text=repuesto.ToString();
			UltiSus.Text=codigo2.ToString();
			string fecha  =DBFunctions.SingleData("select MSUS_FECHA     from DBXSCHEMA.MSUSTITUCION WHERE MSUS_CODANTERIOR='"+codigo2+"'");
			if (fecha.Length>0)
			   FeUlSus.Text=Convert.ToDateTime(fecha).ToString("yyyy-MM-dd");
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

		}
		#endregion
	}
}
