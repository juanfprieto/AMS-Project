using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using System.Runtime.InteropServices;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class TrasladoFondosCarta : System.Web.UI.UserControl
	{
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
			}
		}
		
		protected void Cambiar_Fecha(object Sender,EventArgs e)
		{
			fecha.Text=calendarioFecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void Aceptar_Valores(object Sender,EventArgs e)
		{
			if(aceptarDatos.Text=="Aceptar")
			{
				if(codigoCCO.Text==codigoCCD.Text)
				{
                    Utils.MostrarAlerta(Response, "No se puede transferir a la misma cuenta");
                   	lbSaldo.Text="";
					aceptar.Enabled=false;
				}
				else
				{
					lbSaldo.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(PCUE.pcue_saldoinic+SUM(MTES.mtes_saldo),0) FROM dbxschema.pcuentacorriente PCUE,dbxschema.mtesoreriasaldos MTES WHERE PCUE.pcue_codigo=MTES.pcue_codigo AND MTES.pcue_codigo='"+codigoCCO.Text+"' GROUP BY PCUE.pcue_saldoinic")).ToString("C");
					aceptar.Enabled=true;
					this.codigoCCD.Enabled=false;
					this.codigoCCO.Enabled=false;
					aceptarDatos.Text="Cancelar";
				}
			}
			else if(aceptarDatos.Text=="Cancelar")
			{
				this.codigoCCD.Enabled=true;
				this.codigoCCD.Text="";
				this.codigoCCDb.Text="";
				this.codigoCCO.Enabled=true;
				this.codigoCCO.Text="";
				this.codigoCCOb.Text="";
				aceptarDatos.Text="Aceptar";
				lbSaldo.Text="";
				aceptar.Enabled=false;
			}
		}
		
		protected void Aceptar_Transferencia(object Sender,EventArgs e)
		{
			Control padre=(this.Parent).Parent;
			double valorSaldo=-1;
			double nuevosaldo = 0;
			try{valorSaldo=Convert.ToDouble(lbSaldo.Text.Substring(1));}
			catch{valorSaldo=Convert.ToDouble(lbSaldo.Text.Substring(2,lbSaldo.Text.Length-3))*-1;}
			if(!DatasToControls.ValidarDouble(valorTransferencia.Text))
			{
                Utils.MostrarAlerta(Response, "Valor no valido");
			}
			else
			{
				//				if((Convert.ToDouble(valorTransferencia.Text)>valorSaldo) && valorSaldo>=0)
				//				{
				//					Response.Write("<script language:javascript>alert('El valor de la transferencia es MAYOR al valor del saldo, revice su SOBREGIRO!!!');</script>");
				//				}
				nuevosaldo = valorSaldo - Convert.ToDouble(valorTransferencia.Text);
				if(nuevosaldo<0)
				{
                    Utils.MostrarAlerta(Response, "El saldo de la cuenta queda negativo, por favor proporcione el número de autorización del sobregiro");
					lbInfoAutorizacion.Visible=tbSobregiro.Visible=rfv1.Visible=true;
				}
				//Si la cuenta no está exenta de impuesto (o sea que se cobra impuesto)
				string tipoExenta = "S";   // Todos los traslados por carta son excento de impuesto
		//		if(DBFunctions.SingleData("SELECT tres_exenimpuesto FROM pcuentacorriente WHERE pcue_codigo='"+this.codigoCCO.Text+"'")=="N")
				if(tipoExenta =="N")
					{
					//Si existe algun porcentaje
					if(Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCCO.Text+"'"))!=0)
					{
						((TextBox)padre.FindControl("valorConsignado")).Text=Convert.ToDouble(valorTransferencia.Text).ToString("C");
						((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
						((TextBox)padre.FindControl("totalEfectivo")).Text=(Convert.ToDouble(valorTransferencia.Text)*Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='"+this.codigoCCO.Text+"'"))/100).ToString("C");
						((TextBox)padre.FindControl("totalEfectivo")).ReadOnly=true;
					}
						//Si no existe porcentaje
					else
					{
                        Utils.MostrarAlerta(Response, "La cuenta tiene impuesto pero el valor especificado es cero. Revise los parametros");
						((TextBox)padre.FindControl("valorConsignado")).Text=Convert.ToDouble(valorTransferencia.Text).ToString("C");
						((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
						((TextBox)padre.FindControl("totalEfectivo")).Text=Convert.ToDouble("0").ToString("C");
						((TextBox)padre.FindControl("totalEfectivo")).ReadOnly=true;
					}
				}
					//Si está exenta
				else
				{
					((TextBox)padre.FindControl("valorConsignado")).Text=Convert.ToDouble(valorTransferencia.Text).ToString("C");
					((TextBox)padre.FindControl("valorConsignado")).ReadOnly=true;
					((TextBox)padre.FindControl("totalEfectivo")).Text=Convert.ToDouble("0").ToString("C");
					((TextBox)padre.FindControl("totalEfectivo")).ReadOnly=true;
				}
				((Panel)padre.FindControl("panelValores")).Visible=true;
				((Label)padre.FindControl("lbDetalle")).Text="Detalle de la Transferencia :";
				((Label)padre.FindControl("lbValor")).Text="Valor Transferido :";
				((Label)padre.FindControl("lbTotalEf")).Text="Total Impuestos :";
				valorTransferencia.Enabled=false;
				aceptar.Enabled=false;
				((Button)padre.FindControl("guardar")).Enabled=true;
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
