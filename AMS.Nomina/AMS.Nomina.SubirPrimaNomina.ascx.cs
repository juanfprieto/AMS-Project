namespace AMS.Nomina
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de WebUserControl1.
	/// </summary>
	public class SubirPrimaNomina: System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList ddlPeriodoPago;
		protected System.Web.UI.WebControls.DropDownList ddlPeriodoPrima;
		protected System.Web.UI.WebControls.Button btn_subir;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				 DatasToControls param = new DatasToControls();
				 param.PutDatasIntoDropDownList(ddlPeriodoPago,"Select mqui_codiquin,cast(mqui_anoquin as char(4)) concat '-'concat cast(mqui_mesquin as char(2)) concat '-' concat cast(MQUI_TPERNOMI as char(2)) from DBXSCHEMA.MQUINCENAS ORDER BY 1");
				 param.PutDatasIntoDropDownList(ddlPeriodoPrima, "Select mpri_secuencia,cast(mpri_fechinic as char(11)) concat 'a'  concat ' ' concat cast(mpri_fechfina as char(11)) concat '-' concat coalesce(mpri_tipoprima,'') from DBXSCHEMA.MPRIMAS ORDER BY 1");
			}
		}

		protected void subirPrimas (object Sender,EventArgs e)
		{
			string concPrima;
			int i;
			DataSet primas = new DataSet();
			string tipoPrima=DBFunctions.SingleData("select mpri_tipoprima from dbxschema.mprimas where MPRI_SECUENCIA="+ddlPeriodoPrima.SelectedValue.ToString()+"");
			if(tipoPrima=="Normal")
			{
				concPrima=DBFunctions.SingleData("select cnom_concprimnormcodi from dbxschema.cnomina");
			}
			else
				concPrima=DBFunctions.SingleData("select cnom_concprimajuscodi from dbxschema.cnomina");
				

			//validar si ya subieron las primas al periodo escogido.
			string validar=DBFunctions.SingleData("Select * from DBXSCHEMA.DQUINCENA where dqui_docrefe='"+ddlPeriodoPago.SelectedItem.Text+"' and pcon_concepto = '" + concPrima + "' fetch first 1 rows only");
            if (validar == String.Empty)
			{
				
//				Response.Write("<script language:javascript>alert('"+ddlPeriodoPago.SelectedValue.ToString()+"');</script>");
//				Response.Write("<script language:javascript>alert('"+ddlPeriodoPrima.SelectedValue.ToString()+"');</script>");
				
				DBFunctions.Request(primas,IncludeSchema.NO,"Select * from DBXSCHEMA.DPRIMAS where dpri_secuencia="+ddlPeriodoPrima.SelectedValue.ToString()+" and dpri_valorprima <> 0");
				for (i=0;i<primas.Tables[0].Rows.Count;i++)
				{
					double valdiaprima = double.Parse(primas.Tables[0].Rows[i][2].ToString()) ; // no se divide porque viene el valor neto de la prima / double.Parse(primas.Tables[0].Rows[i][3].ToString());
					double tem1 = Math.Round(valdiaprima,0);
					string prueba="insert into dquincena values(DEFAULT,"+ddlPeriodoPago.SelectedValue.ToString()+",'"+primas.Tables[0].Rows[i][1].ToString()+"','"+concPrima+"',1,"+primas.Tables[0].Rows[i][2].ToString()+","+primas.Tables[0].Rows[i][2].ToString()+","+0+",4 ,"+0+",'"+ddlPeriodoPago.SelectedItem.Text+"','P')";
					DBFunctions.NonQuery("insert into dquincena values(DEFAULT,"+ddlPeriodoPago.SelectedValue.ToString()+",'"+primas.Tables[0].Rows[i][1].ToString()+"','"+concPrima+"',"+primas.Tables[0].Rows[i][3].ToString()+","+tem1.ToString()+","+primas.Tables[0].Rows[i][2].ToString()+","+0+",4 ,"+0+",'"+ddlPeriodoPago.SelectedItem.Text+"','P')");
				}
                Utils.MostrarAlerta(Response, "El periodo de liquidacion " + ddlPeriodoPago.SelectedItem.Text + " subió correctamente.");
			}
			else
                Utils.MostrarAlerta(Response, "El periodo de liquidacion " + ddlPeriodoPago.SelectedItem.Text + " ya fue subido.");

			
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
	}
}
