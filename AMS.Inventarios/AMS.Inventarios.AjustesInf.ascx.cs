// created on 30/10/2003 at 9:31
using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;


namespace AMS.Inventarios
{
	public partial class AjustesInf : System.Web.UI.UserControl
	{
		protected Label lbNumDoc ;
		protected string ano,mes;
		protected double porcentaje;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				ano=DBFunctions.SingleData("SELECT pano_ano FROM cinventario");
				lbYear.Text="  "+ano;
				mes=DBFunctions.SingleData("SELECT pmes_mes FROM cinventario");
				lbMonth.Text="  "+mes;
				if(DBFunctions.RecordExist("SELECT * FROM ppaag where pano_ano="+ano+" and pmes_mes="+mes))
					porcentaje=Convert.ToDouble(DBFunctions.SingleData("SELECT ppaa_inflacion FROM ppaag where pano_ano="+ano+" and pmes_mes="+mes));//Trae el valor del paag valido para este mes y año escogidos
				else
				{
					this.btnEjecuta.Enabled = false;
                    Utils.MostrarAlerta(Response, "No existe un valor de PAAG para el mes " + mes + " en el año " + ano + "!");
					porcentaje=0;
				}
				//Ahora revisamos si ya se realizo el proceso para el mes y ano vigente dentro de cinventario
				lbPorcentaje.Text="  "+porcentaje.ToString();
				DatasToControls bind = new DatasToControls();
                //bind.PutDatasIntoDropDownList(ddlCodDoc, "SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu = 'AI' and tvig_vigencia = 'V' "); //Documento de Ajuste de Inflacion
                Utils.llenarPrefijos(Response, ref ddlCodDoc, "%", "%", "AI");
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION");//Almacen Asociado???????
				bind.PutDatasIntoDropDownList(ddlCentro, "SELECT pcen_codigo, pcen_nombre FROM pcentrocosto");//Centro de Costos
				bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor");//Vendedor
				IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				tbDate.Text = DateTime.Now.GetDateTimeFormats()[6];
				calDate.SelectedDate=DateTime.Now;
			}
			lbDoc.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlCodDoc.SelectedValue+"'");
		}

		public void Ajustar(object sender, System.EventArgs e)
		{
			string palm = ddlAlmacen.SelectedItem.Value;
			int TMovKar=51;//Tipo de Movimiendo de Kardex
			if(DBFunctions.RecordExist("SELECT * FROM ditems WHERE pano_ano="+ano+" AND pmes_mes="+mes+" AND tmov_tipomovi="+TMovKar+" AND palm_almacen='"+palm+"'"))
			 Utils.MostrarAlerta(Response, "Ya se ha realizado este proceso para el mes "+mes+" en el año "+ano+" para el almacen "+ddlAlmacen.SelectedItem.Text+"!");
            else
			{
				///////////////////////////////////////////////////////////////////////////////////////////////
				int n;
				string vDocCod = ddlCodDoc.SelectedValue;
				string numDoc = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo = '"+vDocCod+"'");
				string prefDocRef = ddlCodDoc.SelectedItem.Value;
				int numDocRef=0;
    	    	string mnit_cemp = DBFunctions.SingleData("SELECT mnit_nit from cempresa");
		        string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
    	    	string mes_cinv = DBFunctions.SingleData("SELECT pmes_mes from cinventario");
				string cencost = ddlCentro.SelectedItem.Value;
				string FechaDoc = calDate.SelectedDate.ToString("yyyy-MM-dd");
				string fechSis = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss.ffffff");
				string vend = ddlVendedor.SelectedItem.Value;
				double paag = Convert.ToDouble(lbPorcentaje.Text.Trim());
				double cantidad=0;
				DataSet ds = new DataSet();
        		DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mite_codigo, msal_cantactual, msal_costprom FROM msaldoitemalmacen WHERE pano_ano="+ano_cinv+" AND palm_almacen='"+palm+"';"+
        		//													0					1				2
    	    	                   						"SELECT mite_codigo, SUM(dite_cantidad) FROM ditems WHERE pano_ano="+ano_cinv+" AND pmes_mes="+mes_cinv+" AND tmov_tipomovi=30 GROUP BY mite_codigo");
	        	//													0				1
				ArrayList sqlL=new ArrayList();
				for(n=0;n<ds.Tables[0].Rows.Count;n++)
				{
					DataRow[] selection = ds.Tables[1].Select("mite_codigo='"+ds.Tables[0].Rows[n][0].ToString()+"'");
					if(selection.Length == 0)
						cantidad = Convert.ToDouble(ds.Tables[0].Rows[n][1]);
					else
						cantidad = Convert.ToDouble(ds.Tables[0].Rows[n][1]) - Convert.ToDouble(selection[0][1]);
					//Por el momento en el campo de cantidad, se va a mover la cantidad la cual se ha ajustado la variable double
					sqlL.Add("insert into DITEMS (DITE_SECUENCIA,PDOC_CODIGO,DITE_NUMEDOCU,MITE_CODIGO,DITE_PREFDOCUREFE,DITE_NUMEDOCUREFE,TMOV_TIPOMOVI,MNIT_NIT,PALM_ALMACEN,PANO_ANO,PMES_MES,DITE_FECHDOCU,DITE_CANTIDAD,DITE_VALOUNIT,DITE_COSTPROM,DITE_COSTPROMALMA,PIVA_PORCIVA,DITE_PORCDESC,PVEN_CODIGO,DITE_CANTDEVO,TCAR_CARGO,DITE_COSTPROMHIS,DITE_COSTPROMHISALMA,DITE_VALOPUBL,PCEN_CODIGO,DITE_PROCESO,DITE_INVEINIC,DITE_INVEINICALMA)"+
					         			" values (DEFAULT,'"+ vDocCod+"',"+numDoc+",'"+ds.Tables[0].Rows[n][0].ToString()+"','"+prefDocRef+"', "+numDocRef.ToString()+","+TMovKar.ToString()+",'"+mnit_cemp+"','"+palm+"',"+ano_cinv+","+mes_cinv+",'"+FechaDoc+"',"+cantidad.ToString()+","+(Convert.ToDouble(ds.Tables[0].Rows[n][2])*(paag/100)).ToString()+","+ds.Tables[0].Rows[n][2].ToString()+","+ds.Tables[0].Rows[n][2].ToString()+",0,0,'"+vend+"',0,'"+DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vend+"'")+"',"+ds.Tables[0].Rows[n][2].ToString()+","+ds.Tables[0].Rows[n][2].ToString()+",0,'"+cencost+"','"+fechSis+"',0,0)");
					sqlL.Add("update msaldoitem SET msal_costprom=(msal_costprom*"+paag.ToString()+"/100)+msal_costprom where mite_codigo='"+ds.Tables[0].Rows[n][0].ToString()+"'");
					sqlL.Add("update msaldoitemalmacen SET msal_costprom=(msal_costprom*"+paag.ToString()+"/100)+msal_costprom where mite_codigo='"+ds.Tables[0].Rows[n][0].ToString()+"'");
					if(!DBFunctions.RecordExist("SELECT * FROM MACUMULADOITEM WHERE pano_ano="+calDate.SelectedDate.Year.ToString()+" and pmes_mes="+calDate.SelectedDate.Month.ToString()+" and tmov_tipomovi="+TMovKar.ToString()+" AND mite_codigo = '"+ds.Tables[0].Rows[n][0].ToString()+"'"))
	            		sqlL.Add("insert into MACUMULADOITEM (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,macu_cantidad,macu_costo,macu_precio) values ('"+ds.Tables[0].Rows[n][0].ToString()+"',"+TMovKar.ToString()+","+calDate.SelectedDate.Year.ToString()+","+calDate.SelectedDate.Month.ToString()+",0,0,0)");
					if(!DBFunctions.RecordExist("SELECT * FROM MACUMULADOITEMALMACEN WHERE pano_ano="+calDate.SelectedDate.Year.ToString()+" and pmes_mes="+calDate.SelectedDate.Month.ToString()+" and tmov_tipomovi="+TMovKar.ToString()+" AND mite_codigo = '"+ds.Tables[0].Rows[n][0].ToString()+"'"))
            			sqlL.Add("insert into MACUMULADOITEMALMACEN (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,PALM_ALMACEN,macu_cantidad,macu_costo,macu_precio) values ('"+ds.Tables[0].Rows[n][0].ToString()+"',"+TMovKar.ToString()+","+calDate.SelectedDate.Year.ToString()+","+calDate.SelectedDate.Month.ToString()+",'"+palm+"',0,0,0)");
					sqlL.Add("update macumuladoitem set macu_costo=macu_costo+"+(cantidad*(Convert.ToDouble(ds.Tables[0].Rows[n][2])*(paag/100))).ToString()+", macu_cantidad="+cantidad.ToString()+" WHERE pano_ano="+calDate.SelectedDate.Year.ToString()+" and pmes_mes="+calDate.SelectedDate.Month.ToString()+" and tmov_tipomovi="+TMovKar.ToString()+" AND mite_codigo = '"+ds.Tables[0].Rows[n][0].ToString()+"'");
					sqlL.Add("update macumuladoitemalmacen set macu_costo=macu_costo+"+(cantidad*(Convert.ToDouble(ds.Tables[0].Rows[n][2])*(paag/100))).ToString()+", macu_cantidad="+cantidad.ToString()+" WHERE pano_ano="+calDate.SelectedDate.Year.ToString()+" and pmes_mes="+calDate.SelectedDate.Month.ToString()+" and tmov_tipomovi="+TMovKar.ToString()+" AND mite_codigo = '"+ds.Tables[0].Rows[n][0].ToString()+"' AND palm_almacen='"+palm+"'");
				}
				sqlL.Add("update pdocumento set pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo = '"+vDocCod+"'");
				if(DBFunctions.Transaction(sqlL))
				{
					string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
					Response.Redirect(""+indexPage+"?process=Inventarios.AjustesInf&path=Ajustes por Inflacin");
				}
				else
					lbInfo.Text += "<br> Error :"+DBFunctions.exceptions;
				/////////////////////////////////////////////////////////////////////////////////////////////////////////////
			}
			
		}
		
		protected void ChangeDate(object Sender, EventArgs E)
		{
			tbDate.Text = calDate.SelectedDate.GetDateTimeFormats()[6];
		}
		protected void ChangeDocument(object Sender, EventArgs E)
		{
			DatasToControls bind2 = new DatasToControls();
			lbNumDoc.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo = '" + ddlCodDoc.SelectedItem.Text + "'");
		}
		
		
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
