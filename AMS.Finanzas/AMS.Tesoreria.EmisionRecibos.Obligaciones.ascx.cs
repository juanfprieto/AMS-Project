namespace AMS.Finanzas
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using System.Configuration;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Tesoreria_EmisionRecibos_Obligaciones.
	/// </summary>
	public partial class AMS_Tesoreria_EmisionRecibos_Obligaciones : System.Web.UI.UserControl
	{
		#region Controles
		private DataTable dtObligaciones;
		#endregion Controles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				Session.Remove("DT_OBLIGACIONES");
				CrearTablas();
				Bind();
			}
			else
				dtObligaciones=(DataTable)Session["DT_OBLIGACIONES"];
		}

		private void CrearTablas()
		{
			dtObligaciones=new DataTable();
			dtObligaciones.Columns.Add("MOBL_NUMERO",typeof(string));
			dtObligaciones.Columns.Add("DOBL_NUMEPAGO",typeof(string));
			dtObligaciones.Columns.Add("PBAN_NOMBRE",typeof(string));
			dtObligaciones.Columns.Add("MOBL_FECHA",typeof(DateTime));
			dtObligaciones.Columns.Add("MOBL_SALDO",typeof(double));
			dtObligaciones.Columns.Add("MOBL_MONTPESOS",typeof(double));
			dtObligaciones.Columns.Add("MOBL_MONTINTERESES",typeof(double));
			dtObligaciones.Columns.Add("MOBL_MONTPESOSED",typeof(double));
			dtObligaciones.Columns.Add("MOBL_MONTINTERESESED",typeof(double));
			dtObligaciones.Columns.Add("MOBL_INTERESPAGADO",typeof(double));
			dtObligaciones.Columns.Add("MOBL_INTERESCAUSADO",typeof(double));

			Session["DT_OBLIGACIONES"]=dtObligaciones;

		}

		private void Bind()
		{
			Session["DT_OBLIGACIONES"]=dtObligaciones;
			dgrObligaciones.DataSource=dtObligaciones;
			dgrObligaciones.DataBind();
		}

		public void dgrObligaciones_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtObligaciones.Rows.Count>0)
				dgrObligaciones.EditItemIndex=(int)e.Item.ItemIndex;
			Bind();		
		}
		public void dgrObligaciones_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgrObligaciones.EditItemIndex=-1;
			Bind();
		}
		public void dgrObligaciones_Update(Object sender, DataGridCommandEventArgs e)
		{
			//Validaciones 
			if(dtObligaciones.Rows.Count == 0)
				return;
			double cuota=0, interes=0;
			DateTime fechaPago=new DateTime();
			try
			{
				cuota=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtEdCuota"))).Text);
				interes=Convert.ToDouble(((TextBox)(e.Item.FindControl("txtEdInteres"))).Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "No se puede modificar la fila, revise los valores ingresados");
				return;
			}
			if(cuota>Convert.ToDouble(dtObligaciones.Rows[e.Item.ItemIndex]["MOBL_SALDO"]))
			{
                Utils.MostrarAlerta(Response, "La cuota no puede superar el saldo");
				return;
			}
			dtObligaciones.Rows[e.Item.ItemIndex]["MOBL_MONTPESOSED"]=cuota;
			dtObligaciones.Rows[e.Item.ItemIndex]["MOBL_MONTINTERESESED"]=interes;

			dgrObligaciones.EditItemIndex=-1;
			Bind();
		}

		protected void dgrObligaciones_Item(object Sender,DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName=="Agregar")
			{
				string numOblig=((TextBox)e.Item.FindControl("txtAddNumOblig")).Text;
				string numPago=((TextBox)e.Item.FindControl("txtAddPagoOblig")).Text;
                if(numOblig.Length == 0 || numPago.Length == 0)
                {
                    Utils.MostrarAlerta(Response, "Debe diligenciar el numero de la obligacion y el numero de la cuota del pago ");
                    return;
                }

				if(dtObligaciones.Select("MOBL_NUMERO='"+numOblig+"' AND DOBL_NUMEPAGO="+numPago+" ").Length>0)
                {
                    Utils.MostrarAlerta(Response, "Ya agregó la obligación y pago seleccionado");
					return;
				}

				DataSet dsOblig=new DataSet();
				DBFunctions.Request(dsOblig,IncludeSchema.NO,
					@"SELECT MO.MOBL_NUMERO, DP.DOBL_NUMEPAGO, PB.PBAN_NOMBRE, DP.MOBL_FECHA, 
					(MO.MOBL_MONTPESOS-MO.MOBL_MONTPAGADO) MOBL_SALDO, 
                    CASE WHEN MO.MOBL_MONTPESOS-MO.MOBL_MONTPAGADO >= DP.MOBL_MONTPESOS 
                    THEN DP.MOBL_MONTPESOS ELSE MO.MOBL_MONTPESOS-MO.MOBL_MONTPAGADO END AS MOBL_MONTPESOS,
					DP.MOBL_MONTINTERES, MO.MOBL_INTERESPAGADO, MO.MOBL_INTERESCAUSADO 
					FROM MOBLIGACIONFINANCIERA MO, PBANCO PB, PCUENTACORRIENTE PC, DOBLIGACIONFINANCIERAPLANPAGO DP 
					WHERE PC.PCUE_CODIGO=MO.PCUE_CODIGO AND PC.PBAN_BANCO=PB.PBAN_CODIGO AND 
					DP.MOBL_NUMERO=MO.MOBL_NUMERO AND DP.DOBL_NUMEPAGO="+numPago+@" AND 
					(MO.MOBL_MONTPESOS-MO.MOBL_MONTPAGADO)>0 AND MO.MOBL_NUMERO='"+numOblig+"';");
					
				if(dsOblig.Tables[0].Rows.Count==0)
                {
                    Utils.MostrarAlerta(Response, "No se encontró el pago de la obligación");
					return;
				}

				string numO, bancoO, pagoO;
				DateTime fechaO;
				double saldoM, cuotaM, interesM, interesP, interesC;
				numO=dsOblig.Tables[0].Rows[0]["MOBL_NUMERO"].ToString();
				pagoO=dsOblig.Tables[0].Rows[0]["DOBL_NUMEPAGO"].ToString();
				bancoO=dsOblig.Tables[0].Rows[0]["PBAN_NOMBRE"].ToString();
				fechaO=Convert.ToDateTime(dsOblig.Tables[0].Rows[0]["MOBL_FECHA"]);
				saldoM=Convert.ToDouble(dsOblig.Tables[0].Rows[0]["MOBL_SALDO"]);
				cuotaM=Convert.ToDouble(dsOblig.Tables[0].Rows[0]["MOBL_MONTPESOS"]);
				interesM=Convert.ToDouble(dsOblig.Tables[0].Rows[0]["MOBL_MONTINTERES"]);
				interesC=Convert.ToDouble(dsOblig.Tables[0].Rows[0]["MOBL_INTERESCAUSADO"]);
				interesP=Convert.ToDouble(dsOblig.Tables[0].Rows[0]["MOBL_INTERESPAGADO"]);
				if(cuotaM>saldoM)
					saldoM=cuotaM;

				DataRow drObligacion=dtObligaciones.NewRow();
				drObligacion["MOBL_NUMERO"]=numO;
				drObligacion["DOBL_NUMEPAGO"]=pagoO;
				drObligacion["PBAN_NOMBRE"]=bancoO;
				drObligacion["MOBL_FECHA"]=fechaO;
				drObligacion["MOBL_SALDO"]=saldoM;
				drObligacion["MOBL_MONTPESOS"]=cuotaM;
				drObligacion["MOBL_MONTINTERESES"]=interesM;
				drObligacion["MOBL_MONTPESOSED"]=cuotaM;
				drObligacion["MOBL_MONTINTERESESED"]=interesM;
				drObligacion["MOBL_INTERESPAGADO"]=interesP;
				drObligacion["MOBL_INTERESCAUSADO"]=interesC;
				dtObligaciones.Rows.Add(drObligacion);
			}
			else if(((Button)e.CommandSource).CommandName=="Remover")
			{
				dtObligaciones.Rows[e.Item.DataSetIndex].Delete();
			}
			dgrObligaciones.EditItemIndex=-1;
			Bind();
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
			this.dgrObligaciones.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrObligaciones_ItemDataBound);

		}
		#endregion

		private void dgrObligaciones_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Footer)
			{
				double totalCapital=0, totalInteres=0;
				for(int n=0;n<dtObligaciones.Rows.Count;n++)
				{
					totalCapital+=Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTPESOSED"]);
					totalInteres+=Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTINTERESESED"]);
				}
				e.Item.Cells[7].Text=totalCapital.ToString("C");
				e.Item.Cells[8].Text=totalInteres.ToString("C");
				txtTotalO.Text=(totalCapital+totalInteres).ToString("C");
				ViewState["TXT_OBLIGACION"]=((TextBox)e.Item.FindControl("txtAddNumOblig")).ClientID;
				Session["TOT_OBLIGACION"]=(totalCapital+totalInteres);
				Session["DT_OBLIGACIONES"]=dtObligaciones;
			}
		}
	}
}
