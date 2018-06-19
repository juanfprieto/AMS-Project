
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;    
	using AMS.Forms;
using AMS.Tools;

namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_ActDatosOrdenTemp.
	/// </summary>
	public partial class AMS_Automotriz_ActDatosOrdenTemp : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(!IsPostBack)
			{
				//bind.PutDatasIntoDropDownList(ddlPrefijo,"SELECT pdoc_codigo, pdoc_codigo CONCAT ' - ' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='OT'");
                Utils.llenarPrefijos(Response ,ref ddlPrefijo,"%","%","OT");
				bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mord_numeorde FROM morden WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND test_estado='A' ORDER BY mord_numeorde");
                bind.PutDatasIntoDropDownList(ddlCargo, "SELECT tcar_cargo,tcar_nombre FROM tcargoorden WHERE tcar_cargo <> 'X'");
				lbCargo.Text=DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo=(SELECT tcar_cargo FROM morden WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+")");
			}
		}

		protected void ddlPrefijo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			//bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mord_numeorde FROM morden WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' ORDER BY mord_numeorde");
            bind.PutDatasIntoDropDownList(ddlNumero, "SELECT  mord_numeorde FROM morden WHERE pdoc_codigo= '" + ddlPrefijo.SelectedValue + "' AND TEST_ESTADO <> 'E' AND TEST_ESTADO <> 'F'  ORDER BY mord_numeorde;");
		}

		protected void ddlNumero_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			string cargo=DBFunctions.SingleData("SELECT tcar_cargo FROM morden WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
			lbCargo.Text=DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo=(SELECT tcar_cargo FROM morden WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+")");
            ddlCargo.Items.Clear();
            bind.PutDatasIntoDropDownList(ddlCargo, "SELECT tcar_cargo,tcar_nombre FROM tcargoorden WHERE tcar_cargo <> 'X' and (tcar_cargo not in (select tcar_cargo from mfacturaclientetaller WHERE pdoc_prefordetrab = '" + ddlPrefijo.SelectedValue + "' and mord_numeorde = " + ddlNumero.SelectedValue + " AND TCAR_CARGO <> 'S') OR (SELECT COUNT(*) FROM mfacturaclientetaller WHERE pdoc_prefordetrab = '" + ddlPrefijo.SelectedValue + "' and mord_numeorde = " + ddlNumero.SelectedValue + " AND TCAR_CARGO = 'S') <> 2) ");
		    Mostrar_Panel(cargo);
		}

		protected void ddlCargo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Mostrar_Panel(ddlCargo.SelectedValue);
			btnGuardar.Enabled=true;
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Automotriz.ActDatosOrdenTemp");
		}

		protected void btnGuardar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqls=new ArrayList();
			string cargoActual=DBFunctions.SingleData("SELECT tcar_cargo FROM morden WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
			//Si el cargo escogido no es aseguradora ni garantia
			if(ddlCargo.SelectedValue!="S" && ddlCargo.SelectedValue!="G")
			{
				//Si el cargo de la orden no es aseguradora ni garantia
				if(cargoActual!="S" && cargoActual!="G")
				{
					sqls.Add("UPDATE morden SET tcar_cargo='"+ddlCargo.SelectedValue+"' WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
                }
				else
				{
					if(cargoActual=="S")
						sqls.Add("DELETE FROM dordenseguros WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
				//	else if(cargoActual=="G")
				//		sqls.Add("DELETE FROM dordengarantia WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
					sqls.Add("UPDATE morden SET tcar_cargo='"+ddlCargo.SelectedValue+"' WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
                //    if (cargoActual == "S" && ddlCargo.SelectedValue == "C") // cuando se cambia de SEGUROS a CLIENTE tambien se cambia en las transferencias
           	    }
			}
			//Si el cargo escogido es aseguradora o garantia
			else
			{
				//Si no se cambio el cargo aseguradora
				if(cargoActual=="S" && ddlCargo.SelectedValue=="S")
				{
                    if (ExistenDatosSeguros(ddlPrefijo.SelectedValue, ddlNumero.SelectedValue))
                    {
                        //double porDeducible = Convert.ToDouble(tbPorDedCli.Text.Trim());
                        //double vaMinimoDeducible = Convert.ToDouble(tbDedOT.Text.Trim());
                        sqls.Add("UPDATE dordenseguros SET mnit_nitseguros='" + nitAseguradora.Text + "',mord_siniestro='" + siniestro.Text + "',mord_porcdeducible=" + Convert.ToDouble(porcentajeDeducible.Text).ToString() + ",mord_deduminimo=" + Convert.ToDouble(valorMinDeducible.Text).ToString() + ",mord_autorizacion='" + numeroAutorizacionAsegura.Text + "' WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "' AND mord_numeorde=" + ddlNumero.SelectedValue + "");
                    }
                    else
                        sqls.Add("INSERT INTO dordenseguros VALUES('" + ddlPrefijo.SelectedValue + "'," + ddlNumero.SelectedValue + ",'" + nitAseguradora.Text + "'," + Convert.ToDouble(porcentajeDeducible.Text).ToString() + "," + Convert.ToDouble(valorMinDeducible.Text).ToString() + ",null,'" + siniestro.Text + "','" + numeroAutorizacionAsegura.Text + "')");
               }
				//Si no se cambio el cargo garantia
				else if(cargoActual=="G" && ddlCargo.SelectedValue=="G")
				{
					if(ExistenDatosGarantia(ddlPrefijo.SelectedValue,ddlNumero.SelectedValue))
						sqls.Add("UPDATE dordengarantia SET mnit_nitfabrica='"+nitCompania.Text+"',mord_autorizacion='"+numeroAutorizacionGarant.Text+"' WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
					else
						sqls.Add("INSERT INTO dordengarantia VALUES('"+ddlPrefijo.SelectedValue+"',"+ddlNumero.SelectedValue+",'"+nitCompania.Text+"','"+numeroAutorizacionGarant.Text+"')");
				 	sqls.Add("UPDATE morden SET tcar_cargo='"+ddlCargo.SelectedValue+"' WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
                }
				//Si se cambio de cargo
				else
				{
                    // double porcDeducible = Convert.ToDouble(tbPorDedCli.Text.Trim());
                    // double valMinimoDeducible = Convert.ToDouble(tbDedOT.Text.Trim());

                    if (ddlCargo.SelectedValue=="S" && !ExistenDatosSeguros(ddlPrefijo.SelectedValue,ddlNumero.SelectedValue))
						sqls.Add("INSERT INTO dordenseguros VALUES('"+ddlPrefijo.SelectedValue+"',"+ddlNumero.SelectedValue+",'"+nitAseguradora.Text+"',"+Convert.ToDouble(porcentajeDeducible.Text).ToString()+","+Convert.ToDouble(valorMinDeducible.Text).ToString()+",null,'"+siniestro.Text+"','"+numeroAutorizacionAsegura.Text+"')");
					else if(ddlCargo.SelectedValue=="S" && ExistenDatosSeguros(ddlPrefijo.SelectedValue,ddlNumero.SelectedValue))
						sqls.Add("UPDATE dordenseguros SET mnit_nitseguros='"+nitAseguradora.Text+"',mord_siniestro='"+siniestro.Text+"',mord_porcdeducible="+Convert.ToDouble(porcentajeDeducible.Text).ToString()+",mord_deduminimo="+Convert.ToDouble(valorMinDeducible.Text).ToString()+",mord_autorizacion='"+numeroAutorizacionAsegura.Text+"' WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
					else if(ddlCargo.SelectedValue=="G" && !ExistenDatosGarantia(ddlPrefijo.SelectedValue,ddlNumero.SelectedValue))
						sqls.Add("INSERT INTO dordengarantia VALUES('"+ddlPrefijo.SelectedValue+"',"+ddlNumero.SelectedValue+",'"+nitCompania.Text+"','"+numeroAutorizacionGarant.Text+"')");
					else if(ddlCargo.SelectedValue=="G" && ExistenDatosGarantia(ddlPrefijo.SelectedValue,ddlNumero.SelectedValue))
						sqls.Add("UPDATE dordengarantia SET mnit_nitfabrica='"+nitCompania.Text+"',mord_autorizacion='"+numeroAutorizacionGarant.Text+"' WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
					sqls.Add("UPDATE morden SET tcar_cargo='"+ddlCargo.SelectedValue+"' WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mord_numeorde="+ddlNumero.SelectedValue+"");
				//    if(cargoActual=="S" && ddlCargo.SelectedValue=="C") // cuando se cambia de SEGUROS a CLIENTE tambien se cambia en las transferencias
                }
			}  //   se actualiza el CARGO DE LAS TRANSFERENCIAS al nuevo cargo a menos que el cargo origen ya haya sido facturado
            //sqls.Add("UPDATE mordenTRANSFERENCIA SET tcar_cargo='" + ddlCargo.SelectedValue + "' WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "' AND mord_numeorde=" + ddlNumero.SelectedValue + "  and tcar_cargo='" + cargoActual + "' and tcar_cargo not in (select tcar_cargo from mfacturaclientetaller where pdoc_prefordetrab = '" + ddlPrefijo.SelectedValue + "' and mord_numeorde = " + ddlNumero.SelectedValue + " ) ");                		
			if(DBFunctions.Transaction(sqls))
				lb.Text+="Cargo Actualizado Correctamente !!!" ; //DBFunctions.exceptions+"<br>";
			else
				lb.Text+="Error "+DBFunctions.exceptions+"<br>";
		}

		private bool ExistenDatosSeguros(string prefijo,string numero)
		{
			bool existe=false;
			if(DBFunctions.RecordExist("SELECT * FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numero+""))
				existe=true;
			return existe;
		}

		private bool ExistenDatosGarantia(string prefijo,string numero)
		{
			bool existe=false;
			if(DBFunctions.RecordExist("SELECT * FROM dordengarantia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numero+""))
				existe=true;
			return existe;
		}

		public void LlenarDatosSeguros(string prefijo, string numero)
		{
			DataSet ds=new DataSet();
			if(ExistenDatosSeguros(prefijo,numero))
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mnit_nitseguros AS SEGUROS,mord_siniestro AS SINIESTRO,mord_porcdeducible AS PORCDED,mord_deduminimo AS MINDED,mord_autorizacion AS AUT FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numero+"");
				nitAseguradora.Text     =ds.Tables[0].Rows[0][0].ToString();
				siniestro.Text          =ds.Tables[0].Rows[0][1].ToString();
				porcentajeDeducible.Text=ds.Tables[0].Rows[0][2].ToString();
				valorMinDeducible.Text  =ds.Tables[0].Rows[0][3].ToString();
				numeroAutorizacionAsegura.Text=ds.Tables[0].Rows[0][4].ToString();
			}
		}

		public void LlenarDatosGarantia(string prefijo, string numero)
		{
			DataSet ds=new DataSet();
			if(ExistenDatosGarantia(prefijo,numero))
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mnit_nitfabrica AS FABRICA,mord_autorizacion AS AUT FROM dordengarantia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numero+"");
				nitCompania.Text=ds.Tables[0].Rows[0][0].ToString();
				numeroAutorizacionGarant.Text=ds.Tables[0].Rows[0][1].ToString();
			}
		}

		public void Mostrar_Panel(string cargo)
		{
			if(cargo=="S")
			{
				pnlSeguros.Visible=true;
				pnlGarantia.Visible=false;
				LlenarDatosSeguros(ddlPrefijo.SelectedValue,ddlNumero.SelectedValue);
			}
			else if(cargo=="G")
			{
				pnlGarantia.Visible=true;
				pnlSeguros.Visible=false;
				LlenarDatosGarantia(ddlPrefijo.SelectedValue,ddlNumero.SelectedValue);
			}
			else
			{
				pnlGarantia.Visible=pnlSeguros.Visible=false;
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

		}
		#endregion

	}
}
